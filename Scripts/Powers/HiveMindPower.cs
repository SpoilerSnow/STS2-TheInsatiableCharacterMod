using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;
public class HiveMindPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay == null || cardPlay.Card == null || cardPlay.Card.Owner == null)
        {
            return;
        }
        CardPile drawPile = PileType.Draw.GetPile(base.Owner.Player);
        if (drawPile == null)
        {
            return;
        }
        string playedTitle = cardPlay.Card.Title.TrimEnd('+');
        List<CardModel> matchingCards = drawPile.Cards
            .Where(card => card.Title.TrimEnd('+') == playedTitle)
            .ToList();
        if (!matchingCards.Any())
        {
            return;
        }
        Flash();
        foreach (CardModel matchingCard in matchingCards)
        {
            matchingCard.RemoveFromCurrentPile(silent: true);
            await CardPileCmd.Add(matchingCard, PileType.Draw, CardPilePosition.Top, this, true);
        }
        await CardPileCmd.AutoPlayFromDrawPile(choiceContext, base.Owner.Player, matchingCards.Count, CardPilePosition.Top, false);
    }
}