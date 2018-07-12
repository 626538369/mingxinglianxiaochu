using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class GameEndConfig : ConfigBase
{
	public class GameEndElement
	{
		public int ID ;
		public string End_Pic;
		public string Get_Task;

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
					GameEndElement itemElement;
					if (!LoadGameEndElement(childrenElement, out itemElement))
						continue;
					
					_mGameEndElementList[itemElement.ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadGameEndElement(SecurityElement element, out GameEndElement itemElement)
	{
		itemElement = new GameEndElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("End_Pic");
		if (attribute != null)
			itemElement.End_Pic = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Get_Task");
		if (attribute != null)
			itemElement.Get_Task = StrParser.ParseStr(attribute, "");
		
		return true;
	}


	public Dictionary<int, GameEndElement> GetGameEndElementList()
	{
		return _mGameEndElementList;
	}
	
	private Dictionary<int, GameEndElement> _mGameEndElementList = new Dictionary<int, GameEndElement>();
}
