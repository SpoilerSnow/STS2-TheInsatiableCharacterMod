using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class QuicksandStrike : InsatiableCardModel
{
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromCard<SandStone>()
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(9, ValueProp.Move),
        new PowerVar<QuickSandPower>(5)
    ];
	public QuicksandStrike() 
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
		await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<SandStone>(base.Owner), PileType.Hand, base.Owner);
		await Cmd.Wait(0.5f);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2);
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(3);
	}
}
