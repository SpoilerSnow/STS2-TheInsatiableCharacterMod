using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts;

namespace TheInsatiable.Scripts;
[RegisterPower]
public class HoneyXylitolPower : TemporaryStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<HoneyXylitol>();
    protected override bool IsPositive => true;
}