using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ModelRoundThemeConfig : ConfigBase
{
	public class ModelRoundThemeObject
	{
	
		public int _mThemeID;
		public int _mThemeType;
		public int _NeedScoreBase;
		public int _RewardType;
		public int _RewardNumBase;
		public string _ThemeName;
		public string _ThemeDescription;
		
		public int NatureType1;
		public int NatureType2;
		public int NaturePercent1;
		public int NaturePercent2;
		public int StylePercent;
		
		public  Dictionary<int,int> MaterialToPercentDict = new Dictionary<int, int>();
		
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
					ModelRoundThemeObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_modelRoundThemeObject[itemElement._mThemeID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	
	
	
	
	private bool LoadItemElement(SecurityElement element, out ModelRoundThemeObject itemElement)
	{
		itemElement = new ModelRoundThemeObject();
		
		string attribute = element.Attribute("Theme_ID");
		if (attribute != null)
			itemElement._mThemeID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Theme_Type");
		if (attribute != null)
			itemElement._mThemeType = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Need_Score_Base");
		if (attribute != null)
			itemElement._NeedScoreBase = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Reward_Type");
		if (attribute != null)
			itemElement._RewardType = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Reward_Num_Base");
		if (attribute != null)
			itemElement._RewardNumBase = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Theme_Name");
		if (attribute != null)
			itemElement._ThemeName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Theme_Description");
		if (attribute != null)
			itemElement._ThemeDescription = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Style_Percent");
		if (attribute != null)
			itemElement.StylePercent = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Nature_Type_1");
		if (attribute != null)
			itemElement.NatureType1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Percent_1");
		if (attribute != null)
			itemElement.NaturePercent1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Type_2");
		if (attribute != null)
			itemElement.NatureType2 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Percent_2");
		if (attribute != null)
			itemElement.NaturePercent2 = StrParser.ParseDecInt(attribute, 0);
		
		
		int keyInt = 0;
		int valueInt = 0;
		attribute = element.Attribute("Style1");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style1_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style2");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style2_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);	
		attribute = element.Attribute("Style3");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style3_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style4");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style4_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style5");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style5_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);	
		attribute = element.Attribute("Style6");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style6_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style7");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style7_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);	
		attribute = element.Attribute("Style8");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style8_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style9");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style9_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		attribute = element.Attribute("Style10");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style10_Weight");
		if (attribute != null)
			valueInt = StrParser.ParseDecInt(attribute, 0);
		itemElement.MaterialToPercentDict.Add(keyInt,valueInt);
		return true;
	}
	
	
	
	
	
	
	public bool GetModelRoundThemeObject(int ThemeID, out ModelRoundThemeObject ThemeObject)
	{
		ThemeObject = null;
		
		if (!_modelRoundThemeObject.TryGetValue(ThemeID, out ThemeObject))
		{
			return false;
		}
		return true;
	}
	
	
	protected Dictionary<int,ModelRoundThemeObject> _modelRoundThemeObject = new  Dictionary<int, ModelRoundThemeObject>();

}
