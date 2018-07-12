using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMemory : GUIWindowForm {

	public GameObject BaseInformation;
	public UIButton BackHomeBtn;
	
	public UIScrollView MemoryUIScrollView;
	public UIGrid MemoryUIGrid;
	public GameObject MemoryItem;

	public UILabel ProgressLabel;
	private int ProgressNum = 0;

	private List<int> mCacheList ;

	public UIButton MemoryBtn;
	public UIButton ChallengeInfoBtn;
	public GameObject MemoryInformation;

	public GameObject ChallengeInformation;
	public UIScrollView ChallengeUIScrollView;
	public UIGrid ChallengeUIGrid;
	public GameObject ChallengeTaskItem;

	public UILabel CheckpointLabel ;
	public UILabel ScoreLabel;

	public UILabel FreeNum ;

	public GameObject RankingInformation;
	public UIScrollView RankingUIScrollView;
	public UIGrid RankingUIGrid;
	public GameObject RankingItem;
	public UIButton BackBtn;
	public UILabel RankingNum;
	public UILabel PlayerName;
	public UILabel TotalScore;
	public UILabel CheckpointNum;
	public GameObject OpenChallenge;
	private bool isOpenChallenge = true;
	public GameObject PlayerInformation;
	public CharacterCustomizeOne characterCustomizeOne;
	public GameObject SceneGameObject;
	public GameObject BackLight;
	public GameObject MemoryInfo;
	public UIButton PlayerInformationBackBtn;
	public UILabel PlayerRankingNum;
	public UILabel PlayerPlayerName;
	public UILabel PlayerTotalScore;
	public UILabel PlayerCheckpointNum;
	
	private string[] HangyeDengji = {"HangyeDengjiB","HangyeDengjiA","HangyeDengjiS"};
	private TaskConfig taskConfig;
	private sg.GS2C_Ranking_List_Info_Res.RankingList mRankingInfo;

	protected override void Awake()
	{		
		if (null == Globals.Instance.MGUIManager)
			return;
		base.Awake();

//		if(Globals.Instance.MGameDataManager.MActorData.BasicData.PerfectEndPassNum < 1)
//		{
//			isOpenChallenge = false;
//			NGUITools.SetActive(OpenChallenge , true);
//		}

		UIEventListener.Get(BackHomeBtn.gameObject).onClick += delegate(GameObject go) {
			Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
			this.Close();
		};

		UIEventListener.Get(MemoryBtn.gameObject).onClick += OnClickMemoryBtn;
		UIEventListener.Get(ChallengeInfoBtn.gameObject).onClick += OnClickChallengeInfoBtn;
		UIEventListener.Get(BackBtn.gameObject).onClick += OnClickBackBtn;
		UIEventListener.Get(PlayerInformationBackBtn.gameObject).onClick += OnClickPlayerInformationBackBtn;

//		if(!Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState)
//		{
//			ChallengeInfoBtn.transform.localScale = Vector3.zero;
//			NGUITools.SetActive(ChallengeInfoBtn.gameObject , false);
//		}
//		else
//		{
//			ChallengeInfoBtn.transform.localScale = Vector3.one;
//			NGUITools.SetActive(ChallengeInfoBtn.gameObject , true);
//		}
		if (!GameDefines.Setting_ScreenQuality)
		{
			GameObject mainLight = GameObject.Find("Directional light Main");
			GameObject backLight = GameObject.Find("Directional light Back");
			if (mainLight != null)
			{
				//mainLight.SetActive(false);
				Light mL = mainLight.GetComponent<Light>();
				mL.shadows =  LightShadows.None;
			}
			
			if (backLight != null)
			{
				//backLight.SetActive(false);
			}
		}
	}
	
	protected virtual void Start ()
	{
		base.Start();
		
	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 8;

		mCacheList = new List<int>();
		if(TaskManager.GameEndingList.Count > 0)
		{
			foreach(int id in TaskManager.GameEndingList)
			{
				mCacheList.Add(id);
			}
		}

		taskConfig = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);
		ShowMemoryInfor();
	}
	 

	public void ShowMemoryInfor()
	{
		NGUITools.SetActive(BaseInformation , true);
		NGUITools.SetActive(MemoryInformation , true);
		NGUITools.SetActive(ChallengeInformation , false);
		GameEndConfig gameEndConfig = Globals.Instance.MDataTableManager.GetConfig<GameEndConfig>();
		Dictionary<int, GameEndConfig.GameEndElement> _mGameEndElementList = gameEndConfig.GetGameEndElementList();

		MemoryUIGrid.maxPerLine = ((_mGameEndElementList.Count%2) ==0) ? (_mGameEndElementList.Count/2):(_mGameEndElementList.Count/2 + 1) ;
		HelpUtil.DelListInfo(MemoryUIGrid.transform);
		int i = 0;
		foreach(KeyValuePair<int, GameEndConfig.GameEndElement> mPair in _mGameEndElementList)
		{
			GameObject memoryItem = GameObject.Instantiate(MemoryItem)as GameObject;
			memoryItem.transform.parent = MemoryUIGrid.transform;
			memoryItem.transform.localScale = Vector3.one;
			memoryItem.transform.localPosition = Vector3.zero;
			UITexture iconTexture = memoryItem.transform.Find("IconTexture").GetComponent<UITexture>();
			UITexture unlocked = memoryItem.transform.Find("Unlocked").GetComponent<UITexture>();
			iconTexture.mainTexture =  Resources.Load("Icon/EndingIcon/"+mPair.Value.End_Pic,typeof(Texture2D)) as Texture2D;
			int taskid = GetGameEndingState(mPair.Value.Get_Task);
			if(taskid > 0)
			{
				ProgressNum++;
				NGUITools.SetActive(unlocked.gameObject , false);

				memoryItem.name = "AAMemoryItem"+i;
			}
			else
			{
				NGUITools.SetActive(unlocked.gameObject , true);
				memoryItem.name = "BBMemoryItem"+i;
			}

			UIToggle btn = memoryItem.transform.GetComponent<UIToggle>();
			btn.Data = taskid;

			if(i == 0)
			{
				btn.startsActive = true;
			}

			UIEventListener.Get(btn.gameObject).onClick += OnClickGameEndBtn;

			i++;
		}

		MemoryUIGrid.sorting = UIGrid.Sorting.Custom;
		MemoryUIGrid.repositionNow = true;
		MemoryUIScrollView.ResetPosition();


		ProgressLabel.text = ProgressNum.ToString()+"/"+_mGameEndElementList.Count.ToString();
	}



	private void OnClickGameEndBtn(GameObject obj)
	{
		UIToggle btn = obj.transform.GetComponent<UIToggle>();

		int taskid = (int)btn.Data;

		if(taskid <= 0)
		{
			return ;
		}
		GUIGuoChang.Show();
		Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(delegate (GUITaskTalkView gui){

			NGUITools.SetActive(this.gameObject , false);
			TaskConfig tk = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
			TaskConfig.TaskObject element = null;
			bool hasData = tk.GetTaskObject(taskid, out element);
			if (!hasData)
				return;
			
			gui.PlayLocalTalk(element.Task_Talk_ID,delegate() 
			{
					gui.DestroyThisGUI();
					NGUITools.SetActive(this.gameObject , true);
				
			});
			
			GUIGuoChang.SetTweenPlay(0,delegate() {
				
			});
		});

	}

	private void OnClickMemoryBtn(GameObject obj)
	{
		NGUITools.SetActive(MemoryInformation , true);
		NGUITools.SetActive(ChallengeInformation , false);
	}

	private void OnClickChallengeInfoBtn(GameObject obj)
	{
		if(!isOpenChallenge)
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(7015);
			return;
		}
		NetSender.Instance.C2GSChallengeAgainInfoReq();
	}

	public void StartInfo(){
		NGUITools.SetActive(MemoryInformation , false);
		NGUITools.SetActive(MemoryBtn.gameObject , false);
		NGUITools.SetActive(ChallengeInformation , true);
		NetSender.Instance.C2GSChallengeAgainInfoReq();
	}

	public void ShowAgainChallengeInformation(sg.GS2C_Challenge_Again_Info_Res res)
	{
		NGUITools.SetActive(MemoryInformation , false);
		NGUITools.SetActive(ChallengeInformation , true);

		FreeNum.text = res.againNum.ToString();

		int playerTotalScore = 0;
		HelpUtil.DelListInfo(ChallengeUIGrid.transform);

		foreach(sg.GS2C_Challenge_Again_Info_Res.TaskCompleteInfo info in res.taskCompleteInfo)
		{
			GameObject item = GameObject.Instantiate(ChallengeTaskItem)as GameObject;
			item.transform.parent = ChallengeUIGrid.transform;
			item.transform.localScale = Vector3.one;
			item.transform.localPosition = Vector3.zero;

			item.name = "ChallengeTaskItem"+info.taskId;
			UISprite gradeSprite = item.transform.Find("GradeSprite").GetComponent<UISprite>();
			UILabel taskName = item.transform.Find("TaskName").GetComponent<UILabel>();
			UILabel totalScore = item.transform.Find("TotalScore").GetComponent<UILabel>();
			UIButton challengeBtn = item.transform.Find("ChallengeBtn").GetComponent<UIButton>();

			GameObject star = item.transform.Find("Star").gameObject;
			string[] str = {"PurpSprite","YelloSprite"};
			string[] strbg = {"PurpBgSprite","YelloBgSprite"};
			for(int i = 1;i <= FinalEvaluation(info.grade); i++)
			{
				UISprite bg = star.transform.Find(strbg[i-1]).GetComponent<UISprite>();
				UISprite sprite = star.transform.Find(str[i-1]).GetComponent<UISprite>();
				NGUITools.SetActive(bg.gameObject,false);
				NGUITools.SetActive(sprite.gameObject,true);
			}
			playerTotalScore += info.totalScore;
			gradeSprite.spriteName = HangyeDengji[FinalEvaluation(info.grade)];
			totalScore.text = info.totalScore.ToString();
			TaskConfig.TaskObject element = null;
			bool ishas = taskConfig.GetTaskObject(info.taskId, out element);
			if(!ishas)
				return;
			taskName.text = element.Name;
			challengeBtn.Data = info.taskId;
			UIEventListener.Get(challengeBtn.gameObject).onClick += OnClickChallengeBtn;
		}

		ChallengeUIGrid.sorting = UIGrid.Sorting.Custom;
		ChallengeUIGrid.repositionNow = true;
		ChallengeUIGrid.Reposition();

		CheckpointLabel.text = res.taskCompleteInfo.Count.ToString();
		ScoreLabel.text = playerTotalScore.ToString();

	}

	private void OnClickChallengeBtn(GameObject obj)
	{
		UIButton btn = obj.GetComponent<UIButton>();
		int taskId = (int)btn.Data;
		NetSender.Instance.RequestTaskAcceptReq(taskId,false,0,0,true);
	}

	int FinalEvaluation(float finishLv)
	{
		if(finishLv == 100)
		{
			return 0;
		}else if(finishLv == 200){
			return 1;
		}else if(finishLv == 300){
			return 2;
		}else
		{
			return 0;
		}
	}

	public void ShowRankingInfo(sg.GS2C_Ranking_List_Info_Res res)
	{
		NGUITools.SetActive(BaseInformation , false);
		NGUITools.SetActive(RankingInformation , true);
		HelpUtil.DelListInfo(RankingUIGrid.transform);

		foreach(sg.GS2C_Ranking_List_Info_Res.RankingList info in res.rankingList)
		{
			GameObject item = GameObject.Instantiate(RankingItem)as GameObject;
			item.transform.parent = RankingUIGrid.transform;
			item.transform.localScale = Vector3.one;
			item.transform.localPosition = Vector3.zero;

			item.name = "Item" + getNumOrString(info.ranking);
			UILabel rankingNum = item.transform.Find("RankingNum").GetComponent<UILabel>();
			UILabel playerName = item.transform.Find("PlayerName").GetComponent<UILabel>();
			UILabel totalScore = item.transform.Find("TotalScore").GetComponent<UILabel>();
			UILabel checkpointNum = item.transform.Find("CheckpointNum").GetComponent<UILabel>();
			UISprite kingSprite = item.transform.Find("KingSprite").GetComponent<UISprite>();

			rankingNum.text = info.ranking.ToString();
			playerName.text = info.name;
			totalScore.text = info.totalScore.ToString();
			checkpointNum.text = info.checkpointsNum.ToString();
			if(info.ranking < 4)
			{
				kingSprite.spriteName = "King"+info.ranking.ToString();
				NGUITools.SetActive(kingSprite.gameObject , true);
			}
			else
			{
				NGUITools.SetActive(kingSprite.gameObject , false);
			}
			UIButton btn = item.transform.GetComponent<UIButton>();
			btn.Data = info;
			UIEventListener.Get(btn.gameObject).onClick += OnClickRankingDetailBtn;
		}

		RankingUIGrid.sorting = UIGrid.Sorting.Custom;
		RankingUIGrid.repositionNow = true;
		RankingUIGrid.Reposition();

		if(res.myRanking > 0)
		{
			RankingNum.text = res.myRanking.ToString();
		}
		PlayerName.text = Globals.Instance.MGameDataManager.MActorData.BasicData.Name;
		TotalScore.text = res.myTotalScore.ToString();
		CheckpointNum.text = res.myCheckpointsNum.ToString();
	}

	private void OnClickRankingDetailBtn(GameObject obj)
	{
		UIButton btn = obj.transform.GetComponent<UIButton>();
		mRankingInfo = (sg.GS2C_Ranking_List_Info_Res.RankingList)btn.Data;

		NetSender.Instance.C2GSPlayerInfoReq(mRankingInfo.roleId);
	}

	public void ShowPlayerInfo(sg.GS2C_Player_Info_Res res )
	{
		NGUITools.SetActive(RankingInformation , false);
		NGUITools.SetActive(PlayerInformation , true);
		NGUITools.SetActive(characterCustomizeOne.gameObject , true);
		NGUITools.SetActive(SceneGameObject , true);
		NGUITools.SetActive(MemoryInfo , false);
		characterCustomizeOne.ResetCharacter();
		characterCustomizeOne.ResetFaceCustomBone();
		characterCustomizeOne.generageCharacterOtherPlayer(0,res.roleAppearance,res.roleEquips);
		characterCustomizeOne.transform.localEulerAngles = new Vector3(0,180,0);
		characterCustomizeOne.changeCharacterAnimationController("General_Idle");

		Globals.Instance.MSceneManager.mTaskCameramControl.transform.localPosition = new Vector3 (0f,285f,-1380f);

		PlayerRankingNum.text = mRankingInfo.ranking.ToString();
		PlayerPlayerName.text = mRankingInfo.name;
		PlayerTotalScore.text = mRankingInfo.totalScore.ToString();
		PlayerCheckpointNum.text = mRankingInfo.checkpointsNum.ToString();;
	}

	private void OnClickBackBtn(GameObject obj)
	{
		this.Close();
	}
	private void OnClickPlayerInformationBackBtn(GameObject obj)
	{
		NGUITools.SetActive(RankingInformation , true);
		NGUITools.SetActive(SceneGameObject , false);
		NGUITools.SetActive(PlayerInformation , false);
		NGUITools.SetActive(MemoryInfo , true);
		NGUITools.SetActive(characterCustomizeOne.gameObject , false);
	}


	private int GetGameEndingState(string str)
	{
		if(mCacheList.Count > 0)
		{
			List<int> endingList = new List<int>();
			string[] vecs = str.Split('|');
			
			for (int i = 0; i < vecs.Length; i++)
				endingList.Add( StrParser.ParseDecInt(vecs[i],-1) );

			foreach(int taskid in endingList)
			{
				foreach(int id in mCacheList)
				{
					if(taskid > 0 && id > 0 &&taskid == id)
					{
						return taskid;
					}
				}
			}
		}
		return -1;
	}

	private string getNumOrString(int num)
	{
		string s = "";
		if(num/100 > 0)
		{
			s = num.ToString();
		}
		else if(num/10 > 0)
		{
			s = "0"+num.ToString();
		}else
		{
			s = "00"+num.ToString();
		}
		return s;
	}

	void OnDestroy()
	{
		base.OnDestroy();	
		if (Globals.Instance.MSceneManager != null) {
			Globals.Instance.MSceneManager.mTaskCameramControl.transform.localPosition = Vector3.zero;
			Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
		}

		if(characterCustomizeOne != null)
		{
			DestroyImmediate(characterCustomizeOne.gameObject);
		}
		if(SceneGameObject != null)
		{
			DestroyImmediate(SceneGameObject);
		}
		Resources.UnloadUnusedAssets();
	}
}


