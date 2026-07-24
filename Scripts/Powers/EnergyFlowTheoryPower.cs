using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public sealed class EnergyFlowTheoryPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        IEnumerable<Creature> enumerable = base.CombatState.PlayerCreatures.ToList();
        var sortedPlayers = enumerable.OrderBy(p => (float)p.CurrentHp / p.MaxHp).ToList();
        int hp = base.Amount;
        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            var player = sortedPlayers[i];
            await CreatureCmd.Heal(player, hp);
            hp--;
        }
    }
}