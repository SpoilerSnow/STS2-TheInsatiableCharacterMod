using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Powers;
[RegisterPower]
public class SurvivalOfTheFittestPower : InsatiablePowerModel
{
    public override PowerType Type => PowerType.Buff;
	public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow)];
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        foreach (var creature in base.CombatState.Creatures.Where(c => c != base.Owner))
        {
            if (creature.CurrentHp > 0 && (creature.CurrentHp < 10 || creature.CurrentHp < creature.MaxHp * 0.1))
            {
                Flash();
                await CreatureCmd.TriggerAnim(base.Owner, "EatPlayer", 0.5f);
			    await Cmd.Wait(2f);
                await CreatureCmd.Heal(base.Owner, creature.CurrentHp);
                await TheInsatiableCmd.SwallowCreature(creature);
                await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner, base.Amount, base.Owner, null);
			    await PowerCmd.Apply<DexterityPower>(choiceContext, base.Owner, base.Amount, base.Owner, null);
            }
        }
        if (base.Owner.CurrentHp < 10 || base.Owner.CurrentHp < base.Owner.MaxHp * 0.1)
        {
            Flash();
            await CreatureCmd.TriggerAnim(base.Owner, "EatPlayer", 0.5f);
			await Cmd.Wait(2f);
            await TheInsatiableCmd.SwallowCreature(base.Owner);
        }
    }

}