using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemTipsContent : TipsContent
{
	public GameObject StarTP;
	public GameObject CutLineTP;
	
	public const float TOP_SPACE = 4.0f;
	public const float BOTTOM_SPACE = 4.0f;
	public const float LEFT_SPACE = 8.0f;
	
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
		// _mNameText.SetAlignment(SpriteText.Alignment_Type.Left);
		// _mNameText.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
		// _mContentText.SetAlignment(SpriteText.Alignment_Type.Left);
		// _mContentText.SetAnchor(SpriteText.Anchor_Pos.Middle_Left);
		
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
		ClearList();
	}
	
	public override void SetValue(params object[] args)
	{
		// Clear first
		Destroy();
		
		_mTipsData = (ItemSlotData)args[0];
		if (null == _mTipsData)
			return;
		
		switch (_mTipsData.MItemData.BasicData.MajorType)
		{
		case ItemMajorType.GENERAL_EQUIP:
		{
			ShowGeneralEquip(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.SHIP_EQUIP:
		{
			ShowShipEquip(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.MATERIAL:
		{
			ShowMaterial(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.GIFT_PACKAGE:
		{
			ShowGiftPackage(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.EXPENDABLE:
		{
			ShowExpendable(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.SHIP_CARD:
		{
			showCardTips(_mTipsData.MItemData);
			break;
		}
		case ItemMajorType.EQUIPMENT_CARD:
		{
			showCardTips(_mTipsData.MItemData);
			break;
			
		}
		case ItemMajorType.FEMULAR_CARD:
		{
			showCardTips(_mTipsData.MItemData);
			break;
		}
			
		}
		
		CalculatePosition();
	}
	
	public override void DoLayout (Vector3 pos, Vector2 size)
	{
		_mTipsLayout.DoLayout(pos, size);
	}
	
	private void ClearList()
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
	}
	
	private void showCardTips(ItemData data)
	{
        float totalWidth = _mNameText.maxWidth;
        float totalHeight = 0.0f;
        totalHeight += TOP_SPACE; // Top

        string val = GetNameText(data);
        _mNameText.Text = val;
        totalHeight += _mNameText.LineSpan;
        float tempHeight = totalHeight;

        // Add quality star line
        val = "\n";
        tempHeight += _mContentText.characterSize;
        for (int i = 0; i < 4; ++i)
        {
            GameObject go = BuildStar();
            go.name = "Star";

            _mQualityStarList.Add(go, tempHeight);
        }

        val += GetTypeText(data) + "			" + GetTypePosText(data) + "\n";
        val += GetLevelLimitText(data) + "\n";

        // CutLine, skip one line
        val += "\n";
        {
            tempHeight += 3 * _mContentText.LineSpan;
            GameObject go = BuildCutLine();
            go.name = "CutLine";

            _mCutLineList.Add(go, tempHeight);
        }

        if (null != data.PropertyData)
        {
            float outlineNum = 0;
            val += GetShipCardPropertyDataText(data, out outlineNum);

            // CutLine, skip one line
            val += "\n";
            {
                tempHeight += (outlineNum + 1) * _mContentText.LineSpan;
                GameObject go = BuildCutLine();
                go.name = "CutLine";

                _mCutLineList.Add(go, tempHeight);
            }
        }

        if (_mTipsData.SlotType == ItemSlotType.SHOP)
        {
            val += GetBuyPriceText(data);
        }
        else
            val += GetSellPriceText(data);

        _mContentText.Text = val;

        // Resize the SpriteText height
        totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
        totalHeight += BOTTOM_SPACE; // Bottom

        _mToolTips.GUIWidth = totalWidth;
        _mToolTips.GUIHeight = totalHeight;
	}
	
	private void ShowGeneralEquip(ItemData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		float tempHeight = totalHeight;
		
		// Add quality star line
		val = "\n";
		tempHeight += _mContentText.characterSize;
		for (int i = 0; i < 4; ++i)
		{
			GameObject go = BuildStar();
			go.name = "Star";
			
			_mQualityStarList.Add(go, tempHeight);
		}
		
		val += GetTypePosText(data) + "       " + GetTypeText(data) + "\n";
		val += GetLevelLimitText(data) + "\n";
		
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += 3 * _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		if (null != data.PropertyData)
		{
			float outlineNum = 0;
			val += GetGeneralEquipAdditiveDataText(data, out outlineNum);
			
			// CutLine, skip one line
			val += "\n";
			{
				tempHeight += (outlineNum + 1) * _mContentText.LineSpan;
				GameObject go = BuildCutLine();
				go.name = "CutLine";
				
				_mCutLineList.Add(go, tempHeight);
			}
		}
		
		if (_mTipsData.SlotType == ItemSlotType.SHOP)
		{
			val += GetBuyPriceText(data);
		}
		else
			val += GetSellPriceText(data);
		
		_mContentText.Text = val;
		
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void ShowShipEquip(ItemData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		float tempHeight = totalHeight;
		
		// Add quality star line
		val = "\n";
		tempHeight += _mContentText.characterSize;
		for (int i = 0; i < 4; ++i)
		{
			GameObject go = BuildStar();
			go.name = "Star";
			
			_mQualityStarList.Add(go, tempHeight);
		}
		
		//if (data.BasicData.IsPermitStrengthen)
		//{
		//	val += GetStrengthenText(data) + "\n";
		//	tempHeight += _mContentText.LineSpan;
		//}
		
		val += GetTypePosText(data) + "      " + GetTypeText(data) + "\n";
		tempHeight += _mContentText.LineSpan;
		
		val += GetLevelLimitText(data) + "\n";
		tempHeight += _mContentText.LineSpan;
		
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
	
		{
			float outlineNum = 0;
			val += GetShipEquipPropertyDataText(data, out outlineNum);
			
			// CutLine, skip one line
			val += "\n";
			{
				tempHeight += (outlineNum + 1) * _mContentText.LineSpan;
				GameObject go = BuildCutLine();
				go.name = "CutLine";
				
				_mCutLineList.Add(go, tempHeight);
			}
		}
		
		// Card Additive
		
		if (_mTipsData.SlotType == ItemSlotType.SHOP)
		{
			val += GetBuyPriceText(data);
		}
		else
			val += GetSellPriceText(data);
		
		_mContentText.Text = val;
		
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void ShowMaterial(ItemData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		float tempHeight = totalHeight;
		
		val = GetTypeText(data) + "\n";
		tempHeight += _mContentText.characterSize;
		
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		if (_mTipsData.SlotType == ItemSlotType.SHOP)
		{
			val += GetBuyPriceText(data);
		}
		else
			val += GetSellPriceText(data);
		
		_mContentText.Text = val;
		
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void ShowGiftPackage(ItemData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		float tempHeight = totalHeight;
		
		val = GetTypeText(data) + "\n";
		tempHeight += _mContentText.characterSize;
		
		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		if (data.BasicData.IsPermitSell)
		{
			if (_mTipsData.SlotType == ItemSlotType.SHOP)
			{
				val += GetBuyPriceText(data);
			}
			else
			{
				val += GetSellPriceText(data);
			}
		}
		else
		{
			val += GetNotPermitSellText(data);
		}
		
		_mContentText.Text = val;
		
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private void ShowExpendable(ItemData data)
	{
		float totalWidth = _mNameText.maxWidth;
		float totalHeight = 0.0f;
		totalHeight += TOP_SPACE; // Top
		
		string val = GetNameText(data);
		_mNameText.Text = val;
		totalHeight += _mNameText.LineSpan;
		float tempHeight = totalHeight;
		
		val = GetTypeText(data) + "\n";
		tempHeight += _mContentText.characterSize;

		// CutLine, skip one line
		val += "\n";
		{
			tempHeight += _mContentText.LineSpan;
			GameObject go = BuildCutLine();
			go.name = "CutLine";
			
			_mCutLineList.Add(go, tempHeight);
		}
		
		if (_mTipsData.SlotType == ItemSlotType.SHOP)
		{
			val += GetBuyPriceText(data);
		}
		else
			val += GetSellPriceText(data);
		
		_mContentText.Text = val;
		
		// Resize the SpriteText height
		totalHeight += _mContentText.TopLeft.y - _mContentText.BottomRight.y;
		totalHeight += BOTTOM_SPACE; // Bottom
		
		_mToolTips.GUIWidth = totalWidth;
		_mToolTips.GUIHeight = totalHeight;
	}
	
	private string GetNameText(ItemData data)
	{
		string val = GUIFontColor.Color1H + data.BasicData.Name;
		return val;
	}
	
	private string GetTypeText(ItemData data)
	{
		string val = GUIFontColor.Yellowish + data.BasicData.TypeName;
		return val;
	}
	
	private string GetTypePosText(ItemData data)
	{
		string val = GUIFontColor.Yellowish + data.BasicData.TypePosName;
		return val;
	}
	
	private string GetStrengthenText(ItemData data)
	{
		string val = string.Empty;
	
		return val;
	}
	
	private string GetLevelLimitText(ItemData data)
	{
		string textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100001);
		string val = null;
		
		// Check limit condition
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		if (actorData.BasicData.Level < data.BasicData.UseLevelLimit)
		{
			val = GUIFontColor.PureRed + textFromDict + data.BasicData.UseLevelLimit;
		}
		else
		{
			val = GUIFontColor.Yellowish + textFromDict + data.BasicData.UseLevelLimit;
		}
		
		return val;
	}
	
	private string GetNotPermitSellText(ItemData data)
	{
		string textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100017);
		string val = GUIFontColor.Yellow + textFromDict;
		
		return val;
	}
	
	private string GetBuyPriceText(ItemData data)
	{
		string textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100008);
		string val = null;
		
		// Check limit condition
		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
		if (actorData.WealthData.Money < data.BasicData.BuyPrice)
		{
			val = GUIFontColor.PureRed + textFromDict + data.BasicData.BuyPrice;
		}
		else
		{
			val = GUIFontColor.Yellow + textFromDict + data.BasicData.BuyPrice;
		}
		
		return val;
	}
	
	private string GetSellPriceText(ItemData data)
	{
		string textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100009);
		string val = GUIFontColor.Yellow + textFromDict + data.BasicData.SellPrice;
		
		return val;
	}
	
	private string GetDiscriptionText(ItemData data)
	{
		string val = data.BasicData.Description;
		return val;
	}
	
	private string GetGeneralEquipAdditiveDataText(ItemData data, out float lineNum)
	{
		string textFromDict = "";
		string val = "";
		lineNum = 0;
		
		if (0 != data.PropertyData.Valiant)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100002);
			val = GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.Valiant + "\n";
			
			lineNum++;
		}
		
		if (0 != data.PropertyData.Command)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100003);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.Command + "\n";
			
			lineNum++;
		}
		
		if (0 != data.PropertyData.Intelligence)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100004);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.Intelligence + "\n";
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipLife)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100007);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipLife + "\n";
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipDefense)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100006);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipDefense + "\n";
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipSkillAttack)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100005);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipSkillAttack + "\n";
			lineNum++;
		}
		
		return val;
	}
	
	public string GetShipCardPropertyDataText(ItemData data, out float lineNum)
	{
		string textFromDict = "";
		string val = "";
		lineNum = 0;
		
		if (-1 != data.PropertyData.ShipSeaAttackMin 
			&& 0 != data.PropertyData.ShipSeaAttackMin)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100010);
			int seaAttackAve = Mathf.CeilToInt((data.PropertyData.ShipSeaAttackMin + data.PropertyData.ShipSeaAttackMax ) * 0.5f);
			val += (GUIFontColor.MilkyWhite + textFromDict + seaAttackAve + "\n");
			lineNum++;
		}
		
		if (-1 != data.PropertyData.ShipSubmarineAttackMin 
			&& 0 != data.PropertyData.ShipSubmarineAttackMin)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100011);
			int submarineAve = Mathf.CeilToInt((data.PropertyData.ShipSubmarineAttackMin + data.PropertyData.ShipSubmarineAttackMax ) * 0.5f);
			val += (GUIFontColor.MilkyWhite + textFromDict + submarineAve + "\n");
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipLife)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100007);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipLife + "\n";
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipDefense)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100006);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipDefense + "\n";
			lineNum++;
		}
		
		if (0 != data.PropertyData.ShipSkillAttack)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100005);
			val += GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipSkillAttack + "\n";
			lineNum++;
		}
		
		// Fu zhu bing zhuang
		if (-1 != data.PropertyData.ShipRange 
			&& 0 != data.PropertyData.ShipRange)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100012);
			val += (GUIFontColor.MilkyWhite + textFromDict + data.PropertyData.ShipRange + "\n");
			lineNum++;
		}
					
		if (-1 != data.PropertyData.ShipHitRate 
			&& 0 != data.PropertyData.ShipHitRate)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100013);
			
			float rate = (float)data.PropertyData.ShipHitRate / 100.0f;
			val += (GUIFontColor.MilkyWhite + textFromDict + rate.ToString("F2") + "%\n");
			lineNum++;
		}
					
		if (-1 != data.PropertyData.ShipCritRate 
			&& 0 != data.PropertyData.ShipCritRate)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100014);
			
			float rate = (float)data.PropertyData.ShipCritRate / 100.0f;
			val += (GUIFontColor.MilkyWhite + textFromDict + rate.ToString("F2") + "%\n");
			lineNum++;
		}
					
		if (-1 != data.PropertyData.ShipDodgeRate 
			&& 0 != data.PropertyData.ShipDodgeRate)
		{
			textFromDict = Globals.Instance.MDataTableManager.GetWordText(21100015);
			
			float rate = (float)data.PropertyData.ShipDodgeRate / 100.0f;
			val += (GUIFontColor.MilkyWhite + textFromDict + rate.ToString("F2") + "%\n");
			lineNum++;
		}
		
		return val;
		
	}
	
	private string GetShipEquipPropertyDataText(ItemData data, out float lineNum)
	{
		lineNum = 0;
		
		string textFromDict = "";
		string val = "";
		
	
		
		return val;
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
	
	private void ShowGeneralQualityStar(int quality)
	{
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
	protected SpriteText _mNameText;
	protected SpriteText _mContentText;
	
	private bool _mIsAutoHide;
	private float _mActiveTime;
	private float _mDurationTime;
	
	private Dictionary<GameObject, float> _mQualityStarList;
	private Dictionary<GameObject, float> _mCutLineList;
	
	private ItemSlotData _mTipsData;
}
