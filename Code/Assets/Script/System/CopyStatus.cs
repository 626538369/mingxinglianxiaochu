using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyStatus : GameStatus, IFingerEventListener
{
	public static readonly float SneakDegree = 70.0f;
	public static readonly float SituRotateUnit = 2.0f / Mathf.PI;
	
	public CopyStatus()
	{
		_mPublisher = new CopyStatusPublisher();
	}
	
	public override void Initialize()
	{
		// tzz added for cutscenes camera track
		if(!InitCutscenesCameraTrack()){
			Initialize_impl();
		}
		
		// tzz added
		// close the GUINewCard window if enter copy by click of materials
		//GUINewCard tCard = Globals.Instance.MGUIManager.GetGUIWindow<GUINewCard>();
		//if(tCard != null){
		//	tCard.Close();			
		//}
	}
	
	/// <summary>
	/// Initialize implement function 
	/// </summary>
	protected virtual void Initialize_impl(){
		
		_mIsEnabled = true;
		_mIsFinalBattle = false;
		_mIsPermitRequestBattle = true;
		
		MCurrentCopyData = Globals.Instance.MGameDataManager.MCurrentCopyData;
		
		// Get the FingerEvent Component, Register FingerEventListener
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
		
//		InitActor();
		InitMonsters();
		InitIndicator();
		InitRandomChests();
		
		InitMissionArea();
		
		RegisterSubscriber();
		//InitMissionArea();
		// Change the main ButtonPortGo to BackPortBtn
		_mPublisher.NotifyEnterCopy();
		_mPublisher.NotifyNPCFleetCount(_mMonsterFleetList.Count);
		
		Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.EnterCopy,"CopyID",MCurrentCopyData.MCopyBasicData.CopyID);
	}
	
	// template of camera track for buffering Resource.Load
	private GameObject	m_enterCameraTrackTemplate = null;
	private string		m_formerEnterCameraTrackName = "";
	
	/// <summary>
	/// Inits the cutscenes camera track.
	/// </summary>
	private bool InitCutscenesCameraTrack(){
		
		if(GameDefines.Setting_SkipCopyCameraTrack){
			return false;
		}
		
		string t_enterCameraTrack = Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyEnterCameraTrack;
		
		if(t_enterCameraTrack.Length > 0){
			Cutscenes t_cut = CreateCutscene(t_enterCameraTrack,delegate(Cutscenes _cut){
				
				if(!_cut.m_initActorMonster){
					// intialize the copyStatus
					Initialize_impl();
				}
			});
			
			return !t_cut.m_initActorMonster;
		}
		
		return false;
	}
	
	/// <summary>
	/// Creates the cutscene.
	/// </summary>
	/// <returns>
	/// The cutscene.
	/// </returns>
	/// <param name='_cutsceneName'>
	/// _cutscene name.
	/// </param>
	/// <param name='_playDoneDele'>
	/// _play done dele.
	/// </param>
	/// <exception cref='System.Exception'>
	/// Is thrown when the resouce load is failed.
	/// </exception>
	private Cutscenes  CreateCutscene(string _cutsceneName,Cutscenes.PlayDoneDelegate _playDoneDele){
		if(m_formerEnterCameraTrackName != _cutsceneName){
				
			GameObject t_loadRes = null;
			Globals.Instance.MResourceManager.Load(_cutsceneName, delegate(Object obj, string error) {
				
				if(obj == null){
					Debug.LogError("obj == null InitCutscenesCameraTrack : " + error);
					return;
				}
				
				t_loadRes = (GameObject)obj;
			});
			
			if(t_loadRes == null){
				throw new System.Exception("t_loadRes == null in InitCutscenesCameraTrack");
			}
			
			m_enterCameraTrackTemplate		= t_loadRes;
			m_formerEnterCameraTrackName	= _cutsceneName;
		}
							
		GameObject t_cutsceneObj = (GameObject)GameObject.Instantiate(m_enterCameraTrackTemplate);
		Cutscenes t_cutscenes = t_cutsceneObj.GetComponent<Cutscenes>();
		
		t_cutscenes.SetPlayOverDelegate(delegate(Cutscenes cut){
			_playDoneDele(cut);
		});
		
		return t_cutscenes;		
	}
	
	public override void Release()
	{
		if (_mActorFleet != null)
		{
			Globals.Instance.MPlayerManager.RemoveFleet(_mActorFleet);
		}
		
		ClearMonsters();
		ClearGuideArrows();
		ClearIndicator();
		ClearRandomChests();

		// Get the FingerEvent Component, UnRegister FingerEventListener
		this.SetFingerEventActive(false);
		Globals.Instance.MFingerEvent.Remove3DEventListener(this);
		
		UnRegisterSubscriber();
		
		//long playerID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		//NetSender.Instance.RequestQuitCopy(playerID);
		
		
		_mPublisher.NotifyLeaveCopy();
	}
	
	public override void Pause()
	{
		_mPaused = true;
		_mIsPermitRequestBattle = false;
		
		SetFingerEventActive(false);
		if (_mActorFleet != null)
		{
			_mActorFleet.Stop();
		}
		
		foreach (MonsterFleet fleet in _mMonsterFleetList)
		{
			fleet.Stop();
		}
	}
	
	public override void Resume()
	{
		_mPaused = false;
		_mIsPermitRequestBattle = true;
		
		SetFingerEventActive(true);
		foreach (MonsterFleet fleet in _mMonsterFleetList)
		{
			fleet.SetActive(true);
		}
	}
	
	public override void Update()
	{
		if (!_mIsEnabled || _mPaused)
			return;
		
		UpdateCamera();
		
		// Calculate every frame?
		UpdateGuideArrows();
		UpdateIndicator();
	}
	
	public void SetEnabled(bool enabled)
	{		
		if (_mActorFleet != null)
		{
			_mActorFleet.SetActive(enabled);
			if (!enabled)
				_mActorFleet.Stop();
		}
		
		foreach (MonsterFleet fleet in _mMonsterFleetList)
		{
			fleet.SetActive(enabled);
			
			if (!enabled)
				fleet.Stop();
			
			// Arrow active
			if (null != _mGuideArrowList[fleet])
			{
				_mGuideArrowList[fleet].SetActiveRecursively(enabled);
			}
		}
		
		// Chest list
		foreach (GameObject go in _mChestList.Values)
		{
			if (null != go)
				go.SetActiveRecursively(enabled);
		}
		
		// tzz added 
		// _IndicatorEffect == null when cutscenes state first
		//
		if(_IndicatorEffect != null){
			_IndicatorEffect.SetActiveRecursively(enabled);
		}
		
		if(_mOceanRange != null)
		{
			_mOceanRange.SetActiveRecursively(enabled);
		}
		
		_mIsEnabled = enabled;
		_mPublisher.NotifyNPCFleetCount(_mMonsterFleetList.Count);
	}
	
	public List<MonsterFleet> GetHoldMonsterList()
	{
		return _mMonsterFleetList;
	}
	
	public virtual void OnRequestBattle(GameObject first, GameObject other)
	{
		if (!_mIsPermitRequestBattle)
			return;
		_mIsPermitRequestBattle = false;
		
	
	}
	
	public virtual void OnBattleEnd(GameData.BattleGameData.BattleResult battleResult){
		if(battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.ACTOR_WIN // is win
			&& Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyType == 2 // boss type
			&& _mMonsterFleetList.Count == 1){ // finall monster
			
			// fade in copy scene
			// SceneFadeInOut.Instance.FadeInScene(3);
			
			CreateCutscene("TempArtist/Animation/DefeatBoss",delegate(Cutscenes _cutscenes){
				OnBattleEnd_impl(battleResult);
				// SceneFadeInOut.Instance.FadeInScene(3);
			});
			
		}else{
			OnBattleEnd_impl(battleResult);
		}
		_mPublisher.NotifyNPCFleetCount(_mMonsterFleetList.Count);
	}
	
	protected virtual void OnBattleEnd_impl(GameData.BattleGameData.BattleResult battleResult)
	{
		// Resume the camera
		Globals.Instance.MSceneManager.mMainCamera.transform.position = _mCurrentCamPos;
		Globals.Instance.MSceneManager.mMainCamera.transform.forward = _mCurrentCamForward;
		
		// resume trigger battle, and close Input
		SetEnabled(true);
		SetFingerEventActive(true);
		_mIsPermitRequestBattle = true;
		
		// Clear ReinforcementData
		Globals.Instance.MGameDataManager.MActorData.RemoveReinforcementData();
		
		Vector3 monsterPos = Vector3.zero;
		switch (battleResult.BattleWinResult)
		{
			// Actor win
		case GameData.BattleGameData.EBattleWinResult.ACTOR_WIN:
		{
			// Destroy the guide
			DestroyGuideArrows(_mFightingMonsterFleet);
			
			// Destroy the failed Fleet according to BattleResult
			monsterPos = _mFightingMonsterFleet.Position;
			_mMonsterFleetList.Remove(_mFightingMonsterFleet);
			Globals.Instance.MPlayerManager.RemoveMonsterFleet(_mFightingMonsterFleet);
			_mFightingMonsterFleet = null;

			// Create a random chest
			if (battleResult.ChestID != -1)
			{
				GameObject go = CreateRandomChest(battleResult.ChestID);
				go.transform.position = monsterPos;
			}
			
			// Single Drap information
			OnceBattleFinish();
			
			break;
		}
			// Dogfall
		case GameData.BattleGameData.EBattleWinResult.DOGFALL:
		{
			// Destroy the guide
			DestroyGuideArrows(_mFightingMonsterFleet);
			
			// Destroy the failed Fleet according to BattleResult
			monsterPos = _mFightingMonsterFleet.Position;
			_mMonsterFleetList.Remove(_mFightingMonsterFleet);
			Globals.Instance.MPlayerManager.RemoveMonsterFleet(_mFightingMonsterFleet);
			_mFightingMonsterFleet = null;

			// Create a random chest
			if (battleResult.ChestID != -1)
			{
				GameObject go = CreateRandomChest(battleResult.ChestID);
				go.transform.position = monsterPos;
			}
			break;
		}
			// Monster win
		case GameData.BattleGameData.EBattleWinResult.MONSTER_WIN:
		{
			// tzz added 
			// move the warship by monster collider bounds to avoid the fight immedately again
			//
			Vector3 t_monsterSize 	= _mFightingMonsterFleet.m_battleTrigger.GetComponent<Collider>().bounds.size;
			
			// increase some distance
			t_monsterSize.x += 50;
						
			// get the own actor ship position
			Vector3 t_ownPos = Vector3.zero;
			foreach (WarshipL ws in _mActorFleet._mWarshipList){
				t_ownPos = ws.U3DGameObject.transform.position;
				break;
			}
			
			// monster(Enemy) position
			Vector3 t_monsterPos = _mFightingMonsterFleet.m_copyStatusGameObject.transform.position;	
			
			// actor direct back position
			Vector3 t_direclyBackPos = t_monsterPos + ((t_ownPos - t_monsterPos).normalized * t_monsterSize.x);					
					
			Vector2[] t_restPos = 
			{
				new Vector2(t_direclyBackPos.x,t_direclyBackPos.z),
				new Vector2(t_monsterPos.x + t_monsterSize.x,t_monsterPos.z),
				new Vector2(t_monsterPos.x - t_monsterSize.x,t_monsterPos.z),
				new Vector2(t_monsterPos.x ,				t_monsterPos.z + t_monsterSize.x),
				new Vector2(t_monsterPos.x ,				t_monsterPos.z - t_monsterSize.x),
			};				
			
			foreach(Vector2 pos in t_restPos){
				if(crossArea.Contains(pos)){
					// rest position is in the cross Area
					//
					foreach (WarshipL ws in _mActorFleet._mWarshipList){
						ws.ForceMoveTo(new Vector3(pos.x,t_ownPos.y,pos.y));
					}
					break;
				}
			}
			
			Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.FailedCopyBattle,"CopyID",Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID);
			break;
		}	
		
		}
		
		// Flush ship life
		Dictionary<long,GirlData> dictWarshipData = Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
		List<GameData.BattleGameData.WarshipBattleEndCurrLife> listWarshipLife = GameStatusManager.Instance.MBattleStatus.MBattleResult.ListWarshipBattleEndCurrLife;
		foreach(GameData.BattleGameData.WarshipBattleEndCurrLife warshipLife in listWarshipLife)
		{
			if(dictWarshipData.ContainsKey(warshipLife.ShipID))
			{
				dictWarshipData[warshipLife.ShipID].PropertyData.Life = warshipLife.ShipCurrLife;
			}
		}
		
//		Globals.Instance.MGameDataManager.MActorData.NotifyWarshipUpdate();
		Debug.Log("copystatas");
		Globals.Instance.M3DItemManager.PlayBattleEffect(battleResult, delegate(){
			
			Globals.Instance.MGUIManager.CreateWindow<GUISingleDrop>(delegate(GUISingleDrop gui)
			{
				gui.UpdateData();
			});
		});
		
		if (battleResult.IsFinalBattle)
		{
			_mIsFinalBattle = true;
			
			GameObject go = GraphicsTools.ManualCreateObject("TempArtist/Prefab/Particle/S_LeaveCopy", "LeaveTrigger", false);
			float y = go.transform.position.y;
			LeaveCopyTrigger trigger = go.AddComponent<LeaveCopyTrigger>() as LeaveCopyTrigger;
			go.transform.position = new Vector3(monsterPos.x, y, monsterPos.z);
			go.transform.localScale = Vector3.one;
			mLeaveTrigger = go.transform.position;
			// DisplayCopyDrops();
		}
	}
		
	protected void OnceBattleFinish()
	{
		if (null != NetReceiver.Instance.PlayerInfoChangePacket)
		{
			NetReceiver.Instance.ReceivePlayerInfoChange(NetReceiver.Instance.PlayerInfoChangePacket);
			NetReceiver.Instance.PlayerInfoChangePacket = null;
		}
		
		//GUITempPack.CheckTempPackToPopup();
		
		Debug.Log("battle end");
	}
	
	public virtual void DisplayCopyDrops()
	{
		if (!_mIsFinalBattle)
			return;
		
		// Forbid the input and stop all GameObjects
		if (null != _mActorFleet)
		{
			_mActorFleet.Stop();
		}
		
		CopyBattleFinish();
	}
	
	protected virtual void CopyBattleFinish()
	{	
		_mIsEnabled = false;
		SetFingerEventActive(false);
		
		Globals.Instance.MTaskManager.GoToAccomplishTalk(delegate()
		{
			// Reslove the multiple reset _mIsEnabled and SetFingerEventActive
			_mIsEnabled = false;
			SetFingerEventActive(false);
			
			Vector3 worldPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
			worldPos += Globals.Instance.MSceneManager.mMainCamera.transform.forward * (Globals.Instance.MSceneManager.mMainCamera.near * 1.5f);
		
			// Play Chest effect
			//Globals.Instance.MEffectManager.CreateGoldChest(worldPos, delegate(GameObject effect)
			//{
			//	Globals.Instance.MGUIManager.CreateWindow<GUICopyDrop>(delegate(GUICopyDrop gui)
			//	{
			//		gui.UpdateData();
			//	});
			//});
		});
	}

	#region Handle Finger Event
	public bool IsFingerEventActive()
	{
		return _mIsFingerEventActive;
	}
	
	public void SetFingerEventActive(bool active)
	{
		_mIsFingerEventActive = active;
	}

	public bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
		// if (_mIsFinalBattle)
		// 	return true;
		// 
		MoveWarShipByFingerPos(fingerPos);
		// Debug.Log("ActorFleet want to position is " + worldPos);
		return true;
	}
	
   	public bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		return false;
	}
	
	public bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		
		return false;	
	}
	
	public bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		return false;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		PortStatus.OnFingerPinchMove_imple(delta);	
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		return false;
	}
	
	public bool OnFingerStationaryBeginEvent (int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
	
	public bool OnFingerStationaryEndEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}
	
	public bool OnFingerStationaryEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}
	#endregion
	public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{
		return true;
	}
	public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		if (!_mIsEnabled)
			return true;
		
		MoveWarShipByFingerPos(fingerPos);
		
		return true;
	}
	public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	
	private void MoveWarShipByFingerPos(Vector2 fingerPos)
	{
		Vector3 originalPos = Vector3.zero;
		Vector3 worldPos = Vector3.zero;
		
		Vector3 clipPos = Vector3.zero;
		
		// WarshipL opShip = null;
		// GameObject opObj = null;
		if(GetWorldPosition(fingerPos, out worldPos))
		{
			foreach (WarshipL ws in _mActorFleet._mWarshipList)
			{
				originalPos = ws.U3DGameObject.transform.position;
				
				clipPos.x = Mathf.Clamp(worldPos.x,crossArea.xMin,crossArea.xMax);
				clipPos.z = Mathf.Clamp(worldPos.z,crossArea.yMin,crossArea.yMax);
				
				// Hold the Y value
				clipPos.y = originalPos.y;
				
				if(!ws.MoveTo(clipPos,true)){
					Globals.Instance.MGUIManager.ShowSimpleCenterTips(20400016,true, 1.0f);
				}
//				
//				// The distance from current position to clipPos is so near
//				if (Mathf.Abs(originalPos.x - crossArea.xMin) < 5.0f || Mathf.Abs(originalPos.x - crossArea.xMax) < 5.0f
//					|| Mathf.Abs(originalPos.z - crossArea.yMin) < 5.0f || Mathf.Abs(originalPos.z - crossArea.yMax) < 5.0f)
//				{
//					Globals.Instance.MGUIManager.ShowSimpleCenterTips(20400016, 1.0f);
//				}
//				ws.SetMoveEndDelegate(delegate(WarshipL iWs) 
//				{
//					if (iWs.U3DGameObject.transform.position.x <= crossArea.xMin || iWs.U3DGameObject.transform.position.x >= crossArea.xMax
//						|| iWs.U3DGameObject.transform.position.z <= crossArea.yMin || iWs.U3DGameObject.transform.position.z >= crossArea.yMax)
//					{
//						Globals.Instance.MGUIManager.ShowSimpleCenterTips(20400016, 1.0f);
//					}
//				});
			}
		}
		
		// // LiHaojie 2012.12.21 Add a situ rotation feature
		// if (null != opObj && null != opShip)
		// {
		// 	Vector3 fromDir = opObj.transform.right;
		// 	Vector3 toDir = (clipPos - originalPos);
		// 	toDir.Normalize();
		// 	
		// 	float dot = Vector3.Dot(fromDir, toDir);
		// 	float radian = Mathf.Acos(dot);
		// 	
		// 	Vector3 startEulerAngles = opObj.transform.rotation.eulerAngles;
		// 	Vector3 endEulerAngles = startEulerAngles + Quaternion.FromToRotation(fromDir, toDir).eulerAngles;
		// 	
		// 	// Stop the iTween rotate type first
		// 	opShip.Stop();
		// 	iTween.Stop(opObj, "rotate");
		// 	InSituRotation(opObj, endEulerAngles, SituRotateUnit * radian, delegate() 
		// 	{
		// 		opShip.MoveTo(clipPos);
		// 	});
		// }
		
		// A little raise
		worldPos.y += 0.5f;
		SetIndicatorInfo(_mActorFleet.Position, worldPos);
	}
	
	void InSituRotation(GameObject obj, Vector3 toDir, float time, iTween.EventDelegate del)
	{
		iTween.RotateTo(obj, iTween.Hash("rotation", toDir, "easetype", iTween.EaseType.easeInOutQuart, "time", time), del);
	}
	
	/// <summary>
	/// Gets the actor fleet position.
	/// </summary>
	/// <returns>
	/// The actor fleet position.
	/// </returns>
	private GameObject GetActorFleet(){
		Vector3 t_position = Vector3.zero;
		
		foreach (WarshipL ws in _mActorFleet._mWarshipList)
		{
			return ws.U3DGameObject;
		}
		
		return null;
	}
	
	/// <summary>
	/// Gets the world position to lead ths ship go
	/// </summary>
	/// <returns>
	/// The world position.
	/// </returns>
	/// <param name='screenPos'>
	/// If set to <c>true</c> screen position.
	/// </param>
	/// <param name='worldPos'>
	/// If set to <c>true</c> world position.
	/// </param>
	private bool GetWorldPosition(Vector2 screenPos, out Vector3 worldPos)
	{
		worldPos = Vector3.zero;		
		Ray ray = Globals.Instance.MSceneManager.mMainCamera.ScreenPointToRay( screenPos );		
        RaycastHit hit;
		
		bool t_hited = Physics.Raycast(ray, out hit, Mathf.Infinity,(1 << 4));

		if (t_hited){
			worldPos = hit.point;
		}
		
		return t_hited;
	}
	
//	protected virtual void InitActor()
//	{	
//		int fleetID = 1000;
//		FleetL fleet = Globals.Instance.MPlayerManager.CreateFleet(fleetID);
//		
//		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
//		GirlData wsData = actorData.GetActorFakeWarshipData();
//		
//		GirlData fakeData = new GirlData();
//		fakeData.WarShipID = 1000000;
//		
//		// Huoqu dangqian zhenxing zhong zuixiao suoyin weizhi chuan de logicID
//		if (null != wsData)
//			fakeData.BasicData.LogicID = wsData.BasicData.LogicID;
//		else
//			fakeData.BasicData.LogicID = 1217001000;
//		
//		fakeData.BasicData.IsNpc = false;
//		fakeData.BasicData.FillDataFromConfig();
//		
//		// Use fleet name to display in copy
//		fakeData.BasicData.Name = actorData.BasicData.FleetName;
//		
//		fleet.CreateWarship(fakeData, delegate(WarshipL ws) 
//		{
//			ws.U3DGameObject.name = fakeData.BasicData.Name;
//			ws.GameObjectTag = TagMaskDefine.GFAN_ACTOR;
//			ws.MoveSpeed = 100.0f;
//			ws.IsInCopy = true;
//			
//			// Move to birth position
//			ws.ForceMoveTo(MCurrentCopyData.MCopyBasicData.ActorBirthPosition);
//			ws.Show3DName(true);
//			
//			//add rigid body 
//			Rigidbody rigidbody = ws.U3DGameObject.AddComponent<Rigidbody>() as Rigidbody;
//			if(rigidbody)
//			{
//				rigidbody.useGravity = false;
//				rigidbody.isKinematic = true;
//			}
//			
//			BoxCollider coll = ws.U3DGameObject.AddComponent<BoxCollider>() as BoxCollider;
//			if(coll)
//			{
//				coll.isTrigger = true;
//				coll.center = new Vector3(0,0,0);
//				coll.size = new Vector3(100,20,20);
//			}
//		});
//		
//		// BattleTrigger trigger = ws.U3DGameObject.AddComponent<BattleTrigger>() as BattleTrigger;
//		// trigger.BoxSize = new Vector3(100,20,20);
//		// trigger.TriggerEnterEvents += this.OnRequestBattle;
//		
//		_mActorFleet = fleet;
//	}
//	
//	void ReplaceActorModel()
//	{
//		if (null == _mActorFleet)
//			return;
//		
//		PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
//		GirlData t_wsData = actorData.GetActorFakeWarshipData();
//		if (null == t_wsData)
//			return;
//		
//		t_wsData.BasicData.Name = actorData.BasicData.FleetName;
//		
//		WarshipL tWarship = _mActorFleet._mWarshipList[0];
//		Vector3 destPos = tWarship.U3DGameObject.transform.position;
//		Quaternion destQuat = tWarship.U3DGameObject.transform.rotation;
//		
//		tWarship.ReplaceMode(t_wsData.BasicData.ModelName, delegate(WarshipL ws) 
//		{
//			ws.U3DGameObject.name = t_wsData.BasicData.Name;
//			ws.GameObjectTag = TagMaskDefine.GFAN_ACTOR;
//			ws.MoveSpeed = 100.0f;
//			ws.IsInCopy = true;
//			
//			ws.U3DGameObject.transform.position = destPos;
//			ws.U3DGameObject.transform.rotation = destQuat;
//			
//			ws.Show3DName(true);
//			
//			//add rigid body 
//			Rigidbody rigidbody = ws.U3DGameObject.AddComponent<Rigidbody>() as Rigidbody;
//			if(rigidbody)
//			{
//				rigidbody.useGravity = false;
//				rigidbody.isKinematic = true;
//			}
//			
//			BoxCollider coll = ws.U3DGameObject.AddComponent<BoxCollider>() as BoxCollider;
//			if(coll)
//			{
//				coll.isTrigger = true;
//				coll.center = new Vector3(0,0,0);
//				coll.size = new Vector3(100,20,20);
//			}
//		});
//	}
	
	protected virtual void InitMonsters()
	{
		foreach (CopyMonsterData.MonsterData monsterData in MCurrentCopyData.MCopyMonsterData.MMonsterDataList)
		{
			// Create Fleet
			MonsterFleet monsterFleet = Globals.Instance.MPlayerManager.CreateMonsterFleet(monsterData.FleetID);
			
			// Create Warship
			monsterFleet.CreateMonsterWarship(monsterData, delegate(WarshipL ws) 
			{
				MonsterWarship monster = ws as MonsterWarship;
				monster.ForceMoveTo(monsterData.MonsterPosition);
				monster.U3DGameObject.name = monsterData.MonsterName;
				// monster._thisGameObject.layer = LayerMaskDefine.IGNORE_RAYCAST;
				
				// Register Active Event
				monster.AddTriggerCallback(this.OnRequestBattle);
				monster.StartPathPatrolAI();
				
				// Add into MonsterFleetList
				_mMonsterFleetList.Add(monsterFleet);
				
				// Instantiate 3d arrow
				GameObject prefab = Globals.Instance.M3DItemManager.MonsterArrowTP;
				GameObject go = Globals.Instance.M3DItemManager.CreateEZ3DItem(prefab, Vector3.zero);
				go.name = "MonsterGuideArrow";								
				_mGuideArrowList.Add(monsterFleet, go);
				
				if (monster.VisibleTrigger)
					monster.VisibleTrigger.ControlTarget = go;
			});
		}
	}
	
	protected virtual void ClearMonsters()
	{
		foreach (MonsterFleet monster in _mMonsterFleetList)
		{
			Globals.Instance.MPlayerManager.RemoveMonsterFleet(monster);
		}
		_mMonsterFleetList.Clear();
	}
	
	private void ClearGuideArrows()
	{
		foreach (GameObject go in _mGuideArrowList.Values)
		{
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(go);
		}
		_mGuideArrowList.Clear();
	}
	
	private void DestroyGuideArrows(MonsterFleet key)
	{
		GameObject go = null;
		if (_mGuideArrowList.TryGetValue(key, out go))
		{
			_mGuideArrowList.Remove(key);
			Globals.Instance.M3DItemManager.DestroyEZ3DItem(go);
		}
	}
	
	protected virtual void InitIndicator()
	{
		if (null == _IndicatorEffect)
		{
			_IndicatorEffect = GraphicsTools.ManualCreateObject("TempArtist/Prefab/Particle/S_Point", "Indicator", true);
		}
		
		// if (null == _mRoute)
		// {
		// 	_mRoute = GraphicsTools.ManualCreateObject("Common/Route", "Route");
		// }
	}
	
	protected virtual void ClearIndicator()
	{
		if (null != _IndicatorEffect)
		{
			GraphicsTools.ManualDestroyObject(_IndicatorEffect, true);
			_IndicatorEffect = null;
		}
		
		// if (null != _mRoute)
		// {
		// 	GraphicsTools.ManualDestroyObject(_mRoute);
		//	_mRoute = null;
		// }
	}
	
	protected virtual void SetIndicatorInfo(Vector3 startPos, Vector3 endPos)
	{
		if (null != _IndicatorEffect)
		{
			// _IndicatorEffect.SetActiveRecursively(false);
			_IndicatorEffect.SetActiveRecursively(true);
			_IndicatorEffect.transform.position = endPos;
		}
		
		// if (null != _mRoute)
		// {
		// 	Vector3 dir = endPos - startPos;
		// 	_mRoute.transform.position = 0.5f * (startPos + endPos);
		// 	_mRoute.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
		// }
	}
	
	protected virtual void InitRandomChests()
	{
		// Get the position list
		Object obj = null;
		
		if (null != MCurrentCopyData.MCopyBasicData.ChestPointsFile)
			obj = Resources.Load(MCurrentCopyData.MCopyBasicData.ChestPointsFile);
		else
			obj = Resources.Load("PathPoints/Copy1ChestPoints");
			
		GameObject go = GameObject.Instantiate(obj) as GameObject;
//		AIPathPatrol pathAI = go.GetComponent<AIPathPatrol>() as AIPathPatrol;
//		_mChestPosList = new Vector3[pathAI.pathPointInfos.Length];
//		for (int i = 0; i < _mChestPosList.Length; ++i)
//		{
//			_mChestPosList[i] = pathAI.pathPointInfos[i].Position;
//		}
		GameObject.Destroy(go);
		
		// Must after the _mChestPosList
		CopyData copytData = Globals.Instance.MGameDataManager.MCurrentCopyData;
	//if (0 == copytData.MCopyChestData.ChestIDList.Count)
	//{
	//	Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().UpdateCopyChestShow(_mChestList.Count);
	//	return;
	//}

		foreach (int id in copytData.MCopyChestData.ChestIDList)
		{
			CreateRandomChest(id);
		}
		
	}
	
	protected virtual GameObject CreateRandomChest(int chestID)
	{
		GameObject go = GraphicsTools.ManualCreateObject("TempArtist/Prefab/Build/m_other_box", "Chest" + chestID.ToString(), true);
		ChestTrigger trigger = go.AddComponent<ChestTrigger>() as ChestTrigger;
		trigger.ChestID = chestID;
		
		int index = Random.Range(0, _mChestPosList.Length);
		go.transform.position = _mChestPosList[index];
		_mChestList.Add(chestID, go);
//		Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().UpdateCopyChestShow(_mChestList.Count);
		return go;
	}
	
	public void DestroyRandomChest(int chestID)
	{
		GameObject go = null;
		if (_mChestList.TryGetValue(chestID, out go))
		{
			GraphicsTools.ManualDestroyObject(go, false);
			_mChestList.Remove(chestID);
		}
//		Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>().UpdateCopyChestShow(_mChestList.Count);
	}
	
	private void ClearRandomChests()
	{
		foreach (GameObject go in _mChestList.Values)
		{
			GraphicsTools.ManualDestroyObject(go, false);
		}
		_mChestList.Clear();
	}
	
	public void InitMissionArea()
	{
		GameObject go = GameObject.Find("MissionArea");
		if(go)
		{
			//Vector3 center = go.GetComponent<BoxCollider>().center;
			Vector3 size = go.GetComponent<BoxCollider>().size;
			crossArea.Set(go.transform.position.x - size.x/2,go.transform.position.z - size.z/2,size.x,size.z);
		}
		
		_mOceanRange = GameObject.Find("m_OceanRange");
	}
	
	protected void RegisterSubscriber()
	{
		_mLeaveBattle = EventManager.Subscribe(BattleStatusPublisher.NAME + ":" + BattleStatusPublisher.EVENT_LEAVE_BATTLE);
		_mLeaveBattle.Handler = delegate (object[] args)
		{
			GameData.BattleGameData.BattleResult result = (GameData.BattleGameData.BattleResult)args[0];
			// OnBattleEnd(result);
		};
		
		// When a new formation is applied, then change the new Model
		_mFormationApplySub = EventManager.Subscribe(NetReceiverPublisher.NAME + ":" + NetReceiverPublisher.EVENT_FORMATION_APPLY);
		_mFormationApplySub.Handler = delegate (object[] args)
		{
			int error = (int)args[0];
			if (error != (int)PacketErrorCode.SUCCESS)
				return;
			
			if (GameStatusManager.Instance.MGameState == GameState.GAME_STATE_COPY)
			{
//				ReplaceActorModel();
			}
		};
	}
	
	protected void UnRegisterSubscriber()
	{
		if (null != _mEnterBattle)
		{
			_mEnterBattle.Unsubscribe();
		}
		_mEnterBattle = null;
			
		if (null != _mLeaveBattle)
		{
			_mLeaveBattle.Unsubscribe();
		}
		_mLeaveBattle = null;
		
		if (null != _mFormationApplySub)
		{
			_mFormationApplySub.Unsubscribe();
		}
		_mFormationApplySub = null;
	}
	
	private void UpdateGuideArrows()
	{
		if (0 == _mGuideArrowList.Count)
			return;
		
		// Define the Screen Rect
		Rect screenRect = new Rect();
		
		float w = Screen.width * 0.7f;
		float h = Screen.height * 0.7f;
		
		// Adjust the startPoint(left-bottom)
		screenRect.xMin = (Screen.width - w) * 0.5f;
		screenRect.yMin = (Screen.height - h) * 0.5f;
		
		screenRect.width = w;
		screenRect.height = h;
		
		Vector3 intersectPoint = Vector3.zero;
		
		foreach (MonsterFleet key in _mGuideArrowList.Keys)
		{
			GameObject go = _mGuideArrowList[key];
			
			// tzz added 
			// to fix http://pms.mappn.com/index.php?m=bug&f=view&bugID=5726
			Vector3 t_monsterFleetCamDir	= key.Position - Globals.Instance.MSceneManager.mMainCamera.transform.position;
			float	t_dot 					= Vector3.Dot(t_monsterFleetCamDir.normalized,Globals.Instance.MSceneManager.mMainCamera.transform.forward.normalized);
			
			Vector3 screenPos2;
			
			if(t_dot <= 0.001){
				// tzz modified
				// the monster fleet is back of camera
				//
				Vector3 t_cross			= Vector3.Cross(Globals.Instance.MSceneManager.mMainCamera.transform.forward,t_monsterFleetCamDir).normalized;
				Quaternion t_rotation	= Quaternion.AngleAxis(-(Mathf.Abs(t_dot) + 0.5f) * Mathf.Rad2Deg,t_cross);
				t_monsterFleetCamDir	= t_rotation * t_monsterFleetCamDir + Globals.Instance.MSceneManager.mMainCamera.transform.position;
				
				screenPos2 				= Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(t_monsterFleetCamDir);
			}else{
				screenPos2				= Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(key.Position);
			}
			
			Vector3 screenPos1 = Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(_mActorFleet.Position);
						
			if (HelpUtil.Intersect(screenRect, screenPos1, screenPos2, out intersectPoint))
			{
				// tzz add for set visible of indicator
				go.SetActiveRecursively(true);
				
				// Convert to GUI Space
				intersectPoint = GUIManager.ScreenToGUIPoint(intersectPoint);
				intersectPoint.z = GUIManager.GUI_FARTHEST_Z;
				
				// if (Vector3.Dot(Globals.Instance.MSceneManager.mMainCamera.transform.forward, key.Position - Globals.Instance.MSceneManager.mMainCamera.transform.position) < 0.0f)
				// {
				// 	intersectPoint = GUIManager.GUIToScreenPoint(intersectPoint);
				// 	
				// 	intersectPoint.x = Screen.width - intersectPoint.x;
				// 	intersectPoint.y = Screen.height - intersectPoint.y;
				// }
				
				// Convert to GUI Space
				screenPos1 = GUIManager.ScreenToGUIPoint(screenPos1);
				screenPos1.z = GUIManager.GUI_FARTHEST_Z;
				
				// Calculate the direction
				Vector3 dir = intersectPoint - screenPos1;
				Quaternion rotate = Quaternion.FromToRotation(Vector3.right, dir);
				go.transform.localRotation = rotate;
				
				// Calculate the offset position according to he PackedSprite's size
				Vector3 position = intersectPoint;
				PackedSprite sprite = go.GetComponent<PackedSprite>() as PackedSprite;
				if (dir.x < 0)
				{
					if (null != sprite)
						position.x += sprite.width * 0.5f;
					else
						position.x += 32.0f;
				}
				else
				{
					if (null != sprite)
						position.x -= sprite.width * 0.5f;
					else
						position.x -= 32.0f;
				}
				
				if (dir.y < 0)
				{
					if (null != sprite)
						position.y += sprite.height * 0.5f;
					else
						position.y += 32.0f;
				}
				else
				{
					if (null != sprite)
						position.y -= sprite.height * 0.5f;
					else
						position.y -= 32.0f;
				}
				
				// set the z position above GUIMain
				position.z		= GUIManager.GUI_NEAREST_Z + 200;
				
				// Auto scale
				position.x *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.x;
				position.y *= Globals.Instance.M3DItemManager.EZ3DItemParentScaleInv.y;
				go.transform.localPosition = position;
				
			}else{
				
				// tzz add for set visible of indicator
				go.SetActiveRecursively(false);
			}
		}
	}
	
	private void UpdateIndicator()
	{
		if (null != _mActorFleet)
		{
			if(_IndicatorEffect != null)
				_IndicatorEffect.SetActiveRecursively(_mActorFleet.IsMoving);
		}
	}
	
	private void UpdateCamera()
	{
		if (_mActorFleet == null || _mActorFleet._mWarshipList.Count == 0){
			return;
		}
		
		WarshipL warship = _mActorFleet._mWarshipList[0];		
		
		Vector3 targetPosition = warship.U3DGameObject.transform.position;
		Quaternion targetRotation = warship.U3DGameObject.transform.rotation;
		
		float zDistance = 300.0f;
		float height = 288.0f;
		float moveDamping = 10.0f;
		float rotationDamping = 1.0f;
			
		float xDistance = 0.0f;
		
		// Calculate the current rotation angles
		float wantedRotationAngle = warship.U3DGameObject.transform.rotation.eulerAngles.y;
		float wantedHeight = targetPosition.y + height;
			
		float currentRotationAngle = Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles.y;
		float currentHeight = Globals.Instance.MSceneManager.mMainCamera.transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, moveDamping * Time.deltaTime);
	
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		float wantedX = (targetPosition + targetRotation * Vector3.right).x + xDistance;
		float wantedZ = (targetPosition + targetRotation * Vector3.forward).z - zDistance;
	
		Globals.Instance.MSceneManager.mMainCamera.transform.position = new Vector3 (wantedX, currentHeight, wantedZ);
		Globals.Instance.MSceneManager.mMainCamera.transform.LookAt (new Vector3(wantedX, targetPosition.y, targetPosition.z) );
		
		_mCurrentCamPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		_mCurrentCamForward = Globals.Instance.MSceneManager.mMainCamera.transform.forward;
	}
	
	protected CopyStatusPublisher _mPublisher;
	
	public CopyData MCurrentCopyData = null;
	public CopyDropData MCopyDropData = null;
	
	protected FleetL _mActorFleet;
	protected MonsterFleet _mFightingMonsterFleet = null;
	protected List<MonsterFleet> _mMonsterFleetList = new List<MonsterFleet>();
	
	// Use GUI Control
	private Dictionary<MonsterFleet, GameObject> _mGuideArrowList = new Dictionary<MonsterFleet, GameObject>();
	
	protected Dictionary<int, GameObject> _mChestList = new Dictionary<int, GameObject>();
	protected Vector3[] _mChestPosList;
	
	protected GameObject _mOceanRange;
	
	public bool _mIsEnabled = true;
	public bool _mIsFinalBattle = false;
	protected bool _mIsFingerEventActive = false;
	protected bool _mIsPermitRequestBattle = false;
	
	public Vector3 mLeaveTrigger = Vector3.zero;
	
	private Rect crossArea = new Rect(0,100,1000,100);
	protected GameObject _IndicatorEffect = null;
	// private GameObject _mRoute = null;
	
	private Vector3 _mCurrentCamPos;
	private Vector3 _mCurrentCamForward;
	
	private ISubscriber _mEnterBattle = null;
	private ISubscriber _mLeaveBattle = null;
	private ISubscriber _mFormationApplySub = null;
	
	// by lsj
	public bool isRequestEnterBattle = false;
	
}
