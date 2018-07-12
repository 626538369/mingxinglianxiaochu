using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravityInductor : MonoBehaviour
{
	private float force = 4.0f;
	private bool simulateAccelerometer = false;
	
	// void Start()
	// {
	// 	// make landscape view
	// 	iPhoneSettings.screenOrientation = iPhoneScreenOrientation.Landscape;
	// }
	
	void Update () 
	{
		bool isAllowsimulate = GameStatusManager.Instance.MGameState == GameState.GAME_STATE_PORT;
		isAllowsimulate = false; // Close it
		if (!isAllowsimulate)
			return;
		
		Vector3 dir = Vector3.zero;

#if UNITY_EDITOR || !(UNITY_IPHONE || UNITY_ANDROID)
		// using joystick input instead of iPhone accelerometer
		// dir.x = Input.GetAxis("Vertical");
		dir.y = Input.GetAxis("Horizontal");
		
#elif UNITY_ANDROID || UNITY_IPHONE
		if (GameDefines.Setting_Gravity && Mathf.Abs(Input.acceleration.y) > 0.1f)
		{
			// we assume that device is held parallel to the ground
			// and Home button is in the right hand
			
			// remap device acceleration axis to game coordinates
			// 1) XY plane of the device is mapped onto XZ plane
			// 2) rotated 90 degrees around Y axis
			
			dir.y = -Input.acceleration.y;
			// dir.x = Input.acceleration.x;
			
			//Debug.Log("\nInput.acceleration.x === " + Input.acceleration.x 
			//	+ "\nInput.acceleration.y === " + Input.acceleration.y);
			
			// clamp acceleration vector to unit sphere
			if (dir.sqrMagnitude > 0.6f)
			{
				dir.Normalize();
			}
			else
			{
				dir = Vector3.zero;
			}
		}
#endif
		
		// Debug.Log("dir.sqrMagnitude = " + dir.sqrMagnitude.ToString() + 
		// 	"\nInput.acceleration.x === " + Input.acceleration.x
		// 	 + "\nInput.acceleration.y === " + Input.acceleration.y);
		// 
		if (!dir.Equals(Vector3.zero))
		{
			dir *= force;
			Globals.Instance.MSceneManager.mMainCamera.transform.Rotate(dir, Space.World);
		}
	}
	
	// void OnGUI()
	// {
	// 	GUI.TextField(new Rect(20, 40, 100, 20), string.Format("{0:0.000}", Input.acceleration.x));
	// 	GUI.TextField(new Rect(20, 60, 100, 20), string.Format("{0:0.000}", Input.acceleration.y));
	// 	GUI.TextField(new Rect(20, 80, 100, 20), string.Format("{0:0.000}", Input.acceleration.z));
	// }
}
	