using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using STS2RitsuLib.Interop.AutoRegistration;
using TheInsatiable.Scripts.Piles;

namespace TheInsatiable.Scripts.Patches;

/// <summary>
/// SmartFormat 变量源：为 static_hover_tips.json 中吞噬堆描述的
/// {MaxCapacity:diff()} 和 {MaxTurnsInPile:diff()} 占位符提供实时值。
///
/// 每次 SmartFormat 解析这些选择器时，都会从 SwallowPile 的静态字段读取当前值，
/// 确保遗物/卡牌修改容量或回合上限后 hover tip 自动反映最新数值。
/// </summary>
[RegisterSmartFormatSource]
public class SwallowPileVarSource : ISource
{
    public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
    {
        switch (selectorInfo.SelectorText)
        {
            case MaxCapacityVar.defaultName:
                selectorInfo.Result = new MaxCapacityVar(SwallowPile.MaxCapacity);
                return true;
            case MaxTurnsInPileVar.defaultName:
                selectorInfo.Result = new MaxTurnsInPileVar(SwallowPile.MaxTurnsInPile);
                return true;
            default:
                return false;
        }
    }
}
