using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Item info slot. Use in Shop and ShipEquipChange
/// </summary>
public class ItemInfoSlot : MonoBehaviour 
{
	public UIToggle eventBtn;
	
	public PackageItemIconSlot itemIconSlot;
	public SpriteText name;
	public SpriteText pinzhi;
	
	public SpriteText price;
	public SpriteText pingfen;
	public SpriteText strengthen;
	
	public SpriteText property;
	public SpriteText desc;
	public SpriteText prompt;
	
	public SpriteText hasEquipedText;
	public PackedSprite yinLiangIcon;
	
	[HideInInspector] public ItemSlotData	ItemData;
	
	void Awake()
	{
		itemIconSlot.IgnoreCollider(true);
	}
	
	public void SetChecked(bool check)
	{
		eventBtn.isChecked = check;
	}
	
	public void UpdateSlot(ItemSlotData data)
	{
		ItemData = data;
		itemIconSlot.SetItemData(data);
		
		name.Text = data.MItemData.GetDisplayName();
		pinzhi.Text = data.MItemData.GetDisplayQualityName();

		if (data.SlotType == ItemSlotType.SHOP)
		{
			yinLiangIcon.transform.localScale = Vector3.one;
			price.transform.localScale = Vector3.one;
			
			desc.transform.localScale = Vector3.one;
			desc.Text = data.MItemData.BasicData.Description;
			
			property.transform.localScale = Vector3.zero;
			prompt.transform.localScale = Vector3.zero;
			pingfen.transform.localScale = Vector3.zero;
			strengthen.transform.localScale = Vector3.zero;
		}
		else if (data.SlotType == ItemSlotType.CLOTH_BAG
			|| data.SlotType == ItemSlotType.TEMP_BAG
			|| data.SlotType == ItemSlotType.EQU_CARD_BAG)
		{
			yinLiangIcon.transform.localScale = Vector3.zero;
			price.transform.localScale = Vector3.zero;
			desc.transform.localScale = Vector3.zero;
			pingfen.transform.localScale = Vector3.zero;
			
			property.transform.localScale = Vector3.one;
			prompt.transform.localScale = Vector3.one;
			strengthen.transform.localScale = Vector3.one;
		}
		
		SetPropText(data);
		
		string tColor;
		if(Globals.Instance.MGameDataManager.MActorData.BasicData.Level >= data.MItemData.BasicData.UseLevelLimit){ 
			tColor = GUIFontColor.LimeGreen089210000;
		}else{
			tColor = GUIFontColor.DarkRed210000005;
		}						
		prompt.Text = tColor + string.Format(Globals.Instance.MDataTableManager.GetWordText(10829999), data.MItemData.BasicData.UseLevelLimit);
		

		if (data.SlotType == ItemSlotType.SHOP)
		{
			price.Text = HelpUtil.GetMoneyFormattedText(data.MItemData.BasicData.BuyPrice);
		}
		else if (data.SlotType == ItemSlotType.CLOTH_BAG
			|| data.SlotType == ItemSlotType.SHIP_EQUIPMENT)
		{
			price.Text = HelpUtil.GetMoneyFormattedText(data.MItemData.BasicData.SellPrice);
		}
		
		bool hasEquiped = false;
		if (hasEquiped)
			hasEquipedText.transform.parent.localScale = Vector3.one;
		else
			hasEquipedText.transform.parent.localScale = Vector3.zero;
	}
	
	void SetPropText(ItemSlotData data)
	{
		if (null == data.MItemData.PropertyData)
		{
			property.Text = "";
			return;
		}
		
		Dictionary<string, int> props = new Dictionary<string, int>();
		string propText = "";
		string spaceText = "	";
		if (-1 != data.MItemData.PropertyData.ShipSeaAttack && 0 != data.MItemData.PropertyData.ShipSeaAttack)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030001);
			props.Add(word, data.MItemData.PropertyData.ShipSeaAttack);
		}
		if (-1 != data.MItemData.PropertyData.ShipDefense && 0 != data.MItemData.PropertyData.ShipDefense)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030002);
			props.Add(word, data.MItemData.PropertyData.ShipDefense);
		}
		if (-1 != data.MItemData.PropertyData.ShipLife && 0 != data.MItemData.PropertyData.ShipLife)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030003);
			props.Add(word, data.MItemData.PropertyData.ShipLife);
		}
		if (-1 != data.MItemData.PropertyData.ShipFillSpeed && 0 != data.MItemData.PropertyData.ShipFillSpeed)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030004);
			props.Add(word, data.MItemData.PropertyData.ShipFillSpeed);
		}
		if (-1 != data.MItemData.PropertyData.ShipHitRate && 0 != data.MItemData.PropertyData.ShipHitRate)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030005);
			props.Add(word, data.MItemData.PropertyData.ShipHitRate / 100);
		}
		if (-1 != data.MItemData.PropertyData.ShipDodgeRate && 0 != data.MItemData.PropertyData.ShipDodgeRate)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030006);
			props.Add(word, data.MItemData.PropertyData.ShipDodgeRate / 100);
		}
		if (-1 != data.MItemData.PropertyData.ShipCritRate && 0 != data.MItemData.PropertyData.ShipCritRate)
		{
			string word = Globals.Instance.MDataTableManager.GetWordText(11030007);
			props.Add(word, data.MItemData.PropertyData.ShipCritRate / 100);
		}
		
		int i = 0;
		foreach (KeyValuePair<string, int> keyVal in props)
		{
			if (i == 0)
			{
				propText += keyVal.Key + keyVal.Value.ToString();
			}
			else
			{
				propText += spaceText + keyVal.Key + keyVal.Value.ToString();
			}
			
			i++;
		}
		
		property.Text = propText;
	}
	
	public void SetItemDescTips(ItemSlotData itemData, bool clearFormer = true)
	{
	}
	
	public void Hide(bool hide)
	{			
		gameObject.SetActiveRecursively(!hide);
	}
}
