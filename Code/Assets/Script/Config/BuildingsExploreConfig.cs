using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class BuildingsExploreConfig : ConfigBase 
{
	public class BuildingsExploreElement
	{
		public int nBuildingID;
		public int nPlaceID;
		public string strBuildingName;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag!="Items")
			return false;
		if(element.Children !=null)
		{
			foreach(SecurityElement ChirldrenElement in element.Children)
			{
				if(ChirldrenElement.Tag=="Item")
				{
					BuildingsExploreElement ItemElement;
					if(!LoatItemElement(ChirldrenElement, out ItemElement))
						continue;
					_mItemElementList[ItemElement.nPlaceID] = ItemElement;
				}
			}
			return true;
		}
		
		return false;
	}
	
	private bool LoatItemElement(SecurityElement element,out BuildingsExploreElement ItemElement)
	{
		ItemElement = new BuildingsExploreElement();
		string attribute = element.Attribute("Building_ID");
		if(attribute!=null)
			ItemElement.nBuildingID = StrParser.ParseDecInt(attribute,-1);
		
		attribute = element.Attribute("Place_ID");
		if(attribute!=null)
			ItemElement.nPlaceID = StrParser.ParseDecInt(attribute,-1);
		attribute = element.Attribute("Building_Name");
		if(attribute!=null)
			ItemElement.strBuildingName = StrParser.ParseStr(attribute,"");
		return true;
	}
	
	public bool GetItemElement(int nPlaceID, out BuildingsExploreElement ItemElement)
	{
		ItemElement = null;
		if(!_mItemElementList.TryGetValue(nPlaceID, out ItemElement))
			return false;
		return true;
	}
	
	private Dictionary<int,BuildingsExploreElement> _mItemElementList = new Dictionary<int, BuildingsExploreElement>();
}
