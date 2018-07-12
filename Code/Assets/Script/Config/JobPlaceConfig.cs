using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class JobPlaceConfig : ConfigBase
{
	public class JobPlaceElement
	{
		public int Job_Place_ID;
		public string Job_Place_Name;
		public int Need_Fans;
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
					JobPlaceElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.Job_Place_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out JobPlaceElement itemElement)
	{
		itemElement = new JobPlaceElement();
		
		string attribute = element.Attribute("Job_Place_ID");
		if (attribute != null)
			itemElement.Job_Place_ID = StrParser.ParseDecInt(attribute, -1);
	
		attribute = element.Attribute("Job_Place_Name");
		if (attribute != null)
			itemElement.Job_Place_Name = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Need_Fans");
		if (attribute != null)
			itemElement.Need_Fans = StrParser.ParseDecInt(attribute, -1);

		return true;
	}

	public Dictionary<int, JobPlaceElement> GetJobPlaceElementList()
	{
		return _mItemElementList;
	}

	private Dictionary<int, JobPlaceElement> _mItemElementList = new Dictionary<int, JobPlaceElement>();
}
