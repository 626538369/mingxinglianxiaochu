using UnityEngine;
using System.Collections;

public class CameraSmoothFollow : CameraBehaviour 
{
	// LateUpdate is called once per frame
	public override void LateUpdate () 
	{	
		if (isUseCoroutine)
			return;
		
		if (!enabled)
			return;
		
		if (null == targetTransform)
			return;
		
		float wantedHeight = targetTransform.position.y + height;
		float currentHeight = Globals.Instance.MSceneManager.mMainCamera.transform.position.y;
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		float wantedRotationAngle = targetTransform.eulerAngles.y;
		float currentRotationAngle = Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles.y;
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		Globals.Instance.MSceneManager.mMainCamera.transform.position = targetTransform.position;
		
		Globals.Instance.MSceneManager.mMainCamera.transform.position -= currentRotation * Vector3.forward * distance;
	
		// Set the height of the camera
		Globals.Instance.MSceneManager.mMainCamera.transform.position = new Vector3(Globals.Instance.MSceneManager.mMainCamera.transform.position.x, currentHeight, Globals.Instance.MSceneManager.mMainCamera.transform.position.z);
		
		// Always look at the target
		Globals.Instance.MSceneManager.mMainCamera.transform.LookAt (targetTransform);
	}
	
	// The target we are following
	private Transform targetTransform = null;
	
	// The distance in the x-z plane to the target
	private float distance = 10.0f;
	
	// the height we want the camera to be above the target
	private float height = 5.0f;
	
	// How much we 
	private float heightDamping = 2.0f;
	private float rotationDamping = 5.0f;
}
