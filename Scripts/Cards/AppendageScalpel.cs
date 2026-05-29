using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class AppendageScalpel : InsatiableCardModel
{
	protected override bool HasEnergyCostX => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new CalculationBaseVar(9),
		new ExtraDamageVar(2),
		new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? target)
		{
			if (target == null)
			{
				return 0;
			}
			int debuffCount = target.Powers
				.Where(p => p.Type == PowerType.Debuff && !(p is ITemporaryPower))
				.Sum(p => p.Amount);
			int num = debuffCount / 3;
			return num;
		})
    ];
	public AppendageScalpel()
		: base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.CalculatedDamage)
            .WithHitCount(ResolveEnergyXValue())
            .FromCard(this)
			.Targeting(cardPlay.Target)
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.CalculationBase.UpgradeValueBy(1);
        base.DynamicVars.ExtraDamage.UpgradeValueBy(1);
	}
}