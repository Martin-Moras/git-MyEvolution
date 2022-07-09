using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	[SerializeField] private float CrackSpeed;
	public List<Organ> BirthOrgans { get; set; }

	private float crackTime;
	public Egg(List<Organ> birthOrgans)
	{
		BirthOrgans = birthOrgans;
	}
	private void Start()
	{
		GM.AddRb(gameObject);
		crackTime = CrackSpeed;
	}
	private void Update()
	{
		SetSize();
		SetCrackTime();
	}
	private void SetCrackTime()
	{
		crackTime -= Time.deltaTime;
		if (crackTime <= 0) Crack();
	}
	private void Crack()
	{
		Brain brain = Instantiate(GM.Brain, transform.position, transform.rotation).GetComponent<Brain>();
		brain.name = "Brain";
		brain.BirthOrgans = BirthOrgans;
		Destroy(gameObject);
	}
	private void SetSize()
	{
		float currentScale = GameManager.Remap(0, CrackSpeed, 0, 1, crackTime);
		transform.localScale = Vector3.one * currentScale;
	}
}
