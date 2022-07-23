public class Weight
{
	public int Id;
	public int InNeuronId;
	public int OutNeuronId;
	public float WeightValue;
	public float InVal;
	public float OutVal;
	public bool IsEnabled;
	public bool IsRecurrent;

	public Weight(int id, int inNeuronId, int outNeuronId, float weightValue)
	{
		Id = id;
		InNeuronId = inNeuronId;
		OutNeuronId = outNeuronId;
		WeightValue = weightValue;
	}
}
