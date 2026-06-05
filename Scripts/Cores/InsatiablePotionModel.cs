using BaseLib.Abstracts;

namespace TheInsatiable.Scripts;

public abstract class InsatiablePotionModel : CustomPotionModel, ITheInsatiableModel
{
    public override string? CustomPackedImagePath => $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png";
    public override string? CustomPackedOutlinePath => $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png";
}
