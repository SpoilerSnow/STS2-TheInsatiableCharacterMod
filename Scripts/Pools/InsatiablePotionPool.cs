using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts.Pools;
public class InsatiablePotionPool : TypeListPotionPoolModel
{
	public override string EnergyColorName => "TheInsatiable";
    public override Color LabOutlineColor => new(255f/255f, 190f/255f, 106f/255f, 1f);
    public override string? TextEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy.png";
    public override string? BigEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy_big.png";
}
