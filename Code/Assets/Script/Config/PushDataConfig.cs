using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class PushDataConfig : ConfigBase
{
	public class PushUIInfo
	{
		public string targetUIName;
		public Vector3 targetUIPosition;
	};
	public class PushElement
	{
		public int id;
		public int type;
		public int typeParam1;
		public int typeParam2;
		public List<PushUIInfo> PushUIInfoList = new List<PushUIInfo>();
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
					PushElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.id] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out PushElement itemElement)
	{
		itemElement = new PushElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.id = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("PushType");
		if (attribute != null)
			itemElement.type = StrParser.ParseDecInt(attribute, -1);
				
		attribute = element.Attribute("Param1");
		if (attribute != null)
			itemElement.typeParam1 = StrParser.ParseDecInt(attribute, -1);
	
		attribute = element.Attribute("Param2");
		if (attribute != null)
			itemElement.typeParam2 = StrParser.ParseDecInt(attribute, -1);
		

		
		attribute = element.Attribute("UIName1");
		if (attribute != null)
		{
			PushUIInfo aPushUIInfo = new PushUIInfo();
			aPushUIInfo.targetUIName = attribute;
			
			attribute = element.Attribute("Postion1");
			aPushUIInfo.targetUIPosition = StrParser.ParseVec3(attribute);
			itemElement.PushUIInfoList.Add(aPushUIInfo);
		}
		
		attribute = element.Attribute("UIName2");
		if (attribute != null)
		{
			PushUIInfo aPushUIInfo = new PushUIInfo();
			aPushUIInfo.targetUIName = attribute;
			
			attribute = element.Attribute("Postion2");
			aPushUIInfo.targetUIPosition = StrParser.ParseVec3(attribute);
			itemElement.PushUIInfoList.Add(aPushUIInfo);
		}
		
		attribute = element.Attribute("UIName3");
		if (attribute != null)
		{
			PushUIInfo aPushUIInfo = new PushUIInfo();
			aPushUIInfo.targetUIName = attribute;
			
			attribute = element.Attribute("Postion3");
			aPushUIInfo.targetUIPosition = StrParser.ParseVec3(attribute);
			itemElement.PushUIInfoList.Add(aPushUIInfo);
		}
		
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out PushElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	public int getItemElementCount()
	{
		return _mItemElementList.Count;
	}
	
	private Dictionary<int, PushElement> _mItemElementList = new Dictionary<int, PushElement>();
}
