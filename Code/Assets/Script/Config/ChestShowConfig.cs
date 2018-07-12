using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class ChestShowConfig : ConfigBase
{
	public class ChestShowElement
	{
		public int ID ;
		public int Chest_ID;
		public int Show_Item_Type;
		public int Show_Item_ID;
		public int Show_Item_Num;
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
					 ChestShowElement itemElement;
					if (!LoadChestShowElement(childrenElement, out itemElement))
						continue;
					
					_mChestShowElementList[itemElement.ID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}
	
	private bool LoadChestShowElement(SecurityElement element, out ChestShowElement itemElement)
	{
		itemElement = new ChestShowElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Chest_ID");
		if (attribute != null)
			itemElement.Chest_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Show_Item_Type");
		if (attribute != null)
			itemElement.Show_Item_Type = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Show_Item_ID");
		if (attribute != null)
			itemElement.Show_Item_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Show_Item_Num");
		if (attribute != null)
			itemElement.Show_Item_Num = StrParser.ParseDecInt(attribute, -1);
		return true;
	}


	public Dictionary<int, ChestShowElement> GetChestShowElementList()
	{
		return _mChestShowElementList;
	}
	
	private Dictionary<int, ChestShowElement> _mChestShowElementList = new Dictionary<int, ChestShowElement>();
}
