using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

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
	public override async Task OnDigest()
	{
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), base.DynamicVars.Cards.BaseValue, base.Owner);
		await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
	}
    protected override void OnUpgrade()
	{
        AddKeyword(TheInsatiableKeyword.SelfSwallow);
	}
}