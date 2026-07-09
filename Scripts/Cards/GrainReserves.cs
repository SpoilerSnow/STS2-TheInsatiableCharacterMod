using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[RegisterCard(typeof(InsatiableCardPool))]

public class GrainReserves : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest),
        base.EnergyHoverTip
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new EnergyVar(1)
    ];
    public GrainReserves()
		: base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	public override async Task Digest()
	{
		await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), base.DynamicVars.Cards.BaseValue, base.Owner);
		await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
	}
    protected override void OnUpgrade()
	{
        AddKeyword(TheInsatiableKeyword.SelfSwallow);
	}
}