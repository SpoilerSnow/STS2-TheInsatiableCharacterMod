using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheInsatiable.Scripts.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;
using TheInsatiable.Scripts.CardKeywords;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(TokenCardPool))]

public class GoldRush : InsatiableCardModel
{
	public override bool CanBeGeneratedInCombat => false;
	protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Gold1", 10),
        new DynamicVar("Gold2", 5)
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [
        HoverTipFactory.FromPower<QuickSandPower>(),
        HoverTipFactory.FromKeyword(TheInsatiableKeyword.Digest)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	public GoldRush()
		: base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainGold(base.DynamicVars["Gold1"].IntValue, base.Owner);
    }
    public override async Task OnDigest()
	{
        FlashOnPlayer();
        await Cmd.Wait(0.3f);
		var targets = base.CombatState.Creatures.Where(Creature => Creature.HasPower<QuickSandPower>()).ToList();
        foreach (var creature in targets)
        {
            await PlayerCmd.GainGold(base.DynamicVars["Gold2"].IntValue, base.Owner);
        }
	}
    protected override void OnUpgrade()
	{
		base.DynamicVars["Gold1"].UpgradeValueBy(2);
        base.DynamicVars["Gold2"].UpgradeValueBy(2);
	}
}