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
	[SerializeField] bool UpdateVisualisation;

	private List<GameObject> AllNeurons = new();
	private List<GameObject> AllWeights = new();

	private List<GameObject> InputNeurons = new();
	private List<GameObject> HiddenNeurons = new();
	private List<GameObject> OutputNeurons = new();

	private List<GameObject> Layers = new();

	private void Update()
	{
		CheckUpdate();
		SetWeightPos();
	}
	void CheckUpdate()
	{
		if (!UpdateVisualisation) return;
		UpdateVisualisation = false;
		UpdateAll();
	}
	public void Draw()
	{
		CreateLayers();
		CreateNeurons(Network.AllNeurons);
		CreateWeights();

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
				Layers.Add(newLayer);
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

			for (int i = 0; i < neurons.Count; i++)
			{
				GameObject neuronLayer = Layers.Find(x => x.GetComponent<LayerVisual>().Layer == neurons[i].Layer);
				GameObject newNeuron = Instantiate(NeuronPreFab, neuronLayer.transform);
				newNeuron.GetComponent<NeuronVisual>().Id = neurons[i].Id;
				
				AddNeuronToList();
				// add neuron to his layer
				neuronLayer.GetComponent<LayerVisual>().AddNeuron(newNeuron, NeuronDistance, true);


				void AddNeuronToList()
				{
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

		}
		void CreateWeights()
		{
			foreach (Weight weight in Network.AllWeights)
			{
				GameObject newWeight = Instantiate(WeightPreFab, WeightsObject.transform);
				WeightVisual weightScript = newWeight.GetComponent<WeightVisual>();
				SetIds();
				weightScript.IsRecurent = weight.IsRecurrent;
				weightScript.IsActivated = weight.IsActivaded;
				AllWeights.Add(newWeight);

				void SetIds()
				{
					weightScript.Id = weight.Id;
					weightScript.InId = weight.InId;
					weightScript.OutId = weight.OutId;
				}
			}
		}
	}
	public void UpdateAll()
	{
		DestroyAll(AllNeurons);
		DestroyAll(AllWeights);
		DestroyAll(Layers);

		void DestroyAll(List<GameObject> input)
		{
			foreach (GameObject gameObject in input)
			{
				Destroy(gameObject);
			}
			input.Clear();
		}
		Draw();
	}
	private void SetWeightPos()
	{
		foreach (GameObject weight in AllWeights)
		{
			if (weight == null) continue;
			Transform inputNeuron = AllNeurons.Find(x => x.GetComponent<NeuronVisual>().Id == weight.GetComponent<WeightVisual>().InId).transform;
			Transform outputNeuron = AllNeurons.Find(x => x.GetComponent<NeuronVisual>().Id == weight.GetComponent<WeightVisual>().OutId).transform;

			//weight.transform.position = inputNeuron.position;
			weight.GetComponent<LineRenderer>().SetPosition(0, inputNeuron.position);
			weight.GetComponent<LineRenderer>().SetPosition(1, outputNeuron.position);
		}
	}
}
