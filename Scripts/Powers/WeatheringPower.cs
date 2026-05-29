using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;
public class WeatheringPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new StringVar("AfflictionTitle", ModelDb.Affliction<WeatheringAffliction>().Title.GetFormattedText())];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromAffliction<WeatheringAffliction>(2);
    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        foreach (CardModel allCard in base.Owner.Player.PlayerCombatState.AllCards)
        {
            if (allCard.Owner == base.Owner.Player && !(allCard.Affliction is WeatheringAffliction))
            {
                Flash();
                CardCmd.ClearAffliction(allCard);
            }
        }
    }
	public bool ShouldAfflict(ICombatState combatState, CardModel card, AfflictionModel affliction)
    {
        if (card.Affliction is WeatheringAffliction)
        {
            return true;
        }
        foreach (CardModel allCard in base.Owner.Player.PlayerCombatState.AllCards)
        {
            if (allCard.Owner == base.Owner.Player && !(allCard.Affliction is WeatheringAffliction))
            {
                Flash();
                return false;
            }
        }
        return true;
    }
	public override decimal ModifyHandDraw(Player player, decimal count)
	{
		if (player != base.Owner.Player)
		{
			return count;
		}
		return count + (decimal)base.Amount;
	}
    public int _weatheringCount = 2;
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner))
		{
            CardPile pile = PileType.Hand.GetPile(base.Owner.Player);
		    CardModel cardModel = base.Owner.Player.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
		    if (cardModel != null)
		    {
                if (cardModel.Affliction is WeatheringAffliction existingAffliction)
                {
                    Flash();
                    existingAffliction.Amount += _weatheringCount;
                }
                else
                {
                    Flash();
                    await CardCmd.Afflict<WeatheringAffliction>(cardModel, _weatheringCount);
                }
		    }
        }
    }
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Card.Affliction is WeatheringAffliction)
		{
            int weatheringCount = cardPlay.Card.Affliction.Amount;
            decimal quickSandCount = weatheringCount;
			await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.Owner, quickSandCount, base.Owner, null);
        }
	}
}