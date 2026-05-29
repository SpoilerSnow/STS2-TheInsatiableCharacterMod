using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class ScorchedEarth : InsatiableCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];

	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<ScorchedEarthPower>(1),
        new PowerVar<QuickSandPower>(6)
    ];
	public ScorchedEarth()
		: base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].IntValue, base.Owner.Creature, this);    
		await PowerCmd.Apply<ScorchedEarthPower>(choiceContext, base.Owner.Creature, base.DynamicVars["ScorchedEarthPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.DynamicVars["QuickSandPower"].UpgradeValueBy(2);
		base.EnergyCost.UpgradeBy(-1);
    }
}