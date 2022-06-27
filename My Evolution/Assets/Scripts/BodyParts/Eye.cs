using System.Collections.Generic;
using UnityEngine;

public class EyeDetectetObject : MonoBehaviour
{
	public GameObject DetectetObject { get; set; }
	public float ObjectDistance { get; set; }
	[SerializeField] private GameObject GM;

	public EyeDetectetObject(GameObject detectetObject, float objectPos)
	{
		DetectetObject = detectetObject;
		ObjectDistance = objectPos;
	}
}

public class Eye : MonoBehaviour
{
	public float MaxHealth { get; set; }
	public float LookDistance { get; set; }
	public float ConsumedEnergy { get; set; }

	public Eye(float maxHealth, float lookDistance, float consumedEnergy)
	{
		MaxHealth = maxHealth;
		LookDistance = lookDistance;
		ConsumedEnergy = consumedEnergy;
	}
	public Eye(){}
	public List<EyeDetectetObject> Look()
	{
		LookDistance = 1000;
		List<EyeDetectetObject> output = new();
		
		RaycastHit2D[] allObjectsInLookangle = Physics2D.RaycastAll(transform.position + (transform.right / 20), transform.right, LookDistance);
		
		foreach (RaycastHit2D hit in allObjectsInLookangle)
		{
			EyeDetectetObject detectetObject = new EyeDetectetObject(hit.transform.gameObject, hit.distance);

			if (output.Find(x => x.DetectetObject == detectetObject) != null) continue;

			output.Add(detectetObject);
		}
		return output;
	}
}
