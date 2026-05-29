using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

public class CreatureSwallowedEntry : CombatHistoryEntry
{
    public Creature Creature { get; }
	public override string Description => " swallowed " + Creature.Monster.Id.Entry;
	public CreatureSwallowedEntry(Creature creature, int roundNumber, CombatSide currentSide, CombatHistory history, IEnumerable<Player> players)
		: base(creature, roundNumber, currentSide, history, players)
    {
        Creature = creature;
    }
}