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
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Simulate : InsatiableCardModel
{
	public override bool GainsBlock => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(18, ValueProp.Move),];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Piles),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Insect)
	];
	public Simulate()
		: base(3, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		List<CardModel> list1 = PileType.Draw.GetPile(base.Owner).Cards.Where(card => card.Keywords.Contains(TheInsatiableKeyword.Insect)).ToList();
        List<CardModel> list2 = PileType.Hand.GetPile(base.Owner).Cards.Where(card => card.Keywords.Contains(TheInsatiableKeyword.Insect)).ToList();
        List<CardModel> list3 = PileType.Discard.GetPile(base.Owner).Cards.Where(card => card.Keywords.Contains(TheInsatiableKeyword.Insect)).ToList();
		IEnumerable<CardModel> items = list1.Concat(list2).Concat(list3).ToList();
		CardModel cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(items);
		if (cardModel != null)
		{
			await CardCmd.AutoPlay(choiceContext, cardModel, null);
		}
	}

	protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(6);
	}
}

