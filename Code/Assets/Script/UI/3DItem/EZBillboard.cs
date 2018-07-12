using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EZBillboard : MonoBehaviour 
{
	// Update is called once per frame
	private  Vector3 normal;
	public float ratio = 1.0f;
	public float EZBillboardDistance = 200.0f;
	protected bool first  = true;
	void Update () 
	{
		if(first)
		{
			first  = false;
			normal = this.transform.localScale;
		}
		// LookAt the camera direction
		//this.transform.rotation = Globals.Instance.MSceneManager.mMainCamera.transform.rotation;
		//this.transform.forward =-Globals.Instance.MSceneManager.mMainCamera.transform.forward;
		this.transform.forward = this.transform.position - Globals.Instance.MSceneManager.mMainCamera.transform.position;
		float Distance = Vector3.Magnitude(Globals.Instance.MSceneManager.mMainCamera.transform.position - this.transform.position);
		this.transform.localScale = normal * (Distance / EZBillboardDistance) * ratio;
	
	}
}
