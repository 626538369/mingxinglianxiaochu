using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Girl_SkillConfig : ConfigBase
{
	public class GirlSkillObject
	{
		public int SkillID
		{
			get{return _skillID;}
			set{_skillID = value;}		
		}
		public int GroupID
		{
			get{return _groupID;}
			set{_groupID = value;}
		}
		public int Skill_Name
		{
			get{return _skill_Name;}
			set{_skill_Name = value;}
		}
       	public string  Skill_Icon
		{
			get{return _skill_Icon;}
			set{_skill_Icon = value;}
		}
		public int Skill_Type
		{
			get{return _skill_Type;}
			set{_skill_Type = value;}
		}
		public int Skill_Level
		{
			get{return _skill_Level;}
			set{_skill_Level = value;}
		}
		public int Skill_Time
		{
			get{return _skill_Time;}
			set{_skill_Time = value;}
		}
		public int Reward_Type
		{
			get{return _reward_Type;}
			set{_reward_Type = value;}
		}
		public int Reward_Is_Percent
		{
			get{return _reward_Is_Percent;}
			set{_reward_Is_Percent = value;}
		}
		public int Reward_Count
		{
			get{return _reward_Count;}
			set{_reward_Count = value;}
		}
		public int Reward_Describe
		{
			get{return _reward_Describe;}
			set{_reward_Describe = value;}
		}
		public int Condition_Type
		{
			get{return _condition_Type;}
			set{_condition_Type = value;}
		}
		public int Condition_Item
		{
			get{return _condition_Item;}
			set{_condition_Item = value;}
		}	
		public int Condition_Count
		{
			get{return _condition_Count;}
			set{_condition_Count = value;}
		}
		public int Condition_Describe
		{
			get{return _condition_Describe;}
			set{_condition_Describe = value;}
		}
		public static GirlSkillObject Load (SecurityElement element)
		{
			GirlSkillObject girlskillgobject = new GirlSkillObject();	
			girlskillgobject.SkillID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_ID"),""),-1);
			girlskillgobject.GroupID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Group_ID"),""),-1);
			girlskillgobject.Skill_Name = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Name"),""),-1);
			girlskillgobject.Skill_Icon = StrParser.ParseStr(element.Attribute ("Skill_Icon"), "");
			girlskillgobject.Skill_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Type"),""),-1); 
			girlskillgobject.Skill_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Level"),""),-1); 
			girlskillgobject.Skill_Time = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Time"),""),-1); 
			girlskillgobject.Reward_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Type"),""),-1); 
			girlskillgobject.Reward_Is_Percent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Is_Percent"),""),-1); 
			girlskillgobject.Reward_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Count"),""),-1); 
			girlskillgobject.Reward_Describe = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Describe"),""),-1); 
			girlskillgobject.Condition_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Condition_Type"),""),-1); 
			girlskillgobject.Condition_Item = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Condition_Item"),""),-1); 
			girlskillgobject.Condition_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Condition_Count"),""),-1); 
			girlskillgobject.Condition_Describe = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Condition_Describe"),""),-1); 
//			girlcgobject.CG_ID= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("CG_ID"),""),-1);
//			girlcgobject.CGIcon = StrParser.ParseStr(element.Attribute ("Icon"), "");
			return girlskillgobject;
		}
		
		protected int _skillID;
		protected int _groupID;
		protected int _skill_Name;
		protected string  _skill_Icon;
		protected int _skill_Type;
		protected int _skill_Level;
		protected int _skill_Time;
		protected int _reward_Type;
		protected int _reward_Is_Percent;
		protected int _reward_Count;
		protected int _reward_Describe;
		protected int _condition_Type;
		protected int _condition_Item;
		protected int _condition_Count;
		protected int _condition_Describe;
	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				GirlSkillObject girlskillObject = GirlSkillObject.Load(childrenElement);
				
				if (!_girlSkillObjectDict.ContainsKey(girlskillObject.SkillID))
					_girlSkillObjectDict[girlskillObject.SkillID] = girlskillObject;
			}
		}

		return true;
	}
	public GirlSkillObject GetSkillObjectByID(int _logicID)
	{
		if (!_girlSkillObjectDict.ContainsKey(_logicID))
			return null;
		
		return _girlSkillObjectDict[_logicID];
	}
//	public List<int> GetGirlSkillListByGirlID(int SkillID)
//	{
//		List<int> Temp = new List<int>();
//		foreach(GirlSkillObject obj in _girlSkillObjectDict.Values)
//		{
//			if(obj.SkillID == SkillID)
//			{
//				Temp.Add(obj.SkillID);
//			}
//		}
//		return Temp;
//	}
//	
	public List<GirlSkillObject> GetPlayerSkillMessageList(int SkillType)
	{
		List<GirlSkillObject> Templist = new List<GirlSkillObject>();
		foreach(GirlSkillObject obj in _girlSkillObjectDict.Values)
		{
			if(obj.Skill_Type != 6 && obj.Skill_Type == SkillType)
			{
				Templist.Add(obj);
			}
		}
		return Templist;
	}
	protected Dictionary<int, GirlSkillObject> _girlSkillObjectDict = new Dictionary<int, GirlSkillObject>();
}
