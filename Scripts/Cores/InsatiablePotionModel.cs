using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;

public abstract class InsatiablePotionModel : ModPotionTemplate, ITheInsatiableModel
{
    public override string? CustomImagePath => $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png";
    public override string? CustomOutlinePath => $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png";
}
