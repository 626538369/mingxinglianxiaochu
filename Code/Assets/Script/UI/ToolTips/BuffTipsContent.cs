using UnityEngine;
using System.Collections;

public class BuffTipsContent : TipsContent
{
	//----------------------------------------------
	public const float TOP_SPACE = 4.0f;
	public const float BOTTOM_SPACE = 4.0f;
	public const float LEFT_SPACE = 8.0f;
	public const int MAX_STAR_COUNT = 5;
	//----------------------------------------------
	
	void Awake()
	{
	}
	
	void OnDestroy()
	{}
	
	public override void Init()
	{
		base.Init();
		
		_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
		_mNameText = this.transform.Find("Name").GetComponent<SpriteText>() as SpriteText;
		_mContentText = this.transform.Find("Content").GetComponent<SpriteText>() as SpriteText;
		
		// Use a kind of layout
		_mTipsLayout = new ItemTipsLayout(_mToolTips);
	}
	
	public override void SetSize(float w, float h)
	{
		//_mRootBtn.SetSize(w, h);
	}
	
	public override void SetActiveBG (bool IsOpen)
	{
		//throw new System.NotImplementedException ();
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
		// Clear first
		Destroy();
		
		_mTipsData = (BuffData)args[0];
		if (null == _mTipsData)
			return;
		
		BuildTips(_mTipsData);
		CalculatePosition();
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(BuffData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		
		val = GetBuffEffectText(data);
		val += GetDescriptionText(data);
		val += GetRemainTimeText(data);
		
		_mContentText.Text = val;
		
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void CalculatePosition()
	{
		float offset = 0.0f;
		
		offset += TOP_SPACE;
		offset += BOTTOM_SPACE;
		offset += _mNameText.LineSpan;
		
		// Reposition the NameText
		float halfWidth = 0.5f * _mToolTips.GUIWidth;
		float halfHeight = 0.5f * _mToolTips.GUIHeight;
		
		// Alignment.Center
		Vector3 pos = Vector3.zero;
		float halfTextHeight = 0.5f * _mNameText.LineSpan;
		pos.y = halfHeight - halfTextHeight - TOP_SPACE;
		pos.y += 0.25f * _mNameText.LineSpan;
		pos.z = -0.1f;
		_mNameText.transform.localPosition = pos;
		
		// Alignment.Left
		pos = Vector3.zero;
		halfTextHeight = 0.5f * (_mContentText.TopLeft.y - _mContentText.BottomRight.y);
		pos.x = LEFT_SPACE - halfWidth;
		pos.y -= 0.5f * offset;
		pos.y += BOTTOM_SPACE;
		pos.y += 0.25f * _mContentText.LineSpan;
		pos.z = -0.1f;
		_mContentText.transform.localPosition = pos;
	}
	
	private string GetNameText(BuffData data)
	{
		string val = data.Name;
		return val;
	}
	
	private string GetBuffEffectText(BuffData data)
	{
		string val = data.Descript;
		return val;
	}
	
	private string GetDescriptionText(BuffData data)
	{
		string val = data.Descript;
		return val;
	}
	
	private string GetRemainTimeText(BuffData data)
	{
		string val = data.PersistTime.ToString();
		return val;
	}
	
	private UIButton _mRootBtn;
	private SpriteText _mNameText;
	private SpriteText _mContentText;
	
	private BuffData _mTipsData;
}
