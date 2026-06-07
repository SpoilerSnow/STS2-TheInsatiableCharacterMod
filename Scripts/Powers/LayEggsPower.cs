using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace TheInsatiable.Scripts;

public sealed class LayEggsPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromCard<Hatch>(),
        HoverTipFactory.FromCard<Nibble>(),
    ];
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == base.Owner.Player)
		{
			Flash();
			for (int i = 0; i < Amount; i++)
			{
				await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<Hatch>(base.Owner.Player), PileType.Hand, base.Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<Nibble>(base.Owner.Player), PileType.Hand, base.Owner.Player);
			}
		}
	}
}