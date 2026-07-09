using HarmonyLib;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using STS2RitsuLib.Utils;

namespace TheInsatiable.Scripts;

/// <summary>
/// 当 NCardHolder 持有的卡牌是 InsatiableCardModel 且有多张 CardHoverTip 时，
/// 只显示一张并定时轮流滚动，而非一次性全部展示。
/// </summary>
public static class InsatiableCardHoverTipCyclePatch
{
    /// <summary>卡片 hover tip 轮播切换间隔（秒），可由外部修改。</summary>
    public static float CycleInterval = 0.7f;

    /// <summary>基于弱引用的 AttachedState，自动跟随 NCardHolder 生命周期，不会阻止 GC。</summary>
    private static readonly AttachedState<NCardHolder, CycleData> _cycleState = new(() => null!);

    private sealed class CycleData
    {
        public required List<CardHoverTip> CardTips;
        public required List<IHoverTip> TextTips;
        public int CurrentIndex;
        public Godot.Timer? Timer;
    }

    /// <summary>清除当前 hover tips 并用指定索引的卡片 tip 重建。</summary>
    private static void RefreshTip(NCardHolder holder, CycleData data)
    {
        NHoverTipSet.Remove(holder);

        var tips = new List<IHoverTip>(data.TextTips) { data.CardTips[data.CurrentIndex] };
        var tipSet = NHoverTipSet.CreateAndShow(holder, tips);
        tipSet?.SetAlignmentForCardHolder(holder);
    }

    /// <summary>停止轮播，释放 Timer 并从 AttachedState 中移除。</summary>
    private static void StopCycle(NCardHolder holder)
    {
        var data = _cycleState.GetValueOrDefault(holder);
        if (data == null)
            return;

        data.Timer?.QueueFree();
        data.Timer = null;
        _cycleState.Set(holder, null!);
    }

    // ── Harmony Patches ──────────────────────────────────────────

    /// <summary>拦截 CreateHoverTips，对 StarCardModel 启动单张轮播。</summary>
    [HarmonyPatch(typeof(NCardHolder), "CreateHoverTips")]
    public static class Patch_CreateHoverTips
    {
        [HarmonyPrefix]
        public static bool Prefix(NCardHolder __instance)
        {
            if (__instance.CardNode?.Model is not InsatiableCardModel)
                return true;

            var allTips = __instance.CardNode.Model.HoverTips.ToList();
            var cardTips = allTips.OfType<CardHoverTip>().ToList();

            if (cardTips.Count <= 1)
                return true;

            var textTips = allTips.Where(t => t is not CardHoverTip).ToList();

            // 清理旧的轮播状态
            StopCycle(__instance);

            var data = new CycleData
            {
                CardTips = cardTips,
                TextTips = textTips,
                CurrentIndex = 0
            };

            // 显示第一张卡片 tip
            RefreshTip(__instance, data);

            // 创建定时器用于轮流切换
            var timer = new Godot.Timer
            {
                WaitTime = CycleInterval,
                OneShot = false
            };
            timer.Timeout += () =>
            {
                if (!__instance.IsInsideTree())
                    return;

                var d = _cycleState.GetValueOrDefault(__instance);
                if (d == null)
                    return;

                d.CurrentIndex = (d.CurrentIndex + 1) % d.CardTips.Count;
                RefreshTip(__instance, d);
            };
            __instance.AddChild(timer);
            timer.Start();
            data.Timer = timer;

            _cycleState.Set(__instance, data);

            // 跳过原始方法
            return false;
        }
    }

    /// <summary>拦截 ClearHoverTips，停止轮播并清理 Timer 资源。</summary>
    [HarmonyPatch(typeof(NCardHolder), "ClearHoverTips")]
    public static class Patch_ClearHoverTips
    {
        [HarmonyPrefix]
        public static void Prefix(NCardHolder __instance)
        {
            StopCycle(__instance);
        }
    }
}
