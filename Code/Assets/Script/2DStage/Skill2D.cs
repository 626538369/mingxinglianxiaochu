using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill2D : MonoBehaviour 
{
	GameObject targetObj = null;
	FightCellSlot skillUser = null;
	
	SkillDataSlot skillDataSlot = null;
	SkillState skillState = SkillState.PREPARE;
	
	float currTrackTime = 0.0f;
	bool isFireParticleCreated = false;
	bool isFlyParticleCreated = false;
	bool isHitParticleCreated = false;
	
	bool isTouchTarget;
	bool skillIsEnd;
	
	public static void StartSkill(GameObject target, SkillDataSlot skillData)
	{
		Skill2D skill = target.AddComponent<Skill2D>() as Skill2D;
		skill.Begin(target, skillData);
	}
	
	public void Begin(GameObject target, SkillDataSlot skillData)
	{
		targetObj = target;
		skillUser = target.GetComponent<FightCellSlot>() as FightCellSlot;
		
		skillDataSlot = skillData;
		skillState = SkillState.PREPARE;
	}
	
	public virtual void Update () 
	{
		if (skillIsEnd)
			return;
		
		currTrackTime += Time.deltaTime;
		
		if (!isFireParticleCreated)
		{
			CreateFireParticle();
		}
		
		if (!isFlyParticleCreated)
		{
			CreateFlyParticle();
		}
	}
	
	/**
	 * tzz added for Is nivose skill type
	 */ 
	public bool IsNivoseType(){
		return skillDataSlot.MSkillData.BasicData.SkillType == ESkillType.GROUP_NIVOSE
				|| skillDataSlot.MSkillData.BasicData.SkillType == ESkillType.SINGLE_NIVOSE;
	}
	
	public void OnTouchTarget()
	{
		if (isTouchTarget)
			return;
		
		isTouchTarget = true;
		
		if (!isHitParticleCreated)
		{
			CreateHitParticle();
		}
		
		// Notify destroy this skill? or change the skill state, and destroy by the skill user in next frame update?
		// All targets play be attacked effect
		GUIBattleMain battle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>();
		if (null != battle)
		{
			foreach (SkillDataSlot.AttackTargetData attackTargetData in skillDataSlot._attackTargetDataList.Values)
			{
				FightCellSlot target = battle.GetFightCellSlot(attackTargetData._targetID);
				target.OnBeAttacked(skillDataSlot);
			}
		}
		
		skillState = SkillState.HIT_TARGET;
	}
	
	public void OnSkillEnd()
	{
		skillIsEnd = true;	
		skillState = SkillState.END;
		
		Destroy(this);
	}
	
	public virtual void CreateFireParticle()
	{
		bool isCreate = false;
		
		// Create the delay property particle
		foreach (SkillEffectData.SkillFireParticleData fireParticleData in skillDataSlot._skillEffectData._skillFireParticleList)
		{
			if (currTrackTime >= fireParticleData._delayTime)
			{
				// Play fire sound
				if (null != fireParticleData._skillSound)
					Globals.Instance.MSoundManager.PlaySoundEffect(fireParticleData._skillSound._soundName);
				
				// Create the fire particle
				string effectName = fireParticleData._particleName;
				
				UnityEngine.Object obj = Resources.Load(effectName);
				if (null == obj)
				{
					isCreate = true;
					Debug.Log("[Skill]: Cann't find the effect name " + effectName);
					break;
				}
				
				
				Vector3 vPos = skillUser.transform.position;
				vPos.z += FightCellSlot.EffectSkillOffset;
				
				GameObject go = Globals.Instance.M3DItemManager.CreateEZ3DItem(obj, vPos);
				go.transform.position = vPos;
				obj = null; // Release it
				
				// Inverse the particle
				if (!skillUser.Property.WarshipIsAttacker)
				{
					Vector3 scale = go.transform.localScale;
					scale.x *= -1f;
					go.transform.localScale = scale;
				}
				
				Globals.Instance.M3DItemManager.DestroyEZ3DItem(go, fireParticleData._durationTime);
				
				isCreate = true;
				skillState = SkillState.FIRE;
			}
		}
		
		isFireParticleCreated = isCreate;
	}
	
	public virtual void CreateFlyParticle()
	{
		bool isCreate = false;
		
		foreach (SkillEffectData.SkillFlyParticleData flyParticleData in skillDataSlot._skillEffectData._skillFlyParticleList)
		{
			if (currTrackTime >= flyParticleData._delayTime)
			{
				// tzz added for null
				if(skillUser  == null){
					continue;
				}
				
				// Play fly sound
				if (null != flyParticleData._skillSound)
				{
					Debug.Log("skillUser " + skillUser.gameObject.name + "Sound " + flyParticleData._skillSound._soundName);
					Globals.Instance.MSoundManager.PlaySoundEffect(flyParticleData._skillSound._soundName);
				}
				
				Vector3 startPosition = skillUser.transform.position;
				
				// Create multiple missile effects
				foreach (SkillDataSlot.AttackTargetData attackTargetData in skillDataSlot._attackTargetDataList.Values)
				{
					UnityEngine.Object obj = Resources.Load(flyParticleData._particleName);
					if (null == obj)
					{
						isCreate = true;
						Debug.Log("[Skill]: Cann't find the fly effect name " + flyParticleData._particleName);
						continue;
					}
					
					// Calculate the Projectile ParticleEffect information
					GUIBattleMain battle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>();
					FightCellSlot target = battle.GetFightCellSlot(attackTargetData._targetID);
					Vector3 endPosition = target.transform.position;
					
					float missileHorzSpeed = flyParticleData.speed * Globals.Instance.MGUIManager.widthRatio;
					float targetHorzSpeed = 0.0f; // target.MoveSpeed;
					if (attackTargetData._moveState == (int)GameData.BattleGameData.MoveState.STOP)
						targetHorzSpeed = 0.0f;
					
					startPosition.z += FightCellSlot.EffectSkillOffset;
					endPosition.z += FightCellSlot.EffectSkillOffset;
					GameObject go = Globals.Instance.M3DItemManager.CreateEZ3DItem(obj, startPosition);
					go.transform.position = startPosition;
					obj = null; // Release it
					
					// Rotate the fly particle
					Vector3 dir = endPosition - startPosition;
					dir.Normalize();
					float dotVal = Vector3.Dot(Vector3.right, dir);
					float degAngle = Mathf.Rad2Deg * Mathf.Acos(dotVal);
					
					Vector3 dirCross = Vector3.Cross(Vector3.right, dir);
					if (Vector3.Dot(dirCross, Vector3.forward) < 0)
					{
						degAngle = 360.0f - degAngle;
					}
					
					go.transform.rotation = Quaternion.AngleAxis(degAngle, Vector3.forward);
					if (skillDataSlot.IsCureSkill())
					{
						SkillTimerTrack.Begin(go, 0.001f, delegate() 
						{
							Globals.Instance.M3DItemManager.DestroyEZ3DItem(go);
							
							OnTouchTarget();
							OnSkillEnd();
						});
					}
					else
					{
						SkillLineTrack.Begin(go, startPosition, endPosition, missileHorzSpeed, targetHorzSpeed, delegate() 
						{
							Globals.Instance.M3DItemManager.DestroyEZ3DItem(go);
							// Complete delegate
							
							OnTouchTarget();
							OnSkillEnd();
						});
					}
				
					isCreate = true;
					skillState = SkillState.FLY;
				} // End foreach
			}
		}
		
		isFlyParticleCreated = isCreate;
	}
	
	public virtual void CreateHitParticle()
	{
		bool isCreate = false;
		
		// Create the hit particles
		foreach (SkillEffectData.SkillHitParticleData hitParticleData in skillDataSlot._skillEffectData._skillHitParticleList)
		{
			// Play fire sound
			if (null != hitParticleData._skillSound)
				Globals.Instance.MSoundManager.PlaySoundEffect(hitParticleData._skillSound._soundName);
			
			// Calculate hit particle type according to DamageRange
			List<SkillDamageRange.Rect> rectList = null;
			
			// Check is need to inverse skill damage range
			bool isSingleSkill = skillDataSlot._skillDamageRange._damageRectList.Count == 1;
			if (isSingleSkill || skillUser.Property.WarshipIsAttacker)
			{
				rectList = skillDataSlot._skillDamageRange._damageRectList;
			}
			else
			{
				// Inverse legend, x Axis
				rectList = new List<SkillDamageRange.Rect>(); 
				foreach (SkillDamageRange.Rect rect in skillDataSlot._skillDamageRange._damageRectList)
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
				
				hitPos.z += FightCellSlot.EffectSkillOffset;
				
				GUIBattleMain battle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>();
				if (null == battle)
					return;
				FightCellSlot primaryTarget = battle.GetFightCellSlot(skillDataSlot._primaryTargetID);
				
				// Optimize the warter effect count
				if (hitType == EHitParticleType.HIT_PARTICLE_WATER_SURFACE)
				{
					if (waterBloomIndex % 2 == 0)
					{
						GameObject go = Globals.Instance.M3DItemManager.CreateEZ3DItem(obj, hitPos);
						go.transform.position = hitPos;
						
						// Inverse the particle
						if (!primaryTarget.Property.WarshipIsAttacker)
						{
							Vector3 scale = go.transform.localScale;
							scale.x *= -1f;
							go.transform.localScale = scale;
						}
						
						Globals.Instance.M3DItemManager.DestroyEZ3DItem(go, hitParticleData._durationTime);
					}
				}
				else
				{
					GameObject go = Globals.Instance.M3DItemManager.CreateEZ3DItem(obj, hitPos);
					go.transform.position = hitPos;
					// Inverse the particle
					if (!primaryTarget.Property.WarshipIsAttacker)
					{
						Vector3 scale = go.transform.localScale;
						scale.x *= -1f;
						go.transform.localScale = scale;
					}
					
					Globals.Instance.M3DItemManager.DestroyEZ3DItem(go, hitParticleData._durationTime);
				}
				obj = null; // Release it
				
				isCreate = true;
			}
		}
		
		isHitParticleCreated = isCreate;
	}
	
	protected void CalculateHitParticleType(SkillDamageRange.Rect rect, out EHitParticleType hitType, out Vector3 hitPos)
	{
		hitType = EHitParticleType.HIT_PARTICLE_PRIMARY;
		hitPos = Vector3.zero;
		
		Rect checkRect = new Rect();
		checkRect.xMin = rect.left;
		checkRect.yMin = rect.bottom;
		
		checkRect.xMax = rect.right;
		checkRect.yMax = rect.top;
		
		GUIBattleMain battle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattleMain>();
		if (null == battle)
			return;
		
		// Calculate the real damage rect
		if (skillDataSlot.MSkillData.BasicData.SkillType != ESkillType.GROUP_NIVOSE)
		{
			FightCellSlot target = battle.GetFightCellSlot(skillDataSlot._primaryTargetID);
			if (null == target)
				return;
			
			Vector3 attachPosition = target.transform.position;
			
			checkRect.xMin += attachPosition.x;
			checkRect.yMin += attachPosition.z;
			
			checkRect.xMax += attachPosition.x;
			checkRect.yMax += attachPosition.z;
		}
		
		// Add the be attacked target effect on the target
		foreach (SkillDataSlot.AttackTargetData attackTargetData in skillDataSlot._attackTargetDataList.Values)
		{
			FightCellSlot target = battle.GetFightCellSlot(attackTargetData._targetID);
			Vector3 attachPosition = target.transform.position;

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
		
		hitType = EHitParticleType.HIT_PARTICLE_WATER_SURFACE;
		hitPos.x = checkRect.center.x;
		hitPos.y = 0.0f;
		hitPos.z = checkRect.center.y;
	}
	
	// Design requirement 2013.01.15: client calulate the nivose skill damage range
	void CalculateNivoseHitRange(List<SkillDamageRange.Rect> rectList)
	{
		if (skillDataSlot.MSkillData.BasicData.SkillType == ESkillType.GROUP_NIVOSE)
		{
			rectList.Clear();
			if (skillUser.Property.WarshipIsAttacker)
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
			} // End if ()
		} // End if ()
	}
}