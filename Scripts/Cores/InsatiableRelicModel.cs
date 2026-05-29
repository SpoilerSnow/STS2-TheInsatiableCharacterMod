using BaseLib.Abstracts;

namespace TheInsatiable.Scripts;

public abstract class InsatiableRelicModel : CustomRelicModel
{
    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://TheInsatiable/images/relics/{GetType().Name.Replace("Relic", "")}.png";
    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://TheInsatiable/images/relics/{GetType().Name.Replace("Relic", "")}.png";
    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://TheInsatiable/images/relics/big/{GetType().Name.Replace("Relic", "")}.png";
}