using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Dune : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<QuickSandPower>(7),
        new IntVar("Replay", 1)
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Piles),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    ];

	public Dune()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        int quickSandPowerCount = 0;
        foreach (var creature in base.CombatState.Creatures)
        {
            int creatureCurrent = creature.GetPowerAmount<QuickSandPower>();
            quickSandPowerCount += creatureCurrent;
            if (creatureCurrent > 0)
            {
                await PowerCmd.Remove(creature.GetPower<QuickSandPower>());
            }
        }
        int replayCount = quickSandPowerCount / base.DynamicVars["QuickSandPower"].IntValue;
        List<CardModel> list1 = PileType.Draw.GetPile(base.Owner).Cards.Where(card => !card.Keywords.Contains(CardKeyword.Unplayable)).ToList();
        List<CardModel> list2 = PileType.Hand.GetPile(base.Owner).Cards.Where(card => !card.Keywords.Contains(CardKeyword.Unplayable)).ToList();
        List<CardModel> list3 = PileType.Discard.GetPile(base.Owner).Cards.Where(card => !card.Keywords.Contains(CardKeyword.Unplayable)).ToList();
        IEnumerable<CardModel> items = list1.Concat(list2).Concat(list3).ToList();
        for (int i = 0; i < replayCount; i++)
        {
            CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(items);
            if (cardModel != null)
		    { 
			    cardModel.BaseReplayCount += base.DynamicVars["Replay"].IntValue;
			    CardCmd.Preview(cardModel);
		    }
        }
	}

	protected override void OnUpgrade()
	{
		AddKeyword(CardKeyword.Retain);
	}
}