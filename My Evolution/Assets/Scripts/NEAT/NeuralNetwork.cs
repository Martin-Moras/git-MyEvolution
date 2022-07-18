using System.Collections.Generic;

public class NeuralNetwork
{
    private List<Neuron> InputNeurons;
    private List<Neuron> HiddenNeurons;
    private List<Neuron> OutputNeurons;
    private float startConnectionPersentige;

    private List<Weight> AllWeights;
    private List<Neuron> AllNeurons;

	private List<Weight> TestWeights;
	private List<Neuron> TestNeurons;

	private List<int> Layers;

	void Initialise()
	{
		InitialiseWeights();
		InitialiseNeurons();

		void InitialiseNeurons()
		{

		}
		void InitialiseWeights()
		{
			Conect_InAndOut();

			if (HiddenNeurons.Count == 0) return;

			Conect_InAndHidden();
			Conect_OutAndHidden();


			void Conect_InAndOut()
			{
				foreach (Neuron outputNeuron in OutputNeurons)
				{
					foreach (Neuron inputNeuron in InputNeurons)
					{
						AllWeights.Add(new Weight(0, inputNeuron.Id, outputNeuron.Id));
					}
				}
			}
			void Conect_InAndHidden()
			{
				foreach (Neuron hiddenNeuron in HiddenNeurons)
				{
					foreach (Neuron inputNeuron in InputNeurons)
					{
						AllWeights.Add(new Weight(0, inputNeuron.Id, hiddenNeuron.Id));
					}
				}
			}
			void Conect_OutAndHidden()
			{
				foreach (Neuron outputNeuron in OutputNeurons)
				{
					foreach (Neuron hiddenNeuron in HiddenNeurons)
					{
						AllWeights.Add(new Weight(0, hiddenNeuron.Id, outputNeuron.Id));
					}
				}
			}
		}
	}
	void AddNote()
	{

	}
	void AddConnecktion()
	{

	}
	void MutateWeights()
	{

	}
	void LoadInputs(Neuron[] inputNeurons)
	{

	}
	void RunNetwork()
	{
		int currLayer = 0;
		while (true)
		{
			currLayer++;
			List<Neuron> currLayerNeurons = HiddenNeurons.FindAll(x => x.Layer == currLayer);
			if (currLayerNeurons.Count == 0) break;

			foreach (Neuron neuron in currLayerNeurons)
			{
				// Get all conected weights
				List<Weight> conectedWeights = AllWeights.FindAll(x => x.OutNeuronId == neuron.Id);
				// Neuron
				SetInputVal(neuron, conectedWeights);
				SetOutputVal(neuron);
				// conected Weights
				SetConectedWeights_InVal(conectedWeights, neuron);
				SetConectedWeights_OutVal(conectedWeights);
			}

			void SetInputVal(Neuron neuron, List<Weight> conectedWeights)
			{
				neuron.InVal = 0;

				foreach (Weight weight in conectedWeights)
				{
					neuron.InVal += weight.OutVal;
				}
			}
			void SetOutputVal(Neuron neuron)
			{
				neuron.OutVal = neuron.InVal + neuron.Bias;
			}
			void SetConectedWeights_InVal(List<Weight> conectedWeights, Neuron inputNeuron)
			{
				foreach (Weight weight in conectedWeights)
				{
					weight.InVal = inputNeuron.OutVal;
				}
			}
			void SetConectedWeights_OutVal(List<Weight> conectedWeights)
			{
				foreach (Weight weight in conectedWeights)
				{
					weight.OutVal = weight.InVal * weight.WeightValue;
				}
			}
		}
	} 
}
	/*float[] GetOutput()
	{

	}*/