using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillTipsContent : TipsContent
{
	//----------------------------------------------
	public const float TOP_SPACE = 6.0f;
	public const float BOTTOM_SPACE = 6.0f;
	public const float LEFT_SPACE = 8.0f;
	//----------------------------------------------
	
	void Awake()
	{
		_mTipsData = null;
	}
	
	void OnDestroy()
	{}
	
	public override void Init()
	{
		base.Init();
		
		_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
		_mNameText = this.transform.Find("Name").GetComponent<SpriteText>() as SpriteText;
		_mContentText = this.transform.Find("Content").GetComponent<SpriteText>() as SpriteText;
		_mSkillLegendIcon = this.transform.Find("SkillLegendIcon").GetComponent<PackedSprite>() as PackedSprite;
		
		// Use a kind of layout
		_mTipsLayout = new ItemTipsLayout(_mToolTips);
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
		_mTipsData = null;
	}
	
	public override void SetValue(params object[] args)
	{
		// Clear first
		Destroy();
		
		_mTipsData = args[0] as SkillData;
		if (null == _mTipsData)
			return;
		
		BuildTips(_mTipsData);
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(SkillData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		totalWidth = _mNameText.TotalWidth;
		
		val = GetDescriptText(data);
		_mContentText.Text = val;
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		float tmpWidth = _mContentText.BottomRight.x - _mContentText.TopLeft.x;
		totalWidth = totalWidth > tmpWidth ? totalWidth : tmpWidth;
		totalWidth += 2 * LEFT_SPACE;
		
		if (data.BasicData.SkillLegendID == -1)
		{
			_mSkillLegendIcon.gameObject.SetActiveRecursively(false);
		}
		else
		{
			_mSkillLegendIcon.gameObject.SetActiveRecursively(true);
			string legendIcon = data.BasicData.SkillLegendID.ToString();
			_mSkillLegendIcon.PlayAnim(legendIcon);
			
			totalHeight += _mSkillLegendIcon.height;
			//totalWidth = _mRootBtn.width;
		}
		
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
		
		CalculatePosition();
	}
	
	private void CalculatePosition()
	{
		float halfWidth = 0.5f * _mToolTips.GUIWidth;
		float halfHeight = 0.5f * _mToolTips.GUIHeight;
		
		float offset = 0.0f;
		offset += TOP_SPACE;
		
		// Alignment.Left
		Vector3 pos = Vector3.zero;
		float halfTextHeight = 0.5f * _mNameText.LineSpan;
		pos.x = LEFT_SPACE - halfWidth;
		pos.y = halfHeight - halfTextHeight - TOP_SPACE;
		pos.y += 0.25f * _mNameText.LineSpan;
		pos.z = -1.0f;
		_mNameText.transform.localPosition = pos;
		offset += _mNameText.LineSpan;
		
		// Alignment.Left
		pos = Vector3.zero;
		halfTextHeight = 0.5f * (_mContentText.TopLeft.y - _mContentText.BottomRight.y);
		pos.x = LEFT_SPACE - halfWidth;
		pos.y = halfHeight - halfTextHeight - offset;
		pos.y += 0.25f * _mContentText.LineSpan;
		pos.z = -1.0f;
		_mContentText.transform.localPosition = pos;
		
		// if (_mSkillLegendIcon.gameObject.active)
		// {
		// 	pos = Vector3.zero;
		// }
	}
	
	private string GetNameText(SkillData data)
	{
		string val = GUIFontColor.FloralWhite247246220 + data.BasicData.SkillName;
		return val;
	}
	
	private string GetDescriptText(SkillData data)
	{
		string val = GUIFontColor.White + data.BasicData.SkillDesc;
		return val;
	}
	
	private UIButton _mRootBtn;
	private SpriteText _mNameText;
	private SpriteText _mContentText;
	private PackedSprite _mSkillLegendIcon;
	
	private SkillData _mTipsData;
}
