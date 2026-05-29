using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class GrainReserves : InsatiableCardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Unplayable];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest),
        base.EnergyHoverTip
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2),
        new EnergyVar(1)
    ];
    public GrainReserves()
		: base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    public override async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
	{
		if (card == this && base.CombatState != null)
		{
			int playCount = await GeneratePlayCount(base.CombatState, null);
			for (int i = 0; i < playCount; i++)
			{
                await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
				await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
			}
		}
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Energy.UpgradeValueBy(1);
	}
}