using UnityEngine;
using System.Collections;

public abstract class Stage2D : MonoBehaviour 
{
	// Adjust the z value
	protected virtual void Awake()
	{
		if (null != Globals.Instance.MGUIManager)
		{
			Vector3 camPos = Globals.Instance.MGUIManager.MGUICamera.transform.position;
			float farClip = Globals.Instance.MGUIManager.MGUICamera.far;
			transform.position = new Vector3(camPos.x, camPos.y, camPos.z + farClip - 100.0f);
		}
		
		if (null != Globals.Instance.MSceneManager.mMainCamera)
		{
			Globals.Instance.MSceneManager.mMainCamera.cullingMask = 0;
		}
	}
	
	public abstract void Init();
}
