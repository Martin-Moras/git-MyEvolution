using UnityEngine;
using System;

public class Atributes : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public UnityEngine.Object OrganScript;
	public decimal Level = 1;
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
			if (value == health) return;

			health = value;
			if (value > 0) return;
			CheckDeath();
		}
	}
	public float NeededEnergy { get; private set; }
	private string StoredFoodn;

	private bool isDead = false;

	private void Start()
	{
		SetAtributes();
		if (gameObject.TryGetComponent(out Brain b)) GM.AddRb(gameObject);
		CheckDeath();
	} 
	private void Update()
	{
		StoredFoodn = UsedEnergie.ToString();
	}

	private void SetAtributes()
	{
		if (UsedEnergie == 0) UsedEnergie = 5;
		if (Health == 0) Health = 2;
		GM.NeededEnergyDict.TryGetValue(gameObject.name, out float neededEnergy);
		NeededEnergy = neededEnergy;
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
			if (UsedEnergie < eatAmount) eatAmount = UsedEnergie;
			
			UsedEnergie -= eatAmount;
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
	public void CheckDeath()
	{
		if (transform.parent == null && transform.CompareTag("Organ")) Health = 0;
		if (Health > 0) return;
		if (CheckEnergy()) return;
		if (isDead) return;
		
		isDead = true;
		transform.DetachChildren();
		if (transform.parent != null) transform.SetParent(null);
		GM.AddRb(gameObject);
		RemoveOrganScript();
		
		void RemoveOrganScript()
		{
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
		if (UsedEnergie > 0) return false;
		//if Energy = 0 destroy this gameobject
		Destroy(gameObject);
		return true;
	}
}