using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts;

[RegisterPower]
public class GluttonyPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
	{
		if (card.Owner.Creature == base.Owner)
		{
			CardModel scard = card.CreateClone();
			await TheInsatiableCmd.SwallowCard(choiceContext, scard);
		}
	}
	public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (base.CombatState.Enemies.Contains(creature))
		{
			await TheInsatiableHook.BeforeCreatureSwallow(CombatState, creature, false);
			await TheInsatiableHook.AfterCreatureSwallow(CombatState, creature, false);
		}
	}
}