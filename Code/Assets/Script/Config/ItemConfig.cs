using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class ItemConfig : ConfigBase
{
	public class ItemElement
	{
		public int LogicID;
		public int NameID;
		
		public int ItemCategory;
		public int ItemSmallCategory;
				
		public string Icon;
		public int DescriptionID;
		public string ModelName;
		
		public string OutTextureName0 = "";
		public string OutTextureName1 = "";
		public string OutColor = "";
		
		public int UseLevelLimit;
		public int UseRank;
		public int RarityLevel;
		public int StrengthenLevel;
		
		public int BuyPrice;
		public int SellPrice;
		public int ChangeValue;
		
		public int StackCountLimit;
		public bool IsPermitSell;
		public bool IsPermitDiscard;
		public bool IsGive;
		public bool IsUse;
		
		public int TakeBuffID;
		
//		public int RoleLiterature;
		public int RoleKnowledge;

		public int RoleMath;
		public int RoleLanguage;
		public int RoleTechnology;
//		public int RoleSports;
		public int RoleSport;
//		public int RoleArt;
		public int RoleAct;
		public int RoleContacts;
//		public int RoleCharm;
		public int RoleDeportment;

		public int RoleNowEnergy;
		public int RoleMaxEnergy;
		public int RoleNowLovePoint;
		public int RoleMaxLovePoint;
		public int RoleMinFortune;
		public int RoleNowFortune;
		public int GirlKnowledge;
		public int GirlContacts;
		public int GirlCharm;
		public int	ParamExtend;	
		public int GirlIntimacy;
		public int ItemNature;
		public int Item_Style;
		public int RoloAttributeID = -1;
		public int RoloAttributeValue;
		
		public int GirlAttributeID = -1;
		public int GirlAttributeValue;
		
		public int Pet_Max_Mp;
		public int Pet_Mp_GrowUp;
		public int Pet_Art;
		
		// public int GLEquipJuniorType;
		// public int ShipEquipJuniorType;
		// public int DoctrineEquipType;
		
		public string GiftPackage;
		public string FormulaUseItem;
		public string FormulaProduct;
		
		
		
		public string PictureBlueprint = "";
		public int nBeautyScore;
		public int nItemMaterial;
		
		public int SuitSex;
		public int Girl_Charm;
		public int Item_Theme;
	}
	
	public static string [] ItemAttributeIcon  = {"IconWenxue","IconShuxue","IconYingyu","IconJishu","IconTiyu","IconYishu","Role_Contacts","Role_Contacts","IconJingli","Role_Max_Energy","IconTaoxin1","Role_Max_LovePoint","Role_Min_Fortune","Role_Now_Fortune"};
	public static string [] ItemGirlAttributeIcon = {"Girl_Knowledge","Girl_Knowledge","Girl_Charm"};
	public static int [] ItemAttributeWordID  = {1,1,1,1,1,1,5014,5014,5015,5016,5017,5018,5019,5020};
	public static int [] ItemGirlAttributeWordID  = {5021,5021,5022};
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Item")
				{
					ItemElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.LogicID] = itemElement;
					_mItemElementListTotype[itemElement.ItemSmallCategory] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out ItemElement itemElement)
	{
		itemElement = new ItemElement();
		
		string attribute = element.Attribute("Item_ID");
		if (attribute != null)
			itemElement.LogicID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Rarity_Level");
		if (attribute != null)
			itemElement.RarityLevel = StrParser.ParseDecInt(attribute, -1);

		
		attribute = element.Attribute("Item_Name");
		if (attribute != null)
			itemElement.NameID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Item_Type");
		if (attribute != null)
			itemElement.ItemCategory = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Item_Type_Small");
		if (attribute != null)
			itemElement.ItemSmallCategory = StrParser.ParseDecInt(attribute, -1);
		

		attribute = element.Attribute("Item_Des");
		if (attribute != null)
			itemElement.DescriptionID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Icon");
		if (attribute != null)
			itemElement.Icon = attribute;
		
		attribute = element.Attribute("Model");
		if (attribute != null)
			itemElement.ModelName = attribute;
		
		attribute  = element.Attribute("Change_Texture0");
		if (attribute != null)
			itemElement.OutTextureName0 = attribute;
		
		attribute  = element.Attribute("Change_Texture1");
		if (attribute != null)
			itemElement.OutTextureName1 = attribute;
		
		attribute  = element.Attribute("Change_Color");
		if (attribute != null)
			itemElement.OutColor = attribute;
		
		attribute = element.Attribute("Use_Level");
		if (attribute != null)
			itemElement.UseLevelLimit = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Use_Rank");
		if (attribute != null)
			itemElement.UseRank = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Buy_Price");
		if (attribute != null)
			itemElement.BuyPrice = StrParser.ParseDecInt(attribute, -1);
		
		
		attribute = element.Attribute("Sell_Price");
		if (attribute != null)
			itemElement.SellPrice = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Change_Value");
		if (attribute != null)
			itemElement.ChangeValue = StrParser.ParseDecInt(attribute, -1);
		

		attribute = element.Attribute("Stack_Number");
		if (attribute != null)
			itemElement.StackCountLimit = StrParser.ParseDecInt(attribute, -1);
		
		
		attribute = element.Attribute("Is_Sell");
		if (attribute != null)
			itemElement.IsPermitSell = StrParser.ParseDecInt(attribute, 0) == 1;
		
		
		attribute = element.Attribute("Is_Discard");
		if (attribute != null)
			itemElement.IsPermitDiscard = StrParser.ParseDecInt(attribute, 0) == 1;
		
		
		attribute = element.Attribute("Is_Give");
		if (attribute != null)
			itemElement.IsGive = StrParser.ParseDecInt(attribute, 0) == 1;
		
		attribute = element.Attribute("Is_Use");
		if (attribute != null)
			itemElement.IsUse = StrParser.ParseDecInt(attribute, 0) == 1;
		
		
		attribute = element.Attribute("Role_Knowledge");
		if (attribute != null)
		{
			itemElement.RoleKnowledge = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleKnowledge != 0)
			{
				itemElement.RoloAttributeID = 0;
				itemElement.RoloAttributeValue = itemElement.RoleKnowledge ;
			}
		}
		
		
		attribute = element.Attribute("Role_Math");
		if (attribute != null)
		{
			itemElement.RoleMath = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleMath != 0)
			{
				itemElement.RoloAttributeID = 1;
				itemElement.RoloAttributeValue = itemElement.RoleMath ;
			}
		}
		
		attribute = element.Attribute("Role_Language");
		if (attribute != null)
		{
			itemElement.RoleLanguage = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleLanguage != 0)
			{
				itemElement.RoloAttributeID = 2;
				itemElement.RoloAttributeValue = itemElement.RoleLanguage ;
			}
		}
		
		attribute = element.Attribute("Role_Technology");
		if (attribute != null)
		{
			itemElement.RoleTechnology = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleTechnology != 0)
			{
				itemElement.RoloAttributeID = 3;
				itemElement.RoloAttributeValue = itemElement.RoleTechnology ;
			}
		}
		
		attribute = element.Attribute("Role_Sport");
		if (attribute != null)
		{
			itemElement.RoleSport = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleSport != 0)
			{
				itemElement.RoloAttributeID = 4;
				itemElement.RoloAttributeValue = itemElement.RoleSport ;
			}
		}
		
		attribute = element.Attribute("Role_Act");
		if (attribute != null)
		{
			itemElement.RoleAct = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleAct != 0)
			{
				itemElement.RoloAttributeID = 5;
				itemElement.RoloAttributeValue = itemElement.RoleAct ;
			}
		}
		
		attribute = element.Attribute("Role_Contacts");
		if (attribute != null)
		{
			itemElement.RoleContacts = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleContacts != 0)
			{
				itemElement.RoloAttributeID = 6;
				itemElement.RoloAttributeValue = itemElement.RoleContacts ;
			}
		}
		
		attribute = element.Attribute("Role_Deportment");
		if (attribute != null)
		{
			itemElement.RoleDeportment = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleDeportment != 0)
			{
				itemElement.RoloAttributeID = 7;
				itemElement.RoloAttributeValue = itemElement.RoleDeportment ;
			}
		}
		
		attribute = element.Attribute("Role_Now_Energy");
		if (attribute != null)
		{
			itemElement.RoleNowEnergy = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleNowEnergy != 0)
			{
				itemElement.RoloAttributeID = 8;
				itemElement.RoloAttributeValue = itemElement.RoleNowEnergy ;
			}
		}
		
		attribute = element.Attribute("Role_Max_Energy");
		if (attribute != null)
		{
			itemElement.RoleMaxEnergy = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleMaxEnergy != 0)
			{
				itemElement.RoloAttributeID = 9;
				itemElement.RoloAttributeValue = itemElement.RoleMaxEnergy;
			}
		}
		
		attribute = element.Attribute("Role_Now_LovePoint");
		if (attribute != null)
		{
			itemElement.RoleNowLovePoint = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleNowLovePoint != 0)
			{
				itemElement.RoloAttributeID = 10;
				itemElement.RoloAttributeValue = itemElement.RoleNowLovePoint;
			}
		}
		
		attribute = element.Attribute("Role_Max_LovePoint");
		if (attribute != null)
		{
			itemElement.RoleMaxLovePoint = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleMaxLovePoint != 0)
			{
				itemElement.RoloAttributeID = 11;
				itemElement.RoloAttributeValue = itemElement.RoleMaxLovePoint;
			}
		}
		
		attribute = element.Attribute("Role_Min_Fortune");
		if (attribute != null)
		{
			itemElement.RoleMinFortune = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleMinFortune != 0)
			{
				itemElement.RoloAttributeID = 12;
				itemElement.RoloAttributeValue = itemElement.RoleMinFortune;
			}
		}
		
		attribute = element.Attribute("Role_Now_Fortune");
		if (attribute != null)
		{
			itemElement.RoleNowFortune = StrParser.ParseDecInt(attribute, 0);
			if(itemElement.RoleNowFortune != 0)
			{
				itemElement.RoloAttributeID = 13;
				itemElement.RoloAttributeValue = itemElement.RoleNowFortune;
			}
		}
		

		attribute = element.Attribute("Girl_Knowledge");
		if (attribute != null)
		{
			itemElement.GirlKnowledge = StrParser.ParseDecInt(attribute, 0);
			if (itemElement.GirlKnowledge != 0)
			{
				itemElement.GirlAttributeID = 0;
				itemElement.GirlAttributeValue = itemElement.GirlKnowledge;
			}
		}
		
		attribute = element.Attribute("Girl_Contacts");
		if (attribute != null)
		{
			itemElement.GirlContacts = StrParser.ParseDecInt(attribute, 0);
			if (itemElement.GirlContacts != 0)
			{
				itemElement.GirlAttributeID = 1;
				itemElement.GirlAttributeValue = itemElement.GirlContacts;
			}
		}

		
		attribute = element.Attribute("Girl_Charm");//
		if (attribute != null)
		{
			itemElement.GirlCharm = StrParser.ParseDecInt(attribute, 0);
			if (itemElement.GirlCharm != 0)
			{
				itemElement.GirlAttributeID = 2;
				itemElement.GirlAttributeValue = itemElement.GirlCharm;
			}
		}
		attribute = element.Attribute("Param_Extend");//
		if (attribute != null)
		{
			itemElement.ParamExtend = StrParser.ParseDecInt(attribute, 0);
			if (itemElement.ParamExtend != 0)
			{
				itemElement.GirlAttributeID = 3;
				itemElement.GirlAttributeValue = itemElement.ParamExtend;
			}
		}
		
		attribute = element.Attribute("Girl_Intimacy");
		if (attribute != null)
		{
			itemElement.GirlIntimacy = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Item_Nature");//
		if (attribute != null)
			itemElement.ItemNature = StrParser.ParseDecInt(attribute, 0);
				
		attribute = element.Attribute("Item_Style");//
		if (attribute != null)
			itemElement.Item_Style = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Item_Theme");//
		if (attribute != null)
			itemElement.Item_Theme = StrParser.ParseDecInt(attribute, 0);
		

		attribute = element.Attribute("Package_Info");
		if (attribute != null)
			itemElement.GiftPackage = attribute;
		
		
		attribute = element.Attribute("Consume_Info");
		if (attribute != null)
			itemElement.FormulaUseItem = attribute;
		
		attribute = element.Attribute("Produce_Info");
		if (attribute != null)
			itemElement.FormulaProduct = attribute;
		
		
		
		//attribute = element.Attribute("DurationTime");
		//if (attribute != null)
			//itemElement.StrengthenLevel = StrParser.ParseDecInt(attribute, -1);
		

		
		attribute = element.Attribute("Picture");
		if(attribute != null){
			itemElement.PictureBlueprint = attribute;
		}
		
		attribute = element.Attribute("Beauty_Score");
		if(attribute != null){
			itemElement.nBeautyScore = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Item_Material");
		if(attribute != null){
			itemElement.nItemMaterial = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Suit_Sex");
		if(attribute != null){
			itemElement.SuitSex = StrParser.ParseDecInt(attribute, 0);
		}
		attribute = element.Attribute("Girl_Charm");
		if(attribute != null){
			itemElement.Girl_Charm = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Pet_Max_Mp");
		if(attribute != null){
			itemElement.Pet_Max_Mp = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Pet_Art");
		if(attribute != null){
			itemElement.Pet_Art = StrParser.ParseDecInt(attribute, 0);
		}
		
		attribute = element.Attribute("Pet_Mp_GrowUp");
		if(attribute != null){
			itemElement.Pet_Mp_GrowUp = StrParser.ParseDecInt(attribute, 0);
		}
		
		
		return true;
	}
	
	public bool GetItemElement(long itemLogicID, out ItemElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	public bool GetItemElementToType(int itemSmallType, out ItemElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementListTotype.TryGetValue(itemSmallType, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	private Dictionary<long, ItemElement> _mItemElementList = new Dictionary<long, ItemElement>();
	private Dictionary<int, ItemElement> _mItemElementListTotype = new Dictionary<int, ItemElement>();
}
