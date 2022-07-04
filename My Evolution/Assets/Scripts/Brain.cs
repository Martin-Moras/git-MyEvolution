using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Brain : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public List<Organ> BirthOrgans { get; set; }
	private List<Organ> organs;
	public List<Organ> Organs 
	{
		get
		{
			return organs;
		}
		set
		{
			organs = value;
		}
	}
	private decimal AvailableEnergy;
	private decimal NeededBirthEnergy;
	private int ChiledCount;

	
	[SerializeField] private string AvailableEnergyn;

	private void Start()
	{
		var d = new Organ("", Vector2.zero, 0, 0, new Brain(), new Atributes());
		SetBirthOrgans();
		SetNeededBirthEnergy();
		CreateBody();
		UpdateOrgans();
	}
	private void Update()
	{
		UpdateOrgans();
		ControlOrgans();
		Birth();
		AvailableEnergyn = AvailableEnergy.ToString();
	}
	//Start
	private void SetBirthOrgans()
	{
		if (BirthOrgans != null) return;

		BirthOrgans = new();
		foreach (Transform currentOrgan in transform)
		{
			if (!currentOrgan.CompareTag("Organ")) continue;
			BirthOrgans.Add(GM.ToOrgan(currentOrgan.gameObject));
		}
	}
	private void SetNeededBirthEnergy()
	{
		NeededBirthEnergy = 2;
	}
	private void CreateBody()
	{
		if (transform.childCount > 0) return;
		foreach (Organ organ in BirthOrgans)
		{
			Instantiate(GM.ToGameObject(organ), transform);
		}
	}
	//Update
	private void UpdateOrgans()
	{
		Organs = new();
		foreach (Transform currentOrgan in transform)
		{
			if (!currentOrgan.CompareTag("Organ")) continue;
			var d = GM.ToOrgan(currentOrgan.gameObject);
			GameObject gameObject = d.gameObject;
			Organs.Add();
		}
	}
	private void ControlOrgans()
	{
		Controllfeet();
		ControllMouth();
		ControllEye();

		void Controllfeet()
		{
			float xAxis = Input.GetAxisRaw("Horizontal");
			float yAxis = Input.GetAxisRaw("Vertical");

			foreach (Organ organ in GetOrgans("Foot"))
			{
				Foot foot1 = (Foot)organ.OrganScript;
				foot1.Walk(new Vector2(xAxis, yAxis));
				foot1.Turn(TurnDir());
			}
			int TurnDir()
			{
				if (Input.GetKey(KeyCode.Alpha1)) return 1;
				if (Input.GetKey(KeyCode.Alpha2)) return -1;
				return 0;
			}
		}
		void ControllMouth()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				foreach(Organ organ in GetOrgans("Mouth"))
				{
					Mouth mouth = (Mouth)organ.OrganScript;
					mouth.Bite();
				}
			}
		}
		void ControllEye()
		{
			foreach (Organ bodyPart in GetOrgans("Eye"))
			{
				//print(bodyPart.GetComponent<Eye>().Look().Count);
			}
		}
	}
	private void Birth()
	{
		if (AvailableEnergy < NeededBirthEnergy) return;
		AvailableEnergy -= NeededBirthEnergy;

		List<Vector2> freeOrganPlaces = FreeOrganPlaces();

		//Create chiled
		Brain chiledBrain = Instantiate(GM.Brain, transform.position, transform.rotation).GetComponent<Brain>();
		List<Organ> newOrganList = GM.CopyOrganList(BirthOrgans);
		newOrganList = AddRemoveOrgan(newOrganList);

		chiledBrain.BirthOrgans = newOrganList;


		ChiledCount++;

		List<Organ> AddRemoveOrgan(List<Organ> inputList)
		{
			inputList.Add(AddOrgan());
			return inputList;
			
			Organ AddOrgan()
			{
				string name = GM.AllOrganNames[UnityEngine.Random.Range(0, GM.AllOrganNames.Count - 1)];
				Vector2 LocalPos = freeOrganPlaces[UnityEngine.Random.Range(0, freeOrganPlaces.Count - 1)];
				int LocalRot = UnityEngine.Random.Range(0, 3) * 90;
				object OrganScript = GM.AllOrganScriptsDict[name];
				Organ newOrgan = new Organ(name, LocalPos, LocalRot, 0, OrganScript, new Atributes());
	
				return newOrgan;
			}
		}

		
		List<Vector2> FreeOrganPlaces()
		{
			List<Vector2> output = new();
			List<Vector2> organPosList = GetPos(Organs);

			foreach (Vector2 organPos in organPosList)
			{
				CheckPos(organPos);
			}
			return output;

			void CheckPos(Vector2 organPos)
			{
				Vector2[] checkDir = new Vector2[]
				{
					new Vector2(.1f, 0f),
					new Vector2(-.1f, 0f),
					new Vector2(0f, .1f),
					new Vector2(0f, -.1f)
				};

				foreach (Vector2 dir in checkDir)
				{
					Vector2 pos = organPos + dir;

					CheckDir(pos);
				}
				void CheckDir(Vector2 pos)
				{
					if (IsFree(organPosList, output, pos)) output.Add(pos);
					
					bool IsFree(List<Vector2> organPos, List<Vector2> output, Vector2 pos)
					{
						if (output.Exists(x => x == pos)) return false;
						if (organPos.Exists(x => x == pos)) return false;
						return true;
					}
				}
			}

			List<Vector2> GetPos(List<Organ> input)
			{
				List<Vector2> output = new();
				foreach (Organ organ in input)
				{
					output.Add(organ.LocalPos);
				}
				output.Add(new Vector2(0, 0));
				return output;
			}
		}
	}
	//Funktions
	public void AddEnergy(decimal amount)
	{
		AvailableEnergy += amount;
	}
	public List<Organ> GetOrgans(string organName)
	{
		List<Organ> output = new();

		foreach (Organ organ in Organs)
		{
			if (!organ.Name.Contains(organName)) continue;

			output.Add(organ);
		}
		return output;
	}
}