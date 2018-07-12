using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipTipsContent : TipsContent
{
	//----------------------------------------------
	public GameObject StarTP;
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
		
		_mQualityStarList = new Dictionary<GameObject, float>();
		_mCutLineList = new Dictionary<GameObject, float>();
	}
	
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
		
	}
	public override Vector2 GetSize()
	{
		return new Vector2(0, 0);
	}
	
	public override void Destroy()
	{
		foreach (GameObject go in _mQualityStarList.Keys)
		{
			GameObject.Destroy(go);
		}
		
		foreach (GameObject go in _mCutLineList.Keys)
		{
			GameObject.Destroy(go);
		}
		
		_mQualityStarList.Clear();
		_mCutLineList.Clear();
		
		_mTipsData = null;
	}
	
	public override void SetValue(params object[] args)
	{
		// Clear first
		Destroy();
		
		_mTipsData = (GirlData)args[0];
		if (null == _mTipsData)
			return;
		
		BuildTips(_mTipsData);
		CalculatePosition();
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(GirlData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		//totalHeight += _mNameText.LineSpan;
		
		// Calculate the star position and cutline position
		//float tempHeight = totalHeight;
		
		// Add quality star line
		val = "\n";
//		tempHeight += _mContentText.characterSize;
//		for (int i = 0; i < MAX_STAR_COUNT; ++i)
//		{
//			GameObject go = BuildStar();
//			go.name = "Star";
//			
//			_mQualityStarList.Add(go, tempHeight);
//		}
		
		val += GetTypeText(data) + "\n";
//		// CutLine, skip one line
//		val += "\n";
//		{
//			tempHeight += 2 * _mContentText.LineSpan;
//			GameObject go = BuildCutLine();
//			go.name = "CutLine";
//			
//			_mCutLineList.Add(go, tempHeight);
//		}
		
		val += GetTonnageText(data) + "\n";
		val += GetAttackPropertyText(data) + "\n";
		// CutLine, skip one line
//		val += "\n";
//		{
//			tempHeight += 5 * _mContentText.LineSpan;
//			GameObject go = BuildCutLine();
//			go.name = "CutLine";
//			
//			_mCutLineList.Add(go, tempHeight);
//		}
		val += GetBasePropertyText(data) + "\n";
		val += GetCaptainText(data) + "\n";
		val += GetCardInfoText(data) + "\n";
		_mContentText.Text = val;
		// Resize the SpriteText height
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
		
		// Reposition quality star
		PackedSprite sprite = StarTP.GetComponent<PackedSprite>() as PackedSprite;
		float halfAllStarWidth = 0.5f * sprite.width * _mQualityStarList.Count;
		float startx = 0.5f * sprite.width - halfAllStarWidth;
		int startIndex = 0;
		foreach (GameObject go in _mQualityStarList.Keys)
		{
			float height = _mQualityStarList[go];
			sprite = go.GetComponent<PackedSprite>() as PackedSprite;
			
			pos.x = startx + sprite.width * startIndex;
			pos.y = halfHeight - height + 0.5f * sprite.height;
			pos.z = -0.1f;
			go.transform.localPosition = pos;
			
			startIndex++;
		}

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
	
	private string GetNameText(GirlData data)
	{
		string MColor = GUIFontColor.White;
//		switch(data.BasicData.RarityLevel)
//		{
//		case 1:
//			MColor = GUIFontColor.White;
//			break;
//		case 2:
//			MColor = GUIFontColor.PureGreen;
//			break;
//		case 3:
//			MColor = GUIFontColor.PureBlue;
//			break;
//		case 4:
//			MColor = GUIFontColor.Purple;
//			break;
//		case 5:
//			MColor = GUIFontColor.Orange;
//			break;
//		}
//		Debug.Log(MColor+data.BasicData.RarityLevel);
		string val = MColor + data.CardBase.CardName;;
		return val;
	}
	
	private string GetTypeText(GirlData data)
	{
//		string val = GUIFontColor.Orange + data.BasicData.TypeName;
		string val = GUIFontColor.Orange ;
		return val;
	}
	
	private string GetTonnageText(GirlData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20800020);
//		string val = GUIFontColor.White + wordText + data.BasicData.Tonnage;
			string val = GUIFontColor.White + wordText;
		return val;
	}
	
	private string GetAttackPropertyText(GirlData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20800021);
		string val = GUIFontColor.White + wordText + data.PropertyData.SeaAttack + "\n";
		wordText = Globals.Instance.MDataTableManager.GetWordText(20800022);
		val += GUIFontColor.White + wordText + data.PropertyData.SubmarineAttack + "\n";
		wordText = Globals.Instance.MDataTableManager.GetWordText(20800023);
		val += GUIFontColor.White + wordText + data.PropertyData.SkillAttack;
		
		return val;
	}
	
	private string GetBasePropertyText(GirlData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20800024);
		string val = GUIFontColor.White + wordText + data.PropertyData.Defense + "\n";
		wordText = Globals.Instance.MDataTableManager.GetWordText(20800025);
		val += GUIFontColor.White + wordText + data.PropertyData.Life + "\n";
		wordText = Globals.Instance.MDataTableManager.GetWordText(20800026);
		val += GUIFontColor.White + wordText + data.PropertyData.Range;
		
		return val;
	}
	
	private string GetCaptainText(GirlData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20800027);
		long WarshipGeneralID = data.WarshipGeneralID;
		Dictionary<long,GeneralData> dictGeneralData = Globals.Instance.MGameDataManager.MActorData.GetGeneralDataList();
		string WarshipGeneralName = "lsj";
		if(dictGeneralData.ContainsKey(WarshipGeneralID))
		{
			WarshipGeneralName = dictGeneralData[WarshipGeneralID].BasicData.Name;
		}
		else
		{
			WarshipGeneralName = Globals.Instance.MDataTableManager.GetWordText(20800029);
		}
		string val = GUIFontColor.Color10H + wordText + WarshipGeneralName;
		return val;
	}
	
	private string GetCardInfoText(GirlData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20800028);
		string text = Globals.Instance.MDataTableManager.GetWordText(20800029);
		string val = GUIFontColor.Color10H + wordText + text;
		return val;
	}
	
	private void ShowGeneralQualityStar(int quality)
	{
		int grayCount = (10 - quality) / 2;
		int halfPos = quality % 2;
		if (halfPos == 0)
		{
			halfPos = -1;
		}
		else
		{
			halfPos = quality / 2 + 1;
		}
		
		int index = 0;
		foreach (GameObject go in _mQualityStarList.Keys)
		{
			PackedSprite star = go.GetComponent<PackedSprite>() as PackedSprite;
			if (index < grayCount)
			{
				star.PlayAnim("GrayStar");
			}
			else if (index == halfPos)
			{
				star.PlayAnim("HalfStar");
			}
			else
			{
				star.PlayAnim("Star");
			}
			
			index++;
		}
	}
	
	private GameObject BuildStar()
	{
		GameObject go = Object.Instantiate(StarTP) as GameObject;
		go.transform.parent = this.transform;
		
		return go;
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
	
	private Dictionary<GameObject, float> _mQualityStarList;
	private Dictionary<GameObject, float> _mCutLineList;
	
	private GirlData _mTipsData;
}
