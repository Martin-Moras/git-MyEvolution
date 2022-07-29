using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(NeuralNetwork))]
class NeuralNetworkEditor : Editor
{
	private int removeWeightId;
	private int removeNeuronId;

	public override void OnInspectorGUI()
	{
		NeuralNetwork neuralNetwork = (NeuralNetwork)target;
		if (neuralNetwork == null) return;
		Undo.RecordObject(neuralNetwork, "Change NeuralNetwork");

		DrawStartNeuronAmount();

		EditorGUILayout.BeginHorizontal();
		Label("Weight connection persentage", false, true);
		neuralNetwork.WeightConnectionPersentige = EditorGUILayout.FloatField(neuralNetwork.WeightConnectionPersentige);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		#region Mutation
		
		#region Remove weight with Id
		EditorGUILayout.BeginHorizontal();
		Label("Id", false, true);
		removeWeightId = EditorGUILayout.IntField(removeWeightId);
		if (GUILayout.Button("Remove Weight"))
		{
			neuralNetwork.RemoveWeight_withId(removeWeightId);
		}
		EditorGUILayout.EndHorizontal();
		#endregion
		#region Remove neuron with Id
		EditorGUILayout.BeginHorizontal();
		Label("Id", false, true);
		removeNeuronId = EditorGUILayout.IntField(removeNeuronId);
		if (GUILayout.Button("Remove neuron"))
		{
			neuralNetwork.RemoveNeuron_withId(removeNeuronId);
		}
		EditorGUILayout.EndHorizontal();
		#endregion
		if (GUILayout.Button("Mutate"))
		{
			neuralNetwork.Mutate();
		}
		#region Add
		#region Add weight
		EditorGUILayout.BeginHorizontal();
			Label("Add weight chance", false, true, 54);
			Label("Attempts to add a weight");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
			neuralNetwork.AddWeightChance = EditorGUILayout.FloatField(neuralNetwork.AddWeightChance);
			neuralNetwork.WeightCrationAttempts = EditorGUILayout.IntField(neuralNetwork.WeightCrationAttempts);
		EditorGUILayout.EndHorizontal();
		#endregion
		DisplayFloat(ref neuralNetwork.AddNeuronChance, "Add Neuron chance");
		DisplayFloat(ref neuralNetwork.ChangeActivationFunkChance, "Change activation funktion chance");
		#endregion
		#region Remove
		Label("Remove");

		DisplayFloat(ref neuralNetwork.RemoveWeightChance, "Remove Weight chance");
		DisplayFloat(ref neuralNetwork.RemoveNeuronChance, "Remove Neuron chance");

		EditorGUILayout.Space();
		#endregion
		#region Activate Weight
		EditorGUILayout.BeginHorizontal();
		neuralNetwork.WeightReactivateChance = EditorGUILayout.FloatField(neuralNetwork.WeightReactivateChance);
		EditorGUILayout.LabelField("Anable weight chance");
		EditorGUILayout.EndHorizontal();
		#endregion
		#region Disable weight;
		EditorGUILayout.BeginHorizontal();
		neuralNetwork.DisableWeightChance = EditorGUILayout.FloatField(neuralNetwork.DisableWeightChance);
		Label("Disable weight chance");
		EditorGUILayout.EndHorizontal();
		#endregion
		#region Change weight value
		EditorGUILayout.BeginHorizontal();
		Label("Value chnge chance", false, true, -5);
		Label("Value change range",false ,true, -5);
		Label("Value reset chance");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		neuralNetwork.WeightMutationCance = EditorGUILayout.FloatField(neuralNetwork.WeightMutationCance);
		neuralNetwork.WeightChangeAmount = EditorGUILayout.FloatField(neuralNetwork.WeightChangeAmount);
		neuralNetwork.WeightResetCance = EditorGUILayout.FloatField(neuralNetwork.WeightResetCance);
		EditorGUILayout.EndHorizontal();
		#endregion
		#region Allow recurent weights
		EditorGUILayout.BeginHorizontal();
		neuralNetwork.AllowRecurentWeights = EditorGUILayout.Toggle(neuralNetwork.AllowRecurentWeights);
		EditorGUILayout.LabelField("Allow recurent weights");
		EditorGUILayout.EndHorizontal();
		#endregion
		#endregion

		void DrawStartNeuronAmount()
		{
			EditorGUILayout.BeginHorizontal();
			Label("In neuron amount", false, true);
			Label("Hidden neuron amount", false, true);
			Label("Out neuron amount", false, true);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			neuralNetwork.InputNeuronAmount = EditorGUILayout.IntField(neuralNetwork.InputNeuronAmount);
			neuralNetwork.HiddenNeuronAmount = EditorGUILayout.IntField(neuralNetwork.HiddenNeuronAmount);
			neuralNetwork.OutputNeuronAmount = EditorGUILayout.IntField(neuralNetwork.OutputNeuronAmount);
			EditorGUILayout.EndHorizontal();

		}
	}
	

	private void Label(string content, bool defaultLength = true, bool autoLength = false, float extraSize = 0, float minSize = 0)
	{
		if (defaultLength)
		{
			EditorGUILayout.LabelField(content);
			return;
		}

		float size;

		size = GetAutoSize();
		size += extraSize;

		EditorGUILayout.LabelField(content, GUILayout.MaxWidth(size), GUILayout.MinWidth(size));

		float GetAutoSize()
		{
			if (!autoLength) return 0;

			return content.Length * 6.8f;
		}
	}
	private void DisplayInt(ref int value, string text = "")
	{
		EditorGUILayout.BeginHorizontal();
		value = EditorGUILayout.IntField(value);
		Label(text);
		EditorGUILayout.EndHorizontal();
	}
	private void DisplayFloat(ref float value, string text = "")
	{
		EditorGUILayout.BeginHorizontal();
		value = EditorGUILayout.DelayedFloatField(value);
		Label(text);
		EditorGUILayout.EndHorizontal();
	}
}
//base.OnInspectorGUI();
//DrawDefaultInspector();