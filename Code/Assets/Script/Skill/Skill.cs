using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SkillState
{
	PREPARE,
	
	FIRE,
	FLY,
	HIT_TARGET,
	
	END,
}

public class Skill
{
	public Skill()
	{
		_mSkillID = -1;
	}
	
	public Skill(SkillDataSlot slotData)
	{
		_mSkillID = slotData._skillID;
		_mSkillDataSlot = slotData;
	}
	
	public virtual void Initialize()
	{
		_mMissileEffectList = new List<MissileL>();
		_mRemoveMissileList = new List<MissileL>();
		_mFlyEffectObjs = new List<GameObject>();
		
		_mSkillIsEnd = false;
		_mIsTouchTarget = false;
		
		_mCurrentTrackTime = 0;
		_mIsFireParticleCreated = false;
		_mIsFlyParticleCreated = false;
		_isHitParticleCreated = false;
	}
	
	public virtual void Release()
	{
		foreach (MissileL missile in _mMissileEffectList)
		{
			missile.Release();
		}
		_mMissileEffectList.Clear();
		_mRemoveMissileList.Clear();
		
		foreach (GameObject go in _mFlyEffectObjs)
		{
			GameObject.Destroy(go);
		}
		_mFlyEffectObjs.Clear();
	}
	
	public void SetProperty(SkillDataSlot skillData)
	{
		_mSkillDataSlot = skillData;
	}
	
	public void StartSkill()
	{
		// Parse skill damage legend range
		CalculateDamageLegendRange();
		FillInSkillEffectData();
		
		// Get the skill user
		_mSkillUser = Globals.Instance.MPlayerManager.GetWarship(_mSkillDataSlot._attackerID);
		_mSkillUser.OnAttackStart(this);
		
		_mSkillState = SkillState.PREPARE;
	}
	
	public void CalculateDamageLegendRange()
	{
		// Parse skill damage legend range
		_mSkillDataSlot._skillDamageRange = HelpUtil.ParseSkillRange(_mSkillDataSlot.MSkillData.BasicData.SkillDamageRange);
	}
	
	/**
	 * tzz added for Is nivose skill type
	 */ 
	public bool IsNivoseType(){
		return _mSkillDataSlot.MSkillData.BasicData.SkillType == ESkillType.GROUP_NIVOSE
				|| _mSkillDataSlot.MSkillData.BasicData.SkillType == ESkillType.SINGLE_NIVOSE;
	}
	
	public void FillInSkillEffectData()
	{
		/*Use new SkillEffectConfig.xml and SkillEffectConfig Class
		 * */
		SkillEffectConfig skillEffectConfig = Globals.Instance.MDataTableManager.GetConfig<SkillEffectConfig> ();
		
		/// Parse skill effect information
		SkillEffectConfig.SkillEffectElement skillEffectElement;
		
		// Transfer Animation data
		SkillEffectData.SkillAnimationData animationData = new SkillEffectData.SkillAnimationData();
		// Fixed value!!!
		animationData._animationName = AnimationDefine.ANIMATION_ATTACK; 
		_mSkillDataSlot._skillEffectData._skillAnimation = animationData;
		
		//-------------------------------------------------------------------------------
		// Get the Fire particle information
		int effectID = _mSkillDataSlot.MSkillData.BasicData.SkillReleaseEffectID;
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
		
		_mSkillDataSlot._skillEffectData._skillFireParticleList.Add(fireParticleData);
		//-------------------------------------------------------------------------------
		
		
		//-------------------------------------------------------------------------------
		// Get the Fly particle information
		effectID = _mSkillDataSlot.MSkillData.BasicData.SkillFlyEffectID;
		if (!skillEffectConfig.GetSkillEffectElement(effectID, out skillEffectElement))
		{
			return;
		}
		
		// Transfer Fly particle data
		SkillEffectData.SkillFlyParticleData flyParticleData = new SkillEffectData.SkillFlyParticleData();
		
		flyParticleData._particleName = skillEffectElement._skillParticleElement._particleAssetName;
		flyParticleData._delayTime = skillEffectElement._skillParticleElement._particleDelayTime;
		flyParticleData.speed = _mSkillDataSlot.MSkillData.BasicData.EffectFlySpeed;
		
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
	
		_mSkillDataSlot._skillEffectData._skillFlyParticleList.Add(flyParticleData);
		//-------------------------------------------------------------------------------
		
		//-------------------------------------------------------------------------------
		// Get the Hit particle information
		effectID = _mSkillDataSlot.MSkillData.BasicData.SkillHitEffectID;
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
		_mSkillDataSlot._skillEffectData._skillHitParticleList.Add(hitParticleData);
		//-------------------------------------------------------------------------------
	}
	
	public virtual void CreateFireParticle()
	{
		bool isCreate = false;
		
		// Create the delay property particle
		foreach (SkillEffectData.SkillFireParticleData fireParticleData in _mSkillDataSlot._skillEffectData._skillFireParticleList)
		{
			if (_mCurrentTrackTime >= fireParticleData._delayTime)
			{
				// Play fire sound
				if (null != fireParticleData._skillSound)
					Globals.Instance.MSoundManager.PlaySoundEffect(fireParticleData._skillSound._soundName);
				
				// Create the fire particle
				GameObject go = null;
				string effectName = fireParticleData._particleName;
				
				// Temporary: Resolve the particle overturn
				if (_mSkillDataSlot.MSkillData.BasicData.SkillIsNormalAttack)
				{
					if (_mSkillUser.Property.WarshipIsAttacker)
					{
						effectName = fireParticleData._particleName + "_L";
					}
					else
					{
						effectName = fireParticleData._particleName + "_R";
					}
				}
				
				UnityEngine.Object obj = Resources.Load(effectName);
				if (null == obj)
				{
					isCreate = true;
					Debug.Log("[Skill]: Cann't find the effect name " + effectName);
					break;
				}
				
				go = GameObject.Instantiate(obj, fireParticleData._position, 
					fireParticleData._rotation) as GameObject;
								
				GameObject tagPoint = _mSkillUser.GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_CANNON);
				if (null == tagPoint 
				// tzz added for cure skill
				//
				|| _mSkillDataSlot.IsCureSkill()
				|| _mSkillDataSlot.IsReduceDamangeSkill()){
					go.transform.position = _mSkillUser.U3DGameObject.transform.position;
				}
				else
				{
					go.transform.parent = tagPoint.transform;
					go.transform.localPosition = Vector3.zero;
				}
				
				GameObject.DestroyObject(go, fireParticleData._durationTime);
				isCreate = true;
				
				_mSkillState = SkillState.FIRE;
			}
		}
		
		_mIsFireParticleCreated = isCreate;
	}
	
	public virtual void CreateFlyParticle()
	{
		bool isCreate = false;
		
		foreach (SkillEffectData.SkillFlyParticleData flyParticleData in _mSkillDataSlot._skillEffectData._skillFlyParticleList)
		{
			if (_mCurrentTrackTime >= flyParticleData._delayTime)
			{
				// tzz added for null
				if(_mSkillUser.U3DGameObject  == null){
					continue;
				}
				
				// Play fly sound
				if (null != flyParticleData._skillSound)
				{
					Debug.Log("_mSkillUser " + _mSkillUser.U3DGameObject.name + "Sound " + flyParticleData._skillSound._soundName);
					Globals.Instance.MSoundManager.PlaySoundEffect(flyParticleData._skillSound._soundName);
				}
				
				Vector3 startPosition = _mSkillUser.U3DGameObject.transform.position;
				
				// Create multiple missile effects
				foreach (SkillDataSlot.AttackTargetData attackTargetData in _mSkillDataSlot._attackTargetDataList.Values)
				{
					UnityEngine.Object obj = Resources.Load(flyParticleData._particleName);
					if (null == obj)
					{
						isCreate = true;
						Debug.Log("[Skill]: Cann't find the fly effect name " + flyParticleData._particleName);
						continue;
					}
					
					// Calculate the Projectile ParticleEffect information
					GameObject go = GameObject.Instantiate(obj,Vector3.zero, Quaternion.identity) as GameObject;
					
					WarshipL target = Globals.Instance.MPlayerManager.GetWarship(attackTargetData._targetID);
					Vector3 endPosition = target.U3DGameObject.transform.position;
					
					float missileHorzSpeed = flyParticleData.speed;
					float targetHorzSpeed = target.MoveSpeed;
					if (attackTargetData._moveState == (int)GameData.BattleGameData.MoveState.STOP)
						targetHorzSpeed = 0.0f;
					
					MotionTrack motionTrack = null;
					// Lihaojie 2012.09.26 Add a time track model, solve the problem which caused by the cure skill and damage skill order
					if (_mSkillDataSlot.IsCureSkill())
					{
						motionTrack = new TimeTrack(GameDefines.BATTLE_STEP_TIME - flyParticleData._delayTime - 0.05f);
					}
					else
					{
						switch ( (EWarshipType)_mSkillUser.Property.WarshipType )
						{
						case EWarshipType.CARRIER:
						{
							startPosition.y += 50.0f;
							endPosition.y += 50.0f;
							// missileHorzSpeed = 150;
							motionTrack = new AirCraftTrack(startPosition, endPosition, missileHorzSpeed, targetHorzSpeed);
							break;
						}
						case EWarshipType.SUBMARINE:
						{
							motionTrack = new  LineTrack(startPosition, endPosition, missileHorzSpeed, targetHorzSpeed);
							break;
						}
						case EWarshipType.SURFACE_SHIP:
						{
							// Assume the two speed is in x axis
							float gravityAcceleration = -100.0f;
							
							// MotionTrack motionTrack = new ParabolicTrack(startPosition, endPosition, horzSpeed);
							motionTrack = new ParabolicTrack(startPosition, endPosition, missileHorzSpeed, targetHorzSpeed, gravityAcceleration);
							break;
						}
						}
					}
					
					MissileL missile = new MissileL(go, motionTrack);
					_mMissileEffectList.Add(missile);
					_mFlyEffectObjs.Add(go);
				
					isCreate = true;
					_mSkillState = SkillState.FLY;
				} // End foreach
			}
		}
		
		_mIsFlyParticleCreated = isCreate;
	}
	
	public virtual void CreateHitParticle()
	{
		bool isCreate = false;
		
		// Create the hit particles
		foreach (SkillEffectData.SkillHitParticleData hitParticleData in _mSkillDataSlot._skillEffectData._skillHitParticleList)
		{
			// Play fire sound
			if (null != hitParticleData._skillSound)
				Globals.Instance.MSoundManager.PlaySoundEffect(hitParticleData._skillSound._soundName);
			
			// Calculate hit particle type according to DamageRange
			List<SkillDamageRange.Rect> rectList = null;
			
			// Check is need to inverse skill damage range
			bool isSingleSkill = _mSkillDataSlot._skillDamageRange._damageRectList.Count == 1;
			if (isSingleSkill || _mSkillUser.Property.WarshipIsAttacker)
			{
				rectList = _mSkillDataSlot._skillDamageRange._damageRectList;
			}
			else
			{
				// Inverse legend, x Axis
				rectList = new List<SkillDamageRange.Rect>(); 
				foreach (SkillDamageRange.Rect rect in _mSkillDataSlot._skillDamageRange._damageRectList)
				{
					SkillDamageRange.Rect invRect = new SkillDamageRange.Rect();
					invRect.left = -rect.left;
					invRect.right = -rect.right;
					invRect.bottom = rect.bottom;
					invRect.top = rect.top;
					
					rectList.Add(invRect);
				}
			}
			
			// Calulate the Nivose damage range
			CalculateNivoseHitRange(rectList);
			
			// Optimize the warter effect count
			int waterBloomIndex = 0;
			int waterBloomLimit = 20;
			foreach (SkillDamageRange.Rect rect in rectList)
			{
				EHitParticleType hitType;
				Vector3 hitPos;
					
				CalculateHitParticleType(rect, out hitType, out hitPos);
				
				if (hitType == EHitParticleType.HIT_PARTICLE_WATER_SURFACE
					&& waterBloomIndex >= waterBloomLimit)
				{
					continue;
				}
				
				UnityEngine.Object obj = null;
				switch (hitType)
				{
				case EHitParticleType.HIT_PARTICLE_PRIMARY:
				{
					obj = Resources.Load(hitParticleData._primaryParticleName);
					break;
				}
				case EHitParticleType.HIT_PARTICLE_ASSISTANT:
				{
					obj = Resources.Load(hitParticleData._assistantParticleName);
					break;
				}
				case EHitParticleType.HIT_PARTICLE_WATER_SURFACE:
				{
					waterBloomIndex++;
					obj = Resources.Load(hitParticleData._waterSurfaceParticleName);
					break;
				}
				}
				
				if (null == obj)
				{
					isCreate = true;
					Debug.Log("[Skill]: Cann't find the hit effect name " + hitParticleData._primaryParticleName);
					break;
				}
				
				// Optimize the warter effect count
				if (hitType == EHitParticleType.HIT_PARTICLE_WATER_SURFACE)
				{
					if (waterBloomIndex % 2 == 0)
					{
						GameObject go = GameObject.Instantiate(obj, hitPos, Quaternion.identity) as GameObject;
						MonoBehaviour.DestroyObject(go, 3.0f);
					}
				}
				else
				{
					GameObject go = GameObject.Instantiate(obj, hitPos, Quaternion.identity) as GameObject;
					MonoBehaviour.DestroyObject(go, 3.0f);
				}
				
				
				isCreate = true;
			}
		}
		
		_isHitParticleCreated = isCreate;
	}
	
	// Design requirement 2013.01.15: client calulate the nivose skill damage range
	void CalculateNivoseHitRange(List<SkillDamageRange.Rect> rectList)
	{
		if (_mSkillDataSlot.MSkillData.BasicData.SkillType == ESkillType.GROUP_NIVOSE)
		{
			rectList.Clear();
			if (_mSkillUser.Property.WarshipIsAttacker)
			{
				// Its damage range is the be attacked position
				for (int i = 1; i <= 4; i++)
				{
					for (int j = -3; j <= 2; j++)
					{
						SkillDamageRange.Rect rect = new SkillDamageRange.Rect();
						rect.left = ((float)i - 0.5f) * GameDefines.BATTLE_GRID_WIDTH;
						rect.right = ((float)i + 0.5f) * GameDefines.BATTLE_GRID_WIDTH;
						rect.top = ((float)j + 0.5f) * GameDefines.BATTLE_GRID_HEIGHT;
						rect.bottom = ((float)j - 0.5f) * GameDefines.BATTLE_GRID_HEIGHT;
						
						rectList.Add(rect);
					}
				}
			}
			else
			{
				for (int i = -4; i < -1; i++)
				{
					for (int j = -3; j <= 2; j++)
					{
						SkillDamageRange.Rect rect = new SkillDamageRange.Rect();
						rect.left = ((float)i - 0.5f) * GameDefines.BATTLE_GRID_WIDTH;
						rect.right = ((float)i + 0.5f) * GameDefines.BATTLE_GRID_WIDTH;
						rect.top = ((float)j + 0.5f) * GameDefines.BATTLE_GRID_HEIGHT;
						rect.bottom = ((float)j - 0.5f) * GameDefines.BATTLE_GRID_HEIGHT;
						
						rectList.Add(rect);
					}
				}
			}
		}
	}
	
	protected void CalculateHitParticleType(SkillDamageRange.Rect rect, out EHitParticleType hitType, out Vector3 hitPos)
	{
		hitType = EHitParticleType.HIT_PARTICLE_PRIMARY;
		hitPos = Vector3.zero;
		
		// // Left-Bottom
		// Vector3 leftBottomPos = HelpUtil.GetGridPosition(rect.left, rect.bottom);
		
		// // Left-Top
		// Vector3 leftTopPos = HelpUtil.GetGridPosition(rect.left, rect.top);
		// 
		// // Right-Bottom
		// Vector3 rightBottomPos = HelpUtil.GetGridPosition(rect.right, rect.bottom);
		
		// // Right-Top
		// Vector3 rightTopPos = HelpUtil.GetGridPosition(rect.right, rect.top);
		
		Rect checkRect = new Rect();
		checkRect.xMin = rect.left;
		checkRect.yMin = rect.bottom;
		
		checkRect.xMax = rect.right;
		checkRect.yMax = rect.top;
		
		// Calculate the real damage rect
		if (_mSkillDataSlot.MSkillData.BasicData.SkillType != ESkillType.GROUP_NIVOSE)
		{
			WarshipL target = Globals.Instance.MPlayerManager.GetWarship(_mSkillDataSlot._primaryTargetID);
			Vector3 attachPosition = target.U3DGameObject.transform.position;
			
			checkRect.xMin += attachPosition.x;
			checkRect.yMin += attachPosition.z;
			
			checkRect.xMax += attachPosition.x;
			checkRect.yMax += attachPosition.z;
		}
		
		// Add the be attacked target effect on the target
		foreach (SkillDataSlot.AttackTargetData attackTargetData in _mSkillDataSlot._attackTargetDataList.Values)
		{
			WarshipL target = Globals.Instance.MPlayerManager.GetWarship(attackTargetData._targetID);
			Vector3 attachPosition = target.U3DGameObject.transform.position;

			bool isContains = checkRect.Contains(new Vector2(attachPosition.x, attachPosition.z));
			if (isContains && attackTargetData._isPrimaryTarget)
			{
				hitType = EHitParticleType.HIT_PARTICLE_PRIMARY;
				hitPos = attachPosition;
				return;
			}
			else if (isContains)
			{
				hitType = EHitParticleType.HIT_PARTICLE_ASSISTANT;
				hitPos = attachPosition;
				return;
			}
		} // End foreach
		
		// 
		hitType = EHitParticleType.HIT_PARTICLE_WATER_SURFACE;
		hitPos.x = checkRect.center.x;
		hitPos.y = 0.0f;
		hitPos.z = checkRect.center.y;
	}
	
	protected bool CheckSkill()
	{
		// Check what time the missile is arrive the target position
		foreach (MissileL missle in _mMissileEffectList)
		{
			if (missle.IsTouchTarget)
			{
				OnTouchTarget();
			}
			
			if (missle.IsEnd)
			{
				OnSkillEnd();
				_mRemoveMissileList.Add(missle);
			}
			else
			{
				missle.Update();
			}
		}
		
		foreach (MissileL missle in _mRemoveMissileList)
		{
			missle.Release();
			_mMissileEffectList.Remove(missle);
		}
		_mRemoveMissileList.Clear();
		
		return true;
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		if (_mSkillIsEnd)
			return;
		
		// Accumulate time flag.
		_mCurrentTrackTime += Time.deltaTime;
		
		if (!_mIsFireParticleCreated)
		{
			CreateFireParticle();
		}
		
		if (!_mIsFlyParticleCreated)
		{
			CreateFlyParticle();
		}
		
		if (!CheckSkill())
			return;
	}
	
	public void OnSkillTerminate()
	{
		
	}
	
	public void OnTouchTarget()
	{
		if (_mIsTouchTarget)
			return;
		
		_mIsTouchTarget = true;
		
		if (!_isHitParticleCreated)
		{
			CreateHitParticle();
		}
		
		// Notify destroy this skill? or change the skill state, and destroy by the skill user in next frame update?
		// All targets play be attacked effect
		foreach (SkillDataSlot.AttackTargetData attackTargetData in _mSkillDataSlot._attackTargetDataList.Values)
		{
			WarshipL target = Globals.Instance.MPlayerManager.GetWarship(attackTargetData._targetID);
			target.OnBeAttacked(this);
		}
		
		// Call the callback event method
		_mSkillUser.OnAttackEnd(this);
		
		_mSkillState = SkillState.HIT_TARGET;
	}
	
	public void OnSkillEnd()
	{
		_mSkillIsEnd = true;	
		_mSkillState = SkillState.END;
	}
	
	protected float _mSkillEndStartTime = 0.0f;
	protected float _mSkillEndDurationTime = 0.0f;
	
	protected int _mSkillID;
	protected SkillState _mSkillState;
	
	protected bool _mIsTouchTarget;
	public bool _mSkillIsEnd;
	
	protected WarshipL _mSkillUser;
	
	protected float _mCurrentTrackTime = 0.0f;
	protected bool _mIsFireParticleCreated = false;
	protected bool _mIsFlyParticleCreated = false;
	protected bool _isHitParticleCreated = false;
	
	public SkillDataSlot _mSkillDataSlot;
	
	// This Class cache contents
	protected List<MissileL> _mRemoveMissileList;
	protected List<MissileL> _mMissileEffectList;
	protected List<GameObject> _mFlyEffectObjs;
}
