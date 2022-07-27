using System.Collections.Generic;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NeuralNetwork : MonoBehaviour
{
	[SerializeField] private VisBase VisualNetwork;

	[SerializeField] public int InputNeuronAmount;
	[SerializeField] private int HiddenNeuronAmount;
	[SerializeField] private int OutputNeuronAmount;
	[SerializeField] private float WeightConnectionPersentige = 25;

	[SerializeField] private bool mutate;
	[SerializeField] private int removeWeightId;
	[SerializeField] private bool removeA_Weight;
	[SerializeField] private int removeNeuronId;
	[SerializeField] private bool removeA_Neuron;

	public float WeightMutationCance = 80;
	public float WeightResetCance = 10;
	public float WeightChangeAmount = 20;
	public float AddWeightChance = 5;
	public float WeightCrationAttempts = 20;
	public float WeightRactivateChance = 25;
	public bool AllowRecurentConnections = true;
	
	public float AddNeuronChance = 25;

	public List<Neuron> InputNeurons { get; private set; } = new();// 0 = Input;
	public List<Neuron> HiddenNeurons { get; private set; } = new();// 1 = Hidden;
	public List<Neuron> OutputNeurons { get; private set; } = new();// 2 = Output;
	public List<Neuron> AllNeurons { get; private set; } = new();

	public List<Weight> AllWeights = new();

	private int currNeuronId = 0;

	private void Start()
	{
		Initialise();
		VisualNetwork.Draw();
	}
	private void Update()
	{
		CheckMutate();
		CheckRemoveWeightOrNeuron();
	}
	void CheckMutate()
	{
		if (!mutate) return;
		mutate = false;
		Mutate();
	}
	void CheckRemoveWeightOrNeuron()
	{
		RemoveNeu();
		RemoveWeigh();
		void RemoveNeu()
		{
			if (!removeA_Neuron) return;
			removeA_Neuron = false;
			Neuron neuron = AllNeurons.Find(x => x.Id == removeNeuronId);
			if (neuron == null) return;
			RemoveNeuronOrWeight(neuron);
		}
		void RemoveWeigh()
		{
			if (!removeA_Weight) return;
			removeA_Weight = false;
			Weight weight = AllWeights.Find(x => x.Id == removeWeightId);
			if (weight == null) return;
			RemoveNeuronOrWeight(weight);
		}
		
	}

	void Initialise()
	{
		InitialiseNeurons();
		InitialiseWeights();

		void InitialiseNeurons()
		{
			AddNeurons(InputNeuronAmount, 0, 1);
			AddNeurons(OutputNeuronAmount, 2, 2);
			AddNeurons(HiddenNeuronAmount, 1, 2);

			void AddNeurons(int amount, int neuronType, int layer)
			{
				for (int i = 0; i < amount; i++)
				{
					CreateNeuron(neuronType, layer);
				}
				UpdateOutput_Layer();
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
						if (!RandomPers(WeightConnectionPersentige)) continue;

						CreateWeight(inputNeuron, outputNeuron, true, true);
					}
				}
				UpdateLayersAndWeights();
			}
		}
	}
	public void Mutate()
	{
		MutateWeightValue();
		ReanableWeight();
		AddWeight();
		AddHiddenNeuron();
		VisualNetwork.UpdateAll();

		void MutateWeightValue()
		{
			foreach (Weight weight in AllWeights)
			{
				if (!RandomPers(WeightMutationCance)) continue;

				if (RandomPers(WeightResetCance))
				{
					weight.WeightVal = Random.Range(-20, 20);
				}
				else
				{
					weight.WeightVal %= Random.Range(-WeightChangeAmount, WeightChangeAmount);
				}
			}
		}
		void ReanableWeight()
		{
			if (!RandomPers(WeightRactivateChance)) return;

			List<Weight> disabledWeights = DisabledWeights();
			if (disabledWeights.Count == 0) return;
			disabledWeights[Random.Range(0, disabledWeights.Count - 1)].IsActivaded = true;

			List<Weight> DisabledWeights()
			{
				List <Weight> disabledWeights = new();
				foreach (Weight weight in AllWeights)
				{
					if (!weight.IsActivaded) disabledWeights.Add(weight);
				}
				return disabledWeights;
			}
		}
		void AddWeight()
		{
			if (!RandomPers(AddWeightChance)) return;

			for (int i = 0; i < WeightCrationAttempts; i++)
			{
				Neuron inNeuron = RamdomNeuron();
				Neuron outNeuron = RamdomNeuron();

				if (CreateWeight(inNeuron, outNeuron, true, true) == null) continue;
				
				break;

				
				Neuron RamdomNeuron()
				{
					return AllNeurons[Random.Range(0, AllNeurons.Count - 1)];
				}
			}
			UpdateLayersAndWeights();
		}
		void RemoveRandomWeight()
		{

		}
		void AddHiddenNeuron()
		{
			if (!RandomPers(AddNeuronChance)) return;
			
			Weight randWeight = RandomWeight();
			if (randWeight == null) return;
			randWeight.IsActivaded = false;

			Neuron newNeuron = CreateNewNeuron();
			ConectNewWeightsTo_NewNeuron();
			UpdateLayersAndWeights();

			Weight RandomWeight()
			{
				if (AllWeights.Count == 0) return null;
				for	(int i = 0; i < 100; i++)
				{
					Weight output = AllWeights[Random.Range(0, AllWeights.Count - 1)];
					if (output.IsRecurrent) continue;
					if (!output.IsActivaded) continue;
					return output;
				}
				return null;
			}
			Neuron CreateNewNeuron()
			{
				// get layer from input neuron from randWeight
				int newNeuronLayer = AllNeurons.Find(x => x.Id == randWeight.InId).Layer;
				return CreateNeuron(1, newNeuronLayer + 1);
			}
			void ConectNewWeightsTo_NewNeuron()
			{
				Weight weight1 = CreateWeight(newNeuron, AllNeurons.Find(x => x.Id == randWeight.OutId), false, false);
				weight1.WeightVal = randWeight.WeightVal;
				Weight weight = CreateWeight(AllNeurons.Find(x => x.Id == randWeight.InId), newNeuron, false, false);
			}
		}
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
				List<Weight> conectedWeights = AllWeights.FindAll(x => x.OutId == neuron.Id);
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
					weight.OutVal = weight.InVal * weight.WeightVal;
				}
			}
		}
	} 
	Weight CreateWeight(Neuron inNeuron, Neuron outNeuron, bool checkSameLayer, bool checkRecurent)
	{
		int weightId = GetId(inNeuron.Id, outNeuron.Id);
		Weight newWeight = new Weight(weightId, inNeuron.Id, outNeuron.Id, Random.Range(-20, 20));

		if (!WeightIsValide()) return null;
		if (IsRecurrent()) newWeight.IsRecurrent = true;

		AllWeights.Add(newWeight);
		return newWeight;

		int GetId(int inNeuronId, int outNeuronId)
		{
			return inNeuronId * 1000 + outNeuronId;
		}
		bool WeightIsValide()
		{
			if (WeightExists()) return false;
			if (IsSameNeuron()) return false;
			if (checkSameLayer)
				if (IsSameLayer()) return false;

			if (checkRecurent)
				if (IsRecurrent() && !AllowRecurentConnections) return false;

			return true;

			bool IsSameNeuron()
			{
				return inNeuron.Id == outNeuron.Id;
			}
			bool IsSameLayer()
			{
				return inNeuron.Layer == outNeuron.Layer;
			}
			bool WeightExists()
			{
				foreach (Weight weight in AllWeights)
				{
					if (weight.InId == inNeuron.Id && weight.OutId == outNeuron.Id) return true;
				}
				return false;
			}
		}
		bool IsRecurrent()
			{
				return inNeuron.Layer > outNeuron.Layer;
			}
	}
	Neuron CreateNeuron(int neuronType, int layer)
	{
		currNeuronId++;
		//SetLayer();
		Neuron newNeuron = new Neuron(currNeuronId, neuronType, layer);
		AddNeuronToLists();
		return newNeuron;

		void AddNeuronToLists()
		{
			AllNeurons.Add(newNeuron);

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
	} 
	bool RandomPers(float chance)
	{
		return Random.Range(0, 100) < chance;
	}
	void UpdateLayersAndWeights()
	{
		UpdateNeuronLayers();
		UpdateOutput_Layer();
		UpdateWeights();

		void UpdateNeuronLayers()
		{
			foreach (Neuron neuron in HiddenNeurons)
			{
				int longestPath = GetLongestPathLength(neuron.Id);
				if (longestPath > 0)
				{
					neuron.Layer = longestPath + 1;
				}

				int GetLongestPathLength(int startId)
				{
					int currLongestPath = 0;
					CheckPathOfId(startId, 0);
					return currLongestPath;

					void CheckPathOfId(int startId, int currLenght)
					{
						List<Weight> connectedWeights = GetConnectedWeights();

						if (connectedWeights.Count == 0)
						{
							if (currLongestPath < currLenght) currLongestPath = currLenght;
							return;
						}
						foreach (Weight weight in connectedWeights)
						{
							CheckPathOfId(weight.InId, currLenght + 1);
						}
						List<Weight> GetConnectedWeights()
						{
							List<Weight> output = new();
							
							foreach (Weight weight in AllWeights)
							{
								if (weight.OutId != startId) continue;
								if (weight.IsRecurrent) continue;
								output.Add(weight);
							}
							return output;
						}
					}
				}
			}
		}
		void UpdateWeights()
		{
			foreach (Weight currWeight in AllWeights)
			{
				Neuron inNeuron = AllNeurons.Find(x => x.Id == currWeight.InId);
				Neuron outNeuron = AllNeurons.Find(x => x.Id == currWeight.OutId);

				if (IsRecurent() && !AllowRecurentConnections)
				{
					currWeight.IsActivaded = false;
				}
				if (IsRecurent()) currWeight.IsRecurrent = true;
				else currWeight.IsRecurrent = false;
				if (IsSameNeuron())
				{
					RemoveNeuronOrWeight(currWeight);
					return;
				}
				if (IsSameLayer()) currWeight.IsActivaded = false;
				if(WeightExists())
				{
					RemoveNeuronOrWeight(currWeight);
					return;
				}

				bool IsRecurent()
				{
					return inNeuron.Layer > outNeuron.Layer;
				}
				bool IsSameNeuron()
				{
					return inNeuron.Id == outNeuron.Id;
				}
				bool IsSameLayer()
				{
					return inNeuron.Layer == outNeuron.Layer;
				}
				bool WeightExists()
				{
					bool found_currWeight = false;
					foreach (Weight weight1 in AllWeights)
					{
						if (weight1.Id != currWeight.Id) continue;

						if (found_currWeight) return true;
						found_currWeight = true;
					}
					return false;
				}
			}
		}
	}
	void UpdateOutput_Layer()
	{
		if (OutputNeurons.Count == 0) return;

		int hightesLayer = GetHighestHiddenLayer();
		if (hightesLayer + 1 == OutputNeurons[0].Layer) return;
		// if there are no hidden neurons
		if (hightesLayer == 0) hightesLayer = 1;
		
		foreach (Neuron outputNeuron in OutputNeurons)
		{
			outputNeuron.Layer = hightesLayer + 1;
		}

		int GetHighestHiddenLayer()
		{
			int hightesLayer = 0;

			foreach (Neuron hiddenNeuron in HiddenNeurons)
			{
				if (hiddenNeuron.Layer <= hightesLayer) continue;

				hightesLayer = hiddenNeuron.Layer;
			}
			return hightesLayer;
		}
	}
	void RemoveNeuronOrWeight(object objectToRemove)
	{
		Weight weightToRemove = objectToRemove as Weight;
		Neuron neuronToRemove = objectToRemove as Neuron;
		RemoveWeight(weightToRemove);
		RemoveNeuron(neuronToRemove);
		if (weightToRemove == null && neuronToRemove == null) return;
		UpdateLayersAndWeights();
		VisualNetwork.UpdateAll();

		void RemoveWeight(Weight weight)
		{
			if (weight == null) return;

			Neuron outputNeuron = HiddenNeurons.Find(x => x.Id == weight.OutId);
			AllWeights.Remove(weight);
			weight = null;
			if (outputNeuron == null) return;
			TryRemoveNeuron();

			
			void TryRemoveNeuron()
			{
				if (AllWeights.Exists(x => x.OutId == outputNeuron.Id && !x.IsRecurrent)) return;
				RemoveNeuron(outputNeuron);
			}
		}
		void RemoveNeuron(Neuron neuron)
		{
			if (neuron == null) return;

			RemoveConecktedWeights();
			RemoveNeuronFromLists();
			neuron = null;


			void RemoveConecktedWeights()
			{
				List<Weight> allConected_Weights = AllWeights.FindAll(x => x.OutId == neuron.Id);
				allConected_Weights.AddRange(AllWeights.FindAll(x => x.InId == neuron.Id));

				foreach (Weight conectedWeight in allConected_Weights)
				{
					RemoveWeight(conectedWeight);
				}
			}
			void RemoveNeuronFromLists()
			{
				AllNeurons.Remove(neuron);

				switch (neuron.Type)
				{
					case 0:
						InputNeurons.Remove(neuron);
						break;
					case 1:
						HiddenNeurons.Remove(neuron);
						break;
					case 2:
						OutputNeurons.Remove(neuron);
						break;
				}
			}
		}
	}
}
#if UNITY_EDITOR
[CustomEditor(typeof(NeuralNetwork))]
class NeuralNetworkEditor : Editor
{
	public override void OnInspectorGUI()
	{
		NeuralNetwork neuralNetwork = (NeuralNetwork)target;
		if (neuralNetwork == null) return;
		Undo.RecordObject(neuralNetwork, "Change NeuralNetwork");

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Mutate"))
		{
			neuralNetwork.Mutate();
		}

		if (GUILayout.Button("Remove Weight"))
		{
			neuralNetwork.Mutate();
		}
		EditorGUILayout.EndHorizontal();

	}
}
//base.OnInspectorGUI();
//DrawDefaultInspector();
#endif