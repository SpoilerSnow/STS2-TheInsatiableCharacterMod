using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiablePotionPool))]

public class DigestiveJuicePotion : InsatiablePotionModel
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
		NCombatRoom.Instance?.PlaySplashVfx(target, new Color("94f882"));
        AbstractRoom? currentRoom = base.Owner.RunState.CurrentRoom;
		if (currentRoom == null && currentRoom.RoomType != RoomType.Elite && currentRoom.RoomType != RoomType.Boss)
		{
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "EatPlayer", 0.5f);
			await Cmd.Wait(2f);
            await TheInsatiableCmd.SwallowCreature(target);
        }
    }
}