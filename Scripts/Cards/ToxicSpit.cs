using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]
public class ToxicSpit : InsatiableCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.Insect];
	public ToxicSpit() 
		: base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<Toxic>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(13, ValueProp.Move)];
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
		    .FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/mite/mite_cast")
			.WithHitFx("vfx/vfx_slime_impact")
			.Execute(choiceContext);
		CardModel card = base.CombatState.CreateCard<Toxic>(base.Owner);
		CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Discard, base.Owner));
		await Cmd.Wait(0.5f);
	}

	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(4);
	}
}