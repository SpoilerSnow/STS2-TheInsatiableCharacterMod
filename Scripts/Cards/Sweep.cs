using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public sealed class Sweep : InsatiableCardModel
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        base.EnergyHoverTip
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(1),
        new EnergyVar(1)
    ];
	public Sweep()
		: base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
	    foreach (var player1 in base.CombatState.Players)
        {
            List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, player1)).ToList();
            CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, player1, canSkip: true);
            if (cardModel != null)
            {
                await ((IChoosable)cardModel).OnChosen(choiceContext);
            }
            await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, player1);
            await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, player1);
        }
	}
    protected override void OnUpgrade()
	{
		RemoveKeyword(CardKeyword.Exhaust);
	}
}