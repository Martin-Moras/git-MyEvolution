using System;
using UnityEngine;

public class Mouth : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public float Damage { get; set; }
	private Atributes Atrib { get { return GetComponent<Atributes>(); } }
	private float DigestColdown;
	private float DigestTime;
	[SerializeField] private string StoredFoodn;

	public Mouth(){}
	private void Start()
	{
		if (Damage == 0) Damage = .5f;
		if (DigestColdown == 0) DigestColdown = 5f;
	}
	private void Update()
	{
		SetDigestTime();
		Digest();
		StoredFoodn = Atrib.Energy.ToString();
	}
	public void Bite()
	{
		Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position + transform.right / 10, new Vector2(.09f, .09f), transform.rotation.z);

		foreach (Collider2D collider in targets)
		{
			Atributes colliderAtributes = collider.transform.GetComponent<Atributes>();
			
			if (colliderAtributes == null) continue;
			if (!IseAnotherParent(collider)) continue;

			colliderAtributes.DamageAndEat(ref Atrib.Energy, Convert.ToDecimal(Damage));

			bool IseAnotherParent(Collider2D otherObject)
			{
				int myParent;
				int yourParent;

				if (transform.parent != null)
				{
					myParent = transform.parent.GetInstanceID();
				}
				else
				{
					myParent = transform.GetInstanceID();
				}
				if (otherObject.transform.parent != null)
				{
					yourParent = otherObject.transform.parent.GetInstanceID();
				}
				else
				{
					yourParent = otherObject.transform.GetInstanceID();
				}

				if (myParent == yourParent) return false;
				return true;
			}
		}
	}
	public void TakeFood(ref decimal sorce, decimal amount)
	{
		if (amount <= 0) return;
		if (Atrib.Energy < amount) amount = Atrib.Energy;

		Atrib.Energy -= amount;
		sorce += amount;
	}
	private void Digest()
	{
		if (transform.parent == null) return;
		if (DigestTime > 0) return;
		if (Atrib.Energy <= 0) return;

		DigestTime = DigestColdown;
		decimal digestAmount = 1;
		//give brain energy
		transform.parent.GetComponent<Brain>().GiveEnergy(ref Atrib.Energy, digestAmount);
	}
	private void SetDigestTime()
	{
		if (DigestTime <= 0) return;
		
		DigestTime -= Time.deltaTime;
	}
}
