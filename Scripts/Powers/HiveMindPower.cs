using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class HiveMindPower : InsatiablePowerModel
{
    private class Data
	{
		public readonly HashSet<CardModel> autoPlayingCards = new HashSet<CardModel>();
		public int infiniteAutoPlaysThisTurn;
		public bool showedCapReachedMessage;
	}
	private const int _infiniteAutoPlayCap = 9;
    protected override object InitInternalData()
	{
		return new Data();
	}
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay == null || cardPlay.Card == null || cardPlay.Card.Owner != base.Owner.Player)
        {
            return;
        }
        Data data = GetInternalData<Data>();
        // 防死循环：如果这张牌正在被自动打出，直接跳过（效仿 Hellraiser 的 autoPlayingCards 守卫）
        if (data.autoPlayingCards.Contains(cardPlay.Card))
        {
            return;
        }
        bool flag = true;
        if (base.Owner.CombatState.HittableEnemies.All((Creature c) => c.HpDisplay.IsInfinite()))
        {
            if (data.infiniteAutoPlaysThisTurn >= _infiniteAutoPlayCap)
            {
                flag = false;
                if (!data.showedCapReachedMessage)
                {
                    ThinkCmd.Play(new LocString("powers", "THE_INSATIABLE_POWER_HIVE_MIND_POWER.infiniteAutoPlayCapReached"), base.Owner);
                    data.showedCapReachedMessage = true;
                }
            }
            data.infiniteAutoPlaysThisTurn++;
        }
        else
        {
            ResetInfiniteAutoPlayData();
        }
        if (flag)
        {
            CardPile drawPile = PileType.Draw.GetPile(base.Owner.Player);
            if (drawPile == null)
            {
                return;
            }
            string playedTitle = cardPlay.Card.Title.TrimEnd('+');
            List<CardModel> matchingCards = drawPile.Cards
                .Where(card => card.Title.TrimEnd('+') == playedTitle)
                .ToList();
            if (!matchingCards.Any())
            {
                return;
            }
            Flash();
            // 将即将自动打出的牌加入守卫集合，防止递归触发
            foreach (CardModel matchingCard in matchingCards)
            {
                data.autoPlayingCards.Add(matchingCard);
                matchingCard.RemoveFromCurrentPile(silent: true);
                await CardPileCmd.Add(matchingCard, PileType.Draw, CardPilePosition.Top, this, true);
            }
            SfxCmd.Play("event:/sfx/enemy/enemy_attacks/entomancer/entomancer_cast");
            await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner.Player, matchingCards.Count, CardPilePosition.Top, false);
            // 清除守卫标记
            foreach (CardModel matchingCard in matchingCards)
            {
                data.autoPlayingCards.Remove(matchingCard);
            }
        }
    }
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner))
		{
			return Task.CompletedTask;
		}
		ResetInfiniteAutoPlayData();
		return Task.CompletedTask;
	}
    private void ResetInfiniteAutoPlayData()
	{
		Data internalData = GetInternalData<Data>();
		internalData.infiniteAutoPlaysThisTurn = 0;
		internalData.showedCapReachedMessage = false;
	}
}