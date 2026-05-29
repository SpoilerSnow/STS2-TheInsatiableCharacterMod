using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class LawOfHive : InsatiableCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("LawOfHive", 8),
        new MaxHpVar(2)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Swallow),
        HoverTipFactory.Static(StaticHoverTip.Fatal)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    public LawOfHive()
		: base(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            bool shouldTriggerFatal = enemy.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
            int hpamount = enemy.CurrentHp;
            if (hpamount <= base.DynamicVars["LawOfHive"].IntValue)
            {
                await CreatureCmd.TriggerAnim(base.Owner.Creature, "EatPlayer", 0.5f);
			    await Cmd.Wait(2f);
                await TheInsatiableCmd.SwallowCreature(enemy);
                if (shouldTriggerFatal)
		        {
			        await CreatureCmd.GainMaxHp(base.Owner.Creature, base.DynamicVars.MaxHp.IntValue);
		        }
            }
        }
    }
    protected override void OnUpgrade()
	{
        base.DynamicVars["LawOfHive"].UpgradeValueBy(3);
        base.DynamicVars.MaxHp.UpgradeValueBy(1);
	}
}