using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;

/// <summary>
/// 偷窃技巧 Power：按蚱蜢偷窃优先级从敌方牌组偷取一张牌，
/// 3回合后将其复制加入手牌。
/// </summary>
[RegisterPower]
public class StealTechniquePower : InsatiablePowerModel
{
	private static readonly Func<CardModel, bool>[] _stealPriorities =
	[
		c => c.Enchantment is not Imbued && c.Rarity == CardRarity.Uncommon,
		c => c.Enchantment is not Imbued && (c.Rarity is CardRarity.Common or CardRarity.Rare or CardRarity.Event),
		c => c.Enchantment is not Imbued && (c.Rarity is CardRarity.Basic or CardRarity.Quest),
		c => c.Rarity == CardRarity.Ancient || c.Enchantment is Imbued,
	];

	private CardModel? _stolenCard;

	public override PowerType Type => PowerType.Buff;

	public override PowerStackType StackType => PowerStackType.Counter;

	public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

	protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
		_stolenCard != null
			? [HoverTipFactory.FromCard(_stolenCard)]
			: Array.Empty<IHoverTip>();

	public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		if (base.Applier?.Player == null) return;

		Player ownerPlayer = base.Applier.Player;
		List<CardModel> candidateCards = (
			from c in CardPile.GetCards(ownerPlayer, PileType.Draw, PileType.Discard)
			where c.DeckVersion != null
			select c
		).ToList();

		if (candidateCards.Count == 0) return;

		// 按蚱蜢优先级选择
		IEnumerable<CardModel> selected = candidateCards;
		foreach (Func<CardModel, bool> predicate in _stealPriorities)
		{
			IEnumerable<CardModel> filtered = candidateCards.Where(predicate);
			if (filtered.Any())
			{
				selected = filtered;
				break;
			}
		}

		CardModel cardToSteal = base.CombatState.RunState.Rng.CombatCardSelection.NextItem(selected);
		if (cardToSteal == null) return;

		await TheInsatiableCmd.SwallowCard(new ThrowingPlayerChoiceContext(), cardToSteal);
		_stolenCard = cardToSteal;
	}

	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (!participants.Contains(base.Owner)) return;

		await PowerCmd.Decrement(this);

		if (base.Amount <= 0 && _stolenCard != null)
		{
			Flash();
			CardModel copy = _stolenCard.CreateClone();
			copy.SetToFreeThisTurn();
			await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, base.Owner.Player);
			await PowerCmd.Remove(this);
		}
	}
}
