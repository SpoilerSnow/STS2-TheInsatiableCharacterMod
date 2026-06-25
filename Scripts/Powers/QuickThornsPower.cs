using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts;

[RegisterPower]
public class QuickThornsPower : TemporaryThornsPower
{
	public override AbstractModel OriginModel => ModelDb.Card<QuickThorns>();
	protected override bool IsPositive => true;
}