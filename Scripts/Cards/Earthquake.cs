using STS2RitsuLib.Interop.AutoRegistration;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheInsatiable.Scripts.Pools;
using TheInsatiable.Scripts.Powers;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Helpers;

namespace TheInsatiable.Scripts.Cards;

[RegisterCard(typeof(InsatiableCardPool))]

public class Earthquake : InsatiableCardModel
{
    public Earthquake()
		: base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<QuickSandPower>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(10, ValueProp.Move),
        new PowerVar<QuickSandPower>(4)
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        Vector2 enemiesCenter = Vector2.Zero;
        int enemyCount = 0;
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            var enemyNode = enemy.GetCreatureNode();
            if (enemyNode != null)
            {
                enemiesCenter += enemyNode.GlobalPosition;
                enemyCount++;
            }
        }
        if (enemyCount > 0)
        {
            enemiesCenter /= enemyCount;
        }
        Node2D node2D = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("vfx/vfx_decimillipede_rocks")).Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        node2D.GlobalPosition = enemiesCenter;
        base.Owner.Creature.GetVfxContainer()?.AddChildSafely(node2D);
        foreach (var enemy in base.CombatState.HittableEnemies)
        {
            await CreatureCmd.LoseBlock(new ThrowingPlayerChoiceContext(), enemy, enemy.Block, base.Owner.Creature);
        }
        await Cmd.Wait(0.45f);
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_blunt")
            .Execute(choiceContext);
		await PowerCmd.Apply<QuickSandPower>(new ThrowingPlayerChoiceContext(), base.CombatState.HittableEnemies, base.DynamicVars["QuickSandPower"].BaseValue, base.Owner.Creature, this);
	}
    protected override void OnUpgrade()
	{
        base.DynamicVars.Damage.UpgradeValueBy(3);
		base.DynamicVars["QuickSandPower"].UpgradeValueBy(2);
	}

}