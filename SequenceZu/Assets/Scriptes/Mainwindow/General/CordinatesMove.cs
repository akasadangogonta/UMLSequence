using UnityEngine;
using System.Collections;

public class CordinatesMove : AutoAddEventTrigger {
	private GameObject MainCamera;
	private GameObject createArea;

	private MouseEvent mouseEvent = MouseEvent.None;

	private Vector2 originPosCamera;
	private Vector2 originPosArea;

	private Vector2 beforeMouse;
	private Vector2 afterMouse;
	private float moveX, moveY;

	private bool alreadyLoop = false;

	protected float GetScale { get { return GeneralController.GetScale; } }

	override protected void AwakeMain () 
	{
		MainCamera = GameObject.Find ("/Main Camera");
		createArea = GameObject.Find ("/MainCanvas/CreateArea");

		originPosCamera = MainCamera.transform.position;
		originPosArea = createArea.transform.position;

		beforeMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		afterMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		moveX = 0;
		moveY = 0;
	}
	
	override protected void Start()
	{
		base.Start ();
	}

	void Update () 
	{
		if (Input.GetMouseButtonDown (2)) 
		{
			PushEvent(MouseEvent.Center);
		}
	}
	private void PushEvent(MouseEvent mouseEvent)
	{
		this.mouseEvent = mouseEvent;
	}

	override public void OnMouseDown()
	{ 
		if (mouseEvent == MouseEvent.None) 
		{
			if (alreadyLoop == false)
			{
				Invoke ("OnMouseDown", 0.01F);
				alreadyLoop = true;
				return;
			}
		}
		else if (mouseEvent == MouseEvent.Center)
		{
			originPosCamera = MainCamera.transform.position;
			beforeMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		}
		
		alreadyLoop = false;
	}

	override public void OnMouseUp()
	{
		mouseEvent = MouseEvent.None;

		createArea.transform.position = new Vector2 (originPosArea.x, originPosArea.y);
		MainCamera.transform.position = new Vector3(originPosCamera.x + (moveX), originPosCamera.y + (moveY), -20);
	}

	override public void OnDrag()
	{
		if (mouseEvent == MouseEvent.Center)
		{
			Move ();
		}
	}

	private void Move()
	{
		afterMouse = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (beforeMouse.x != afterMouse.x ||
			beforeMouse.y != afterMouse.y) 
		{
			moveX = (beforeMouse.x - afterMouse.x);
			moveY = (beforeMouse.y - afterMouse.y);

			createArea.transform.position = new Vector2 (originPosArea.x - moveX, originPosArea.y - moveY);
			//MainCamera.transform.position = new Vector2(originPos.x + moveX, originPos.y + moveY );
		}
	}

}
