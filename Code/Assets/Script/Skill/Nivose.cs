using UnityEngine;
using System.Collections;

public class NiVose : Skill 
{
	public NiVose(SkillDataSlot dataSlot,WarshipL skillUser) :
		base(dataSlot)
	{
		_mFireDuration = 0.0f;
		_mRTTCam = null;
		_mSkillUser = skillUser;
	}
	
	~NiVose()
	{
	}
	
	public override void Initialize ()
	{
		base.Initialize ();
		
		// if not custom camera stat
		// play the camera track
		if(!BattleStatus.CustomCameraState && _mSkillUser.Property.WarshipIsAttacker){
		
			switch (_mSkillDataSlot.MSkillData.BasicData.SkillType)
			{
			case ESkillType.SINGLE_NIVOSE:
				_mCamTrackName = "PathPoints/NivoseSingleTrack";
				break;
			case ESkillType.GROUP_NIVOSE:
				_mCamTrackName = "PathPoints/NivoseGroupTrack";
				break;
			case ESkillType.GROUP_REPAIR_NIVOSE:
				_mCamTrackName = "PathPoints/NivoseGroupRepairTrack";
				break;
			}
			
			Globals.Instance.MCamTrackController.StopAllTracks();
			
			CameraTrack tCameraTrack = Globals.Instance.MCamTrackController.StartNivoseTrack(_mCamTrackName,_mSkillDataSlot);
			
			// tzz modified
			// for the the Battle status camera track
			//
			if(tCameraTrack != null && tCameraTrack.keyFrameInfos.Length >= 1){
				if(GameStatusManager.Instance.MBattleStatus != null){
					CameraTrack tBattleCT = GameStatusManager.Instance.MBattleStatus.GetCurrBattleFightCameraTrack();
					
					if(tBattleCT != null && tBattleCT.keyFrameInfos.Length >= 1){
						// replace the final key 
						//					
						tCameraTrack.keyFrameInfos[tCameraTrack.keyFrameInfos.Length - 1] = tBattleCT.keyFrameInfos[tBattleCT.keyFrameInfos.Length - 1];
					}
				}
			}		
		}
	}
	
	public override void Release ()
	{
		base.Release ();
		
		if (null != _mRTTCam)
			GameObject.Destroy(_mRTTCam.gameObject);
		_mRTTCam = null;
		
		if (null != _mOcclusionPlane)
			GameObject.Destroy(_mOcclusionPlane);
		_mOcclusionPlane = null;
		
		// tzz fucked to comment
		// this CameraTrack will be used by Nivose.cs to Start and Stop it
		// whill stop new Nivose...
		//
		//Globals.Instance.MCamTrackController.StopTrack(_mCamTrackName);
	}
	
	// Update is called once per frame
	public override void Update () 
	{
		if (_mSkillIsEnd)
			return;
		
		_mCurrentTrackTime += Time.deltaTime;
		if (!_mIsFireParticleCreated)
		{
			CreateFireParticle();
		}
		
		if (null != _mRTTCam)
		{
			_mRTTCam.transform.position = Globals.Instance.MSceneManager.mMainCamera.transform.position;
			_mRTTCam.transform.rotation = Globals.Instance.MSceneManager.mMainCamera.transform.rotation;
		}
		
		switch (_mSkillState)
		{
		case SkillState.FIRE:
		{
			if (_mFireDuration <= 0.0f)
			{
				if (!_mIsFlyParticleCreated)
				{					
					CreateFlyParticle();
				}
			}
			_mFireDuration -= Time.deltaTime;
			
			break;
		}
		case SkillState.FLY:
		{
			if (null != _mRTTCam)
			{
				if (null != _mOcclusionPlane)
					GameObject.Destroy(_mOcclusionPlane);
				_mOcclusionPlane = null;
	
				GameObject.Destroy(_mRTTCam.gameObject);
				_mRTTCam = null;
				
				int cullingMask = LayerMaskDefine.GetCullMaskEveryThing();
				cullingMask -= LayerMaskDefine.GetLayerCullMask(LayerMaskDefine.GUI);
				cullingMask -= LayerMaskDefine.GetLayerCullMask(LayerMaskDefine.NIVOSE_RENDER);
				Globals.Instance.MSceneManager.mMainCamera.cullingMask = cullingMask;
				LayerMaskDefine.SetLayerRecursively(_mSkillUser.U3DGameObject, LayerMaskDefine.DEFAULT);
			}
			break;
		}
		case SkillState.HIT_TARGET:
		{
			if (null != _mFollowTarget)
			{
				// iTween.MoveTo(Globals.Instance.MSceneManager.mMainCamera.gameObject, _mOrigPosition, 1.5f);
				// iTween.RotateTo(Globals.Instance.MSceneManager.mMainCamera.gameObject, _mOrigEulerAngles, 1.5f);
			}
			break;
		}
		case SkillState.END:
		{
			break;
		}
			
		}
		
		if (!CheckSkill())
			return;
	}
	
	public override void CreateFireParticle ()
	{
		// Get the skill user
		_mSkillUser = Globals.Instance.MPlayerManager.GetWarship(_mSkillDataSlot._attackerID);
		if (!_mSkillUser.Property.WarshipIsAttacker)
		{
			base.CreateFireParticle();
			
			// tzz modified for creating fire state
			//
			if(_mIsFireParticleCreated){
				_mSkillState = SkillState.FIRE;
			}			
			return;
		}

		bool isCreate = false;
		foreach (SkillEffectData.SkillFireParticleData fireParticleData in _mSkillDataSlot._skillEffectData._skillFireParticleList)
		{
			if (_mCurrentTrackTime >= fireParticleData._delayTime)
			{
				if (!isCreate)
				{
					LayerMaskDefine.SetLayerRecursively(_mSkillUser.U3DGameObject, LayerMaskDefine.NIVOSE_RENDER);
					PrepareNivose();
				}
				
				if (null != fireParticleData._skillSound)
					Globals.Instance.MSoundManager.PlaySoundEffect(fireParticleData._skillSound._soundName);
				
				// Create the fire particle
				GameObject go = null;
				
				string effectName = fireParticleData._particleName;
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
				go = GameObject.Instantiate(Resources.Load(effectName), fireParticleData._position, 
					fireParticleData._rotation) as GameObject;
				
				GameObject tagPoint = _mSkillUser.GetTagPointGameObject(TagPointDefine.TAG_POINT_SHIP_CANNON);
				if (null == tagPoint)
				{
					go.transform.position = _mSkillUser.U3DGameObject.transform.position;
				}
				else
				{
					go.transform.parent = tagPoint.transform;
					go.transform.localPosition = Vector3.zero;
				}
				
				LayerMaskDefine.SetLayerRecursively(go, LayerMaskDefine.NIVOSE_RENDER);
				GameObject.DestroyObject(go, fireParticleData._durationTime);
				
				_mFireDuration = fireParticleData._durationTime;
				isCreate = true;
				_mSkillState = SkillState.FIRE;
			}
		}
		
		_mIsFireParticleCreated = isCreate;
	}
	
	public override void CreateFlyParticle ()
	{
		// tzz modified for enemy NiVose fly particle
		//
//		if (!_mSkillUser.Property.WarshipIsAttacker)
//		{
//			base.CreateFlyParticle();
//			
//			// tzz modified for creating fly state
//			//
//			if(_mIsFlyParticleCreated){
//				_mSkillState = SkillState.FLY;
//			}
//			return;
//		}

		bool isCreate = false;
		foreach (SkillEffectData.SkillFlyParticleData flyParticleData in _mSkillDataSlot._skillEffectData._skillFlyParticleList)
		{
			if (_mCurrentTrackTime >= flyParticleData._delayTime)
			{
				if (null != flyParticleData._skillSound)
					Globals.Instance.MSoundManager.PlaySoundEffect(flyParticleData._skillSound._soundName);
				
				WarshipL skillUser = Globals.Instance.MPlayerManager.GetWarship(_mSkillDataSlot._attackerID);
				Vector3 startPosition = skillUser.U3DGameObject.transform.position;
				
				// Create multiple missile effects
				foreach (SkillDataSlot.AttackTargetData attackTargetData in _mSkillDataSlot._attackTargetDataList.Values)
				{
					
					string effectName = flyParticleData._particleName;
					if (_mSkillUser.Property.WarshipIsAttacker)
					{
						effectName = flyParticleData._particleName + "_L";
					}
					else
					{
						effectName = flyParticleData._particleName + "_R";
					}
					
					// Calculate the Projectile ParticleEffect information
					GameObject go = GameObject.Instantiate(Resources.Load(effectName), 
						Vector3.zero, Quaternion.identity) as GameObject;
					if (null == go)
					{
						Debug.Log("The effect resource " + effectName + "is not found");
						continue;
					}
					
					WarshipL target = Globals.Instance.MPlayerManager.GetWarship(attackTargetData._targetID);
					Vector3 endPosition = target.U3DGameObject.transform.position;
					
					float missileHorzSpeed = flyParticleData.speed;
					float targetHorzSpeed = target.MoveSpeed;
					if (attackTargetData._moveState == (int)GameData.BattleGameData.MoveState.STOP)
						targetHorzSpeed = 0.0f;
					
					MotionTrack motionTrack = null;
					
					startPosition.y += 50.0f;
					endPosition.y += 50.0f;
					
					// missileHorzSpeed = 150;
					go.transform.position = startPosition;
					motionTrack = new AirCraftTrack(startPosition, endPosition, missileHorzSpeed, targetHorzSpeed);
					
					MissileL missile = new MissileL(go, motionTrack);
					_mMissileEffectList.Add(missile);
					_mFlyEffectObjs.Add(go);
					
					isCreate = true;
					_mSkillState = SkillState.FLY;
					break;
				} // End foreach
			}
		}
		
		_mIsFlyParticleCreated = isCreate;

	}
	
	public override void CreateHitParticle ()
	{
		base.CreateHitParticle ();
	}
	
	private void PrepareNivose()
	{
		// _mOrigPosition = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		// _mOrigEulerAngles = Globals.Instance.MSceneManager.mMainCamera.transform.rotation.eulerAngles;
		// _mOrigCullingMask = Globals.Instance.MSceneManager.mMainCamera.cullingMask;
		// 
		// Globals.Instance.MSceneManager.mMainCamera.transform.position = _mSkillUser.U3DGameObject.transform.position + new Vector3(0.0f, 50.0f, -100.0f);
		// Globals.Instance.MSceneManager.mMainCamera.transform.LookAt(_mSkillUser.U3DGameObject.transform.position);
		
		int cullingMask = LayerMaskDefine.GetCullMaskEveryThing();
		cullingMask -= LayerMaskDefine.GetLayerCullMask(LayerMaskDefine.GUI);
		cullingMask -= LayerMaskDefine.GetLayerCullMask(LayerMaskDefine.NIVOSE_RENDER);
		Globals.Instance.MSceneManager.mMainCamera.cullingMask = cullingMask;
		
		// Duplicate a RTT camera
		GameObject cam = new GameObject();
		cam.name = "RttCamera";
		_mRTTCam = cam.AddComponent(typeof(Camera)) as Camera;
		_mRTTCam.CopyFrom(Globals.Instance.MSceneManager.mMainCamera);
		_mRTTCam.cullingMask = LayerMaskDefine.GetLayerCullMask(LayerMaskDefine.NIVOSE_RENDER);
		_mRTTCam.clearFlags = CameraClearFlags.Depth;
		_mRTTCam.depth = Globals.Instance.MSceneManager.mMainCamera.depth + 2;
		_mRTTCam.gameObject.tag = TagMaskDefine.UNTAGGED;

		// A transparent plane
		CreateOcclusionPlane();
	}
	
	private void CreateOcclusionPlane()
	{
		Object obj = Resources.Load("Common/OcclusionPlane");
		_mOcclusionPlane = Object.Instantiate(obj) as GameObject;
		LayerMaskDefine.SetLayerRecursively(_mOcclusionPlane, LayerMaskDefine.DEFAULT);
		
		_mOcclusionPlane.transform.parent = Globals.Instance.MSceneManager.mMainCamera.transform;
		_mOcclusionPlane.transform.localRotation = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
		_mOcclusionPlane.transform.localPosition = new Vector3(0.0f, 0.0f, Globals.Instance.MSceneManager.mMainCamera.near + 0.1f);
		_mOcclusionPlane.transform.localScale = new Vector3(Globals.Instance.MSceneManager.mMainCamera.near, 1.0f, Globals.Instance.MSceneManager.mMainCamera.near);
	}
	
	private Camera _mRTTCam;
	private float _mFireDuration;
	private GameObject _mOcclusionPlane;
	
	private string _mCamTrackName;

	private Vector3 _mOrigPosition;
	private Vector3 _mOrigEulerAngles;
	private int _mOrigCullingMask;
	
	private GameObject _mFollowTarget;
}
