namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class MaxTurnsInPileVar : DynamicVar
{
	public const string defaultName = "MaxTurnsInPile";
	public MaxTurnsInPileVar(int maxTurnsInPile)
		: base("MaxTurnsInPile", maxTurnsInPile)
	{
	}
	public MaxTurnsInPileVar(string name, int maxTurnsInPile)
		: base(name, maxTurnsInPile)
	{
	}
}