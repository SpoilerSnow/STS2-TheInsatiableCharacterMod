using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;
public class InsatiableNoEscapePower : InsatiablePowerModel
{
	public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Single;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
	public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
	{
		Creature owner = base.Owner;
		if (applier?.Player?.Character is InsatiableCharacter insatiable)
		{
			if (owner.HasPower<InsatiableNoEscapePower>() is true)
			{
				insatiable.HasNoEscapePower = true;
			}
		}
	}
	public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? _, CardModel? __)
	{
		if (target == base.Owner && props.IsPoweredAttack() && result.UnblockedDamage > 0)
		{
			Flash();
			await CardPileCmd.AddGeneratedCardToCombat(base.CombatState.CreateCard<InsatiableSwallow>(base.Owner.Player), PileType.Hand, base.Owner.Player);
		}
	}
	
	public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
	{
		if (side != base.Owner.Side)
		{
			return;
		}
		Player? player = base.Owner.Player;
		if (player == null)
		{
			return;
		}
		IEnumerable<CardModel> cards = PileType.Draw.GetPile(player).Cards.Concat(PileType.Hand.GetPile(player).Cards).Concat(PileType.Discard.GetPile(player).Cards);
		if (cards.Any((CardModel card) => card is InsatiableSwallow))
		{
			return;
		}
		foreach (Creature enemy in base.CombatState.Enemies.ToList())
		{
			if (!enemy.IsDead)
			{
				if (SpineTracker.PlayerSpine != null)
		        {
					await CreatureCmd.TriggerAnim(((PowerModel)this).Owner, "EatPlayer", 0.5f);
					await Cmd.Wait(2f);
		        }
				await TheInsatiableCmd.SwallowCreature(enemy);
			}
		}
	}
	public override Task AfterCombatEnd(CombatRoom room)
    {
		if (base.Owner.Player?.Character is InsatiableCharacter insatiable)
		{
			insatiable.HasNoEscapePower = false;
		}
		return Task.CompletedTask;
	}
}