using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public GameObject HandObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }
	public float Damage { get; set; }
	public float Range { get; set; }

	public bool IsHolding { get; private set; }


	public Hand(GameObject handObject, Vector2 location, float maxHealth, float damage)
	{
		HandObject = handObject;
		Location = location;
		MaxHealth = maxHealth;
		Damage = damage;
	}
	public Hand(){}
}