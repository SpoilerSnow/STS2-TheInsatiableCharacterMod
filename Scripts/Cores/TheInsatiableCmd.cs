using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts;

public class TheInsatiableCmd
{
    public static async Task SwallowCard(PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow = false, bool skipVisuals = false)
    {
        if (!CombatManager.Instance.IsOverOrEnding)
		{
			ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
            await CardCmd.Exhaust(choiceContext, card);
            CombatManager.Instance.History.CardSwallowed(combatState, card);
            await TheInsatiableHook.AfterCardSwallow(combatState, choiceContext, card, causedBySelfSwallow);
        }
    }
    public static async Task SwallowCreature(Creature creature, bool force = false)
    {
        if (!CombatManager.Instance.IsOverOrEnding)
		{
            ICombatState combatState = creature.CombatState ?? creature.CombatState;
            await CreatureCmd.Kill(creature);
            CombatManager.Instance.History.CreatureSwallowed(combatState, creature);
            await TheInsatiableHook.AfterCreatureSwallow(combatState, creature, force);
        }
    }
}