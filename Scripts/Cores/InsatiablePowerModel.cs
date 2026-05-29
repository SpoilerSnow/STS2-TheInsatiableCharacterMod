using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public abstract class InsatiablePowerModel : CustomPowerModel, ITheInsatiableModel
{
    public override string CustomPackedIconPath => $"res://TheInsatiable/images/powers/{GetType().Name.Replace("Power", "")}.png";
	public override string CustomBigIconPath => $"res://TheInsatiable/images/powers/big/{GetType().Name.Replace("Power", "")}.png";
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
       return Task.CompletedTask;
    }
}