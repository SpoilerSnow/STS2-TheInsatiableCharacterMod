using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Piles;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Relics;

[RegisterRelic(typeof(InsatiableRelicPool))]
public class GastrolithRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Gulp),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest)
	];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new MaxTurnsInPileVar(1),
		new DynamicVar("Repeat", 1)
	];
	public override Task BeforeCombatStart()
	{
		SwallowPile.MaxTurnsInPile -= (int)DynamicVars["MaxTurnsInPile"].BaseValue;
        return Task.CompletedTask;
    }
	public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		var swallowPile = CardPile.Get(Entry.SwallowPile, player);
		if (swallowPile != null && player == base.Owner)
		{
			int count = player.Relics.OfType<GastrolithRelic>().Count() * (int)DynamicVars["Repeat"].BaseValue;
			var cards = swallowPile.Cards.ToList();
			foreach (var c in cards)
			{
				if (c is InsatiableCardModel icm)
				{
					for (int i = 0; i < count; i++)
					{
						await icm.OnDigest();
					}
				}
			}
		}
	}
	public override async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
    {
		if (card.Owner == base.Owner)
		{
			int count = card.Owner.Relics.OfType<GastrolithRelic>().Count() * (int)DynamicVars["Repeat"].BaseValue;
			if (card is InsatiableCardModel icm)
			{
				for (int i = 0; i < count; i++)
				{
					await icm.OnGulp();
				}
			}
		}
    }
}
