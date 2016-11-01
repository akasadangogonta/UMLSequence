using UnityEngine;
using System.Collections;

public class HideEditor : MonoBehaviour {
	public void OnClick()
	{
		GeneralController.instanceThisGameObject.GetComponent<InvokeEditorWindow> ().HideEditorWindow ();
		GeneralController.instanceThisGameObject.GetComponent<InvokeEditorWindow> ().linkBaseObj.UpdateObjectsDataToSaveData ();
	}
}
