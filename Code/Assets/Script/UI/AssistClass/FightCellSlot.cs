using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameData;
public class FightCellSlot : MonoBehaviour 
{
	public UIToggle eventBtn;
	
	// Actor avatars or Npc avatars
	public KnightAvatarSlot actorAvatarSlot;
	public KnightAvatarSlot npcAvatarSlot;
	
	public PackedSprite fmtEyeIcon;
	public SpriteText eyePropText;
	
	// Battle 
	public PackedSprite progressBG = null;
	public UISlider lifeProgress = null;
	public UISlider powerProgress = null;
	public UISlider fireProgress = null;
	
	float fireProgressTotalStepTime = 0;
	float fireProgressCounter = 0;
	
	[HideInInspector] public static readonly float PowerIncreasePerAttack = 25f;
	[HideInInspector] public static readonly float PowerIncreasePerBeAttacked = 15f;
	
	[HideInInspector] public WarshipProperty Property = null;
	// Use to simulate the server logic, this is the previous step CurrentLife, and is assigned in the battle step end
	[HideInInspector] public int numBeAttackedPerStep = 0;
	[HideInInspector] public int numCurrBeAttackedPerStep = 0;
	
	[HideInInspector] public int fmtLogicId = -1;
	[HideInInspector] public FormationData.SingleLocation SingleCellData = null;
	[HideInInspector] public GirlData ShipData = null;
	
	[HideInInspector] public static readonly float EffectSkillOffset = -4.0f;
	[HideInInspector] public static readonly float EffectDamageBloodOffset = EffectSkillOffset - 1.0f;
	[HideInInspector] public static readonly float EffectSkillNameOffset = EffectDamageBloodOffset - 2.0f;
	
	Animation animComponent = null;
	EmitEffectFontUtil emitEffectFont = null;
	
	void Awake()
	{
		progressBG.transform.localScale = Vector3.one;
		lifeProgress.transform.localScale = Vector3.one;
		powerProgress.transform.localScale = Vector3.one;
	}
	
	void OnDestroy()
	{
		emitEffectFont = null;
	}
	
	void LateUpdate()
	{
//		if (null != fireProgress)
//		{
//			// set the fire buffer
//			if(!fireProgress.IsHidden() 
//				&& fireProgressTotalStepTime > 0 && fireProgressCounter < fireProgressTotalStepTime)
//			{
//				fireProgressCounter += Time.deltaTime;
//				
//				if(fireProgressCounter > fireProgressTotalStepTime)
//				{
//					fireProgressCounter = fireProgressTotalStepTime;
//				}
//				
//				fireProgress.Value = fireProgressCounter / fireProgressTotalStepTime;
//			}
//		}
	}
	
	public void SetChecked(bool check)
	{
		eventBtn.isChecked = check;
		actorAvatarSlot.SetChecked(check);
	}
	
	public void UpdateCellFmtData(FormationData fmtData, FormationData.SingleLocation cellData)
	{
		if(cellData.isArrayEye)
		{
			fmtEyeIcon.transform.localScale = Vector3.one;
			fmtEyeIcon.PlayAnim(cellData.buffData.Icon);
			
			eyePropText.transform.localScale = Vector3.one;
			SetEyeBuffData(cellData);
		}
		else
		{
			fmtEyeIcon.transform.localScale = Vector3.zero;
			eyePropText.transform.localScale = Vector3.zero;
		}
		
		actorAvatarSlot.Hide(true, false);
		npcAvatarSlot.Hide(true, false);
		progressBG.transform.localScale = Vector3.one;
		lifeProgress.transform.localScale = Vector3.one;
		powerProgress.transform.localScale = Vector3.one;
	}
	
	void SetEyeBuffData(FormationData.SingleLocation cellData)
	{
		string textCol = GUIFontColor.Red255000000;
		if (cellData.buffData.Name.Contains("+"))
			textCol = GUIFontColor.Cyan000255204;
		
		int influenceVal = cellData.buffData.Influence1.Value;
		string wordHit = Globals.Instance.MDataTableManager.GetWordText(10420032);
		string wordDodge = Globals.Instance.MDataTableManager.GetWordText(10420033);
		string wordCrit = Globals.Instance.MDataTableManager.GetWordText(10420034);
		if (cellData.buffData.Name.Contains(wordHit)
			|| cellData.buffData.Name.Contains(wordDodge)
			|| cellData.buffData.Name.Contains(wordCrit))
		{
			if (Mathf.Abs(influenceVal) % 100 < 50)
			{
				if (influenceVal < 0)
					influenceVal -= 50;
				else 
					influenceVal += 50;
			}
				
			influenceVal = Mathf.RoundToInt(influenceVal / 100.0f);
		}
		
		eyePropText.Text = textCol + cellData.buffData.Name + influenceVal.ToString();
	}
	
	public void UpdateCellShipData(GirlData shipData)
	{
		if (null != fmtEyeIcon)
			fmtEyeIcon.transform.localScale = Vector3.zero;
		if (null != eyePropText)
			eyePropText.transform.localScale = Vector3.zero;
		
		ShipData = shipData;
		if (null != shipData)
		{
			if (Property.WarshipIsNpc)
			{
				actorAvatarSlot.Hide(true, false);
				
				npcAvatarSlot.Hide(false, false);
				animComponent = npcAvatarSlot.GetComponent<Animation>() as Animation;
				npcAvatarSlot.UpdateSlot(shipData);
			}
			else
			{
				npcAvatarSlot.Hide(true, false);
				
				actorAvatarSlot.Hide(false, false);
				animComponent = actorAvatarSlot.GetComponent<Animation>() as Animation;
				actorAvatarSlot.UpdateSlot(shipData);
			}
		}
		else
		{
			actorAvatarSlot.Hide(true, false);
			npcAvatarSlot.Hide(true, false);
			
			progressBG.transform.localScale = Vector3.one;
			lifeProgress.transform.localScale = Vector3.one;
			powerProgress.transform.localScale = Vector3.one;
		}
	}
	
	public void SyncPropComponent(GameData.BattleGameData.Fleet fleetData, GirlData data, bool isNpc,GameData.BattleGameData.Ship battleShipData)
	{
		if (null == data)
			return;
		
		ShipData = data;
		if (null == Property)
		{
			Property = gameObject.GetComponent<WarshipProperty>() as WarshipProperty;
			if (null == Property) Property = gameObject.AddComponent(typeof(WarshipProperty)) as WarshipProperty;
		}
		
		Property.WarshipFleetID		= fleetData.FleetID;
		Property.WarshipIsAttacker	= fleetData.IsAttacker;
		Property.WarshipIsNpc		= isNpc;
		
		Property.WarshipID = data.roleCardId;
//		Property.WarshipLogicID = data.BasicData.LogicID;
//		Property.Name = data.BasicData.Name;
//		Property.WarshipIcon = data.BasicData.Icon;
//		
//		Property.WarshipType = (int)data.BasicData.Type;
//		Property.TypeID = (int)data.BasicData.TypeID;
//		
		//Property.WarshipCurrentLife = data.PropertyData.Life;
		//Property.WarshipSimulateLife = data.PropertyData.Life;
		
		Property.WarshipCurrentLife = battleShipData.InitialCurrentLife;
		Property.WarshipSimulateLife = battleShipData.InitialCurrentLife;
		
		//Property.WarshipMaxLife = data.PropertyData.MaxLife;
		
		Property.WarshipCurrentPower = data.PropertyData.Power;
		Property.WarshipSimulatePower = data.PropertyData.Power;
		Property.WarshipMaxPower = 100;
		
		Property.CurrentFillSpeed = data.PropertyData.FillSpeed;
		
		if (0 == Property.WarshipMaxLife)
		{
			Debug.LogError("The dividend is 0");
			return;
		}
		
		progressBG.transform.localScale = Vector3.zero;
		lifeProgress.transform.localScale = Vector3.zero;
		powerProgress.transform.localScale = Vector3.zero;
		
		UpdateLifeProgress((float)Property.WarshipSimulateLife);
		UpdatePowerProgress((float)Property.WarshipSimulatePower);
		
		if (null == emitEffectFont)
		{
			emitEffectFont = gameObject.GetComponent<EmitEffectFontUtil>() as EmitEffectFontUtil;
			if (null == emitEffectFont) emitEffectFont = gameObject.AddComponent(typeof(EmitEffectFontUtil)) as EmitEffectFontUtil;
		}
		
		mPublisher.NotifyCreate(this);
	}
	
	public void InverseAvatar()
	{
		if (Property.WarshipIsNpc)
		{
			Vector3 scale = npcAvatarSlot.transform.localScale;
			scale.x *= -1f;
			npcAvatarSlot.transform.localScale = scale;
		}
		else
		{
			Vector3 scale = actorAvatarSlot.transform.localScale;
			scale.x *= -1f;
			actorAvatarSlot.transform.localScale = scale;
		}
	}
	
	public void Attack(SkillDataSlot skillData)
	{
		// Assign the real normal attack skill id
		int normalSkillId = 1301999000;
		if (skillData._skillID == 0)
		{
			skillData.SkillLogicId = normalSkillId;
		}
		
		if (skillData.SkillLogicId == 1301316000)
		{
			Debug.Log("Current skill id is " + skillData.SkillLogicId);
		}
		Skill2D.StartSkill(this.gameObject, skillData);
		
		UpdatePowerProgress((float)Property.WarshipCurrentPower);
		if (null != animComponent)
		{
			if (skillData.SkillLogicId == normalSkillId)
				animComponent.Play("Attack", PlayMode.StopAll);
			else
				animComponent.Play("Skill", PlayMode.StopAll);
		}
		
		if (null != emitEffectFont)
			emitEffectFont.EmitEffectFont(gameObject, (GameData.BattleGameData.AttackState)skillData._attckerAttackState, skillData.MSkillData.BasicData.SkillName);
		mPublisher.NotifyAttackStart(this, skillData);
	}
	
	public void OnBeAttacked(SkillDataSlot skillData)
	{
		mPublisher.NotifyBeAttacked(this, skillData);
		
		numCurrBeAttackedPerStep++;
		
		// Check this warship is death
		long shipId = ShipData.roleCardId;
		SkillDataSlot.AttackTargetData beAttackData = skillData._attackTargetDataList[shipId];
		
		int damage = beAttackData._beAttackedDamage;
		GameData.BattleGameData.AttackedState attackedState = (GameData.BattleGameData.AttackedState)beAttackData._beAttackedState;
		if (attackedState != GameData.BattleGameData.AttackedState.DODGE)
		{
			Property.WarshipSimulateLife -= damage;
			
			// tzz modified for damage and cure HP
			if (Property.WarshipSimulateLife > Property.WarshipMaxLife)
				Property.WarshipSimulateLife = Property.WarshipMaxLife;
			// Property.WarshipSimulateLife = Mathf.Clamp(Property.WarshipSimulateLife,0,Property.WarshipMaxLife);	
			
			if (numCurrBeAttackedPerStep == numBeAttackedPerStep)
			{
				// Reset
				numBeAttackedPerStep = 0;
				numCurrBeAttackedPerStep = 0;
				
				if (Property.WarshipSimulateLife <= 0.0f)
				{
					OnDeath();
				}
				else
				{
					if (null != animComponent)
					{
						animComponent.Play("Hit", PlayMode.StopAll);
						// animComponent.CrossFade("Hit");
					}
				}
			}
			else
			{
				if (null != animComponent)
				{
					animComponent.Play("Hit", PlayMode.StopAll);
				}
			}
		}
		else
		{
			if (null != animComponent)
			{
				animComponent.Play("Dodge", PlayMode.StopAll);
			}
		}

		UpdateLifeProgress((float)Property.WarshipSimulateLife);
		UpdatePowerProgress((float)Property.WarshipCurrentPower);
		
		//tzz added for REDUCE_DAMAGE_SKILL_ATTACKED
		//
		if(!skillData.IsReduceDamangeSkill()){
			if(skillData.IsCureSkill()){	
				emitEffectFont.EmitEffectFont(gameObject, (GameData.BattleGameData.AttackedState)skillData._attckerAttackState, Mathf.Abs(damage));
			}else{
				emitEffectFont.EmitEffectFont(gameObject, attackedState, damage);
			}			
		}
		
		// tzz added for shaking camera
		List<SkillEffectData.SkillFireParticleData> t_list = skillData._skillEffectData._skillFireParticleList;
		if(t_list.Count > 0 && t_list[t_list.Count - 1]._shakeCamera && skillData.IsNivoseType()){
			// iTween.ShakePosition(Globals.Instance.MSceneManager.mMainCamera.gameObject,new Vector3(20,20,0),1);
			
#if UNITY_IPHONE || UNITY_ANDROID
			if(GameDefines.Setting_ShakeEnable){
				Handheld.Vibrate();
			}			
#endif
		}
	}
	
	// void OnAttackEnd(Skill skill)
	// {
	// 	// According the skill purpose to do someting
	// 	mPublisher.NotifyAttackEnd(this, skill);
	// 	GameStatusManager.Instance.MBattleStatus.OnAttackEnd(this, skill);
	// }
	
	void OnDeath()
	{
		// Note the call order
		mPublisher.NotifyDeath(this);
		
		if (null != animComponent)
		{
			// animComponent.Play("Die", PlayMode.StopAll);
			// if (Property.WarshipIsAttacker)
			// {
			// }
			// else
			// {
			// }
		}
		
//		//if (null != ShipData)
//		//	BattleBloodControl.DestoryBloodStrip(ShipData.WarShipID);
		UpdateCellShipData(null);
	}
	
	void UpdateLifeProgress(float currLife)
	{
		if (null != lifeProgress)
		{
			float val = currLife / Property.WarshipMaxLife;
			val = Mathf.Clamp(val, 0.0f, 1.0f);
			lifeProgress.sliderValue = val;
		}
	}
	
	void IncreasePowerProgress(float addPower)
	{
		if (null != powerProgress)
		{
			float val = addPower / Property.WarshipMaxPower;
			powerProgress.sliderValue = powerProgress.sliderValue + val;
		}
	}
	
	void UpdatePowerProgress(float currPower)
	{
		if (null != powerProgress)
		{
			float val = currPower / Property.WarshipMaxPower;
			val = Mathf.Clamp(val, 0.0f, 1.0f);
			
			powerProgress.sliderValue = val;
		}
	}
	
	void UpdateFireProgress(float val)
	{
		if (null != powerProgress)
			powerProgress.sliderValue = val;
	}
	
	public void SetBufferStepInterval(List<int> bufferList, int currStep){
		int tStepInterval	= 0;
		int tIdx 			= bufferList.IndexOf(currStep);
		
		if(tIdx != -1){
			if(bufferList.Count - 1 == tIdx){
				tStepInterval = WarshipConfig.MaxFillSpeedAttr * 2;
				
			}else{
				tStepInterval = bufferList[tIdx + 1] - bufferList[tIdx];
			}			
		}else{
			return;
		}
		
		fireProgressTotalStepTime = tStepInterval * 2.6f;
		fireProgressCounter = 0;
		
		// fireProgress.Value = 0;
		// 
		// if(fireProgress.IsHidden()){
		// 	fireProgress.transform.localScale = Vector3.zero;
		// }		
	}
	
	WarshipPublisher mPublisher = new WarshipPublisher();
	readonly string[] PinzhiIconNames = new string[]{"KuangDing", "KuangBing", "KuangYi", "KuangJia", "KuangShen"};
}
