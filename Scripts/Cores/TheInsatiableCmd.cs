using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts;
using TheInsatiable.Scripts.Piles;

public class TheInsatiableCmd
{
    public static async Task<bool> SwallowCard(PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow = false, bool skipVisuals = false)
    {
        if (!CombatManager.Instance.IsOverOrEnding)
		{
			ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
            var swallowPile = Entry.SwallowPile.GetPile(card.Owner);
            if (swallowPile.Cards.Count >= SwallowPile.MaxCapacity)
            {
                ThinkCmd.Play(new LocString("combat_messages", "SWALLOW_PILE_FULL"), card.Owner.Creature, 2.0);
                return false;
            }
            await TheInsatiableHook.BeforeCardSwallow(combatState, card, causedBySelfSwallow);
            await CardPileCmd.Add(card, swallowPile);
            SwallowPile.OnCardAdded(card);
            CombatManager.Instance.History.CardSwallowed(combatState, card);
            await TheInsatiableHook.AfterCardSwallow(combatState, choiceContext, card, causedBySelfSwallow);
            return true;
        }
        return false;
    }
    public static async Task SwallowCreature(Creature creature, bool force = false)
    {
        if (!CombatManager.Instance.IsOverOrEnding)
		{
            ICombatState combatState = creature.CombatState ?? creature.CombatState;
            await TheInsatiableHook.BeforeCreatureSwallow(combatState, creature, force);
            await CreatureCmd.Kill(creature);
            CombatManager.Instance.History.CreatureSwallowed(combatState, creature);
            await TheInsatiableHook.AfterCreatureSwallow(combatState, creature, force);
        }
    }
}