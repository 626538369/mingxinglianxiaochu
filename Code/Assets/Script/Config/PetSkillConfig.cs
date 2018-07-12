using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class PetSkillConfig : ConfigBase
{
	public class PetSkillConfigObject
	{
		public int Pet_Skill_ID;
		public int Add_Type;
		public int Add_Num;
		public int Weight;
	}
	
	private bool LoadItemElement(SecurityElement element, out PetSkillConfigObject itemElement)
	{
		itemElement = new PetSkillConfigObject();
		string attribute = element.Attribute("Pet_Skill_ID");
		if (attribute != null)
			itemElement.Pet_Skill_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Add_Type");
		if (attribute != null)
			itemElement.Add_Type = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Add_Num");
		if (attribute != null)
			itemElement.Add_Num = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Weight");
		if (attribute != null)
			itemElement.Weight = StrParser.ParseDecInt(attribute, 0);
		
		
		
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
					PetSkillConfigObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mPetSkillConfigObjectDic.Add(itemElement);
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	public List<PetSkillConfigObject> PetSkillDictionary()
	{
		return _mPetSkillConfigObjectDic;
	}

//	protected Dictionary<int, PetSkillConfigObject> _mPetSkillConfigObjectDic = new  Dictionary<int, PetSkillConfigObject>();
	
	protected List<PetSkillConfigObject> _mPetSkillConfigObjectDic = new List<PetSkillConfigObject>();
}
