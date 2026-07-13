using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Churning : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromPower<PoisonPower>()
    ];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7, ValueProp.Move),
        new PowerVar<PoisonPower>(2)
    ];
	public Churning()
		: base(2, CardType.Attack, CardRarity.Rare, TargetType.RandomEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        IEnumerable<CardModel> enumerable =Entry.SwallowPile.GetPile(base.Owner).Cards.ToList();
        int SwallowPile = Entry.SwallowPile.GetPile(base.Owner).Cards.Count;
		foreach (CardModel item in enumerable)
		{
			await CardPileCmd.Add(item, PileType.Hand);
		}
        for (int i = 0; i < SwallowPile; i++)
		{
			Creature enemy = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this, cardPlay)
                .Targeting(enemy)
		        .WithHitFx("vfx/vfx_slime_impact")
		        .Execute(choiceContext);
            await PowerCmd.Apply<PoisonPower>(choiceContext, enemy, base.DynamicVars.Poison.BaseValue, base.Owner.Creature, this);
        }
	}
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(2);
        base.DynamicVars.Poison.UpgradeValueBy(1);
	}
}