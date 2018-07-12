using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortTipsContent : TipsContent
{
	//----------------------------------------------
	public GameObject FlagTP;
	public GameObject CutLineTP;
	//----------------------------------------------
	public const float TOP_SPACE = 4.0f;
	public const float BOTTOM_SPACE = 4.0f;
	public const float LEFT_SPACE = 8.0f;
	public const int MAX_STAR_COUNT = 5;
	//----------------------------------------------
	
	void Awake()
	{
		_mTipsData = null;
		
		_mCutLineList = new Dictionary<GameObject, float>();
	}
	
	public override void Init()
	{
		base.Init();
		
		_mRootBtn = this.transform.GetComponent<UIButton>() as UIButton;
		_mNameText = this.transform.Find("Name").GetComponent<SpriteText>() as SpriteText;
		_mContentText = this.transform.Find("Content").GetComponent<SpriteText>() as SpriteText;
		_mFlagIcon = this.transform.Find("NationalFlagIcon").GetComponent<PackedSprite>() as PackedSprite;
		
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
		return new Vector2(0,0);
	}
	
	public override void Destroy()
	{
		foreach (GameObject go in _mCutLineList.Keys)
		{
			GameObject.Destroy(go);
		}
		_mCutLineList.Clear();
		
		_mTipsData = null;
	}
	
	public override void SetValue(params object[] args)
	{
		// Clear first
		Destroy();
		
		_mTipsData = (PortData)args[0];
		if (null == _mTipsData)
			return;
		
		BuildTips(_mTipsData);
		CalculatePosition();
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(PortData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		
		// Calculate the star position and cutline position
		float tempHeight = totalHeight;
		val = GetFightPropertyText(data) + "		" + GetNationNameText(data) + "\n";
		
		val += GetFlourishLevelText(data) + "\n";
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 3 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		if (!data.BasicData.IsCanContest)
		{
			val += GetNeedCampText(data) + "\n";
			tempHeight += _mContentText.LineSpan;
		}
		if (-1 != data.BasicData.LevelRequire)
		{
			val += GetNeedLevelText(data) + "\n";
			tempHeight += _mContentText.LineSpan;
		}
		if (-1 != data.BasicData.FeatRequire)
		{
			val += GetNeedFeatText(data) + "\n";
			tempHeight += _mContentText.LineSpan;
		}
		
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 1 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		val += GetFlourishChampionNameText(data) + "\n";
		val += GetFightChampionNameText(data);
		
		_mContentText.Text = val;
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
		
		BuildNationIcon(data);
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
		
		// Alignment Flag
		pos = Vector3.zero;
		float iconHalfHeight = 0.5f * _mFlagIcon.height;
		float iconHalfWidth = 0.5f * _mFlagIcon.height;
		pos.x = halfWidth - iconHalfWidth;
		pos.y = halfHeight - iconHalfHeight;
		pos.z = -0.1f;
		_mFlagIcon.transform.localPosition = pos;

		// Reposition cutline
		pos = Vector3.zero;
		UIButton btn = null;
		foreach (GameObject go in _mCutLineList.Keys)
		{
			float height = _mCutLineList[go];
			btn = go.GetComponent<UIButton>() as UIButton;
			
			//pos.y = halfHeight - height + 0.5f * btn.height;
			pos.z = -0.1f;
			go.transform.localPosition = pos;
		}
	}
	
	private string GetNameText(PortData data)
	{
		string val = GUIFontColor.Purple + data.BasicData.PortName;
		return val;
	}
	
	private string GetNationNameText(PortData data)
	{
		string val = GUIFontColor.Orange + data.BasicData.CountryName;
		return val;
	}
	
	private void BuildNationIcon(PortData data)
	{
		_mFlagIcon.PlayAnim(data.BasicData.CountryFlagIcon);
	}
	
	private string GetFightPropertyText(PortData data)
	{
		string val = "";
		if (data.BasicData.IsCanContest)
		{
			switch ( (CampType)data.BasicData.CampID )
			{
			case CampType.ALLIED_POWERS:
			{
				val += GUIFontColor.Orange + Globals.Instance.MDataTableManager.GetWordText(21800004);
				break;
			}
			case CampType.AXIS_POWSERS:
			{
				val += GUIFontColor.Orange + Globals.Instance.MDataTableManager.GetWordText(21800005);
				break;
			}
			case CampType.NEUTRAL:
			{
				val += GUIFontColor.PureRed + Globals.Instance.MDataTableManager.GetWordText(21800003);
				break;
			}
			}
		}
		else
		{
			switch ( (CampType)data.BasicData.CampID )
			{
			case CampType.ALLIED_POWERS:
			{
				val += GUIFontColor.Orange + Globals.Instance.MDataTableManager.GetWordText(21800001);
				break;
			}
			case CampType.AXIS_POWSERS:
			{
				val += GUIFontColor.Orange + Globals.Instance.MDataTableManager.GetWordText(21800002);
				break;
			}
			}
		}
		
		return val;
	}
	
	private string GetFlourishLevelText(PortData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800006);
		string val = GUIFontColor.Orange + wordText + data.FlourishData.FlourishLevel;
		return val;
	}
	
	private string GetNeedCampText(PortData data)
	{
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800007);
		string col = GUIFontColor.White;
		string val = wordText;
		switch ( (CampType)data.BasicData.CampRequire )
		{
		case CampType.ALLIED_POWERS:
		{
			wordText = Globals.Instance.MDataTableManager.GetWordText(21800008);
			
			if (actorData.BasicData.CampID == CampType.AXIS_POWSERS)
				col = GUIFontColor.BloodRed;
			break;
		}
		case CampType.AXIS_POWSERS:
		{
			wordText = Globals.Instance.MDataTableManager.GetWordText(21800009);
			
			if (actorData.BasicData.CampID == CampType.ALLIED_POWERS)
				col = GUIFontColor.BloodRed;
			break;
		}
		case CampType.NEUTRAL:
		{
			wordText = Globals.Instance.MDataTableManager.GetWordText(21800010);
			
			// if (actorData.PlayerCampID == CampType.AXIS_POWSERS)
			// 	col = GUIFontColor.BloodRed;
			break;
		}
		case CampType.UNLIMIT:
		{
			wordText = Globals.Instance.MDataTableManager.GetWordText(21800011);
			break;
		}
		}
		
		val = val + col + wordText;
		return val;
	}
	
	private string GetNeedLevelText(PortData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800012);
		string val = GUIFontColor.PureRed + wordText + data.BasicData.LevelRequire;
		
		return val;
	}
	
	private string GetNeedFeatText(PortData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800013);
		string val = GUIFontColor.PureRed + wordText + data.BasicData.FeatRequire;
		return val;
	}
	
	private string GetFlourishChampionNameText(PortData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800014);
		string val = GUIFontColor.White + wordText + data.BasicData.HighestInvestorName;
		return val;
	}
	
	private string GetFightChampionNameText(PortData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21800015);
		string val = GUIFontColor.White + wordText + data.BasicData.OccupierName;
		return val;
	}

	private GameObject BuildCutLine()
	{
		GameObject go = Object.Instantiate(CutLineTP) as GameObject;
		go.transform.parent = this.transform;
		
		return go;
	}
	
	private UIButton _mRootBtn;
	private SpriteText _mNameText;
	private SpriteText _mContentText;
	private PackedSprite _mFlagIcon;
	
	private Dictionary<GameObject, float> _mCutLineList;
	
	private PortData _mTipsData;
}
