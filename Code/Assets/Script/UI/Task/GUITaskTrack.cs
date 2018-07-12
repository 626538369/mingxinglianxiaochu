using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUITaskTrack : GUIWindow
{
	Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		
		this.gameObject.transform.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		//this.gameObject.transform.localPosition = new Vector3(350,50, 0);
		this.gameObject.transform.localPosition = new Vector3(0,0, 0);
		this.GUILevel = 25;
		//_mPanelManager = gameObject.GetComponent( typeof(UIPanelManager) ) as UIPanelManager;
		
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		mTouchPosXMin = mTouchPosXMin*Globals.Instance.MGUIManager.widthRatio;
		mTouchPosXMax = mTouchPosXMax*Globals.Instance.MGUIManager.widthRatio;
		mTouchPosYMin = mTouchPosYMin*Globals.Instance.MGUIManager.heightRatio;
		mTouchPosYMax = mTouchPosYMax*Globals.Instance.MGUIManager.heightRatio;
		
		
		//if(mButtonObj != null)
		//{
        // UIEventListener.Get(mButtonObj).onClick += OnHuaDongBtn;
		//}
	}
	
	bool mHuaDongSign = false;
	bool mIsHuaDongIng = false;
	void OnHuaDongBtn(GameObject obj)
	{
		if(!mIsHuaDongIng)
		{
			if(mHuaDongSign)
			{
				AnimatePosition.Do(mParentObj, EZAnimation.ANIM_MODE.To, new Vector3(386, mParentObj.transform.localPosition.y, mParentObj.transform.localPosition.z), EZAnimation.quinticIn, 0.6f, 0, StartDelegate, EndDelegate);
			}
			else
			{
				AnimatePosition.Do(mParentObj, EZAnimation.ANIM_MODE.To, new Vector3(576, mParentObj.transform.localPosition.y, mParentObj.transform.localPosition.z), EZAnimation.quarticOut, 0.6f, 0, StartDelegate, EndDelegate);
			}
			mHuaDongSign = !mHuaDongSign;
			mIsHuaDongIng = true;
		}
	}
	
	protected void StartDelegate(EZAnimation anim)
	{
	}
	
	protected void EndDelegate(EZAnimation anim)
	{
		//ButtonArrow.transform.localScale = new Vector3(-ButtonArrow.transform.localScale.x,1,1);
		//mIsHuaDongIng = false;
	}
	
	public void UpdateData()
	{
		//this.gameObject.transform.localPosition = new Vector3(300f,0f, Globals.Instance.MTaskManager.mTrackViewZ);
		
		CheckBuildingTaskState();
		CreateTarckView();
		
	}
	
	void CheckBuildingTaskState()
	{

	}
	
	void CreateTarckView()
	{
		
	}
	
	void InitButton(UIButton button, Dictionary<int, string> mTaskDialogDic, int numstr, TALKSTATE state)
	{
		//SpriteText ShowText = button.transform.FindChild("Text").GetComponent( typeof(SpriteText) ) as SpriteText;
		//SpriteText ShowNum = button.transform.FindChild("Num").GetComponent( typeof(SpriteText) ) as SpriteText;
		//ShowNum.transform.localScale = Vector3.zero;
		UILabel ShowLable = button.transform.Find("UILableTaskName").GetComponent( typeof(UILabel) ) as UILabel;

			
		
		int taskid = (int)button.Data;
		
		if(taskid == -1)
		{
			//ShowText.SetAnchor(SpriteText.Anchor_Pos.Middle_Center);
			//ShowText.Text = "升至"+(Globals.Instance.MGameDataManager.MActorData.BasicData.Level + 1)+"级，您可扫荡战役提升等级";
			//ShowNum.Text = "";
			int requirLevel = Globals.Instance.MGameDataManager.MActorData.BasicData.Level + 1;
			ShowLable.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(22100016),requirLevel) ;
			return;
		}
		
		if(taskObjectDic.ContainsKey(taskid))
		{
			TaskConfig.TaskObject task = taskObjectDic[taskid];
			//ShowText.transform.localScale = Vector3.one;
			//ShowLable.text = mTaskDialogDic[task.Name];
		}
		
		return;
		
		
		PackedSprite TotalAvatar = button.transform.Find("AvatarIcon").GetComponent(typeof(PackedSprite)) as PackedSprite;
		PackedSprite  npcIcon = button.transform.Find("NPCIcon").GetComponent(typeof(PackedSprite)) as PackedSprite;
		
		if(taskObjectDic.ContainsKey(taskid))
		{
			TaskConfig.TaskObject task = taskObjectDic[taskid];
			
			//ShowText.Text = mTaskDialogDic[task.Name]+task.Task_ID+" T"+task.Task_Type;
			//ShowLable.seaColor = SeaClientColorType.FloralWhite247246220;
			string curTalkIndex = "";
//			if(state == TALKSTATE.MIDDLE)
//				curTalkIndex = task.Middle_Task_Warn; ///现在的作用是头像了
//			else if(state == TALKSTATE.ACCOMPLISH)
//				curTalkIndex = task.Accomplish_Task_Warn;
//			else if(state == TALKSTATE.COMPLETE)
//				curTalkIndex = task.Complete_Task_Warn;
//			else
//			{
//				curTalkIndex = task.Before_Task_Warn;
//				//ShowLable.seaColor = SeaClientColorType.LimeGreen089210000;
//			}
//			
			//ShowText.transform.localScale = Vector3.one;
//			ShowLable.text = mTaskDialogDic[task.Name];
			
			//if(mTaskDialogDic.ContainsKey(curTalkIndex))
			//{
			//	//ShowText.Text = mTaskDialogDic[curTalkIndex]+" T"+task.Task_Type+state+"id"+curTalkIndex;
			//	
			//	if(GameDefines.ToastEnabled)
			//	{
			//		ShowText.Text = "[#u]" + mTaskDialogDic[curTalkIndex]+(task.Task_ID - 700000000);
			//	}
			//	else
			//	{
			//		ShowText.Text = "[#u]" + mTaskDialogDic[curTalkIndex];
			//	}
			//	ShowLable.Text = mTaskDialogDic[task.Name];
			//	ShowText.transform.localScale = Vector3.one;
			//}
//			
//			if (curTalkIndex == "role")
//				curTalkIndex = Globals.Instance.MGameDataManager.MActorData.BasicData.AvatarName;
//			
//			if (TotalAvatar.GetAnim(curTalkIndex) != null)
//			{
//				TotalAvatar.transform.localScale = Vector3.zero;
//				TotalAvatar.PlayAnim(curTalkIndex);
//				npcIcon.transform.localScale = Vector3.one;
//			}
//			if (npcIcon.GetAnim(curTalkIndex) != null)
//			{
//				npcIcon.transform.localScale = Vector3.zero;
//				npcIcon.PlayAnim(curTalkIndex);
//				TotalAvatar.transform.localScale = Vector3.one;
//			}
//			
//			int type = 0;
//			
//			if(task.Task_Type == (int)EType.TYPE_KILL && (state == TALKSTATE.MIDDLE || state == TALKSTATE.ACCOMPLISH))
//			{
//				//ShowNum.Text = (numstr+"/"+task.Kill_NPC_Num);
//				//ShowNum.transform.localScale = Vector3.one;
//			}
//			else
//			{
//				//ShowNum.Text = "";
//			}
		}
		else
		{
		
		}
		
		
	}
	
	public void MoveCameraToTaskPos(TaskConfig.TaskObject taskObject, bool isAcceptNpc)
	{
		
		
//if(Globals.Instance.MTaskManager.GoToTalk(holdBuildingList[key].Property.LogicID, delegate(){
//					if(holdBuildingList[key].Property.Type == EBuildingType.DEFENCE_FACILITY || holdBuildingList[key].Property.Type == EBuildingType.MARINE_BOARD)
//					{
//						Globals.Instance.MTeachManager.NewBuildingClickedEvent(holdBuildingList[key].U3DGameObject.name);
//					}
//				}))
//				{
					
//				}
	}
	
	private void TaskBefore(int taskid)
	{
		TaskConfig.TaskObject taskObject = taskObjectDic[taskid];
		
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus)
//		{
//			List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//			if(mMonsterFleetList.Count > 0)
//			{
//				Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//				
//				foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//				{
//					ws.MoveTo(despos);
//				}
//			}
//		}
//		else if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//		{
//			if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObject.Before_Task_SeaID)
//			{
//				//GUIWorldMap.AutoTransPort(taskObject.Before_Task_SeaID, delegate(int portId) {
//				//	Debug.Log("------------------Requst MoveCameraToTaskPos-----------------------portId = "+portId);
//				//	MoveCameraToTaskPos(taskObject, true);
//				//});
//			}
//			else
//			{
//				MoveCameraToTaskPos(taskObject, true);
//			}
//		}
	}
	
	private void TaskMidle(TaskData taskData)
	{
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus)
//		{
//			List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//			if(mMonsterFleetList.Count > 0)
//			{
//				Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//				
//				foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//				{
//					ws.MoveTo(despos);
//				}
//			}
//		}
//		else if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//		{
//			if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID)
//			{
//			//	GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID, delegate(int portId) {
//			//		if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//			//		{
//			//			Debug.Log("------------------Requst Enter Copy List-----------------------portId = "+portId);
//			//			int actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//			//			NetSender.Instance.RequestCopyList(actorID, portId);
//			//			GUIRadarScan.Show();
//			//			Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//			//		}
//			//	});
//			}
//			else
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestCopyList(actorID, portID);
//					Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//				}
//			}
//		}
//		
//		return;
//		
//		if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID)
//		{
//			//GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID, delegate(int portId) {
//			//	if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//			//	{
//			//		Debug.Log("------------------Requst Enter Copy List-----------------------portId = "+portId);
//			//		int actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//			//		NetSender.Instance.RequestCopyList(actorID, portId);
//			//		Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//			//	}
//			//});
//		}
//		else
//		{
//			if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestCopyList(actorID, portID);
//					Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//				}
//				
//			}
//			else
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type == (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestEnterPort(actorID, portID);
//				}
//				else
//				{
//					List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//					if(mMonsterFleetList.Count > 0)
//					{
//						Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//						
//						foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//						{
//							ws.MoveTo(despos);
//						}
//					}
//				}
//			}
//		}
	}
	
	private void TaskAccomplish(TaskData taskData)
	{
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus)
//		{
//			List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//			if(mMonsterFleetList.Count > 0)
//			{
//				Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//				
//				foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//				{
//					ws.MoveTo(despos);
//				}
//			}
//		}
//		else if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//		{
//			if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID)
//			{
//				//GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID, delegate(int portId) {
//				//	if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//				//	{
//				//		Debug.Log("------------------Requst Enter Copy List-----------------------portId = "+portId);
//				//		int actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//				//		NetSender.Instance.RequestCopyList(actorID, portId);
//				//		Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//				//	}
//				//});
//			}
//			else
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestCopyList(actorID, portID);
//					Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//				}
//			}
//		}
//		
//		return;
//		
//		if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID)
//		{
//			//GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Accomplish_Task_SeaID, delegate(int portId) {
//			//	if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//			//	{
//			//		Debug.Log("------------------Requst Enter Copy List-----------------------portId = "+portId);
//			//		int actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//			//		NetSender.Instance.RequestCopyList(actorID, portId);
//			//		Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//			//	}
//			//});
//		}
//		else
//		{
//			if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type != (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestCopyList(actorID, portID);
//					Globals.Instance.MTaskManager.SetCurTaskCopyId(taskObjectDic[taskData.Task_ID].TargetID);
//				}
//				
//			}
//			else
//			{
//				if(taskObjectDic[taskData.Task_ID].Task_Type == (int)EType.TYPE_TALK)
//				{
//					long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//					int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//					NetSender.Instance.RequestEnterPort(actorID, portID);
//				}
//				else
//				{
//					/*List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//					if(mMonsterFleetList.Count > 0)
//					{
//						Vector2 despos = Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position);
//						GameStatusManager.Instance.MCopyStatus.OnFingerDownEvent(0, despos);
//					}
//					*/
//					
//					List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//					if(mMonsterFleetList.Count > 0)
//					{
//						Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//						
//						foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//						{
//							ws.MoveTo(despos);
//						}
//					}
//				}
//			}
//		}
//		
	}
	
	private void TaskComplete(TaskData taskData)
	{
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus)
//		{
//			List<MonsterFleet> mMonsterFleetList = GameStatusManager.Instance.MCopyStatus.GetHoldMonsterList();
//			if(mMonsterFleetList.Count > 0)
//			{
//				Vector3 despos = mMonsterFleetList[0]._monsterWarshipList[0].U3DGameObject.transform.position;
//				
//				foreach (WarshipL ws in Globals.Instance.MPlayerManager.GetFleet(1000)._mWarshipList)
//				{
//					ws.MoveTo(despos);
//				}
//			}
//		}
//		else if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//		{
////			if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Complete_Task_SeaID)
////			{
////				//GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Complete_Task_SeaID, delegate(int portId) {
////				//	Debug.Log("------------------Requst MoveCameraToTaskPos-----------------------portId = "+portId);
////				//	MoveCameraToTaskPos(taskObjectDic[taskData.Task_ID], false);
////				//});
////			}
////			else
////			{
////				MoveCameraToTaskPos(taskObjectDic[taskData.Task_ID], false);
////			}
//		}
//		
//		return;
//		
//		if(Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID != taskObjectDic[taskData.Task_ID].Complete_Task_SeaID)
//		{
//			//GUIWorldMap.AutoTransPort(taskObjectDic[taskData.Task_ID].Complete_Task_SeaID, delegate(int portId) {
//			//	Debug.Log("------------------Requst MoveCameraToTaskPos-----------------------portId = "+portId);
//			//	MoveCameraToTaskPos(taskObjectDic[taskData.Task_ID], false);
//			//});
//		}
//		else
//		{
//			if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus)
//			{
//				MoveCameraToTaskPos(taskObjectDic[taskData.Task_ID], false);
//			}
//			else
//			{
//				long actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
//				int portID = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
//				NetSender.Instance.RequestEnterPort(actorID, portID);
//			}
//		}
	}
	
	public void DailyTrack(int taskid)
	{
		if(Globals.Instance.MTaskManager.mTaskDailyData.mCurDailyState == TaskDailyData.DAILYTASKSTATE.CANCOMPLETE)
		{
			//Globals.Instance.MGUIManager.CreateWindow<GUITaskDaily>(
			//delegate (GUITaskDaily gui)
			//{
			//	gui.UpdateData();
			//}
			//);
			return;
		}
		
		
		Debug.Log("DAILYTASKTYPE.LEARNCARD");


	}
	
	public void UnfinishTaskDoSameThing(int taskid)
	{
		TaskData taskData = null;
		for(int j = 0; j < Globals.Instance.MTaskManager._mUnfinishList.Count; j++)
		{
			if(taskid == Globals.Instance.MTaskManager._mUnfinishList[j].Task_ID)
			{
				taskData = Globals.Instance.MTaskManager._mUnfinishList[j];
			}
		}
		if(GameStatusManager.Instance.MGameState == GameState.GAME_STATE_PORT)
		{
			if(taskData != null)
			{
				if(taskData.IsTaskDaily)
				{
					DailyTrack(taskData.Task_ID);
				}
				else
				{
					if(taskData.State == TALKSTATE.MIDDLE)
					{
						TaskMidle(taskData);
					}
					else if(taskData.State == TALKSTATE.ACCOMPLISH)
					{
						TaskAccomplish(taskData);
					}
					else if(taskData.State == TALKSTATE.COMPLETE)
					{
						TaskComplete(taskData);
					}
				}
			}
			else
			{
				//int actorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
				//int portId = Globals.Instance.MGameDataManager.MCurrentPortData.PortID;
				//NetSender.Instance.RequestCopyList(actorID, portId);
			}
		}
		else if(GameStatusManager.Instance.MGameState == GameState.GAME_STATE_COPY)
		{
			
			Status2DCopy status2DCopy = (Status2DCopy)GameStatusManager.Instance.MCopyStatus;
			if (null != status2DCopy && null != status2DCopy.StageCopy)
			{
				List<GameObject> monsterList =  status2DCopy.StageCopy.GetMonsterList();
				if (monsterList.Count > 0)
				{
					GameObject oneMonster = monsterList[0];
					status2DCopy.StageCopy.moveToTargetPostion(oneMonster.transform.position);
				}
				else{
					status2DCopy.StageCopy.moveToTargetPostion(status2DCopy.StageCopy.WebwayGates.transform.position);
				}
			}
			
		
		}
	}
	
	private void OnPressedButtonToUnfinish(GameObject go)
	{
		UIButton button = (UIButton) go.transform.GetComponent<UIButton>();
		if(button == null) return;
					
		UnfinishTaskDoSameThing((int)button.Data);
		
		for(int i = 1; i < _mButtonList.Length; i++)
		{
			if(_mButtonList[i].gameObject.active)
			{
				TaskTrackButton taskTrack = _mButtonList[i].gameObject.transform.GetComponent( typeof(TaskTrackButton) ) as TaskTrackButton;
				if(taskTrack != null)
				{
					taskTrack.GoToUp();
				}
			}
		}
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");
	
	
	}
	
	
	private void OnPressedButtonToCanAccept(GameObject obj ){
		
		UIButton button = (UIButton) obj.transform.GetComponent<UIButton>();
		if(button == null) return;
		
		if (!Globals.Instance.MTeachManager.checkAcceptCondition())
		return ;

		//_mPanelManager.MoveBack();
		//MoveCameraToTaskPos((TaskConfig.TaskObject)button.Data, true);
		TaskBefore((int)button.Data);
		
		for(int i=1; i<_mButtonList.Length; i++)
		{
			if(_mButtonList[i].gameObject.active)
			{
				TaskTrackButton taskTrack = _mButtonList[i].gameObject.transform.GetComponent( typeof(TaskTrackButton) ) as TaskTrackButton;
				if(taskTrack != null)
				{
					taskTrack.GoToUp();
				}
			}
		}
		Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");

	}
	
	
	public GameObject mParentObj = null;
	//public UIButton mButtonObj = null;
	//public PackedSprite ButtonArrow = null;
	
	public UIButton[] _mButtonList;
	
	
	private bool _IsShowSignObj = true;
	
	public GameObject _mTaskStateObj = null;
	public Texture texture7 = null;
	public Texture texture1 = null;
	
	private float mTouchPosXMin = 750;
	private float mTouchPosXMax = 960;
	private float mTouchPosYMin = 490;
	private float mTouchPosYMax = 540;
	
	private float ur = 255.0f;
	private float ug = 240.0f;
	private float ub = 0.0f;
	private float cr = 105.0f;
	private float cg = 247.0f;
	private float cb = 0.0f;
	
	
	private string mCurTeachKey = "x24";
}
