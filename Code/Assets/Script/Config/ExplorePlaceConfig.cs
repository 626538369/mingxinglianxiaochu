using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ExplorePlaceConfig : ConfigBase {
	
	public class ExplorePlaceInfo
	{
		public int  placeID;
		public string pictureName;
	};

	public override bool Load (SecurityElement element)
	{
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				// server
				ExplorePlaceInfo explorePlaceInfo = new ExplorePlaceInfo();
				explorePlaceInfo.placeID = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Place_ID"),""), -1);
				explorePlaceInfo.pictureName = StrParser.ParseStr(childrenElement.Attribute("Plcace_Picture"),"");
				_mPlaceObjectDic.Add(explorePlaceInfo.placeID ,explorePlaceInfo);
			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out ExplorePlaceInfo itemElement)
	{
		itemElement = null;
		if (!_mPlaceObjectDic.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	protected Dictionary<int, ExplorePlaceInfo> _mPlaceObjectDic = new  Dictionary<int, ExplorePlaceInfo>();
}
