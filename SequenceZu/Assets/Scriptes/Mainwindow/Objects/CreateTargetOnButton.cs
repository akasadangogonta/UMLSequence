using UnityEngine;
using System.Collections;

public class CreateTargetOnButton : MonoBehaviour {

	public GameObject CreateTargetObj;
	public GameObject setParentTarget;

	protected long GetCurId { get { return GeneralController.GetCurId; } }

	public void ClickButton()
	{
		GeneralController.SetCurId (GetCurId + 1);
		
		InstantiateObj ();
	}

	private void InstantiateObj()
	{
		GameObject instanceTargetObj = Instantiate (CreateTargetObj) as GameObject;

		instanceTargetObj.GetComponent<ButtonTranspoter>().SetDataTranspoter();
		if (setParentTarget == null) 
		{
			Debug.LogWarning("setParentTarget is Null!!");
			return;
		}
		instanceTargetObj.transform.SetParent(setParentTarget.transform);
		instanceTargetObj.transform.localPosition = new Vector3 (0, 0, 0);
		instanceTargetObj.transform.localScale = new Vector2 (1, 1);
	}

}
