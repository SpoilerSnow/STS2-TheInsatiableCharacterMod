using System.Reflection; // 引入反射命名空间
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History; // 引入 CombatHistoryEntry 所在命名空间
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInsatiable.Scripts;

public class MillipedeNeverDiePower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    private Dictionary<int, CardModel> _lastPlayedCardsPerTurn = new Dictionary<int, CardModel>();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await base.AfterCardPlayed(choiceContext, cardPlay);
        if (cardPlay.Card.Owner != base.Owner.Player) return;
        if (cardPlay.Card.IsDupe) return;
        if (cardPlay.Card is MillipedeNeverDie) return;
        
        // 记录当前及未来回合的牌，这里可以直接获取当前的 TurnNumber
        int currentTurn = base.Owner.Player.PlayerCombatState.TurnNumber;
        _lastPlayedCardsPerTurn[currentTurn] = cardPlay.Card;
    }

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        // 获得 Power 时，初始化历史记录
        InitializeHistory();
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner)) return;
        
        int currentTurn = base.Owner.Player.PlayerCombatState.TurnNumber;
        int layers = base.Amount;
        
        for (int i = 1; i <= layers; i++)
        {
            int targetTurn = currentTurn - i;
            if (targetTurn < 1) continue;
            
            if (_lastPlayedCardsPerTurn.TryGetValue(targetTurn, out CardModel cardModel))
            {
                await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(), null);
            }
        }
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        _lastPlayedCardsPerTurn.Clear();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 从战斗历史记录中读取并初始化之前回合最后打出的牌
    /// </summary>
    private void InitializeHistory()
    {
        _lastPlayedCardsPerTurn.Clear();
        int currentTurn = base.Owner.Player.PlayerCombatState.TurnNumber;
        
        var entries = CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>()
            .Where(e => e.CardPlay.Card.Owner.Creature == base.Owner)
            .Where(e => !e.CardPlay.Card.IsDupe)
            .Where(e => e.CardPlay.Card is not MillipedeNeverDie);

        foreach (var entry in entries)
        {
            // 使用反射获取该历史记录发生时的玩家回合数
            int turn = GetPlayerTurnNumberAtEntry(entry);
            
            // 只记录当前回合之前的牌
            if (turn > 0 && turn < currentTurn)
            {
                _lastPlayedCardsPerTurn[turn] = entry.CardPlay.Card;
            }
        }
    }

    /// <summary>
    /// 通过反射获取 CombatHistoryEntry 中记录的指定玩家的回合数
    /// </summary>
    private int GetPlayerTurnNumberAtEntry(CombatHistoryEntry entry)
    {
        // 获取 CombatHistoryEntry 类中的私有字段 _playerTurnNumbers
        var field = typeof(CombatHistoryEntry).GetField("_playerTurnNumbers", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            // 将其转换为 Dictionary<ulong, int> (Key为玩家NetId，Value为TurnNumber)
            var dict = field.GetValue(entry) as Dictionary<ulong, int>;
            if (dict != null && dict.TryGetValue(base.Owner.Player.NetId, out int turn))
            {
                return turn;
            }
        }
        return -1; // 如果反射失败则返回 -1
    }
}