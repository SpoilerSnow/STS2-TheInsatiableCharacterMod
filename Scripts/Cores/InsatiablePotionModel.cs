using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts;

public abstract class InsatiablePotionModel : ModPotionTemplate, ITheInsatiableModel
{
    public override PotionAssetProfile AssetProfile => new(
        ImagePath: $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png",
        OutlinePath: $"res://TheInsatiable/images/potions/{GetType().Name.Replace("Potion", "")}.png"
    );
}
