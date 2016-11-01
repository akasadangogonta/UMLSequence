using UnityEngine;
using System.Collections;

public class EditorWindowMovingController : MonoBehaviour 
{
	protected long id;
	private Color[] defaultColor;
	
	private MouseEvent mouseEvent = MouseEvent.None;
	private double touchMoment;
	private float doubleClickLimit = 0.5F;
	
	private bool alreadyLoop = false;
	
	private Vector2 originPos;
	
	private Vector2 beforeMouse;
	private Vector2 afterMouse;
	private float moveX, moveY;

	protected long GetCurId { get { return GeneralController.GetCurId; } }
	protected float GetScale { get { return GeneralController.GetScale; } }

	void Awake () 
	{
		touchMoment = Time.time;

		//originPos = new Vector2 (395F, 300);//.gameObject.transform.position;
		//this.gameObject.transform.position = originPos;
		
		beforeMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		afterMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		moveX = 0;
		moveY = 0;
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			PushEvent(MouseEvent.Left);
		} 
		else if (Input.GetMouseButtonDown (1))
		{
			PushEvent(MouseEvent.Right);
		} 
		else if (Input.GetMouseButtonDown (2)) 
		{
			PushEvent(MouseEvent.Center);
		}
	}
	private void PushEvent(MouseEvent mouseEvent)
	{
		if (OutArea ()) return;	
		this.mouseEvent = mouseEvent;
	}
	
	private bool OutArea()
	{
		Vector3 curMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 targetPos = this.transform.position;
		Vector2 targetSize = this.GetComponent<RectTransform> ().sizeDelta;
		
		if (curMousePos.x > targetPos.x - ((targetSize.x * GetScale) / 2) &&
		    curMousePos.x < targetPos.x + ((targetSize.x * GetScale) / 2) &&
		    curMousePos.y > targetPos.y - ((targetSize.y * GetScale) / 2) &&
		    curMousePos.y < targetPos.y + ((targetSize.y * GetScale) / 2))
		{
			return false;
		}
		return true;
	}
	
	public void OnMouseDown()
	{
		if (mouseEvent == MouseEvent.None)
		{
			if(alreadyLoop == false)
			{
				Invoke ("OnMouseDown", 0.01F);
				alreadyLoop = true;
				return;
			}
		}
		else if (mouseEvent == MouseEvent.Left)
		{
			beforeMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}
		else if (mouseEvent == MouseEvent.Right)
		{
			beforeMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}
		
		alreadyLoop = false;
	}
	
	public void OnMouseUp()
	{
		/*
		originPos = this.gameObject.transform.position;
		*/
	}
	
	public void OnDrug()
	{
		/*
		if (mouseEvent == MouseEvent.Left ||
		    mouseEvent == MouseEvent.Right)
		{
			MoveEditorWindow (this.gameObject);
		}
		*/
	}

	//inner process
	protected void MoveEditorWindow(GameObject targetObj)
	{
		/*
		afterMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (beforeMouse.x != afterMouse.x ||
		    beforeMouse.y != afterMouse.y) 
		{
			moveX = (beforeMouse.x - afterMouse.x);
			moveY = (beforeMouse.y - afterMouse.y);
			
			this.gameObject.transform.position = new Vector2 (originPos.x - (moveX * 50), originPos.y - (moveY * 50));

		}
		*/
	}
}
