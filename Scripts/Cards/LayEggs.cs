using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class LayEggs : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromCard<Hatch>(),
        HoverTipFactory.FromCard<Nibble>(),
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<LayEggsPower>(1)];
	public LayEggs()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<LayEggsPower>(choiceContext, base.Owner.Creature, base.DynamicVars["LayEggsPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
