using MegaCrit.Sts2.Core.Combat;
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
        if (cardPlay.Card.Owner != base.Owner.Player)
        {
            return;
        }
        if (cardPlay.Card.IsDupe)
        {
            return;
        }
        if (cardPlay.Card is MillipedeNeverDie)
        {
            return;
        }
        int currentTurn = base.Owner.Player.PlayerCombatState.TurnNumber;
        _lastPlayedCardsPerTurn[currentTurn] = cardPlay.Card;
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner))
		{
			return;
		}
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
}