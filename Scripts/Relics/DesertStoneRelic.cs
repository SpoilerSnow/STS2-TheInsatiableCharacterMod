using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableRelicPool))]

public class DesertStoneRelic : InsatiableRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<QuickSandPower>(4), 
		new CardsVar(1)
		];
    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (participants.Contains(base.Owner.Creature) && base.Owner.PlayerCombatState.TurnNumber <= 1)
		{
			foreach (Creature hittableEnemy2 in base.Owner.Creature.CombatState.HittableEnemies)
		    {
			    await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), hittableEnemy2, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, null);
		    }
		    Flash();
		}
    }
	public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
	{
		if (player == base.Owner && combatState.RoundNumber == 1)
		{
			List<CardModel> list = new List<CardModel>();
			for (int i = 0; i < base.DynamicVars.Cards.IntValue; i++)
			{
				list.Add(base.Owner.Creature.CombatState.CreateCard<SandStone>(base.Owner));
			}
			await CardPileCmd.AddGeneratedCardsToCombat(list, PileType.Hand, base.Owner);
		}
	}
	public override RelicModel? GetUpgradeReplacement()
	{
		return (RelicModel?)(object)ModelDb.Relic<PolishedDesertStoneRelic>();
	}
}