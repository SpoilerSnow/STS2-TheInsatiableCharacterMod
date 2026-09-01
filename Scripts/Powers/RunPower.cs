using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class RunPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.ForEnergy(this)];
	public override decimal ModifyHandDraw(Player player, decimal count)
	{
		if (player != base.Owner.Player)
		{
			return count;
		}
		return count + (decimal)base.Amount;
	}
	public override decimal ModifyMaxEnergy(Player player, decimal amount)
	{
		if (player != base.Owner.Player)
		{
			return amount;
		}
		return amount + (decimal)base.Amount;
	}
	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        IEnumerable<CardModel> statusCards = CardFactory.GetDistinctForCombat(base.Owner.Player, ModelDb.CardPool<StatusCardPool>().GetUnlockedCards(base.Owner.Player.UnlockState, base.Owner.Player.RunState.CardMultiplayerConstraint), base.Amount, base.Owner.Player.RunState.Rng.CombatCardGeneration).Concat([base.CombatState.CreateCard<FranticEscape>(base.Owner.Player)]);
        foreach (CardModel statusCard in statusCards)
        {
            Flash();
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(statusCard, PileType.Draw, base.Owner.Player, CardPilePosition.Random));
        }
    }
}