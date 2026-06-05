using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableRelicPool))]

public class ReverseHourglassRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Common;

	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];

	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<QuickSandPower>(1)];

	public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
	{
		if (!(power is QuickSandPower))
		{
			return 0m;
		}
		if (giver != base.Owner.Creature)
		{
			return 0m;
		}
		return base.DynamicVars["QuicksandPower"].BaseValue;
	}

	public override Task AfterModifyingPowerAmountGiven(PowerModel power)
	{
		Flash();
		return Task.CompletedTask;
	}
}