using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
	void Awake()
	{
		_skillList = new List<Skill>();
	}
	
	void Start()
	{
	}
	
	void OnDestroy()
	{
		Cleanup();
	}
	
	void OnDisable()
	{
		Cleanup();
	}
	
	void Update()
	{
		List<Skill> tempReomoveList = new List<Skill>();
		foreach (Skill skill in _skillList)
		{
			skill.Update();
			
			if (skill._mSkillIsEnd)
			{
				tempReomoveList.Add(skill);
			}
		}
		
		foreach (Skill skill in tempReomoveList)
		{
			RemoveSkill(skill);
		}
		tempReomoveList.Clear();
	}
	
	public void Cleanup()
	{
		foreach (Skill skill in _skillList)
		{
			skill.Release();
		}
		_skillList.Clear();
	}
	
	public Skill CreateSkill(SkillDataSlot skillData,WarshipL skillUser)
	{
		Skill skill = null;
		
		switch (skillData.MSkillData.BasicData.SkillType)
		{
		case ESkillType.SINGLE_NIVOSE:
		case ESkillType.GROUP_NIVOSE:
		case ESkillType.GROUP_REPAIR_NIVOSE:
		{
			skill = new NiVose(skillData,skillUser);
			break;
		}
		default:
		{
			skill = new Skill(skillData);
			break;
		}
		}
		skill.Initialize();
		
		_skillList.Add(skill);
		return skill;
	}
	
	public void RemoveSkill(Skill skill)
	{
		skill.Release();
		_skillList.Remove(skill);
	}
	
	public List<Skill> _skillList;
}