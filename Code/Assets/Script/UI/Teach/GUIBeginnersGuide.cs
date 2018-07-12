using UnityEngine;
using System.Collections;

public class GUIBeginnersGuide : GUIWindow {
	
	public GameObject ArrowBootMode;
	public GameObject BoxLeft;
	public GameObject BoxRight;
	public GameObject BoxUp;
	public GameObject BoxDown;
	public UISprite Arrow;

	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		_mIsLoaded = true;
		
		this.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		
		this.transform.localScale = Vector3.one;
		
		base.GUILevel = 0;
	}
	
	public override void SetVisible (bool visible)
	{
		base.SetVisible (visible);
	}
	public static void Show()
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIBeginnersGuide>(delegate(GUIBeginnersGuide g){
			g.SetVisible(true);
		});
	}

	public static void Hide()
	{
		GUIBeginnersGuide tBeginner = Globals.Instance.MGUIManager.GetGUIWindow<GUIBeginnersGuide>();
		if(tBeginner != null)
		{
			tBeginner.SetVisible(false);
		}
	}

	public void SetArrowBootMode(Vector3 arrowBootMode)
	{
		this.SetVisible(true);
		ArrowBootMode.transform.localPosition = arrowBootMode;
		ArrowBootMode.transform.localScale = Vector3.one;
		BoxLeft.transform.localPosition = new Vector3(0f,0f,0f);
		BoxRight.transform.localPosition = new Vector3(0f,0f,0f);
		BoxUp.transform.localPosition = new Vector3(0f,0f,0f);
		BoxDown.transform.localPosition = new Vector3(0f,0f,0f);
		NGUITools.SetActive(Arrow.gameObject,false);
	}
	
	public void HideArrowBootMode()
	{
		this.SetVisible(true);
		ArrowBootMode.transform.localPosition = new Vector3(0f,0f,0f);
		ArrowBootMode.transform.localScale = Vector3.one;
		BoxLeft.transform.localPosition = new Vector3(0f,0f,0f);
		BoxRight.transform.localPosition = new Vector3(0f,0f,0f);
		BoxUp.transform.localPosition = new Vector3(0f,0f,0f);
		BoxDown.transform.localPosition = new Vector3(0f,0f,0f);
		NGUITools.SetActive(Arrow.gameObject,false);
	}
	
	public void ShowArrowBootMode(Vector3 correction,float correctionScale,float curWidth,float curHeight,Vector3 arrowPositon,Vector3 arrowRotate,int zoomType)
	{
		this.SetVisible(true);
		NGUITools.SetActive(ArrowBootMode,true);
		NGUITools.SetActive(Arrow.gameObject,true);
		if (zoomType == 1)
		{
			float widthRatio = Globals.Instance.MGUIManager.widthRatio;
			float heightRatio = Globals.Instance.MGUIManager.heightRatio;
			float zoomRatio = 0f;
			if(widthRatio >= heightRatio)
			{
				zoomRatio = widthRatio;
			}
			else
			{
				zoomRatio = heightRatio;
			}
			curWidth = curWidth * zoomRatio /widthRatio;
			curHeight = curHeight * zoomRatio/heightRatio;
		}
		else if(zoomType == 2)
		{
			float widthRatio = Globals.Instance.MGUIManager.widthRatio;
			float heightRatio = Globals.Instance.MGUIManager.heightRatio;
			float zoomRatio = 0f;
			if(widthRatio >= heightRatio)
			{
				zoomRatio = heightRatio;
			}
			else
			{
				zoomRatio = widthRatio;
			}
			curWidth = curWidth * zoomRatio /widthRatio;
			curHeight = curHeight * zoomRatio/heightRatio;
		}
		
		ArrowBootMode.transform.localPosition = new Vector3(correction.x,correction.y,correction.z);
		ArrowBootMode.transform.localScale = new Vector3(correctionScale,correctionScale);
		BoxLeft.transform.localPosition = new Vector3(-(curWidth/2),0f,0f);
		BoxRight.transform.localPosition = new Vector3(curWidth/2,0f,0f);
		BoxUp.transform.localPosition = new Vector3(0f,curHeight/2,0f);
		BoxDown.transform.localPosition = new Vector3(0f,-(curHeight/2),0f);
		float arrowPositonX = arrowPositon.x*Globals.Instance.MGUIManager.heightRatio/Globals.Instance.MGUIManager.widthRatio;
		float arrowPositonY = arrowPositon.y*Globals.Instance.MGUIManager.widthRatio/Globals.Instance.MGUIManager.heightRatio;
		
		Arrow.transform.localPosition = new Vector3(arrowPositonX,arrowPositonY,arrowPositon.z);;
		Arrow.transform.localEulerAngles = arrowRotate;
	}	
	
}
