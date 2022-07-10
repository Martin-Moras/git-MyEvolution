using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	[SerializeField] private float CrackSpeed;
	public List<Organ> BirthOrgans { get; set; }
	private Atributes Atrib;

	private float crackTime;
	public Egg(List<Organ> birthOrgans)
	{
		BirthOrgans = birthOrgans;
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
		Atrib.Damage(9999);
		Atrib.CheckDeath();
	}
	private void CreateOrgans()
	{
		//Create brain
		Organ brainOrgan = BirthOrgans.Find(x => x.Name == "Brain");
		GameObject brainObject = GM.CreateGameobject(brainOrgan, null, ref Atrib.Energy, transform.position, transform.rotation);
		Brain brain = brainObject.GetComponent<Brain>();
		brain.BirthOrgans = BirthOrgans;

		foreach (Organ organ in BirthOrgans)
		{
			if (organ == brainOrgan) continue;

			GM.CreateGameobject(organ, brainObject.transform, ref Atrib.Energy, organ.LocalPos, Quaternion.Euler(0, 0, organ.LocalRot));
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
