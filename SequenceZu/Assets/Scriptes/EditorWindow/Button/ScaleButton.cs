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
	private EditorTargetControllBase targetObj;

	System.Action<Vector3> ScaleChange;

	private readonly float limitBigger = 2.0F;
	private readonly float limitSmaller = 0.1F;

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

		//float adjustAngleZ = Mathf.Floor (angles.z * 10) / 10;

		float add = (int)direct * oneClickPoint;
		if ((scales.x + add) > limitBigger ||
			(scales.x + add) < limitSmaller)
		{
			return;
		}

		Vector3 newPoint = new Vector3 (scales.x + add, scales.y + add, scales.z);

		ScaleChange (newPoint);
	}
}
