using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.HoverTips;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class AppendageSpecialization : InsatiableCardModel
{
    private static List<EnchantmentModel>? allEnchantments;

    private static List<EnchantmentModel> AllEnchantments
    {
        get
        {
            if (allEnchantments == null)
            {
                allEnchantments = new List<EnchantmentModel>();
                var enchantmentTypes = ModelDb.AllAbstractModelSubtypes
                    .Where(t => t != null && t.IsSubclassOf(typeof(EnchantmentModel)) && !t.IsAbstract);
                foreach (Type type in enchantmentTypes)
                {
                    try
                    {
                        ModelId id = ModelDb.GetId(type);
                        EnchantmentModel? enchantment = ModelDb.GetByIdOrNull<EnchantmentModel>(id);

                        if (enchantment != null)
                        {
                            allEnchantments.Add(enchantment);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return allEnchantments;
        }
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Piles)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Count", 2),
        new DynamicVar("Enchant", 2)
    ];
	public AppendageSpecialization()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        List<CardModel> list1 = PileType.Draw.GetPile(base.Owner).Cards.Where(card => card.Type == CardType.Attack).ToList();
        List<CardModel> list2 = PileType.Hand.GetPile(base.Owner).Cards.Where(card => card.Type == CardType.Attack).ToList();
        List<CardModel> list3 = PileType.Discard.GetPile(base.Owner).Cards.Where(card => card.Type == CardType.Attack).ToList();
        IEnumerable<CardModel> items = list1.Concat(list2).Concat(list3).ToList();
        for (int i = 0; i < base.DynamicVars["Count"].BaseValue; i++)
        {
            IEnumerable<CardModel> items2 = items.Where(card => !card.IsUpgraded).ToList();
            CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(items2);
            if (cardModel != null)
		    { 
			    CardCmd.Upgrade(cardModel);
			    CardCmd.Preview(cardModel);
		    }
        }
        for (int i = 0; i < base.DynamicVars["Count"].BaseValue; i++)
        {
            IEnumerable<CardModel> items2 = items.Where(card => card.Enchantment is null).ToList();
            CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(items2);
            if (cardModel != null)
		    {
                var availableEnchantments = AllEnchantments.Where(e =>
                {
                    if (e.IsMock) return false;
                    if (e is DeprecatedEnchantment) return false;
                    if (e is Goopy) return false;
                    if (e is Inky && !(cardModel.TargetType is TargetType.AnyEnemy or TargetType.AllEnemies)) return false;
                    return e.CanEnchant(cardModel);
                }).ToList();
                EnchantmentModel enchantmentModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(availableEnchantments);
                EnchantmentModel mutableCopy = enchantmentModel.ToMutable();
			    CardCmd.Enchant(mutableCopy, cardModel, base.DynamicVars["Enchant"].BaseValue);
			    CardCmd.Preview(cardModel);
		    }
        }
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars["Count"].UpgradeValueBy(1);
	}
}