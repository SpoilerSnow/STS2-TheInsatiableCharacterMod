using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;

public abstract class InsatiableRelicModel : ModRelicTemplate, ITheInsatiableModel
{
    public override RelicAssetProfile AssetProfile => new(
        // 小图标（原版85x85）
        IconPath: $"res://TheInsatiable/images/relics/{GetType().Name.Replace("Relic", "")}.png",
        // 轮廓图标（原版85x85）
        IconOutlinePath: $"res://TheInsatiable/images/relics/outline/{GetType().Name.Replace("Relic", "")}.png",
        // 大图标（原版256x256）
        BigIconPath: $"res://TheInsatiable/images/relics/big/{GetType().Name.Replace("Relic", "")}.png"
    );
    public virtual Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
        return Task.CompletedTask;
    }
    public virtual Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force = false)
    {
       return Task.CompletedTask;
    }
}