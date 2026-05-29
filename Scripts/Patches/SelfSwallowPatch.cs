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
        await player.PlayerCombatState.OrbQueue.BeforeTurnEnd(choiceContext);
		if (CombatManager.Instance.IsOverOrEnding)
		{
			return;
		}
		CardPile pile = PileType.Hand.GetPile(player);
		PileType.Discard.GetPile(player);
		List<CardModel> turnEndCards = new List<CardModel>();
		List<CardModel> list = new List<CardModel>();
        List<CardModel> selfSwallowCards = new List<CardModel>();
		foreach (CardModel card in pile.Cards)
		{
			if (card.HasTurnEndInHandEffect)
			{
				turnEndCards.Add(card);
			}
			else if (card.Keywords.Contains(CardKeyword.Ethereal) && Hook.ShouldEtherealTrigger(player.Creature.CombatState, card))
			{
				list.Add(card);
			}
            else if (card.Keywords.Contains(TheInsatiableKeyword.SelfSwallow) && TheInsatiableHook.ShouldSelfSwallowTrigger(player.Creature.CombatState, card))
            {
                selfSwallowCards.Add(card);
            }
		}
		foreach (CardModel item in list)
		{
			await CardCmd.Exhaust(choiceContext, item, causedByEthereal: true);
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
	public static async Task Postfix(Task __result, PlayerChoiceContext choiceContext)
	{
		await CardPileCmd.Add(this, PileType.Play);
		if (LocalContext.IsMe(Player.Owner))
		{
			await Cmd.CustomScaledWait(0.3f, 0.6f);
		}
		await OnTurnEndInHand(choiceContext);
		if (Keywords.Contains(CardKeyword.Ethereal))
		{
			await CardCmd.Exhaust(choiceContext, this, causedByEthereal: true);
			return;
		}
		if (Keywords.Contains(TheInsatiableKeyword.SelfSwallow))
		{
			await TheInsatiableCmd.SwallowCard(choiceContext, this, causedBySelfSwallow: true);
			return;
		}
		CardPile pile = GetResultPileTypeForOnTurnEndInHandEffect().GetPile(Player.Owner);
		await CardPileCmd.Add(this, pile);
	}
}

