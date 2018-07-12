using UnityEngine;
using System.Collections;

public class AIChase : AIBase 
{
	public const float INTERVAL = 0.1f;
	
	//--------------------------------------------
	public GameObject TargetGameObj
	{
		set 
		{ 
			targetGameObj = value; 
			
			monitorInterval = 0.0f;
			MoveTo(targetGameObj.transform.position);
		}
	}
	//--------------------------------------------
	
	//--------------------------------------------
	public float Speed
	{
		set { speed = value; }
	}
	//--------------------------------------------
	
	
	protected override void Awake()
	{
		monitorInterval = 0.0f;
	}
	
	protected override void OnDestroy()
	{}
	
	protected override void Update()
	{
		if (!isEnabled)
			return;
		
		if (null == controlGameObj|| null == targetGameObj)
			return;
		
		if (monitorInterval >= INTERVAL)
		{
			// Adjust the monitor transform
			monitorInterval = 0.0f;
			MoveTo(targetGameObj.transform.position);
		}
		monitorInterval += Time.deltaTime;
		UpdateMovement();
	}
	
	public override void Interrupt()
	{
		base.Interrupt();
	}
	
	public override void Restart()
	{
		base.Restart();
	}
	
	public override void Resume()
	{
		base.Resume();
	}
	
	private void MoveTo(Vector3 pos)
	{
		// Calculate move direction
		Vector3 currentPosition = controlGameObj.transform.position;
		destination = pos;
		direction = pos - currentPosition;
		direction.Normalize();
	}
	
	private void UpdateMovement()
	{
		float rotationDamping = 1.5f;
		
		Vector3 currentPosition = controlGameObj.transform.position;
		Vector3 wantedPosition = currentPosition + direction * speed * Time.deltaTime;
		
		// Slowly rotate
		Quaternion currentRotation = controlGameObj.transform.rotation;
		Quaternion destRotation = Quaternion.FromToRotation(controlGameObj.transform.right, direction);
		destRotation = currentRotation * destRotation;
		currentRotation = Quaternion.Lerp(currentRotation, destRotation, Time.deltaTime * rotationDamping);
		currentRotation.eulerAngles = new Vector3(0, currentRotation.eulerAngles.y, 0);
		
		controlGameObj.transform.position = wantedPosition;
		controlGameObj.transform.rotation = currentRotation;
	}
	
	private GameObject targetGameObj;
	
	private float speed;
	private float monitorInterval;
	private Vector3 direction;
	private Vector3 destination;
}
