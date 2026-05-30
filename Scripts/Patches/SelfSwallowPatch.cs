using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts;

[HarmonyPatch(typeof(CombatManager), "DoTurnEnd")]
public static class CombatManager_DoTurnEnd_SelfSwallowPatch
{
    public static async Task Postfix(Task __result, Player player, PlayerChoiceContext choiceContext)
    {
		await __result;
        await player.PlayerCombatState.OrbQueue.BeforeTurnEnd(choiceContext);
		if (CombatManager.Instance.IsOverOrEnding)
		{
			return;
		}
		CardPile pile = PileType.Hand.GetPile(player);
		PileType.Discard.GetPile(player);
		List<CardModel> turnEndCards = new List<CardModel>();
        List<CardModel> selfSwallowCards = new List<CardModel>();
		foreach (CardModel card in pile.Cards)
		{
			if (card.HasTurnEndInHandEffect)
			{
				turnEndCards.Add(card);
			}
            else if (card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow) && TheInsatiableHook.ShouldSelfSwallowTrigger(player.Creature.CombatState, card))
            {
                selfSwallowCards.Add(card);
            }
		}
        foreach (CardModel item3 in selfSwallowCards)
        {
            await TheInsatiableCmd.SwallowCard(choiceContext, item3, causedBySelfSwallow: true);
        }
		foreach (CardModel item2 in turnEndCards)
		{
			await item2.OnTurnEndInHandWrapper(choiceContext);
		}
    }
}

[HarmonyPatch(typeof(CardModel), "OnTurnEndInHandWrapper")]
public static class CardModel_OnTurnEndInHandWrapper_SelfSwallowPatch
{
    public static async void Postfix(CardModel __instance, PlayerChoiceContext choiceContext)
    {
        if (__instance.Keywords.Contains(TheInsatiableKeyword.SelfSwallow))
        {
            await TheInsatiableCmd.SwallowCard(choiceContext, __instance, causedBySelfSwallow: true);
			return;
        }
    }
}


