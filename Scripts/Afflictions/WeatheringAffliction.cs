using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;
[RegisterAffliction]
public class WeatheringAffliction : ModAfflictionTemplate
{
	public override bool HasExtraCardText => true;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
}