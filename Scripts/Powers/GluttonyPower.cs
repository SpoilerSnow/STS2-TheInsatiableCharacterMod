using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class GluttonyPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];
	public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        SwallowPile.LockMaxCapacity();
        return Task.CompletedTask;
    }
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
	{
		if (card.Owner.Creature == base.Owner)
		{
			Flash();
			CardModel scard = card.CreateClone();
			await TheInsatiableCmd.SwallowCard(choiceContext, scard);
		}
	}
	public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (creature != CombatManager.Instance.History.Entries.OfType<CreatureSwallowedEntry>())
		{
			Flash();
			await TheInsatiableHook.BeforeCreatureSwallow(CombatState, creature, false);
			CombatManager.Instance.History.CreatureSwallowed(CombatState, creature);
			await TheInsatiableHook.AfterCreatureSwallow(CombatState, creature, false);
		}
	}
}