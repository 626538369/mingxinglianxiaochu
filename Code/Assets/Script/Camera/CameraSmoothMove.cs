using UnityEngine;
using System.Collections;

public enum MoveMethod
{
	METHOD_FIXED_POSITION,
	METHOD_FIXED_DIRECTION,
}

public class CameraSmoothMove : CameraBehaviour 
{
	#region Property
	public float MoveSpeed
	{
		get { return _moveSpeed; }
		set { _moveSpeed = value; }
	}
	
	public Vector3 MoveDestPosition
	{
		get { return _moveDestPosition; }
		set 
		{ 
			_isMoving = true;
			_moveDestPosition = value; 
		}
	}
	
	public Vector3 MoveDirection
	{
		get { return _moveDirection; }
		set { _moveDirection = value; }
	}
	#endregion
	
	// Update is called once per frame
	public override void LateUpdate () 
	{
		if (isUseCoroutine)
			return;
		
		if (!enabled)
			return;
		
		if (!_isMoving)
			return;
		
		switch (_moveMethod)
		{
		case MoveMethod.METHOD_FIXED_POSITION:
		{
			float distance = Vector3.Distance(Globals.Instance.MSceneManager.mMainCamera.transform.position, _moveDestPosition);
			float squareMovement = _moveSpeed * Time.deltaTime;
			squareMovement *= squareMovement;
			if (distance <= squareMovement)
			{
				_isMoving = false;
				
				// Globals.Instance.MSceneManager.mMainCamera.transform.rotation = wantedRotation;
				Globals.Instance.MSceneManager.mMainCamera.transform.position = _moveDestPosition;
				
				return;
			}
			
			Quaternion currentRotation = Globals.Instance.MSceneManager.mMainCamera.transform.rotation;
			Vector3 currentDirection = Globals.Instance.MSceneManager.mMainCamera.transform.forward;
			
			Vector3 currentPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
			Vector3 destDirection = _moveDestPosition - currentPos;
			destDirection.Normalize();
				
			Quaternion destRotation = Quaternion.FromToRotation(currentDirection, destDirection);
			destRotation = currentRotation * destRotation;
			
			float rotateDamping = 0.5f;
			Quaternion wantedRotation = Quaternion.Slerp(currentRotation, destRotation, rotateDamping * Time.deltaTime);
			
			Vector3 wantedPosition = currentPos;
			wantedPosition += wantedRotation * Vector3.forward * _moveSpeed * Time.deltaTime;
			// wantedPosition += destDirection * _moveSpeed * Time.deltaTime;
			
			Globals.Instance.MSceneManager.mMainCamera.transform.rotation = wantedRotation;
			Globals.Instance.MSceneManager.mMainCamera.transform.position = wantedPosition;
			
			break;
		}
		case MoveMethod.METHOD_FIXED_DIRECTION:
		{
			// Local coordinate system x & y & z direction
			Vector3 offset = _moveDirection * _moveSpeed * Time.deltaTime;
			Globals.Instance.MSceneManager.mMainCamera.transform.Translate(offset, Space.World);
			break;
		}
			
		}
		
	}
	
	public void Reset()
	{
		_isMoving = false;
		
		// Local coordinate system x & y & z direction
		_moveDirection = Vector3.zero;
		_moveDestPosition = Vector3.zero;
		
		// _moveDirection += Globals.Instance.MSceneManager.mMainCamera.transform.forward;
		// _moveDirection += Globals.Instance.MSceneManager.mMainCamera.transform.right;
		// _moveDirection += Globals.Instance.MSceneManager.mMainCamera.transform.up;
		// 483.2611 157.8072 -110.9078
		// 46.9128 0 0 
		// 483.2611 254.1271 -35.7297
	}
	
	private bool _isMoving;
	
	private MoveMethod _moveMethod = MoveMethod.METHOD_FIXED_POSITION;
	private float _moveSpeed = 1.0f;
	private Vector3 _moveDestPosition;
	private Vector3 _moveDirection;
}
