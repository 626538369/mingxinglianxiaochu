using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIPlayer : GUIWindowForm {

	public PlayerBasicProperties playerBasicProperties;
	public NewWealthGroup newWealthGroup;
	public UIButton BackHomeBtn;
	public UIButton ZhengRongBtn;

	public UILabel DateLabel;
	public UIScrollView DateUIScrollView;
	public UIGrid DateUIGrid;
	public GameObject MemoItem;


	private PlayerData playerData;
	private TaskConfig task;
	private DateTime mStartTime;

	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();		
		base.enabled = true;		

		task = Globals.Instance.MDataTableManager.GetConfig<TaskConfig>();

		mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);

		UIEventListener.Get(BackHomeBtn.gameObject).onClick += delegate(GameObject go) {
			this.Close();
		};
		UIEventListener.Get (ZhengRongBtn.gameObject).onClick += OnClickZhengRong;
	}
	void Start () 
	{
		base.Start();

	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 20;

		playerData = Globals.Instance.MGameDataManager.MActorData;

		DateTime mStartTime = Convert.ToDateTime(GameDefines.GameStartDateTime);
		DateTime mCurrentTime = mStartTime.AddDays(playerData.starData.nLineDay);
		DateLabel.text = mCurrentTime.ToString("yyyy-MM-dd");
	}
	 

	public void ShowMemoInformation()
	{
		HelpUtil.DelListInfo(DateUIGrid.transform);

		Dictionary<int,int> lineDayDic = new Dictionary<int, int>();
		List<int> lineDayList = new List<int>();
		foreach(KeyValuePair<int,int> mPair in Globals.Instance.MTaskManager._mFinishedList)
		{
			int taskid = mPair.Key;
			TaskConfig.TaskObject taskObject = null;
			bool ishas = task.GetTaskObject(taskid,out taskObject);
			if(!ishas)
			{
				return;
			}
			int lineDayTime= mPair.Value + taskObject.Memo_Time_Start;
			lineDayDic.Add(mPair.Key,lineDayTime);
			lineDayList.Add(lineDayTime);
		}

		for(int i = 0; i < lineDayList.Count -1 ; i++)
		{
			for(int j = 0; j <  lineDayList.Count -i-1; j++)
			{
				if( lineDayList[j] < lineDayList[j+1])
				{
					int t = lineDayList[j+1];
					lineDayList[j+1] = lineDayList[j];
					lineDayList[j] = t;
				}
			}
		}

		List<int> lineDayDesc = new List<int>();
		for(int i = 0; i < lineDayList.Count ; i++)
		{
			foreach(KeyValuePair<int,int> mPair in lineDayDic)
			{
				if(mPair.Value == lineDayList[i] && !lineDayDesc.Contains(mPair.Key))
				{
					lineDayDesc.Add((mPair.Key));
				}
			}
		}
		for(int i = 0; i < lineDayDesc.Count; i++)
		{
			int taskid = lineDayDesc[i];
			int completeLineDay = Globals.Instance.MTaskManager._mFinishedList[taskid];
			GameObject memoItem = GameObject.Instantiate(MemoItem) as GameObject;
			memoItem.transform.parent = DateUIGrid.transform;
			memoItem.transform.localScale = Vector3.one;
			memoItem.transform.localPosition = Vector3.zero;

			memoItem.name = "MemoItem" + getNumOrString(i);

			UILabel memoLabel = memoItem.transform.Find("MemoLabel").GetComponent<UILabel>();
			if( i == 0)
			{
				UISprite newSprite = memoItem.transform.Find("NewSprite").GetComponent<UISprite>();
				NGUITools.SetActive(newSprite.gameObject , true);
			}
			TaskConfig.TaskObject taskObject = null;
			bool ishas = task.GetTaskObject(taskid,out taskObject);
			if(!ishas)
			{
				return;
			}

			string lineDayStart = mStartTime.AddDays(completeLineDay + taskObject.Memo_Time_Start).ToString("yyyy-MM-dd");
			string lineDayEnd= mStartTime.AddDays(completeLineDay + taskObject.Memo_Time_End).ToString("yyyy-MM-dd");
			if( taskObject.Memo_Time_Start != taskObject.Memo_Time_End)
			{
				memoLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(6005),lineDayStart,lineDayEnd) + taskObject.Memo_Desc;
			}
			else
			{
				memoLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(6004),lineDayStart) + taskObject.Memo_Desc;
			}
		}
		DateUIGrid.sorting = UIGrid.Sorting.Custom;
		DateUIGrid.repositionNow = true;
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
	}

	public void OnClickZhengRong(GameObject obj)
	{
		GUIGuoChang.Show();
		Globals.Instance.MGUIManager.CreateWindow<GUIChangeAppearance>(delegate(GUIChangeAppearance gui){
			NGUITools.SetActive(this.gameObject , false);
			GUIGuoChang.SetTweenPlay(0,delegate {
				gui.UpdateZeroStep();
			});
			gui.CloseChangeAppearanceEvent += delegate() {
				NGUITools.SetActive(this.gameObject , true);
			};
		});
	}
}


