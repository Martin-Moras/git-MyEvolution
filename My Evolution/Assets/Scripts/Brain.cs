using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*[Serializable]
public class Test
{
	int A { get; set; }
	int B { get; set; }
	int C { get; set; }
	int D { get; set; }
	public Test(int a, int b, int c, int d)
	{
		A = a;
		B = b;
		C = c;
		D = d;
	}
}*/

public class Brain : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public List<GameObject> BirthOrgans { get; set; }
	public List<GameObject> Organs { get; set; }
	private decimal AvailableEnergy;
	private decimal NeededBirthEnergy;
	private int ChiledCount;

	
	[SerializeField] private string AvailableEnergyn;

	private void Start()
	{
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
			if (currentOrgan.tag != "Organ") continue;
			BirthOrgans.Add(currentOrgan.gameObject);
		}
	}
	private void SetNeededBirthEnergy()
	{
		NeededBirthEnergy = 2;
	}
	private void CreateBody()
	{
		if (transform.childCount > 0) return;
		foreach (GameObject organ in BirthOrgans)
		{
			Instantiate(organ, transform);
		}
	}
	//Update
	private void ControlOrgans()
	{
		Controllfeet();
		ControllMouth();
		ControllEye();

		void Controllfeet()
		{
			float xAxis = Input.GetAxisRaw("Horizontal");
			float yAxis = Input.GetAxisRaw("Vertical");

			foreach (GameObject foot in GetOrgans("Foot"))
			{
				foot.GetComponent<Foot>().Walk(new Vector2(xAxis, yAxis));
				foot.GetComponent<Foot>().Turn(TurnDir());
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
				foreach(GameObject organ in GetOrgans("Mouth"))
				{
					organ.GetComponent<Mouth>().Bite();
				}
			}
		}
		void ControllEye()
		{
			foreach (GameObject bodyPart in GetOrgans("Eye"))
			{
				//print(bodyPart.GetComponent<Eye>().Look().Count);
			}
		}
	}
	private void UpdateOrgans()
	{
		Organs = new();
		foreach (Transform currentOrgan in transform)
		{
			if (currentOrgan.tag != "Organ") continue;
			Organs.Add(currentOrgan.gameObject);
		}
	}
	private void Birth()
	{
		if (AvailableEnergy < NeededBirthEnergy) return;
		AvailableEnergy -= NeededBirthEnergy;

		//Create chiledOrgan
		List<Vector2> freeOrganPlaces = FreeOrganPlaces();
		
		GameObject newOrgan = new();
		newOrgan = GM.AllOrgans[UnityEngine.Random.Range(0, GM.AllOrgans.Count - 1)];
		newOrgan.transform.localPosition = freeOrganPlaces[UnityEngine.Random.Range(0, freeOrganPlaces.Count - 1)];
		newOrgan.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 3) * 90);
		//Create chiled
		Brain chiledBrain = Instantiate(GM.Brain, transform.position, transform.rotation).GetComponent<Brain>();
		chiledBrain.BirthOrgans = Clone(BirthOrgans);
		chiledBrain.BirthOrgans.Add(newOrgan);

		ChiledCount++;

		List<Vector2> FreeOrganPlaces()
		{
			List<Vector2> output = new();
			List<GameObject> myOrgansAndBrain = new();
			myOrgansAndBrain.Add(gameObject);
			myOrgansAndBrain = Organs;
			List<Vector2> organPos = OrganPos();

			foreach (GameObject organ in myOrgansAndBrain)
			{
				Vector2 currentOrganPos = organ.transform.localPosition;
				Vector2[] checkDir = new Vector2[]
				{
					new Vector2(.1f, 0f),
					new Vector2(-.1f, 0f),
					new Vector2(0f, .1f),
					new Vector2(0f, -.1f)
				};

				foreach (Vector2 dir in checkDir)
				{
					Vector2 pos = currentOrganPos + dir;

					if (IfFree(organPos, output, pos)) output.Add(pos);
				}

				bool IfFree(List<Vector2> organPos, List<Vector2> output, Vector2 pos)
				{
					if (output.Exists(x => x == pos)) return false;
					if (organPos.Exists(x => x == pos)) return false;
					return true;
				}
			}
			return output;

			List<Vector2> OrganPos()
			{
				List<Vector2> output = new();
				foreach (GameObject organ in myOrgansAndBrain)
				{
					output.Add(organ.transform.localPosition);
				}
				return output;
			}
		}
	}
	//Funktions
	public void AddEnergy(decimal amount)
	{
		AvailableEnergy += amount;
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
	public List<GameObject> Clone(List<GameObject> input)
	{
		List<GameObject> output = new();

		/*Test newTest = new Test(0, 1, 2, 3);

		Test test = DeepCopy(newTest);
		List<GameObject> output = new();
*/


		foreach (GameObject current in input)
		{
			//output.Add(DeepCopy(current));
			GameObject organ = Instantiate(current);
			output.Add(organ);
			Destroy(organ);
		}
		return output;

		static T DeepCopy<T>(T other)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, other);
				ms.Position = 0;
				return (T)formatter.Deserialize(ms);
			}
		}
	}
}