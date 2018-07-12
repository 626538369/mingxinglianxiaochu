using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class SkillConfig : ConfigBase
{
	public class SkillObject
	{
		public int SkillLogicID
        {
            get { return _mSkillID; }
            set { _mSkillID = value; }
        }

        public int SkillType
        {
            get { return _skillType; }
            set { _skillType = value; }
        }

        public int SkillNameID
        {
            get { return _skillNameID; }
            set { _skillNameID = value; }
        }

        public int SkillDescID
        {
            get { return _skillDescID; }
            set { _skillDescID = value; }
        }

        public string SkillIconID
        {
            get { return _skillIconID; }
            set { _skillIconID = value; }
        }

        public int SkillLegendID
        {
            get { return _skillLegendID; }
            set { _skillLegendID = value; }
        }

        public string DamageRange
        {
            get { return _damageRange; }
            set { _damageRange = value; }
        }

        public int DamagePercent
        {
            get { return _damagePercent; }
            set { _damagePercent = value; }
        }

        public int SkillDamage
        {
            get { return _skillDamage; }
            set { _skillDamage = value; }
        }

        public int PowerUsed
        {
            get { return _powerUsed; }
            set { _powerUsed = value; }
        }

        public int SkillReleaseEffectID
        {
            get { return _skillReleaseEffectID; }
            set { _skillReleaseEffectID = value; }
        }

        public int SkillFlyEffectID
        {
            get { return _skillFlyEffectID; }
            set { _skillFlyEffectID = value; }
        }

        public int SkillHitEffectID
        {
            get { return _skillHitEffectID; }
            set { _skillHitEffectID = value; }
        }
		
		public float EffectFlySpeed
        {
            get { return _effectFlySpeed; }
            set { _effectFlySpeed = value; }
        }
		
		public int SkillWord
        {
            get { return mSkillWord; }
            set { mSkillWord = value; }
        }
		
		public static SkillObject Load(SecurityElement element)
		{
			SkillObject skillObject = new SkillObject();
			
			skillObject.SkillLogicID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_ID"),""),-1);
			skillObject.SkillType = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Type"),""),-1);
			skillObject.SkillNameID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Name"),""),-1);
			skillObject.SkillDescID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Desc"),""),-1);
			skillObject.SkillIconID = StrParser.ParseStr(element.Attribute("Skill_Icon"),"");
			skillObject.SkillLegendID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Legend"),""),-1);
			skillObject.DamageRange = StrParser.ParseStr(element.Attribute("Damage_Range"),"");
			skillObject.DamagePercent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Damage_Percent"),""),-1);
			skillObject.SkillDamage = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Damage"),""),-1);
			skillObject.PowerUsed = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Power_Used"),""),-1);
			skillObject.SkillReleaseEffectID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Release_Effect"),""),-1);
			skillObject.SkillFlyEffectID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Fly_Effect"),""),-1);
			skillObject.SkillHitEffectID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Hit_Effect"),""),-1);
			skillObject.mSkillWord		=  StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Skill_Word"),""),-1);
			skillObject.EffectFlySpeed = StrParser.ParseFloat(element.Attribute("Fly_Speed"),-1f);

			return skillObject;
		}

        private int _mSkillID;
        private int _skillType;
        private int _skillNameID;
        private int _skillDescID;
        private string _skillIconID;
        private int _skillLegendID;
        private string _damageRange;
        private int _damagePercent;
        private int _skillDamage;
        private int _powerUsed;
        private int _skillReleaseEffectID;
        private int _skillFlyEffectID;
        private int _skillHitEffectID;
		private float _effectFlySpeed;
		
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
				
				if (!_skillObjectDict.ContainsKey(skillObject.SkillLogicID))
					_skillObjectDict[skillObject.SkillLogicID] = skillObject;
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
	
	protected Dictionary<int,SkillObject> _skillObjectDict = new  Dictionary<int, SkillObject>();
}
