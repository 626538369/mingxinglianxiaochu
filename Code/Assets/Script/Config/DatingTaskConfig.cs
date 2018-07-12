using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class DatingTaskConfig : ConfigBase 
{
	public class DatingTaskElement
	{
		public int nQiaoDuanID;
		public int nTaskID;
		public int nWeight;
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
					DatingTaskElement itemElement = new DatingTaskElement();
					itemElement.nQiaoDuanID = StrParser.ParseDecInt(element.Attribute("QiaoDuan_ID"),-1);
					itemElement.nTaskID = StrParser.ParseDecInt(element.Attribute("Task_ID"),-1);
					itemElement.nWeight = StrParser.ParseDecInt(element.Attribute("Weight"),-1);
					_mItemElementList[itemElement.nQiaoDuanID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}
	
	public bool GetItemElement(int nID, out DatingTaskElement itemElement)
	{
		itemElement = null;
		if(!_mItemElementList.TryGetValue(nID, out itemElement))
			return false;
		return true;
	}
	
   private Dictionary<int,DatingTaskElement> _mItemElementList = new Dictionary<int, DatingTaskElement>();
}
