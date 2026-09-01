using STS2RitsuLib.Interop.AutoRegistration;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInsatiable.Scripts.Potions;

[RegisterPotion(typeof(InsatiablePotionPool))]

public class DigestiveJuicePotion : InsatiablePotionModel
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Boss", 6),
        new DynamicVar("Elite", 4),
        new DynamicVar("Other", 2)
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<DigestiveJuicePower>()];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
		NCombatRoom.Instance?.PlaySplashVfx(target, new Color("94f882"));
        AbstractRoom? currentRoom = base.Owner.RunState.CurrentRoom;
		if (currentRoom != null)
		{
            if (currentRoom.RoomType == RoomType.Boss)
            {
                await PowerCmd.Apply<DigestiveJuicePower>(choiceContext, target, base.DynamicVars["Boss"].BaseValue, base.Owner.Creature, null);
            }
            else if (currentRoom.RoomType == RoomType.Elite)
            {
                await PowerCmd.Apply<DigestiveJuicePower>(choiceContext, target, base.DynamicVars["Elite"].BaseValue, base.Owner.Creature, null);
            }
            else
            {
                await PowerCmd.Apply<DigestiveJuicePower>(choiceContext, target, base.DynamicVars["Other"].BaseValue, base.Owner.Creature, null);
            }
        }
    }
}