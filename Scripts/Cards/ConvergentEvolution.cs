using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class ConvergentEvolution : InsatiableCardModel
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

	public override IEnumerable<CardKeyword> CanonicalKeywords =>[CardKeyword.Exhaust];

	protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ConvergentEvolutionPower>(2)];

	public ConvergentEvolution()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
	{
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.PowerUpAnimDelay);
		ConvergentEvolutionPower ConvergentEvolutionPower = base.Owner.Creature.Powers.OfType<ConvergentEvolutionPower>().FirstOrDefault((ConvergentEvolutionPower s) => s.PlayerTarget == cardPlay.Target.Player);
		decimal baseValue = base.DynamicVars["ConvergentEvolutionPower"].BaseValue;
		if (ConvergentEvolutionPower != null)
		{
			await PowerCmd.ModifyAmount(choiceContext, ConvergentEvolutionPower, baseValue, base.Owner.Creature, this);
			return;
		}
		ConvergentEvolutionPower = await PowerCmd.Apply<ConvergentEvolutionPower>(choiceContext, base.Owner.Creature, baseValue, base.Owner.Creature, this);
		if (ConvergentEvolutionPower != null)
		{
			ConvergentEvolutionPower.PlayerTarget = cardPlay.Target.Player;
		}
	}

	protected override void OnUpgrade()
	{
		base.EnergyCost.UpgradeBy(-1);
	}
}