using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarshipL 
{
	public delegate void CreateCallback(WarshipL ws);
	public delegate void MoveDelegate(WarshipL ws);
	
	//-----------------------------------------------------------
	public static readonly float NAME_HEIGHT_OFFSET = 40.0f;
	//-----------------------------------------------------------
	
	/// <summary>
	/// The find waypoint array.
	/// </summary>
	private static Vector2[] smFindWaypointArray		= new Vector2[100];
	
	/// <summary>
	/// The sm find waypoing number.
	/// </summary>
	private static int		smFindWaypointNum			= 0;
	
	/// <summary>
	/// The index of the sm find waypoint.
	/// </summary>
	private static int		smFindWaypointIndex			= 0;
	
	/// <summary>
	/// The state of the sm find waypoint.
	/// </summary>
	private bool			mFindWaypointState			= false;
	
	
	#region Property
	public GameObject U3DGameObject
	{
		get { return _thisGameObject; }
	}
	
	public string GameObjectTag
	{
		get 
		{ 
			if (null == _thisGameObject)
				return null;
			
			return _thisGameObject.tag; 
		}
		set 
		{
			if (_thisGameObject != null)
				TagMaskDefine.SetTagRecuisively(_thisGameObject, value);
		}
	}
	
	public int MInstanceID
	{
		get 
		{
			// System.Type t = this.GetType();
			// System.Reflection.FieldInfo[] filedInfos = t.GetFields();
			// 
			// System.Reflection.FieldInfo info = filedInfos[0];
			// int id = this.GetHashCode();
			// id = info.GetHashCode();
			if (null != _thisGameObject)
				return _thisGameObject.GetInstanceID();
			else
			{
				throw new MissingReferenceException();
			}
		}
	}
	
	public bool IsInCopy
	{
		get { return _isInCopy; }
		set { _isInCopy = value; }
	}
	
	public float MoveSpeed
	{
		get { return _mMoveSpeed; }
		set { _mMoveSpeed = value; }
	}
	
	//------------------------------------------------------------
	public WarshipProperty Property
	{
		get { return _mProperty; }
	}
	
	public GirlData GirlData
	{
		get { return _mData; }
	}
	
	public bool IsLoaded
	{
		get;
		set;
	}
	
	//------------------------------------------------------------
	#endregion
	
	public WarshipL(GirlData data)
	{
		if (null != data)
		{
			_mData = data;
		
			_warshipID = data.roleCardId;
//			_warshipID = data.WarShipID;
//			_warshipLogicID = data.BasicData.LogicID;
//			_warshipResourceName = data.BasicData.ModelName;
		}
		
		
		_mAnimationController = null;
	}
	
	public virtual void Initialize(CreateCallback callback)
	{
		// Object obj = Resources.Load (_warshipResourceName);
		// if (null == obj)
		// 	return;
		// 	
		// _thisGameObject = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
		// // Control the GameObject ourself
		// GameObject.DontDestroyOnLoad(_thisGameObject);
		// 
		// if (_warshipName != null)
		// 	_thisGameObject.name = _warshipName;
		// Animation am = _thisGameObject.GetComponent<Animation>();
		// if (am != null)
		// {
		// 	_mAnimationController = new AnimationController(am);
		// }
		// else
		// {
		// 	am = _thisGameObject.GetComponentInChildren<Animation>();
		// 	if (am != null)
		// 		_mAnimationController = new AnimationController(am);
		// }
		// 
		// _emitEffectFontUtil = _thisGameObject.AddComponent<EmitEffectFontUtil>();
		// 
		// AddPropertyComp();
		// Create3DName();
		// Show3DName(false);
		// 
		// InitializeAttachmentSlots();
		// 
		// // Notify warship is created
		// OnWarshipCreated();
		// 
		// if (null != callback)
		// 	callback(this);
		// 
		// IsLoaded = true;
		
		Globals.Instance.MResourceManager.Load(_warshipResourceName, delegate(Object obj, string error) 
		{
			if (null == obj)
				return;
			
			_thisGameObject = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
			// Control the GameObject ourself
			GameObject.DontDestroyOnLoad(_thisGameObject);
			LayerMaskDefine.SetLayerRecursively(_thisGameObject, LayerMaskDefine.REFLECTION);
			
			if (_warshipName != null)
				_thisGameObject.name = _warshipName;
			Animation am = _thisGameObject.GetComponent<Animation>();
			if (am != null)
			{
				_mAnimationController = new AnimationController(am);
			}
			else
			{
				am = _thisGameObject.GetComponentInChildren<Animation>();
				if (am != null)
					_mAnimationController = new AnimationController(am);
			}
			
			_emitEffectFontUtil = _thisGameObject.AddComponent<EmitEffectFontUtil>();
			
			AddPropertyComp();
			Create3DName();
			CreateWarshipHeader();
			Show3DName(false);
			
			InitializeAttachmentSlots();
			
			// Notify warship is created
			OnWarshipCreated();
			
			if (null != callback)
				callback(this);
			
			IsLoaded = true;
		});
	}
	
	public void ReplaceMode(string newResouce, CreateCallback callback)
	{
		Globals.Instance.MResourceManager.Load(newResouce, delegate(Object obj, string error) 
		{
			if (null == obj)
				return;
			
			GameObject.Destroy(_thisGameObject);
			
			_thisGameObject = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
			GameObject.DontDestroyOnLoad(_thisGameObject);
			LayerMaskDefine.SetLayerRecursively(_thisGameObject, LayerMaskDefine.REFLECTION);
			
			if (_warshipName != null)
				_thisGameObject.name = _warshipName;
			
			Animation am = _thisGameObject.GetComponent<Animation>();
			if (am != null)
			{
				_mAnimationController = new AnimationController(am);
			}
			else
			{
				am = _thisGameObject.GetComponentInChildren<Animation>();
				if (am != null)
					_mAnimationController = new AnimationController(am);
			}
			
			_emitEffectFontUtil = _thisGameObject.AddComponent<EmitEffectFontUtil>();
			
			AddPropertyComp();
			Create3DName();
			CreateWarshipHeader();
			Show3DName(false);
			
			InitializeAttachmentSlots();
			
			if (null != callback)
				callback(this);
			
			IsLoaded = true;
		});
	}
	
	public virtual void Release()
	{
		ReleaseAttachmentSlots();
	
		if (_thisGameObject != null)
		{
			GameObject.Destroy(_thisGameObject);
			_thisGameObject = null; // tzz add for clear attribute member
			
			BattleBloodControl.DestoryBloodStrip(_warshipID);			
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(_mEZ3DName.gameObject);
		}
		
		// deleete the general avatar if has
		if(mWarshipHeader != null){
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(mWarshipHeader.gameObject);
			mWarshipHeader = null;
		}
		
		// _warshipID = -1;
		_warshipResourceName = string.Empty;
		
	}
	
	public virtual void AddPropertyComp()
	{
		if (null == _mData)
			return;
		
		// Add Property Component;
		_mProperty = _thisGameObject.AddComponent(typeof(WarshipProperty)) as WarshipProperty;
		
		// 2012.05.02 LiHaojie use new PropertyComponent
		_mProperty.WarshipID = _mData.roleCardId;
//		_mProperty.WarshipLogicID = _mData.BasicData.LogicID;
//		_mProperty.Name = _mData.BasicData.Name;
//		
//		_mProperty.WarshipType = (int)_mData.BasicData.Type;
//		_mProperty.TypeID = (int)_mData.BasicData.TypeID;
		
		// Fleet data
		// _mProperty.WarshipFleetID = fleet._fleetID;
		// _mProperty.WarshipIsAttacker = fleet._isAttachFleet;
		
		_mProperty.WarshipCurrentLife = _mData.PropertyData.Life;
		_mProperty.WarshipSimulateLife = _mData.PropertyData.Life;
		//_mProperty.WarshipMaxLife = _mData.PropertyData.MaxLife;
		
		_mProperty.WarshipCurrentPower = _mData.PropertyData.Power;
		_mProperty.WarshipSimulatePower = _mData.PropertyData.Power;
		_mProperty.WarshipMaxPower = 100;
		
		_bloodValue = (float)Property.WarshipSimulateLife / (float)Property.WarshipMaxLife;
	}
	
	#region AttachmentSlot
	public void InitializeAttachmentSlots()
	{
	
	}
	
	public void ReleaseAttachmentSlots()
	{
		_mAttachmentSlotList.Clear();
	}
	
	public void SetDefaultAttachObjectActive(bool visible)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		// Inactive now
		DeactiveAllAttachObjects();
		
		// Default Chimney effect
		if (visible)
		{
			_mTagPointRootGameObject.active = true;
			GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_CHIMNEY);
			if (null != go)
				ActiveAttachObject(go);
		}
		
	}
	
	public GameObject GetTagPointGameObject(string name)
	{
		if (null == _mTagPointRootGameObject)
			return null;
		
		Transform tf = _mTagPointRootGameObject.transform.Find(name);
		if (null != tf)
		{
			GameObject go = tf.gameObject;
			return go;
		}
		
		return null;
	}
	
	public void AttahcObjectTo(string parentName, string prefab)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		GameObject parent = GetTagPointGameObject(parentName);
		AttachObjectTo(parent, prefab);
	}
	
	public void AttachObjectTo(GameObject parent, GameObject child)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		AttachmentSlot attachmentSlot = null;
		bool isContain = _mAttachmentSlotList.TryGetValue(parent.GetInstanceID(), out attachmentSlot);
		if (!isContain || attachmentSlot == null)
		{
			attachmentSlot = new AttachmentSlot(parent);
			_mAttachmentSlotList.Add(parent.GetInstanceID(), attachmentSlot);
		}
		
		GameObject result = attachmentSlot.AttachObject(child, Vector3.zero, Quaternion.identity);
	}
	
	public void AttachObjectTo(GameObject parent, string prefab)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		AttachmentSlot attachmentSlot = null;
		bool isContain = _mAttachmentSlotList.TryGetValue(parent.GetInstanceID(), out attachmentSlot);
		if (!isContain || attachmentSlot == null)
		{
			attachmentSlot = new AttachmentSlot(parent);
			_mAttachmentSlotList.Add(parent.GetInstanceID(), attachmentSlot);
		}
		
		Object obj = Resources.Load(prefab);
		if (null == obj)
		{
			//
			return;
		}
		
		GameObject result = attachmentSlot.AttachObject(obj, Vector3.zero, Quaternion.identity);
		obj = null;
	}
	
	public void DetachObjectFrom(string parentName, GameObject child)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		GameObject parent = GetTagPointGameObject(parentName);
		DetachObjectFrom(parent, child);
	}
	
	public void DetachObjectFrom(GameObject parent, GameObject child)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		AttachmentSlot attachmentSlot = null;
		bool isContain = _mAttachmentSlotList.TryGetValue(parent.GetInstanceID(), out attachmentSlot);
		if (!isContain)
			return;
		
		attachmentSlot.DetachObject(child);
	}
	
	public void ActiveAttachObject(GameObject parent, GameObject child)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		AttachmentSlot attachmentSlot = null;
		bool isContain = _mAttachmentSlotList.TryGetValue(parent.GetInstanceID(), out attachmentSlot);
		if (!isContain)
			return;
		
		if (!child.active)
			attachmentSlot.ActiveAttachObject(child);
	}
	
	public void DeactiveAttachObject(GameObject parent, GameObject child)
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		AttachmentSlot attachmentSlot = null;
		bool isContain = _mAttachmentSlotList.TryGetValue(parent.GetInstanceID(), out attachmentSlot);
		if (!isContain)
			return;
		
		if (child.active)
			attachmentSlot.DeactiveAttachObject(child);
	}
	
	public void ActiveAttachObject(GameObject parent)
	{
		if (!parent.active)
			parent.SetActiveRecursively(true);
	}
	
	public void DeactiveAttachObject(GameObject parent)
	{
		if (parent.active)
			parent.SetActiveRecursively(false);
	}
	
	public void ActiveAllAttachObjects()
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		_mTagPointRootGameObject.SetActiveRecursively(true);
	}
	
	public void DeactiveAllAttachObjects()
	{
		if (null == _mTagPointRootGameObject)
			return;
		
		_mTagPointRootGameObject.SetActiveRecursively(false);
	}
	
	#endregion
	
	public void PlayAnimation(int animationID, float speed, bool loop, bool autoStop)
	{

	}
	
	public void PlayAnimation(string animation, float speed, bool loop, bool autoStop)
	{
		if (_mAnimationController == null)
			return;
		
		_mAnimationController.PlayAnimation(animation, speed, loop, autoStop);
		
		// Add animation event.
		_mAnimationController.addAnimationFinishEvent(animation, this.OnAnimationFinish);
	}
	
	public void StopAnimation()
	{
		if (_mAnimationController == null)
			return;
		
		_mAnimationController.StopAnimation();
	}
	
	public void StopAnimation(string name)
	{
		if (_mAnimationController == null)
			return;
		
		_mAnimationController.StopAnimation(name);
	}
	
	public void SetMoveEndDelegate(MoveDelegate del)
	{
		moveEndDelegate = del;
	}
	
	// Implement a skill body, include Encapsulate  the Particles, Actions, SkillData etc.	
	public void ForceMoveTo(Vector3 destination)
	{
		_isMoving = false;
		_moveDestination = destination;
		
		// Directly move the GameObject to destination
		this._thisGameObject.transform.position = destination;
	}
	
	public bool MoveTo(Vector3 destination,bool findWay = false)
	{
		Vector3 currentPosition = _thisGameObject.transform.position;
		mFindWaypointState = findWay;
		
		Vector3 finalDestPos = destination;
		
		if(findWay){
			FindWayPoint.Instance.ComputeWayPoint(new Vector2(currentPosition.x,currentPosition.z),
												new Vector2(destination.x,destination.z),smFindWaypointArray,out smFindWaypointNum);
			if(smFindWaypointNum == 0){
				return false;
			}
			
			smFindWaypointIndex = 0;
			destination.x = smFindWaypointArray[0].x;
			destination.z = smFindWaypointArray[0].y;
		}
		
		_isMoving = true;
		_moveDestination = destination;
		_moveDestination.y = currentPosition.y;
				
		_moveDirection = _moveDestination - currentPosition;
		_moveDirection.Normalize();
		
		StopAnimation();
		
		{
			// Active Wave Effect
			GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_LEFT);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_RIGHT);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_BACK);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_CHIMNEY);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_LEFT);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_RIGHT);
			if (null != go)
				ActiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_BACK);
			if (null != go)
				ActiveAttachObject(go);
		}
		
		if(findWay){
				
			float mahatunDistance = Mathf.Abs(finalDestPos.x - smFindWaypointArray[smFindWaypointNum - 1].x) + 
									Mathf.Abs(finalDestPos.z - smFindWaypointArray[smFindWaypointNum - 1].y);
			return mahatunDistance < 2;
			
		}else{
			return true;
		}
	}
	
	public virtual void Stop()
	{
		if(mFindWaypointState){
			if(++smFindWaypointIndex < smFindWaypointNum){
				// tzz added for find way point
				//
				Vector3 t_destPos = new Vector3(smFindWaypointArray[smFindWaypointIndex].x,0,smFindWaypointArray[smFindWaypointIndex].y);							
				MoveTo(t_destPos,false);
				mFindWaypointState = true;
				return;
			}
		}
		
		mFindWaypointState = false;
		_isMoving = false;
		_moveDirection = Vector3.zero;
		_moveDestination = _thisGameObject.transform.position;
		PlayAnimation(AnimationDefine.ANIMATION_IDLE, 1.0f, true, false);
		
		{
			// InActive Wave Effect
			GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_LEFT);
			if (null != go)
				DeactiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_FRONT_RIGHT);
			if (null != go)
				DeactiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_WAVE_BACK);
			if (null != go)
				DeactiveAttachObject(go);
			
			go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_CHIMNEY);
			if (null != go)
				DeactiveAttachObject(go);
		}
	}
	
	public void ForceSink()
	{
		// Play death animation
		int animationID = 0;
			
		//TODO
		PlayAnimation(animationID, 1.0f, false, false);
	}
	
	public virtual void SetActive(bool visible)
	{
		if (_thisGameObject == null)
			return;
		
		_thisGameObject.SetActiveRecursively(visible);
		//Globals.Instance.MSystemUIMain.SetActorNameActive(visible, _warshipID);
		SetDefaultAttachObjectActive(visible);
		
		if (null != _mEZ3DName && null != _mEZ3DName.gameObject)
		{
			_mEZ3DName.gameObject.SetActiveRecursively(visible);
		}
	}
	
	private void ActiveGameObject(GameObject current)
	{
		for (int childIndex = 0; childIndex < current.transform.GetChildCount(); ++ childIndex)
		{
			GameObject child = current.transform.GetChild(childIndex).gameObject;
			child.active = true;
			
			ActiveGameObject(child);
		}
	}
	
	private void InActiveGameObject(GameObject current)
	{
		for (int childIndex = 0; childIndex < current.transform.GetChildCount(); ++ childIndex)
		{
			GameObject child = current.transform.GetChild(childIndex).gameObject;
			child.active = false;
			
			InActiveGameObject(child);
		}
	}
	
	public void Attack(SkillDataSlot skillData)
	{
		// Assign the real normal attack skill id
		if (skillData._skillID == 0)
		{
			EWarshipTypeID type = (EWarshipTypeID)Property.TypeID;
			switch (type)
			{
			case EWarshipTypeID.AIRCRAFT_CARRIER:
			{
				skillData._skillID = NormaAttackDefine.NORMAL_AIRCRAFT_CARRIER;
				break;
			}
			case EWarshipTypeID.UNDER_WATER_CRAFT:
			{
				skillData._skillID = NormaAttackDefine.NORMAL_UNDER_WATER_CRAFT;
				break;
			}
			default:
			{
				skillData._skillID = NormaAttackDefine.NORMAL_CANNON_CRAFT;
				break;
			}
			}
		}
		
		// Fill in skill data from DataTable
		skillData.MSkillData.BasicData.SkillLogicID = skillData._skillID;
		skillData.MSkillData.FillDataFromConfig();
		
		// Create skill, and execute the skill logic
		Skill skill = Globals.Instance.MSkillManager.CreateSkill(skillData,this);
		skill.StartSkill();
		
		// add attack effect font
		GameData.BattleGameData.AttackState attackState = (GameData.BattleGameData.AttackState)skillData._attckerAttackState;
		BattleBloodControl bloodControl = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>()._bloodControl;
		if(attackState == GameData.BattleGameData.AttackState.NORMAL_ATTACK_CRIT)
		{
			bloodControl.EmitEffectFont(_thisGameObject.transform, _thisGameObject.transform.position, new Vector3(0f,40f,0f), BattleEffectFont.EffectType.CRIT, 0, "");
		}
		else if(attackState == GameData.BattleGameData.AttackState.SKILL_ATTACK)
		{
			// Only skill attack allow display effect font
			if (!skillData.MSkillData.BasicData.SkillIsNormalAttack){
				bloodControl.EmitEffectFont(_thisGameObject.transform, 
					_thisGameObject.transform.position, new Vector3(0f,30f,0f), BattleEffectFont.EffectType.SKILL, 0, skillData.MSkillData.BasicData.SkillName);
			}
		}
		else if(attackState == GameData.BattleGameData.AttackState.SKILL_ATTACK_CRIT)
		{
			bloodControl.EmitEffectFont(_thisGameObject.transform, _thisGameObject.transform.position, new Vector3(0f,40f,0f), BattleEffectFont.EffectType.CRIT_SKILL, 0, skillData.MSkillData.BasicData.SkillName);
			
		}else if(attackState == GameData.BattleGameData.AttackState.TREAT_SKILL_ATTACKED
			|| attackState == GameData.BattleGameData.AttackState.REDUCE_DAMAGE_SKILL_ATTACKED){
			// tzz added for display name of TREAT_SKILL_ATTACKED and REDUCE_DAMAGE_SKILL_ATTACKED
			//
			bloodControl.EmitEffectFont(_thisGameObject.transform, 
					_thisGameObject.transform.position, new Vector3(0f,30f,0f), BattleEffectFont.EffectType.SKILL, 0, skillData.MSkillData.BasicData.SkillName);
		}
	}
	
	public void OnAttackStart(Skill skill)
	{	
		string animName = skill._mSkillDataSlot._skillEffectData._skillAnimation._animationName;
		PlayAnimation(animName, 1.0f, false, false);
		
		GameStatusManager.Instance.MBattleStatus.OnAttackStart(this, skill);
		
	}
	
	public void OnAttackEnd(Skill skill)
	{
		// According the skill purpose to do someting
		_mEventPublisher.NotifyAttackEnd(this, skill);
		GameStatusManager.Instance.MBattleStatus.OnAttackEnd(this, skill);
	}
	
	public void OnBeAttacked(Skill skill)
	{
		_numCurrentBeAttacked += 1;
		
		// Check this warship is death
		SkillDataSlot.AttackTargetData beAttackData = skill._mSkillDataSlot._attackTargetDataList[this._warshipID];
		int damage = beAttackData._beAttackedDamage;
		GameData.BattleGameData.AttackedState attackedState = 
			(GameData.BattleGameData.AttackedState)beAttackData._beAttackedState;
		
		if (attackedState != GameData.BattleGameData.AttackedState.DODGE)
		{
			Property.WarshipSimulateLife -= damage;
			
			// tzz modified for damage and cure HP
			if (Property.WarshipSimulateLife > Property.WarshipMaxLife)
				Property.WarshipSimulateLife = Property.WarshipMaxLife;
			// Property.WarshipSimulateLife = Mathf.Clamp(Property.WarshipSimulateLife,0,Property.WarshipMaxLife);			
			
			GirlData.PropertyData.Life = Property.WarshipSimulateLife;
			
			float remainLifePercent = (float)Property.WarshipSimulateLife / Property.WarshipMaxLife;
			if ( remainLifePercent <= 0.4f && remainLifePercent > 0.2f )
			{
				GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_BREAK_DOWN);
				if (null != go)
					ActiveAttachObject(go);
			}
			else if (remainLifePercent <= 0.2f && remainLifePercent > 0f)
			{
				GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_DESTROY_FRONT);
				if (null != go)
					ActiveAttachObject(go);
				go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_DESTROY_BACK);
				if (null != go)
					ActiveAttachObject(go);
			}
		}
		
		if (_numCurrentBeAttacked == _numBeAttacked)
		{
			_numBeAttacked = 0;
			_numCurrentBeAttacked = 0;
			
			if (Property.WarshipSimulateLife <= 0.0f)
			{
				Stop();
				PlayAnimation(AnimationDefine.ANIMATION_DEAD,  1.0f, false, false);
				
				{
					// Play die particle, falling into sea effect
					GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_SPRAY);
					if (null != go)
						ActiveAttachObject(go);
				}
			}
			else
			{
				PlayAnimation(AnimationDefine.ANIMATION_HURT,  1.0f, false, false);
			}
		}
		else
		{
			// Play be hit animation
			PlayAnimation(AnimationDefine.ANIMATION_HURT,  1.0f, false, false);
		}
		
		_bloodValue = (float)Property.WarshipSimulateLife / (float)Property.WarshipMaxLife;
		_bloodValue = Mathf.Clamp(_bloodValue, 0.0f, 1.0f);
				
		//tzz added for REDUCE_DAMAGE_SKILL_ATTACKED
		//
		if(!skill._mSkillDataSlot.IsReduceDamangeSkill()){
			if(skill._mSkillDataSlot.IsCureSkill()){	
				_emitEffectFontUtil.EmitEffectFont(_thisGameObject, (GameData.BattleGameData.AttackedState)skill._mSkillDataSlot._attckerAttackState, Mathf.Abs(damage));
			}else{
				_emitEffectFontUtil.EmitEffectFont(_thisGameObject, attackedState, damage);
			}			
		}
		
		_mEventPublisher.NotifyBeAttacked(this, skill);
		GameStatusManager.Instance.MBattleStatus.OnBeAttacked(this, skill);
				
		// tzz added for shaking camera
		List<SkillEffectData.SkillFireParticleData> t_list = skill._mSkillDataSlot._skillEffectData._skillFireParticleList;
		if(t_list.Count > 0 && t_list[t_list.Count - 1]._shakeCamera && skill.IsNivoseType()){
			iTween.ShakePosition(Globals.Instance.MSceneManager.mMainCamera.gameObject,new Vector3(20,20,0),1);
			
#if UNITY_IPHONE || UNITY_ANDROID
			if(GameDefines.Setting_ShakeEnable){
				Handheld.Vibrate();
			}			
#endif
		}
	}
	
	public void OnAnimationFinish(System.Object data)
	{
		AnimationController.AnimationStateSlot slot = data as AnimationController.AnimationStateSlot;
		if (slot._name == "Move")
		{
			// Debug.Log("The animation is End, it's name is: " + slot._name);
		}
		else if (slot._name == AnimationDefine.ANIMATION_ATTACK)
		{
			// Debug.Log("The animation is End, it's name is: " + slot._name);
		}
		else if (slot._name == AnimationDefine.ANIMATION_DEAD)
		{
			// Debug.Log("The animation is End, it's name is: " + slot._name);
			
			// The death flag, the we will destroy this in the next frame
			_isDeath = true;
			OnWarshipDeath();
		}
	}
	
	public void OnWarshipCreated()
	{
		_mEventPublisher.NotifyCreate(this);
		GameStatusManager.Instance.MBattleStatus.OnWarshipCreated(this);
	}
	
	public void OnWarshipDeath()
	{
		{
			GameObject go = GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_BREAK_DOWN);
			if (null != go)
				ActiveAttachObject(go);
		}
		
		_mEventPublisher.NotifyDeath(this);
		GameStatusManager.Instance.MBattleStatus.OnWarshipDeath(this);
	}
	
	public void Update()
	{
		UnityEngine.Profiling.Profiler.BeginSample("WarshipL.Update");
		
		//show the blood of the warship
		if(_bloodDisplay)
		{
			GUIBattle guiBattle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if(guiBattle != null && guiBattle._bloodControl != null)
				guiBattle._bloodControl.DisplayBloodStrip(_warshipID, _bloodValue, _thisGameObject.transform.position, new Vector3(0f,20f,0f));
		}
		
		UpdateMovement();
		UpdateAnimation();
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
//	public void UpdateNamePosition()
//	{
//		Profiler.BeginSample("WarshipL.UpdateNamePosition");
//		
//		if (_mEZ3DName != null			
//			&& _mEZ3DName.gameObject.active){
//			
//			Vector3 worldPos = _thisGameObject.transform.position;
//			worldPos.y += NAME_HEIGHT_OFFSET;
//			_mEZ3DName.Reposition(worldPos);
//		}
//		
//		Profiler.EndSample();
//	}
	
	public void UpdateMovement()
	{
		UnityEngine.Profiling.Profiler.BeginSample("WarshipL.UpdateMovement");
		
		if (!_isMoving)
		{
			if (_isInCopy)
			{
				Vector3 pos = _thisGameObject.transform.position;
				
				if (Time.frameCount % 8 == 0)
				{
					pos.y = _mIsPlus ? pos.y + 0.005f : pos.y - 0.005f;
					_mIsPlus = !_mIsPlus;
				}
				
				_thisGameObject.transform.position = pos;
			}
			
		}else{
			
			UnityEngine.Profiling.Profiler.BeginSample("WarshipL.UpdateMovement.QuaternionOP");
			
			// Smooth move
			// Calculate move direction
			Vector3 currentPosition = _thisGameObject.transform.position;
			Vector3 wantedPosition = currentPosition + _moveDirection * _mMoveSpeed * Time.deltaTime;
			
			// Slowly rotate the warship
			float rotationDamping = 1.5f;
			Quaternion currentRotation = _thisGameObject.transform.rotation;
			
			Quaternion destRotation = Quaternion.FromToRotation(_thisGameObject.transform.right, _moveDirection);
			destRotation = currentRotation * destRotation;
			
			// Quaternion destRotation = Quaternion.FromToRotation(Vector3.right, _moveDirection);
			currentRotation = Quaternion.Lerp(currentRotation, destRotation, Time.deltaTime * rotationDamping);
			// Force the x & z axis angle is 0
			currentRotation.eulerAngles = new Vector3(0.0f, currentRotation.eulerAngles.y, 0.0f);
						
			_thisGameObject.transform.rotation = currentRotation;
			_thisGameObject.transform.position = wantedPosition;
			
			UnityEngine.Profiling.Profiler.EndSample();
			
			UnityEngine.Profiling.Profiler.BeginSample("WarshipL.UpdateMovement.Vector3OP");
			
			// Detect whether to stop according to one frame * movespeed
			Vector3 leftMovement 		= _moveDestination - currentPosition;
			Vector3 t_currMoveDirection = leftMovement.normalized;
			
			Vector3 nextFrameMovement 	= _moveDirection * _mMoveSpeed * Time.deltaTime;
			float distanceEqual 		= Vector3.Distance(leftMovement, nextFrameMovement);
						
			if (distanceEqual <= 2.0f 
			// tzz modified for stop ship when Time.deltaTime is too large
			//
			|| Vector3.Dot(_moveDirection,t_currMoveDirection) < 0 )
			{
				
				_thisGameObject.transform.position = _moveDestination;
				
				Stop();		
				
				if (null != moveEndDelegate) 
				{
					moveEndDelegate(this);
				}
			}
			
			UnityEngine.Profiling.Profiler.EndSample();			
		}
		
		UnityEngine.Profiling.Profiler.EndSample();		
	}
	
	public void UpdateAnimation()
	{
		UnityEngine.Profiling.Profiler.BeginSample("WarshipL.UpdateAnimation");
		
		if (_mAnimationController != null){
			_mAnimationController.Update();
		}
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
	public bool IsHasGameObject(GameObject dest)
	{
		return IsEqualGameObject(_thisGameObject, dest);
	}
	
	private bool IsEqualGameObject(GameObject src, GameObject dest)
	{
		if (src == dest)
			return true;
		
		foreach (Transform tf in src.transform)
		{
			if (IsEqualGameObject(tf.gameObject, dest))
				return true;
		}
		
		return false;
	}
	
	public virtual void Create3DName()
	{
		if (null != _mEZ3DName && _mEZ3DName.gameObject)
		{
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(_mEZ3DName.gameObject);
			_mEZ3DName = null;
		}
		
		_mEZ3DName = Create3DNameImpl(_thisGameObject,_mProperty.Name);
	}
	
	/// <summary>
	/// Create3s the D name impl.
	/// </summary>
	/// <returns>
	/// The D name impl.
	/// </returns>
	/// <param name='worldPos'>
	/// World position.
	/// </param>
	/// <param name='showText'>
	/// Show text.
	/// </param>
	public static EZ3DSimipleText Create3DNameImpl(object gameObject,string name){
				
		string showText = GUIFontColor.White + name;		
		EZ3DSimipleText text = (EZ3DSimipleText)Globals.Instance.M3DItemManager.Create3DSimpleText(gameObject, showText,NAME_HEIGHT_OFFSET);
		text.gameObject.name = name;
		
		return text;
	}
	
	/// <summary>
	/// tzz added
	/// Creates the general avatar if has
	/// </summary>
	private void CreateWarshipHeader(){
		if(GameStatusManager.Instance.GetStatus() == GameState.GAME_STATE_BATTLE){
			mWarshipHeader = Globals.Instance.M3DItemManager.CreateGeneralAvatar(this);
		}
		
	}
	
	private void Create3DBlood()
	{}
	
	public  void Show3DName(bool show)
	{
		_mEZ3DName.gameObject.SetActiveRecursively(show);
	}
	
	public void Show3DBlood(bool show)
	{
	}
	
	public void setBloodDisplay(bool boolean)
	{
		_bloodDisplay = boolean;
	}
	
	private WarshipPublisher _mEventPublisher = new WarshipPublisher();
	
	private GameObject _thisGameObject;
	private GameObject _mTagPointRootGameObject;
		
	protected EZ3DSimipleText _mEZ3DName;
	
	private WarshipProperty _mProperty = null;
	public GirlData _mData = null;
	private AnimationController _mAnimationController = null;
	
	// An warship have how many TagPoints
	private Dictionary<int, AttachmentSlot> _mAttachmentSlotList = new Dictionary<int, AttachmentSlot>();
	
	// Declare public variable
	public long _warshipID;
	public int _warshipLogicID;
	public string _warshipName;
	
	public int _warshipResourceID;
	public string _warshipResourceName;
	
	private bool _isNpc = false;
	
	// Use to simulate the server logic, this is the previous step CurrentLife, and is assigned in the battle step end
	public int _numBeAttacked;
	public int _numCurrentBeAttacked;
	
	// Move logic
	public bool _isMoving;
	protected float _mMoveSpeed = GameDefines.BATTLE_GRID_WIDTH / GameDefines.BATTLE_STEP_TIME;
	
	private Vector3 _moveDirection;
	private Vector3 _moveDestination;
	
	// If is true, then destroy it in the next frame
	public bool _isDeath = false;
	
	private bool _isInCopy = false;
	private bool _mIsPlus = false;
	
	private EmitEffectFontUtil _emitEffectFontUtil;
	
	private float _bloodValue = 1f;
	private bool _bloodDisplay = false;
	/*
	public float _emitEffectFontTime = 0.5f;
	public int _emitEffectFontCount = 0;
	public GameData.BattleGameData.AttackedState _currentAttackState;
	public int _currentEffectFontDamage = 0;
	*/
	
	MoveDelegate moveEndDelegate;
	
	/// <summary>
	/// tzz added 
	/// The m warship general avatar.
	/// </summary>
	private EZWarshipHeader	mWarshipHeader = null;
	
	/// <summary>
	/// Gets the warship header for header and buffer progress
	/// </summary>
	/// <value>
	/// The warship header.
	/// </value>
	public EZWarshipHeader WarshipHeader{
		get{return mWarshipHeader;}
	}
	
	
}
