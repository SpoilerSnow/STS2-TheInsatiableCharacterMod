using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[RegisterRelic(typeof(InsatiableRelicPool))]
public class ObeliskRelic : InsatiableRelicModel
{
	public override RelicRarity Rarity => RelicRarity.Rare;
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.FromKeyword(CardKeyword.Innate),
		HoverTipFactory.FromKeyword(CardKeyword.Retain),
	];
	bool activated = false;
	public override Task BeforeCombatStart()
	{
		foreach (CardModel card in base.Owner.PlayerCombatState.AllCards)
		{
			bool hasInnate = card.Keywords.Contains(CardKeyword.Innate);
			bool hasRetain = card.Keywords.Contains(CardKeyword.Retain);
			if (hasInnate && !hasRetain)
			{
				CardCmd.ApplyKeyword(card, CardKeyword.Retain);
				activated = true;
			}
			else if (!hasInnate && hasRetain)
			{
				CardCmd.ApplyKeyword(card, CardKeyword.Innate);
				activated = true;
			}
		}
		if (activated) Flash();
		return Task.CompletedTask;
	}
	public override async Task AfterCardEnteredCombat(CardModel card)
	{
		if (card.Owner == base.Owner)
		{
			bool hasInnate = card.Keywords.Contains(CardKeyword.Innate);
			bool hasRetain = card.Keywords.Contains(CardKeyword.Retain);
			if (hasInnate && !hasRetain)
			{
				CardCmd.ApplyKeyword(card, CardKeyword.Retain);
				activated = true;
			}
			else if (!hasInnate && hasRetain)
			{
				CardCmd.ApplyKeyword(card, CardKeyword.Innate);
				activated = true;
			}
		}
		if (activated) Flash();
	}
}
