using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using TheInsatiable.Scripts;

namespace TheInsatiable.Scripts;

public class QuickThornsPower : TemporaryThornsPower
{
	public override AbstractModel OriginModel => ModelDb.Card<QuickThorns>();
	protected override bool IsPositive => true;
}