using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class Evaporate : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<QuickSandPower>(7),
        new CardsVar(1)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
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
        if (quicksandamount >= hpamount)
        {
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "EatPlayer", 0.5f);
			await Cmd.Wait(2f);
            await TheInsatiableCmd.SwallowCreature(cardPlay.Target);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(4);
    }
}