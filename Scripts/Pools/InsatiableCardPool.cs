using BaseLib.Abstracts;
using Godot;

namespace TheInsatiable.Scripts;

public class InsatiableCardPool : CustomCardPoolModel
{
	public override string Title => "Insatiable";
	public override string? TextEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy.png";	
	public override string? BigEnergyIconPath => "res://TheInsatiable/images/ui/the_insatiable_energy_big.png";
	public override Color DeckEntryCardColor => new(255f/255f, 190f/255f, 106f/255f, 1f);
	public override Color ShaderColor => new(255f/255f, 190f/255f, 106f/255f, 1f);
	public override bool IsColorless => false;

}
