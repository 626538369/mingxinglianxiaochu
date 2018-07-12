using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class MoneyToDiamondConfig : ConfigBase
{
	public class MoneyToDiamondElement
	{
		public int ID ;
		public int Need_Money;
		public int Get_Diamond;
		public string Icon;
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
					MoneyToDiamondElement itemElement;
					if (!LoadMoneyToDiamondElement(childrenElement, out itemElement))
						continue;
					
					_mMoneyToDiamondElementList[itemElement.ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadMoneyToDiamondElement(SecurityElement element, out MoneyToDiamondElement itemElement)
	{
		itemElement = new MoneyToDiamondElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Need_Money");
		if (attribute != null)
			itemElement.Need_Money = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Diamond");
		if (attribute != null)
			itemElement.Get_Diamond = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Icon");
		if (attribute != null)
			itemElement.Icon = StrParser.ParseStr(attribute, "");

		return true;
	}
	
	public MoneyToDiamondElement GetMoneyToDiamondElement(int TrainID)
	{
		if(_mMoneyToDiamondElementList.ContainsKey(TrainID))
		{
			return _mMoneyToDiamondElementList[TrainID];
		}
		
		return null;
	}

	public Dictionary<int, MoneyToDiamondElement> GetMoneyToDiamondElementList()
	{
		return _mMoneyToDiamondElementList;
	}
	
	private Dictionary<int, MoneyToDiamondElement> _mMoneyToDiamondElementList = new Dictionary<int, MoneyToDiamondElement>();
}
