using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using TheInsatiable.Scripts.Piles;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Dessert : InsatiableCardModel
{
	public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
		new BlockVar(7, ValueProp.Move),
		new MaxCapacityVar(1),
	];
	protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
		HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)
    ];
	public Dessert()
		: base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		CardPile pile = PileType.Discard.GetPile(base.Owner);
		CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs)).FirstOrDefault();
        if (cardModel != null)
		{
			bool swallowed = await TheInsatiableCmd.SwallowCard(choiceContext, cardModel);
            if (swallowed == true && choiceContext != null)
            {
                if (!SwallowPile.IsLocked)
                {
                    SwallowPile.MaxCapacity += (int)DynamicVars["MaxCapacity"].BaseValue;
                }
            }
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(3);
	}
}