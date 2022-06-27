using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GM", menuName = "Game Manager")]
public class GameManager : ScriptableObject
{
	[SerializeField] public GameObject Ear;
	[SerializeField] public GameObject Eye;
	[SerializeField] public GameObject Foot;
	[SerializeField] public GameObject Hand;
	[SerializeField] public GameObject Mouth;
	[SerializeField] public GameObject Shield;
	[SerializeField] public GameObject Stomach;
	[SerializeField] public GameObject Brain;

	public List<GameObject> AllOrgans
	{
		get
		{
			return new()
			{
				Ear,
				Eye,
				Foot,
				Hand,
				Mouth,
				Shield,
				Stomach,
			};
		}
	}
	public List<object> AllOrganScripts
	{
		get
		{
			return new()
			{
				new Ear(),
				new Eye(),
				new Foot(),
				new Hand(),
				new Mouth(),
				new Shield(),
				new Stomach(),
				new Brain()
			};
		}
	}
	public List<string> AllOrganNames
	{
		get
		{
			return new()
			{
				"Ear",
				"Eye",
				"Foot",
				"Hand",
				"Mouth",
				"Shield",
				"Stomach",
				"Brain"
			};
		}
	}
}