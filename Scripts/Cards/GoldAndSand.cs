using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]
public class GoldAndSand : InsatiableCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<QuickSandPower>(4),
		new EnergyVar(2)
	];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        base.EnergyHoverTip
    ];
	public GoldAndSand()
		: base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
		await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Energy.UpgradeValueBy(1);
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(-2);
	}
}