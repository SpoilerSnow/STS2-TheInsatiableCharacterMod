using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class MillipedeNeverDie : InsatiableCardModel
{
	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<MillipedeNeverDiePower>(1)];
	public MillipedeNeverDie()
		: base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<MillipedeNeverDiePower>(choiceContext, base.Owner.Creature, base.DynamicVars["MillipedeNeverDiePower"].BaseValue, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
    {
       base.DynamicVars["MillipedeNeverDiePower"].UpgradeValueBy(1);
    }
}