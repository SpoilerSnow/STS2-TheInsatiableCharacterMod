using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class InsatiableThrash : InsatiableCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new DamageVar(8, ValueProp.Move),
		new RepeatVar(2)];
	public InsatiableThrash() 
		: base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
		    .WithHitCount(base.DynamicVars.Repeat.IntValue)
			.FromCard(this, cardPlay)
			.Targeting(cardPlay.Target)
			.OnlyPlayAnimOnce()
			.WithAttackerAnim("Thrash", 0.3f)
			.WithAttackerFx(null, "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_thrash")
			.WithHitFx("vfx/vfx_scratch")
			.Execute(choiceContext);
		CardModel cardModel = (await CardSelectCmd.FromCombatPile(prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, 1), context: choiceContext, pile: PileType.Discard.GetPile(base.Owner), player: base.Owner)).FirstOrDefault();
		if (cardModel != null)
		{
			if (IsUpgraded)
			{
				CardCmd.Upgrade(cardModel);
			}
			await CardPileCmd.Add(cardModel, PileType.Hand);
		}
	}

	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(1);
	}
}
