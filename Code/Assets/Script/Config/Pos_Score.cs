using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Pos_Score : ConfigBase
{
	public class PosObject
	{

		public int Pos_ID ;
		public string Pos1_NvName;
		public string Pos2_NvName;
		public string Pos1_NanName;
		public string Pos2_NanName;
		public string Pos_Pet;
		public string Pos_Icon;
		public string Pos_Name;
		public int Cost_Ingot;
		public int Cost_Money;
		public string Pos_Score;
		public int Score_Max;
		public string Act_Len;
		
		public string Pos1_Position;
		public string Pos1_Rotation;
		public string Pos2_Position;
		public string Pos2_Rotation;
	}
	
	private bool LoadItemElement(SecurityElement element, out PosObject itemElement)
	{
		itemElement = new PosObject();
		string attribute = element.Attribute("Pos_ID");
		if (attribute != null)
			itemElement.Pos_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Pos1_NvName");
		if (attribute != null)
			itemElement.Pos1_NvName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos2_NvName");
		if (attribute != null)
			itemElement.Pos2_NvName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos1_NanName");
		if (attribute != null)
			itemElement.Pos1_NanName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos2_NanName");
		if (attribute != null)
			itemElement.Pos2_NanName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos_Pet");
		if (attribute != null)
			itemElement.Pos_Pet = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos_Icon");
		if (attribute != null)
			itemElement.Pos_Icon = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos_Name");
		if (attribute != null)
			itemElement.Pos_Name = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos_Score");
		if (attribute != null)
			itemElement.Pos_Score = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Score_Max");
		if (attribute != null)
			itemElement.Score_Max = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Act_Len");
		if (attribute != null)
			itemElement.Act_Len = StrParser.ParseStr(attribute, "");
		
		
		attribute = element.Attribute("Cost_Ingot");
		if (attribute != null)
			itemElement.Cost_Ingot = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Cost_Money");
		if (attribute != null)
			itemElement.Cost_Money = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Pos1_Position");
		if (attribute != null)
			itemElement.Pos1_Position = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos1_Rotation");
		if (attribute != null)
			itemElement.Pos1_Rotation = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos2_Position");
		if (attribute != null)
			itemElement.Pos2_Position = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pos2_Rotation");
		if (attribute != null)
			itemElement.Pos2_Rotation = StrParser.ParseStr(attribute, "");
		
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
					PosObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mPosObjectDic[itemElement.Pos_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
		
	
	public bool GetTaskObject(int taskID, out PosObject posObject)
	{
		posObject = null;
		
		if (!_mPosObjectDic.TryGetValue(taskID, out posObject))
		{
			return false;
		}
		return true;
	}
	
	

	protected Dictionary<int, PosObject> _mPosObjectDic = new  Dictionary<int, PosObject>();
	
	
}
