using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Pools;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class LurchForLunch : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => IsUpgraded ? [
        HoverTipFactory.FromCard<InsatiableSwallow>(),
        base.EnergyHoverTip] : 
    [HoverTipFactory.FromCard<InsatiableSwallow>()];
    public LurchForLunch()
		: base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        if (IsUpgraded)
		{
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
		}
        await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<InsatiableSwallow>(base.Owner), PileType.Hand, base.Owner);
    }
}
