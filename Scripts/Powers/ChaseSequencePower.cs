using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Combat.Ui.ExtraCornerAmountLabels;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class ChaseSequencePower : InsatiablePowerModel, IPowerExtraIconAmountLabelSpecsProvider
{
    private class Data
	{
		public int remainingTurns;
	}
    public int TopRightDisplayAmount => 6 - GetInternalData<Data>().remainingTurns % 6;
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<FranticEscape>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
    ];
    protected override object InitInternalData()
    {
        return new Data { remainingTurns = 0 };
    }
    public IReadOnlyList<ExtraIconAmountLabelSpec> GetPowerExtraIconAmountLabelSpecs()
    {
        return
        [
            ExtraIconAmountLabelSpec.Plain(
                ExtraIconAmountLabelCorner.TopRight,
                TopRightDisplayAmount.ToString())
        ];
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
		if (participants.Contains(base.Owner))
		{
        Data data = GetInternalData<Data>();
        data.remainingTurns++;
        InvokeDisplayAmountChanged();
        if (TopRightDisplayAmount != 6)
        {
            return;
        }
        IEnumerable<Player> enumerable = base.CombatState.Players.Where((Player p) => p.Creature.IsAlive && p != base.Owner.Player);
        foreach (Player player1 in enumerable)
		{
            List<CardModel> list1 = PileType.Draw.GetPile(player1).Cards.ToList();
            List<CardModel> list2 = PileType.Hand.GetPile(player1).Cards.ToList();
            List<CardModel> list3 = PileType.Discard.GetPile(player1).Cards.ToList();
            IEnumerable<CardModel> items = list1.Concat(list2).Concat(list3).ToList();
            foreach (CardModel card in items)
            {
                if (card is FranticEscape)
                {
                    Flash();
                    await CreatureCmd.TriggerAnim(base.Owner, "EatPlayer", 0.5f);
			        await Cmd.Wait(2f);
                    await TheInsatiableCmd.SwallowCreature(player1.Creature);
                }
            }
        }
		}
    }
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != base.Owner.Player)
        {
            return;
        }
        IEnumerable<Player> enumerable = base.CombatState.Players.Where((Player p) => p.Creature.IsAlive && p != base.Owner.Player);
        foreach (Player player1 in enumerable)
        {
            IEnumerable<CardModel> statusCards = CardFactory.GetDistinctForCombat(player1, ModelDb.CardPool<StatusCardPool>().GetUnlockedCards(base.Owner.Player.UnlockState, base.Owner.Player.RunState.CardMultiplayerConstraint), base.Amount, base.Owner.Player.RunState.Rng.CombatCardGeneration).Concat([base.CombatState.CreateCard<FranticEscape>(base.Owner.Player)]);
            foreach (CardModel statusCard in statusCards)
            {
                Flash();
                CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(statusCard, PileType.Draw, base.Owner.Player, CardPilePosition.Random));
            }
        }
    }
}