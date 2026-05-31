



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
    [HarmonyPostfix]
    public static async Task SelfSwallowPatch(Task __result, Player player, PlayerChoiceContext choiceContext)
    {
        await __result;
        await player.PlayerCombatState.OrbQueue.BeforeTurnEnd(choiceContext);
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        CardPile pile = PileType.Hand.GetPile(player);
        PileType.Discard.GetPile(player);
        List<CardModel> selfSwallowCards = new List<CardModel>();
        foreach (CardModel card in pile.Cards)
        {
            if (card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow) && TheInsatiableHook.ShouldSelfSwallowTrigger(player.Creature.CombatState, card))
            {
                selfSwallowCards.Add(card);
            }
        }
        foreach (CardModel item3 in selfSwallowCards)
        {
            await TheInsatiableCmd.SwallowCard(choiceContext, item3, causedBySelfSwallow: true);
        }
    }
}