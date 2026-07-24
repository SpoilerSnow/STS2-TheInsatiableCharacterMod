using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.Cards;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class LikeMothToFlamePower : TemporaryStrengthPower
{
	public override AbstractModel OriginModel => ModelDb.Card<LikeMothToFlame>();
    protected override bool IsPositive => true;
}