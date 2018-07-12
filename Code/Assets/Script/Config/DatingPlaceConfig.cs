using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class DatingPlaceConfig : ConfigBase 
{
	public class DatingPlaceElement
	{
		public int nGirlID;
		public int nPlaceID;
		public string strPlaceName;
		public int nPlaceType;
		public int nPlaceLevel;
		public int nMinIntimacy;
		public int nMaxIntimacy;
		public int nLovePointSub;
		public int nItemIDSub;
		public int nItemNumSub;
		public int nQiaoDuanID;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		if(element.Children!=null)
		{
			foreach(SecurityElement ChildrenElement in element.Children)
			{
				if(ChildrenElement.Tag=="Item")
				{
					DatingPlaceElement itemElement = new DatingPlaceElement();
					itemElement.nGirlID = StrParser.ParseDecInt(ChildrenElement.Attribute("Girl_ID"),-1);
					itemElement.nPlaceID = StrParser.ParseDecInt(ChildrenElement.Attribute("Place_ID"),-1);
					itemElement.strPlaceName = StrParser.ParseStr(ChildrenElement.Attribute("Place_Name"),"");
					itemElement.nPlaceType = StrParser.ParseDecInt(ChildrenElement.Attribute("Place_Type"),-1);
					itemElement.nPlaceLevel = StrParser.ParseDecInt(ChildrenElement.Attribute("Place_Need_Level"),-1);
					itemElement.nMinIntimacy = StrParser.ParseDecInt(ChildrenElement.Attribute("Need_Min_Intimacy"),-1);
					itemElement.nMaxIntimacy = StrParser.ParseDecInt(ChildrenElement.Attribute("Need_Max_Intimacy"),-1);
					itemElement.nLovePointSub = StrParser.ParseDecInt(ChildrenElement.Attribute("LovePoint_Sub"),-1);
					itemElement.nItemIDSub = StrParser.ParseDecInt(ChildrenElement.Attribute("Item_ID_Sub"),-1);
					itemElement.nItemNumSub = StrParser.ParseDecInt(ChildrenElement.Attribute("Item_num_Sub"),-1);
					itemElement.nQiaoDuanID = StrParser.ParseDecInt(ChildrenElement.Attribute("QiaoDuan_ID"),-1);
					_mItemElementList[itemElement.nPlaceID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}
	
	public bool GetItemElement(int nID, out DatingPlaceElement itemElement)
	{
		itemElement = null;
		if(!_mItemElementList.TryGetValue(nID, out itemElement))
			return false;
		return true;
	}
	

	
   private Dictionary<int,DatingPlaceElement> _mItemElementList = new Dictionary<int, DatingPlaceElement>();

}
