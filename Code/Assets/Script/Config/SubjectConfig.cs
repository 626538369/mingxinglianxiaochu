using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class SubjectConfig : ConfigBase
{
	public class SubjectElement
	{
		public int SubjectID;
		public int TermID;
		public string SubjectIcon;
		public int SubjectNameID;
		public int SubjectTipsID;
		public int SubjectType;
		public int MustSelect;
		public int EnergyNeed;
		public int TotalScore;
		public int LiteratureGet;
		public int MathGet;
		public int LanguageGet;
		public int TechnologyGet;
		public int SportsGet;
		public int ArtGet;
		public int RewardExp;
		public List<int> PropertyValueList = new List<int>();
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
					SubjectElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.SubjectID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out SubjectElement itemElement)
	{
		itemElement = new SubjectElement();
		
		string attribute = element.Attribute("Subject_ID");
		if (attribute != null)
			itemElement.SubjectID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Term_ID");
		if (attribute != null)
			itemElement.TermID = StrParser.ParseDecInt(attribute, -1);
				
		attribute = element.Attribute("Subject_Name");
		if (attribute != null)
			itemElement.SubjectNameID = StrParser.ParseDecInt(attribute, -1);
	
		attribute = element.Attribute("Subject_Describe");
		if (attribute != null)
			itemElement.SubjectTipsID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Subject_Icon");
		if (attribute != null)
			itemElement.SubjectIcon = attribute;	
		
		
		attribute = element.Attribute("Subject_Type");
		if (attribute != null)
			itemElement.SubjectType = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Energy_Need");
		if (attribute != null)
			itemElement.EnergyNeed = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Must_Select");
		if (attribute != null)
			itemElement.MustSelect = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Total_Score");
		if (attribute != null)
			itemElement.TotalScore = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("LiteratureGet");
		if (attribute != null)
			itemElement.LiteratureGet = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("MathGet");
		if (attribute != null)
			itemElement.MathGet = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("LanguageGet");
		if (attribute != null)
			itemElement.LanguageGet = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("TechnologyGet");
		if (attribute != null)
			itemElement.TechnologyGet = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("SportsGet");
		if (attribute != null)
			itemElement.SportsGet = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("ArtGet");
		if (attribute != null)
			itemElement.ArtGet = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Reward_Exp");
		if (attribute != null)
			itemElement.RewardExp = StrParser.ParseDecInt(attribute, -1);
		
		
		itemElement.PropertyValueList.Add(itemElement.LiteratureGet);
		itemElement.PropertyValueList.Add(itemElement.MathGet);
		itemElement.PropertyValueList.Add(itemElement.LanguageGet);
		itemElement.PropertyValueList.Add(itemElement.TechnologyGet);
		itemElement.PropertyValueList.Add(itemElement.SportsGet);
		itemElement.PropertyValueList.Add(itemElement.ArtGet);

		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out SubjectElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	private Dictionary<int, SubjectElement> _mItemElementList = new Dictionary<int, SubjectElement>();
}
