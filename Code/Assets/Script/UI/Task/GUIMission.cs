using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMission : GUIWindowForm
{
	
	public GameObject FriendsView;
	public GameObject MissionView;
	public UIButton ButtonMap;
	public UILabel CityNameLabel;
	public UILabel MissionNumLabel;
	public GameObject CityInfor;
	public UILabel CityInforDaily;
	public GameObject DailyTaskBtnNull;
	public UITable MainTable;
	public UIScrollView MainScrollView;
	public GameObject MissionItemPrefab;
	public GameObject CompleteMissionItemPrefab;
	
	public GameObject MainMission;
	public GameObject NoMissionInfo;
	
	private string[] HangyeDengji = {"HangyeDengjiD","HangyeDengjiC","HangyeDengjiB","HangyeDengjiA","HangyeDengjiS"};
	public enum TaskRequireVisibleTypeEnum {
		SEXLIMIT = 1,		//(1,"任务性别需求"),//1-男 2-女//
		ITEMCOUNT = 2,		//(2,"道具达到"),//
		BEFORETASKID = 3,	//(3, "完成指定ID任务"),//num为0，则表示还没完成某任务//
		ROLEATTR = 4,		//(4,"玩家属性达到"),  //57-演技 40-魅力,1-金钱 2-钻石//
		HAVEARTIST = 5,	//(5, "艺术家品阶获得"),  //num表示品阶，表示获得某个品阶的艺术家//
		FANSNUM = 6,		//(6, "某地区粉丝数达到"),//type标识地区 //
	}
	
	public enum TaskCategoryEnum
	{
		DAILY = 1,   		//日常//
		MAIN = 2,			//主线//
		BRANCH = 3,			//支线//
		ARTIST = 4,			//艺术家//
		ACTIVITY = 5,		//活动//
		UnKnownType = 9999,	//未知//
	}
	
	public enum TaskRewardTypeEnum
	{
		PUBLISH = 1,		//发行物			//
		ITEM = 2,			//道具			//
		POSTURE = 3, 		//动作			//	
		ROLE_ATTR = 4,		//玩家属性达到   	//
		GET_ARTTST = 5,		// 艺术家获得		//
		FANS_NUM = 6,		//粉丝数			//
		AREA_OPEN = 7,   	//开发某个地区	//
		UNKNOWNTYPE = 9999  // 未知类型		//
	}
	
	public enum PublishType
	{
		MAGAZINE = 1,//(1, "杂志") //
		PHOTO = 2,//(2, "写真") //
		ADVERT = 3,//(3, "广告") //
		MOVICE = 4,//(4, "电影")//
	}
	
	public enum BaseRewardType
	{
		MONEY = 1,			// 金钱//
		DIAMOND = 2,		// 钻石//
		ACTING = 3,			// 演技//
		CHARM = 4			// 魅力//
	}
	
	public UIScrollView FriendPanel;
	public UITable FriendGrid;
	public GameObject FriendItem;
	
	public class StorageTaskInfo
	{
		public int taskId;
		public bool UseMagnifier;
		public bool UseSlowMotion;
		public List<int> PosInfo;
		public int PassCount;
		public int ShareCount;
		public float DifficultyNum;
		public int totalScore;
	}
	public class StorageFriendInfo
	{
		public long friendId;
		public int friendType;
		public int friendGender;
		public string friednAppearance;
		public string friendCloth;
		public int friendActing;
	}
	
	int taskID;

	sg.GS2C_Task_Invite_List_Res mTaskInviteList;
	Dictionary<int,GameObject> mMissionItemList = new Dictionary<int, GameObject>();
	int mRefreshTaskSpendMoney = 0;
	int mRefreshTaskCount = 0;
	
	public int TaskID
	{
		get
		{
			return taskID;
		}
	}
	
	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		
		base.Awake();
		base.enabled = true;
		UIEventListener.Get(ButtonMap.gameObject).onClick += OnClickButtonMap;
	}
	
	protected virtual void Start ()
	{
		base.Start();
	}
	
	protected override void OnDestroy()
	{
		base.OnDestroy();
		
		mMissionItemList.Clear();
			
		PortStatus port = ((PortStatus)GameStatusManager.Instance.MCurrentGameStatus);
		if(port != null)
			port.orbitCamera.SetRotationStatus(true);
	}

	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		this.GUILevel = 10;
		
		DailyTaskBtnNull.transform.localScale = Vector3.zero;
		
		StorageFriendInfo friendInfo = new StorageFriendInfo();
		friendInfo.friendId = -1;
		friendInfo.friendType = -1;
		friendInfo.friendGender = 0;
		friendInfo.friednAppearance = null;
		friendInfo.friendCloth = null;
		friendInfo.friendActing = 0;
		Globals.Instance.MTaskManager.mTaskDailyData.FriendInfo = friendInfo;
		
		PortStatus port = ((PortStatus)GameStatusManager.Instance.MCurrentGameStatus);
		port.orbitCamera.SetRotationStatus(false);
	}
	
	public void ShowMissionList(sg.GS2C_Enter_City_Res res)
	{
		mMissionItemList.Clear();
		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
		this.SetVisible(true);
		if(res.cityId == 9)
		{
			CityInforDaily.text = res.taskLis.Count.ToString()+"/"+20;
			NGUITools.SetActive(CityInfor.gameObject,false);
			NGUITools.SetActive(CityInforDaily.gameObject,true);
		}
		else
		{
			NGUITools.SetActive(CityInforDaily.gameObject,false);
			NGUITools.SetActive(CityInfor.gameObject,true);
		}
		NGUITools.SetActive(FriendsView,false);
		NGUITools.SetActive(NoMissionInfo,false);
		
		mRefreshTaskSpendMoney = res.refreshMoney;
		mRefreshTaskCount = res.refreshCount;
		Globals.Instance.MTaskManager.mTaskDailyData.CurrentCityID = res.cityId;
		Map_Citys MapCity = Globals.Instance.MDataTableManager.GetConfig<Map_Citys>();
		Map_Citys.Map_CitysObject MapObj = MapCity.GetMap_CitysObjectByID(res.cityId);
		if(MapObj == null)
			return;
		
		CityNameLabel.text = MapObj.Citys_Name;
		MissionNumLabel.text = res.taskLis.Count.ToString();
		HelpUtil.DelListInfo(MainTable.transform);
		
		foreach(sg.GS2C_Enter_City_Res.completedTask completetask in res.compLis)
		{
			GameObject completeMissionObj = GameObject.Instantiate(CompleteMissionItemPrefab)as GameObject;
			completeMissionObj.transform.parent = MainTable.transform;
			completeMissionObj.transform.localScale = Vector3.one;
			completeMissionObj.transform.localPosition = new Vector3(0,0,0);
			GameObject completeMissionBase = completeMissionObj.transform.Find("MissionBase").gameObject;
			GameObject completeStars = completeMissionBase.transform.Find("Stars").gameObject;
			GameObject completeTween = completeMissionObj.transform.Find("Tween").gameObject;
			NGUITools.SetActive(completeTween.gameObject,false);
			//基础信息-----//
			UISprite rankSprite =  completeMissionBase.transform.Find("Rank").GetComponent<UISprite>();
			UISprite PassSprite =  completeMissionBase.transform.Find("Pass").GetComponent<UISprite>();
			UISprite SexSprite =  completeMissionBase.transform.Find("SexSprite").GetComponent<UISprite>();
			UILabel JingLiNumLabel = completeMissionBase.transform.Find("JingLiNumLabel").GetComponent<UILabel>();
			UILabel NameLabel = completeMissionBase.transform.Find("NameLabel").GetComponent<UILabel>();
			UIButton DareButton = completeMissionBase.transform.Find("DareButton").GetComponent<UIButton>();
			//详细信息---//
			UILabel completeMissionBriefing= completeTween.transform.Find("MissionBriefingLabel").GetComponent<UILabel>();
			UILabel completeMoneyLabel = completeTween.transform.Find("MoneyLabel").GetComponent<UILabel>();
			UILabel completeFansLabel = completeTween.transform.Find("FansLabel").GetComponent<UILabel>();
			UILabel completeNoRewardLabel = completeTween.transform.Find("NoRewardLabel").GetComponent<UILabel>();
			UIButton completeRankButton = completeTween.transform.Find("RankButton").GetComponent<UIButton>();
			
			Task comtask = Globals.Instance.MDataTableManager.GetConfig<Task>();
			Task.TaskObject completeElement = null;
			bool hasData = comtask.GetTaskObject(completetask.taskId, out completeElement);
			if (!hasData)
			{
				Debug.Log(" completetask  ================  Null" +completetask.taskId );
				return;
			}
			completeMissionObj.name = "AMission"+completetask.taskId;
			rankSprite.spriteName = HangyeDengji[FinalEvaluation(completetask.level)];
			PassSprite.spriteName = FinalEvaluation(completetask.level) == 4 ? "IconManfen":"IconPass";
			SexSprite.spriteName = completeElement.Sex_Icon;

			NameLabel.text = completeElement.Name;
			completeMissionBriefing.text = completeElement.Task_Desc;
			
			if(FinalEvaluation(completetask.level) == 0&&completeElement.Progress_Count <= 0)
			{
				NGUITools.SetActive(completeStars,false);
				NGUITools.SetActive(rankSprite.gameObject,false);
			}
			else
			{
				string[] stars = {"Green","Blue","Purple","Red","Yellow"};
				string[] starBg = {"GreenBg","BlueBg","PurpleBg","RedBg","YellowBg"};
				for(int i = 0; i < 5;i++)
				{
					UISprite currentStar = completeStars.transform.Find(stars[i]).GetComponent<UISprite>();
					UISprite currentStarbg = completeStars.transform.Find(starBg[i]).GetComponent<UISprite>();
				
					if(i <= FinalEvaluation(completetask.level))
					{
						NGUITools.SetActive(currentStarbg.gameObject,false);
						NGUITools.SetActive(currentStar.gameObject,true);
					}else
					{
						NGUITools.SetActive(currentStarbg.gameObject,true);
						NGUITools.SetActive(currentStar.gameObject,false);
					}
				}
			}

			
			NGUITools.SetActive(completeMoneyLabel.gameObject,false);
			NGUITools.SetActive(completeFansLabel.gameObject,false);
			NGUITools.SetActive(completeNoRewardLabel.gameObject,false);
			List<string[]> RewardArray = ParsingRewards(completeElement.Rewards);
			if(!completetask.costEnergy)
			{
				NGUITools.SetActive(JingLiNumLabel.gameObject,false);
				NGUITools.SetActive(completeNoRewardLabel.gameObject,true);
			}else
			{
				JingLiNumLabel.text = "-"+completeElement.Need_Energy.ToString();
				NGUITools.SetActive(JingLiNumLabel.gameObject,true);
				
				bool isReward = false;
				for(int i = 0;i < RewardArray.Count; i++)
				{
					switch(StrParser.ParseDecInt(RewardArray[i][0],-1))
					{
						case (int)TaskRewardTypeEnum.ROLE_ATTR:	
							switch(StrParser.ParseDecInt(RewardArray[i][1],-1))
							{
								case (int)BaseRewardType.MONEY:
									isReward = true;
									NGUITools.SetActive(completeMoneyLabel.gameObject,true);
									completeMoneyLabel.text = (StrParser.ParseDecInt(RewardArray[i][2],-1)/2.0 == StrParser.ParseDecInt(RewardArray[i][2],-1)/2? StrParser.ParseDecInt(RewardArray[i][2],-1)/2: StrParser.ParseDecInt(RewardArray[i][2],-1)/2+1).ToString();
									break;
							}
							break;
						case (int)TaskRewardTypeEnum.FANS_NUM:
							isReward = true;
							NGUITools.SetActive(completeFansLabel.gameObject,true);
							completeFansLabel.text = (StrParser.ParseDecInt(RewardArray[i][2],-1)/2.0 == StrParser.ParseDecInt(RewardArray[i][2],-1)/2? StrParser.ParseDecInt(RewardArray[i][2],-1)/2: StrParser.ParseDecInt(RewardArray[i][2],-1)/2+1).ToString();
							break;
					}
					
				}
				if(!isReward)
				{
					NGUITools.SetActive(completeNoRewardLabel.gameObject,true);
				}
			}
			
			
			DareButton.Data = completetask;
			UIEventListener.Get(DareButton.gameObject).onClick += OnClickChallengeAgain;
		}
		
		
		foreach(sg.GS2C_Enter_City_Res.visibleTask taskInfo in res.taskLis )
		{
			GameObject MissionObj = GameObject.Instantiate(MissionItemPrefab)as GameObject;
			MissionObj.transform.parent = MainTable.transform;
			MissionObj.transform.localScale = Vector3.one;
			MissionObj.transform.localPosition = new Vector3(0,0,0);
			UIToggle Toggle = MissionObj.transform.GetComponent<UIToggle>();
			GameObject MissionBase = MissionObj.transform.Find("MissionBase").gameObject;
			GameObject Tween = MissionObj.transform.Find("Tween").gameObject;
			UISprite Unlocked = MissionObj.transform.Find("Unlocked").GetComponent<UISprite>();
			
			//基础信息-----//
			UITexture GradeTexture =  MissionBase.transform.Find("GradeTexture").GetComponent<UITexture>();
			UISprite IssuedSprite = MissionBase.transform.Find("IssuedSprite").GetComponent<UISprite>();
			UISprite SexSprite =  MissionBase.transform.Find("SexSprite").GetComponent<UISprite>();
			UILabel JingLiNumLabel = MissionBase.transform.Find("JingLiNumLabel").GetComponent<UILabel>();
			UILabel NameLabel = MissionBase.transform.Find("NameLabel").GetComponent<UILabel>();
			UIButton DareButton = MissionBase.transform.Find("DareButton").GetComponent<UIButton>();
			
			//详细信息---//
			UILabel MissionBriefingLabel = Tween.transform.Find("MissionBriefingLabel").GetComponent<UILabel>();
			UILabel MoneyLabel = Tween.transform.Find("MoneyLabel").GetComponent<UILabel>();
			UILabel FansLabel = Tween.transform.Find("FansLabel").GetComponent<UILabel>();
			UILabel DiamondLabel = Tween.transform.Find("DiamondLabel").GetComponent<UILabel>();
			UILabel NoRewardLabel = Tween.transform.Find("NoRewardLabel").GetComponent<UILabel>();
			UITexture RewardItemOne = Tween.transform.Find("RewardItemOne").GetComponent<UITexture>();
			UITexture RewardItemTwo = Tween.transform.Find("RewardItemTwo").GetComponent<UITexture>();
			UIButton ChangeButton = Tween.transform.Find("ChangeButton").GetComponent<UIButton>();
		
			
			
			//未解锁信息--//
			UILabel LockLabel = Unlocked.transform.Find("LockLabel").GetComponent<UILabel>();
			UILabel TimeLabel = Unlocked.transform.Find("TimeLabel").GetComponent<UILabel>();
			
			Task task = Globals.Instance.MDataTableManager.GetConfig<Task>();
			Task.TaskObject element = null;
			bool hasData = task.GetTaskObject(taskInfo.taskId, out element);
			if (!hasData)
			{
				Debug.Log(" Task  ================  Null" +taskInfo.taskId );
				return;
			}
			
			
			SexSprite.spriteName = element.Sex_Icon;		
			
			if(element.Task_Category == (int)TaskCategoryEnum.DAILY)
			{
				NGUITools.SetActive(ChangeButton.gameObject,true);
			}else
			{
				NGUITools.SetActive(ChangeButton.gameObject,false);
			}
			
			if(element.Publish_Type == -1)
			{
				NGUITools.SetActive(IssuedSprite.gameObject,false);
			}else
			{
				NGUITools.SetActive(IssuedSprite.gameObject,true);
			}

			NameLabel.text = element.Name;
			JingLiNumLabel.text = "-"+element.Need_Energy.ToString();
			
			NGUITools.SetActive(Tween.gameObject,false);
			if(taskInfo.isLock)
			{
				MissionObj.name = "CMission"+taskInfo.taskId;
				NGUITools.SetActive(Unlocked.gameObject,true);
				
				BoxCollider BtnBox =  DareButton.gameObject.transform.GetComponent<BoxCollider>();
				Destroy(BtnBox);
				// 解锁条件 -- //
				if(taskInfo.remainTime > 0)
				{
					TimeLabel.text = taskInfo.remainTime.ToString();
				}else
				{
					NGUITools.SetActive(TimeLabel.gameObject,false);
				}
				
				
				foreach(sg.BI_Condition condition in taskInfo.unlockConditions)
				{
					switch(condition.type)
					{
						case (int)TaskRequireVisibleTypeEnum.SEXLIMIT:
							LockLabel.text = "1";
							break;
						case (int)TaskRequireVisibleTypeEnum.ITEMCOUNT:
							LockLabel.text = "2";
							break;
						case (int)TaskRequireVisibleTypeEnum.BEFORETASKID:
							
							Task.TaskObject Specify = null;
							bool SpecifyBool = task.GetTaskObject(condition.itemId, out Specify);
							if (!SpecifyBool)
								return;
							LockLabel.text = Specify.Name;
							break;
						case (int)TaskRequireVisibleTypeEnum.ROLEATTR:
							break;
						case (int)TaskRequireVisibleTypeEnum.HAVEARTIST:
							break;
						case (int)TaskRequireVisibleTypeEnum.FANSNUM:
							break;
					}
				}
				
				UIButton btn = MissionObj.transform.GetComponent<UIButton>();
				btn.onClick.Clear();
				TweenHeight th = MissionObj.transform.GetComponent<TweenHeight>();
				Destroy(th);
				TweenRotation tr = MissionObj.transform.Find("Picture").transform.Find("ArrowObject").transform.GetComponent<TweenRotation>();
				Destroy(tr);
			}else
			{
				int RewardItemState = 0;
				MissionObj.name = "BMission"+taskInfo.taskId;
				NGUITools.SetActive(Unlocked.gameObject,false);	
				//已经解锁、 详细信息赋值//
				
				MissionBriefingLabel.text = element.Task_Desc;
				NGUITools.SetActive(MoneyLabel.gameObject,false);
				NGUITools.SetActive(FansLabel.gameObject,false);
				NGUITools.SetActive(DiamondLabel.gameObject,false);
				NGUITools.SetActive(RewardItemOne.gameObject,false);
				NGUITools.SetActive(RewardItemTwo.gameObject,false);
				NGUITools.SetActive(NoRewardLabel.gameObject,false);
				
				
				List<string[]> RewardArray = ParsingRewards(element.Rewards);
				if(RewardArray.Count <= 0)
				{
					NGUITools.SetActive(NoRewardLabel.gameObject,true);
				}else
				{
					for(int i = 0;i < RewardArray.Count; i++)
					{
						
					}
				}
			}
			Toggle.Data = taskInfo.isLock;
			UIEventListener.Get(Toggle.gameObject).onClick += OnClickToggle;
			ChangeButton.Data = taskInfo.taskId;
			UIEventListener.Get(ChangeButton.gameObject).onClick += OnClickChangeButton;
			DareButton.Data = taskInfo.taskId;
			UIEventListener.Get(DareButton.gameObject).onClick += OnClickDareButton;
			
			mMissionItemList.Add(taskInfo.taskId,MissionObj);
		}
		
		MainTable.repositionNow = true;
//		MainScrollView.ResetPosition();//
		
		int completeNum = res.compLis.Count;
		int unfinishNum = res.taskLis.Count;
		if((completeNum+unfinishNum)*300 > 1482)
		{
			if(unfinishNum*300 > 1482)
			{
				MainScrollView.Press(true);
				SpringPanel.Begin(MainScrollView.gameObject,new Vector3(-8,completeNum*300+738,0),13);
			}
			else
			{
				MainScrollView.Press(true);
				SpringPanel.Begin(MainScrollView.gameObject,new Vector3(-8,(completeNum*300+unfinishNum*300- 744),0),13);
			}
		}

		
		if(Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId != 0)
		{
	
		}
		
	}
	
	private void StartNextTask(int id)
	{

		Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId = 0;
	}
	
	private List<string[]> ParsingRewards(string Rewards)
	{
		List<string[]> 	RewardType = new List<string[]>();
		if(Rewards != "")
		{
			string[] RewardInfo = Rewards.Split(',');
			for(int i = 0; i < RewardInfo.Length;i++)
			{
				RewardType.Add(RewardInfo[i].Split(':'));
			}
		}

		return RewardType;
	}
	
	
	public void NoMissionShow()
	{
		NGUITools.SetActive(MainMission,false);
		NGUITools.SetActive(FriendsView,false);
		NGUITools.SetActive(NoMissionInfo,true);
		GameObject NoInfo = NoMissionInfo.transform.Find("CityInfoMation").gameObject;
		UILabel CityName = NoInfo.transform.Find("CityNameLabel").GetComponent<UILabel>();
		UILabel NumLabel = NoInfo.transform.Find("MissionNumLabel").GetComponent<UILabel>();
		UILabel TimeLabel = NoInfo.transform.Find("TimeLabel").GetComponent<UILabel>();
		
		Map_Citys MapCity = Globals.Instance.MDataTableManager.GetConfig<Map_Citys>();
		Map_Citys.Map_CitysObject MapObj = MapCity.GetMap_CitysObjectByID(Globals.Instance.MTaskManager.mTaskDailyData.CurrentCityID);
		if(MapObj == null)
			return;
		CityName.text = MapObj.Citys_Name;
		NumLabel.text = "0";
	}
	public void ShowFriendsList(sg.GS2C_Task_Invite_List_Res res)
	{
		mTaskInviteList = res;
		this.SetVisible(true);
		NGUITools.SetActive(MissionView,false);
		NGUITools.SetActive(FriendsView,true);
		
		taskID = res.taskId;
		UILabel MissionName = FriendsView.transform.Find("MissionName").gameObject.transform.Find("MissionNameLabel").GetComponent<UILabel>();
		Task tk = Globals.Instance.MDataTableManager.GetConfig<Task>();
		Task.TaskObject element = null;
		bool hasData = tk.GetTaskObject(res.taskId, out element);
		if (!hasData)
			return;
		MissionName.text = element.Name;
		HelpUtil.DelListInfo(FriendGrid.transform);
		
		foreach(sg.GS2C_Task_Invite_List_Res.canInviteFriend friend in res.friendLis)
		{
			GameObject obj = GameObject.Instantiate(FriendItem)as GameObject;
			obj.transform.parent = FriendGrid.transform;
			obj.transform.localScale = Vector3.one;
			obj.transform.localPosition = new Vector3(0,0,-5);
			
			if(friend.friendType == (int)JobManager.InviteType.Pet)
			{
				obj.name = "AA"+ friend.friendType;
			}else if(friend.friendType == (int)JobManager.InviteType.NPC)
			{
				obj.name = "BB"+ friend.friendType;
			}else if(friend.friendType == (int)JobManager.InviteType.FRIEND)
			{
				obj.name = "CC"+ friend.friendType;
			}else if(friend.friendType == (int)JobManager.InviteType.NEARBY)
			{
				obj.name = "DD"+ friend.friendType;
			}
			
			GameObject AllLabel = obj.transform.Find("AllLabel").gameObject;
			GameObject Details = obj.transform.Find("Tween").transform.Find("GameObject").transform.Find("Details").gameObject;
			UITexture FriendIcon = AllLabel.transform.Find("FriendIcon").GetComponent<UITexture>();
			UILabel NameLabel = AllLabel.transform.Find("NameLabel").GetComponent<UILabel>();
			UIButton AddFriendButton = AllLabel.transform.Find("AddFriendButton").GetComponent<UIButton>();
			UIButton CheckButton = AllLabel.transform.Find("CheckButton").GetComponent<UIButton>();
			UISprite mFriendSign = AllLabel.transform.Find("Friendsign").GetComponent<UISprite>();
			UILabel FriendshipLabel = AllLabel.transform.Find("FriendshipLabel").GetComponent<UILabel>();
			
			UIEventListener.Get(AddFriendButton.gameObject).onClick += OnClickAddFriendButton;
			UIEventListener.Get(CheckButton.gameObject).onClick += OnClickCheckButton;
			
			UILabel ActingLabel = Details.transform.Find("ActingLabel").GetComponent<UILabel>();
			UILabel FansLabel = Details.transform.Find("FansLabel").GetComponent<UILabel>();
			if(friend.friendType == (int)JobManager.InviteType.Pet)
			{
				PetInfoMation PetInfo = Globals.Instance.MGameDataManager.MActorData.PetInfo;
				ActingLabel.text = (PetInfo.baseActing+PetInfo.incActing).ToString();
				FansLabel.text = "0";
				NameLabel.text = PetInfo.petName;
				NGUITools.SetActive(AddFriendButton.gameObject,false);		
				NGUITools.SetActive(mFriendSign.gameObject,false);
				FriendshipLabel.text = "10";
				AddFriendButton.Data = friend;
				CheckButton.Data = friend;
				FriendIcon.mainTexture = Resources.Load("Icon/AvatarIcon/Npc12001",typeof(Texture2D))as Texture2D;
			}else
			{
				ActingLabel.text = friend.actSkill.ToString();
				FansLabel.text = friend.fans.ToString();
				NameLabel.text = friend.friendName;
				if(friend.friendType == (int)JobManager.InviteType.NEARBY)
				{
					NGUITools.SetActive(AddFriendButton.gameObject,true);
					NGUITools.SetActive(mFriendSign.gameObject,false);
					FriendshipLabel.text = "5";
				}else
				{
					NGUITools.SetActive(AddFriendButton.gameObject,false);
					NGUITools.SetActive(mFriendSign.gameObject,true);
					FriendshipLabel.text = "10";
				}
				AddFriendButton.Data = friend;
				CheckButton.Data = friend;
				if(friend.friendIcon != "")
				{
					FriendIcon.mainTexture = Resources.Load("Icon/AvatarIcon/"+friend.friendIcon,typeof(Texture2D))as Texture2D;
				}
			}

			UIButton NextButton = obj.transform.Find("Tween").transform.Find("GameObject").transform.Find("NextButton").GetComponent<UIButton>();
			NextButton.Data = friend;
			UIEventListener.Get(NextButton.gameObject).onClick += OnClickNextButton;
			NGUITools.SetActive(obj.transform.Find("Tween").gameObject,false);
			
			UISprite cost = NextButton.gameObject.transform.Find("Sprite").GetComponent<UISprite>();
			UILabel label = NextButton.gameObject.transform.Find("Label").GetComponent<UILabel>();
			if(friend.friendType != (int)JobManager.InviteType.FRIEND&&friend.friendType != (int)JobManager.InviteType.NEARBY)
			{
				NPCConfig Config = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
				NPCConfig.NPCObject NpcObj = null;
				bool isHas = Config.GetNPCObject((int)friend.friendId,out NpcObj);
				if(!isHas)
				{
					NGUITools.SetActive(cost.gameObject,false);
					label.transform.localPosition = Vector3.zero;
					continue;
				}
				
				if(NpcObj.Cost_Money != -1)
				{
					cost.spriteName = "IconJinqian";
				}else if(NpcObj.Cost_Ingot != -1)
				{
					cost.spriteName = "IconZuanshi";
				}else
				{
					NGUITools.SetActive(cost.gameObject,false);
					label.transform.localPosition = Vector3.zero;	
				}
			}
		}
		FriendGrid.repositionNow = true;
		
	}
	
	private void OnClickAddFriendButton(GameObject obj)
	{
		UIButton AddBtn = obj.transform.GetComponent<UIButton>();
		sg.GS2C_Task_Invite_List_Res.canInviteFriend Friend = (sg.GS2C_Task_Invite_List_Res.canInviteFriend)AddBtn.data;
		string FriendName = Friend.friendName;
		long RoleId = Friend.friendId;

	}
	private void OnClickCheckButton(GameObject obj)
	{

	}
	
	private void OnClickNextButton(GameObject obj)
	{
		UIButton btn = obj.transform.GetComponent<UIButton>();
		sg.GS2C_Task_Invite_List_Res.canInviteFriend friend = (sg.GS2C_Task_Invite_List_Res.canInviteFriend)btn.Data;
		StorageFriendInfo friendInfo = new StorageFriendInfo();
		friendInfo.friendId = friend.friendId;
		friendInfo.friendType = friend.friendType;
		friendInfo.friendGender = friend.friendGender;
		friendInfo.friednAppearance = friend.friednAppearance;
		if(friend.friendType == (int)JobManager.InviteType.Pet)
		{
			friendInfo.friendCloth =  Globals.Instance.MGameDataManager.MActorData.PetInfo.itemId.ToString();
		}else
			friendInfo.friendCloth = friend.friendCloth;
		friendInfo.friendActing = friend.actSkill;
		Globals.Instance.MTaskManager.mTaskDailyData.FriendInfo = friendInfo;
		
		NPCConfig Config = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
		NPCConfig.NPCObject NpcObj = null;
		bool isHas = Config.GetNPCObject((int)friend.friendId,out NpcObj);
		if(!isHas)
		{
			NetSender.Instance.RequestTaskInviteReq(taskID,(int)friend.friendId,friend.friendType);
			return;
		}
		if(friend.friendType != (int)JobManager.InviteType.FRIEND&&friend.friendType != (int)JobManager.InviteType.NEARBY)
		{
			if(NpcObj.Cost_Money != -1)
			{
				Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui){
					gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
					gui.SetDialogType(EDialogType.CommonType, null);
					string flag = string.Format(Globals.Instance.MDataTableManager.GetWordText(4014),NpcObj.Cost_Money,Globals.Instance.MDataTableManager.GetWordText(4015));
					gui.SetText(flag);
				},EDialogStyle.DialogOkCancel,delegate() {
					NetSender.Instance.RequestTaskInviteReq(taskID,(int)friend.friendId,friend.friendType);
				});
				
			}else if(NpcObj.Cost_Ingot != -1)
			{
				Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui){
					gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
					gui.SetDialogType(EDialogType.CommonType, null);
					string flag = string.Format(Globals.Instance.MDataTableManager.GetWordText(4014),NpcObj.Cost_Ingot,Globals.Instance.MDataTableManager.GetWordText(4016));
					gui.SetText(flag);
				},EDialogStyle.DialogOkCancel,delegate() {
					NetSender.Instance.RequestTaskInviteReq(taskID,(int)friend.friendId,friend.friendType);
				});
			}else
			{
				NetSender.Instance.RequestTaskInviteReq(taskID,(int)friend.friendId,friend.friendType);
			}
		}
	}
	
	public void StartTask()
	{
		GUIGuoChang.Show();
		Globals.Instance.MGUIManager.CreateWindow<GUIPhotoGraph>(delegate(GUIPhotoGraph gui)
		{	
			GUIGuoChang.SetTweenPlay(0,delegate() {
				
			});

			
			this.IsReturnMainScene = false;
			this.Close();
			gui.DrawReadyView();
	
		});
	}
	
	public void InviteFriendsFail()
	{
		ShowFriendsList(mTaskInviteList);
		GUIMain main = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
		if(main != null)
		{
			main.SetVisible(false);
		}

	}
	
	private void OnClickToggle(GameObject obj)
	{
		UIToggle tog = obj.transform.GetComponent<UIToggle>();
		bool isLock = (bool)tog.Data;
		
		
		
		
	}
	
	private void OnClickChangeButton(GameObject obj)
	{
		UIButton change = obj.GetComponent<UIButton>();
		Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui){
			gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
			gui.SetDialogType(EDialogType.CommonType, null);
			string flag = "";
			if(mRefreshTaskSpendMoney > 0)
			{
				flag = string.Format(Globals.Instance.MDataTableManager.GetWordText(4021),mRefreshTaskSpendMoney);
			}
			else
			{
				flag = Globals.Instance.MDataTableManager.GetWordText(4022);
			}
			gui.SetText(flag);	
			},EDialogStyle.DialogOkCancel,delegate() {
//				NetSender.Instance.RequestTaskDailyGetRandom((int)change.Data);
			}
		);
	}
	
//	public void RefreshTaskRes(sg.GS2C_TaskDaily_GetRandom_Res res)
//	{
//		for(int i = 0; i < MainTable.transform.childCount; i++)
//		{
//			Transform item = MainTable.transform.GetChild(i);
//			TweenHeight isUpdate = item.gameObject.GetComponent<TweenHeight>();
//			if(isUpdate != null)
//			{
//				isUpdate.updateTable = false;
//			}
//		}
//		
//		DailyTaskBtnNull.transform.localScale = Vector3.one;
//		mRefreshTaskSpendMoney = res.refreshCost;
//		mRefreshTaskCount = res.refreshCount;
//		GameObject oldTask = mMissionItemList[res.taskId];
//		Vector3 oldTaskPos = oldTask.transform.localPosition;
//		
//		GameObject MissionObj = GameObject.Instantiate(oldTask )as GameObject;
//		MissionObj.transform.parent = MainTable.transform;
//		MissionObj.transform.localScale = Vector3.one;
//		MissionObj.transform.localPosition = new Vector3(oldTaskPos.x+1300,oldTaskPos.y,oldTaskPos.z);
//		GameObject MissionBase = MissionObj.transform.FindChild("MissionBase").gameObject;
//		GameObject Tween = MissionObj.transform.FindChild("Tween").gameObject;
//		UISprite Unlocked = MissionObj.transform.FindChild("Unlocked").GetComponent<UISprite>();
//		NGUITools.SetActive(Unlocked.gameObject,false);
//		
//		oldTask.transform.parent = MainScrollView.transform;
//		Destroy(oldTask.GetComponent<UIPlayTween>());
//		Destroy(oldTask.GetComponent<UIButton>());
//		Destroy(oldTask.GetComponent<UIToggle>());
//		Destroy(oldTask.GetComponent<TweenHeight>());
//		
//		
//		//基础信息-----//
//		UITexture GradeTexture =  MissionBase.transform.FindChild("GradeTexture").GetComponent<UITexture>();
//		UISprite IssuedSprite = MissionBase.transform.FindChild("IssuedSprite").GetComponent<UISprite>();
//		UISprite SexSprite =  MissionBase.transform.FindChild("SexSprite").GetComponent<UISprite>();
//		UILabel JingLiNumLabel = MissionBase.transform.FindChild("JingLiNumLabel").GetComponent<UILabel>();
//		UILabel NameLabel = MissionBase.transform.FindChild("NameLabel").GetComponent<UILabel>();
//		UIButton DareButton = MissionBase.transform.FindChild("DareButton").GetComponent<UIButton>();
//		
//		//详细信息---//
//		UILabel MissionBriefingLabel = Tween.transform.FindChild("MissionBriefingLabel").GetComponent<UILabel>();
//		UILabel MoneyLabel = Tween.transform.FindChild("MoneyLabel").GetComponent<UILabel>();
//		UILabel FansLabel = Tween.transform.FindChild("FansLabel").GetComponent<UILabel>();
//		UILabel DiamondLabel = Tween.transform.FindChild("DiamondLabel").GetComponent<UILabel>();
//		UILabel NoRewardLabel = Tween.transform.FindChild("NoRewardLabel").GetComponent<UILabel>();
//		UITexture RewardItemOne = Tween.transform.FindChild("RewardItemOne").GetComponent<UITexture>();
//		UITexture RewardItemTwo = Tween.transform.FindChild("RewardItemTwo").GetComponent<UITexture>();
//		UIButton ChangeButton = Tween.transform.FindChild("ChangeButton").GetComponent<UIButton>();
//		
//		mMissionItemList.Add(res.newTaskId,MissionObj);
//		Task task = Globals.Instance.MDataTableManager.GetConfig<Task>();
//		Task.TaskObject element = null;
//		bool hasData = task.GetTaskObject(res.newTaskId, out element);
//		if (!hasData)
//		{
//			Debug.Log(" Task  ================  Null" +res.newTaskId );
//			return;
//		}
//		SexSprite.spriteName = element.Sex_Icon;		
//		
//		if(element.Task_Category == (int)TaskCategoryEnum.DAILY)
//		{
//			NGUITools.SetActive(ChangeButton.gameObject,true);
//		}else
//		{
//			NGUITools.SetActive(ChangeButton.gameObject,false);
//		}
//		
//		if(element.Publish_Type == -1)
//		{
//			NGUITools.SetActive(IssuedSprite.gameObject,false);
//		}else
//		{
//			NGUITools.SetActive(IssuedSprite.gameObject,true);
//		}
//
//		NameLabel.text = element.Name;
//		JingLiNumLabel.text = "-"+element.Need_Energy.ToString();
//		
//		IssueItem iss = MainTable.GetComponent<IssueItem>();
//		iss.SetCurrentObj(MissionObj,Tween);
//		UIPlayTween[] tween = MissionObj.GetComponents<UIPlayTween>();
//		tween[0].Play(false);
//		tween[1].Play(false);
//		
//		
//		int RewardItemState = 0;
//		MissionObj.name = "BMission"+res.newTaskId;
//		//已经解锁、 详细信息赋值//
//		
//		MissionBriefingLabel.text = element.Task_Desc;
//		NGUITools.SetActive(MoneyLabel.gameObject,false);
//		NGUITools.SetActive(FansLabel.gameObject,false);
//		NGUITools.SetActive(DiamondLabel.gameObject,false);
//		NGUITools.SetActive(RewardItemOne.gameObject,false);
//		NGUITools.SetActive(RewardItemTwo.gameObject,false);
//		NGUITools.SetActive(NoRewardLabel.gameObject,false);
//		
//		
//		List<string[]> RewardArray = ParsingRewards(element.Rewards);
//		if(RewardArray.Count <= 0)
//		{
//			NGUITools.SetActive(NoRewardLabel.gameObject,true);
//		}else
//		{
//			for(int i = 0;i < RewardArray.Count; i++)
//			{
//				switch(StrParser.ParseDecInt(RewardArray[i][0],-1))
//				{
//					case (int)TaskRewardTypeEnum.PUBLISH:
//					
//						PublishConfig publish = Globals.Instance.MDataTableManager.GetConfig<PublishConfig>();
//						PublishConfig.MagazineObj publishObj = null;
//						bool isHas = publish.GetMagazineObject(StrParser.ParseDecInt(RewardArray[i][1],-1),out publishObj);
//						if(!isHas)
//						{
//							Debug.Log("Publish does not exist   ID is " + StrParser.ParseDecInt(RewardArray[i][1],-1));
//							return;
//						}
//			
//						if(RewardItemState == 0)
//						{
//							RewardItemState = 1;
//							NGUITools.SetActive(RewardItemOne.gameObject,true);
//							RewardItemOne.mainTexture = Resources.Load("Icon/PublishIcon/"+publishObj.Icon,typeof(Texture2D)) as Texture2D;
//						}else
//						{
//							NGUITools.SetActive(RewardItemTwo.gameObject,true);
//							RewardItemTwo.mainTexture = Resources.Load("Icon/PublishIcon/"+publishObj.Icon,typeof(Texture2D)) as Texture2D;
//						}
//					
//					
//						break;
//					case (int)TaskRewardTypeEnum.ITEM: 	
//						
//						ItemConfig item = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
//						ItemConfig.ItemElement ItemEle = null;
//						if (!item.GetItemElement(StrParser.ParseDecInt(RewardArray[i][1],-1), out ItemEle))
//						{
//							Debug.Log(StrParser.ParseDecInt(RewardArray[i][1],-1)+" ==  Null");
//							return;	
//						}
//							
//						if(RewardItemState == 0)
//						{
//							RewardItemState = 1;
//							NGUITools.SetActive(RewardItemOne.gameObject,true);
//							RewardItemOne.mainTexture =  Resources.Load("Icon/ItemIcon/"+ItemEle.Icon,typeof(Texture2D)) as Texture2D;
//						}else
//						{
//							NGUITools.SetActive(RewardItemTwo.gameObject,true);
//							RewardItemTwo.mainTexture =  Resources.Load("Icon/ItemIcon/"+ItemEle.Icon,typeof(Texture2D)) as Texture2D;
//						}
//						break;
//					case (int)TaskRewardTypeEnum.POSTURE: 
//								
//						Pos_Score pos_score = Globals.Instance.MDataTableManager.GetConfig<Pos_Score>();
//						Pos_Score.PosObject GetPos= null;
//						if (! pos_score.GetTaskObject(StrParser.ParseDecInt(RewardArray[i][1],-1), out GetPos))
//						{
//							Debug.Log(StrParser.ParseDecInt(RewardArray[i][1],-1) + " ==  Null   PosID");
//							return;
//						}
//							
//						if(RewardItemState == 0)
//						{
//							RewardItemState =1;
//							NGUITools.SetActive(RewardItemOne.gameObject,true);
//							RewardItemOne.mainTexture =  Resources.Load("Icon/PosIcon/" + GetPos.Pos_Icon,typeof(Texture2D)) as Texture2D;
//						}else
//						{
//							NGUITools.SetActive(RewardItemTwo.gameObject,true);
//							RewardItemTwo.mainTexture =  Resources.Load("Icon/PosIcon/"+GetPos.Pos_Icon,typeof(Texture2D)) as Texture2D;
//						}
//						break;
//					case (int)TaskRewardTypeEnum.ROLE_ATTR:	
//						switch(StrParser.ParseDecInt(RewardArray[i][1],-1))
//						{
//							case (int)BaseRewardType.MONEY:
//								NGUITools.SetActive(MoneyLabel.gameObject,true);
//								MoneyLabel.text = StrParser.ParseDecInt(RewardArray[i][2],-1).ToString();
//								break;
//							case (int)BaseRewardType.DIAMOND:
//								NGUITools.SetActive(DiamondLabel.gameObject,true);
//								DiamondLabel.text = StrParser.ParseDecInt(RewardArray[i][2],-1).ToString();
//								break;
//							case (int)BaseRewardType.ACTING:
//								break;
//							case (int)BaseRewardType.CHARM:
//								break;
//						}
//						break;
//					case (int)TaskRewardTypeEnum.GET_ARTTST:
//						WarshipConfig config = Globals.Instance.MDataTableManager.GetConfig<WarshipConfig>();
//						WarshipConfig.WarshipObject warshipObj = config.GetWarshipObjectByID(StrParser.ParseDecInt(RewardArray[i][1],-1));
//					
//				
//						break;
//					case (int)TaskRewardTypeEnum.FANS_NUM:
//						NGUITools.SetActive(FansLabel.gameObject,true);
//						FansLabel.text = StrParser.ParseDecInt(RewardArray[i][2],-1).ToString();
//						break;
//					case (int)TaskRewardTypeEnum.AREA_OPEN: 
//						break;
//					case (int)TaskRewardTypeEnum.UNKNOWNTYPE: 
//						break;
//			
//				}
//			}
//		}
//		
//		ChangeButton.Data = res.newTaskId;
//		UIEventListener.Get(ChangeButton.gameObject).onClick += OnClickChangeButton;
//		DareButton.Data = res.newTaskId;
//		UIEventListener.Get(DareButton.gameObject).onClick += OnClickDareButton;
//		
//		TweenPosition newTween = MissionObj.AddComponent<TweenPosition>();
//		newTween.from = new Vector3(oldTaskPos.x+1300,oldTaskPos.y,oldTaskPos.z) ;
//		newTween.to = oldTaskPos;
//		newTween.duration = 0.6f;
//		EventDelegate.Add(newTween.onFinished,delegate() {
//			Destroy(newTween);
//			DailyTaskBtnNull.transform.localScale = Vector3.zero;
//			
//			for(int i = 0; i < MainTable.transform.childCount; i++)
//			{
//				Transform item = MainTable.transform.GetChild(i);
//				TweenHeight itemTween = item.gameObject.GetComponent<TweenHeight>();
//				if(itemTween != null)
//				{
//					itemTween.updateTable = true;
//				}
//			}
//		},true);
//		
//		TweenPosition oldTween = oldTask.AddComponent<TweenPosition>();
//		oldTween.from = oldTask.transform.localPosition;
//		oldTween.to = new Vector3(oldTask.transform.localPosition.x-1300,oldTask.transform.localPosition.y,oldTask.transform.localPosition.z);
//		oldTween.duration = 0.4f;
//		NGUITools.SetActive( oldTask.transform.FindChild("Tween").gameObject,true);
//		EventDelegate.Add(oldTween.onFinished,delegate() {
//			mMissionItemList.Remove(res.taskId);
//			Destroy(oldTween.gameObject);
//		},true);
//	}
//	
	void OnClickChallengeAgain(GameObject obj)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		sg.GS2C_Enter_City_Res.completedTask completetask = (sg.GS2C_Enter_City_Res.completedTask)btn.Data;
		TaskManager.ChallengeAgain again = new TaskManager.ChallengeAgain();
		again.isChallengeAgain = true;
		again.cityId = Globals.Instance.MTaskManager.mTaskDailyData.CurrentCityID;
		again.evaluationl = FinalEvaluation(completetask.level);
		again.challengeAgainScene = 1;
		Globals.Instance.MTaskManager.challengeAgain = again;
	}
	
	private void OnClickDareButton(GameObject obj)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		int taskID = (int)btn.Data;
		

		
	}
	
	public void TaskAcceptRes(sg.GS2C_Task_Accept_Res res)
	{
		if(res.talkId != null)
		{
			this.SetVisible(false);
			
			GUIMain guim = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guim != null)
			{
				guim.SetVisible(false);
			}
			
	
			GUIGuoChang.Show();
			Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(
				delegate (GUITaskTalkView gui)
				{
					gui.PlayLocalTalk(res.talkId,delegate() {
						if(Globals.Instance.MTaskManager.challengeAgain!= null){
							
							Task tk = Globals.Instance.MDataTableManager.GetConfig<Task>();
							Task.TaskObject element = null;
							bool hasData = tk.GetTaskObject(res.taskId, out element);
							if (!hasData)
								return;
							if(element.Progress_Count <= 0)
							{
								if(Globals.Instance.MTaskManager.challengeAgain.challengeAgainScene == 0)
								{
									NetSender.Instance.PlayerGetCompletedReq(Globals.Instance.MTaskManager.challengeAgain.cityId);
								}
								else if(Globals.Instance.MTaskManager.challengeAgain.challengeAgainScene == 1)
								{
									NetSender.Instance.RequestEnterCityReq(Globals.Instance.MTaskManager.mTaskDailyData.CurrentCityID);
									Globals.Instance.MTaskManager.challengeAgain = null;
								}
							}else
							{
								TaskAcceptDeal(res);
								if(guim != null)
								{
									guim.SetVisible(false);
								}
							}
							
						}else
						{
							TaskAcceptDeal(res);
							if(guim != null)
							{
								guim.SetVisible(false);
							}
						}
					});
				
					GUIGuoChang.SetTweenPlay(0,delegate() {
					
					});
				}
			);
		}else
		{
			TaskAcceptDeal(res);
		}
	}
	
	private void TaskAcceptDeal(sg.GS2C_Task_Accept_Res res)
	{
		if(!res.isSingleTask)
		{
			NetSender.Instance.RequestTaskInviteListReq(res.taskId);
		}else
		{
			
			Task.TaskObject element = null;
			Task task = Globals.Instance.MDataTableManager.GetConfig<Task>();
			bool hasData = task.GetTaskObject(res.taskId, out element);
			if (!hasData)
				return;	
			if(element.Progress_Count <= 0)
			{
				GUIGuoChang.Show();
				Globals.Instance.MGUIManager.CreateWindow<GUIPhotoGraph>(delegate(GUIPhotoGraph photo){
//					photo.WealthGroup.SetUpdateNow(false);
					NetSender.Instance.RequestTaskCompleteReq(res.taskId);
				});
			}else
			{
				this.Close();
				GUIGuoChang.Show();
				Globals.Instance.MGUIManager.CreateWindow<GUIPhotoGraph>(delegate(GUIPhotoGraph gui)
				{
					GUIGuoChang.SetTweenPlay(0,delegate() {
						
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
	}
	
	
	public void OnClickMission(GameObject Obj)
	{
		
	}
	
	
	protected void OnClickButtonMap(GameObject Obj)
	{	

	}
	
	
	int FinalEvaluation(double finishLv)
	{
		if(finishLv == 1.0)
		{
			return 0;
		}
		else if(finishLv == 1.25)
		{
			return 1;
		}
		else if(finishLv == 1.5)
		{
			return 2;
		}
		else if(finishLv == 1.75)
		{
			return 3;
		}
		else if(finishLv == 2.0)
		{
			return 4;
		}
		else
		{
			return 0;
		}
	}
	
	
	
	
	
	public void SetSprite(GameObject tween)
	{

		NGUITools.SetActive(tween.gameObject,!NGUITools.GetActive(tween.gameObject));
	}
	
}
