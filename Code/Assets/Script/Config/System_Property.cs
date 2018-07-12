using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class System_Property : ConfigBase
{
	public class SystemPropertyObject
	{
		public int ID;
		public string Conf_Name;
		public int Conf_Value;
		public string Msg;

	}
	
	private bool LoadItemElement(SecurityElement element, out SystemPropertyObject itemElement)
	{
		itemElement = new SystemPropertyObject();
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Conf_Name");
		if (attribute != null)
			itemElement.Conf_Name = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Conf_Value");
		if (attribute != null)
			itemElement.Conf_Value = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Msg");
		if (attribute != null)
			itemElement.Msg = StrParser.ParseStr(attribute, "");
		
		return true;
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
					SystemPropertyObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mSystemPropertyObjectDic[itemElement.ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
		
	
	public bool GetSystemPropertyObject(int ID, out SystemPropertyObject systemPropertyObject)
	{
		systemPropertyObject = null;
		
		if (!_mSystemPropertyObjectDic.TryGetValue(ID, out systemPropertyObject))
		{
			return false;
		}
		return true;
	}
	
	public int GetConfValue(int ID)
	{
		return _mSystemPropertyObjectDic[ID].Conf_Value;
	}

	protected Dictionary<int, SystemPropertyObject> _mSystemPropertyObjectDic = new  Dictionary<int, SystemPropertyObject>();
	
	
}
