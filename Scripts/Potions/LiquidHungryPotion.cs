using STS2RitsuLib.Interop.AutoRegistration;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Cards;

namespace TheInsatiable.Scripts.Potions;

[RegisterPotion(typeof(InsatiablePotionPool))]

public class LiquidHungryPotion : InsatiablePotionModel
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<InsatiableSwallow>()];
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        AssertValidForTargetedPotion(target);
		NCombatRoom.Instance?.PlaySplashVfx(target, new Color("a296a3"));
        int num = CardPile.MaxCardsInHand - base.Owner.PlayerCombatState.Hand.Cards.Count;
        for (int i = 0; i < num; i++)
        {
            await CardPileCmd.AddGeneratedCardToCombat(base.Owner.Creature.CombatState.CreateCard<InsatiableSwallow>(base.Owner), PileType.Hand, base.Owner);
        }
    }
}