using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class MobileDune : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromPower<QuickSandPower>(),
		HoverTipFactory.FromPower<SandySkyPower>()
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<MobileDunePower>(1),
		new PowerVar<SandySkyPower>(2)
	];
	public MobileDune()
		: base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<MobileDunePower>(choiceContext, base.Owner.Creature, base.DynamicVars["MobileDunePower"].BaseValue, base.Owner.Creature, this);
		await PowerCmd.Apply<SandySkyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["SandySkyPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}