using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class DinosaurFossilPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        Reward? reward = null;
        for (int i = 0; i < base.Amount; i++)
        {
        int choice = base.Owner.Player.RunState.Rng.CombatCardSelection.NextInt(4);
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
                reward = new PotionReward(base.Owner.Player);
                break;
            case 3:
                reward = new GoldReward(75, base.Owner.Player, false);
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