using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Relics;

[RegisterRelic(typeof(InsatiableRelicPool))]

public class ReverseHourglassRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Common;

	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];

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
		if (target == base.Owner.Creature)
		{
			return 0m;
		}
		return base.DynamicVars["QuickSandPower"].BaseValue;
	}

	public override Task AfterModifyingPowerAmountGiven(PowerModel power)
	{
		Flash();
		return Task.CompletedTask;
	}
}