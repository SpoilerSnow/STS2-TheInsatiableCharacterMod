using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Phototaxis : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromPower<WeakPower>(),
		HoverTipFactory.FromCard<NegativePhototaxis>(),
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new PowerVar<WeakPower>(1),
		new PowerVar<PhototaxisPower>(1),
    ];
	public Phototaxis()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.AttackAnimDelay);
		await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars.Weak.IntValue, base.Owner.Creature, this);
		await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<NegativePhototaxis>(base.Owner), PileType.Hand, base.Owner);
		await PowerCmd.Apply<PhototaxisPower>(choiceContext, base.Owner.Creature, base.DynamicVars["PhototaxisPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars["PhototaxisPower"].UpgradeValueBy(1);
		base.DynamicVars.Weak.UpgradeValueBy(1);
	}
}