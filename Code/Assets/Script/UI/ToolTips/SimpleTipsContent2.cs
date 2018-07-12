using UnityEngine;
using System.Collections;

public class SimpleTipsContent2 : TipsContent
{
	//----------------------------------------------
	public const float TOP_SPACE = 16.0f;
	public const float BOTTOM_SPACE = 16.0f;
	public const float LEFT_SPACE = 24.0f;
	public const float RIGHT_SPACE = 24.0f;
	//----------------------------------------------
	
	void Awake()
	{
	}
	
	public override void Init()
	{
		base.Init();
		
		_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
		_mContentText = this.transform.Find("Content").GetComponent<SpriteText>() as SpriteText;
		
		_mTipsLayout = new TopCenterLayout(_mToolTips);
	}
	
	public override void SetSize(float w, float h)
	{
		//_mRootBtn.SetSize(w, h);
	}
	
	public override Vector2 GetSize()
	{
		return new Vector2(0, 0);
	}
	
	public override void Destroy()
	{

	}
	public override void SetActiveBG (bool IsOpen)
	{
		
	}
	
	public override void SetValue(params object[] args)
	{
		_mText = (string)args[0];
		
		BuildTips(_mText);
		CalculatePosition();
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		// _mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(string text)
	{
		float totalWidth = 0.0f;
		float totalHeight =0;
		totalWidth += LEFT_SPACE;
		
		string val = text;
		_mContentText.Text = val;
		
		totalWidth += _mContentText.BottomRight.x - _mContentText.TopLeft.x;
		totalWidth += RIGHT_SPACE;
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void CalculatePosition()
	{
		float offset = 0.0f;
		
		offset += TOP_SPACE;
		offset += BOTTOM_SPACE;
		
		float halfWidth = 0.5f * _mToolTips.GUIWidth;
		float halfHeight = 0.5f * _mToolTips.GUIHeight;
		
		// Alignment.Left
		Vector3 pos = Vector3.zero;
		float halfTextHeight = 0.5f * (_mContentText.TopLeft.y - _mContentText.BottomRight.y);
		pos.x = LEFT_SPACE - halfWidth;
		// pos.y -= 0.5f * offset;
		// pos.y += BOTTOM_SPACE;
		pos.y = 0.0f;
		pos.y += 0.25f * _mContentText.LineSpan;
		pos.z = -0.1f;
		_mContentText.transform.localPosition = pos;
	}
	
	private UIButton _mRootBtn;
	private SpriteText _mContentText;
	
	private string _mText;
}
