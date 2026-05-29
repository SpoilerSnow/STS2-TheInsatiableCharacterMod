using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

public interface ITheInsatiableModel
{
    public virtual Task BeforeCardSwallow(CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task BeforeCreatureSwallow(Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
    {
        return Task.CompletedTask;
    }
    public virtual bool ShouldSelfSwallowTrigger(CardModel card)
    {
        return true;
	}
}