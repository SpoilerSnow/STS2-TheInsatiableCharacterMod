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

public class LayEggs : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
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