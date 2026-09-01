using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheInsatiable.Scripts.CardKeywords;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class SpinningSilk : InsatiableCardModel
{
	private bool _powerTarget;
	public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.Insect];
	public SpinningSilk() 
		: base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromPower<WeakPower>(),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Gulp)
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(4, ValueProp.Move),
        new RepeatVar(2),
        new PowerVar<WeakPower>(1),
		new DynamicVar("WeakGulp", 1),
    ];
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .WithHitCount(base.DynamicVars.Repeat.IntValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_spit")
			.WithHitFx("vfx/vfx_attack_blunt")
			.Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars.Weak.IntValue, base.Owner.Creature, this);
	}
	public override async Task OnGulp()
	{
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		foreach (Creature hittableEnemy in base.CombatState.HittableEnemies)
		{
			await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), hittableEnemy, base.DynamicVars["WeakGulp"].IntValue, base.Owner.Creature, this);
		}
	}
	public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
	{
		if (power.Type != PowerType.Buff)
		{
			return Task.CompletedTask;
		}
		if (amount <= 0m)
		{
			return Task.CompletedTask;
		}
		if (target != base.Owner.Creature)
		{
			return Task.CompletedTask;
		}
		_powerTarget = true;
		return Task.CompletedTask;
	}
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
	{
		if (_powerTarget != true)
		{
			return;
		}
		if (power.Type == PowerType.Buff && amount > 0m)
		{
			CardPile? pile = base.Pile;
		    if (pile != null && pile.Type != PileType.Discard && this.Owner == base.Owner)
		    {
			    await CardCmd.AutoPlay(choiceContext, this, null);
		    }
		}
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(1);
		base.DynamicVars.Weak.UpgradeValueBy(1);
	}
}
