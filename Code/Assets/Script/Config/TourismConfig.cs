using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class TourismConfig : ConfigBase
{
	public class TourismElement
	{
		public int TourismID;
		public string TourismName;
		public int NeedMoney;
		public int Reduce_Fatigue;
		public int Task_ID;
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
					TourismElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.TourismID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out TourismElement itemElement)
	{
		itemElement = new TourismElement();
		
		string attribute = element.Attribute("TourismID");
		if (attribute != null)
			itemElement.TourismID = StrParser.ParseDecInt(attribute, -1);
	
		attribute = element.Attribute("TourismName");
		if (attribute != null)
			itemElement.TourismName = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("NeedMoney");
		if (attribute != null)
			itemElement.NeedMoney = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Reduce_Fatigue");
		if (attribute != null)
			itemElement.Reduce_Fatigue = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Task_ID");
		if (attribute != null)
			itemElement.Task_ID = StrParser.ParseDecInt(attribute, -1);

		return true;
	}

	public Dictionary<int, TourismElement> GetTourismElementList()
	{
		return _mItemElementList;
	}

	private Dictionary<int, TourismElement> _mItemElementList = new Dictionary<int, TourismElement>();
}
