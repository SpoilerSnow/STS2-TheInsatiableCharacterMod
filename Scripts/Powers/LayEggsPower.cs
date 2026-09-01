using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.Cards;

namespace TheInsatiable.Scripts.Powers;

[RegisterPower]
public class LayEggsPower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromCard<Hatch>(),
        HoverTipFactory.FromCard<Nibble>(),
    ];
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == base.Owner.Player)
		{
			Flash();
			SfxCmd.Play("event:/sfx/enemy/enemy_attacks/egg_layer/egg_layer_lay");
			for (int i = 0; i < Amount; i++)
			{
				CardModel hatch = base.CombatState.CreateCard<Hatch>(base.Owner.Player);
                CardModel nibble = base.CombatState.CreateCard<Nibble>(base.Owner.Player);
				await CardPileCmd.AddGeneratedCardToCombat(hatch, PileType.Hand, base.Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(nibble, PileType.Hand, base.Owner.Player);
			}
		}
	}
}