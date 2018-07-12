using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class SubjectExtraRewardConfig : ConfigBase
{
	public class SubjectRewardElement
	{
		public int SubjectRewardID;
		public int RewardType;
		public int LimitTerm;
		public int PropertyType;
		public bool RewardIsPercent;
		public float RewardValue;
		public string RewardTips;
		public string RewardPicture;
		public string RewardEffect;
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
					SubjectRewardElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.SubjectRewardID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out SubjectRewardElement itemElement)
	{
		itemElement = new SubjectRewardElement();
		
		string attribute = element.Attribute("Extra_Reward_ID");
		if (attribute != null)
			itemElement.SubjectRewardID = StrParser.ParseDecInt(attribute, -1);
		

		attribute = element.Attribute("Extra_Reward_Type");
		if (attribute != null)
			itemElement.RewardType = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Limit_Term");
		if (attribute != null)
			itemElement.LimitTerm = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Reward_Type");
		if (attribute != null)
			itemElement.PropertyType = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Reward_Is_Percent");
		if (attribute != null)
			itemElement.RewardIsPercent = StrParser.ParseBool(attribute,true);
		
		attribute = element.Attribute("Reward_Param");
		if (attribute != null)
			itemElement.RewardValue = StrParser.ParseFloat(attribute, -1);
		
		attribute = element.Attribute("Reward_Intro");
		if (attribute != null)
			itemElement.RewardTips = attribute;
		
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out SubjectRewardElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	private Dictionary<int, SubjectRewardElement> _mItemElementList = new Dictionary<int, SubjectRewardElement>();
}
