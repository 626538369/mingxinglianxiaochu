using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Role_Rank_Feat : ConfigBase
{
	public class RoleRankFeatObject
	{
		public int Role_Rank;
		public int Item_ID;
		public int Feat;
		public int General_Command_Modify;
		public int General_Intelligence_Modify;
		public int General_Chivalrous_Modify;
		public int Max_Life_Modify;
		public int Max_Ship_Num;
		public int General_Skill;
		
		

	}
	
	private bool LoadItemElement(SecurityElement element, out RoleRankFeatObject itemElement)
	{
		itemElement = new RoleRankFeatObject();
		string attribute = element.Attribute("Role_Rank");
		if (attribute != null)
			itemElement.Role_Rank = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Item_ID");
		if (attribute != null)
			itemElement.Item_ID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Feat");
		if (attribute != null)
			itemElement.Feat = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("General_Command_Modify");
		if (attribute != null)
			itemElement.General_Command_Modify = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("General_Intelligence_Modify");
		if (attribute != null)
			itemElement.General_Intelligence_Modify = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("General_Chivalrous_Modify");
		if (attribute != null)
			itemElement.General_Chivalrous_Modify = StrParser.ParseDecInt(attribute, 0);

		attribute = element.Attribute("Max_Life_Modify");
		if (attribute != null)
			itemElement.Max_Life_Modify = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Max_Ship_Num");
		if (attribute != null)
			itemElement.Max_Ship_Num = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("General_Skill");
		if (attribute != null)
			itemElement.General_Skill = StrParser.ParseDecInt(attribute, 0);
		
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
					RoleRankFeatObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mRoleRankFeatObjectDic[itemElement.Role_Rank] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
		
	
	public bool GetRoleRankFeatObject(int mRole_Rank, out RoleRankFeatObject roleRankFeatObject)
	{
		roleRankFeatObject = null;
		
		if (!_mRoleRankFeatObjectDic.TryGetValue(mRole_Rank, out roleRankFeatObject))
		{
			return false;
		}
		return true;
	}
	
	public int GetCurrentGradeExp(int mRole_Rank)
	{
		if(mRole_Rank < 1)
		{
			return 0;
		}
		return _mRoleRankFeatObjectDic[mRole_Rank].Feat;
	}

	protected Dictionary<int, RoleRankFeatObject> _mRoleRankFeatObjectDic = new  Dictionary<int, RoleRankFeatObject>();
	
	
}
