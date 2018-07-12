using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class Task_Label : ConfigBase
{
	public class TaskLabelElement
	{
		public int Task_ID;
		public int Label_ID;
		public int Prob;
		public string Title;
		public int Give_Up;
		public int Group_Id;
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
					TaskLabelElement itemElement;
					if (!LoadTaskLabelElement(childrenElement, out itemElement))
						continue;
					
					_mTaskLabelElementList[itemElement.Label_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadTaskLabelElement(SecurityElement element, out TaskLabelElement itemElement)
	{
		itemElement = new TaskLabelElement();
		
		string attribute = element.Attribute("Task_ID");
		if (attribute != null)
			itemElement.Task_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Label_ID");
		if (attribute != null)
			itemElement.Label_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Prob");
		if (attribute != null)
			itemElement.Prob = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Title");
		if (attribute != null)
			itemElement.Title = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Give_Up");
		if (attribute != null)
			itemElement.Give_Up = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Group_Id");
		if (attribute != null)
			itemElement.Group_Id = StrParser.ParseDecInt(attribute, -1);

		return true;
	}
	
	public TaskLabelElement GetTaskLabelElement(int taskLabelID)
	{
		if(_mTaskLabelElementList.ContainsKey(taskLabelID))
		{
			return _mTaskLabelElementList[taskLabelID];
		}
		
		return null;
	}

	public Dictionary<int, TaskLabelElement> GetTaskLabelElementList()
	{
		return _mTaskLabelElementList;
	}
	
	private Dictionary<int, TaskLabelElement> _mTaskLabelElementList = new Dictionary<int, TaskLabelElement>();
}
