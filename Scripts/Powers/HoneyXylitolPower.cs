using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheInsatiable.Scripts;

namespace TheInsatiable.Scripts;
public class HoneyXylitolPower : TemporaryStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<HoneyXylitol>();
    protected override bool IsPositive => true;
}