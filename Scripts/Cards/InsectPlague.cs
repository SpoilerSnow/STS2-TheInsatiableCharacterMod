using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class InsectPlague : InsatiableCardModel
{
	private const int energyCost = 1;
	private const CardType type = CardType.Attack;
	private const CardRarity rarity = CardRarity.Uncommon;
	private const TargetType targetType = TargetType.AnyEnemy;
	private const bool shouldShowInCardLibrary = true;
    protected override bool ShouldGlowGoldInternal => CombatManager.Instance.History.Entries
        .OfType<DamageReceivedEntry>()
        .Any(e => e.Dealer == base.Owner.Creature
            && e.Result.Props.IsPoweredAttack()
            && e.HappenedThisTurn(base.CombatState));
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10, ValueProp.Move),
        new PowerVar<VulnerablePower>(1)
    ];
	public InsectPlague() 
		: base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, nameof(cardPlay.Target));
		bool hasAttackedTarget = CombatManager.Instance.History.Entries
			.OfType<DamageReceivedEntry>()
			.Any(e => e.Receiver == cardPlay.Target
				&& e.Dealer == base.Owner.Creature
				&& e.Result.Props.IsPoweredAttack()
				&& e.HappenedThisTurn(base.CombatState));
		if (hasAttackedTarget)
		{
			await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars.Vulnerable.BaseValue, base.Owner.Creature, this);
		}
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this)
			.Targeting(cardPlay.Target)
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4);
        base.DynamicVars.Vulnerable.UpgradeValueBy(1);
	}
}