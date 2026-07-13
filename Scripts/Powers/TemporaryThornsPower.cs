using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Combat.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace TheInsatiable.Scripts.Powers;

// 注册power并设置Inherit = true，使得继承这个类的power自动被注册
[RegisterPower(Inherit = true)]
public abstract class TemporaryThornsPower<T> : ModTemporaryAppliedPowerTemplate<T, ThornsPower> where T : AbstractModel
{   
	public virtual bool HasCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/powers/{GetType().Name.Replace("Power", "")}.png");
    public virtual bool HasBigCustomPortrait => ResourceLoader.Exists($"res://TheInsatiable/images/powers/big/{GetType().Name.Replace("Power", "")}.png");
    public override string? CustomIconPath => HasCustomPortrait ? ($"res://TheInsatiable/images/powers/{GetType().Name.Replace("Power", "")}.png") : ($"res://TheInsatiable/images/powers/the_insatiable_power.png");
	public override string? CustomBigIconPath => HasCustomPortrait ? ($"res://TheInsatiable/images/powers/big/{GetType().Name.Replace("Power", "")}.png") : ($"res://TheInsatiable/images/powers/big/the_insatiable_power.png");
    protected override bool IsPositive => true; // 正面效果还是负面
    protected override bool UntilEndOfOtherSideTurn => true; // 为 true 时，在另一方回合结束时过期；否则在拥有者一方回合结束时过期。
    protected override int LastForXExtraTurns => 0; // 额外持续回合数

    // 推荐重载描述，以达到多个power共享一条文本的效果
    // 例如这里的文本需要在powers.json中写"TEST_POWER_TEMP_POWER.description"和"TEST_POWER_TEMP_POWER_DOWN.description"
    public override LocString Description => new("powers", IsPositive ? "THE_INSATIABLE_POWER_TEMPORARY_THORNS_POWER.description" : "THE_INSATIABLE_POWER_TEMPORARY_THORNS_POWER_DOWN.description");
	protected override string SmartDescriptionLocKey
	{
		get
		{
			if (!IsPositive)
			{
				return "THE_INSATIABLE_POWER_TEMPORARY_THORNS_POWER_DOWN.smartDescription";
			}
			return "THE_INSATIABLE_POWER_TEMPORARY_THORNS_POWER.smartDescription";
		}
	}
}
