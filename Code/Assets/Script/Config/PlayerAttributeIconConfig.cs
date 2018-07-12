using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class PlayerAttributeIconConfig : ConfigBase
{
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
					int attributeID = 0;
					string attributeIcon = "";
					string attribute = childrenElement.Attribute("AttributeID");
					if (attribute != null)
						attributeID = StrParser.ParseDecInt(attribute, -1);
					
					attribute = childrenElement.Attribute("AttributeIcon");
					if (attribute != null)
						attributeIcon = attribute;
					
					_mItemElementList[attributeID] = attributeIcon;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	
	public bool  GetItemElement(int itemLogicID,out string itemElement)
	{
		itemElement = "";
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	
	private Dictionary<int, string> _mItemElementList = new Dictionary<int, string>();
}
