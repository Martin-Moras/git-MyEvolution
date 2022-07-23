using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisBase : MonoBehaviour
{
	[SerializeField] NeuralNetwork Network;
	[SerializeField] GameObject WeightsObject;

	[SerializeField] GameObject NeuronPreFab;
	[SerializeField] GameObject WeightPreFab;
	[SerializeField] GameObject LayerPreFab;

	[SerializeField] float LayerDistance;
	[SerializeField] float NeuronDistance;

	private List<GameObject> AllNeurons = new();

	private List<GameObject> InputNeurons = new();
	private List<GameObject> HiddenNeurons = new();
	private List<GameObject> OutputNeurons = new();

	private List<GameObject> layers = new();
	
	public void Draw()
	{
		CreateLayers();
		CreateNeurons(Network.InputNeurons);
		CreateNeurons(Network.HiddenNeurons);
		CreateNeurons(Network.OutputNeurons);

		void CreateLayers()
		{
			int layerAmount = HiddenLayersAmount() + 2;
			float firstLayerPos = layerAmount / 2 * -1 * LayerDistance;
			
			for (int i = 0; i < layerAmount; i++)
			{
				Vector3 layerPos = Vector3.right * (firstLayerPos + (LayerDistance * i));
				GameObject newLayer = Instantiate(LayerPreFab, transform);
				
				newLayer.transform.localPosition = layerPos;
				newLayer.GetComponent<LayerVisual>().Layer = i + 1;
				layers.Add(newLayer);
			}
			
			int HiddenLayersAmount()
			{
				List<int> hiddenLayers = new();

				foreach(Neuron hiddenNeuron in Network.HiddenNeurons)
				{
					if (hiddenLayers.Exists(x => x == hiddenNeuron.Layer)) continue;
					hiddenLayers.Add(hiddenNeuron.Layer);
				}
				return hiddenLayers.Count;
			}
		}
		void CreateNeurons(List<Neuron> neurons)
		{
			if (neurons.Count == 0) return;

			float firstNeuronPos = (neurons.Count - 1) * NeuronDistance / 2 * -1;

			for (int i = 0; i < neurons.Count; i++)
			{
				GameObject neuronLayer = layers.Find(x => x.GetComponent<LayerVisual>().Layer == neurons[i].Layer);

				GameObject newNeuron = Instantiate(NeuronPreFab, neuronLayer.transform);

				Vector3 neuronPos = Vector3.up * (firstNeuronPos + (NeuronDistance * i));

				newNeuron.transform.localPosition = neuronPos;
				newNeuron.GetComponent<NeuronVisual>().Id = neurons[i].Id;
				//Add neuron to list
				AllNeurons.Add(newNeuron);
				switch (neurons[0].Type)
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
		}
		void CreateWeights()
		{
			foreach (Weight weight in Network.AllWeights)
			{
				GameObject newWeight = Instantiate(WeightPreFab, WeightsObject.transform);

				newWeight.transform.position = AllNeurons.Find(x => x.GetComponent<NeuronVisual>.id == newWeight.GetComponent<WeightVisual>().InputId)
				newWeight.GetComponent<LineRenderer>().set
			}
		}
	}
}
