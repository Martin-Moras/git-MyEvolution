
[System.Serializable]
public class Neuron
{
	public int Id { get; set; }
	public int Type;// 0 = Input; 1 = Hidden; 2 = Output; 3 = Bias;
	public int Layer { get; set; }
	
	public float InVal { get; set; }
	public float OutVal { get; set; }

	public Neuron (int id, int type, int layer)
	{
		Id = id;
		Type = type;
		Layer = layer;
	}
}
