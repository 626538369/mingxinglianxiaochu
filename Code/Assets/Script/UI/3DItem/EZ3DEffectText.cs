using UnityEngine;
using System.Collections;

public class EZ3DEffectText : EZ3DItem 
{
	void Awake()
	{
		_mEffectText = this.GetComponent<PackedSprite>() as PackedSprite;
	}
	
	public override void SetValue(params object[] args)
	{
		_mEffectText.PlayAnim((string)args[0]);
	}
	
	public override void Reposition(Vector3 worldPos)
	{
		// Vector3 objVpPosInMainCam = Globals.Instance.MSceneManager.mMainCamera.WorldToViewportPoint(worldPos);
		// Vector3 objVpPosInEZCam = Globals.Instance.MGUIManager.MGUICamera.WorldToViewportPoint(worldPos);
		
		// Vector3 objScreenPosInMainCam = Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(worldPos);
		// // Vector3 objScreenPosInEZCam = Globals.Instance.MGUIManager.MGUICamera.WorldToScreenPoint(worldPos);
		// 
		// float screenWidth = Screen.width;
		// float screenHeight = Screen.height;
		// 
		// Vector3 objScreenPos = Vector3.zero;
		// // objScreenPos.x = (objVpPosInMainCam.x - 0.5f) * screenWidth;
		// // objScreenPos.y = (objVpPosInMainCam.y - 0.5f) * screenHeight;
		// 
		// objScreenPos.x = objScreenPosInMainCam.x - 0.5f * screenWidth;
		// objScreenPos.y = objScreenPosInMainCam.y - 0.5f * screenHeight;
		// objScreenPos.z = 100.0f;
		
		// _mText.transform.localPosition = objScreenPos;
		
		Vector3 guiPos = GUIManager.WorldToGUIPoint(worldPos);
		this.transform.localPosition = new Vector3(guiPos.x, guiPos.y, GUIManager.GUI_FARTHEST_Z);
	}
	
	private PackedSprite _mEffectText;
}
