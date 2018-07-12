using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Publish_Npc : ConfigBase
{
	public class Publish_NpcObj
	{
		public int Publish_ID; 
		public int Type;
		public int Score;
		public string Name;
		public string Actors;
	}
	
	private bool LoadItemElement(SecurityElement element, out Publish_NpcObj itemElement)
	{
		itemElement = new Publish_NpcObj();
		string attribute = element.Attribute("Publish_ID");
		if (attribute != null)
			itemElement.Publish_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type");
		if (attribute != null)
			itemElement.Type = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Score");
		if (attribute != null)
			itemElement.Score = StrParser.ParseDecInt(attribute, 0);

		attribute = element.Attribute("Name");
		if (attribute != null)
			itemElement.Name = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Actors");
		if (attribute != null)
			itemElement.Actors = StrParser.ParseStr(attribute, "");
		
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
					Publish_NpcObj itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mPublish_NpcDic[itemElement.Publish_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	public bool GetPublish_NpcObject(int publishID, out Publish_NpcObj publish_NpcObject)
	{

		publish_NpcObject = null;
		
		if (!_mPublish_NpcDic.TryGetValue(publishID, out publish_NpcObject))
		{
			return false;
		}
		return true;
	}
	protected Dictionary<int, Publish_NpcObj> _mPublish_NpcDic = new  Dictionary<int, Publish_NpcObj>();

}
