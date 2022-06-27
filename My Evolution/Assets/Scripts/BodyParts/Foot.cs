using UnityEngine;

public class Foot : MonoBehaviour
{
	[SerializeField] private GameManager GM;
	public GameObject FootObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }
	public float ConsumedEnergy { get; set; }

	public Foot(GameObject footObject, Vector2 location, float maxHealth, float consumedEnergy)
	{
		FootObject = footObject;
		Location = location;
		MaxHealth = maxHealth;
		ConsumedEnergy = consumedEnergy;
	}
	public Foot(){}

	public void Walk(Vector2 dir)
	{
		Rigidbody2D parentRb = transform.parent.GetComponent<Rigidbody2D>();

		parentRb.AddForce(dir.normalized * 60 * Time.deltaTime);
	}
	public void Turn(int dir)
	{
		Rigidbody2D parentRb = transform.parent.GetComponent<Rigidbody2D>();

		parentRb.AddTorque(dir * 4 * Time.deltaTime);
	}
}
