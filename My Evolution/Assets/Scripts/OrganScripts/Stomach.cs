using UnityEngine;
using System.Collections.Generic;

public class Stomach : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	private decimal FoodStorageSize;
	private Atributes Atrib { get { return GetComponent<Atributes>(); } }
	private float DigestColdown;
	private float DigestTime;
	[SerializeField] string StoredFoodn;

	public Stomach() { }

	private void Start()
	{
		if (DigestColdown == 0) DigestColdown = .1f;
		if (FoodStorageSize == 0) FoodStorageSize = 5m;
	}
	private void Update()
	{
		GetFood();
		Digest();
		StoredFoodn = Atrib.Energy.ToString();
	}
	private void GetFood()
	{

		List<GameObject> allMouths = transform.parent.GetComponent<Brain>().GetOrgans("Mouth");

		foreach (GameObject organ in allMouths)
		{
			Mouth mouth = organ.GetComponent<Mouth>();
			if (mouth == null) continue;

			decimal amount = .1m;
			mouth.TakeFood(ref Atrib.Energy, amount);
		}
	}
	private void Digest()
	{
		SetDigestTime();
		if (DigestTime > 0) return;
		if (Atrib.Energy <= 0) return;

		DigestTime = DigestColdown;
		decimal digestAmount = .1m;
		transform.parent.GetComponent<Brain>().GiveEnergy(ref Atrib.Energy, digestAmount);
	}
	private void SetDigestTime()
	{
		if (DigestTime <= 0) return;

		DigestTime -= Time.deltaTime;
	}
}
