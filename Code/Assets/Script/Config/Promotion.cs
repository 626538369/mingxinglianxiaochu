using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class Promotion : ConfigBase
{
	public class PromotionElement
	{
		public int Promotion_ID ;
		public int LineDay_Begin;
		public int LineDay_End;
		public string Reward;
		public int Free_Chest;
		public int Original_Price;
		public int Discount_Price;
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
					 PromotionElement itemElement;
					if (!LoadPromotionElement(childrenElement, out itemElement))
						continue;
					
					_mPromotionElementList[itemElement.Promotion_ID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}
	
	private bool LoadPromotionElement(SecurityElement element, out PromotionElement itemElement)
	{
		itemElement = new PromotionElement();
		string attribute = element.Attribute("Promotion_ID");
		if (attribute != null)
			itemElement.Promotion_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("LineDay_Begin");
		if (attribute != null)
			itemElement.LineDay_Begin = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("LineDay_End");
		if (attribute != null)
			itemElement.LineDay_End = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Reward");
		if (attribute != null)
			itemElement.Reward = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Free_Chest");
		if (attribute != null)
			itemElement.Free_Chest = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Original_Price");
		if (attribute != null)
			itemElement.Original_Price = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Discount_Price");
		if (attribute != null)
			itemElement.Discount_Price = StrParser.ParseDecInt(attribute, -1);
		return true;
	}


	public Dictionary<int, PromotionElement> GetPromotionElementList()
	{
		return _mPromotionElementList;
	}

	public PromotionElement getPromotionElement(int promotionId)
	{
		PromotionElement promotionElement = null;
		_mPromotionElementList.TryGetValue(promotionId,out promotionElement);
		return promotionElement;
	}

	private Dictionary<int, PromotionElement> _mPromotionElementList = new Dictionary<int, PromotionElement>();
}
