using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheInsatiable.Scripts;

[Pool(typeof(InsatiableCardPool))]

public class GoldRush : InsatiableCardModel
{
    private const string _goldKey = "Gold";
	public override bool CanBeGeneratedInCombat => false;
	protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Gold", 10)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	public GoldRush()
		: base(0, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var targets = base.CombatState.Creatures
        .Where(Creature => Creature.HasPower<QuickSandPower>())
        .ToList();
        foreach (var creature in targets)
        {
            await PlayerCmd.GainGold(base.DynamicVars["Gold"].IntValue, base.Owner);
        }
    }
    protected override void OnUpgrade()
	{
		base.DynamicVars["Gold"].UpgradeValueBy(5);
	}
}