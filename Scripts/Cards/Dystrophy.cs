using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Piles;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Localization;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Dystrophy : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        base.EnergyHoverTip
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new EnergyVar(1),
        new MaxCapacityVar(4),
    ];
    public Dystrophy()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<GastricCapacityPower>(choiceContext, base.Owner.Creature, -base.DynamicVars["MaxCapacity"].IntValue, base.Owner.Creature, this);
        if (Entry.SwallowPile.GetPile(base.Owner).Cards.Count >= SwallowPile.MaxCapacity)
        {
            ThinkCmd.Play(new LocString("combat_messages", "SWALLOW_PILE_FULL_2"), base.Owner.Creature, 2.0);
        }
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }
    protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars["MaxCapacity"].UpgradeValueBy(-1);
	}
}