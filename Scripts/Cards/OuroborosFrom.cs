using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class OuroborosFrom : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [TheInsatiableKeyword.SelfSwallow];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Dynamic)
	];
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<OuroborosFromPower>(1)];
	public OuroborosFrom()
		: base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<OuroborosFromPower>(choiceContext, base.Owner.Creature, base.DynamicVars["OuroborosFromPower"].BaseValue, base.Owner.Creature, this);
		List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            await ((IChoosable)cardModel).OnChosen(choiceContext);
        }
	}
	protected override void OnUpgrade()
    {
        RemoveKeyword(TheInsatiableKeyword.SelfSwallow);
    }
}