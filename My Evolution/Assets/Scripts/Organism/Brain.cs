using System;
using UnityEngine;
using System.Collections.Generic;

public class Brain : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	private Atributes Atrib { get { return GetComponent<Atributes>(); } }

	[SerializeField] private float GrowSpeed;
	[SerializeField] private string AvailableEnergyn;

	public Organism BirthOrganism { get; set; }
	public List<GameObject> Organs { get; set; }
	private Organism ChiledOrganism { get; set; }

	private decimal NeededBirthEnergy;
	private int ChiledCount;
	private float growTime;


	private void Start()
	{
		growTime = GrowSpeed;
		SetBirthOrgans();
		SetChiledOrgans();
		//CreateBody();
		UpdateOrgans();
	}
	private void Update()
	{
		UpdateOrgans();
		Grow();
		ControlOrgans();
		Birth();
		AvailableEnergyn = Atrib.Energy.ToString();
	}
	//Start
	private void InitialiseNeuralNetwork()
	{

	}
	private void SetBirthOrgans()
	{
		List<Organ> chiledOrgans = ChiledOrganism.BirthOrgans;
		if (BirthOrganism != null) return;

		BirthOrganism = new();
		foreach (Transform currOrgan in transform)
		{
			if (!currOrgan.CompareTag("Organ")) continue;
			chiledOrgans.Add(GM.ToOrgan(currOrgan.gameObject));
		}
		chiledOrgans.Add(GM.ToOrgan(gameObject));
	}
	private void CreateBody()
	{
		if (transform.childCount > 0) return;
		foreach (Organ organ in BirthOrganism.BirthOrgans)
		{
			if (organ.Name != "Brain") continue;
			GM.ToGameObject(organ, transform);
		}
	}
	//Update
	private void Grow()
	{
		if (growTime <= 0) return;
		SetGrowTime();
		float currentScale = GameManager.Remap(0, GrowSpeed, 0, 1, growTime);
		transform.localScale = Vector3.one * currentScale;
		if (growTime <= 0)
		{
			transform.localScale = Vector3.one;
			growTime = 0;
		}

		void SetGrowTime()
		{
			growTime -= Time.deltaTime;
		}
	}
	private void UpdateOrgans()
	{
		Organs = new();
		foreach (Transform organ in transform)
		{
			if (!organ.CompareTag("Organ")) continue;
			Organs.Add(organ.gameObject);
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

			foreach (GameObject organ in GetOrgans("Foot"))
			{
				Foot foot1 = organ.GetComponent<Foot>();
				foot1.Walk(new Vector2(xAxis, yAxis));
				foot1.Turn(TurnDir());
			}
			int TurnDir()
			{
				bool onePressed = Input.GetKey(KeyCode.Alpha1);
				bool twoPressed = Input.GetKey(KeyCode.Alpha2);

				if (onePressed && !twoPressed) return 1;
				if (twoPressed && !onePressed) return -1;
				return 0;
			}
		}
		void ControllMouth()
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				foreach(GameObject organ in GetOrgans("Mouth"))
				{
					Mouth mouth = organ.GetComponent<Mouth>();
					mouth.Bite();
				}
			}
		}
		void ControllEye()
		{
			foreach (GameObject organ in GetOrgans("Eye"))
			{
				//print(organ.GetComponent<Eye>().Look().Count);
			}
		}
	}
	private void Birth()
	{
		if (Atrib.Energy < NeededBirthEnergy) return;

		//Create chiled
		Egg egg = Instantiate(GM.Egg, transform.position, transform.rotation).GetComponent<Egg>();
		egg.name = "Egg";
		egg.BirthOrganism = ChiledOrganism;
		egg.GiveEnergy(ref Atrib.Energy, NeededBirthEnergy);

		ChiledCount++;
		SetChiledOrgans();
	}
	private void SetChiledOrgans()
	{
		List<Organ> chiledOrgans = ChiledOrganism.BirthOrgans;

		chiledOrgans = GM.CopyOrganList(BirthOrganism.BirthOrgans);
		List<Vector2> freeOrganPlaces = FreeOrganPlaces();
		AddRemoveOrgan();
		SetNeededBirthEnergy(); 
		
		void SetNeededBirthEnergy()
		{
			NeededBirthEnergy = 0;
			foreach (Organ organ in chiledOrgans)
			{
				NeededBirthEnergy += (decimal)GM.NeededEnergyDict[organ.Name] * organ.Level;
			}
		}
		void AddRemoveOrgan()
		{
			chiledOrgans.Add(AddOrgan());

			Organ AddOrgan()
			{
				string name = GM.AllOrganNames[UnityEngine.Random.Range(0, GM.AllOrganNames.Count - 1)];
				Vector2 LocalPos = freeOrganPlaces[UnityEngine.Random.Range(0, freeOrganPlaces.Count - 1)];
				int LocalRot = UnityEngine.Random.Range(0, 3) * 90;
				Organ newOrgan = new(name, LocalPos, LocalRot, 0, GM, 0);

				return newOrgan;
			}
		}
		List<Vector2> FreeOrganPlaces()
		{
			List<Vector2> output = new();
			List<Vector2> organPosList = GetPos(chiledOrgans);

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
				return output;
			}
		}
	}
	public void GiveEnergy(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (sorce <= 0) return;
		if (sorce < amount) amount = sorce;
		Atrib.Energy += amount;
		sorce -= amount;
	}
	public List<GameObject> GetOrgans(string organName)
	{
		List<GameObject> output = new();

		foreach (GameObject organ in Organs)
		{
			if (!organ.name.Contains(organName)) continue;

			output.Add(organ);
		}
		return output;
	}
}