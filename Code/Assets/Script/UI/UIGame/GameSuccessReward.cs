using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSuccessReward : MonoBehaviour {

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


	public enum BaseRewardType
	{
		MONEY = 1,			// 金钱//
		DIAMOND = 2,		// 钻石//
		ACTING = 3,			// 演技//
		CHARM = 4			// 魅力//
	}
	public GameObject RewardObj;
	public UISlider ScoreSlider;
	public GameObject Star1Obj;
	public GameObject Star1Anim;
	public GameObject Star2Obj;
	public GameObject Star2Anim;
	public GameObject Star3Obj;
	public GameObject Star3Anim;

	public UILabel CurrentScore;


	public UILabel RewardMoneyNum;
	public UILabel RewardDiamondNum;
	public UILabel RewardFansNum;

	public UITexture RewardItem1;
	public UILabel RewardItem1Name;
	public UITexture RewardItem2;
	public UILabel RewardItem2Name;

	public UIButton ReStartBtn;
	public UIButton CompleteBtn;

	public GameObject FansRewardObj;
	public UILabel FansName;
	public UITexture FansIcon;
	public UIButton FansOkBtn;


	private int MoneyBaseReward = 0;
	private int FansBaseReward = 0;
	private int DiamondBaseReward = 0;
	private int MoneyAppraisalReward = 0;
	private int FansAppraisalReward = 0;
	private int DiamondAppraisalReward = 0;
	private Dictionary<int,int> addMoneyList = new Dictionary<int,int>();
	private UITexture addItemOne ;
	private UILabel addItemNameLabelOne;
	private UITexture addItemTwo ;
	private UILabel addItemNameLabelTwo;

	TaskConfig.TaskObject element = null;
	private double mCurrentfinishLv ;
	private int TaskID;

	void Awake () {

		UIEventListener.Get (ReStartBtn.gameObject).onClick += OnClickRestarBtn;
		UIEventListener.Get (CompleteBtn.gameObject).onClick += OnClickCompleteBtn;

		UIEventListener.Get (FansOkBtn.gameObject).onClick += OnClickFansOkBtn;
	}


	public void ShowSuccessReward(sg.GS2C_Task_Complete_Res res){
		this.gameObject.SetActive (true);

		foreach(sg.GS2C_Task_Complete_Res.TaskReward reward in res.rewards)
		{
			if(reward.rewardType == (int)TaskRewardTypeEnum.GET_ARTTST)
			{
				RewardObj.SetActive(false);
				FansRewardObj.SetActive(true);
				WarshipConfig config = Globals.Instance.MDataTableManager.GetConfig<WarshipConfig>();
				WarshipConfig.WarshipObject warshipElement = null;
				config.GetWarshipElement(reward.itemId ,out warshipElement);
				if(warshipElement == null)
				{
					return;
				}
				FansName.text = warshipElement.Name;
				FansIcon.mainTexture =  Resources.Load("Icon/FansIcon/"+warshipElement.Fans_Icon,typeof(Texture2D)) as Texture2D; 
				return;
			}
		}

		RewardObj.SetActive(true);
		FansRewardObj.SetActive(false);

		mCurrentfinishLv = res.finishLv;
		TaskID = res.taskId;

		CompleteBtn.Data = res.nextTaskId;

		TaskConfig task = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();
		bool hasData = task.GetTaskObject(res.taskId, out element);
		if (!hasData)
			return;	

		ScoreSlider.value = (EliminationMgr.Score*1f) / EliminationMgr.Instance.star3;
		float baseX = 630f / EliminationMgr.Instance.star3;
		float baseX2 = 630f / 2f;
		float star1X = baseX * EliminationMgr.Instance.star1 - baseX2;
		Star1Obj.transform.localPosition = new Vector3 (star1X , Star1Obj.transform.localPosition.y,Star1Obj.transform.localPosition.z);
		float star2X = baseX * EliminationMgr.Instance.star2 - baseX2;
		Star2Obj.transform.localPosition = new Vector3 (star2X , Star2Obj.transform.localPosition.y,Star2Obj.transform.localPosition.z);
		float star3X = baseX * EliminationMgr.Instance.star3 - baseX2;
		Star3Obj.transform.localPosition = new Vector3 (star3X , Star3Obj.transform.localPosition.y,Star3Obj.transform.localPosition.z);

		if(EliminationMgr.Score >= EliminationMgr.Instance.star1){
			Star1Anim.SetActive (true);
		}
		if(EliminationMgr.Score >= EliminationMgr.Instance.star2){
			Star2Anim.SetActive (true);
		}
		if(EliminationMgr.Score >= EliminationMgr.Instance.star3){
			Star3Anim.SetActive (true);
		}

		CurrentScore.text = EliminationMgr.Score.ToString ();


		List<string[]> RewardArray = ParsingRewards(element.Rewards);
		if(RewardArray.Count > 0)
		{
			for(int i = 0;i < RewardArray.Count; i++)
			{
				switch(StrParser.ParseDecInt(RewardArray[i][0],-1))
				{
				case (int)TaskRewardTypeEnum.ROLE_ATTR:	
					switch(StrParser.ParseDecInt(RewardArray[i][1],-1))
					{
					case (int)BaseRewardType.MONEY:

						MoneyBaseReward = StrParser.ParseDecInt(RewardArray[i][2],-1);
						break;
					case (int)BaseRewardType.DIAMOND:
						DiamondBaseReward = StrParser.ParseDecInt(RewardArray[i][2],-1);
						break;
					}
					break;
				case (int)TaskRewardTypeEnum.FANS_NUM:
					FansBaseReward = StrParser.ParseDecInt(RewardArray[i][2],-1);
					break;
				}
			}	
		}
		NGUITools.SetActive(RewardItem1.gameObject,false);
		NGUITools.SetActive(RewardItem2.gameObject,false);
		int RewardItemState = 0;
		foreach(sg.GS2C_Task_Complete_Res.TaskReward reward in res.rewards)
		{

			switch(reward.rewardType)
			{
			case (int)TaskRewardTypeEnum.ITEM: 	

				ItemConfig item = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
				ItemConfig.ItemElement ItemEle = null;
				bool hasDataItem = item.GetItemElement(reward.itemId, out ItemEle);
				if (!hasDataItem)
					return;	

				if(RewardItemState == 0)
				{
					RewardItemState = 1;
					NGUITools.SetActive(RewardItem1.gameObject,true);
					RewardItem1.mainTexture =  Resources.Load("Icon/ItemIcon/"+ItemEle.Icon,typeof(Texture2D)) as Texture2D;
					if(reward.num > 1)
					{
						RewardItem1Name.text = Globals.Instance.MDataTableManager.GetWordText(reward.itemId)+"X"+reward.num;
					}
					else
					{
						RewardItem1Name.text = Globals.Instance.MDataTableManager.GetWordText(reward.itemId);
					}
					addItemOne = RewardItem1;
					addItemNameLabelOne = RewardItem1Name;
					addMoneyList.Add(1,reward.clothAddMoney);
					Debug.Log(" 1 -- reward.clothAddMoney = "+ reward.clothAddMoney);
				}else
				{
					NGUITools.SetActive(RewardItem2.gameObject,true);
					RewardItem2.mainTexture =  Resources.Load("Icon/ItemIcon/"+ItemEle.Icon,typeof(Texture2D)) as Texture2D;
					if(reward.num > 1)
					{
						RewardItem2Name.text = Globals.Instance.MDataTableManager.GetWordText(reward.itemId)+"X"+reward.num;
					}else
					{
						RewardItem2Name.text = Globals.Instance.MDataTableManager.GetWordText(reward.itemId);
					}
					addItemTwo = RewardItem2;
					addItemNameLabelTwo = RewardItem2Name;
					addMoneyList.Add(2,reward.clothAddMoney);
					Debug.Log(" 2 -- reward.clothAddMoney = "+ reward.clothAddMoney);
				}

				break;
			case (int)TaskRewardTypeEnum.POSTURE: 
				

				break;
			case (int)TaskRewardTypeEnum.ROLE_ATTR:	
				switch(reward.itemId)
				{
				case (int)BaseRewardType.MONEY:
					MoneyAppraisalReward = reward.num - MoneyBaseReward;
					if(MoneyAppraisalReward > 0)
					{
						RewardMoneyNum.text = "[000000]"+MoneyBaseReward+"[-]"+ "[00A542]"+ " + " +MoneyAppraisalReward.ToString()+"[-]";
					}else
					{
						RewardMoneyNum.text = "[000000]"+MoneyBaseReward+"[-]";
					}

					break;
				case (int)BaseRewardType.DIAMOND:
					DiamondAppraisalReward = reward.num - DiamondBaseReward;
					if(DiamondAppraisalReward > 0)
					{
						RewardDiamondNum.text = "[000000]"+DiamondBaseReward +"[-]"+ "[00A542]"+" + " + DiamondAppraisalReward.ToString()+"[-]";
					}else
					{
						RewardDiamondNum.text = "[000000]"+DiamondBaseReward+"[-]";
					}

					break;
				case (int)BaseRewardType.ACTING:
					break;
				case (int)BaseRewardType.CHARM:
					break;
				}
				break;

			case (int)TaskRewardTypeEnum.FANS_NUM:
				FansAppraisalReward = reward.num - FansBaseReward;
				if(FansAppraisalReward > 0)
				{
					RewardFansNum.text = "[000000]"+FansBaseReward.ToString() +"[-]"+ "[00A542]"+" + " + FansAppraisalReward.ToString()+"[-]";
				}else
				{
					RewardFansNum.text = "[000000]"+FansBaseReward.ToString() +"[-]";
				}
				break;
			case (int)TaskRewardTypeEnum.UNKNOWNTYPE: 
				break;
			}
		}

		addMoney ();
	}


	private List<string[]> ParsingRewards(string Rewards)
	{
		string[] RewardInfo = Rewards.Split(',');
		List<string[]> 	RewardType = new List<string[]>();

		for(int i = 0; i < RewardInfo.Length;i++)
		{
			RewardType.Add(RewardInfo[i].Split(':'));
		}
		return RewardType;
	}
	public void OnClickRestarBtn(GameObject obj){
		TaskManager.ChallengeAgain mChallengeAgain = new TaskManager.ChallengeAgain();
		mChallengeAgain.isChallengeAgain = true;
		mChallengeAgain.evaluationl = (float)mCurrentfinishLv;
		Globals.Instance.MTaskManager.challengeAgain = mChallengeAgain;
		NetSender.Instance.RequestTaskAcceptReq(TaskID,true);
	}

	public void OnClickCompleteBtn(GameObject obj){

		UIButton btn = obj.transform.GetComponent<UIButton>();
		Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId = (int)btn.Data;
		Globals.Instance.MTaskManager.challengeAgain = null;
		if(mCurrentfinishLv >= 2 || element.Progress_Count <= 0)
		{
			if(Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId > 0)
			{
				GUIPhotoGraph guiPhoto = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph> ();
				if(guiPhoto != null){
					guiPhoto.IsReturnMainScene = false;
					guiPhoto.Close();
				}
				GUIGuoChang.Show();

				Globals.Instance.MTaskManager.StartNextTask(Globals.Instance.MTaskManager.mTaskDailyData.NextTaskId);
			}
			else
			{
				GUIPhotoGraph guiPhoto = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph> ();
				if(guiPhoto != null){
					guiPhoto.Close ();
				}
			}
		}
		else
		{
			NetSender.Instance.C2GSTaskSetCompleteReq(TaskID , mCurrentfinishLv);
		}
	}


	public void OnClickFansOkBtn(GameObject obj){

		GUIPhotoGraph gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph> ();
		if(gui != null){
			gui.Close ();
		}
		NetSender.Instance.RequestSellWarship ();
	}


	public void addMoney()
	{
		if(addMoneyList.Count > 0)
		{
			foreach(KeyValuePair<int,int> mPair in addMoneyList)
			{
				if(mPair.Value <= 0)
					continue;
				if(mPair.Key == 1)
				{
					addMoneyEffect(addItemOne,addItemNameLabelOne,mPair.Value);
				}
				else if(mPair.Key == 2)
				{
					addMoneyEffect(addItemTwo,addItemNameLabelTwo,mPair.Value);
				}
			}
		}
	}
	void addMoneyEffect(UITexture texture ,UILabel label , int money )
	{
		UILabel getLabel = texture.transform.Find("getLabel").GetComponent<UILabel>();
		NGUITools.SetActive(getLabel.gameObject , true);
		TweenScale tweenScale = TweenScale.Begin(getLabel.gameObject , 0.8f , Vector3.one);
		tweenScale.from = new Vector3(1.5f,1.5f,1.5f);
		tweenScale.to = Vector3.one;
		EventDelegate.Add(tweenScale.onFinished , delegate {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(6006);
			TweenRotation tween1 = TweenRotation.Begin(texture.gameObject , 0.5f ,Quaternion.Euler(new Vector3(0f,90f,0f)));
			tween1.from = new Vector3(0f,0f,0f);
			tween1.to = new Vector3(0f,90f,0f);
			tween1.duration = 0.5f;
			tween1.ResetToBeginning();
			tween1.PlayForward();
			tween1.onFinished.Clear();
			EventDelegate.Add(tween1.onFinished,delegate() {
				texture.mainTexture = null;
				NGUITools.SetActive(getLabel.gameObject , false);
				UISprite sprite = texture.transform.Find("Sprite").GetComponent<UISprite>();
				NGUITools.SetActive(sprite.gameObject , true);
				label.text = Globals.Instance.MDataTableManager.GetWordText(6007) + money.ToString();
				tween1.from = new Vector3(0f,90f,0f);
				tween1.to = new Vector3(0f,0f,0f);
				tween1.duration = 0.5f;
				tween1.ResetToBeginning();
				tween1.PlayForward();
				tween1.onFinished.Clear();
			},true);
		},true);
	}
}
