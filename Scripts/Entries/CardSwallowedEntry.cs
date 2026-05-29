using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

public class CardSwallowedEntry : CombatHistoryEntry
{
	public CardModel Card { get; }
	public override string Description => base.Actor.Player.Character.Id.Entry + " swallowed " + Card.Id.Entry;
	public CardSwallowedEntry(CardModel card, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
		: base(card.Owner.Creature, roundNumber, currentSide, history, players)
	{
		Card = card;
	}
}
