using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class DeathHighway : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
	protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(8, ValueProp.Move),
        new CardsVar(2)
    ];
	public DeathHighway() 
		: base(1, CardType.Attack, CardRarity.Uncommon,TargetType.AnyEnemy)
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
		await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
	}
	public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount <= 0m) && applier == base.Owner.Creature && power is QuickSandPower)
        {
            PileType? currentPile = this.Pile?.Type;
            if (currentPile == PileType.Draw || currentPile == PileType.Discard)
            {
				if (currentPile == PileType.Draw && this.Pile?.Cards.FirstOrDefault() == this)
                {
                    return;
                }
                await CardPileCmd.Add(this, PileType.Draw, CardPilePosition.Top);
            }
        }
    }
	protected override void OnUpgrade()
	{
		base.DynamicVars.Damage.UpgradeValueBy(3);
	}
}