using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EMotionTrackType
{
	TYPE_NONE = 0,
	
	TYPE_MISSILE = 1,
	TYPE_TORPEDO = 2,
	TYPE_ARI_PLANE_GROUP = 3,
	
	TYPE_INVALID
}

public enum EHitParticleType
{
	HIT_PARTICLE_PRIMARY,
	HIT_PARTICLE_ASSISTANT,
	HIT_PARTICLE_WATER_SURFACE,
}

public class SkillEffectData
{
	public enum EParticleType
	{
		PARTICLE_FIRE,
		PARTICLE_MOTION,
		PARTICLE_HIT,
	}
		
	public class SkillAnimationData
	{
		public string _animationName;
//		bool _isLoop;
//		
//		float _delayTime;
//		float _durationTime;
	}
	
	public class SkillFireParticleData
	{
		public string _particleName;
		
		public float _delayTime;
		public float _durationTime;
		
		//! tzz added for shake camera 
		public bool _shakeCamera;
//		
//		bool _isAttachToBone;
//		
//		string _boneName;
//		Vector3 _offsetPosition;
//		Quaternion _offsetOrientation;
		
		public Vector3 _position;
		public Quaternion _rotation;
		
		public SkillSoundData _skillSound = null;
	}
	
	public class SkillFlyParticleData
	{
		public string _particleName;
		public float _delayTime;
		
		public float speed;
//		bool _isAutoTrack;
//		
//		Vector3 _startPosition;
//		Vector3 _endPosition;
//		
//		string _startBoneName;
//		Vector3 _startOffsetPosition;
//		Quaternion _startOffsetOrientation;
//		string _endBoneName;
//		Vector3 _endOffsetPosition;
//		Quaternion _endOffsetOrientation;
		
		public Vector3 _position;
		public Vector3 _rotation;
		
		public SkillSoundData _skillSound = null;
	}
	
	public class SkillHitParticleData
	{
		public string _particleName;
		public string _primaryParticleName;
		public string _assistantParticleName;
		public string _waterSurfaceParticleName;
		
		public float _delayTime;
		public float _durationTime;
//		
//		bool _isAttachToBone;
//		
//		string _boneName;
//		Vector3 _offsetPosition;
//		Quaternion _offsetOrientation;
		
//		public Vector3 _attachPosition;
		
		public Vector3 _position;
		public Vector3 _rotation;
		
		public SkillSoundData _skillSound = null;
	}
	
	public class SkillSoundData
	{
		public string _soundName;
		public bool _isLoop;
		public float _volume;
	}
	
	public SkillAnimationData _skillAnimation;
	
	public List<SkillFireParticleData> _skillFireParticleList 
		= new List<SkillFireParticleData>();
	
	public List<SkillFlyParticleData> _skillFlyParticleList 
		= new List<SkillFlyParticleData>();
	
	public List<SkillHitParticleData> _skillHitParticleList 
		= new List<SkillHitParticleData>();
}

/*
public class SkillBaseData
{
	public int _mSkillID;
	public int _skillType;
}
*/

public class SkillDamageRange
{
	// [[-1,-1],[1,-1],[-1,1],[1,1]]
	public class Rect
	{
		public int x;
		public int z;
		
		public float left;
		public float top;
		public float right;
		public float bottom;
	}
	
	public List<Rect> _damageRectList = new List<Rect>();
}

public class SkillDataSlot
{
	public SkillData MSkillData = new SkillData();
	
	public int _skillID;
	// The skill user id
	public long _attackerID;
	// The skill user attack state?
	public int _attckerAttackState;
	
	// The skill target struct
	public class AttackTargetData
	{
		public long _targetID;
		public int _moveState;
		public int _beAttackedState;
		public int _beAttackedDamage;
		
		public bool _isPrimaryTarget;
		// public Vector3 _targetPosition;
	}
	public long _primaryTargetID;
	public Dictionary<long, AttackTargetData> _attackTargetDataList = new Dictionary<long, AttackTargetData>();
	
	public SkillDamageRange _skillDamageRange = new SkillDamageRange();
	public SkillEffectData _skillEffectData = new SkillEffectData();
	
	/// <summary>
	/// tzz added 
	/// Initializes a new instance of the <see cref="SkillDataSlot"/> class.
	/// </summary>
	/// <param name='logicID'>
	/// Logic I.
	/// </param>
	/// <param name='attackerID'>
	/// Attacker I.
	/// </param>
	/// <param name='attackerAttackState'>
	/// Attacker attack state.
	/// </param>
	public SkillDataSlot(int logicID,long attackerID,int attackerAttackState){
		_skillID 			= logicID;
		_attackerID 		= attackerID;
		_attckerAttackState = attackerAttackState;
		
		SkillLogicId = logicID;
	}
	
	/**
	 * tzz added
	 * 
	 * is cure skill 
	 */ 
	public bool IsCureSkill(){
		return _attckerAttackState == (int)GameData.BattleGameData.AttackedState.TREAT_SKILL_ATTACKED;
	}
	
	/**
	 * tzz added for return is this skill is reduce damage to itself
	 */ 
	public bool IsReduceDamangeSkill(){ 
		return _attckerAttackState == (int)GameData.BattleGameData.AttackedState.REDUCE_DAMAGE_SKILL_ATTACKED;
	}
	
	public bool IsNivoseType(){
		return MSkillData.BasicData.SkillType == ESkillType.GROUP_NIVOSE
				|| MSkillData.BasicData.SkillType == ESkillType.SINGLE_NIVOSE;
	}
	
	public int SkillLogicId
	{
		get { return _skillID; }
		set
		{
			_skillID = value;
			MSkillData.BasicData.SkillLogicID = value;
			PrepareData();
		}
	}
	
	void PrepareData()
	{
		if (0 == MSkillData.BasicData.SkillLogicID
			|| -1 == MSkillData.BasicData.SkillLogicID)
			return;
		
		if (null != MSkillData)
			MSkillData.BasicData.FillDataFromConfig();
		
		CalculateDamageLegendRange();
		FillInSkillEffectData();
	}
	
	void CalculateDamageLegendRange()
	{
		// Parse skill damage legend range
		_skillDamageRange = HelpUtil.ParseSkillRange(MSkillData.BasicData.SkillDamageRange);
	}
	
	void FillInSkillEffectData()
	{
		/*Use new SkillEffectConfig.xml and SkillEffectConfig Class
		 * */
		SkillEffectConfig skillEffectConfig = Globals.Instance.MDataTableManager.GetConfig<SkillEffectConfig> ();
		
		/// Parse skill effect information
		SkillEffectConfig.SkillEffectElement skillEffectElement = null;
		
		// Transfer Animation data
		SkillEffectData.SkillAnimationData animationData = new SkillEffectData.SkillAnimationData();
		// Fixed value!!!
		animationData._animationName = AnimationDefine.ANIMATION_ATTACK; 
		_skillEffectData._skillAnimation = animationData;
		
		//-------------------------------------------------------------------------------
		// Get the Fire particle information
		int effectID = MSkillData.BasicData.SkillReleaseEffectID;
		if (!skillEffectConfig.GetSkillEffectElement(effectID, out skillEffectElement))
		{
			return;
		}
		// Transfer Fire particle data
		SkillEffectData.SkillFireParticleData fireParticleData = new SkillEffectData.SkillFireParticleData();
		fireParticleData._particleName = skillEffectElement._skillParticleElement._particleAssetName;
		fireParticleData._delayTime = skillEffectElement._skillParticleElement._particleDelayTime;
		fireParticleData._durationTime = skillEffectElement._skillParticleElement._particleDurationTime;
		fireParticleData._shakeCamera	= skillEffectElement._skillParticleElement._shakeCamera;
		
		// Fire sound information
		if (null != skillEffectElement._skillSoundElement)
		{
			fireParticleData._skillSound = new SkillEffectData.SkillSoundData();
			fireParticleData._skillSound._soundName = skillEffectElement._skillSoundElement._soundAssetName;
			fireParticleData._skillSound._isLoop = skillEffectElement._skillSoundElement._soundIsLoop;
			fireParticleData._skillSound._volume = skillEffectElement._skillSoundElement._soundVolume;
			
			if (null == fireParticleData._skillSound._soundName)
			{
				Debug.Log("[Skill:] The skill fire sound is null.");
			}
		}
		
		_skillEffectData._skillFireParticleList.Add(fireParticleData);
		//-------------------------------------------------------------------------------
		
		
		//-------------------------------------------------------------------------------
		// Get the Fly particle information
		effectID = MSkillData.BasicData.SkillFlyEffectID;
		if (!skillEffectConfig.GetSkillEffectElement(effectID, out skillEffectElement))
		{
			return;
		}
		
		// Transfer Fly particle data
		SkillEffectData.SkillFlyParticleData flyParticleData = new SkillEffectData.SkillFlyParticleData();
		
		flyParticleData._particleName = skillEffectElement._skillParticleElement._particleAssetName;
		flyParticleData._delayTime = skillEffectElement._skillParticleElement._particleDelayTime;
		flyParticleData.speed = MSkillData.BasicData.EffectFlySpeed;
		
		// Fly sound information
		if (null != skillEffectElement._skillSoundElement)
		{
			flyParticleData._skillSound = new SkillEffectData.SkillSoundData();
			flyParticleData._skillSound._soundName = skillEffectElement._skillSoundElement._soundAssetName;
			flyParticleData._skillSound._isLoop = skillEffectElement._skillSoundElement._soundIsLoop;
			flyParticleData._skillSound._volume = skillEffectElement._skillSoundElement._soundVolume;
			
			if (null == flyParticleData._skillSound._soundName)
			{
				Debug.LogWarning("[Skill:] The skill fly sound is null.");
			}
		}
	
		_skillEffectData._skillFlyParticleList.Add(flyParticleData);
		//-------------------------------------------------------------------------------
		
		//-------------------------------------------------------------------------------
		// Get the Hit particle information
		effectID = MSkillData.BasicData.SkillHitEffectID;
		if (!skillEffectConfig.GetSkillEffectElement(effectID, out skillEffectElement))
		{
			return;
		}
		// Transfer Hit particle data
		SkillEffectData.SkillHitParticleData hitParticleData = new SkillEffectData.SkillHitParticleData();
		for (int i = 0; i < skillEffectElement._skillParticleElementList.Count; ++i)
		{
			if (i == (int)EHitParticleType.HIT_PARTICLE_PRIMARY)
			{
				hitParticleData._particleName = skillEffectElement._skillParticleElementList[i]._particleAssetName;
				hitParticleData._primaryParticleName = skillEffectElement._skillParticleElementList[i]._particleAssetName;
				
				hitParticleData._delayTime = skillEffectElement._skillParticleElementList[i]._particleDelayTime;
				hitParticleData._durationTime = skillEffectElement._skillParticleElementList[i]._particleDurationTime;
			}
			else if (i == (int)EHitParticleType.HIT_PARTICLE_ASSISTANT)
			{
				hitParticleData._assistantParticleName = skillEffectElement._skillParticleElementList[i]._particleAssetName;
			}
			else if (i == (int)EHitParticleType.HIT_PARTICLE_WATER_SURFACE)
			{
				hitParticleData._waterSurfaceParticleName = skillEffectElement._skillParticleElementList[i]._particleAssetName;
			}
		}
		
		// Hit sound information
		if (null != skillEffectElement._skillSoundElement)
		{
			hitParticleData._skillSound = new SkillEffectData.SkillSoundData();
			hitParticleData._skillSound._soundName = skillEffectElement._skillSoundElement._soundAssetName;
			hitParticleData._skillSound._isLoop = skillEffectElement._skillSoundElement._soundIsLoop;
			hitParticleData._skillSound._volume = skillEffectElement._skillSoundElement._soundVolume;
			
			if (null == hitParticleData._skillSound._soundName)
			{
				Debug.LogWarning("[Skill:] The skill hit sound is null.");
			}
		}
		_skillEffectData._skillHitParticleList.Add(hitParticleData);
		//-------------------------------------------------------------------------------
	}
}
