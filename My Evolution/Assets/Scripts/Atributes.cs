using UnityEngine;
using System;

public class Atributes : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public UnityEngine.Object OrganScript;
	public decimal Level = 1;
	private decimal usedEnergy;
	public decimal UsedEnergy 
	{
		get { return usedEnergy; }
		set
		{
			if (value < 0) print("EROR!!!!!!!!!!!!!!");
			usedEnergy = value;
			CheckEnergy();
		}
	}
	public decimal Energy;
	private float health;
	private float Health
	{
		get { return health; }
		set
		{
			if (value < 0) value = 0;
			if (value == health) return;

			health = value;
			if (value > 0) return;
			CheckDeath();
		}
	}
	public decimal NeededEnergy 
	{ 
		get
		{
			GM.NeededEnergyDict.TryGetValue(gameObject.name, out float neededEnergy);
			return (decimal)neededEnergy;
		} 
	}
	private string UsedEnergyS;

	private bool isDead = false;

	private void Start()
	{
		//EnergyCounters = GameObject.Find("Energy Counter");
		SetAtributes();
		if (gameObject.TryGetComponent(out Brain b)) GM.AddRb(gameObject);
		CheckDeath();
	} 
	private void Update()
	{
		UsedEnergyS = UsedEnergy.ToString();
		//EnergyCounters.GetComponent<EnergyCounter>().AllEnergy += Energy;
		//EnergyCounters.GetComponent<EnergyCounter>().AllUsedEnergy += UsedEnergy;

	}

	private void SetAtributes()
	{
		if (CompareTag("Food")) UsedEnergy = 5;
		if (Health == 0) Health = 2;
	}
	public void DamageAndEat(ref decimal sorce, decimal amount)
	{
		float amountF = Convert.ToSingle(amount);
		if (Health <= 0) GetFoot(ref sorce, amount);
		
		if (Health < amountF)
		{
			decimal eatAmount = amount - Convert.ToDecimal(Health);
			amountF = Health;
			Damage(amountF);
			GetFoot(ref sorce, eatAmount);
		}
		Damage(amountF);
		
		void GetFoot(ref decimal sorce, decimal eatAmount)
		{
			if (eatAmount <= 0) return;
			if (UsedEnergy < eatAmount) eatAmount = UsedEnergy;
			
			UsedEnergy -= eatAmount;
			sorce += eatAmount;
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
		if (healAmount <= 0) return;
		Health += healAmount;
	}
	public void SetUsedEnergy(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (sorce <= 0) return;
		if (sorce < amount) amount = sorce;
		UsedEnergy += amount;
		sorce -= amount;
	}
	public void GiveOrganEnergy(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (sorce <= 0) return;
		if (sorce < amount) amount = sorce;
		Energy += amount;
		sorce -= amount;
	}
	public void TakeFood(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (Energy < amount) amount = Energy;

		Energy -= amount;
		sorce += amount;
	}
	public void CheckDeath()
	{
		if (transform.parent == null && !transform.CompareTag("Brain") && !transform.CompareTag("Egg")) Health = 0;
		if (Health > 0) return;
		if (CheckEnergy()) return;
		if (isDead) return;
		
		isDead = true;
		transform.DetachChildren();
		if (transform.parent != null) transform.SetParent(null);
		GM.AddRb(gameObject);
		UsedEnergy += Energy;
		Energy = 0;
		RemoveOrganScript();
		
		void RemoveOrganScript()
		{
			if (OrganScript == null) return;
			if (OrganScript.name == "Brain")
			{
				Brain brain = (Brain)OrganScript;
				foreach (GameObject child in brain.Organs)
				{
					child.GetComponent<Atributes>().CheckDeath();
				}
			}
			Destroy(OrganScript);
		}
	}
	public bool CheckEnergy()
	{
		if (!isDead) return false;
		if (usedEnergy > 0) return false;
		//if Energy = 0 destroy this gameobject
		transform.DetachChildren();
		Destroy(gameObject);
		return true;
	}
}