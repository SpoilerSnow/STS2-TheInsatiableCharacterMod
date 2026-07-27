using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class InsectPlague : InsatiableCardModel
{
    protected override bool ShouldGlowGoldInternal => CombatManager.Instance.History.Entries
        .OfType<DamageReceivedEntry>()
        .Any(e => e.Dealer == base.Owner.Creature
		    && e.Receiver != null
            && e.Result.Props.IsPoweredAttack()
            && e.HappenedThisTurn(base.CombatState));
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(10, ValueProp.Move),
        new PowerVar<VulnerablePower>(1)
    ];
	public InsectPlague() 
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
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
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3);
        base.DynamicVars.Vulnerable.UpgradeValueBy(1);
	}
}
