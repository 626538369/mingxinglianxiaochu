using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ConstellationConfig : ConfigBase
{
	public class ConstellationElement
	{
		public int nID;
		public string StrName;
		public string StrIcon;
		public string StrDesc;
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
					ConstellationElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.nID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out ConstellationElement itemElement)
	{
		itemElement = new ConstellationElement();
		
		string attribute = element.Attribute("Constellation_ID");
		if (attribute != null)
			itemElement.nID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Constellation_Name");
		if (attribute != null)
			itemElement.StrName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Constellation_Icon");
		if (attribute != null)
			itemElement.StrIcon = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Constellation_Describe");
		if (attribute != null)
			itemElement.StrDesc = StrParser.ParseStr(attribute, "");
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out ConstellationElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	private Dictionary<int, ConstellationElement> _mItemElementList = new Dictionary<int, ConstellationElement>();
}