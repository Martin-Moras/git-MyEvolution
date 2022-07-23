using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
	[SerializeField] private VisBase VisualNetwork;

	[SerializeField] private int InputNeuronAmount;
	[SerializeField] private int HiddenNeuronAmount;
	[SerializeField] private int OutputNeuronAmount;
	[SerializeField] private float ConnectionPersentige;

	public List<Neuron> InputNeurons = new();// 0 = Input;
	public List<Neuron> HiddenNeurons = new();// 1 = Hidden;
	public List<Neuron> OutputNeurons = new();// 2 = Output;

	public List<Weight> AllWeights;

	private List<Weight> TestWeights;
	private List<Neuron> TestNeurons;

	private List<int> Layers;
	private int currNeuronId = 0;

	private void Start()
	{
		Initialise();
		VisualNetwork.Draw();
	}

	void Initialise()
	{
		InitialiseWeights();
		InitialiseNeurons();

		void InitialiseNeurons()
		{
			AddNeurons(InputNeuronAmount, 0);
			AddNeurons(OutputNeuronAmount, 2);
			AddNeurons(HiddenNeuronAmount, 1);

			void AddNeurons(int amount, int neuronType)
			{
				for (int i = 0; i < amount; i++)
				{
					CreateNeuron(neuronType);
				}
			}
		}
		void InitialiseWeights()
		{
			AddWeights(InputNeurons, OutputNeurons);
			AddWeights(InputNeurons, HiddenNeurons);
			AddWeights(HiddenNeurons, OutputNeurons);

			void AddWeights(List<Neuron> inNeurons, List<Neuron> outNeurons)
			{
				if (inNeurons.Count == 0) return; 
				if (outNeurons.Count == 0) return;

				foreach (Neuron outputNeuron in outNeurons)
				{
					foreach (Neuron inputNeuron in inNeurons)
					{
						AllWeights.Add(CreateWeight(inputNeuron, outputNeuron));
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
				// conected Weights
				SetConectedWeights_InVal(conectedWeights, neuron);
				SetConectedWeights_OutVal(conectedWeights);
				// Neuron
				SetInputVal(neuron, conectedWeights);
				SetOutputVal(neuron);
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
				neuron.OutVal = neuron.InVal;
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
	Weight CreateWeight(Neuron inNeuron, Neuron outNeuron)
	{
		int weightId = GetId(inNeuron.Id, outNeuron.Id);
		Weight newWeight = new Weight(weightId, inNeuron.Id, outNeuron.Id, Random.Range(-20, 20));
		return newWeight;

		int GetId(int inNeuronId, int outNeuronId)
		{
			return inNeuronId * 1000 + outNeuronId;
		}
	}
	Neuron CreateNeuron(int neuronType)
	{
		int layer = SetLayer();

		currNeuronId++;
		Neuron newNeuron = new Neuron(currNeuronId, neuronType, layer);
		AddNeuronToList();
		UpdateOutputNeuron_Layer();

		return newNeuron;

		void AddNeuronToList()
		{
			switch (neuronType)
			{
				case 0:
					InputNeurons.Add(newNeuron);
					break;
				case 1:
					HiddenNeurons.Add(newNeuron);
					break;
				case 2:
					OutputNeurons.Add(newNeuron);
					break;
			}
		}
		int SetLayer()
		{
			int layer = -1;
			switch (neuronType)
			{
				case 0:
					layer = 1;
					break;
				case 1:
					layer = 2;
					break;
				case 2:
					layer = 2;
					break;
			}
			return layer;
		}
		void UpdateOutputNeuron_Layer()
		{
			if (OutputNeurons.Count == 0) return;

			int hightesLayer = GetHighestHiddenLayer();
			if (hightesLayer < OutputNeurons[0].Layer) return;

			foreach (Neuron outputNeuron in OutputNeurons)
			{
				outputNeuron.Layer = hightesLayer + 1;
			}

			int GetHighestHiddenLayer()
			{
				int hightesLayer = 1;

				foreach (Neuron hiddenNeuron in HiddenNeurons)
				{
					if (hiddenNeuron.Layer <= hightesLayer) continue;

					hightesLayer = hiddenNeuron.Layer;
				}
				return hightesLayer;
			}
		}
	}
}