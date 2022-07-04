using UnityEngine;
using System;

public class Atributes : MonoBehaviour
{
	private decimal usedEnergy;
	public decimal UsedEnergie 
	{
		get { return usedEnergy; }
		private set
		{
			if (value < 0) print("EROR!!!!!!!!!!!!!!");
			CheckEnergy();
			usedEnergy = value;
		}
	}
	private float health;
	private float Health
	{
		get { return health; }
		set
		{
			if (value < 0) value = 0;
			health = value;
			if (value > 0) return;
			CheckDeath();
		}
	}
	private string StoredFoodn;
	[SerializeField] private GameManager GM;

	private bool isDead = false;

	private void Start()
	{
		if (UsedEnergie == 0) UsedEnergie = 5;
		if (Health == 0) Health = 2;
		CheckDeath();
	} 
	private void Update()
	{
		StoredFoodn = UsedEnergie.ToString();
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
			if (UsedEnergie < eatAmount)
			{
				eatAmount = UsedEnergie;
			}
			UsedEnergie -= eatAmount;
			CheckDeath();
			return eatAmount;
		}
	}
	public void Damage(float damageAmount)
	{
		if (Health <= 0) return;
		
		if (Health < damageAmount) damageAmount = Health;

		Health -= damageAmount;
		
		CheckDeath();
	}
	public void Heal(float healAmount)
	{
		Health += healAmount;
	}
	public void CheckDeath()
	{
		if (transform.parent == null && !transform.CompareTag("Brain") && Health != 0) Health = 0;
		if (Health > 0) return;
		if (CheckEnergy()) return;
		if (isDead) return;
		
		isDead = true;
		transform.DetachChildren();
		if (transform.parent != null) transform.SetParent(null);
		AddRigidbody();
		RemoveOrganScript();
		
		void AddRigidbody()
		{
			if (gameObject.GetComponent<Rigidbody2D>() != null) return;
			
			Rigidbody2D deadRb = gameObject.AddComponent<Rigidbody2D>();
			deadRb.drag = 2;
		}
		void RemoveOrganScript()
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
			Brain brain = GetComponent<Brain>();
			if (brain != null)
			{
				Destroy(brain);
				foreach (Organ child in brain.Organs)
				{
					child.GetComponent<Atributes>().CheckDeath();
				}
				return;
			}
		}
	}
	public bool CheckEnergy()
	{
		if (!isDead) return false;
		//if Energy = 0 destroy this gameobject
		if (UsedEnergie > 0) return false;
		
		Destroy(gameObject);
		return true;
	}
}