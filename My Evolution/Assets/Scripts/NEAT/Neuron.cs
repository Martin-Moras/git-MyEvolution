using UnityEngine;

[System.Serializable]
public class Neuron
{
	public int Id { get; set; }
	public int Type;// 0 = Input; 1 = Hidden; 2 = Output; 3 = Bias;
	public int Layer { get; set; }
	
	public float InVal { get; set; }
	public float OutVal { get; set; }
	public int activationFunktion; // Sigmioid = 0; Linear = 1; SQRT = 2; Sinus = 3; ABS = 4; Relu = 5

	public Neuron (int id, int type, int layer)
	{
		Id = id;
		Type = type;
		Layer = layer;
	}
	public void ChangeActivationFunk()
	{
		int maxActivationFuncId = 5;
		activationFunktion = Random.Range(0, maxActivationFuncId);
	}
	public void SetOutput()
	{
		switch (activationFunktion)
		{
			case 0: Sigmioid(InVal);
				break;
			case 1: Linear(InVal);
				break;
			case 2: SQRT(InVal);
				break;
			case 3: Sin(InVal);
				break;
			case 4: ABS(InVal);
				break;
			case 5: Relu(InVal);
				break;
		}


		float Sigmioid(float value)
		{
			float e = 2.7182818284f;
			return 1 / (1 + Mathf.Pow(e, value));
		}
		float Linear(float value)
		{
			return value;
		}
		float SQRT(float value)
		{
			return Mathf.Sqrt(value);
		}
		float Sin(float value)
		{
			return Mathf.Sin(value);
		}
		float ABS(float value)
		{
			return Mathf.Abs(value);
		}
		float Relu(float value)
		{
			if (value < 0) return 0;
			return value;
		}
	}
}
