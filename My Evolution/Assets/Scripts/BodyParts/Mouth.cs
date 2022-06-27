using UnityEngine;

public class Mouth : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public GameObject MouthObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }
	public float Damage { get; set; }

	public decimal StoredFood;
	private float DigestColdown;
	private float DigestTime;
	[SerializeField] private string StoredFoodn;

	public Mouth(GameObject mouthObject, Vector2 location, float maxHealth, float damage)
	{
		MouthObject = mouthObject;
		Location = location;
		MaxHealth = maxHealth;
		Damage = damage;
	}
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
		StoredFoodn = StoredFood.ToString();
	}
	public void Bite()
	{
		Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position + transform.right / 10, new Vector2(.1f, .1f), transform.rotation.z);

		foreach (Collider2D collider in targets)
		{
			Atributes colliderAtributes = collider.transform.GetComponent<Atributes>();
			
			if (colliderAtributes == null) continue;
			if (!IseAnotherParent(collider)) continue;

			StoredFood += colliderAtributes.DamageAndEat(Damage);

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
	public decimal TakeFood(decimal amount)
	{
		if (amount > StoredFood) amount = StoredFood;

		StoredFood -= amount;
		return amount;
	}
	private void Digest()
	{
		if (transform.parent == null) return;
		if (DigestTime > 0) return;
		if (StoredFood <= 0) return;

		decimal digestAmount = 1;

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
