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
        if (CombatManager.Instance == null
            || CombatManager.Instance.IsOverOrEnding
            || player == null
            || player.Creature == null
            || player.Creature.IsDead
            || player.PlayerCombatState == null)
        {
            return;
        }
        var handPile = PileType.Hand.GetPile(player);
        if (handPile == null || handPile.Cards == null) return;
        List<CardModel> swallowCards = new List<CardModel>();
        foreach (var card in handPile.Cards)
        {
            if (card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow) && TheInsatiableHook.ShouldSelfSwallowTrigger(player.Creature.CombatState, card))
            {
                swallowCards.Add(card);
            }
        }
        foreach (var card in swallowCards)
        {
            await TheInsatiableCmd.SwallowCard(choiceContext, card, causedBySelfSwallow: true);
        }
    }
}
