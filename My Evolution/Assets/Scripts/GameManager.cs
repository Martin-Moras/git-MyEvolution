using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GM", menuName = "Game Manager")]
public class GameManager : ScriptableObject
{
	[SerializeField] public GameObject Ear;
	[SerializeField] public GameObject Eye;
	[SerializeField] public GameObject Foot;
	[SerializeField] public GameObject Hand;
	[SerializeField] public GameObject Mouth;
	[SerializeField] public GameObject Shield;
	[SerializeField] public GameObject Stomach;
	[SerializeField] public GameObject Brain;

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
			Dictionary<string, GameObject> output = new()
			{
				{ "Ear", Ear },
				{ "Eye", Eye },
				{ "Foot", Foot },
				{ "Hand", Hand },
				{ "Mouth", Mouth },
				{ "Shield", Shield },
				{ "Stomach", Stomach },
				{ "Brain", Brain },
			};
			return output;
		}
	}
	public List<object> AllOrganScripts
	{
		get
		{
			return new()
			{
				new Ear(),
				new Eye(),
				new Foot(),
				new Hand(),
				new Mouth(),
				new Shield(),
				new Stomach(),
				new Brain()
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
	public Dictionary<string, object> AllOrganScriptsDict = new()
	{
		{ "Ear", Instantiate(new Ear()) },
		{ "Eye", Instantiate(new Eye()) },
		{ "Foot", Instantiate(new Foot()) },
		{ "Hand", Instantiate(new Hand()) },
		{ "Mouth", Instantiate(new Mouth()) },
		{ "Shield", Instantiate(new Shield()) },
		{ "Stomach", Instantiate(new Stomach()) },
		{ "Brain", Instantiate(new Brain()) },
	};

	public Organ ToOrgan(GameObject inputOrgan)
	{
		return new Organ
			(
				inputOrgan.name,
				inputOrgan.transform.localPosition,
				0,
				(int)inputOrgan.transform.localRotation.z,
				GetOrganScript(inputOrgan),
				inputOrgan.GetComponent<Atributes>()
			);
	}
	public GameObject ToGameObject(Organ inputOrgan)
	{
		GameObject outputOrgan = Instantiate(AllOrgansDict[inputOrgan.Name]);
		outputOrgan.name = inputOrgan.Name;
		outputOrgan.transform.localPosition = inputOrgan.LocalPos;
		outputOrgan.transform.localRotation = Quaternion.Euler(0, 0, inputOrgan.LocalRot);
		Atributes atributes = outputOrgan.GetComponent<Atributes>();
		atributes = inputOrgan.OrganAtributes;

		return outputOrgan;
	}
	public List<Organ> CopyOrganList(List<Organ> input)
	{
		List<Organ> output = new();
		foreach (Organ inputOrgan in input)
		{
			output.Add(NewOrgan(inputOrgan));
		}
		return output;
	}
	public Organ NewOrgan(Organ inputOrgan)
	{
		return new Organ(inputOrgan.Name, inputOrgan.LocalPos, inputOrgan.LocalRot, inputOrgan.Level, inputOrgan.OrganScript.GetType(), new Atributes());
	}
	public T Reset<T>(T input)
	{
		return default;
	}
	public GameObject AddOrganScript(object input, GameObject gameObject)
	{
		if (input.GetType() == typeof(Ear)) gameObject.AddComponent<Ear>();
		else if (input.GetType() == typeof(Eye)) gameObject.AddComponent<Eye>();
		else if (input.GetType() == typeof(Foot)) gameObject.AddComponent<Foot>();
		else if (input.GetType() == typeof(Hand)) gameObject.AddComponent<Hand>();
		else if (input.GetType() == typeof(Mouth)) gameObject.AddComponent<Mouth>();
		else if (input.GetType() == typeof(Shield)) gameObject.AddComponent<Shield>();
		else if (input.GetType() == typeof(Brain)) gameObject.AddComponent<Brain>();

		return gameObject;
	}
	public object GetOrganScript(GameObject gameObject)
	{
		Ear ear = gameObject.GetComponent<Ear>();
		if (ear != null)
		{
			return ear;
		}
		Eye eye = gameObject.GetComponent<Eye>();
		if (eye != null)
		{
			return eye;
		}
		Foot foot = gameObject.GetComponent<Foot>();
		if (foot != null)
		{
			return foot;
		}
		Hand hand = gameObject.GetComponent<Hand>();
		if (hand != null)
		{
			return hand;
		}
		Mouth mouth = gameObject.GetComponent<Mouth>();
		if (mouth != null)
		{
			return mouth;
		}
		Shield shield = gameObject.GetComponent<Shield>();
		if (shield != null)
		{
			return shield;
		}
		Stomach stomach = gameObject.GetComponent<Stomach>();
		if (stomach != null)
		{
			return stomach;
		}

		Brain brain = gameObject.GetComponent<Brain>();
		if (brain != null)
		{
			return brain;
		}
		return null;
	}
}
	/*public GameObject AddOrganScript(GameObject inputObject, string name)
	{
		string type = name;

		switch (type)
		{
			case "Ear":
				Ear ear = outputOrgan.AddComponent<Ear>();
				ear = (Ear)inputOrgan.OrganScript;
				break;

			case "Eye":
				Eye eye = outputOrgan.AddComponent<Eye>();
				eye = (Eye)inputOrgan.OrganScript;
				break;

			case "Foot":
				Foot foot = outputOrgan.AddComponent<Foot>();
				foot = (Foot)inputOrgan.OrganScript;
				break;

			case "Hand":
				Hand hand = outputOrgan.AddComponent<Hand>();
				hand = (Hand)inputOrgan.OrganScript;
				break;

			case "Mouth":
				Mouth mouth = outputOrgan.AddComponent<Mouth>();
				mouth = (Mouth)inputOrgan.OrganScript;
				break;

			case "Shield":
				Shield shield = outputOrgan.AddComponent<Shield>();
				shield = (Shield)inputOrgan.OrganScript;
				break;

			case "Stomach":
				Stomach stomach = outputOrgan.AddComponent<Stomach>();
				stomach = (Stomach)inputOrgan.OrganScript;
				break;

			case "Brain":
				Brain brain = outputOrgan.AddComponent<Brain>();
				brain = (Brain)inputOrgan.OrganScript;
				break;
		}
	}*/