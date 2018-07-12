using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ModelRoundLevelConfig : ConfigBase
{
	public class ModelRoundLevelObject
	{
		public int Level;
		public int Need_Level;
		public int Need_Score_Weight;
		public int Reward_Money_Weight;
		public int Reward_Exp_Weight;
		public Dictionary<int ,string > natureItemInfo = new Dictionary<int ,string>();
	
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
					ModelRoundLevelObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_modelRoundLevelObject[itemElement.Level] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out ModelRoundLevelObject itemElement)
	{
		itemElement = new ModelRoundLevelObject();
		
		string attribute = element.Attribute("Level");
		if (attribute != null)
			itemElement.Level = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Need_Level");
		if (attribute != null)
			itemElement.Need_Level = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Need_Score_Weight");
		if (attribute != null)
			itemElement.Need_Score_Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Reward_Money_Weight");
		if (attribute != null)
			itemElement.Reward_Money_Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Reward_Exp_Weight");
		if (attribute != null)
			itemElement.Reward_Exp_Weight = StrParser.ParseDecInt(attribute, 0);
		
		int keyInt = 0;
		string valueStr = "";
		attribute = element.Attribute("Level_Type1");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Level_Type1_Text");
		if (attribute != null)
			valueStr = StrParser.ParseStr(attribute, "");
		itemElement.natureItemInfo.Add(keyInt,valueStr);
		attribute = element.Attribute("Level_Type2");
		if (attribute != null)
			keyInt = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Level_Type2_Text");
		if (attribute != null)
			valueStr = StrParser.ParseStr(attribute, "");
		itemElement.natureItemInfo.Add(keyInt,valueStr);
		
		
		return true;
	}
	
	public bool GetModelRoundLevelObject(int levelID, out ModelRoundLevelObject levelObject)
	{
		levelObject = null;
		
		if (!_modelRoundLevelObject.TryGetValue(levelID, out levelObject))
		{
			return false;
		}
		return true;
	}
	
	protected Dictionary<int,ModelRoundLevelObject> _modelRoundLevelObject = new  Dictionary<int, ModelRoundLevelObject>();

}
