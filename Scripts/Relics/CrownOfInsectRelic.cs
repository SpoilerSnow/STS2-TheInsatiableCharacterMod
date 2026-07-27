using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Relics;

[RegisterRelic(typeof(InsatiableRelicPool))]

public class CrownOfInsectRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.ForEnergy(this)];
    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
	{
		modifiedCost = originalCost;
		if (card.Owner != base.Owner)
		{
			return false;
		}
		if (originalCost <= 0m)
		{
			return false;
		}
        if (!card.Keywords.Contains(TheInsatiableKeyword.Insect))
        {
           return false;
        }
		modifiedCost = originalCost - base.DynamicVars.Energy.BaseValue;
		if (modifiedCost < 0m)
		{
			modifiedCost = default(decimal);
		}
		return true;
	}
}