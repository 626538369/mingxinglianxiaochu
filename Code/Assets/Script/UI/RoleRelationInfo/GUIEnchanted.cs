using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIEnchanted : GUIWindow 
{
	
	public UIImageButton ButtonBack;
	
	int mNoteHua = 0;
	
	public UIImageButton ButtonExit;
	
	public GameObject CGItemPrefab;
	//public UIDraggablePanel uiDraggablePanel;
	public UIGrid uiGrid;
	Task_Play mTaskConfig;
	
	int mMaxHua;
	//bool bIsMaxInThisChapter = false;
	//bool bIsMaxInAllChapter = false;
	
	Dictionary<int,List<int>> mChapterDic = new Dictionary<int, List<int>>();
	List<int> mBranchChapter = new List<int>();
	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
		base.Awake();	
		
		base.enabled = true;
		
		mTaskConfig = Globals.Instance.MDataTableManager.GetConfig<Task_Play>();
		
		UIEventListener.Get(ButtonExit.gameObject).onClick += OnClickExitBtn;
		UIEventListener.Get(ButtonBack.gameObject).onClick += OnClickButtonBack;
	}
	
	protected virtual void Start ()
	{
		base.Start();
		DisplayChapterList();
		NGUITools.SetActive(ButtonBack.gameObject,false);
		
	}
	
	protected override void OnDestroy ()
	{
		//Globals.Instance.MTeachManager.NewSetAllBuildingColliderEnabled(true);
		base.OnDestroy ();
	}
	
	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		
		_mIsLoaded = true;
		
		this.GUILevel = 10;
		
		
	
	}
	

	private void OnClickExitBtn(GameObject obj)
	{
		this.Close();
	}
	private void OnClickButtonBack(GameObject obj)
	{
		NGUITools.SetActive(ButtonBack.gameObject,false);
		DisplayChapterList();
	}
	
	
	public  void DisplayChapterList()
	{
		HelpUtil.DelListInfo(uiGrid.transform);

		int i = 0;
		foreach(var Chapter in mChapterDic)
		{
			
			GameObject CGObj = GameObject.Instantiate(CGItemPrefab)as GameObject;
			CGObj.transform.parent = uiGrid.transform;
			CGObj.transform.localScale = Vector3.one;
			CGObj.transform.localPosition = new Vector3(0,0,-5);
			UIToggle checbox = CGObj.GetComponent<UIToggle>();
			
			UILabel TitleLabel = CGObj.transform.Find("TitleLabel").GetComponent<UILabel>();
			int numHua = 0;
			if(mChapterDic[Chapter.Key].Contains(mNoteHua))
				numHua = mChapterDic[Chapter.Key].Count - 1;
			else
				numHua = mChapterDic[Chapter.Key].Count;
			TitleLabel.text = mTaskConfig.GetNameByChapter(Chapter.Key) +"(" + numHua.ToString()  + Globals.Instance.MDataTableManager.GetWordText(6013)+ ")";
			

			checbox.Data = Chapter.Key;
			UIEventListener.Get(checbox.gameObject).onClick += delegate(GameObject Obj)
			{
				UIToggle check = CGObj.GetComponent<UIToggle>();
				int data = (int)check.Data;
				DisplayHua(data);
			};
			++i;
		}
		
		if(0 != mBranchChapter.Count)
		{
			GameObject CGObj = GameObject.Instantiate(CGItemPrefab)as GameObject;
			CGObj.transform.parent = uiGrid.transform;
			CGObj.transform.localScale = Vector3.one;
			CGObj.transform.localPosition = new Vector3(0,0,-5);
			UIToggle checbox = CGObj.GetComponent<UIToggle>();
			
			UILabel TitleLabel = CGObj.transform.Find("TitleLabel").GetComponent<UILabel>();
			TitleLabel.text = "Branch";
			
			
			UIEventListener.Get(checbox.gameObject).onClick += delegate(GameObject Obj)
			{
				DisplayBranchHua();
			};
			
		}
		uiGrid.repositionNow = true;
		//uiDraggablePanel.ResetPosition();
	}
	
	private void DisplayHua(int Chapter)
	{
		NGUITools.SetActive(ButtonBack.gameObject,true);
		HelpUtil.DelListInfo(uiGrid.transform);
		
		mChapterDic[Chapter].Sort();
	
		mChapterDic[Chapter].Reverse();
	    int i = 0;
		foreach(int Hua in mChapterDic[Chapter])
		{

			GameObject CGObj = GameObject.Instantiate(CGItemPrefab)as GameObject;
			Task_Play.Task_PlayObject Data = mTaskConfig.GetObjByID(Hua);
			UILabel TitleLabel = CGObj.transform.Find("TitleLabel").GetComponent<UILabel>();
			
			
			CGObj.transform.parent = uiGrid.transform;
			CGObj.transform.localScale = Vector3.one;
			CGObj.transform.localPosition = new Vector3(0,0,-5);
			UIToggle checbox = CGObj.GetComponent<UIToggle>();
			
			
			if(Hua == mNoteHua)
			{
				UILabel DisableTitleLabel = CGObj.transform.Find("DisableTitleLabel").GetComponent<UILabel>();
				NGUITools.SetActive(DisableTitleLabel.gameObject,true);
				NGUITools.SetActive(TitleLabel.gameObject,false);
				DisableTitleLabel.text = "Next" + Data.Task_Title;
				checbox.enabled = false;
			}
			else
			{
				TitleLabel.text = Data.Task_Title;
				checbox.Data = Data;
				UIEventListener.Get(checbox.gameObject).onClick += delegate(GameObject Obj)
				{
					UIToggle check = CGObj.GetComponent<UIToggle>();
					Task_Play.Task_PlayObject DataIn = mTaskConfig.GetObjByID(Hua);
					Globals.Instance.MGUIManager.CreateWindow<GUITaskTalkView>(delegate (GUITaskTalkView gui)
					{
						NGUITools.SetActive(this.gameObject,false);
						gui.ClentTask = true;
						gui.UpdateData(Data.Task_IDList[0], delegate(){
							gui.DestroyThisGUI();
							NGUITools.SetActive(this.gameObject,true);});
					});
				};	
			}

			++i;
		}
		uiGrid.repositionNow = true;
		
	}

    void DisplayBranchHua()
	{
		NGUITools.SetActive(ButtonBack.gameObject,true);
		HelpUtil.DelListInfo(uiGrid.transform);
		foreach(int Chapter in mBranchChapter)
		{	
			GameObject CGObj = GameObject.Instantiate(CGItemPrefab)as GameObject;
			CGObj.transform.parent = uiGrid.transform;
			CGObj.transform.localScale = Vector3.one;
			CGObj.transform.localPosition = new Vector3(0,0,-5);
			UIToggle checbox = CGObj.GetComponent<UIToggle>();
			
			UILabel TitleLabel = CGObj.transform.Find("TitleLabel").GetComponent<UILabel>();
			TitleLabel.text = mTaskConfig.GetNameByChapter(Chapter);
			
			
			checbox.Data = Chapter;
			UIEventListener.Get(checbox.gameObject).onClick += delegate(GameObject Obj)
			{
//				UIToggle check = CGObj.GetComponent<UIToggle>();
//				int data = (int)check.Data;
//				DisplayHua(data);
			};
		}
		uiGrid.repositionNow = true;
		
		
	}
	public void  SetChapterDic( List<sg.GS2C_Cg_Get_Res.Cg> Dic)
	{
		mChapterDic.Clear();
		mNoteHua = 0;
		mMaxHua = 0;
		Dic.Reverse();
		int i = 0;
		foreach(sg.GS2C_Cg_Get_Res.Cg Type in Dic)
		{
			if(1== mTaskConfig.GetCGTypeByID(Type.CgId))//
			{
				int type = 0;
				if(0 == i)
				{
					type = Type.CgId/100;
					mMaxHua = Type.CgId;
					if(mMaxHua/100 == (mMaxHua + 1)/100)//当最大话，和提示话在同一章的时候//
					{
						Task_Play.Task_PlayObject T_PlayObj = mTaskConfig.GetObjByID((mMaxHua + 1));
						if(null != T_PlayObj)
						{
							mNoteHua = mMaxHua + 1;	
						}
					}
					else
					{
						type ++;//下一章节//
						Task_Play.Task_PlayObject T_PlayObj = mTaskConfig.GetObjByID((type * 100 + 1));//下一章节存在//
						if(null != T_PlayObj)
						{
							mNoteHua = type * 100 + 1;	
						}	
					}
					
					if(0 != mNoteHua)
					{
						List<int> temp = new List<int>();
						temp.Add(mNoteHua);
						mChapterDic.Add(type,temp);
					}
							
				}
				type = Type.CgId/100;
				if(mChapterDic.ContainsKey(type))
				{
					mChapterDic[type].Add(Type.CgId);
				}
				else
				{
					List<int> temp = new List<int>();
					temp.Add(Type.CgId);
					mChapterDic.Add(type,temp);
				}
				++i;
			}
			else
			{
				mBranchChapter.Clear();
				mBranchChapter.Add(Type.CgId);
			}

		}
		if(0 == Dic.Count)
		{
			mNoteHua = 101;
			List<int> temp = new List<int>();
			temp.Add(mNoteHua);
			mChapterDic.Add(mNoteHua/100,temp);
		}
	}

}
