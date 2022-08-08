using UnityEngine;
[System.Serializable]
public class Organ
{
	[SerializeField] private GameManager GM;
	public string Name { get; set; }
	public decimal Level { get { return 1; } set { } }
	public decimal UsedEnergy { get; set; }
	public Vector2 LocalPos { get; set; }
	public int LocalRot { get; set; }
	public decimal NeededEnergy 
	{
		get { return (decimal)GM.NeededEnergyDict[Name] * Level; } 
	}

	public Organ(string name, Vector2 localPos, int localRot, decimal level, GameManager gm, decimal usedEnergy)
	{
		Name = name;
		LocalPos = localPos;
		LocalRot = localRot;
		Level = level;
		GM = gm;
		UsedEnergy = usedEnergy;
	}
	public Organ() { }
	public Organ Copy()
	{
		return (Organ)MemberwiseClone();
	}
	public void SetUsedEnergy(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (sorce <= 0) return;
		if (sorce < amount) amount = sorce;
		UsedEnergy += amount;
		sorce -= amount;
	}
}