using UnityEngine;
using System.Collections;

public enum ScaleDirect
{
	Bigger = 1,
	Smaller = -1,
}
public class ScaleButton : AutoAddEventTrigger 
{
	public ScaleDirect direct;

	private BaseObj originObj;
	private EditorBaseObj targetObj;

	System.Action<Vector3> ScaleChange;

	private readonly float limitBigger = 3.0F;
	private readonly float limitSmaller = 0.5F;

	public void SetData(BaseObj originObj, System.Action<Vector3> ScaleChange)
	{
		this.originObj = originObj;
		this.targetObj = targetObj;
		this.ScaleChange = ScaleChange;
	}

	public override void OnMouseDown()
	{
		Debug.Log ("linbaBaseObj.type = " + originObj.type);

		float oneClickPoint = 0.1F;

		Vector3 scales = originObj.transform.localScale;
		float add = (int)direct * oneClickPoint;
		float newScale = scales.x + add;

		float adjustNewScale = Mathf.Round (newScale * 10) / 10;

		if (adjustNewScale > limitBigger ||
		    adjustNewScale < limitSmaller)
		{
			return;
		}

		Vector3 newPoint = new Vector3 (adjustNewScale, adjustNewScale, adjustNewScale);

		ScaleChange (newPoint);
	}
}
