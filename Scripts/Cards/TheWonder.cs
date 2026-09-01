using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public sealed class TheWonder : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<TheWonderPower>(1)];
	public TheWonder()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
       await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
	   await PowerCmd.Apply<TheWonderPower>(choiceContext, base.Owner.Creature, base.DynamicVars["TheWonderPower"].BaseValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
		AddKeyword(CardKeyword.Retain);
	}
}