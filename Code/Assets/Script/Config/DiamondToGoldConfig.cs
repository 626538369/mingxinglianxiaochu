using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class DiamondToGoldConfig : ConfigBase
{
	public class DiamondToGoldElement
	{
		public int ID ;
		public int Need_Diamond;
		public int Get_Gold;
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
					DiamondToGoldElement itemElement;
					if (!LoadDiamondToGoldElement(childrenElement, out itemElement))
						continue;
					
					_mDiamondToGoldElementList[itemElement.ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadDiamondToGoldElement(SecurityElement element, out DiamondToGoldElement itemElement)
	{
		itemElement = new DiamondToGoldElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Need_Diamond");
		if (attribute != null)
			itemElement.Need_Diamond = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Gold");
		if (attribute != null)
			itemElement.Get_Gold = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Icon");
		if (attribute != null)
			itemElement.Icon = StrParser.ParseStr(attribute, "");

		return true;
	}
	
	public DiamondToGoldElement GetDiamondToGoldElement(int TrainID)
	{
		if(_mDiamondToGoldElementList.ContainsKey(TrainID))
		{
			return _mDiamondToGoldElementList[TrainID];
		}
		
		return null;
	}

	public Dictionary<int, DiamondToGoldElement> GetDiamondToGoldElementList()
	{
		return _mDiamondToGoldElementList;
	}
	
	private Dictionary<int, DiamondToGoldElement> _mDiamondToGoldElementList = new Dictionary<int, DiamondToGoldElement>();
}
