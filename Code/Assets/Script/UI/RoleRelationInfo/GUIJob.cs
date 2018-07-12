using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GUIJob : GUIWindowForm 
{
	public PlayerBasicProperties playerBasicProperties;
	public NewWealthGroup newWealthGroup;
	public UIButton BackHomeBtn;

	public UIScrollView JobPlaceUIScrollView;
	public UIGrid JobPlaceUIGrid;
	public GameObject JobPlaceItem;

	public GameObject JobInformation;
	public UIButton BackBtn;
	public UILabel JobPlaceNameLabel;

	public UIScrollView JobIScrollView;
	public UIGrid JobUIGrid;
	public GameObject JobItem;

	public UILabel RefreshLabel;
	public GameObject PlayAnimation;
	public UILabel JobNameLabel;
	public GameObject RewardInfor;
	public GameObject AttributeInfor;
	public UISprite JobAnimation;
	public UILabel PiLaoRewardLabel;
	public UILabel MoneyRewardLabel;
	public UILabel FansRewardLabel;
	public GameObject CompleteTips;

	private PlayerData playerData;
	private JobConfig jobConfig;
	private JobPlaceConfig jobPlaceConfig;

	private Dictionary<int,List<int>> CacheJobPlace = new Dictionary<int,List<int>>();

	private string[] NeedAttribute = {"IconBiaoyan","IconDongGan","IconXueShi","IconYiTai"};

	private int faintState = 0;

	// --------------保存一下需要在奖励界面显示的信息---------------//
	
	private int mTemporary_JobPiLaoNum;
	private int mTemporary_JobMoneyNum;
	private int mTemporary_JobFansNum;
	// --------------------------------------------------//

	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		base.Awake();
		Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.TASKCAMERA);

		playerData = Globals.Instance.MGameDataManager.MActorData;
		jobConfig = Globals.Instance.MDataTableManager.GetConfig<JobConfig>();
		jobPlaceConfig = Globals.Instance.MDataTableManager.GetConfig<JobPlaceConfig>();
		
		UIEventListener.Get(BackHomeBtn.gameObject).onClick += delegate(GameObject go) {
			this.Close();
		};

		UIEventListener.Get(BackBtn.gameObject).onClick += delegate(GameObject go) {
			NGUITools.SetActive(JobPlaceUIScrollView.gameObject , true);
			NGUITools.SetActive(JobInformation.gameObject , false);
		};

		RefreshLabel.text = ShowRefreshTime();
			
		FristCacheJobListInfor();
	}

	private string ShowRefreshTime()
	{
		System.DateTime mStartTime = System.Convert.ToDateTime(GameDefines.GameStartDateTime);
		System.DateTime mCurrentTime = mStartTime.AddDays(playerData.starData.nLineDay);

		int year = 0;
		int month = 0;
		int day = 0;
		if(mCurrentTime.Month==12)
		{
			if(mCurrentTime.Day >= 15)
			{
				year = mCurrentTime.Year + 1;
				month = 1;
				day = 1;
			}
			else
			{
				year = mCurrentTime.Year;
				month = mCurrentTime.Month;
				day = 15;
			}
		}
		else
		{
			if(mCurrentTime.Day >= 15)
			{
				year = mCurrentTime.Year;
				month = mCurrentTime.Month + 1;
				day = 1;
			}
			else
			{
				year = mCurrentTime.Year;
				month = mCurrentTime.Month;
				day = 15;
			}
		}
		System.DateTime mRefreshTime = System.Convert.ToDateTime(year.ToString()+"-"+month.ToString()+"-"+day.ToString());

		return string.Format(Globals.Instance.MDataTableManager.GetWordText(3002),mRefreshTime.ToString("yyyy-MM-dd"));
	}


	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		base.GUILevel = 8;


	}
	
	protected virtual void Start ()
	{
		base.Start();

	}

	private void FristCacheJobListInfor()
	{
		CacheJobPlace.Clear();

		if(Globals.Instance.MJobManager.getJobPlaceInformationDic.Count <= 0)
		{
			GUIRadarScan.Show();
			NetSender.Instance.C2GSReadJobListReq();
		}
		else
		{
			foreach(KeyValuePair<int,List<int>> mJobPlace in Globals.Instance.MJobManager.getJobPlaceInformationDic)
			{
				List<int> jobList = new List<int>();
				foreach(int va in mJobPlace.Value)
				{
					jobList.Add(va);
				}
				CacheJobPlace.Add(mJobPlace.Key,jobList);
			}
			ShowJobPlaceInfor();
		}
	}

	public void CacheJobListInfor()
	{
		CacheJobPlace.Clear();
		if(Globals.Instance.MJobManager.getJobPlaceInformationDic.Count > 0)
		{
			foreach(KeyValuePair<int,List<int>> mJobPlace in Globals.Instance.MJobManager.getJobPlaceInformationDic)
			{
				List<int> jobList = new List<int>();
				foreach(int va in mJobPlace.Value)
				{
					jobList.Add(va);
				}
				CacheJobPlace.Add(mJobPlace.Key,jobList);
			}
		}
		ShowJobPlaceInfor();
	}


	private void ShowJobPlaceInfor()
	{
		NGUITools.SetActive(JobPlaceUIScrollView.gameObject , true);
		NGUITools.SetActive(JobInformation.gameObject , false);
		HelpUtil.DelListInfo(JobPlaceUIGrid.transform);
		foreach(KeyValuePair<int, JobPlaceConfig.JobPlaceElement> jobPlaceElement in jobPlaceConfig.GetJobPlaceElementList())
		{
			GameObject jobPlaceItem = GameObject.Instantiate(JobPlaceItem)as GameObject;
			jobPlaceItem.transform.parent = JobPlaceUIGrid.transform;
			jobPlaceItem.transform.localScale = Vector3.one;
			jobPlaceItem.transform.localPosition = Vector3.zero;

			jobPlaceItem.name = "JobPlaceItem" + jobPlaceElement.Key;

			UILabel nameLabel = jobPlaceItem.transform.Find("NameLabel").GetComponent<UILabel>();
			UILabel noGoIntoLabel = jobPlaceItem.transform.Find("NoGoIntoLabel").GetComponent<UILabel>();
			UIButton GoIntoBtn = jobPlaceItem.transform.Find("GoIntoBtn").GetComponent<UIButton>();

			nameLabel.text = jobPlaceElement.Value.Job_Place_Name;
			if(jobPlaceElement.Value.Need_Fans > playerData.starData.nRoleFenSi)
			{
				NGUITools.SetActive(noGoIntoLabel.gameObject , true);
				noGoIntoLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(3001),jobPlaceElement.Value.Need_Fans);  //  需要文本ID //
				NGUITools.SetActive(GoIntoBtn.gameObject , false);
			}
			else
			{
				NGUITools.SetActive(noGoIntoLabel.gameObject , false);
				NGUITools.SetActive(GoIntoBtn.gameObject , true);
				GoIntoBtn.Data = jobPlaceElement.Value;
				UIEventListener.Get(GoIntoBtn.gameObject).onClick += OnClickGoIntoBtn;
			}
		}
		JobPlaceUIGrid.sorting = UIGrid.Sorting.Custom;
		JobPlaceUIGrid.repositionNow = true;
		JobPlaceUIGrid.onReposition += RefreshGuide;
	}

	private void RefreshGuide()
	{
		Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIClothShop");
	}

	private void OnClickGoIntoBtn(GameObject obj)
	{
		UIButton btn = obj.transform.GetComponent<UIButton>();
		JobPlaceConfig.JobPlaceElement element = (JobPlaceConfig.JobPlaceElement)btn.Data;

		ShowSingleJobInfor(element);
	}


	private void ShowSingleJobInfor(JobPlaceConfig.JobPlaceElement jobPlaceElement)
	{
		List<int> mSingleJobList = GetSinglePlaceJobList(jobPlaceElement.Job_Place_ID);
	
		HelpUtil.DelListInfo(JobUIGrid.transform);
		for(int i = 0; i < mSingleJobList.Count; i++)
		{
			GameObject jobItem = GameObject.Instantiate(JobItem)as GameObject;
			jobItem.transform.parent = JobUIGrid.transform;
			jobItem.transform.localScale = Vector3.one;
			jobItem.transform.localPosition = Vector3.zero;

			jobItem.name = "JobItem" + i;

			JobConfig.JobElement jobElement = jobConfig.GetSingleElement(mSingleJobList[i]);

			if(jobElement == null)
			{
				Debug.Log("mSingleJobList[i] = " + mSingleJobList[i] + "-- jobPlaceElement.Job_Place_ID = " + jobPlaceElement.Job_Place_ID);
				return;
			}

			UILabel jobNameLabel = jobItem.transform.Find("JobNameLabel").GetComponent<UILabel>();
			UISprite needAttributeOne = jobItem.transform.Find("NeedAttributeOne").GetComponent<UISprite>();
			UILabel numOneLabel = needAttributeOne.gameObject.transform.Find("NumOneLabel").GetComponent<UILabel>();
			UISprite needAttributeTwo = jobItem.transform.Find("NeedAttributeTwo").GetComponent<UISprite>();
			UILabel numTwoLabel = needAttributeTwo.gameObject.transform.Find("NumTwoLabel").GetComponent<UILabel>();
			UISprite rewardOne = jobItem.transform.Find("RewardOne").GetComponent<UISprite>();
			UILabel rewardNumOne = rewardOne.gameObject.transform.Find("RewardNumOne").GetComponent<UILabel>();
			UISprite rewardTwo = jobItem.transform.Find("RewardTwo").GetComponent<UISprite>();
			UILabel rewardNumTwo = rewardTwo.gameObject.transform.Find("RewardNumTwo").GetComponent<UILabel>();

			jobNameLabel.text = jobElement.Job_Name;

			NGUITools.SetActive(needAttributeTwo.gameObject , false);

			int j = 0;
			foreach(KeyValuePair<int,int> mPair in GetNeedAttribute(jobElement))
			{
				if(j == 0)
				{
					needAttributeOne.spriteName = NeedAttribute[mPair.Key];
//					if(GetPlayerAttributeNum(mPair.Key) >= mPair.Value)
//					{
//						numOneLabel.text = "[21ce4a]"+mPair.Value.ToString()+"[-]";
//					}
//					else
//					{
//						numOneLabel.text = "[be2131]"+mPair.Value.ToString()+"[-]";
//					}
					numOneLabel.text = mPair.Value.ToString();
				}
				else if(j == 1)
				{
					needAttributeTwo.spriteName = NeedAttribute[mPair.Key];
//					if(GetPlayerAttributeNum(mPair.Key) >= mPair.Value)
//					{
//						numTwoLabel.text = "[21ce4a]"+mPair.Value.ToString()+"[-]";
//					}
//					else
//					{
//						numTwoLabel.text = "[be2131]"+mPair.Value.ToString()+"[-]";
//					}
					numTwoLabel.text = mPair.Value.ToString();
					NGUITools.SetActive(needAttributeTwo.gameObject , true);
				}
				j++;
			}

			rewardNumOne.text = jobElement.Get_Money.ToString();
			rewardNumTwo.text = jobElement.Get_Fans.ToString();
			UIButton workBtn = jobItem.transform.Find("WorkBtn").GetComponent<UIButton>();
			workBtn.Data = jobElement;
			UIEventListener.Get(workBtn.gameObject).onClick += OnClickWorkBtn;
		}
		JobUIGrid.sorting = UIGrid.Sorting.Custom;
		JobUIGrid.repositionNow = true;
		JobUIGrid.onReposition += RefreshGuide;

		NGUITools.SetActive(JobPlaceUIScrollView.gameObject , false);
		NGUITools.SetActive(JobInformation.gameObject , true);
		JobPlaceNameLabel.text = jobPlaceElement.Job_Place_Name;

	}

	private void OnClickWorkBtn(GameObject obj)
	{
		UIButton btn = obj.transform.GetComponent<UIButton>();
		JobConfig.JobElement jobElement = (JobConfig.JobElement)btn.Data;

		foreach(KeyValuePair<int,int> mPair in GetNeedAttribute(jobElement))
		{
			if(GetPlayerAttributeNum(mPair.Key) < mPair.Value)
			{
				Globals.Instance.MGUIManager.ShowSimpleCenterTips(3003);
				return;
			}
		}

		JobNameLabel.text = jobElement.Job_Name;
		mTemporary_JobPiLaoNum = jobElement.Get_Fatigue;
		NetSender.Instance.C2GSJobRewardReq(jobElement.JobID);
	}

	public void getJobRewardInfo(sg.GS2C_Job_Reward_Res res)
	{
		NGUITools.SetActive(BackHomeBtn.gameObject , false);
		if(res.faintState)
		{
			this.Close();
			
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null)
			{
				guiMain.FaintTreatment(1);
			}
			
			return;
		}
		mTemporary_JobMoneyNum = res.moneyNum;
		mTemporary_JobFansNum = res.fansNum;

		NGUITools.SetActive(JobInformation.gameObject , false);
		NGUITools.SetActive(PlayAnimation.gameObject , true);
		
		PiLaoRewardLabel.text = "+"+mTemporary_JobPiLaoNum;
		MoneyRewardLabel.text = "+"+mTemporary_JobMoneyNum;
		FansRewardLabel.text = "+"+mTemporary_JobFansNum;
		if(Globals.Instance.MGameDataManager.MActorData.starData.IsTimeborrowing){
			NGUITools.SetActive(PiLaoRewardLabel.gameObject , false);
		}else{
			NGUITools.SetActive(PiLaoRewardLabel.gameObject , true);
		}
		StartCoroutine(NeedWait());
	}

	IEnumerator NeedWait()
	{
		yield return new WaitForSeconds(1f);  
		NGUITools.SetActive(RewardInfor , true);
		NGUITools.SetActive(AttributeInfor , false);
		JobAnimation.gameObject.transform.GetComponent<FixedTimeRefreshPosition>().StopRefresh();
		yield return new WaitForSeconds(1f); 
		NGUITools.SetActive(AttributeInfor , true);
		yield return new WaitForSeconds(1f);  
		NGUITools.SetActive(BackHomeBtn.gameObject , true);
		playerBasicProperties.UpdatePlayerBasicProperties();
		newWealthGroup.UpdateWealth();
		NGUITools.SetActive(CompleteTips , true);
		NGUITools.SetActive(RewardInfor , false);
		if(Globals.Instance.MTeachManager.IsOpenTeach&&Globals.Instance.MTeachManager.NewGetTeachStep("x07") == 4)
		{
			Globals.Instance.MTeachManager.SetTeachStep("x07",5,true);
		}
	}

	private int GetPlayerAttributeNum(int type)
	{
		int mNum = 0;
		switch(type)
		{
		case 0:
			mNum = playerData.starData.nRoleYanJi + playerData.starData.equipYanJi;
			break;
		case 1:
			mNum = playerData.starData.nRoleDongGan + playerData.starData.equipDongGan;
			break;
		case 2:
			mNum = playerData.starData.nRoleXueShi + playerData.starData.equipXueShi;
			break;
		case 3:
			mNum = playerData.starData.nRoleYiTai + playerData.starData.equipYiTai;
			break;
		}
		return mNum;
	}

	private Dictionary<int , int> GetNeedAttribute(JobConfig.JobElement jobElement)
	{
		Dictionary<int , int> dic = new Dictionary<int, int>();
		if(jobElement != null)
		{
			if(jobElement.Need_Act > 0)
			{
				dic.Add(0,jobElement.Need_Act);
			}
			if(jobElement.Need_Sport > 0)
			{
				dic.Add(1,jobElement.Need_Sport);
			}
			if(jobElement.Need_Knowledge > 0)
			{
				dic.Add(2,jobElement.Need_Knowledge);
			}
			if(jobElement.Need_Deportment > 0)
			{
				dic.Add(3,jobElement.Need_Deportment);
			}
		}
		return dic;
	}

	private List<int> GetSinglePlaceJobList(int placeID)
	{
		if(!CacheJobPlace.ContainsKey(placeID)||CacheJobPlace[placeID].Count <= 0)
		{
			GUIRadarScan.Show();
			List<int> mList = jobConfig.GetJobSingleElementList(placeID);

			CacheJobPlace.Add(placeID,mList);


			NetSender.Instance.C2GSModifyJobListReq(CacheJobPlace ,playerData.starData.nLineDay);

			GUIRadarScan.Hide();
			return mList;
		}

		foreach(KeyValuePair<int,List<int>> mPair in CacheJobPlace)
		{
			if(placeID == mPair.Key)
			{
				return mPair.Value;
			}
		}
		return null;
	}

	public Transform getTeachJobFromJobID(int jobID)
	{
		for (int i=0; i<JobUIGrid.transform.childCount; i++)
		{
			Transform item = JobUIGrid.transform.GetChild(i);
			
			UIButton workBtn = item.transform.Find("WorkBtn").GetComponent<UIButton>();
			JobConfig.JobElement jobElement = (JobConfig.JobElement)workBtn.Data;
			if (jobElement != null && jobElement.JobID == jobID )
			{
				return item;
			}
		}
		return null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Globals.Instance.MSceneManager != null) {
			Globals.Instance.MSceneManager.ChangeCameraActiveState(SceneManager.CameraActiveState.MAINCAMERA);
		}
	}
}
