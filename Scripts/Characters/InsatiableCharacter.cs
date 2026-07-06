using System.Runtime.InteropServices;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Godot;
using Color = Godot.Color;

namespace TheInsatiable.Scripts;

[RegisterCharacter]
public class InsatiableCharacter : ModCharacterTemplate<InsatiableCardPool, InsatiableRelicPool, InsatiablePotionPool>
{
    // 角色名称颜色
    public override Color NameColor => new(188f / 255f, 130f / 255f, 54f / 255f, 1f);
    // 能量图标轮廓颜色
    public override Color EnergyLabelOutlineColor => new(127f / 255f, 86f / 255f, 31f / 255f, 1f);
    // 地图上角色标记颜色
    public override Color MapDrawingColor => new(188f / 255f, 130f / 255f, 54f / 255f, 1f);
    // 对话框颜色
    public override Color DialogueColor => new(188f / 255f, 130f / 255f, 54f / 255f, 1f);
    // 角色语音气泡颜色
    public override VfxColor SpeechBubbleColor => VfxColor.Orange;
    // 人物性别（男女中立）
    public override CharacterGender Gender => CharacterGender.Neutral;
    // 初始金币
    public override int StartingGold => 99;
    // 初始血量
    public override int StartingHp => 80;
    // 攻击动画延迟
    public override float AttackAnimDelay => 0.3f;
    // 施法动画延迟
    public override float CastAnimDelay => 0f;
    // 如果你的人物不需要时间线小故事，加上这句。
    public override bool RequiresEpochAndTimeline => false;
    // 角色资源资产配置
    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new(
            Scenes: new(
                // 人物模型tscn路径。
                VisualsPath: "res://TheInsatiable/animations/the_insatiable_character.tscn",
                // 能量表盘tscn路径。
                EnergyCounterPath: "res://TheInsatiable/scenes/insatiable_energy_counter.tscn",
                // 商店人物场景。
                MerchantAnimPath: "res://TheInsatiable/animations/the_insatiable_merchant.tscn"
                // 篝火休息场景（暂未自定义）。
                // RestSiteAnimPath: "res://scenes/rest_site/characters/ironclad_rest_site.tscn"
            ),
            Ui: new(
                // 人物头像路径。
                IconTexturePath: "res://TheInsatiable/characterui/character_icon_the_insatiable.png",
                // 人物头像路径外壳。
                IconOutlineTexturePath: "res://TheInsatiable/characterui/character_icon_the_insatiable_outline.png",
                // 游戏左上角头像、角色统计页头像、每日挑战角色头像。
                IconPath: "res://TheInsatiable/scenes/insatiable_icon.tscn",
                // 人物选择背景。
                CharacterSelectBgPath: "res://TheInsatiable/scenes/char_select_bg_insatiable.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: "res://TheInsatiable/characterui/char_select_the_insatiable.png",
                // 人物选择图标-锁定状态。
                CharacterSelectLockedIconPath: "res://TheInsatiable/characterui/char_select_the_insatiable_locked.png",
                // 人物选择过渡动画（暂未自定义）。
                // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
                // 地图上的角色标记图标、表情轮盘上的角色头像。
                MapMarkerPath: "res://TheInsatiable/characterui/map_marker_the_insatiable.png"
            ),
            Vfx: new(
                // 卡牌拖尾场景。
                TrailPath: "res://scenes/vfx/card_trail_regent.tscn"
            ),
            Audio: new(
                // 攻击音效
                AttackSfx: "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite",
                // 施法音效
                CastSfx: "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate",
                // 死亡音效
                DeathSfx: "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher",
                // 角色选择音效（暂未自定义）。
                // CharacterSelectSfx: null,
                // 过渡音效。
                CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
            ),
            Multiplayer: new(
                // 多人模式-手指。
                ArmPointingTexturePath: "res://TheInsatiable/characterui/multiplayer_hand_the_insatiable_point.png",
                // 多人模式剪刀石头布-石头。
                ArmRockTexturePath: "res://TheInsatiable/characterui/multiplayer_hand_the_insatiable_rock.png",
                // 多人模式剪刀石头布-布。
                ArmPaperTexturePath: "res://TheInsatiable/characterui/multiplayer_hand_the_insatiable_paper.png",
                // 多人模式剪刀石头布-剪刀。
                ArmScissorsTexturePath: "res://TheInsatiable/characterui/multiplayer_hand_the_insatiable_scissors.png"
            )
            // 其余如果有需要自行取消注释使用
            // Spine: null,
            // VisualCues: null,
            // WorldProceduralVisuals: null,
            // 以下为让遗物根据你的人物展现不同的图像资源，在列表里添加即可
            // VanillaCardVisualOverrides: [],
            // VanillaRelicVisualOverrides: [],
            // VanillaPotionVisualOverrides: []
        ));

    // 自动转换人物场景，让你不需要手动挂脚本。
    protected override NCreatureVisuals? TryCreateCreatureVisuals() =>
        RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);

    // 初始卡组
    protected override IEnumerable<CardModel> LocalStartingDeck => [
        ModelDb.Card<StrikeInsatiable>(),
        ModelDb.Card<StrikeInsatiable>(),
        ModelDb.Card<StrikeInsatiable>(),
        ModelDb.Card<StrikeInsatiable>(),
        ModelDb.Card<DefendInsatiable>(),
        ModelDb.Card<DefendInsatiable>(),
        ModelDb.Card<DefendInsatiable>(),
        ModelDb.Card<DefendInsatiable>(),
        ModelDb.Card<SandBlowing>(),
        ModelDb.Card<SandwormBite>(),
    ];

    // 初始遗物
    protected override IEnumerable<RelicModel> LocalStartingRelics => [
        ModelDb.Relic<DesertStoneRelic>(),
    ];

    private bool _hasLiquified;
    public bool HasNoEscapePower
    {
        get => _hasLiquified;
        set => _hasLiquified = value;
    }

    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx()
    {
        int num = 5;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount(list, num);
        Span<string> span = CollectionsMarshal.AsSpan(list);
        int num2 = 0;
        span[num2] = "vfx/vfx_attack_blunt";
        num2++;
        span[num2] = "vfx/vfx_heavy_blunt";
        num2++;
        span[num2] = "vfx/vfx_attack_slash";
        num2++;
        span[num2] = "vfx/vfx_scratch";
        num2++;
        span[num2] = "vfx/vfx_bite";
        return list;
    }

    protected override CreatureAnimator? SetupCustomCreatureAnimator(MegaSprite controller)
    {
        AnimState val1 = new AnimState("idle_loop", true);
        AnimState val6 = new AnimState("no_escape", true);
        AnimState val2 = new AnimState("cast", false);
        AnimState val2a = new AnimState("cast", false);
        AnimState val3 = new AnimState("attack", false);
        AnimState val3a = new AnimState("attack", false);
        AnimState val4 = new AnimState("hurt", false);
        AnimState val4a = new AnimState("hurt", false);
        AnimState val5 = new AnimState("die", false);
        AnimState val5a = new AnimState("die", false);
        AnimState val7 = new AnimState("eat_player", false);
        AnimState val7a = new AnimState("eat_player", false);
        AnimState val8 = new AnimState("salivate", false);
        AnimState val8a = new AnimState("salivate", false);
        AnimState val9 = new AnimState("attack_thrash", false);
        AnimState val9a = new AnimState("attack_thrash", false);
        val2.NextState = val1;
        val3.NextState = val1;
        val4.NextState = val1;
        val7.NextState = val1;
        val8.NextState = val1;
        val9.NextState = val1;
        val2a.NextState = val6;
        val3a.NextState = val6;
        val4a.NextState = val6;
        val7a.NextState = val6;
        val8a.NextState = val6;
        val9a.NextState = val6;
        CreatureAnimator valtotal = new CreatureAnimator(val1, controller);
        valtotal.AddAnyState("Idle", val1, () => !HasNoEscapePower);
        valtotal.AddAnyState("Idle", val6, () => HasNoEscapePower);
        valtotal.AddAnyState("Cast", val2, () => !HasNoEscapePower);
        valtotal.AddAnyState("Cast", val2a, () => HasNoEscapePower);
        valtotal.AddAnyState("Attack", val3, () => !HasNoEscapePower);
        valtotal.AddAnyState("Attack", val3a, () => HasNoEscapePower);
        valtotal.AddAnyState("Hit", val4, () => !HasNoEscapePower);
        valtotal.AddAnyState("Hit", val4a, () => HasNoEscapePower);
        valtotal.AddAnyState("Dead", val5, () => !HasNoEscapePower);
        valtotal.AddAnyState("Dead", val5a, () => HasNoEscapePower);
        valtotal.AddAnyState("NoEscape", val6, () => HasNoEscapePower);
        valtotal.AddAnyState("EatPlayer", val7, () => !HasNoEscapePower);
        valtotal.AddAnyState("EatPlayer", val7a, () => HasNoEscapePower);
        valtotal.AddAnyState("Salivate", val8, () => !HasNoEscapePower);
        valtotal.AddAnyState("Salivate", val8a, () => HasNoEscapePower);
        valtotal.AddAnyState("Thrash", val9, () => !HasNoEscapePower);
        valtotal.AddAnyState("Thrash", val9a, () => HasNoEscapePower);
        return valtotal;
    }
}