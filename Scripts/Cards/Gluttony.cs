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

public class Gluttony : InsatiableCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.SelfSwallow];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GluttonyPower>(1)];
	public Gluttony()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);  
		await PowerCmd.Apply<GluttonyPower>(choiceContext, base.Owner.Creature, base.DynamicVars["GluttonyPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        RemoveKeyword(TheInsatiableKeyword.SelfSwallow);
    }
}