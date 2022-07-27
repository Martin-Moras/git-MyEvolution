using System.Collections.Generic;
using UnityEngine;

public class LayerVisual : MonoBehaviour
{
	public int Layer;
	private List<GameObject> NeuronsInLayer = new();

	public void AddNeuron(GameObject neuron, float neuronDistance, bool updateNeuronPos)
	{
		NeuronsInLayer.Add(neuron);
		if (updateNeuronPos) UpdateNeuronPos(neuronDistance);
	}
	private void UpdateNeuronPos(float neuronDistance)
	{
		float firstPos = (NeuronsInLayer.Count - 1) * neuronDistance / 2 * -1;

		for (int i = 0; i < NeuronsInLayer.Count; i++)
		{
			if (IsNull()) continue;

			Vector3 neuronPos = Vector3.up * (firstPos + (neuronDistance * i));
			NeuronsInLayer[i].transform.localPosition = neuronPos;
			
			
			bool IsNull()
			{
				if (NeuronsInLayer[i] != null) return false;

				NeuronsInLayer.RemoveAt(i);
				return true;
			}
		}
	}
}
