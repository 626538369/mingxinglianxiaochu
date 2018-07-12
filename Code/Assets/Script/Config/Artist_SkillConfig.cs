using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Artist_SkillConfig : ConfigBase
{
	public class SkillObject
	{
		public int Skill_ID
        {
            get { return _skillID; }
            set { _skillID = value; }
        }

        public int SkillType
        {
            get { return _skillType; }
            set { _skillType = value; }
        }

        public string Skill_Name
        {
            get { return _skillName; }
            set { _skillName = value; }
        }

        public string Skill_Desc
        {
            get { return _skillDesc; }
            set { _skillDesc = value; }
        }

        public string Skill_Icon
        {
            get { return _skillIcon; }
            set { _skillIcon = value; }
        }

        public int Skill_Damage_Type
        {
            get { return _skill_Damage_Type; }
            set { _skill_Damage_Type = value; }
        }

        public int Skill_Damage
        {
            get { return _skill_Damage; }
            set { _skill_Damage = value; }
        }
		public int Reward_Publish_Type
        {
            get { return _reward_Publish_Type; }
            set { _reward_Publish_Type = value; }
        }

		public static SkillObject Load(SecurityElement element)
		{
			SkillObject skillObject = new SkillObject();
			
			skillObject.Skill_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_ID"),""),-1);
			skillObject.SkillType = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SkillType"),""),-1);
			skillObject.Skill_Name =  StrParser.ParseStr(element.Attribute("Skill_Name"),"");
			skillObject.Skill_Desc = StrParser.ParseStr(element.Attribute("Skill_Desc"),"");
			skillObject.Skill_Icon = StrParser.ParseStr(element.Attribute("Skill_Icon"),"");
			skillObject.Skill_Damage_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Damage_Type"),""),-1);
			skillObject.Skill_Damage = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Damage"),""),-1);
			skillObject.Reward_Publish_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Publish_Type"),""),-1);

			return skillObject;
		}

        private int _skillID;
        private int _skillType;
        private string _skillName;
        private string _skillDesc;
        private string _skillIcon;
        private int _skill_Damage_Type;
		private int _skill_Damage;
		private int _reward_Publish_Type;
		
		/// <summary>
		/// tzz added for
		/// The skill fire general speak word.
		/// </summary>
		private int mSkillWord;
	}
	
    public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				SkillObject skillObject = SkillObject.Load(childrenElement);
				
				if (!_skillObjectDict.ContainsKey(skillObject.Skill_ID))
					_skillObjectDict[skillObject.Skill_ID] = skillObject;
			}
			return true;
		}
		return false;
	}
	
	// 2012.04.12 LiHaojie Add Get SkillObject interface
	public bool GetSkillObject(int skillID, out SkillObject skillObject)
	{
		skillObject = null;
		
		if (!_skillObjectDict.ContainsKey(skillID))
			return false;
		
		skillObject = _skillObjectDict[skillID];
		return true;
	}
	public SkillObject GetSkillObjectBySkill_Icon(string Skill_Icon)
	{
		foreach(SkillObject obj in _skillObjectDict.Values)
		{
			if(obj.Skill_Icon == Skill_Icon)
			{
				return obj;				
			}
		}
		return null;
	}
	
	protected Dictionary<int,SkillObject> _skillObjectDict = new  Dictionary<int, SkillObject>();
}
