using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipCompareTipsContent : TipsContent
{
	//----------------------------------------------
	private GameObject FlagTP;
	//----------------------------------------------
	private const float TOP_SPACE = 4.0f;
	private const float BOTTOM_SPACE = 4.0f;
	private const float LEFT_SPACE = 8.0f;
	private const int MAX_STAR_COUNT = 5;
	//----------------------------------------------
	
	void Awake()
	{
		_mTipsData = null;
	}
	
	public override void Init()
	{
		base.Init();
		// Use a kind of layout
		_mTipsLayout = new ItemTipsLayout(_mToolTips);
		_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
	}
	
	public override void SetSize(float w, float h)
	{
		//_mRootBtn.SetSize(w, h);
	}
	public override void SetActiveBG (bool IsOpen)
	{
		
	} 
	
	public override Vector2 GetSize()
	{
		return new Vector2(0, 0);
	}
	
	public override void Destroy()
	{
		TextLeft.Text = "";
		TextLeftValue.Text = "";
		TextRight.Text = "";
		TextRightValue.Text = "";
		_mTipsData = null;
	}
	
	public override void SetValue(params object[] args)
	{
		// Clear first
		Destroy();
		
		_mTipsData = (string[])args;
		if (null == _mTipsData)
			return;
		
		BuildTips(_mTipsData);
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		//pos.x = pos.x - GetSize().x * 0.5f;
		pos.y = pos.y + Screen.height * 0.5f - 20f - GetSize().y * 0.5f;
		pos.z -= 1f;
		_mToolTips.transform.position = pos;
		//_mTipsLayout.DoLayout(pos, size);
		//Debug.Log(pos.ToString()+":"+size.ToString());
	}
	
	private void BuildTips(string[] data)
	{
		TextLeft.Text = GUIFontColor.Color2H + Globals.Instance.MDataTableManager.GetWordText(20600019)+"\n"+
			Globals.Instance.MDataTableManager.GetWordText(20600021)+"\n"+
				Globals.Instance.MDataTableManager.GetWordText(20600028)+"\n";
		
		TextLeftValue.Text = data[0]+"\n"+data[1]+"\n"+data[2]+"\n";
		
		TextRight.Text = GUIFontColor.Color2H + Globals.Instance.MDataTableManager.GetWordText(20600020)+"\n"+
			Globals.Instance.MDataTableManager.GetWordText(20600022)+"\n"+
				Globals.Instance.MDataTableManager.GetWordText(20600023)+"\n";
		
		TextRightValue.Text = data[3]+"\n"+data[4]+"\n"+data[5]+"\n";

		//_mToolTips.GUIWidth = _mRootBtn.width;
		//_mToolTips.GUIHeight = _mRootBtn.height;
	}

	private UIButton _mRootBtn;

	
	public SpriteText TextLeft;
	public SpriteText TextLeftValue;
	public SpriteText TextRight;
	public SpriteText TextRightValue;
	
	
	private string[] _mTipsData;
}
