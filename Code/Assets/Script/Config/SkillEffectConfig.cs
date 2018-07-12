using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

/// <summary>
/// Skill effect config.
/// LiHaojie
/// 2012.04.10
/// </summary>
public class SkillEffectConfig : ConfigBase 
{
	public class SkillEffectElement
	{
		public int _skillEffectID;
		public string _skillEffectName;
		
		public SkillParticleElement _skillParticleElement;
		public SkillSoundElement _skillSoundElement;
		
		public List<SkillParticleElement> _skillParticleElementList = new List<SkillParticleElement>();
		
		/*
		public SkillAnimationElement _skillAnimationElement;
		public SkillFireParticleElement _skillFireParticleElement;
		public SkillFlyParticleElement _skillFlyParticleElement;
		public SkillHitParticleElement _skillHitParticleElement;
		*/
	}
	
	public class SkillParticleElement
	{
		public string _particleAssetName;
		
		public float _particleDelayTime;
		public float _particleDurationTime;
		
		// tzz added for shaking camera configure
		public bool _shakeCamera = false;
	}
	
	public class SkillSoundElement
	{
		public string _soundAssetName;
		public bool _soundIsLoop;
		public float _soundVolume;
	}
	
	/*
	public class SkillAnimationElement
	{
		public string _animationName;
	}
	
	public class SkillFireParticleElement
	{
		public string _particleAssetName;
		public float _particleDelayTime;
		public float _particleDurationTime;
		
		public SkillSoundElement _soundElement;
	}
	
	public class SkillFlyParticleElement
	{
		public string _particleAssetName;
		public float _particleDelayTime;
		// public float _particleDurationTime;
		
		public SkillSoundElement _soundElement;
	}
	
	public class SkillHitParticleElement
	{
		public string _particleAssetName;
		// public float _particleDelayTime;
		// public float _particleDurationTime;
		
		public SkillSoundElement _soundElement;
	}
	*/
	
	/// <summary>
	/// Load the specified element.
	/// </summary>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "SkillEffects")
			return false;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "SkillEffect")
				{
					SkillEffectElement skillEffectElement;
					if (!LoadSkillElement(childrenElement, out skillEffectElement))
						continue;
					
					_skillEffectElementList[skillEffectElement._skillEffectID] = skillEffectElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	public bool LoadSkillElement(SecurityElement element, out SkillEffectElement skillEffectElement)
	{
		skillEffectElement = new SkillEffectElement();
		if (element.Tag != "SkillEffect")
			return false;
		
		skillEffectElement._skillEffectID = StrParser.ParseDecInt(element.Attribute("EffectID"), -1);
		skillEffectElement._skillEffectName = StrParser.ParseStr(element.Attribute("Name"), "");
		
		if (element.Children == null)
			return false;
		
		foreach(SecurityElement childrenElement in element.Children)
		{
			if (childrenElement.Tag == "Particle")
			{
				if (!LoadSkillParticleElement(childrenElement, out skillEffectElement._skillParticleElement))
					continue;
				
				skillEffectElement._skillParticleElementList.Add(skillEffectElement._skillParticleElement);
			}
			else if (childrenElement.Tag == "Sound")
			{
				if (!LoadSkillSoundElement(childrenElement, out skillEffectElement._skillSoundElement))
					continue;	
			}
		}
		
		return true;
	}
	
	public bool LoadSkillParticleElement(SecurityElement element, out SkillParticleElement particleElement)
	{
		particleElement = new SkillParticleElement();
		if (element.Tag != "Particle")
			return false;
		
		string attribute = element.Attribute("Asset");
		if (attribute != null)
			particleElement._particleAssetName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("DelayTime");
		if (attribute != null)
			particleElement._particleDelayTime = StrParser.ParseFloat(attribute, 0.0f);
		
		attribute = element.Attribute("DurationTime");
		if (attribute != null)
			particleElement._particleDurationTime = StrParser.ParseFloat(attribute, 0.0f);
		
		attribute = element.Attribute("ShakeCamera");
		if(attribute != null){
			particleElement._shakeCamera	= bool.Parse(attribute);
		}
		
		return true;	
	}
	
	/// <summary>
	/// Loads the skill sound element.
	/// </summary>
	/// <returns>
	/// The skill sound element.
	/// </returns>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	/// <param name='soundElement'>
	/// If set to <c>true</c> sound element.
	/// </param>
	public bool LoadSkillSoundElement(SecurityElement element, out SkillSoundElement soundElement)
	{
		soundElement = new SkillSoundElement();
		if (element.Tag != "Sound")
			return false;
		
		string attribute = element.Attribute("Asset");
		if (attribute != null)
			soundElement._soundAssetName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Loop");
		if (attribute != null)
			soundElement._soundIsLoop = StrParser.ParseBool(attribute, false);
		
		attribute = element.Attribute("Volume");
		if (attribute != null)
			soundElement._soundVolume = StrParser.ParseFloat(attribute, 100.0f);
		
		return true;
	}
	
	// Get category
	public bool GetSkillEffectElement(int skillEffectID, out SkillEffectElement skillEffectElement)
	{
		skillEffectElement = null;
		
		if (!_skillEffectElementList.ContainsKey(skillEffectID))
			return false;
		
		skillEffectElement = _skillEffectElementList[skillEffectID];
		
		return true;
	}
	
	public bool GetSkillEffectElement(string skillEffectName, out SkillEffectElement skillEffectElement)
	{
		skillEffectElement = null;
		
		foreach (SkillEffectElement val in _skillEffectElementList.Values)
		{
			if (val._skillEffectName == skillEffectName)
			{
				skillEffectElement = val;
				
				return true;
			}
		}
		
		return false;
	}
	
	/// <summary>
	/// The _skill effect element list.
	/// </summary>
	protected Dictionary<int, SkillEffectElement> _skillEffectElementList = new Dictionary<int, SkillEffectElement>();
	
	/*
	public bool LoadSkillElement(SecurityElement element, out SkillEffectElement skillElement)
	{
		skillElement = new SkillEffectElement();
		if (element.Tag != "SkillEffect")
			return false;
		
		skillElement._mSkillID = StrParser.ParseDecInt(element.Attribute("ID"), -1);
		skillElement._skillEffectName = StrParser.ParseStr(element.Attribute("Name"), "");
		
		if (element.Children == null)
			return false;
		
		foreach(SecurityElement childrenElement in element.Children)
		{
			if (childrenElement.Tag == "Animation")
			{
				// skillElement._skillAnimationElement = new SkillAnimationElement();
				if (!LoadSkillAnimationElement(childrenElement, out skillElement._skillAnimationElement))
					continue;
			}
			else if (childrenElement.Tag == "FireEffect")
			{
				// skillElement._skillFireParticleElement = new SkillFireParticleElement();
				if (!LoadSkillFireParticleElement(childrenElement, out skillElement._skillFireParticleElement))
					continue;	
			}
			else if (childrenElement.Tag == "FlyEffect")
			{
				// skillElement._skillFlyParticleElement = new SkillFlyParticleElement();
				if (!LoadSkillFlyParticleElement(childrenElement, out skillElement._skillFlyParticleElement))
					continue;
			}
			else if (childrenElement.Tag == "HitEffect")
			{
				// skillElement._skillHitParticleElement = new SkillHitParticleElement();
				if (!LoadSkillHitParticleElement(childrenElement, out skillElement._skillHitParticleElement))
					continue;
			}
				
		}
		
		return true;
	}
	
	/// <summary>
	/// Loads the skill animation element.
	/// </summary>
	/// <returns>
	/// The skill animation element.
	/// </returns>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	/// <param name='skillAnimationElement'>
	/// If set to <c>true</c> skill animation element.
	/// </param>
	public bool LoadSkillAnimationElement(SecurityElement element, out SkillAnimationElement skillAnimationElement)
	{
		skillAnimationElement = new SkillAnimationElement();
		if (element.Tag != "Animation")
			return false;
		
		skillAnimationElement._animationName = StrParser.ParseStr(element.Attribute("Name"), "");
		
		return true;
	}
	
	/// <summary>
	/// Loads the skill fire particle element.
	/// </summary>
	/// <returns>
	/// The skill fire particle element.
	/// </returns>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	/// <param name='skillFireParticleElement'>
	/// If set to <c>true</c> skill fire particle element.
	/// </param>
	public bool LoadSkillFireParticleElement(SecurityElement element, out SkillFireParticleElement skillFireParticleElement)
	{
		skillFireParticleElement = new SkillFireParticleElement();
		if (element.Tag != "FireEffect")
			return false;
		
		if (element.Children == null)
			return false;
		
		foreach(SecurityElement childrenElement in element.Children)
		{
			if (childrenElement.Tag == "Particle")
			{
				skillFireParticleElement._particleAssetName = StrParser.ParseStr(childrenElement.Attribute("Asset"), "");
				skillFireParticleElement._particleDelayTime = StrParser.ParseFloat(childrenElement.Attribute("DelayTime"), 0.0f);
				skillFireParticleElement._particleDurationTime = StrParser.ParseFloat(childrenElement.Attribute("DurationTime"), 0.0f);
			}
			else if (childrenElement.Tag == "Sound")
			{
				// skillFireParticleElement._soundElement = new SkillSoundElement();
				if (!LoadSkillSoundElement(childrenElement, out skillFireParticleElement._soundElement))
					continue;
			}
		}
		
		
		return true;
	}
	
	/// <summary>
	/// Loads the skill fly particle element.
	/// </summary>
	/// <returns>
	/// The skill fly particle element.
	/// </returns>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	/// <param name='skillFlyParticleElement'>
	/// If set to <c>true</c> skill fly particle element.
	/// </param>
	public bool LoadSkillFlyParticleElement(SecurityElement element, out SkillFlyParticleElement skillFlyParticleElement)
	{
		skillFlyParticleElement = new SkillFlyParticleElement();
		if (element.Tag != "FlyEffect")
			return false;
		
		if (element.Children == null)
			return false;
		
		foreach(SecurityElement childrenElement in element.Children)
		{
			if (childrenElement.Tag == "Particle")
			{
				skillFlyParticleElement._particleAssetName = StrParser.ParseStr(childrenElement.Attribute("Asset"), "");
				skillFlyParticleElement._particleDelayTime = StrParser.ParseFloat(childrenElement.Attribute("DelayTime"), 0.0f);
				// skillFlyParticleElement._particleDurationTime = StrParser.ParseFloat(childrenElement.Attribute("DurationTime"), 0.0f);
			}
			else if (childrenElement.Tag == "Sound")
			{
				// skillFlyParticleElement._soundElement = new SkillSoundElement();
				if (!LoadSkillSoundElement(childrenElement, out skillFlyParticleElement._soundElement))
					continue;
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// Loads the skill hit particle element.
	/// </summary>
	/// <returns>
	/// The skill hit particle element.
	/// </returns>
	/// <param name='element'>
	/// If set to <c>true</c> element.
	/// </param>
	/// <param name='skillHitParticleElement'>
	/// If set to <c>true</c> skill hit particle element.
	/// </param>
	public bool LoadSkillHitParticleElement(SecurityElement element, out SkillHitParticleElement skillHitParticleElement)
	{
		skillHitParticleElement = new SkillHitParticleElement();
		if (element.Tag != "HitEffect")
			return false;
		
		if (element.Children == null)
			return false;
		
		foreach(SecurityElement childrenElement in element.Children)
		{
			if (childrenElement.Tag == "Particle")
			{
				skillHitParticleElement._particleAssetName = StrParser.ParseStr(childrenElement.Attribute("Asset"), "");
				// skillHitParticleElement._particleDelayTime = StrParser.ParseFloat(childrenElement.Attribute("DelayTime"), 0.0f);
				// skillHitParticleElement._particleDurationTime = StrParser.ParseFloat(childrenElement.Attribute("DurationTime"), 0.0f);
			}
			else if (childrenElement.Tag == "Sound")
			{
				// skillHitParticleElement._soundElement = new SkillSoundElement();
				if (!LoadSkillSoundElement(childrenElement, out skillHitParticleElement._soundElement))
					continue;
			}
		}
		
		return true;
	}
	*/
}
