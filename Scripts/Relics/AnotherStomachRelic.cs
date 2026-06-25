using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInsatiable.Scripts;

[RegisterRelic(typeof(InsatiableRelicPool))]

public class AnotherStomachRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
    public override async Task AfterCreatureSwallow(ICombatState combatState, Creature creature, bool force)
    {
        Flash();
        await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.IntValue);
    }
}