using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;

public abstract class InsatiablePowerModel : ModPowerTemplate, ITheInsatiableModel
{
    public virtual bool HasCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/powers/{GetType().Name.Replace("Power", "")}.png");
    public virtual bool HasBigCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/powers/big/{GetType().Name.Replace("Power", "")}.png");
    public override string? CustomIconPath => HasCustomPortrait ? ($"res://TheInsatiable/images/powers/{GetType().Name.Replace("Power", "")}.png") : ($"res://TheInsatiable/images/powers/the_insatiable_power.png");
	public override string? CustomBigIconPath => HasCustomPortrait ? ($"res://TheInsatiable/images/powers/big/{GetType().Name.Replace("Power", "")}.png") : ($"res://TheInsatiable/images/powers/big/the_insatiable_power.png");
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
       return Task.CompletedTask;
    }
}