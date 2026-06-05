using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiablePotionPool))]

public class BottledQuickSandPotion : InsatiablePotionModel
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuickSandPower>(9)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
		NCombatRoom.Instance?.PlaySplashVfx(target, new Color("f2e35c"));
		await PowerCmd.Apply<QuickSandPower>(choiceContext, target, base.DynamicVars["QuickSandPower"].BaseValue, base.Owner.Creature, null);
    }
}