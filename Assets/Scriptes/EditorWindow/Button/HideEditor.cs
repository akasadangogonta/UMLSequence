using UnityEngine;
using System.Collections;

public class HideEditor : MonoBehaviour {
	public void OnClick()
	{
		GeneralController.instanceThisGameObject.GetComponent<CreateEditorWindow> ().HideEditorWindow ();
		GeneralController.instanceThisGameObject.GetComponent<CreateEditorWindow> ().linkBaseObj.UpdateObjectsDataToSaveData ();
	}
}
