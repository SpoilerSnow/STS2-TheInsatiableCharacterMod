using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public sealed class Reinforce : InsatiableCardModel
{
	public override bool GainsBlock => true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromCard<SandStone>()
    ];
	public Reinforce()
		: base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.GainBlock(base.Owner.Creature, base.Owner.Creature.Block, ValueProp.Unpowered | ValueProp.Move, cardPlay);
        foreach (var creature in base.CombatState.Creatures)
        {
            int creatureCurrent = creature.GetPowerAmount<QuickSandPower>();
            if (creatureCurrent > 0)
            {
                await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), creature, creatureCurrent, base.Owner.Creature, this);
            }
        }
        var sandstoneCards = PileType.Hand.GetPile(base.Owner).Cards.Where(card => card is SandStone).ToList();
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
