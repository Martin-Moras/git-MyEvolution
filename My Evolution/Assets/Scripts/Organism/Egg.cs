using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	[SerializeField] private float CrackSpeed;
	public Organism BirthOrganism { get; set; }
	private Atributes Atrib;

	private float crackTime;
	public Egg(List<Organ> birthOrgans)
	{
		BirthOrganism.BirthOrgans = birthOrgans;
	}
	private void Start()
	{
		Atrib = GetComponent<Atributes>();
		GM.AddRb(gameObject);
		crackTime = CrackSpeed;
	}
	private void Update()
	{
		SetCrackTime();
		SetSize();
	}
	private void SetCrackTime()
	{
		crackTime -= Time.deltaTime;
		if (crackTime <= 0) Crack();
	}
	private void Crack()
	{
		CreateOrgans();

		//Destroy(gameObject);
		Atrib.Kill();
	}
	private void CreateOrgans()
	{
		Organ brainOrgan;
		GameObject brainObject;
		Brain brainScript;

		void CreateOrganism()
		{


			brainOrgan = BirthOrganism.BirthOrgans.Find(x => x.Name == "Brain");
			brainObject = GM.CreateGameobject(brainOrgan, null, ref Atrib.Energy, transform.position, transform.rotation);
			brainScript = brainObject.GetComponent<Brain>();
			brainScript.BirthOrganism = BirthOrganism;
		}
		void CreateOrgans()
		{
			foreach (Organ organ in BirthOrganism.BirthOrgans)
			{
				if (organ == brainOrgan) continue;

				GM.CreateGameobject(organ, brainObject.transform, ref Atrib.Energy, organ.LocalPos, Quaternion.Euler(0, 0, organ.LocalRot));
			}
		}
		
	}
	private void SetSize()
	{
		float currentScale = GameManager.Remap(0, CrackSpeed, 0, 1, crackTime);
		transform.localScale = Vector3.one * currentScale;
	}
	public void GiveEnergy(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (sorce <= 0) return;
		if (sorce < amount) amount = sorce;
		GetComponent<Atributes>().Energy += amount;
		sorce -= amount;
	}
}
