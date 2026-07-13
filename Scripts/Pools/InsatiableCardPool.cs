using Godot;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace TheInsatiable.Scripts.Pools;

public class InsatiableCardPool : TypeListCardPoolModel, IModColorfulPhilosophersCardPool
{
	public override string EnergyColorName => "TheInsatiable";
	public override string Title => "The Insatiable";
	public override string? TextEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy.png";
	public override string? BigEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy_big.png";
	public override Color DeckEntryCardColor => new(255f/255f, 190f/255f, 106f/255f, 1f);
	private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateRgbShaderMaterial(255f/255f, 190f/255f, 106f/255f);
	public override Material? PoolFrameMaterial => _poolFrameMaterial;
	public override bool IsColorless => false;
}
