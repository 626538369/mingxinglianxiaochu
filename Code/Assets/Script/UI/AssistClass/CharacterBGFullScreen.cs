using UnityEngine;
using System.Collections;

public class CharacterBGFullScreen : MonoBehaviour 
{

	private Transform target;
	int width ;
	int hight ;

	void Awake()
	{
		target = transform;
	}
	void Start ()
	{
		UITexture uiTexture = this.transform.GetComponent<UITexture>();
		width = uiTexture.width;
		hight = uiTexture.height;
		Globals.Instance.MSceneManager.mTaskCamera.ResetAspect();
		float tanCamera = Globals.Instance.MSceneManager.mTaskCamera.aspect;
		if (tanCamera < GUIManager.DEFAULT_SCREEN_WIDTH / GUIManager.DEFAULT_SCREEN_HEIGHT)
		{
			float fovValue = Globals.Instance.MSceneManager.mTaskCamera.fieldOfView;
			float lengthDis = Vector3.Distance(Globals.Instance.MSceneManager.mTaskCamera.transform.position,target.position);
			
			float frustumHeight = 2.0f * lengthDis * Mathf.Tan(fovValue * 0.5f * Mathf.Deg2Rad);
			float frustumWidth = frustumHeight * tanCamera;
			
			float unitWeight = Mathf.Abs(frustumHeight*uiTexture.aspectRatio);
			
			
			uiTexture.width = (int)unitWeight + 10;
			uiTexture.height = (int)frustumHeight + (int)(10/tanCamera);
			foreach(Transform data in this.transform)
			{
				UITexture inTexture = data.GetComponent<UITexture>();
				if (inTexture != null)
				{
					inTexture.width = (int)(uiTexture.width/width * inTexture.width);
					inTexture.height = (int)(uiTexture.height/hight * inTexture.height);
				}
			}
		}
		else{
			float fovValue = Globals.Instance.MSceneManager.mTaskCamera.fieldOfView;
			float lengthDis = Vector3.Distance(Globals.Instance.MSceneManager.mTaskCamera.transform.position,target.position);
			
			float frustumHeight = 2.0f * lengthDis * Mathf.Tan(fovValue * 0.5f * Mathf.Deg2Rad);
			float frustumWidth = frustumHeight * tanCamera;
			
			float unitHeight = Mathf.Abs(frustumWidth/uiTexture.aspectRatio);
			
			
			uiTexture.width = (int)frustumWidth + 10;
			uiTexture.height = (int)unitHeight + (int)(10/tanCamera);
			foreach(Transform data in this.transform)
			{
				UITexture inTexture = data.GetComponent<UITexture>();
				if (inTexture != null)
				{
					inTexture.width = (int)(uiTexture.width/width * inTexture.width);
					inTexture.height = (int)(uiTexture.height/hight * inTexture.height);
				}
			}
		}
	}
	public void Reset()
	{
		Start ();
	}

}