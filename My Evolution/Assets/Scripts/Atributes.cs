using UnityEngine;
using System;

public class Atributes : MonoBehaviour
{
	public decimal UsedEnergie;
	private float Health;
	private string StoredFoodn;
	[SerializeField] private GameManager GM;

	private bool isDead = false;

	private void Start()
	{
		if (UsedEnergie == 0) UsedEnergie = 5;
		if (Health == 0) Health = 2;
		CheckForDeath();
	} 
	private void Update()
	{
		StoredFoodn = UsedEnergie.ToString();
	}

	public void Damage(float damageAmount)
	{
		if (Health <= 0) return;
		
		if (Health < damageAmount) damageAmount = Health;

		Health -= damageAmount;
		
		CheckForDeath();
	}
	public decimal DamageAndEat(float damageAmount)
	{
		if (Health <= 0)
			return Eat(Convert.ToDecimal(damageAmount));
		
		if (Health < damageAmount)
		{
			decimal eatAmount = Convert.ToDecimal(damageAmount - Health);
			damageAmount = Health;
			Damage(damageAmount);
			return Eat(eatAmount);
		}
		Damage(damageAmount);
		return 0;
		decimal Eat(decimal eatAmount)
		{
			CheckForDeath();
			if (UsedEnergie < eatAmount)
			{
				eatAmount = UsedEnergie;
			}
			UsedEnergie -= eatAmount;
			return eatAmount;
		}
	}
	public void Heal(float healAmount)
	{
		Health += healAmount;
	}
	public void CheckForDeath()
	{
		if (isDead) return;
		if (Health <= 0)
		{
			transform.DetachChildren();
			isDead = true;
		}
		else return;
		if (NoBrain()) AddRigidbody();
		if (transform.parent != null) transform.SetParent(null);

		if (TryDestroyThis()) return;
		RemoveOrganScript();

		bool TryDestroyThis()
		{
			transform.DetachChildren();
			//if Energy = 0 destroy this gameobject
			if (UsedEnergie > 0) return false;
			Destroy(gameObject);
			return true;
		}
		void AddRigidbody()
		{
			if (gameObject.GetComponent<Rigidbody2D>() != null) return;
			
			Rigidbody2D deadRb = gameObject.AddComponent<Rigidbody2D>();
			deadRb.drag = 2;
		}
		void RemoveOrganScript()
		{
			{ 
			Ear Ear = GetComponent<Ear>();
			if (Ear != null)
			{
				Destroy(Ear);
				return;
			}
			Eye Eye = GetComponent<Eye>();
			if (Eye != null)
			{
				Destroy(Eye);
				return;
			}
			Foot Foot = GetComponent<Foot>();
			if (Foot != null)
			{
				Destroy(Foot);
				return;
			}
			Hand Hand = GetComponent<Hand>();
			if (Hand != null)
			{
				Destroy(Hand);
				return;
			}
			Mouth Mouth = GetComponent<Mouth>();
			if (Mouth != null)
			{
				Destroy(Mouth);
				return;
			}
			Shield Shield = GetComponent<Shield>();
			if (Shield != null)
			{
				Destroy(Shield);
				return;
			}
			Stomach Stomach = GetComponent<Stomach>();
			if (Stomach != null)
			{
				Destroy(Stomach);
				return;
			}
			}
			Brain brain = GetComponent<Brain>();
			if (brain != null)
			{
				Destroy(brain);
				foreach (GameObject child in brain.Organs)
				{
					child.GetComponent<Atributes>().CheckForDeath();
				}
				return;
			}
		}
		bool NoBrain()
		{
			foreach(Transform trans in transform)
			{
				if (trans.GetComponent<Brain>() != null) 
					return true;
			}
			return false;
		}
	}
}