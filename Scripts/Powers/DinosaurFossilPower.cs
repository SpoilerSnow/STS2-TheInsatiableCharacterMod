using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class DinosaurFossilPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        Reward? reward = null;
        for (int i = 0; i < base.Amount; i++)
        {
        int choice = base.Owner.Player.RunState.Rng.Niche.NextInt(4);
        switch (choice)
        {
            case 0:
            {
                reward = new CardReward(CardCreationOptions.ForRoom(base.Owner.Player, RoomType.Boss), 1, base.Owner.Player);
                break;
            }
            case 1:
                reward = new RelicReward(base.Owner.Player);
                break;
            case 2:
                room.AddExtraReward(base.Owner.Player, new PotionReward(base.Owner.Player));
                room.AddExtraReward(base.Owner.Player, new PotionReward(base.Owner.Player));
                reward = null;
                break;
            case 3:
                reward = new GoldReward(100, base.Owner.Player, false);
                break;
        }
        if (reward != null)
        {
            room.AddExtraReward(base.Owner.Player, reward);
        }
        }
        await Task.CompletedTask;
    }
}