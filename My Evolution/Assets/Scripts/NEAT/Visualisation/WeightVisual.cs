using UnityEngine;

public class WeightVisual : MonoBehaviour
{
	[SerializeField] Color Activated;
	[SerializeField] Color Deactivated;
	[SerializeField] Color Recurent;
	public int Id;
	public int InId;
	public int OutId;
	private bool isActiv = true;
	public bool IsActivated
	{
		get
		{
			return isActiv;
		}
		set
		{
			isActiv = value;
			UpdateColor();
		}
	}
	private bool isRecur = true;
	public bool IsRecurent
	{
		get
		{
			return isRecur;
		}
		set
		{
			isRecur = value;
			UpdateColor();
		}
	}

	private void UpdateColor()
	{
		if (!isActiv)
		{
			GetComponent<LineRenderer>().startColor = Deactivated;
			GetComponent<LineRenderer>().endColor = Deactivated;
			return;
		}
		if (isRecur)
		{
			GetComponent<LineRenderer>().startColor = Recurent;
			GetComponent<LineRenderer>().endColor = Recurent;
			return;
		}
		if (isActiv)
		{
			GetComponent<LineRenderer>().startColor = Activated;
			GetComponent<LineRenderer>().endColor = Activated;
			return;
		}
	}
}