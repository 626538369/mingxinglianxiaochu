using UnityEngine;
using System.Collections;

public class SimpleTipsContent : TipsContent
{
	//----------------------------------------------
	public const float TOP_SPACE = 16.0f;
	public const float BOTTOM_SPACE = 16.0f;
	public const float LEFT_SPACE = 24.0f;
	public const float RIGHT_SPACE = 24.0f;
	//----------------------------------------------
	private bool isopen = true;
	void Awake()
	{
	}
	
	public override void Init()
	{
		base.Init();
		
		//_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
		// _mNameText = this.transform.FindChild("Name").GetComponent<SpriteText>() as SpriteText;
		_mContentText = this.transform.Find("Content").GetComponent<UILabel>() as UILabel;
		_mSpriteBG = this.transform.Find("SpriteBG").GetComponent<UISprite>() as UISprite;
		_mSpriteSmallBG = this.transform.Find("SpriteSmallBG").GetComponent<UISprite>() as UISprite;
		_mTipsLayout = new SimpleTipsLayout(_mToolTips);
	}
	
	public override void SetSize(float w, float h)
	{
		//_mRootBtn.SetSize(w, h);
	}
	public override void SetActiveBG(bool Isopen)
	{
		_mSpriteSmallBG.height = _mContentText.height + 150;
		_mSpriteSmallBG.width = _mContentText.width ;
		isopen = Isopen;
		if(Isopen)
		{
			_mSpriteBG.transform.localScale = Vector3.zero;
			_mSpriteSmallBG.transform.localScale = Vector3.one;
		}
		else
		{
			_mSpriteBG.transform.localScale = Vector3.one;
			_mSpriteSmallBG.transform.localScale = Vector3.zero;
			
			
		}
	}
	
	public override Vector2 GetSize()
	{
		return new Vector2(0, 0);
	}
	
	public override void Destroy()
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
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(string text)
	{

		string val = text;
		_mContentText.text = val;
		
		float totalWidth = 0.0f;
		//float totalHeight = _mContentText. + 10;
		float totalHeight = _mContentText.border.y + 10;
		
		totalWidth += LEFT_SPACE;
		// totalHeight += TOP_SPACE; // Top
		
		// Resize the SpriteText height
		// totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		// totalHeight += BOTTOM_SPACE; // Bottom
		
		totalWidth += _mContentText.border.x;
		totalWidth += RIGHT_SPACE;
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
		
	}
	
	private void CalculatePosition()
	{
		float offset = 0.0f;
		
		offset += TOP_SPACE;
		offset += BOTTOM_SPACE;
		
		// Reposition the NameText
		float halfWidth = 0.5f * _mToolTips.GUIWidth;
		float halfHeight = 0.5f * _mToolTips.GUIHeight;
		
		// Alignment.Left
		Vector3 pos = Vector3.zero;
		float halfTextHeight = 0.5f * (_mContentText.border.y);
		pos.x = LEFT_SPACE - halfWidth;
		// pos.y -= 0.5f * offset;
		// pos.y += BOTTOM_SPACE;
		pos.y = 0.0f;
		//pos.y += 0.25f * _mContentText.LineSpan;
		pos.y += 3;
		pos.z = -6.1f;
		_mContentText.transform.localPosition = pos;
		UIWidget colors = _mContentText.transform.GetComponent<UIWidget>();
		if(isopen)
		{
			colors.color = new Color(0.25f,1f,0.52f,1f);
		}
		else
		{
			colors.color = new Color(1,1,1,1);
		}
		
	}
	
	private UIButton _mRootBtn;
	private UILabel _mContentText;
	private UISprite _mSpriteBG;
	private UISprite _mSpriteSmallBG;
	private string _mText;
}
