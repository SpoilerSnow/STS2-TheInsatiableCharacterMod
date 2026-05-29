using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class HoneyXylitol : InsatiableCardModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest),
        HoverTipFactory.FromPower<StrengthPower>()
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<HoneyXylitolPower>(2),
        new PowerVar<StrengthPower>(2),
    ];
    public HoneyXylitol()
		: base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<HoneyXylitolPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars["HoneyXylitolPower"].IntValue, base.Owner.Creature, this);
	}
    public override async Task AfterCardSwallow(ICombatState combatState, PlayerChoiceContext choiceContext, CardModel card, bool causedBySelfSwallow)
	{
		if (card == this && base.CombatState != null)
		{
			int playCount = await GeneratePlayCount(base.CombatState, null);
			for (int i = 0; i < playCount; i++)
			{
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature, base.DynamicVars.Strength.IntValue, base.Owner.Creature, this);
			}
		}
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars["HoneyXylitolPower"].UpgradeValueBy(1);
        base.DynamicVars.Strength.UpgradeValueBy(1);
	}
}