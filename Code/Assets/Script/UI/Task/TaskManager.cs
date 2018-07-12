using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.Text;
using System.IO;
using System.Security.Cryptography;

/*struct TaskItem
{
	public int taskId=1;				//任务id
	public int2 killedNPCNum=2;			//已杀怪物数量
	public bool isPass=3;				//是否通过副本
	public bool isArrival=4;			//是否已到达地点
}
 */
public class TaskManager : MonoBehaviour
{
	public enum TaskTriggerEvent
	{
		STROLL  = 1,
		WORK = 2,
		STUDY = 3,
		GIFT = 4,
	};
	
	public enum TaskCategory
	{
		DAILY = 1, 	// 1-日常任务//
		MAIN = 2,		// 2-主线任务//
		BRANCH = 3,	// 3-支线任务//
		ARTIST = 4, 	// 4-艺术家任务//
		ACTIVITY = 5, // 5-活动任务//
		LABEL = 8 ,	// 选项卡任务//
		EXPLORE = 9, 	//探索任务//
		
		TRAINING = 11 , 	//属性变化任务//
		CHANGEDATE = 12, 	//日期变化任务//
		CLOTHESNUM = 13, 	//服装触发任务//
		
		UnknownType = 9999,
	};
	
	public enum TaskBranchCategory
	{
		BDATING = 1,
		BEXPLORE = 2,
	};

	public enum TaskRequireVisibleTypeEnum {
		SEXLIMIT = 1,		//(1,"任务性别需求"),//1-男 2-女//
		ITEMCOUNT = 2,		//(2,"道具达到"),//
		BEFORETASKID = 3,	//(3, "完成指定ID任务"),//num为0，则表示还没完成某任务//
		ROLEATTR = 4,		//(4,"玩家属性达到"),  //57-演技 40-魅力,1-金钱 2-钻石//
		HAVEARTIST = 5,	//(5, "艺术家品阶获得"),  //num表示品阶，表示获得某个品阶的艺术家//
		FANSNUM = 6,		//(6, "某地区粉丝数达到"),//type标识地区 //
	}

	public enum TaskState
	{
		RUNNING = 1,
		PASS = 2,
		UNPASS = 3,
	};



	public class BuildTaskInfo
	{
		public int BuildID;
		public int TaskID;
		public bool IsMainTask;
	}
	
	public Dictionary<int,BuildTaskInfo> BuildTaskInfoList = new Dictionary<int, BuildTaskInfo>();

	Promotion promotionConfig ;

	public void InitTaskData()
	{
		NetSender.Instance.RequestTaskGetCompleted();
		
		NetSender.Instance.RequestTaskGetRunning();

		NetSender.Instance.ShareCountInfoReq();

	    mCurDatingGirid = -1;
		
		promotionConfig = Globals.Instance.MDataTableManager.GetConfig<Promotion>();
		//NetSender.Instance.RequestTaskGetAccept(-1,0,0);
		
		//NetSender.Instance.C2GSRequestGirlDateingList();
		//NetSender.Instance.C2GSPlayerReminderList();
		
		NetSender.Instance.RequestIngotExchangeIsExchange();
		
		_mWarshiDeath = EventManager.Subscribe(WarshipPublisher.NAME + ":" + WarshipPublisher.EVENT_DEATH);
		_mWarshiDeath.Handler = delegate (object[] args)
		{
			FightCellSlot dShip = (FightCellSlot)args[0];
			if (null == dShip)
				return;
			
//			if(GetIsHaveKillNPC(dShip.Property.WarshipLogicID))
//			{
//				UpdateTaskTrackView();
//				if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MBattleStatus)
//				{
//					GUITaskTrack mGUITaskTrack = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTrack>();
//					if(mGUITaskTrack != null)
//					{
//						mGUITaskTrack.SetVisible(false);
//					}
//				}
//			}
		};
	}
	
	public bool GoToTalk(int key, TaskDelegate callback)
	{
		return false;
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		int taskid = -1;
		TALKSTATE state = TALKSTATE.BEFORE;
		
		for(int i=0; i<Globals.Instance.MTaskManager._mUnfinishList.Count; i++)
		{
			TaskData taskData = Globals.Instance.MTaskManager._mUnfinishList[i];
			if(taskData.IsTaskDaily)
			{
				continue;//xiu gai ren wu wu fa ti jiao; 20121009
				//break;
			}
			
			//if( Globals.Instance.MGameDataManager.MCurrentPortData.PortID == taskObjectDic[taskData.Task_ID].Complete_Task_SeaID)
			{
				if(taskData != null && taskData.State == TALKSTATE.COMPLETE)
				{
					taskid = taskData.Task_ID;
				}
			}
		}
		
		for(int i=0; i<Globals.Instance.MTaskManager._mCanAcceptList.Count; i++)
		{
			if(!taskObjectDic.ContainsKey(Globals.Instance.MTaskManager._mCanAcceptList[i]))
				continue;
			//if( Globals.Instance.MGameDataManager.MCurrentPortData.PortID == taskObjectDic[Globals.Instance.MTaskManager._mCanAcceptList[i]].Before_Task_SeaID)
			{
				taskid = Globals.Instance.MTaskManager._mCanAcceptList[i];
			}
		}
		
		if(taskid == -1)
			return false;
		
		Globals.Instance.MTaskManager.mCurTaskId = taskid;
		
		GUIRadarScan.Show();
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(
			delegate (GUITaskTalkView gui)
			{
				GUIRadarScan.Hide();
				gui.UpdateData(taskid, delegate(){callback();});
			}
		);
		
		return true;
	}
	
	public TaskData GetTaskState(int taskid)
	{
		TaskData taskData = null;
		for(int i = 0; i < _mUnfinishList.Count; i++)
		{
			if(taskid == _mUnfinishList[i].Task_ID)
			{
				taskData = _mUnfinishList[i];
			}
		}
		return taskData;
	}

	
	public bool SetTaskStateToAccomplish(int taskid)
	{
		for(int i = 0; i < _mUnfinishList.Count; i++)
		{
			if(taskid == _mUnfinishList[i].Task_ID)
			{
				_mUnfinishList[i].State = TALKSTATE.ACCOMPLISH;
				UpdateTaskTrackView();
			}
		}
		return true;
	}
	
	public bool SetTaskStateToFinished(int taskid)
	{
		for(int i = 0; i < _mUnfinishList.Count; i++)
		{
			if(taskid == _mUnfinishList[i].Task_ID)
			{
				_mUnfinishList[i].State = TALKSTATE.COMPLETE;
				UpdateTaskTrackView();
			}
		}
		return true;
	}
	
	public bool GetIsHaveKillNPC(int LogicID)
	{
//		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
//		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
//		taskConfig.GeTaskObjectList(out taskObjectDic);
//		
//		for(int i = 0; i < _mUnfinishList.Count; i++)
//		{
//			if(taskObjectDic.ContainsKey(_mUnfinishList[i].Task_ID))
//			{
//				if(taskObjectDic[_mUnfinishList[i].Task_ID].Kill_NPC_ID == LogicID)
//				{
//					Globals.Instance.MTaskManager._mUnfinishList[i].KillCacheNum--;
//					return true;
//				}
//			}
//		}
		return false;
	}
	
	public void ClearCurTask()
	{
		mCurTaskId = -1;
	}
	
	public bool UpdateTaskTrackView()
	{
		//Globals.Instance.MGUIManager.CreateWindow<GUITaskTrack>(
		//delegate (GUITaskTrack gui)
		//{
		//	gui.UpdateData();
		//}
		//);
		return true;
	}
	
	public bool GoToMidTalk()
	{
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		mCurTaskId = -1;
//		for(int i = 0; i < _mUnfinishList.Count; i++)
//		{
//			if(_mUnfinishList[i].IsTaskDaily)
//			{
//				break;
//			}
//			
//			TaskConfig.TaskObject taskObject = taskObjectDic[_mUnfinishList[i].Task_ID];
//			
//			//if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus && Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			if(Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			{
//				if(_mUnfinishList[i].State == TALKSTATE.MIDDLE && (!_mUnfinishList[i].IsCompletedMidTalk))
//				{
//					mCurTaskId = taskObject.Task_ID;
//				}
//			}
//		}
		
		if(mCurTaskId == -1)
		{
			return false;
		}
		
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(
		delegate (GUITaskTalkView gui)
		{
		if(gui != null)
		{
			gui.UpdateData(mCurTaskId, null);
		}
		}
		);
		
		return true;
	}
	
	public delegate void TaskDelegate();
	
	public bool GoToAccomplishTalk(TaskDelegate callback)
	{
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		mCurTaskId = -1;
//		for(int i = 0; i < _mUnfinishList.Count; i++)
//		{
//			Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>0IsTaskDaily = "+_mUnfinishList[i].IsTaskDaily);
//			if(_mUnfinishList[i].IsTaskDaily)
//			{
//				Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>1IsTaskDaily = "+_mUnfinishList[i].IsTaskDaily);
//				break;
//			}
//			TaskConfig.TaskObject taskObject = taskObjectDic[_mUnfinishList[i].Task_ID];
//			
//			//if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus && Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			if(Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			{
//				if(_mUnfinishList[i].State == TALKSTATE.ACCOMPLISH&& !_mUnfinishList[i].IsCompletedAccomplishTalk)
//				{
//					mCurTaskId = taskObject.Task_ID;
//				}
//			}
//		}
		
		if(mCurTaskId == -1)
		{
			callback();
			return false;
		}
		
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(
		delegate (GUITaskTalkView gui)
		{
		gui.UpdateData(
			mCurTaskId, 
			delegate()
			{
					callback();
				Debug.Log("-------------Talk----------------Talk--------------------");
			}
			);
		}
		);
		
		return true;
	}
	
	public bool UpdateTaskTalkView()
	{
		TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
		taskConfig.GeTaskObjectList(out taskObjectDic);
		
		mCurTaskId = -1;
//		for(int i = 0; i < _mUnfinishList.Count; i++)
//		{
//			TaskConfig.TaskObject taskObject = taskObjectDic[_mUnfinishList[i].Task_ID];
//			
//			//if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MCopyStatus && Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			if(Globals.Instance.MGameDataManager.MCurrentCopyData.MCopyBasicData.CopyID == taskObject.Pass_Copy_ID)
//			{
//				if(_mUnfinishList[i].State == TALKSTATE.MIDDLE && (!_mUnfinishList[i].IsCompletedMidTalk))
//				{
//					mCurTaskId = taskObject.Task_ID;
//				}
//				if(_mUnfinishList[i].State == TALKSTATE.ACCOMPLISH&& !_mUnfinishList[i].IsCompletedAccomplishTalk)
//				{
//					mCurTaskId = taskObject.Task_ID;
//				}
//			}
//		}
		
		if(mCurTaskId == -1)
		{
			return false;
		}
		
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(
		delegate (GUITaskTalkView gui)
		{
		if(gui != null)
		{
			gui.UpdateData(mCurTaskId, null);
		}
		}
		);
		
		return true;
	}
	
	public void SetCurTaskCopyId(int curTaskCopyId)
	{
		mCurTaskCopyId = curTaskCopyId;
	}
	
	public int GetCurTaskCopyId()
	{
//		if(mCurTaskCopyId < 0)
//		{
//			TaskConfig taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
//			Dictionary<int, TaskConfig.TaskObject> taskObjectDic;
//			taskConfig.GeTaskObjectList(out taskObjectDic);
//			
//			for(int i = 0; i < _mUnfinishList.Count; i++)
//			{
//				if(taskObjectDic.ContainsKey(_mUnfinishList[i].Task_ID))
//				{
//					TaskConfig.TaskObject taskObject = taskObjectDic[_mUnfinishList[i].Task_ID];
//					if(taskObject != null && taskObject.Task_Type != (int)EType.TYPE_TALK)
//					{
//						mCurTaskCopyId = taskObject.TargetID;
//						break;
//					}
//				}
//				else
//				{
//					if(!_mUnfinishList[i].IsTaskDaily)
//					{
//						Debug.LogError("Not Find Task_ID = "+_mUnfinishList[i].Task_ID);
//					}
//				}
//			}
//		}
		return mCurTaskCopyId;
	}

	//================ 任务流程  ------------------------ //


	public void TravelTaskIDRes(int taskid)
	{
//		this.SetVisible(false);
	
		GUIGuoChang.Show();

		GUIGameOutcome guiGameOutcome = Globals.Instance.MGUIManager.GetGUIWindow<GUIGameOutcome>();
		if(guiGameOutcome!=null)
		{
			guiGameOutcome.IsReturnMainScene = false;
			guiGameOutcome.Close();
		}

		GUITravel guiTravel = Globals.Instance.MGUIManager.GetGUIWindow<GUITravel>();
		if(guiTravel!=null)
		{
			guiTravel.IsReturnMainScene = false;
			guiTravel.Close();
		}
		
		
		GUIPhotoGraph guiPhotoGraph = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph>();
		if(guiPhotoGraph!=null)
		{
			guiPhotoGraph.IsReturnMainScene = false;
			guiPhotoGraph.Close();
		}

		GUIMemory guiMemory = Globals.Instance.MGUIManager.GetGUIWindow<GUIMemory>();
		if(guiMemory != null)
		{
			NGUITools.SetActive(guiMemory.gameObject , false);
		}

		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(delegate (GUITaskTalkView gui){

			TaskConfig tk = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
			TaskConfig.TaskObject element = null;
			bool hasData = tk.GetTaskObject(taskid, out element);
			if (!hasData)
				return;
			
			gui.PlayLocalTalk(element.Task_Talk_ID,delegate() 
			{
				TaskAcceptDeal(taskid);
			});
		});
	}
	
	
	private void TaskAcceptDeal(int taskid)
	{
		TaskConfig.TaskObject element = null;
		TaskConfig task = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		bool hasData = task.GetTaskObject(taskid, out element);
		if (!hasData)
			return;	
		GUIGuoChang.Show();
		if(element.Progress_Count <= 0)
		{
			GUIRadarScan.Show();
			if(element.Is_End == 1)
			{
				if(element.Is_Perfect_End == 1)
				{
					NetSender.Instance.RequestTaskCompleteReq(taskid);
				}
				else
				{
					GUITaskTalkView taskview = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTalkView>();
					if(taskview != null){
						taskview.DestroyThisGUI();
					}
					Globals.Instance.MGUIManager.CreateWindow<GUIGameOutcome>(delegate(GUIGameOutcome guiGameOutcome) {
						
						guiGameOutcome.EnterGameOutcome(taskid);
						
						GUIGuoChang.SetTweenPlay(0,delegate() {});
					});
				}
			}
			else
			{
				NetSender.Instance.RequestTaskCompleteReq(taskid);
			}
		}else
		{
			GUITaskTalkView taskview = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTalkView>();
			if(taskview != null){
				taskview.DestroyThisGUI();
			}
			Globals.Instance.MGUIManager.CreateWindow<GUIPhotoGraph>(delegate(GUIPhotoGraph gui) {
				GUIGuoChang.SetTweenPlay(0,delegate() {

//					EliminationMgr.Instance.GameFieldAnimationEndStartGame ();
				});
				GUIMain guimain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
				if(guimain != null)
				{
					guimain.SetVisible(false);					
				}
				gui.DrawReadyView();
			});
		}
	}
	
	
	public void StartNextTask(int id , bool isSpendDiamond = false, int fatherTaskId = 0)
	{
		if(Globals.Instance.MGameDataManager.MActorData.starData.IsTimeborrowing){
			GUIGuoChang.Hide();
			return;
		}
		if(CheckGuideTask(id))
		{
			// 如果新手引导 不需要接任务就先加到缓存里面 -- //
			if(id > 0)
			{
				bool ishave = false;
				foreach(sg.GS2C_Task_GetRunning_Res.GetRunning_TaskItem taskitem in Globals.Instance.MGameDataManager.MActorData.MainTaskIDList)
				{
					if(taskitem.taskId == id)
					{
						ishave = true;
					}
				}
				if(!ishave)
				{
					sg.GS2C_Task_GetRunning_Res.GetRunning_TaskItem taskitem = new sg.GS2C_Task_GetRunning_Res.GetRunning_TaskItem();
					taskitem.taskId = id;
					taskitem.taskState = 1;
					taskitem.taskLastLevel = 0;
					
					Globals.Instance.MGameDataManager.MActorData.MainTaskIDList.Add(taskitem);
				}
			}
			GUITaskTalkView taskview = Globals.Instance.MGUIManager.GetGUIWindow<GUITaskTalkView>();
			if(taskview != null){
				taskview.DestroyThisGUI();
			}
			GUIGuoChang.Hide();
			return;
		}

		if(id == 0)
		{
			GUITravel guiTravel = Globals.Instance.MGUIManager.GetGUIWindow<GUITravel>();
			if(guiTravel!=null)
			{
				guiTravel.Close();
			}else
			{
				GUIMain main = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
				if(main != null){
					main.SetVisible(true);	
					Globals.Instance.MSceneManager.mMainCamera.enabled = true;
				}
			}
			GUIGuoChang.Hide();
			Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId = 0;
			return;
		}
		

		NetSender.Instance.RequestTaskAcceptReq(id,isSpendDiamond,0,fatherTaskId);
	}

	public bool CheckGuideTask(int id)
	{
		bool ishas = false;
		if(Globals.Instance.MTeachManager.IsOpenTeach)
		{
			for(int i = 0; i < mBeginnersGuideTaskID.Length; i++)
			{
				Debug.Log("mBeginnersGuideTaskID[i] = "+ mBeginnersGuideTaskID[i] + "--Globals.Instance.MTeachManager.NewGetTeachStep(mBeginnersGuideIndex[i]) = " + Globals.Instance.MTeachManager.NewGetTeachStep(mBeginnersGuideIndex[i]));
				if(mBeginnersGuideTaskID[i] == id && Globals.Instance.MTeachManager.NewGetTeachStep(mBeginnersGuideIndex[i]) < mBeginnersGuideValue[i] )
				{
					GUIGuoChang.Hide();
					
					GUIMain main = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
					if(main != null){
						main.SetVisible(true);	
						main.SetFunctionState();
						Globals.Instance.MSceneManager.mMainCamera.enabled = true;
					}
					ishas = true;
				}
			}
		}
		return ishas;
	}

	public TourismConfig.TourismElement GetTourismElement(int tourismID)
	{
		TourismConfig tourismConfig = Globals.Instance.MDataTableManager.GetConfig<TourismConfig>();
		TourismConfig.TourismElement element = null;
		foreach(TourismConfig.TourismElement mPair in tourismConfig.GetTourismElementList().Values)
		{
			if(mPair.TourismID == tourismID)
			{
				element = mPair;
			}
		}
		return element;
	}


	public bool NotDisplayingRewardsTask(int taskid)
	{
		for(int i = 0; i < NoRewardDisplayfromTaskID.Length; i++)
		{
			if(taskid == NoRewardDisplayfromTaskID[i])
			{
				return true;
			}
		}
		return false;
	}


	//  日期变化检测是否触发礼包//
	public void NewDateChangePackGiftEvent(int date)
	{
		if(CurrentPromotionGiftId != -1 && CurrentPromotionGiftState == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null&&guiMain.IsVisible)
			{
				guiMain.ShowPackGift();
			}
		}

		if(CurrentFollowState == 1 && CurrentFollowBtnStatus)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null&&guiMain.IsVisible)
			{
				guiMain.ShowFollowInfo();
			}
		}
	}
	public int CurrentPromotionGiftId = -1;
	public int CurrentPromotionGiftState = -1;
	public int CurrentFollowState = -1;
	public bool CurrentFollowBtnStatus = false;
	public List<int> HaveToBuyPackGift = new List<int>(); // 已买礼包//
	public bool MemoryChallenge = false; //  重新挑战 以前的光卡 //
	//------------------------------------------------------ //

	public int[] mBeginnersGuideTaskID = {10000600};
	public string[] mBeginnersGuideIndex = {"x05"};
	public int[] mBeginnersGuideValue= {8};

	private int[] NoRewardDisplayfromTaskID = {10000300};

	ISubscriber _mWarshiDeath = null;
	
	int mCurTaskCopyId = -1;
	
	/// </summary>当前备忘录系统时间服务器发过来//
	public long mSystemTimeS2C = 0; 
	
	public string currentExploreBuildingName = "";
	public string currentTaskExploreName = "";
	public int currentExplorePlaceID = 0;
	public string buildingExploreBG = "";
	public int exploreCount;
	public int exploreTotalCount;
	public bool exploreFinished = false;
	public List<sg.GS2C_Buildings_Res.Buildings_Mes> BuildingLablesList;
	
	public int mCurDatingGirid = -1;
	
	public int mFinishedTaskRightNow = 0;
	public int mCurTaskId = -1;
	/// 当有探索任务的时候，即使收到建筑功能列表 也先不显示呢////// 
	public bool hasExploreTask = false;
	public List<TaskData> _mUnfinishList = new List<TaskData>();
	public Dictionary<int,int> _mFinishedList = new Dictionary<int,int>();
	public List<int> _mCanAcceptList = new List<int>();
	//private Task _mTaskConfig = new Task();
	
	public TaskDailyData mTaskDailyData = new TaskDailyData();
	
	public List<int> _mDatingList = new List<int>();//约会女友列表//
	public Dictionary<int,int> mDtDating = new Dictionary<int, int>();
	public Dictionary<int,bool> mDtDatingNew = new Dictionary<int, bool>();
	public Dictionary<int,bool> mDtDatingFlag = new Dictionary<int, bool>();
	public Dictionary<int,int> mDtDatingFlagMoney = new Dictionary<int, int>();
	public Dictionary<int,int> mDtDatingFlagShi = new Dictionary<int, int>();
	public Dictionary<int,bool> mDtDatingExtraFlag = new Dictionary<int, bool>();
	
	public List<int> _mPlaceList = new List<int>();//约会地点列表 学校了//
	public bool mbDatingRole = false;
	public string mStrRoleName;
	public List<int> _mDatingListPalce = new List<int>();
	public List<int> _mDatingListRem = new List<int>();
	
	
	/// 改进版约会数据//
	/// 
	/// 联系人女友列表//
	public class DatingInfo
	{
		public long nGirld; ///女友ID//
		public int nPlaceID; ///已反邀女友的前往地点ID//
		public long nDatingId; ///反邀记录ID//
	}
	public Dictionary<long,DatingInfo> MapDatingInfo= new Dictionary<long, DatingInfo>();
	
	/// <summary>
	/// 备忘录
	/// </summary>
	public class Record
	{
		public int type; ///约会，节日,生日等 //
		public long date;
		public bool isDone; ///标识约会是否已经完成 //
		public int placeId;
		public string placeName; 
		public long  girlId;  ///针对约会，生日类型//  
		public long nDatingID; ///约会记录ID- actionid//
		public string FestivalInfo; ///节日描述 针对节日//
		public bool IsAtTime = false;
	}
	public Dictionary<long,Record> mapRecord = new Dictionary<long, Record>();
	
	///约会女友的约会地点列表//
	/// <summary>
	/// 额外花费 为探索需要花费
	/// </summary>
	public class ExtraPay
	{
		public bool bNeedPay;///判断需要花费金钱的//
		public int  nMoney;
		public int  nDiamond;
	}
	
	///可约会地点数据//
	public class DatingPlaces
	{
		public int nPlaceID;
		public bool bNew; ///是否是新地点//
		public bool bMeet;///是否满足 个人体力 爱心点数等//
		public ExtraPay extra; ///未探索的,判断是否为为探索地点//
		public int nGirlID; ///此地点所属女友//
	}
	//再次挑战需要的数据//
	public class ChallengeAgain
	{
		public bool isChallengeAgain;
		public int  cityId;
		public float evaluationl;
		public int challengeAgainScene;
	}
	public TaskManager.ChallengeAgain challengeAgain = null;
	//飞机需要的数据//
	public class Fly
	{
		public int cityId;
	}
	public TaskManager.Fly fly = null;
	//人物头像网址//
//	public class HeadURL
//	{
//		public string headURL;
//	}
//	public TaskManager.HeadURL url = null;
	public class HeadURL
	{
		public Texture URLTexture;
		public string URL;
	}
	public TaskManager.HeadURL urlTexture = null;
	public Dictionary<int,DatingPlaces> mapDatingPlaces = new Dictionary<int, DatingPlaces>();
	
	///约会对话//
	public int bDatingSucess = 0;//shibaiyuehui
	
	///当前赴约的角色女友ID//
	public long nCurRoleDatingGirlID = 0;
	/// <summary>
	/// 换装时头发骨骼完整
	/// </summary>
	public bool NeedBone = true;


	public bool IsGetShareReward = false;
	

	public static List<int> GameEndingList = new List<int>();
	
	private static readonly string gameFinishDataFilename = "finishTheEnd.set";
	
	public static void ReadGameFinishDataFilename()
	{
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath +"/"+Globals.Instance.MGameDataManager.MActorData.PlayerID + gameFinishDataFilename,FileMode.Open,FileAccess.Read)){
				using(StreamReader t_sr = new StreamReader(t_file)){
					if ( t_sr != null )
					{
						string[] vecs = t_sr.ReadLine().Split('|');
						for (int i = 0; i < vecs.Length; i++)
							if(!GameEndingList.Contains(StrParser.ParseDecInt(vecs[i],-1)))
							{
								GameEndingList.Add( StrParser.ParseDecInt(vecs[i],-1) );
							}
					}
				}
			}
		}catch(System.Exception ex){
			Debug.LogWarning("Open finishTheEnd file failed, Exception:" + ex.Message);
		}
	}
	
	public static void WriteGameFinishDataFilename(int taskid){
		
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath  +"/"+Globals.Instance.MGameDataManager.MActorData.PlayerID + gameFinishDataFilename,FileMode.Create,FileAccess.Write)){
				using(StreamWriter t_sw = new StreamWriter(t_file)){

					if(!GameEndingList.Contains(taskid))
					{
						GameEndingList.Add(taskid);
					}
					string str = "";
					foreach(int id in GameEndingList)
					{
						str += id.ToString() + "|";
					}
					t_sw.WriteLine(str);
				}
			}
		}catch(System.Exception ex){
			Debug.LogError("Write finishTheEnd file failed, Exception:" + ex.Message);
		}
	}



	private static readonly string DAILY_FIRST_LOGIN_FILE = "dailyfriststatefile.txt";
	public static string getDailyFristLoginTime(){
		string lastLoginTime = "";
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + DAILY_FIRST_LOGIN_FILE,FileMode.Open,FileAccess.Read)){
				using(StreamReader t_sr = new StreamReader(t_file)){
					if ( t_sr != null )
					{
						lastLoginTime = t_sr.ReadLine();
					}
				}
			}
		}catch(System.Exception ex){
			Debug.Log(" lastLoginTime =  " + ex.Message);
		}
		
		return lastLoginTime;
	}
	
	public static void SetDailyFristLoginTime(string dataTime){
		// S filesystem
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + DAILY_FIRST_LOGIN_FILE,FileMode.Create,FileAccess.Write)){
				using(StreamWriter t_sw = new StreamWriter(t_file)){
					t_sw.WriteLine(dataTime);
				}
			}
		}catch(System.Exception ex){
			Debug.LogError( DAILY_FIRST_LOGIN_FILE + " create failed! Error: " + ex.Message);
		}
	}
}
