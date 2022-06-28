using UnityEngine;

public class Organ : MonoBehaviour
{
    public GameObject OrganObject { get; set; }
    public Vector2 LocalPos { get; set; }
    public decimal Level { get; set; }

    public Organ(GameObject organObject, Vector2 localPos, decimal level)
	{
		OrganObject = organObject;
		LocalPos = localPos;
		Level = level;
	}
}
