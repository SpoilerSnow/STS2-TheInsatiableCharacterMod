using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(StatusCardPool))]
public class SandStone : InsatiableCardModel
{
	private const int energyCost = 1;
	private const CardType type = CardType.Status;
	private const CardRarity rarity = CardRarity.Status;
	private const TargetType targetType = TargetType.Self;
	private const bool shouldShowInCardLibrary = true;
	public SandStone() 
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
    }
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuickSandPower>(4)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        foreach (var creature in base.CombatState.Creatures)
        {
            await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), creature, DynamicVars["QuickSandPower"].BaseValue, base.Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["QuickSandPower"].UpgradeValueBy(2); 
    }
}
