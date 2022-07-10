using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GM", menuName = "Game Manager")]
public class GameManager : ScriptableObject
{
	public GameObject Ear;
	public GameObject Eye;
	public GameObject Foot;
	public GameObject Hand;
	public GameObject Mouth;
	public GameObject Shield;
	public GameObject Stomach;
	public GameObject Brain;
	
	public GameObject Egg;

	[SerializeField] private float Ear_NeededEnergy;
	[SerializeField] private float Eye_NeededEnergy;
	[SerializeField] private float Foot_NeededEnergy;
	[SerializeField] private float Hand_NeededEnergy;
	[SerializeField] private float Mouth_NeededEnergy;
	[SerializeField] private float Shield_NeededEnergy;
	[SerializeField] private float Stomach_NeededEnergy;
	[SerializeField] private float Brain_NeededEnergy;


	[SerializeField] private float RbDrag;
	[SerializeField] private float RbAngDrag;

	public List<GameObject> AllOrgans
	{
		get
		{
			return new()
			{
				Ear,
				Eye,
				Foot,
				Hand,
				Mouth,
				Shield,
				Stomach,
			};
		}
	}
	public Dictionary<string, GameObject> AllOrgansDict
	{
		get
		{
			return  new()
			{
				{ "Ear", Ear },
				{ "Eye", Eye },
				{ "Foot", Foot },
				{ "Hand", Hand },
				{ "Mouth", Mouth },
				{ "Shield", Shield },
				{ "Stomach", Stomach },
				{ "Brain", Brain }
			};
		}
	}
	public Dictionary<string, float> NeededEnergyDict
	{
		get
		{
			return new()
			{
				{ "Ear", Ear_NeededEnergy },
				{ "Eye", Eye_NeededEnergy },
				{ "Foot", Foot_NeededEnergy },
				{ "Hand", Hand_NeededEnergy },
				{ "Mouth", Mouth_NeededEnergy },
				{ "Shield", Shield_NeededEnergy },
				{ "Stomach", Stomach_NeededEnergy },
				{ "Brain", Brain_NeededEnergy }
			};
		}
	}
	public List<string> AllOrganNames
	{
		get
		{
			return new()
			{
				"Ear",
				"Eye",
				"Foot",
				"Hand",
				"Mouth",
				"Shield",
				"Stomach",
				"Brain"
			};
		}
	}

	public Organ ToOrgan(GameObject inputOrgan)
	{
		return new Organ
			(
				inputOrgan.name,
				inputOrgan.transform.localPosition,
				0,
				(int)inputOrgan.transform.localRotation.z,
				this,
				0
			);
	}
	public GameObject ToGameObject(Organ inputOrgan, Transform parent)
	{
		GameObject outputOrgan = Instantiate(AllOrgansDict[inputOrgan.Name], parent);
		Atributes atributes = outputOrgan.GetComponent<Atributes>();
		outputOrgan.name = inputOrgan.Name;
		outputOrgan.transform.localPosition = inputOrgan.LocalPos;
		outputOrgan.transform.localRotation = Quaternion.Euler(0, 0, inputOrgan.LocalRot);
		atributes.Level = inputOrgan.Level;
		atributes.UsedEnergy = inputOrgan.UsedEnergy;

		return outputOrgan;
	}
	public List<Organ> CopyOrganList(List<Organ> input)
	{
		List<Organ> output = new();
		foreach (Organ inputOrgan in input)
		{
			output.Add(inputOrgan.Copy());
		}
		return output;
	}
	public GameObject CreateGameobject(Organ organ, Transform parent, ref decimal energySorce, Vector2 pos, Quaternion rot)
	{
		GameObject obj = Instantiate(AllOrgansDict[organ.Name], parent);
		obj.name = organ.Name;
		obj.transform.localPosition = pos;
		obj.transform.localRotation = rot;
		Atributes atributes = obj.GetComponent<Atributes>();
		atributes.Level = organ.Level;
		atributes.SetUsedEnergy(ref energySorce, atributes.NeededEnergy);
		
		return obj;
	}
	//Rigidbody
	public Rigidbody2D AddRb(GameObject input)
	{
		if (input.GetComponent<Rigidbody2D>() != null) return null;

		Rigidbody2D outputRb = input.AddComponent<Rigidbody2D>();
		outputRb.drag = RbDrag;
		outputRb.angularDrag = RbAngDrag;

		return outputRb;
	}
	//Math
	public static float Lerp(float a, float b, float t)
	{
		//return a + (b - a) * t;
		return (1 - t) * a + b * t;
	}
	public static float InvLerp(float a, float b, float v)
	{
		//return b - a * v;
		return (v - a) / (b - a);
	}
	public static float Remap(float iMin, float iMax, float oMin, float oMax, float value)
	{
		//i = input range
		//o = output range
		float t = InvLerp(iMin, iMax, value);
		return Lerp(oMax, oMin, t);
	}
}
/*public object GetOrganScript(GameObject obj)
	{
		obj.TryGetComponent(out Eye eye);
		if (eye != null) return eye;
		obj.TryGetComponent(out Foot foot);
		if (foot != null) return foot;
		obj.TryGetComponent(out Hand hand);
		if (hand != null) return hand;
		obj.TryGetComponent(out Mouth mouth);
		if (mouth != null) return mouth;
		obj.TryGetComponent(out Shield shield);
		if (shield != null) return shield;
		obj.TryGetComponent(out Stomach stomach);
		if (stomach != null) return stomach;
		obj.TryGetComponent(out Brain brain);
		if (brain != null) return brain;
		return null;
	}*/