using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ear : MonoBehaviour
{
	public GameObject EarObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }
	public float ConsumedEnergy { get; set; }

	public Ear(GameObject earObject, Vector2 location, float maxHealth, float consumedEnergy)
	{
		EarObject = earObject;
		Location = location;
		MaxHealth = maxHealth;
		ConsumedEnergy = consumedEnergy;
	}
	public Ear() { }
}