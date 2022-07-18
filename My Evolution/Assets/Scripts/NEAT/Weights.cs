public class Weight
{
	public NeuralNetwork MyNetwork;

	public int Id;
	public int InNeuronId;
	public int OutNeuronId;
	public float WeightValue;
	public float InVal { get; set; }
	public float OutVal { get;  set; }
	public bool IsEnabled;
	public bool IsRecurrent;

	public Weight(int id, int inNeuronId, int outNeuronId)
	{
		Id = id;
		InNeuronId = inNeuronId;
		OutNeuronId = outNeuronId;
	}
}
