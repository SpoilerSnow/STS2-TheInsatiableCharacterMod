using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public sealed class Reinforce : InsatiableCardModel
{
	public override bool GainsBlock => true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromCard<SandStone>()
    ];
	public Reinforce()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.GainBlock(base.Owner.Creature, base.Owner.Creature.Block, ValueProp.Unpowered | ValueProp.Move, cardPlay);
        int selfCurrent = base.Owner.Creature.GetPowerAmount<QuickSandPower>();
        if (selfCurrent > 0)
        {
            await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, selfCurrent, base.Owner.Creature, this, false);
        }
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            int enemyCurrent = enemy.GetPowerAmount<QuickSandPower>();
            if (enemyCurrent > 0)
            {
                await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), enemy, enemyCurrent, base.Owner.Creature, this);
            }
        }
        var sandstoneCards = PileType.Hand.GetPile(base.Owner).Cards
                                          .Where(card => card is SandStone)
                                          .ToList();
        foreach (CardModel sandstone in sandstoneCards)
        {
            CardModel card = sandstone.CreateClone();
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, base.Owner);
        }
	}
	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}
