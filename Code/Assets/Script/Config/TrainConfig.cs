using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class TrainConfig : ConfigBase
{
	public class TrainElement
	{
		public int Train_ID;
		public string Train_Name;
		public int Train_Type;
		public int Train_Lv;
		public int Need_Rate;
		public int Need_Money;
		public int Get_Act;
		public int Get_Sport;
		public int Get_Knowledge;
		public int Get_Deportment;
		public int Get_Fatigue;
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
					TrainElement itemElement;
					if (!LoadTrainElement(childrenElement, out itemElement))
						continue;
					
					_mTrainElementList[itemElement.Train_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadTrainElement(SecurityElement element, out TrainElement itemElement)
	{
		itemElement = new TrainElement();
		
		string attribute = element.Attribute("Train_ID");
		if (attribute != null)
			itemElement.Train_ID = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Train_Name");
		if (attribute != null)
			itemElement.Train_Name = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Train_Type");
		if (attribute != null)
			itemElement.Train_Type = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Train_Lv");
		if (attribute != null)
			itemElement.Train_Lv = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Need_Rate");
		if (attribute != null)
			itemElement.Need_Rate = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Need_Money");
		if (attribute != null)
			itemElement.Need_Money = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Act");
		if (attribute != null)
			itemElement.Get_Act = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Sport");
		if (attribute != null)
			itemElement.Get_Sport = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Knowledge");
		if (attribute != null)
			itemElement.Get_Knowledge = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Deportment");
		if (attribute != null)
			itemElement.Get_Deportment = StrParser.ParseDecInt(attribute, -1);
		attribute = element.Attribute("Get_Fatigue");
		if (attribute != null)
			itemElement.Get_Fatigue = StrParser.ParseDecInt(attribute, -1);
		return true;
	}
	
	public TrainElement GetTrainElement(int TrainID)
	{
		if(_mTrainElementList.ContainsKey(TrainID))
		{
			return _mTrainElementList[TrainID];
		}
		
		return null;
	}

	public Dictionary<int, TrainElement> GetTrainElementList()
	{
		return _mTrainElementList;
	}
	
	private Dictionary<int, TrainElement> _mTrainElementList = new Dictionary<int, TrainElement>();
}
