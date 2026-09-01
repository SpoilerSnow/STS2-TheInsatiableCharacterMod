using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Evaporate : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<QuickSandPower>(8),
        new CardsVar(1)
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)
    ];
    public Evaporate()
        : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), cardPlay.Target, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);
        int quicksandamount = cardPlay.Target.GetPowerAmount<QuickSandPower>();
        int hpamount = cardPlay.Target.CurrentHp;
        if (hpamount > 0 && quicksandamount >= hpamount)
        {
            await TheInsatiableCmd.SwallowCreature(base.Owner.Creature, cardPlay.Target);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(5);
    }
}