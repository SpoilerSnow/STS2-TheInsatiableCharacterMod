using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

public class TheInsatiableHook
{
    public static async Task BeforeCardSwallow(ICombatState combatState, CardModel card, bool causedBySelfSwallow)
	{
		foreach (AbstractModel model in combatState.IterateHookListeners())
		{
            if (model is ITheInsatiableModel theInsatiableModel)
			{
			    await theInsatiableModel.BeforeCardSwallow(card, causedBySelfSwallow);
			    model.InvokeExecutionFinished();
            }
		}
	}
    public static async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        foreach (AbstractModel model in combatState.IterateHookListeners())
		{
            if (model is ITheInsatiableModel theInsatiableModel)
			{
                choiceContext.PushModel(model);
			    await theInsatiableModel.AfterCardSwallow(combatState, choiceContext, card, causedBySelfSwallow);
			    model.InvokeExecutionFinished();
                choiceContext.PopModel(model);
            }
		}
    }
    public static async Task BeforeCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
        foreach (AbstractModel model in combatState.IterateHookListeners())
        {
            if (model is ITheInsatiableModel theInsatiableModel)
            {
                await theInsatiableModel.BeforeCreatureSwallow(creature, force);
                model.InvokeExecutionFinished();
            }
        }
    }
    public static async Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
        foreach (AbstractModel model in combatState.IterateHookListeners())
		{
            if (model is ITheInsatiableModel theInsatiableModel)
			{
			    await theInsatiableModel.AfterCreatureSwallow(combatState, creature, force);
			    model.InvokeExecutionFinished();
            }
		}
    }
	public static bool ShouldSelfSwallowTrigger(ICombatState combatState, CardModel card)
    {
        foreach (AbstractModel item in combatState.IterateHookListeners())
        {
            if (item is ITheInsatiableModel theInsatiableModel)
            {
                if (!theInsatiableModel.ShouldSelfSwallowTrigger(card))
                {
                    return false;
                }
            }
        }
        return true;
    }
}