namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class MaxCapacityVar : DynamicVar
{
	public const string defaultName = "MaxCapacity";
	public MaxCapacityVar(int maxCapacity)
		: base("MaxCapacity", maxCapacity)
	{
	}
	public MaxCapacityVar(string name, int maxCapacity)
		: base(name, maxCapacity)
	{
	}
}