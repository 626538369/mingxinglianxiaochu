using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ShopRankExpConfig : ConfigBase
{
	public class ShopRankExpObject
	{
		public int Shop_Level;
		public int Exp; 
		public string Can_Fresh_Type;
		public int Can_Refresh_Num;
		public string Shop_Des;
	}
	
	private bool LoadItemElement(SecurityElement element, out ShopRankExpObject itemElement)
	{
		itemElement = new ShopRankExpObject();
		string attribute = element.Attribute("Shop_Level");
		if (attribute != null)
			itemElement.Shop_Level = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Exp");
		if (attribute != null)
		itemElement.Exp = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Can_Fresh_Type");
		if (attribute != null)
			itemElement.Can_Fresh_Type = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Can_Refresh_Num");
		if (attribute != null)
			itemElement.Can_Refresh_Num = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Shop_Des");
		if (attribute != null)
			itemElement.Shop_Des = StrParser.ParseStr(attribute, "");
		
		return true;
	}
	
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
					ShopRankExpObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mShopRankExpDic[itemElement.Shop_Level] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}	
	
	public bool GetRankExpObject(int Level, out ShopRankExpObject RankExpObject)
	{
		RankExpObject = null;
		
		if (!_mShopRankExpDic.TryGetValue(Level, out RankExpObject))
		{
			return false;
		}
		return true;
	}
	
	public int GetlLevelRankExp(int Level)
	{
		return _mShopRankExpDic[Level].Exp;
	}
	

	protected Dictionary<int, ShopRankExpObject> _mShopRankExpDic = new  Dictionary<int, ShopRankExpObject>();
	
}
