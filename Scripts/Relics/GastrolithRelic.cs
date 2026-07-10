using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts;

[RegisterRelic(typeof(InsatiableRelicPool))]
public class GastrolithRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Dynamic)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("MaxTurnsInPile", 1)];
	public override Task BeforeCombatStart()
	{
		SwallowPile.MaxTurnsInPile -= (int)DynamicVars["MaxTurnsInPile"].BaseValue;
        return Task.CompletedTask;
	}
}
