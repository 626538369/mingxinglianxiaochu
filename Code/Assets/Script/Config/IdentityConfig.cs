using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class IdentityConfig : ConfigBase
{
	public class IdentityElement
	{
		public int nID;
		public int nPay;
		public string StrName;
		public string StrDesc;
		public string StrIcon;
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
					IdentityElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.nID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out IdentityElement itemElement)
	{
		itemElement = new IdentityElement();
		
		string attribute = element.Attribute("Identity_ID");
		if (attribute != null)
			itemElement.nID = StrParser.ParseDecInt(attribute, -1);
		
	     attribute = element.Attribute("Need_Ingot");
		if (attribute != null)
			itemElement.nPay = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Identity_Name");
		if (attribute != null)
			itemElement.StrName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Identity_Describe");
		if (attribute != null)
			itemElement.StrDesc = StrParser.ParseStr(attribute, "");
			
		attribute = element.Attribute("Identity_Icon");
		if (attribute != null)
			itemElement.StrIcon = StrParser.ParseStr(attribute, "");
		
		if(itemElement.nID<=100)
		{
			nCount = itemElement.nID;
		}
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out IdentityElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	public bool GetItemElement(string avatarIconName, out IdentityElement itemElement)
	{
		itemElement = null;
		foreach (IdentityElement  identityElement in _mItemElementList.Values)
		{
			if (identityElement.StrIcon == avatarIconName)
			{
				itemElement = identityElement;
				return true;
			}
		}
		
		return false;
	}
	
	
	//xuyaoxianshenfendejishumax
	public int GetMapCount()
	{
		return nCount;
	}
	
	private Dictionary<int, IdentityElement> _mItemElementList = new Dictionary<int, IdentityElement>();
	private int    nCount = 0;
}

