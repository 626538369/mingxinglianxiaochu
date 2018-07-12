using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralTipsContent : TipsContent
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
	
	public override Vector2 GetSize()
	{
		return new Vector2(0, 0);
	}
	
	public override void SetActiveBG (bool IsOpen)
	{
		
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
		
		_mTipsData = (GeneralData)args[0];
		if (null == _mTipsData)
			return;
		if (null == _mTipsData.BasicData)
			return;
		
		BuildTips(_mTipsData);
		CalculatePosition();
	}
	
	public override void DoLayout(Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void BuildTips(GeneralData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		
		// Calculate the star position and cutline position
		float tempHeight = totalHeight;
		
		// Add quality star line
		val = "\n";
		tempHeight += _mContentText.characterSize;
		for (int i = 0; i < MAX_STAR_COUNT; ++i)
		{
			GameObject go = BuildStar();
			go.name = "Star";
			
			_mQualityStarList.Add(go, tempHeight);
		}
		ShowGeneralQualityStar(data);
		
		val += GetOccupationText(data) + "			" + GetLevelText(data) + "\n";
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 2 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		val += GetPropertyText(data) + "\n";
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 4 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		val += GetAttackPropertyText(data) + "\n";
		val += GetBasePropertyText(data) + "\n";
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 6 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		val += GetSkillText(data);
		
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
	
	private string GetNameText(GeneralData data)
	{
		string val = GUIFontColor.Purple + data.BasicData.Name;
		return val;
	}
	
	private string GetOccupationText(GeneralData data)
	{
		string val = GUIFontColor.Orange + data.BasicData.ProfessionName;
		return val;
	}
	
	private string GetLevelText(GeneralData data)
	{
		string val = GUIFontColor.Orange + data.BasicData.Level + "级";
		return val;
	}
	
	private string GetPropertyText(GeneralData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(20700003);
		
		string val = GUIFontColor.Orange + wordText + data.PropertyData.Valiant + "\n";
		
		wordText = Globals.Instance.MDataTableManager.GetWordText(20700004);
		val += GUIFontColor.Orange + wordText + data.PropertyData.Command + "\n";
		
		wordText = Globals.Instance.MDataTableManager.GetWordText(20700005);
		val += GUIFontColor.Orange + wordText + data.PropertyData.Intelligence;
		
		return val;
	}
	
	private string GetAttackPropertyText(GeneralData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(100000);
		string val = GUIFontColor.Orange + wordText + data.PropertyData.SeaAttack + "\n";
		val += GUIFontColor.Orange + wordText + data.PropertyData.SubmarineAttack + "\n";
		val += GUIFontColor.Orange + wordText + data.PropertyData.SkillAttack;
		
		return val;
	}
	
	private string GetBasePropertyText(GeneralData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(100000);
		string val = GUIFontColor.Orange + wordText + data.PropertyData.Defense + "\n";
		val += GUIFontColor.Orange + wordText + data.PropertyData.Life ;
		
		return val;
	}
	
	private string GetSkillText(GeneralData data)
	{
		string wordText = Globals.Instance.MDataTableManager.GetWordText(21100005);
		string val = GUIFontColor.Orange + wordText + data.SkillDatas[0].BasicData.SkillName + "\n";
		
		wordText = Globals.Instance.MDataTableManager.GetWordText(10310117);
		val += GUIFontColor.Orange + wordText + data.SkillDatas[0].BasicData.SkillName;
		
		return val;
	}
	
	private void ShowGeneralQualityStar(GeneralData data)
	{
		int quality = data.BasicData.QualityStar;
		
		int highStarCount = quality / 2;
		int halfPos = quality % 2;
		if (halfPos == 0)
		{
			halfPos = -1;
		}
		else
		{
			halfPos = quality / 2;
		}
		
		int index = 0;
		foreach (GameObject go in _mQualityStarList.Keys)
		{
			PackedSprite star = go.GetComponent<PackedSprite>() as PackedSprite;
			if (index < highStarCount)
			{
				star.PlayAnim("Star");
			}
			else if (index == halfPos)
			{
				star.PlayAnim("HalfStar");
			}
			else
			{
				star.PlayAnim("GrayStar");
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
	
	private GeneralData _mTipsData;
}
