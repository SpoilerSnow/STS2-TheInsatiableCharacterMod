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
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class EatLessMoreOften : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
	[
		new DamageVar(4, ValueProp.Move),
		new RepeatVar(4)
	];
	public EatLessMoreOften() 
		: base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
			.WithHitCount(base.DynamicVars.Repeat.IntValue)
			.FromCard(this)
			.Targeting(cardPlay.Target)
			.WithHitFx("vfx/vfx_bite")
			.Execute(choiceContext);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Repeat.UpgradeValueBy(1);
	}
	public override Task AfterCardEnteredCombat(CardModel card)
	{
		if (card != this)
		{
			return Task.CompletedTask;
		}
		if (base.IsClone)
		{
			return Task.CompletedTask;
		}
		int amount = CombatManager.Instance.History.Entries.OfType<CardSwallowedEntry>().Count((CardSwallowedEntry e) => e.Card.Owner == base.Owner && e.HappenedThisTurn(base.CombatState))
		           + CombatManager.Instance.History.Entries.OfType<CreatureSwallowedEntry>().Count((CreatureSwallowedEntry e) => e.HappenedThisTurn(base.CombatState));
		ReduceCostBy(amount);
		return Task.CompletedTask;
	}
	public override Task BeforeCardSwallow(CardModel card, bool causedBySelfSwallow)
	{
		if (card.Owner != base.Owner)
		{
			return Task.CompletedTask;
		}
		ReduceCostBy(1);
		return Task.CompletedTask;
	}
    public override Task BeforeCreatureSwallow(Creature creature, bool force)
    {
        if (creature == null)
		{
			return Task.CompletedTask;
		}
		ReduceCostBy(1);
		return Task.CompletedTask;
    }
	private void ReduceCostBy(int amount)
	{
		base.EnergyCost.AddThisTurn(-amount);
	}
}