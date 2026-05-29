using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts;

[HarmonyPatch(typeof(CombatManager), "DoTurnEnd")]
public static class CombatManager_DoTurnEnd_SelfSwallowPatch
{
    public static async Task Postfix(Task __result, Player player, PlayerChoiceContext choiceContext)
    {
        await __result;
        var handPile = PileType.Hand.GetPile(player);
        List<CardModel> turnEndCards = new List<CardModel>();
        List<CardModel> swallowCards = new List<CardModel>();
        foreach (var card in handPile.Cards)
        {
            if (card.HasTurnEndInHandEffect)
			{
				turnEndCards.Add(card);
			}
			else if (card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow) && TheInsatiableHook.ShouldSelfSwallowTrigger(player.Creature.CombatState, card))
            {
                swallowCards.Add(card);
            }
        }
        foreach (var card in swallowCards)
        {
            await TheInsatiableCmd.SwallowCard(choiceContext, card, causedBySelfSwallow: true);
        }
        foreach (CardModel item2 in turnEndCards)
		{
			await item2.OnTurnEndInHandWrapper(choiceContext);
		}
    }
}
