using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class DinosaurFossil : InsatiableCardModel
{
    public override bool CanBeGeneratedInCombat => false;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<DinosaurFossilPower>(1)];
	public DinosaurFossil()
		: base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);  
		await PowerCmd.Apply<DinosaurFossilPower>(choiceContext, base.Owner.Creature, base.DynamicVars["DinosaurFossilPower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}