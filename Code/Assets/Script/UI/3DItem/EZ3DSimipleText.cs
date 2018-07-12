using UnityEngine;
using System.Collections;

public class EZ3DSimipleText : EZ3DItem 
{
	public const float HORIZONTAL_OFFSET = 16.0f;
	
	/// <summary>
	/// tzz fucked
	/// The m game object.
	/// </summary>
	private GameObject	mGameObject	= null;
	
	/// <summary>
	/// tzz fucked
	/// The m world position if GameObject is null
	/// </summary>
	private Vector3		mWorldPosition;
	
	/// <summary>
	/// tzz funcked
	/// The m world position offset.
	/// </summary>
	private float		mWorldPositionOffsetY;
	
	void Awake()
	{
		_mBtnText = this.GetComponent<UIButton>() as UIButton;
		_mSpriteText = _mBtnText.transform.Find("SpriteText").GetComponent<SpriteText>() as SpriteText;
		
		UIEventListener.Get(_mBtnText.gameObject).onClick += OnClickBtn;
	}
	
	public override void SetValue(params object[] args)
	{
		if(args[0] is GameObject){
			mGameObject			= (GameObject)args[0];
		}else{
			mWorldPosition			= (Vector3)args[0];
		}
		
		
		_mBtnText.GetComponent<GUIText>().text		= (string)args[1];
		_mSpriteText.Text	= _mBtnText.GetComponent<GUIText>().text;
		
		float totalWidth	= _mSpriteText.BottomRight.x - _mSpriteText.TopLeft.x;
		totalWidth += 2 * HORIZONTAL_OFFSET;
				
		mWorldPositionOffsetY	= (float)args[2];
	}
	
	public void SetValueChangeDelegate(UIEventListener.VoidDelegate del = null)
	{
		valChangeDel = del;
	}
	
	void OnClickBtn(GameObject obj)
	{
		if (null != valChangeDel)
		{
			valChangeDel(obj);
		}
	}
	
	void LateUpdate(){
		
		Vector3 guiPos;
		
		if(mGameObject != null){
			guiPos = GUIManager.WorldToGUIPoint(mGameObject.transform.position + new Vector3(0,mWorldPositionOffsetY,0));
		}else{
			guiPos = mWorldPosition;
		}
		
		guiPos.x -= 10;
		guiPos.y += 18;
		guiPos.x *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.x;
		guiPos.y *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.y;
		
		this.transform.localPosition = new Vector3(guiPos.x, guiPos.y, GUIManager.GUI_FARTHEST_Z + 100);
	}
	
	public override void Reposition(Vector3 worldPos)
	{
		// tzz fucked
		// delete them code all
	}
	
	/// <summary>
	/// Gets the button text.
	/// </summary>
	/// <value>
	/// The button text.
	/// </value>
	public UIButton BtnText{
		get{return _mBtnText;}
	}
	
	private UIButton _mBtnText;
	private SpriteText _mSpriteText;
	
	private Quaternion _mDefaultQuat;
	UIEventListener.VoidDelegate valChangeDel = null;
}
