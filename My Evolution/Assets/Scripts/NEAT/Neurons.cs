public class Neuron
{
	public NeuralNetwork MyNetwork;

	public int Id { get; private set; }
	public string Type;
	public int Layer { get; private set; }
	
	public float InVal { get; set; }
	public float OutVal { get; set; }
	public float Bias;

	public Neuron (NeuralNetwork myNetwork, int id, string type, int layer)
	{
		MyNetwork = myNetwork;
		Id = id;
		Type = type;
		Layer = layer;
	}
}
