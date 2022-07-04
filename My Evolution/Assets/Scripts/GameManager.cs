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
		{ "Ear", new Ear() },
		{ "Eye", new Eye() },
		{ "Foot", new Foot() },
		{ "Hand", new Hand() },
		{ "Mouth", new Mouth() },
		{ "Shield", new Shield() },
		{ "Stomach", new Stomach() },
		{ "Brain", new Brain() },
	};

	public Organ ToOrgan(GameObject inputOrgan)
	{
		return new Organ
			(
				inputOrgan.name,
				inputOrgan.transform.localPosition,
				0,
				(int)inputOrgan.transform.localRotation.z,
				inputOrgan.name,
				new Atributes()
			);
	}
	public GameObject ToGameObject(Organ inputOrgan)
	{
		GameObject outputOrgan = new();
		outputOrgan.name = inputOrgan.Name;
		outputOrgan.transform.localPosition = inputOrgan.LocalPos;
		outputOrgan.transform.localRotation = Quaternion.Euler(0, 0, inputOrgan.LocalRot);
		AddOrganScript(inputOrgan.OrganScript, outputOrgan);
		outputOrgan.AddComponent<Atributes>();

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
		return new Organ(inputOrgan.name, inputOrgan.LocalPos, inputOrgan.LocalRot, inputOrgan.Level, Reset(inputOrgan.OrganScript), new Atributes());
	}
	public T Reset<T>(T input)
	{
		return default(T);
	}
	public T AddOrganScript<T>(T input, GameObject gameObject)
	{
		return gameObject.GetComponent<T>();
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