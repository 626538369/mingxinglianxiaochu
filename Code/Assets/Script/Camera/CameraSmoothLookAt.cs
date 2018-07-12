using UnityEngine;
using System.Collections;

public class CameraSmoothLookAt : CameraBehaviour 
{
	public Vector3 PLookAtPosition
	{
		get { return _lookAtPosition; }
		set { _lookAtPosition = value; }
	}
	
	public float PRotateDamping
	{
		get { return _rotationDamping; }
		set { _rotationDamping = value; }
	}
	
	// Update is called once per frame
	public override void LateUpdate () 
	{
		if (isUseCoroutine)
			return;
		
		if (!enabled)
			return;
		
		Vector3 currentPosition = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		Quaternion rotation = Quaternion.LookRotation(_lookAtPosition, currentPosition);
		
		Quaternion currentRotation = Globals.Instance.MSceneManager.mMainCamera.transform.rotation;
		Globals.Instance.MSceneManager.mMainCamera.transform.rotation = Quaternion.Slerp(currentRotation, rotation, _rotationDamping * Time.deltaTime);
	}
	
	private float _rotationDamping = 0.4f;
	private Vector3 _lookAtPosition = Vector3.zero;
}
