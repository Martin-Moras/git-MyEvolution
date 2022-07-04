using System;
using UnityEngine;
using System.Collections.Generic;

public class Stomach : MonoBehaviour
{
	public GameObject StomachObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }

	[SerializeField] private GameManager GM;
	private decimal FoodStorageSize;
	private decimal StoredFood;
	private float DigestColdown;
	private float DigestTime;
	[SerializeField] string StoredFoodn;

	public Stomach(GameObject stomachObject, Vector2 location, float maxHealth)
	{
		StomachObject = stomachObject;
		Location = location;
		MaxHealth = maxHealth;
	}
	public Stomach(){}
	private void Start()
	{
		if (DigestColdown == 0) DigestColdown = .1f;
		if (FoodStorageSize == 0) FoodStorageSize = 5m;
	}
	private void Update()
	{
		GetFood();
		Digest();
		SetDigestTime();
		StoredFoodn = StoredFood.ToString();
	}
	private void GetFood()
	{
		decimal getFoodAmount = .1m;

		List<Organ> allMouths = transform.parent.GetComponent<Brain>().GetOrgans("Mouth");

		foreach (Organ organ in allMouths)
		{
			Mouth mouth = (Mouth)organ.OrganScript;

			decimal freeFoodStorage = FoodStorageSize - StoredFood;
			if (freeFoodStorage - getFoodAmount < 0) return;

			StoredFood += mouth.GetComponent<Mouth>().TakeFood(getFoodAmount);
		}
	}
	private void Digest()
	{
		if (DigestTime > 0) return;
		if (StoredFood <= 0) return;

		decimal digestAmount = .1m;

		if (StoredFood < 1) digestAmount = StoredFood;
		StoredFood -= digestAmount;

		transform.parent.GetComponent<Brain>().AddEnergy(digestAmount);
		
		DigestTime = DigestColdown;
	}
	private void SetDigestTime()
	{
		if (DigestTime <= 0) return;

		DigestTime -= Time.deltaTime;
	}
}
