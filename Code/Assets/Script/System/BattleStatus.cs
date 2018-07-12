using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleStatus : GameStatus, IFingerEventListener
{
	public static readonly float DELAY_TIME = 1.5f;
	
	public static Vector3 CamOrignalPos = Vector3.zero;
	Vector3 SceneOffset = Vector3.zero;
	
	public enum EBattleState
	{
		INIT,
		BEGIN,
		DO_STEP,
		DELAY,
		END,
	}
		
	// Public Access Value
	public GameData.BattleGameData.BattleResult MBattleResult
	{
		get { return _battleResult; }
		set { _battleResult = value; }
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="BattleStatus"/> class.
	/// </summary>
	public BattleStatus()
	{
		_mPublisher = new BattleStatusPublisher();
		
		m_battleFailedPrompt = new bool[BATTLE_FAILED_PROMPT_COPY_ID.Length];		
	}
	
	public override void Initialize()
	{
		GUIRadarScan.Hide();
		
		// tzz added
		// close the EXIT_COPY dialog to prevent from click ok to send exit copy scene
		//
		GUIDialog.Destroy();
		
	
	}
	
	protected virtual void InitializeImpl(){
		ResetValue();

		_mPublisher.NotifyEnterBattle();
		
		if(Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>() == null)
		{
			// GUI
			Globals.Instance.MGUIManager.CreateWindow<GUIBattle>(m_guiLoadedCallback);
		}
		else
		{
			Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>().UpdateData();
		}
	}
	
	/**
	 *	by lsj for the case of port defense battle 
	 */
	public virtual void InitBattleData()
	{
		GUIRadarScan.Hide();
		
		// !!!! follow 3 function order can't be changed !!!!
		InitCameraPos();
		BeginBattleLogic();
		PlayBattleCameraTrack();
			
		// tzz added for create the selected prefab effection
		//
		if(m_battleSelectedPrefab != null){
			GameObject.Destroy(m_battleSelectedPrefab);
		}
		
		Object t_obj = Resources.Load(BattleShipSelectedPrefab);
		m_battleSelectedPrefab  = GameObject.Instantiate(t_obj) as GameObject;
		m_battleSelectedPrefab.SetActiveRecursively(false);
		
		GameObject.DontDestroyOnLoad(m_battleSelectedPrefab);
		
		// start create
		// Add Finger event
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
		
		// Add sneak attack effect
		if (_battleResult.SneakAttackType != GameData.BattleGameData.SneakAttackType.FACE_ATTACK)
		{
			Globals.Instance.M3DItemManager.PlaySneakEffect(_battleResult, null);
		}
		
		if(CustomCameraState){
			Globals.Instance.MCamTrackController.StopTrack(FightCameraTrack);
			
			KeyFrameInfo[] t_cameraInfo = m_battleCameraTrack.keyFrameInfos;
			KeyFrameInfo t_endPos = t_cameraInfo[t_cameraInfo.Length - 1];
			
			CameraTrack.ITweenMoveTo(Globals.Instance.MSceneManager.mMainCamera.gameObject,t_endPos,null,1.0f);			
		}
		

		_mBattleState = EBattleState.DO_STEP;
	}
	
	void InitCameraPos()
	{
		CamOrignalPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		
		switch (_battleResult.BattleType)
		{
			case GameData.BattleGameData.BattleType.COPY_BATTLE:
			{
				SceneOffset = new Vector3(0.0f, 0.0f, 0.0f);
				break;
			}
			case GameData.BattleGameData.BattleType.ARENA_BATTLE:
			case GameData.BattleGameData.BattleType.PORT_VIE_BATTLE:
			case GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE:
			case GameData.BattleGameData.BattleType.TASK_BATTLE:
			{
				SceneOffset = new Vector3(1500.0f, 0.0f, 400.0f);
				// Globals.Instance.MSceneManager.mMainCamera.transform.rotation.eulerAngles = Vector3.zero;
				break;
			}
		}
		
		Vector3 tCameraPos = Globals.Instance.MSceneManager.mMainCamera.transform.position;
		
		// Find the BattleCamStartPos Object
		GameObject go = GameObject.Find("BattleCamStartPos");
		
		if (null != go){
			Globals.Instance.MSceneManager.mMainCamera.transform.position = go.transform.position;
			Globals.Instance.MSceneManager.mMainCamera.transform.rotation = go.transform.rotation;
		}else{
			Globals.Instance.MSceneManager.mMainCamera.transform.position = tCameraPos + SceneOffset;
		}		
	}
	
	private void PlayBattleCameraTrack(){
		
		// Pick a camera track file
		int type = CalcBattleViewOfField();
		if (type == 0){
			FightCameraTrack = "PathPoints/CameraBattleTrackSma";
		}
		else if (type == 1){
			FightCameraTrack = "PathPoints/CameraBattleTrackMid";
		}
		else if (type == 2){
			FightCameraTrack = "PathPoints/CameraBattleTrackBig";
		}
		
		m_battleCameraTrack = Globals.Instance.MCamTrackController.StartTrack(FightCameraTrack, Globals.Instance.MSceneManager.mMainCamera.transform, true);
		

	}
	
	public override void Release()
	{
		ResetValue();
		
		BattleGeneralCmd.DestroyAllBattleGeneralCmd();
		
		// Destroy all fleets and warships
		foreach (FleetL fleet in _currentFleets.Values)
		{
			Globals.Instance.MPlayerManager.RemoveFleet(fleet);
		}
		_currentFleets.Clear();
		
		this.SetFingerEventActive(false);
		Globals.Instance.MFingerEvent.Remove3DEventListener(this);
		
		// Clean up some module, if we don't use the change scene
		Globals.Instance.MSkillManager.Cleanup();
		Globals.Instance.MCamTrackController.StopTrack(FightCameraTrack);
		
		// tzz added for hide the battle ship selected prefab special effect
		if(m_battleSelectedPrefab != null){
						
			GameObject.Destroy(m_battleSelectedPrefab);
			m_battleSelectedPrefab = null;
		}	
	}
	
	protected void ResetValue()
	{
		isFirst = true;
		battleStepFlag = 0;
		battleStepDuration = GameDefines.BATTLE_STEP_TIME;
		
		_isBattleEnd = false;
		_isChangeStep = true;
		_isPauseBattle = false;
		
		_mSelectShip = null;
		_mBattleState = EBattleState.INIT;
		
		_mDelayTime = 0.0f;
		GameStatusManager.Instance._pCopyStatus.isRequestEnterBattle = false;
	}
	
	/**
	 * tzz added
	 * get the current state 
	 */ 
	public EBattleState GetLogicState(){
		return _mBattleState;
	}
	
	public override void Pause()
	{
		_isPauseBattle = true;
	}
	
	public override void Resume()
	{
		_isPauseBattle = false;
	}
	
	bool isFirst = true;
	public override void Update()
	{
		if(_isPauseBattle)
			return;
		
		if (_isBattleEnd)
			return;
		
		UpdateBattleLogic();
		
		// tzz added 
		// update the position of ship selected special effect 
		if(m_battleSelectedPrefab != null 
		&& m_battleSelectedPrefab.active){
			
			if(_mSelectShip != null && _mSelectShip.U3DGameObject != null){
			
				Vector3 t_pos = _mSelectShip.U3DGameObject.transform.localPosition;
				t_pos.y += 3.0f;
				m_battleSelectedPrefab.transform.localPosition = t_pos;
				
			}else{
				m_battleSelectedPrefab.SetActiveRecursively(false);
			}
		}
		
		float stepRatio = (float)battleStepFlag / _battleResult.BattleSteps.Count;
		if(isFirst &&  stepRatio >= GameDefines.QUICK_BTN_ENABLED_RATIO)
		{
			isFirst = false;
			if(Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>() != null)
			{
				Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>().QuickBattleBtnVisibleDelay();
			}
		}
		//Debug.Log("当前步数  " + battleStepFlag + "/" + _battleResult.BattleSteps.Count + "  : " + stepRatio);
	}
	
	// 2012.04.05 LiHaojie Begin the battle logic
	public virtual void BeginBattleLogic()
	{
		// Recalculate the formation bounds
		actorFormations.Clear();
		enemyFormations.Clear();
		
		FleetL fleet = null;
		WarshipL warship = null;
		
		foreach (GameData.BattleGameData.Fleet fleetData in _battleResult.Fleets) 
		{
			List<GameData.BattleGameData.Ship> shipDatas = fleetData.Ships;
			
			fleet = Globals.Instance.MPlayerManager.CreateFleet(fleetData.FleetID);
			fleet._isActorFleet = !fleetData.NPCFleet;
			fleet._isAttachFleet = fleetData.IsAttacker;
			
			PlayerData data = null;
			if(_battleResult.BattleType == GameData.BattleGameData.BattleType.PORT_VIE_BATTLE &&
				Globals.Instance.MPortVieManager.puFlagReverseFleetPosition)
			{
				if(fleet._isAttachFleet)
				{
					data = Globals.Instance.MGameDataManager.MEnemyData;
				}
				else
				{
					data = Globals.Instance.MGameDataManager.MActorData;
				}
			}
			else
			{
				if(fleet._isAttachFleet)
				{
					data = Globals.Instance.MGameDataManager.MActorData;
				}
				else
				{
					data = Globals.Instance.MGameDataManager.MEnemyData;
				}
			}
			
			foreach (GameData.BattleGameData.Ship shipData in shipDatas) 
			{
				GirlData wsData = data.GetGirlData(shipData.ShipID);
			
				//tzz added 
				wsData.WarshipGeneralLogicID = shipData.GeneralLogicID;
				wsData.WarshipGeneralAvatar	= shipData.GeneralAvatar;
				
				fleet.CreateWarship(wsData, delegate(WarshipL ws) 
				{
					warship = ws;
					warship.setBloodDisplay(true);
					warship._warshipLogicID = shipData.LogicShipID;
//					warship.U3DGameObject.name = wsData.BasicData.Name;
					warship.GameObjectTag = TagMaskDefine.GFAN_FIGHT_WARSHIP;
					
					// Fleet data
					warship.Property.WarshipFleetID		= fleet._fleetID;
					warship.Property.WarshipIsAttacker	= fleet._isAttachFleet;
					warship.Property.WarshipIsNpc		= shipData.IsNpc;
					
					// // A specifical value, use to simulate server running
					warship._numBeAttacked = 0;
					
					Vector3 position = HelpUtil.GetWarFiledGridPosition (shipData.Position);
					position += SceneOffset;
					warship.ForceMoveTo(position);
					
					// Rotate 180 around y axis
					if (!fleet._isAttachFleet)
					{
						warship.U3DGameObject.transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
					}
				});
				
				if (fleet._isAttachFleet)
					AddFormationCell(shipData.Position, actorFormations);
				else
					AddFormationCell(shipData.Position, enemyFormations);
			}
			
			// If is Npc fleet
			if (fleetData.NPCFleet == true) 
			{
				// The enemy fleet information
				_enemyFleet = fleet;
			} 
			else 
			{
				// The actor information
				_actorFleet = fleet;
			}
			
			_currentFleets.Add(fleet._fleetID, fleet);
		}
		
		//tzz added for store the fov in order restore when Battle end
		//
		CustomCameraFormerFov = Globals.Instance.MSceneManager.mMainCamera.fov;
		Globals.Instance.MSceneManager.mMainCamera.fov = 60;
		
	}
	
	public virtual void UpdateBattleLogic()
	{
		UnityEngine.Profiling.Profiler.BeginSample("BattleStatus.UpdateBattleLogic");
		
		switch (_mBattleState)
		{
			case EBattleState.DO_STEP:
			{
				// Per step execute GameDefines.BATTLE_STEP_TIME time
				if (_isChangeStep)
				{
					// Per battle step
					GameData.BattleGameData.BattleStep oneStepData = _battleResult.BattleSteps[battleStepFlag];
					OneBattleStep(battleStepFlag, oneStepData);
						
					_isChangeStep = false;
					
					battleStepFlag++;
					battleStepDuration = GameDefines.BATTLE_STEP_TIME;
				}
				
				battleStepDuration -= Time.deltaTime;
				if (battleStepDuration <= 0.0f)
				{
					if (battleStepFlag >= _battleResult.BattleSteps.Count)
					{
						_mBattleState = EBattleState.DELAY;
						UnityEngine.Profiling.Profiler.EndSample();
						return;
					}
					else
					{
						// Execute one battle step settlement
						OnOneBattleStepEnd();
						
						// Terminate current step, and prepare the next step
						_isChangeStep = true;
					}
				}
				
				_mPublisher.NotifyOnStep();
				
				break;
			}
			case EBattleState.DELAY:
			{
				_mDelayTime += Time.deltaTime;
				if (_mDelayTime > DELAY_TIME)
				{
					_mBattleState = EBattleState.END;
				}
				
				_mPublisher.NotifyBattleDelay();
				break;
			}
			case EBattleState.END:
			{
				_isBattleEnd = true;
				EndBattleLogic();
				break;
			}
			
		}
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
	// The end battle logic
	public virtual void EndBattleLogic()
	{
		Release();
		
		bool tCloseGUIBattle = true; 
		string sceneName = string.Empty;
		switch (_battleResult.BattleType)
		{
		case GameData.BattleGameData.BattleType.COPY_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_COPY);
			break;
		}
		case GameData.BattleGameData.BattleType.ARENA_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_PORT);
			Globals.Instance.MSceneManager.mMainCamera.transform.position = CamOrignalPos;
			break;
		}
		case GameData.BattleGameData.BattleType.PORT_VIE_BATTLE:
		{
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_PORT);
			Globals.Instance.MSceneManager.mMainCamera.transform.position = CamOrignalPos;
			
			Globals.Instance.MPortVieManager.sIsVieBattling = false;
			//GUIPortVie guiVie = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortVie>();
			//if(Globals.Instance.MPortVieManager.sIsShowVieRewardView)
			//{
			//	Globals.Instance.MPortVieManager.sIsShowVieRewardView = false;
			//	Globals.Instance.MPortVieManager.CreateGUIPortVieReward();
			//	guiVie.Close();
			//}
			//else
			//{
			//	guiVie.SetVisible(true);
			//	//test whether to continue the next battle
			//	guiVie.BattleAnimationEnd();
			//}
			break;
		}
		case GameData.BattleGameData.BattleType.PROT_DEFENSE_BATTLE:
		{
			if(Globals.Instance.MPortDefenseManager.puIsShowWaveResult)
			{
				GUIBattle t = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
				if (null != t)
					t.UpdateWaveResultDialogData();
			}
			else
			{
				Globals.Instance.MPortDefenseManager.UpdateDefenseBattle();
			}
			break;
		}
		case GameData.BattleGameData.BattleType.TASK_BATTLE:
		{
			Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
			{
				gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
				gui.SetDialogType(EDialogType.TASK_BATTLE, null);
			},EDialogStyle.DialogOk
			);
			break;
		}
		}
		
		GUIBattle guiBattle = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
		if (null != guiBattle && tCloseGUIBattle)
			guiBattle.Close();
		
		_mPublisher.NotifyLeaveBattle(this.MBattleResult);
		
		//tzz added for reset the camera fov if camera is player custom state
		//
		Globals.Instance.MSceneManager.mMainCamera.fov = CustomCameraFormerFov;
		
		// tzz added for stopping the shake screen
		//
		iTween.Stop(Globals.Instance.MSceneManager.mMainCamera.gameObject,"shake");
		
		// tzz added for battle failed process
		// http://pms.mappn.com/index.php?m=bug&f=view&bugID=5654
		//
		//if(_battleResult.BattleWinResult == GameData.BattleGameData.EBattleWinResult.MONSTER_WIN){
			BattleFailedProcess();
		//}
	}
	
	//! tzz added for battle fialed
	public virtual void BattleFailedProcess(){
		int t_currCopyID = Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyMonsterData.CopyID;
		
		for(int i = 0;i < BATTLE_FAILED_PROMPT_COPY_ID.Length;i++){
			if(t_currCopyID == BATTLE_FAILED_PROMPT_COPY_ID[i]){
				
				//if(!m_battleFailedPrompt[i]){	// haven't prompted yet
				
				int index = i;
				
		
					
				//	m_battleFailedPrompt[i] = true;
				//}
				
				break;
			}
		}
	}
	
	public virtual bool OneBattleStep(int step, GameData.BattleGameData.BattleStep stepData)
	{
		UnityEngine.Profiling.Profiler.BeginSample("BattleStatus.OneBattleStep");
		
		// Has 3 parts
		/**Part1: 
		 * 1).Trim the basic data of all warships in this step
		 * 2).Calculate the count of a warship is attacked
		 * */
		
		/**Part2:
		 * 1).Simulate the all warships running logic
		 * 2).Display its
		 * */
		
		/**Part3: Implement in OnOneBattleStepEnd()
		 * 1).Trim all warships InitialLife for the next step simulate
		 * */
		// Debug.Log("The current stepID is : " + stepData.StepID.ToString());
	
		foreach(GameData.BattleGameData.BattleShip battleShip in stepData.BattleShips)
		{
			// // Search the Warship
			// WarshipL warship = Globals.Instance.MPlayerManager.GetWarship(battleShip.ShipID);
			// 
			// // Assign the base value
			// warship.Property.WarshipCurrentLife = battleShip.CurrentLife;
			// warship.Property.WarshipCurrentPower = (short)battleShip.CurrentPower;
			
			// Attack is the separate logic
			GameData.BattleGameData.AttackState attackState = battleShip.AttackState;
			if (attackState == GameData.BattleGameData.AttackState.IDLE)
				continue;
			
			// Construct a skill or a attackEvent? Include multiple be attacked target
			foreach (GameData.BattleGameData.AttackedTarget attackedTarget in battleShip.AttackedTargets) 
			{
				// Stat. the num of be attacked
				WarshipL targetWarship = Globals.Instance.MPlayerManager.GetWarship(attackedTarget.ShipID);
				if (null == targetWarship)
				{
					Debug.Log("[BattleStatus]: Why the target ship " + attackedTarget.ShipID + " is null?");
					continue;
				}
				
				targetWarship._numBeAttacked += 1;
			}
		}
		
		// Part2
		foreach(GameData.BattleGameData.BattleShip battleShip in stepData.BattleShips)
		{
			// Search the Warship
			WarshipL warship = Globals.Instance.MPlayerManager.GetWarship(battleShip.ShipID);
			if (null == warship)
			{
				Debug.Log("[BattleStatus]: Why the ship " + battleShip.ShipID + " is null?");
				continue;
			}
			
			// Assign the base value
			warship.Property.WarshipCurrentLife = battleShip.CurrentLife;
			warship.Property.WarshipCurrentPower = (short)battleShip.CurrentPower;
			
			// tzz added for modify some bug
			// 
			warship.GirlData.PropertyData.Life		= battleShip.CurrentLife;
			warship.GirlData.PropertyData.Power	= battleShip.CurrentPower;
			
			Vector3 currentPosition = warship.U3DGameObject.transform.position;

			Vector3 targetPosition = HelpUtil.GetWarFiledGridPosition(battleShip.CurrentPosition);
			targetPosition += SceneOffset;
			
			// Perform the step logci
			GameData.BattleGameData.MoveState moveState = battleShip.MoveState;
			switch (moveState)
			{
			case GameData.BattleGameData.MoveState.MOVE:
			{
				// move speed
				warship.MoveTo(targetPosition);
				
				// tzz added
				// for teach first move in Teach first enter scene
				// check TeachFirstEnterGame.cs for detail
				//if(m_teachStartMovingEvent != null){
				//	m_teachStartMovingEvent(TeachBattleEvent.e_startMoving);
				//	m_teachStartMovingEvent = null;
				//	
				//	battleStepDuration = 0.0f;
				//	_mDelayTime = DELAY_TIME;
				//}
				break;
			}
			case GameData.BattleGameData.MoveState.STOP:
			{
				warship.ForceMoveTo(targetPosition);
				break;
			}
			case GameData.BattleGameData.MoveState.SINK:
			{
				//warship.ForceSink();
				break;
			}
			}
			
			// Attack is the separate logic
			GameData.BattleGameData.AttackState attackState = battleShip.AttackState;
			if (attackState == GameData.BattleGameData.AttackState.IDLE)
				continue;			
			
			// tzz added for setting buffer of fire
			if(warship.WarshipHeader != null && battleShip.BattleBuffersList != null){
				warship.WarshipHeader.SetBufferStepInterval(battleShip.BattleBuffersList,step);
			}
						
			// Only perform the skill effect, construct the skill data now
			SkillDataSlot skillData = new SkillDataSlot(battleShip.SkillLogicID,warship._warshipID,(int)attackState);
			
			// Construct a skill or a attackEvent? Include multiple be attacked target
			int targetIndex = 0;
			foreach (GameData.BattleGameData.AttackedTarget attackedTarget in battleShip.AttackedTargets) 
			{
				WarshipL targetWarship = Globals.Instance.MPlayerManager.GetWarship(attackedTarget.ShipID);
				if (null == targetWarship)
				{
					Debug.Log("[BattleStatus]: Why the target ship " + attackedTarget.ShipID + " is null?");
					continue;
				}
				
				int targetMoveState;
				GetWarshipMoveStateInStep(attackedTarget.ShipID, stepData, out targetMoveState);

				SkillDataSlot.AttackTargetData attackTargetData = new SkillDataSlot.AttackTargetData();
				
				attackTargetData._targetID = attackedTarget.ShipID;
				attackTargetData._moveState = targetMoveState;
				attackTargetData._beAttackedState = (int)attackedTarget.AttackedState;
				attackTargetData._beAttackedDamage = attackedTarget.Damage;
				
				if (targetIndex == 0)
				{
					skillData._primaryTargetID = attackTargetData._targetID;
					attackTargetData._isPrimaryTarget = true;
				}else{
					attackTargetData._isPrimaryTarget = false;
				}
				
				skillData._attackTargetDataList.Add(attackedTarget.ShipID, attackTargetData);
								
				// tzz added for general command show
				if(warship.WarshipHeader != null && warship.WarshipHeader.Avatar != null
				&& !skillData.MSkillData.BasicData.SkillIsNormalAttack // is NOT normal skill
				&& targetIndex == 0 ){	// is first target
					BattleGeneralCmd.ShowGeneralCmd(warship.WarshipHeader.Avatar,
													Globals.Instance.MDataTableManager.GetWordText(skillData.MSkillData.BasicData.SkillWord));
				}
				
				targetIndex++;
			}
			
			warship.Attack(skillData);
		}
		
		UnityEngine.Profiling.Profiler.EndSample();
		return true;
	}
	
	public virtual void OnOneBattleStepEnd()
	{
		// // ReAssign the _warshipInitialLife, for the next step simulation
		// foreach (FleetL fleet in _currentFleets.Values)
		// {
		// 	foreach (WarshipL warship in fleet._mWarshipList)
		// 	{
		// 		// warship.Property.WarshipSimulateLife = warship.Property.WarshipCurrentLife;
		// 		// warship.Property.WarshipSimulatePower = warship.Property.WarshipCurrentPower;
		// 		// 
		// 		// warship.GirlData.PropertyData.Life = warship.Property.WarshipCurrentLife;
		// 		// warship.GirlData.PropertyData.Power = warship.Property.WarshipCurrentPower;
		// 		
		// 		warship._numBeAttacked = 0;
		// 		warship._numCurrentBeAttacked = 0;
		// 	}
		// }
		
		if (null == _mSelectShip)
			return;
		
		GUIBattle gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
		if (null != gui && gui.IsVisible)
		{
			gui.UpdateShipInfo(_mSelectShip);
		}
	}
	
	// #region CameraControl
	// public void UpdateCamera()
	// {
	// 	if (_currentFleets.Count < 2)
	// 		return;
	// 	
	// 	// Note: one cell is 100 X 50 units(metres)
	// 	// Calculate the distance from actor fleet position to npc enemy fleet
	// 	Vector3 actorFleetEndPosition = _actorFleet._theFleetEndPosition;
	// 	Vector3 enemyFleetEndPosition = _enemyFleet._theFleetEndPosition;
	// 	
	// 	// The Range of (battle starting to 16 cell)
	// 	float distanceOfFleets = Mathf.Abs(actorFleetEndPosition.x - enemyFleetEndPosition.x);
	// 	if (distanceOfFleets >= 15 * 100)
	// 	{
	// 		// Follow the target
	// 		// WarshipL warship = _actorFleet.GetWarship(11003);
	// 		Vector3 actorFocusPosition = _actorFleet._theFleetFocusPosition;
	// 	
	// 		float distance = 150.0f;
	// 		float height = 40.0f;
	// 		float moveDamping = 20.0f;
	// 		float rotationDamping = 1.5f;
	// 		
	// 		// Calculate the current rotation angles
	// 		float wantedRotationAngle = 0.0f;
	// 		float wantedHeight = actorFocusPosition.y + height;
	// 			
	// 		float currentRotationAngle = Globals.Instance.MSceneManager.mMainCamera.transform.eulerAngles.y;
	// 		float currentHeight = Globals.Instance.MSceneManager.mMainCamera.transform.position.y;
	// 		
	// 		// Damp the rotation around the y-axis
	// 		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	// 	
	// 		// Damp the height
	// 		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, moveDamping * Time.deltaTime);
	// 	
	// 		// Convert the angle into a rotation
	// 		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
	// 		
	// 		// Set the position of the camera on the x-z plane to:
	// 		// distance meters behind the target
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.position = actorFocusPosition;
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.position -= currentRotation * Vector3.forward * distance;
	// 	
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.position = new Vector3 (Globals.Instance.MSceneManager.mMainCamera.transform.position.x, currentHeight, Globals.Instance.MSceneManager.mMainCamera.transform.position.z);
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.LookAt (actorFocusPosition);
	// 	}
	// 	// The Range of (16 cell to 12 cell)
	// 	else if (distanceOfFleets < 15 * 100 && distanceOfFleets >= 11 * 100)
	// 	{
	// 		// Calculate the look at point
	// 		Rect actorRect3D = _actorFleet._fleetRect3D;
	// 		Rect enemyRect3D = _enemyFleet._fleetRect3D;
	// 		
	// 		_actorFleet._fleetRect3DCenterPosition.x += actorRect3D.width * 0.1f;
	// 		
	// 		// Adjust the camera position and fovY(Field of view), need adjust the ratio of screen resolution?
	// 		// Confirm the target position
	// 		float xDistance = 150;
	// 		float height = 150.0f;
	// 		float moveDamping = 0.01f;
	// 		
	// 		// float camFov = Globals.Instance.MSceneManager.mMainCamera.fov;
	// 		Globals.Instance.MSceneManager.mMainCamera.fov = 60;
	// 		// float camAspect = Globals.Instance.MSceneManager.mMainCamera.aspect;
	// 		
	// 		float tan = Mathf.Tan(78 * Mathf.Deg2Rad);
	// 		float zDistance = xDistance * tan; // Tangent 60degree
	// 		
	// 		Vector3 lookAtPosition = _actorFleet._fleetRect3DCenterPosition;
	// 		Vector3 currentPosition = Globals.Instance.MSceneManager.mMainCamera.transform.position;
	// 		
	// 		float currentHeight = currentPosition.y;
	// 		float wantedHeight = lookAtPosition.y + height;
	// 		// Damp the height
	// 		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, moveDamping * Time.deltaTime);
	// 		
	// 		float wantedX = Mathf.Lerp (currentPosition.x, lookAtPosition.x - xDistance, moveDamping * Time.deltaTime);
	// 		float wantedZ = Mathf.Lerp (currentPosition.z, lookAtPosition.z - zDistance, moveDamping * Time.deltaTime);
	// 		
	// 		Vector3 wantedPosition = new Vector3(wantedX, currentHeight, wantedZ);
	// 		
	// 		// Set the height of the camera
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.position = wantedPosition;
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.LookAt(lookAtPosition);	
	// 	}
	// 	// The Range of (less equal 10 cell)
	// 	else
	// 	{
	// 		// Calculate the look at point
	// 		Rect actorRect3D = _actorFleet._fleetRect3D;
	// 		Rect enemyRect3D = _enemyFleet._fleetRect3D;
	// 		
	// 		_actorFleet._fleetRect3DCenterPosition.x += actorRect3D.width * 0.3f;
	// 		
	// 		// float camFov = Globals.Instance.MSceneManager.mMainCamera.fov;
	// 		Globals.Instance.MSceneManager.mMainCamera.fov = 60;
	// 		// float camAspect = Globals.Instance.MSceneManager.mMainCamera.aspect;
	// 		
	// 		// Adjust the camera position and fovY(Field of view), need adjust the ratio of screen resolution?
	// 		// Confirm the target position
	// 		float xDistance = 50.0f;
	// 		float height = 40.0f;
	// 		float moveDamping = 0.4f;
	// 		
	// 		float tan = Mathf.Tan(78 * Mathf.Deg2Rad);
	// 		float zDistance = xDistance * tan; // Tangent 60degree
	// 		
	// 		Vector3 lookAtPosition = _actorFleet._fleetRect3DCenterPosition;
	// 		Vector3 currentPosition = Globals.Instance.MSceneManager.mMainCamera.transform.position;
	// 		
	// 		float currentHeight = currentPosition.y;
	// 		float wantedHeight = lookAtPosition.y + height;
	// 		// Damp the height
	// 		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, moveDamping * Time.deltaTime);
	// 		
	// 		float wantedX = Mathf.Lerp (currentPosition.x, lookAtPosition.x - xDistance, moveDamping * Time.deltaTime);
	// 		float wantedZ = Mathf.Lerp (currentPosition.z, lookAtPosition.z - zDistance, moveDamping * Time.deltaTime);
	// 		
	// 		Vector3 wantedPosition = new Vector3(wantedX, currentHeight, wantedZ);
	// 		
	// 		// Set the height of the camera
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.position = wantedPosition;
	// 		Globals.Instance.MSceneManager.mMainCamera.transform.LookAt(lookAtPosition);	
	// 	}
	// }
	// #endregion
	
	public void OnAttackStart(WarshipL warship, Skill skill)
	{	
	}
	
	public void OnAttackEnd(WarshipL warship, Skill skill)
	{
	}
	
	public virtual void OnBeAttacked(WarshipL warship, Skill skill)
	{
		// tzz added
		// for teach first fire in Teach first enter scene
		// check TeachFirstEnterGame.cs for detail
		if(m_teachFirstFireEvent != null){
			m_teachFirstFireEvent(TeachBattleEvent.e_firstFire);
			m_teachFirstFireEvent = null;
			
			battleStepDuration = 0.0f;
			_mDelayTime = DELAY_TIME;
		}
	}
	
	public void OnWarshipCreated(WarshipL warship)
	{
	}
	
	public virtual void OnWarshipDeath(WarshipL warship)
	{
		// List<WarshipL> tmpRemoveWarshipList = new List<WarshipL>();
		// foreach (FleetL fleet in _currentFleets.Values)
		// {
		// 	tmpRemoveWarshipList.Clear();
		// 	
		// 	foreach (WarshipL ws in fleet._mWarshipList)
		// 	{
		// 		if (ws == warship)
		// 		{
		// 			tmpRemoveWarshipList.Add(ws);
		// 			break;
		// 		}
		// 	}
		// 	
		// 	foreach (WarshipL ws in tmpRemoveWarshipList)
		// 	{
		// 		fleet._mWarshipList.Remove(ws._warshipID);
		// 	}
		// }
		
		if(m_teachOwnShipDownEvent != null ){
			m_teachOwnShipDownEvent(TeachBattleEvent.e_ownShipDown);
			m_teachOwnShipDownEvent = null;
			
			battleStepDuration = 0.0f;
			_mDelayTime = DELAY_TIME;
		}
	}
	
	#region Handle Finger Event
	private bool _mIsFingerEventActive = false;
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
		_mSelectShip = null;
		if(m_battleSelectedPrefab != null){
			m_battleSelectedPrefab.SetActiveRecursively(false);	
		}	
		
		GameObject pickObject = SceneQueryUtil.PickObject(fingerPos);
		if (null == pickObject)
		{
			// Clean up Object GUI
			GUIBattle gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if (null != gui && gui.IsVisible)
			{
				gui.SetShipInfoVisible(false);
			}
			return true;
		}
		
		if ( !pickObject.CompareTag(TagMaskDefine.GFAN_FIGHT_WARSHIP) )
		{
			// Clean up Object GUI
			GUIBattle gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if (null != gui && gui.IsVisible)
			{
				gui.SetShipInfoVisible(false);
			}
			return true;
		}
		
		WarshipL pickWarship = Globals.Instance.MPlayerManager.GetWarship(pickObject);
		if (null == pickWarship)
		{
			// Clean up Object GUI
			GUIBattle gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if (null != gui && gui.IsVisible)
			{
				gui.SetShipInfoVisible(false);
			}
			return true;
		}
		
		_mSelectShip = pickWarship;
		if(m_battleSelectedPrefab != null){
			m_battleSelectedPrefab.SetActiveRecursively(true);
		}		
		
		// Display Object GUI
		Globals.Instance.MGUIManager.CreateWindow<GUIBattle>(delegate(GUIBattle gui)
		{
			gui.UpdateData(pickWarship);
		}
		);

		return true;
	}
	
   	public bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		return true;
	}
	
	public bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		if(CustomCameraState){
			
			m_fingerMoveState		= true;
			m_fingerMoveBeginPos	= fingerPos;
			
		}else{
			
			m_fingerMoveState = false;
		}
		
		return true;	
	}
	
	public bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		if(m_fingerMoveState){
			
			float t_detal_x = m_fingerMoveBeginPos.x - fingerPos.x;
			float t_detal_y = m_fingerMoveBeginPos.y - fingerPos.y;
							
			Vector3 t_movePosition = Globals.Instance.MSceneManager.mMainCamera.transform.localPosition + new Vector3(t_detal_x,0,t_detal_y) * CustomCameraMoveScale;
						
			float t_min_x = GUIBattle.CameraTrackEndPos.x - 200;
			float t_max_x = GUIBattle.CameraTrackEndPos.x + 200;
			
			float t_min_z = GUIBattle.CameraTrackEndPos.z - 100;
			float t_max_z = GUIBattle.CameraTrackEndPos.z + 100;
			
			t_movePosition.x = Mathf.Clamp(t_movePosition.x,t_min_x,t_max_x);
			t_movePosition.z = Mathf.Clamp(t_movePosition.z,t_min_z,t_max_z);
			
			Globals.Instance.MSceneManager.mMainCamera.transform.localPosition = t_movePosition;
		}
		return true;	
	}
	
	public bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		m_fingerMoveState = false;
		return true;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		if(CustomCameraState){
			m_fingerPinchState = true;
		}else{			
			m_fingerPinchState = false;
		}
		
		return true;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		if(m_fingerPinchState){
			PortStatus.OnFingerPinchMove_imple(delta);
		}
		
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		m_fingerPinchState = false;
		return true;
	}
	
	public bool OnFingerStationaryBeginEvent (int fingerIndex, Vector2 fingerPos)
	{
		return true;
	}
	
	public bool OnFingerStationaryEndEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return true;
	}
	
	public bool OnFingerStationaryEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return true;
	}
	#endregion
	
	public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{
		return true;
	}
	public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		return true;
	}
	public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	
	private void GetWarshipMoveStateInStep(long warshipID, 
		GameData.BattleGameData.BattleStep stepData, out int moveState)
	{
		moveState = -1;
		foreach (GameData.BattleGameData.BattleShip battleShip in stepData.BattleShips) 
		{
			if (battleShip.ShipID == warshipID) 
			{
				moveState = (int)battleShip.MoveState;
				break;
			}
		} // End foreach
	}
	
	readonly int MoveStep = 2;
	private int CalcBattleViewOfField()
	{
		bool[] actorColFilled = new bool[4];
		bool[] enemyColFilled = new bool[4];
		for (int i = 0; i < 4 ; ++i)
		{
			actorColFilled[i] = false;
			enemyColFilled[i] = false;
		}
		
		int NumMaxRow = 8;
		for (int i = 3; i < 7; ++i)
		{
			bool isFilled = false;
			for (int j = 0; j < NumMaxRow; ++j)
			{
				isFilled = IsInEndFormation(i - MoveStep, j, actorFormations);
				if (isFilled) break;
			}
			
			actorColFilled[i - 3] = isFilled;
		}
		
		for (int i = 8; i < 12; ++i)
		{
			bool isFilled = false;
			for (int j = 0; j < NumMaxRow; ++j)
			{
				isFilled = IsInEndFormation(i + MoveStep, j, enemyFormations);
				if (isFilled) break;
			}
			enemyColFilled[i - 8] = isFilled;
		}
		
		if (actorColFilled[0] || enemyColFilled[3]
			|| actorColFilled[1] || enemyColFilled[2])
		{
			return 2; // Big camera track
		}
		else if (actorColFilled[2] || enemyColFilled[1])
		{
			return 1; // Middle
		}
		else if (actorColFilled[0] && enemyColFilled[0])
		{
			return 0; // Small
		}
		
		return 1;
	}
	
	void AddFormationCell(string grid, List<FormationCell> list)
	{
		int x, z;
		HelpUtil.CalculateGrid(grid, out x, out z);
		
		if (-1 == x || -1 == z)
			return;
			
		FormationCell cell;
		cell.x = x;
		cell.z = z;
		list.Add(cell);
	}
	
	bool IsInEndFormation(int x, int z, List<FormationCell> list)
	{
		if (null == list)
			return false;
		
		for (int i = 0; i < list.Count; ++i)
		{
			if (list[i].x== x && list[i].z == z)
				return true;
		}
		
		return false;
	}
	
	/**
	 * tzz added
	 * set the delegate for teach callback
	 * 
	 * @param _type			event type 
	 * @param _delegate		delegate event
	 */ 
	public virtual void SetTeachEventDelegateType(TeachBattleEvent _type,TeachEventDelegate _delegate){
		switch(_type){
		case TeachBattleEvent.e_startMoving:
			m_teachStartMovingEvent = _delegate;
			break;
		case TeachBattleEvent.e_firstFire:
			m_teachFirstFireEvent 	= _delegate;
			break;
		case TeachBattleEvent.e_ownShipDown:
			m_teachOwnShipDownEvent = _delegate;
			break;
		}
	}
	
	public virtual  void BeginBattleDoStep()
	{
		if(m_teachStartMovingEvent != null ){
		 	m_teachStartMovingEvent(TeachBattleEvent.e_startMoving);
			m_teachStartMovingEvent = null;
	 	
			_isPauseBattle = true;
		 }
	}
	
	//! the teach event delegate declare
	public delegate void TeachEventDelegate(TeachBattleEvent _event);
	
	//! teach battle event 
	public enum TeachBattleEvent{
		e_startMoving,
		e_firstFire,
		e_ownShipDown,
		e_battleEnd,
	};
	
	//! event delegate for teach process check TeachFirstEnterGame.cs for detail
	private event TeachEventDelegate m_teachStartMovingEvent;
	private event TeachEventDelegate m_teachFirstFireEvent;
	private event TeachEventDelegate m_teachOwnShipDownEvent;	
	
	//! battle gui loaded callback event
	[HideInInspector] public GUIManager.GUICallback<GUIBattle> m_guiLoadedCallback = null;
	
	//! the fight initialize camera track
	public static string FightCameraTrack		= "PathPoints/CameraBattleTrack";
	
	//! the resource path of battle ship selected
	private static readonly string BattleShipSelectedPrefab = "TempArtist/Prefab/Particle/S_Shipring";
	
	//! custom camera moving scale factor 
	private static readonly float	CustomCameraMoveScale = 0.05f;
	
	//! whether custom camera state
	public static bool CustomCameraState = false;
	
	//! the custom camera former fov
	public static float CustomCameraFormerFov	= 1.0f;
	
	//! the battle ship selected prefab special effect
	private GameObject	m_battleSelectedPrefab = null;
	
	//! finger move state
	private bool m_fingerMoveState = false;
	
	//! finger pinch state
	private bool m_fingerPinchState = false;
	
	//! finger move begin position
	private Vector2 m_fingerMoveBeginPos = Vector2.zero;
	
	//! battle camera track in order to get the end position of track
	[HideInInspector]public CameraTrack	m_battleCameraTrack = null;
	
	/// <summary>
	/// Gets the curr battle fight camera track.
	/// </summary>
	/// <returns>
	/// The curr battle fight camera track.
	/// </returns>
	public CameraTrack GetCurrBattleFightCameraTrack(){
		return m_battleCameraTrack;
	}
	
	
	//! tzz added for tips text id when battle failed http://pms.mappn.com/index.php?m=bug&f=view&bugID=5654
	protected static readonly int[]		BATTLE_FAILED_PROMPT_TEXT =
	{
		22600095,
		22600095,
		22600095,
		22600095,
	};
	
	//! tzz added for tips copy id when battle failed http://pms.mappn.com/index.php?m=bug&f=view&bugID=5654
	protected static readonly int[]		BATTLE_FAILED_PROMPT_COPY_ID = 
	{
		1701010020,
		1702010020,
		1702010070,
		1701010070,
	};
	
	//! has prompt text for battle failed
	private bool[]		m_battleFailedPrompt = null;
	
	
	protected BattleStatusPublisher _mPublisher;	
	
	public FleetL _actorFleet;
	public FleetL _enemyFleet;
	protected Dictionary<long, FleetL> _currentFleets = new Dictionary<long, FleetL> ();
	
	public bool _isBattleEnd = false;
	public bool _isChangeStep = true;
	public bool _isPauseBattle = false;
	
	protected EBattleState _mBattleState;
	
	//battle step
	protected int battleStepFlag = 0;
	protected float battleStepDuration;
	protected bool _mSupportDelay = true;
	protected float _mDelayTime = 0.0f;
	
	private FightCutscene	mFightCutscene = null;	
	
	// Battle result.
	protected GameData.BattleGameData.BattleResult _battleResult;
	
	// 
	struct FormationCell
	{
		public int x;
		public int z;
	}
	
	private WarshipL _mSelectShip;
	List<FormationCell> actorFormations = new List<FormationCell>();
	List<FormationCell> enemyFormations = new List<FormationCell>();
	
}
