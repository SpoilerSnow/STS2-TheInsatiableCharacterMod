using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public class GluttonyPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool _)
	{
		if (card.Owner.Creature == base.Owner)
		{
            await TheInsatiableHook.BeforeCardSwallow(CombatState, card, false);
			await TheInsatiableHook.AfterCardSwallow(CombatState, choiceContext, card, false);
		}
	}
	public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (creature == base.CombatState.Enemies)
		{
			await TheInsatiableHook.BeforeCreatureSwallow(CombatState, creature, false);
			await TheInsatiableHook.AfterCreatureSwallow(CombatState, creature, false);
		}
	}
}