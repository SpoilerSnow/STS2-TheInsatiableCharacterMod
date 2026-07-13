using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Patches;

[HarmonyPatch(typeof(CombatManager), "DoTurnEnd")]
public static class CombatManager_DoTurnEnd_SelfSwallowPatch
{
    [HarmonyPostfix]
    public static async Task SelfSwallowPatch(Task __result, Player player, PlayerChoiceContext choiceContext)
    {
        await __result;
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        CardPile pile = PileType.Hand.GetPile(player);
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

[HarmonyPatch(typeof(CardModel), "OnTurnEndInHandWrapper")]
public static class CardModel_OnTurnEndInHandWrapper_SelfSwallowPatch
{
    [HarmonyPostfix]
    public static async Task OnTurnEndInHandWrapperSelfSwallowPatch(
        Task __result,
        CardModel __instance,
        PlayerChoiceContext choiceContext)
    {
        // 等待原方法执行完毕
        await __result;
        if (CombatManager.Instance.IsOverOrEnding)
        {
            return;
        }
        // 检查卡牌是否有 SelfSwallow 关键字
        if (__instance.Keywords.Contains(TheInsatiableKeyword.SelfSwallow))
        {
            // 触发吞噬效果
            await TheInsatiableCmd.SwallowCard(choiceContext, __instance, causedBySelfSwallow: true);
        }
    }
}
