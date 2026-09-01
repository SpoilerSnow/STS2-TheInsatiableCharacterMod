using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using TheInsatiable.Scripts.CardKeywords;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class MetamorphosisDevelopment : InsatiableCardModel
{
    public override bool GainsBlock => true;
    public MetamorphosisDevelopment()
		: base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Piles),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<WeakPower>(),
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(8, ValueProp.Move),
        new PowerVar<DexterityPower>(1),
        new EnergyVar(1),
        new PowerVar<WeakPower>(2)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Energy.IntValue, base.Owner.Creature, this);
        List<CardModel> cards = choesnpile1.Select(c => base.CombatState.CreateCard((CardModel)c, base.Owner)).ToList();
        CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            CardModel? swallowedCard = await ((IChoosable)cardModel).OnChosen(choiceContext);
            if (swallowedCard != null && choiceContext != null)
            {
                if (swallowedCard.Affliction != null)
                {
                    PowerModel powerModel = await PowerCmd.Apply<WeakPower>(choiceContext, base.Owner.Creature, base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);
                    if (powerModel != null)
		            {
			            powerModel.SkipNextDurationTick = false;
		            }
                }
                if (swallowedCard.Enchantment != null || swallowedCard.IsUpgraded)
                {
                    await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner.Creature, base.DynamicVars["DexterityPower"].BaseValue, base.Owner.Creature, this);
                }
            }
        }
    }
    protected override void OnUpgrade()
	{
        base.DynamicVars.Block.UpgradeValueBy(4);
	}
}