using UnityEngine;

public class Shield : MonoBehaviour
{
	public GameObject ShieldObject { get; set; }
	public Vector2 Location { get; set; }
	public float MaxHealth { get; set; }
	public float ConsumedEnergy { get; set; }

	public Shield(GameObject shieldObject, Vector2 location, float maxHealth, float consumedEnergy)
	{
		ShieldObject = shieldObject;
		Location = location;
		MaxHealth = maxHealth;
		ConsumedEnergy = consumedEnergy;
	}
	public Shield(){}
}
