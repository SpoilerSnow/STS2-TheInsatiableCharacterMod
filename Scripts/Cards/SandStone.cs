using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace TheInsatiable.Scripts;

[Pool(typeof(StatusCardPool))]
public class SandStone : InsatiableCardModel
{
	private const int energyCost = 1;
	private const CardType type = CardType.Status;
	private const CardRarity rarity = CardRarity.Status;
	private const TargetType targetType = TargetType.AllEnemies;
	private const bool shouldShowInCardLibrary = true;
	public SandStone() 
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
    }
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [
        CardKeyword.Retain,
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuickSandPower>(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["QuickSandPower"].UpgradeValueBy(2); 
    }
}