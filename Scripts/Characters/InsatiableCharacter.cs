using System.Runtime.InteropServices;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using Color = Godot.Color;

namespace TheInsatiable.Scripts;
public class InsatiableCharacter : PlaceholderCharacterModel
{
	// 角色名称颜色
	public override Color NameColor => new(188f/255f, 130f/255f, 54f/255f, 1f);
	// 能量图标轮廓颜色
	public override Color EnergyLabelOutlineColor => new(127f/255f, 86f/255f, 31f/255f, 1f);
	// 地图上角色标记颜色
	public override Color MapDrawingColor => new(188f/255f, 130f/255f, 54f/255f, 1f);
	// 对话框颜色
	public override Color DialogueColor => new(188f/255f, 130f/255f, 54f/255f, 1f);
	// 角色语音气泡颜色
	public override VfxColor SpeechBubbleColor => VfxColor.Orange;
	// 人物性别（男女中立）
	public override CharacterGender Gender => CharacterGender.Neutral;
	// 初始血量
	public override int StartingHp => 80;
	// 人物模型tscn路径。要自定义见下。
	public override string CustomVisualPath => "res://TheInsatiable/animations/the_insatiable_character.tscn";
	// 卡牌拖尾场景。
	public override string CustomTrailPath => "res://scenes/vfx/card_trail_regent.tscn";
	// 人物头像路径。
	public override string CustomIconTexturePath => "res://TheInsatiable/characterui/character_icon_the_insatiable.png";
	// 人物头像路径外壳。
	public override string CustomIconOutlineTexturePath => "res://TheInsatiable/characterui/character_icon_the_insatiable_outline.png";
	// 人物头像2号。
	public override string CustomIconPath => "res://TheInsatiable/scenes/insatiable_icon.tscn";
	// 能量表盘tscn路径。要自定义见下。
	public override string CustomEnergyCounterPath => "res://TheInsatiable/scenes/insatiable_energy_counter.tscn";
	// 篝火休息场景。
	// public override string CustomRestSiteAnimPath => "res://scenes/rest_site/characters/ironclad_rest_site.tscn";
	// 商店人物场景。
	// public override string CustomMerchantAnimPath => "res://scenes/merchant/characters/ironclad_merchant.tscn";
	// 多人模式-手指。
	// public override string CustomArmPointingTexturePath => null;
	// 多人模式剪刀石头布-石头。
	// public override string CustomArmRockTexturePath => null;
	// 多人模式剪刀石头布-布。
	// public override string CustomArmPaperTexturePath => null;
	// 多人模式剪刀石头布-剪刀。
	// public override string CustomArmScissorsTexturePath => null;

	// 人物选择背景。
	public override string CustomCharacterSelectBg => "res://TheInsatiable/scenes/char_select_bg_insatiable.tscn";
	// 人物选择图标。
	public override string CustomCharacterSelectIconPath => "res://TheInsatiable/characterui/char_select_the_insatiable.png";
	// 人物选择图标-锁定状态。
	public override string CustomCharacterSelectLockedIconPath => "res://TheInsatiable/characterui/char_select_the_insatiable_locked.png";
	// 人物选择过渡动画。
	// public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";
	// 地图上的角色标记图标、表情轮盘上的角色头像
	public override string CustomMapMarkerPath => "res://TheInsatiable/characterui/map_marker_the_insatiable.png";
	// 攻击音效
	public override string CustomAttackSfx => "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_lunging_bite";
	// 施法音效
	public override string CustomCastSfx => "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_salivate";
	// 死亡音效
	public override string CustomDeathSfx => "event:/sfx/enemy/enemy_attacks/the_insatiable/the_insatiable_finisher";
	// 角色选择音效
	// public override string CharacterSelectSfx => null;
	// 过渡音效。这个不能删。
	public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

	public override CardPoolModel CardPool => ModelDb.CardPool<InsatiableCardPool>();
	public override RelicPoolModel RelicPool => ModelDb.RelicPool<InsatiableRelicPool>();
	public override PotionPoolModel PotionPool => ModelDb.PotionPool<InsatiablePotionPool>();

	// 初始卡组
	public override IEnumerable<CardModel> StartingDeck => [
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
	public override IReadOnlyList<RelicModel> StartingRelics => [
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
	public override CreatureAnimator GenerateAnimator(MegaSprite controller)
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
