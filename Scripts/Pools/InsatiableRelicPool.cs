using STS2RitsuLib.Scaffolding.Content;

namespace TheInsatiable.Scripts.Pools;
public class InsatiableRelicPool : TypeListRelicPoolModel
{
	public override string EnergyColorName => "TheInsatiable";
    public override string? TextEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy.png";
    public override string? BigEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy_big.png";
}
