using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.CardKeywords;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Entities.Players;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public sealed class ChaseSequence : InsatiableCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate];
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
		base.EnergyHoverTip,
		HoverTipFactory.FromCard<FranticEscape>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
		new PowerVar<ChaseSequencePower>(2),
		new PowerVar<RunPower>(1),
		new EnergyVar(1)
	];
	public ChaseSequence()
		: base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "PowerUp", base.Owner.Character.CastAnimDelay);
	    await PowerCmd.Apply<ChaseSequencePower>(choiceContext, base.Owner.Creature, base.DynamicVars["ChaseSequencePower"].BaseValue, base.Owner.Creature, this);
		IEnumerable<Player> enumerable = base.CombatState.Players.Where((Player p) => p.Creature.IsAlive && p != base.Owner);
		foreach (Player player1 in enumerable)
		{
			await PowerCmd.Apply<RunPower>(choiceContext, player1.Creature, base.DynamicVars["RunPower"].BaseValue, base.Owner.Creature, this);
		}
	}
    protected override void OnUpgrade()
	{
        DynamicVars["ChaseSequencePower"].UpgradeValueBy(-1);
	}
}