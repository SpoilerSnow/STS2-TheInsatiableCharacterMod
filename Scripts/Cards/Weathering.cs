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

public class Weathering : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromAffliction<WeatheringAffliction>(2);
	protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
	public Weathering()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<WeatheringPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		AddKeyword(CardKeyword.Innate);
	}
}