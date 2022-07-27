[System.Serializable]
public class Weight
{
	public int Id;
	public int InId;
	public int OutId;
	public float WeightVal;
	public float InVal;
	public float OutVal;
	public bool IsActivaded = true;
	public bool IsRecurrent;

	public Weight(int id, int inNeuronId, int outNeuronId, float weightValue)
	{
		Id = id;
		InId = inNeuronId;
		OutId = outNeuronId;
		WeightVal = weightValue;
	}
}
