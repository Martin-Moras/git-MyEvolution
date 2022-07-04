using UnityEngine;

public class Organ
{
	[SerializeField] private GameManager GM;

	public string Name { get; set; }
	public Vector2 LocalPos { get; set; }
	public int LocalRot { get; set; }
	public decimal Level { get; set; }
	private object organScript;
	public object OrganScript 
	{
		get { return organScript; }
		set
		{
			organScript = value;
			/*var input = value as GameObject;
			if (input != null)
			{
				Ear Ear = input.GetComponent<Ear>();
				if (Ear != null)
				{
					organScript = Ear;
				}
				Eye eye = input.GetComponent<Eye>();
				if (eye != null)
				{
					organScript = eye;
				}
				Foot foot = input.GetComponent<Foot>();
				if (foot != null)
				{
					organScript = foot;
				}
				Hand hand = input.GetComponent<Hand>();
				if (hand != null)
				{
					organScript = hand;
				}
				Mouth mouth = input.GetComponent<Mouth>();
				if (mouth != null)
				{
					organScript = mouth;
				}
				Shield shield = input.GetComponent<Shield>();
				if (shield != null)
				{
					organScript = shield;
				}
				Stomach stomach = input.GetComponent<Stomach>();
				if (stomach != null)
				{
					organScript = stomach;
				}

				Brain brain = input.GetComponent<Brain>();
				if (brain != null)
				{
					organScript = brain;
				}
			}*/
		}
	}
	public Atributes OrganAtributes { get; set; }

	public Organ(string name, Vector2 localPos, int localRot, decimal level, object organScript, Atributes organAtributes)
	{
		Name = name;
		LocalPos = localPos;
		LocalRot = localRot;
		Level = level;
		OrganScript = organScript;
		OrganAtributes = organAtributes;
	}
	/*public Organ()
	{
		Name = "";
		LocalPos = Vector2.zero;
		LocalRot = 0;
		Level = 0;
		OrganScript = null;
		OrganAtributes = new Atributes();
	}*/
	public Organ() { }
}