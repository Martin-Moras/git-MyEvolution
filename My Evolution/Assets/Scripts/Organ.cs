using UnityEngine;
[System.Serializable]
public class Organ
{
	[SerializeField] private GameManager GM;
	
	public string Name { get; set; }
	public decimal Level { get { return 1; } set { } }
	public decimal Energy { get; set; }
	public Vector2 LocalPos { get; set; }
	public int LocalRot { get; set; }

	public Organ(string name, Vector2 localPos, int localRot, decimal level, GameManager gm, decimal energy)
	{
		Name = name;
		LocalPos = localPos;
		LocalRot = localRot;
		Level = level;
		GM = gm;
		Energy = energy;
	}
	public Organ() { }
	public Organ Clone()
	{
		return (Organ)MemberwiseClone();
	}
}