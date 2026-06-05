using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

public abstract class InsatiableRelicModel : CustomRelicModel, ITheInsatiableModel
{
    // 小图标（原版85x85）
    public override string? PackedIconPath => $"res://TheInsatiable/images/relics/{GetType().Name.Replace("Relic", "")}.png";
    // 轮廓图标（原版85x85）
    protected override string? PackedIconOutlinePath => $"res://TheInsatiable/images/relics/{GetType().Name.Replace("Relic", "")}.png";
    // 大图标（原版256x256）
    protected override string? BigIconPath => $"res://TheInsatiable/images/relics/big/{GetType().Name.Replace("Relic", "")}.png";
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
       return Task.CompletedTask;
    }
}