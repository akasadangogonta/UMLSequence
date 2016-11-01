using UnityEngine;
using System.Collections;

public enum TurnDirect
{
	Right = -1,
	Left = 1,
}
public class AngleButton : AutoAddEventTrigger 
{
	public TurnDirect direct;

	private EditorBaseObj targetObj;

	System.Action<float> AngleChange;

	public void SetData(EditorBaseObj targetObj, System.Action<float> AngleChange)
	{
		this.targetObj = targetObj;
		this.AngleChange = AngleChange;
	}

	public override void OnMouseDown()
	{
		Debug.Log ("linbaBaseObj.type = " + targetObj.type);

		float oneClickPoint = 22.5F;

		var angles = targetObj.transform.rotation.eulerAngles;
		float adjustAngleZ = Mathf.Floor (angles.z * 10) / 10;

		float newPoint = adjustAngleZ + ((int)direct * oneClickPoint);

		AngleChange (newPoint);
	}
}
