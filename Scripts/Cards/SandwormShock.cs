using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]
public class SandwormShock : InsatiableCardModel
{
    protected override bool HasEnergyCostX => true;
	private const bool shouldShowInCardLibrary = true;
    public SandwormShock()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromPower<VulnerablePower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<QuickSandPower>(5),
        new PowerVar<VulnerablePower>(1)
        ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int powerAmount = ResolveEnergyXValue();
		if (base.IsUpgraded)
		{
			powerAmount++;
		}
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		for (int i = 0; i < powerAmount; i++)
		{
			await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
		}
	}
}