using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIMain : GUIWindow
{
	public enum ESceneType  
	{
		PORT,//港口场景
		COPY,//副本场景
		DOCK,//玩家码头
	}
	
	//角色名字//
	public NewWealthGroup newWealthGroup;
	public UILabel TextRoleName; 
	public UILabel FansLabel;
//	public UITexture AvatarIconTexture;
	public UIButton RoleInfo;  // 详细信息按钮//
	public GameObject NewRoleInfo; // 备忘录new图标// 
	public GameObject LeftControl;
	public UILabel SuperFsanLabel;
	public UIButton SuperFansBtn;
	public UIButton ButtonClothShop;
	public UIButton ButtonChangeCloth;
	public UIButton KTPlay;
	public UIButton ButtonMemory;
	public UIButton ButtonRanking;

	public UIButton  GotoBtn;
	public GameObject DownControl;
	public GameObject AllFunctionBtn;
	public UIButton TrainingBtn;
	public UIButton JobBtn;
	public UIButton TravelBtn;
	public UIButton RestBtn;
	public UILabel DateEventLabel; 
	public UILabel FatigueValueLabel;
	public UILabel NeedRestLabel;
	public UILabel MainTaskLabel;
	public UILabel WeekLabel;

	//晕倒相关 -- //

	public GameObject FaintEffect;
	public GameObject FaintInterface;
	public UIButton AutomaticBtn;
	public UIButton FastBtn;
	public UILabel DateLabel;
	public UILabel RecoveryLabel;

	// -------- 时间借用魔法 ---- //
	public GameObject TimeBorrowing;
	public UIButton BorrowTrainingBtn;
	public UIButton BorrowJobBtn;
	public UIButton BorrowStarPathBtn;
	public UIButton BorrowExitBtn;
	public UILabel TimeNumLabel;
	public UIButton AddTimeBorrowingBtn;

	private int mFaintState ;

	// ------------//
	public GameObject RestEffect;
	public GameObject DateChangeAnimation;
	public GameObject CurtainSprite;
	public GameObject DateChange;

	public TweenPosition YearDateLabel;
	public UILabel YearBeforeLabel;
	public UILabel YearAfterLabel;

	public TweenPosition MonthDateLabel;
	public UILabel MonthBeforeLabel;
	public UILabel MonthAfterLabel;

	public TweenPosition DayDateLabel;
	public UILabel DayBeforeLabel;
	public UILabel DayAfterLabel;

	public UIButton ChestBtn;
	public GameObject FreeDraw;
	public UIButton PromotionalGiftBtn;
	public UIButton FollowBtn;
	public GameObject FollowNewSprite;
	private int mDateDownTime ;

	private PlayerData mPlayerDate;
	private int mLastLineDay ;

	private TaskConfig taskConfig;
	private bool mRestState = false;
	private int mMainTaskID = -1;

	private int mCoolDownTime = 3;

	private bool UpdateTimeLine = false;

	protected override void Awake()
	{
		base.Awake();	
		
		if(!Application.isPlaying || Globals.Instance.MGUIManager == null || Globals.Instance.MTaskManager == null) return;

		WeekLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(7017),Globals.Instance.MGameDataManager.MActorData.BasicData.PerfectEndPassNum);

		if(!Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState)
		{
			ButtonRanking.transform.localScale = Vector3.zero;
			ChestBtn.transform.localScale = Vector3.zero;
			FollowBtn.transform.parent.localScale = Vector3.zero;
		}
		else
		{
			FollowBtn.transform.parent.localScale = Vector3.one;
			ButtonRanking.transform.localScale = Vector3.one;
		}
		KTPlay.transform.localScale = Vector3.zero;

	}

	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		_mMainUI = this.gameObject.transform;
		_mMainUI.parent = Globals.Instance.MGUIManager.MGUICamera.transform;
		_mMainUI.localPosition = new Vector3(0,0,800);
		base.GUILevel = 25;
		RegisterInfoChange();
		taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
	
		UIEventListener.Get(RoleInfo.gameObject).onClick += OnClickPlayerInfoBtn;

		OrbitCamera camera = Globals.Instance.MSceneManager.mMainCamera.transform.parent.GetComponent<OrbitCamera>();
		camera.enabled = true;

		mPlayerDate = Globals.Instance.MGameDataManager.MActorData;
		mLastLineDay = mPlayerDate.starData.nLineDay;
		mFaintState = mPlayerDate.BasicData.CountryID;

		UIEventListener.Get(ButtonClothShop.gameObject).onClick += OnClickButtonFuzhaungdian;
		UIEventListener.Get(ButtonChangeCloth.gameObject).onClick += OnClickButtonHuanzhuang;
		UIEventListener.Get(KTPlay.gameObject).onClick += OnClickKTPlay;
		UIEventListener.Get(ButtonRanking.gameObject).onClick += OnClickButtonRanking;
		UIEventListener.Get(ButtonMemory.gameObject).onClick += OnClickButtonMemory;
		UIEventListener.Get(TrainingBtn.gameObject).onClick += OnClickTrainingBtn;
		UIEventListener.Get(JobBtn.gameObject).onClick += OnClickJobBtn;
		UIEventListener.Get(TravelBtn.gameObject).onClick += OnClickTravelBtn;
		UIEventListener.Get(RestBtn.gameObject).onClick += OnClickRestBtn;
		UIEventListener.Get(GotoBtn.gameObject).onClick += OnClickGotoBtn;

		UIEventListener.Get(AutomaticBtn.gameObject).onClick += OnClickAutomaticBtn;
		UIEventListener.Get(FastBtn.gameObject).onClick += OnClickFastBtn;
		UIEventListener.Get(SuperFansBtn.gameObject).onClick += OnClickSuperFansBtn;
		UIEventListener.Get(ChestBtn.gameObject).onClick += OnClickChestBtn;
		UIEventListener.Get(PromotionalGiftBtn.gameObject).onClick += OnClickPromotionalGiftBtn;
		UIEventListener.Get(FollowBtn.gameObject).onClick += OnClickFollowBtn;
		UIEventListener.Get(BorrowTrainingBtn.gameObject).onClick += OnClickTrainingBtn;
		UIEventListener.Get(BorrowJobBtn.gameObject).onClick += OnClickJobBtn;
		UIEventListener.Get(BorrowStarPathBtn.gameObject).onClick += OnClickBorrowStarPathBtn;
		UIEventListener.Get(BorrowExitBtn.gameObject).onClick += OnClickBorrowExitBtn;
		UIEventListener.Get(AddTimeBorrowingBtn.gameObject).onClick += OnClickAddTimeBorrowingBtn;


		UpdateMainTask();
		UpdateMainTime();
		FaintTreatment(mFaintState);

		Globals.Instance.MTeachManager.NewDateChangeEvent(mPlayerDate.starData.nLineDay);
	}

	
	//控制当前是出港还是进港
	public void SetUISceneStatus(ESceneType sceneType)
	{
		return;
		switch(sceneType)
		{
		case ESceneType.PORT:

			break;
		case ESceneType.COPY:
		case ESceneType.DOCK:
			break;
		}
	}
	
	public override void SetVisible(bool visible)
	{
		base.SetVisible(visible);
		PortStatus port = ((PortStatus)GameStatusManager.Instance.MCurrentGameStatus);
		port.orbitCamera.SetRotationStatus(visible);
		if(visible)
		{
			newWealthGroup.UpdateWealth();

			if(UpdateMainTime())
			{
				UpdateMainTask();
			}
		}
		Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIMain");
	}

	public void SetFunctionState()
	{
		AllFunctionBtn.transform.localScale = Vector3.one;
		GotoBtn.transform.localScale = Vector3.zero;
		MainTaskLabel.text = "";
	}

	//init ui data
	void InitUIData(PlayerData playerData)
	{
		TextRoleName.text =  playerData.BasicData.Name + "";
		FansLabel.text = HelpUtil.GetMoneyFormattedText(playerData.starData.nRoleFenSi);

		SuperFsanLabel.text = Globals.Instance.MGameDataManager.MActorData.WarshipList.Count.ToString();
		TimeNumLabel.text = "X"+playerData.WealthData.Oil.ToString();

//		if(Globals.Instance.MTaskManager.urlTexture!=null)
//		{
//			AvatarIconTexture.mainTexture = Globals.Instance.MTaskManager.urlTexture.URLTexture;
//		}
//		else
//		{
//			AvatarIconTexture.mainTexture = Resources.Load("Icon/AvatarIcon/GirlAvatar03",typeof(Texture2D)) as Texture2D;
//		}
	}

	void UpdatePlayerInformation(PlayerData playerData)
	{
		DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
		DateTime mCurrentTime = mStartTime.AddDays(playerData.starData.nLineDay);
//		DateEventLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
		string piLaoText = "[00ff00]" + playerData.starData.nRolePiLao +"/100[-]";
		if(playerData.starData.nRolePiLao > 40&&playerData.starData.nRolePiLao < 80)
		{
			piLaoText = "[ffff00]" + playerData.starData.nRolePiLao +"/100[-]";
		}
		else if(playerData.starData.nRolePiLao >= 80)
		{
			piLaoText = "[ff0000]" + playerData.starData.nRolePiLao +"/100[-]";
		}
		FatigueValueLabel.text = piLaoText;
		if(playerData.starData.nRolePiLao >= 80)
		{
			NeedRestLabel.transform.localScale = Vector3.one;
			FatigueValueLabel.transform.localPosition = new Vector3(500f,140f,0f);
		}
		else
		{
			NeedRestLabel.transform.localScale = Vector3.zero;
			FatigueValueLabel.transform.localPosition = new Vector3(500f,114f,0f);
		}

		if(mCurrentTime.Day == 1 || mCurrentTime.Day == 15)
		{
			if(Globals.Instance.MJobManager.getJobPlaceInformationDic.Count > 0&&
			   Globals.Instance.MJobManager.GetJobRefreshLastTime != playerData.starData.nLineDay)
			{
				RefreshJobList(playerData);
			}
		}

		if(playerData.starData.IsTimeborrowing){
			DownControl.transform.localScale = Vector3.zero;
			TimeBorrowing.transform.localScale = Vector3.one;
			TimeNumLabel.text = "X"+playerData.WealthData.Oil.ToString();
		}else{
			DownControl.transform.localScale = Vector3.one;
			TimeBorrowing.transform.localScale = Vector3.zero;
		}
	}
	private void RefreshJobList(PlayerData playerData)
	{
		Dictionary<int,List<int>> jobRefreshDic = new Dictionary<int, List<int>>();
		JobPlaceConfig jobPlaceConfig = Globals.Instance.MDataTableManager.GetConfig<JobPlaceConfig>();
		JobConfig jobConfig = Globals.Instance.MDataTableManager.GetConfig<JobConfig>();
		foreach(KeyValuePair<int, JobPlaceConfig.JobPlaceElement> jobPlaceElement in jobPlaceConfig.GetJobPlaceElementList())
		{
			jobRefreshDic.Add(jobPlaceElement.Key,jobConfig.GetJobSingleElementList(jobPlaceElement.Key));
		}
		Globals.Instance.MJobManager.getJobPlaceInformationDic = jobRefreshDic;
		
		NetSender.Instance.C2GSModifyJobListReq(jobRefreshDic ,playerData.starData.nLineDay);
		Globals.Instance.MJobManager.GetJobRefreshLastTime = playerData.starData.nLineDay;
	}
	
	public void HeadURL()
	{
//		if(Globals.Instance.MTaskManager.urlTexture!=null)
//		{
//			AvatarIconTexture.mainTexture = Globals.Instance.MTaskManager.urlTexture.URLTexture;
//		}	
	}




	private void OnClickPlayerInfoBtn(GameObject obj)
	{	
		Globals.Instance.MGUIManager.CreateWindow<GUIPlayer>(delegate(GUIPlayer gui)
		{
			NetSender.Instance.RequestTaskGetCompleted();
		});
	}

	public void Update(){

	}


	//register info change 
	void RegisterInfoChange()
	{
		//attribute

		_mPlayerInfoUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_PLAYERINFO_UPDATE);
		_mPlayerInfoUpdate.Handler = delegate (object[] args)
		{
			PlayerData playerData = (PlayerData)args[0];
			UpdatePlayerInformation(playerData);
			
		};

		_mInfoUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_WORTH_UPDATE);
		_mInfoUpdate.Handler = delegate (object[] args)
		{
			PlayerData playerData = (PlayerData)args[0];
			InitUIData(playerData);
			
		};
		
		_mPlayerLevelUpdate = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_LEVEL_UPDATE);
		_mPlayerLevelUpdate.Handler = delegate (object[] args)
		{
			PlayerData playerData = (PlayerData)args[0];
			InitUIData(playerData);
		};
	}
	
	public Vector3 GetButtonRankPosition()
	{
		return Vector3.zero;
	}

	public void CleanUpTaskCache()
	{
		mMainTaskID = -1;
		mRestState = false;
		mLastLineDay = 0;
		mFaintState  = 0;
	}
	

	//设置当前的额物体
	private Transform _mMainUI;
	//界面更新事件
	ISubscriber _mPlayerInfoUpdate = null;
	ISubscriber _mInfoUpdate = null;
	ISubscriber _mPlayerLevelUpdate = null;
	

	private void OnClickTrainingBtn(GameObject obj)
	{
		GUIRadarScan.Show();

		NetSender.Instance.C2GSTrainListReq();

	}

	private void OnClickJobBtn(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIJob>(delegate(GUIJob guiJob)
		{
		});
	}

	private void OnClickTravelBtn(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateWindow<GUITravel>(delegate(GUITravel guiTravel)
		{
			
		});
	}


	private void OnClickRestBtn(GameObject obj)
	{
		mRestState = true;
		NetSender.Instance.C2GSRestRewardReq(0);
	}

	private void OnClickGotoBtn(GameObject obj)
	{
		Globals.Instance.MTaskManager.StartNextTask(mPlayerDate.MainTaskIDList[0].taskId);
	}

	public bool getRestEffect()
	{
		if(mRestState)
		{
			return true;
		}
		return false;
	}

	public bool ShowRestEffect()
	{
		if(mRestState)
		{
			mRestState = false;
			RestEffect.transform.localScale = Vector3.one;
//			StartCoroutine(RestNeedWaitTime());

			InvokeRepeating("RestNeedWaitTime",0f,1f);

			return true;
		}

		return false;
	}

	private void RestNeedWaitTime()
	{
		mCoolDownTime-- ;
		if(mCoolDownTime <= 0 )
		{
			CancelInvoke("RestNeedWaitTime");
			mCoolDownTime = 3;
			RestEffect.transform.localScale = Vector3.zero;

			if(UpdateMainTime())
			{
				UpdateMainTask();
			}
		}
	}

	public delegate void TweenEventDelegate();
	private void DateChangeCurtainAnimationStart(TweenEventDelegate onActiveFinished = null)
	{
		DateChangeAnimation.transform.localScale = Vector3.one;
		CurtainSprite.transform.localPosition = new Vector3(0f,2100f,0f);
		TweenPosition tween = TweenPosition.Begin(CurtainSprite , 0.4f,Vector3.zero);
		CurtainSprite.transform.localScale = Vector3.one;
		EventDelegate.Add(tween.onFinished,delegate() {

			if (onActiveFinished != null)
					onActiveFinished();
		},true);
	}

	private bool UpdateMainTime()
	{
		if(mLastLineDay < mPlayerDate.starData.nLineDay)
		{
			DateChangeCurtainAnimationStart(delegate() {
				Vector3 vec = new Vector3(0f,90f,0f);
				DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
				DateTime mLastTime = mStartTime.AddDays(mLastLineDay);
				DateTime mCurrentTime = mStartTime.AddDays(mPlayerDate.starData.nLineDay);
//				DateEventLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
				int mLastYear = mLastTime.Year;
				int mLastMonth = mLastTime.Month;
				int mLastDay = mLastTime.Day;
				
				int mCurrentYear = mCurrentTime.Year;
				int mCurrentMonth = mCurrentTime.Month;
				int mCurrentDay = mCurrentTime.Day;
				
				YearBeforeLabel.text = mLastYear.ToString();
				YearAfterLabel.text = mCurrentYear.ToString();
				
				MonthBeforeLabel.text = mLastMonth.ToString();
				MonthAfterLabel.text = mCurrentMonth.ToString();
				
				DayBeforeLabel.text = mLastDay.ToString();
				DayAfterLabel.text = mCurrentDay.ToString();
				
				int mChangeNum = 0;
//				TweenPosition tweenPositionYear ;
				
				float durationTime = 1f;
				
				if(mCurrentYear != mLastYear)
				{
					YearDateLabel.from = Vector3.zero;
					YearDateLabel.to = vec;
					YearDateLabel.duration = durationTime;
					YearDateLabel.delay = 0f;
				
					YearDateLabel.ResetToBeginning();
					YearDateLabel.PlayForward();
					mChangeNum++;
				}
				
				if(mCurrentMonth != mLastMonth)
				{
					MonthDateLabel.from = Vector3.zero;
					MonthDateLabel.to = vec;
					MonthDateLabel.duration = durationTime;
					MonthDateLabel.delay = mChangeNum * durationTime;
				
					MonthDateLabel.ResetToBeginning();
					MonthDateLabel.PlayForward();
					mChangeNum++;
				}
				
				if(mCurrentDay != mLastDay)
				{
					DayDateLabel.from = Vector3.zero;
					DayDateLabel.to = vec;
					DayDateLabel.duration = durationTime;
					DayDateLabel.delay = mChangeNum * durationTime;
				
					DayDateLabel.ResetToBeginning();
					DayDateLabel.PlayForward();
					mChangeNum++;
				}
				
				
				
				mDateDownTime =  (int)(mChangeNum*durationTime);

				DateChange.transform.localScale = Vector3.one;

				InvokeRepeating("ChangeTimeNeedWaitTime",2f, 1f);
				
			});
			UpdateTimeLine = true;
			return false;
		}
		else
		{
			DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
			DateTime mCurrentTime = mStartTime.AddDays(mPlayerDate.starData.nLineDay);
			DateEventLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
			UpdateTimeLine = false;
			return true;
		}
	}


	private void ChangeTimeNeedWaitTime()
	{
		mDateDownTime-- ;
		if(mDateDownTime <= 0 )
		{
			CancelInvoke("ChangeTimeNeedWaitTime");

			DateChange.transform.localScale = Vector3.zero;
//			DateChangeAnimation.transform.localScale = Vector3.zero;
			YearDateLabel.transform.localPosition = Vector3.zero;
			MonthDateLabel.transform.localPosition = Vector3.zero;
			DayDateLabel.transform.localPosition = Vector3.zero;
			
			mLastLineDay = mPlayerDate.starData.nLineDay;
			DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
			DateTime mCurrentTime = mStartTime.AddDays(mPlayerDate.starData.nLineDay);
			DateEventLabel.text = mCurrentTime.ToString("yyyy-MM-dd");

			DateChangeCurtainAnimationEnd(delegate() {
				Globals.Instance.MTeachManager.NewDateChangeEvent(mPlayerDate.starData.nLineDay);
				UpdateMainTask();
			});
		}
	}

	private void DateChangeCurtainAnimationEnd(TweenEventDelegate onActiveFinished = null)
	{
		TweenPosition tween = TweenPosition.Begin(CurtainSprite , 0.4f,new Vector3(0f,2100f,0f));
		EventDelegate.Add(tween.onFinished,delegate() {

			CurtainSprite.transform.localPosition = new Vector3(0f,2100f,0f);
			CurtainSprite.transform.localScale = Vector3.zero;
			DateChangeAnimation.transform.localScale = Vector3.zero;
			if (onActiveFinished != null)
				onActiveFinished();
		},true);
	}

	public void UpdateMainTask()
	{
		if(mPlayerDate.MainTaskIDList.Count > 0)
		{
			AllFunctionBtn.transform.localScale = Vector3.zero;
			GotoBtn.transform.localScale = Vector3.one;


			if(mPlayerDate.MainTaskIDList.Count > 1)
			{
				if(!(mPlayerDate.MainTaskIDList[0].taskState == (int)TaskManager.TaskState.PASS) &&
				   !(mPlayerDate.MainTaskIDList[0].taskState == (int)TaskManager.TaskState.UNPASS))
				{
					GUIGuoChang.Show();
//					NetSender.Instance.RequestTaskAcceptReq(mPlayerDate.MainTaskIDList[0].taskId,0);
					Globals.Instance.MTaskManager.StartNextTask(mPlayerDate.MainTaskIDList[0].taskId);
				}
				else
				{
					ShowTaskSettlement(mPlayerDate.MainTaskIDList[0]);
				}
			}
			else
			{
				if(!(mPlayerDate.MainTaskIDList[0].taskState == (int)TaskManager.TaskState.PASS) &&
				   !(mPlayerDate.MainTaskIDList[0].taskState == (int)TaskManager.TaskState.UNPASS))
				{
					if(NowCheckGuideTask(mPlayerDate.MainTaskIDList[0].taskId))
					{
						return;
					}
					if(mPlayerDate.MainTaskIDList[0].taskId != mMainTaskID)
					{
						mMainTaskID = mPlayerDate.MainTaskIDList[0].taskId;
						TaskConfig.TaskObject taskObject = null;
						bool ishas = taskConfig.GetTaskObject(mMainTaskID , out taskObject);
						if(!ishas)
						{
							return;
						}
						
						if(taskObject.Is_Delay == 0 || mPlayerDate.MainTaskIDList[0].passCount > 0)
						{
							GUIGuoChang.Show();
//							NetSender.Instance.RequestTaskAcceptReq(mPlayerDate.MainTaskIDList[0].taskId,0);
							Globals.Instance.MTaskManager.StartNextTask(mPlayerDate.MainTaskIDList[0].taskId);
							return;
						}
						
						MainTaskLabel.text  = taskObject.Task_Desc;
					}
				}
				else 
				{
					ShowTaskSettlement(mPlayerDate.MainTaskIDList[0]);
				}
			}
		}
		else
		{
			AllFunctionBtn.transform.localScale = Vector3.one;
			GotoBtn.transform.localScale = Vector3.zero;
			MainTaskLabel.text = "";
			if(UpdateTimeLine )
			{
				GUIRadarScan.Show();
				NetSender.Instance.C2GSTaskPlaceReq((int)TaskManager.TaskCategory.CHANGEDATE,-1);
			}
		}
		UpdateTimeLine = false;
	}

	// 显示任务结算界面 // 
	private void ShowTaskSettlement(sg.GS2C_Task_GetRunning_Res.GetRunning_TaskItem taskitem)
	{
		if(Globals.Instance.MGameDataManager.MActorData.starData.IsTimeborrowing){
			return;
		}
		Globals.Instance.MGUIManager.CreateWindow<GUIGameOutcome>(delegate(GUIGameOutcome guiGameOutcome) {
			guiGameOutcome.ShowTaskSettlement(taskitem);
		});
	}


	/// <summary>
	/// 晕倒处理-- 
	/// </summary>
	public void FaintTreatment(int state)
	{
		mFaintState = state;
		if(mFaintState == 1)
		{
			FaintEffect.transform.localScale = Vector3.one;
			DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
			DateTime mCurrentTime = mStartTime.AddDays(mPlayerDate.starData.nLineDay);
			FaintInterface.transform.localScale = Vector3.one;
			RecoveryLabel.transform.localScale = Vector3.zero;
			
			DateLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
		}
	}


	private void OnClickAutomaticBtn(GameObject obj)
	{
		// 发送 恢复 协议 增加7 天时间 --
		NetSender.Instance.C2GSRestRewardReq(2);
	}

	public bool getFaintState()
	{
		if(mFaintState == 1)
		{
			return true;
		}
		return false;
	}


	// 自动恢复 返回 --  增加时间 //
	public bool AutomaticRecoveryRes()
	{
		if(mFaintState == 1)
		{
			FaintEffect.transform.localScale = Vector3.one;
			FaintInterface.transform.localScale = Vector3.zero;
			RecoveryLabel.transform.localScale = Vector3.one;
			
			DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
			DateTime mCurrentTime = mStartTime.AddDays(mLastLineDay);
			RecoveryLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
			
			InvokeRepeating("AutomaticRecoveryInvoke", 1f , 1f);

			return true;
		}
		return false;
	}

	private void AutomaticRecoveryInvoke()
	{
		mLastLineDay++;
		DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
		DateTime mCurrentTime = mStartTime.AddDays(mLastLineDay);
		RecoveryLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
		if(mLastLineDay > mPlayerDate.starData.nLineDay)
		{
			mLastLineDay = mPlayerDate.starData.nLineDay;
			FaintEffect.transform.localScale = Vector3.zero;
			CancelInvoke("AutomaticRecoveryInvoke");
			mFaintState  = 0;
			UpdateMainTask();
		}
		else if(mLastLineDay == mPlayerDate.starData.nLineDay)
		{
			DateEventLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
		}
	}

	private void OnClickFastBtn(GameObject obj)
	{
		if(mPlayerDate.WealthData.GoldIngot < 50)
		{
			Globals.Instance.MGUIManager.ShowErrorTips(20007);
			return;
		}
		NetSender.Instance.C2GSRestRewardReq(1);
	}

	private void OnClickSuperFansBtn(GameObject obj)
	{
		if(Globals.Instance.MGameDataManager.MActorData.WarshipList.Count <= 0)
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(1015);
			return;
		}
		Globals.Instance.MGUIManager.CreateWindow<GUISuperFans>(delegate(GUISuperFans guiSuperFans){});
	}
	
	private void OnClickChestBtn(GameObject obj)
	{
		NetSender.Instance.GetLotteryInfoReq(true);
	}

	private void OnClickPromotionalGiftBtn(GameObject obj)
	{
		if(Globals.Instance.MTaskManager.CurrentPromotionGiftId <= 0)
			return;
		Globals.Instance.MGUIManager.CreateWindow<GUIPromotionGift>(delegate(GUIPromotionGift gui){
			gui.ShowPromotionGift();
		});
	}

	private void OnClickFollowBtn(GameObject obj)
	{
		NetSender.Instance.C2GSParticularConcernInfoReq();
	}

	private void OnClickButtonHuanzhuang(GameObject obj)
	{
		Globals.Instance.MTaskManager.NeedBone = false;
		Globals.Instance.MGUIManager.CreateWindow<GUIChangeCloth>(delegate(GUIChangeCloth Change){});
	}

	private void OnClickButtonFuzhaungdian(GameObject obj)
	{

		GUIRadarScan.Show();
		NetSender.Instance.C2GSRequestShopItems(510,(int)mPlayerDate.BasicData.Gender);
	}

	private void OnClickButtonMemory(GameObject obj)
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIMemory>(delegate(GUIMemory guiMemory){});
	}

	public void OnClickBorrowExitBtn(GameObject obj){
		NetSender.Instance.C2GSUpdateTimeborrowingReq(false);
	}
	public void OnClickAddTimeBorrowingBtn(GameObject obj){
		NetSender.Instance.RequestBuyOilPrice();
	}

	public void OnClickBorrowStarPathBtn(GameObject obj){
		Globals.Instance.MGUIManager.CreateWindow<GUIMemory>(delegate(GUIMemory guiMemory){
			guiMemory.StartInfo();
		});
	}
	private void OnClickKTPlay(GameObject obj)
	{
//		string str ="101119" + Globals.Instance.MGameDataManager.MActorData.PlayerID.ToString();

		#if UNITY_ANDROID
		AndroidSDKAgent.OpenKTPlay();
		#endif
		
		#if UNITY_IPHONE
		//U3dAppStoreSender.OpenKTPlay();
		#endif
	}

	private void OnClickButtonRanking(GameObject obj)
	{
		NetSender.Instance.C2GSRankingListInfoReq();
	}

	private bool NowCheckGuideTask(int id)
	{
		bool ishas = false;
		if(Globals.Instance.MTeachManager.IsOpenTeach)
		{
			for(int i = 0; i < Globals.Instance.MTaskManager.mBeginnersGuideTaskID.Length; i++)
			{
				if(Globals.Instance.MTaskManager.mBeginnersGuideTaskID[i] == id && Globals.Instance.MTeachManager.NewGetTeachStep(Globals.Instance.MTaskManager.mBeginnersGuideIndex[i]) <  Globals.Instance.MTaskManager.mBeginnersGuideValue[i])
				{
					SetFunctionState();
					ishas = true;
				}
			}
		}
		return ishas;
	}

	public void ShowPackGift()
	{
		if(Globals.Instance.MTaskManager.CurrentPromotionGiftId <= 0)
			return;
		if(mPlayerDate.MainTaskIDList.Count > 0)
			return;
		Globals.Instance.MTaskManager.CurrentPromotionGiftState = -1;
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(delegate (GUITaskTalkView gui){
			gui.PlayLocalTalk(10000100,delegate() {
				this.SetVisible(true);
				Globals.Instance.MGUIManager.CreateWindow<GUIPromotionGift>(delegate(GUIPromotionGift guiPromotionGift){
					gui.DestroyThisGUI();
					guiPromotionGift.ShowPromotionGift();
				});
			});
		});
	}

	public void ShowFollowInfo()
	{
		if(!Globals.Instance.MTaskManager.CurrentFollowBtnStatus)
			return;
		if(mPlayerDate.MainTaskIDList.Count > 0)
			return;
		Globals.Instance.MTaskManager.CurrentFollowState = -1;
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(delegate (GUITaskTalkView gui){
			gui.PlayLocalTalk(10000101,delegate() {
				this.SetVisible(true);
				Globals.Instance.MGUIManager.CreateWindow<GUIParticularConcern>(delegate(GUIParticularConcern guiParticularConcern){
					gui.DestroyThisGUI();
					guiParticularConcern.ShowMainInformation();
				});
			});
		});
	}

	public void UpdateLotteryState()
	{
		if(Globals.Instance.MGameDataManager.MActorData.LotteryNumber > 0)
		{
			FreeDraw.transform.localScale = Vector3.one;
		}
		else 
		{
			FreeDraw.transform.localScale = Vector3.zero;
		}
	}

	public void FunctionButtonState()
	{
		int techStep = Globals.Instance.MTeachManager.NewGetTeachStep("x06");
		UISprite mTrainingClose = TrainingBtn.transform.Find("Animation").transform.Find("Sprite").transform.Find("Close").GetComponent<UISprite>();
		if(Globals.Instance.MTeachManager.IsOpenTeach&&techStep < 1)
		{
			mTrainingClose.transform.localScale = Vector3.one;
			TrainingBtn.isEnabled = false;
			NewRoleInfo.transform.localScale = Vector3.zero;
		}
		else
		{
			NewRoleInfo.transform.localScale = Vector3.one;
			mTrainingClose.transform.localScale = Vector3.zero;
			TrainingBtn.isEnabled = true;
		}

		techStep = Globals.Instance.MTeachManager.NewGetTeachStep("x07");
		UISprite mJobClose = JobBtn.transform.Find("Animation").transform.Find("Sprite").transform.Find("Close").GetComponent<UISprite>();
		if(Globals.Instance.MTeachManager.IsOpenTeach&&techStep < 1)
		{
			mJobClose.transform.localScale = Vector3.one;
			JobBtn.isEnabled = false;
		}
		else
		{
			mJobClose.transform.localScale = Vector3.zero;
			JobBtn.isEnabled = true;
		}

		techStep = Globals.Instance.MTeachManager.NewGetTeachStep("x09");
		UISprite mTravelClose = TravelBtn.transform.Find("Animation").transform.Find("Sprite").transform.Find("Close").GetComponent<UISprite>();
		if(Globals.Instance.MTeachManager.IsOpenTeach&&techStep < 1)
		{
			mTravelClose.transform.localScale = Vector3.one;
			TravelBtn.isEnabled = false;
		}
		else
		{
			mTravelClose.transform.localScale = Vector3.zero;
			TravelBtn.isEnabled = true;
		}

		techStep = Globals.Instance.MTeachManager.NewGetTeachStep("x08");
		UISprite mRestClose = RestBtn.transform.Find("Animation").transform.Find("Sprite").transform.Find("Close").GetComponent<UISprite>();
		if(Globals.Instance.MTeachManager.IsOpenTeach&&techStep < 1)
		{
			mRestClose.transform.localScale = Vector3.one;
			RestBtn.isEnabled = false;
		}
		else
		{
			mRestClose.transform.localScale = Vector3.zero;
			RestBtn.isEnabled = true;
		}

		techStep = Globals.Instance.MTeachManager.NewGetTeachStep("x10");

		if (!Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState) {
			ChestBtn.transform.localScale = Vector3.zero;
		} else {
			if(Globals.Instance.MTeachManager.IsOpenTeach&&techStep < TeachManager.TeachFinishedValue)
			{
				ChestBtn.transform.localScale = Vector3.zero;
			}
			else
			{
				ChestBtn.transform.localScale = Vector3.one;
				if(Globals.Instance.MTaskManager.CurrentFollowBtnStatus)
				{
					FollowBtn.transform.localScale = Vector3.one;
				}
			}
		}
	}

}
