using System.Collections.Generic;
using UnityEngine;

public class Organism
{
	[SerializeField] GameManager GM;
	public decimal NeededEnergy { get { return GetNeededEnergy(); } }
	private List<Organ> birthOrgans;
	public List<Organ> BirthOrgans 
	{
		get
		{
			return GM.CopyOrganList(birthOrgans);
		}
		set
		{
			birthOrgans = value;
		}
	}


	private decimal GetNeededEnergy()
	{
		decimal output = 0;
		foreach (Organ organ in BirthOrgans)
		{
			output += organ.NeededEnergy;
		}
		return output;
	}
	public Organism Copy()
	{
		return (Organism)MemberwiseClone();
	}
}
