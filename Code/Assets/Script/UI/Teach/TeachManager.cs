using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class JianTouObject
{
	public string JianTouName;
	public string Key;
	public int Value;
	public bool IsCloseGuide = false;
	public bool IsSaveToServer = true;
}

public class TeachManager : MonoBehaviour {
	
	// 开启和关闭新手引导//
	
	public bool mIsOpenTeach = true;
	string testOpenOneTeachName = "x17111";
	private bool mIsWaitForSecond = false;
	private float mWaitForSecond = 0.0f;
	private bool mHasCheckException = false;
	public bool IsOpenTeach{get{return mIsOpenTeach;} set{mIsOpenTeach = value;}}
	public GameObject mNewJianTouC = null;
	private Dictionary<string, int> mNewTeachDataDic = new Dictionary<string, int>();
	private Transform mRootTran;
	public  GameObject mTalkObjectPrefab;
	public  GameObject mTalkTargetObj;
	public  static int TeachFinishedValue = 1000000; // 完成//
	private int TeachXunHuanValue = -1000000; // 循环//
	
	public enum ETeachGUIMainDir
	{
		ListLeft,
		ListRight,
		ListNo,
	}
	
	ETeachGUIMainDir mGUIMainDir = ETeachGUIMainDir.ListNo;
	
	private float mWidthRatio;
	private float mHeightRatio;
	
	void Start()
	{
		GameObject go = GameObject.Find("UICamera");
		if(null != go)
		{
			mRootTran = go.transform;
		}
		
		mWidthRatio = Globals.Instance.MGUIManager.widthRatio;
		mHeightRatio = Globals.Instance.MGUIManager.heightRatio;
	}
	
	public void NewOpenFirstTeach()
	{

		
	}
	
	public void ReceiveMessage(GameObject go, string funcName)
	{
		if(!mIsOpenTeach){return;}
	
	
		if(funcName == "OnClick" && go != null)
		{
			NewButtonTapEvent(go);
		}

	}
	
	public bool NewIsTeachFinished()
	{
		if (!Globals.Instance.MTeachManager.IsOpenTeach)
			return true;
		int teachStep = Globals.Instance.MTeachManager.NewGetTeachStep("x21");
		if ( teachStep < 1)
		{
			return false;
		}
		return true;
	}
	
	
	void Update () {
		if(!mIsOpenTeach){return;}
		
		if (mWaitForSecond > 0)
		{
			mWaitForSecond -= Time.deltaTime;
			if (mWaitForSecond < 0)
			{
				NewRefreshAllTeach();
			}
		}
	}
	
	
	// 打开界面时，刷新该界面的引导状态并进行显示和屏蔽界面//
	public void NewOpenWindowEvent(string _guiName)
	{
		if(!mIsOpenTeach){return;}
		
		if("GUIRadarScan" == _guiName)
		{
			return;
		}
		
		if("GUIGuoChang" == _guiName)
		{
			return;
		}
	
		Debug.Log("NewOpenWindowEvent" + _guiName);
		
		NewRefreshAllTeach();
		
//		Globals.Instance.MPushDataManager.RefreshPushUI(_guiName);
	}
	
	
	// 进入港口，刷新建筑物和主界面的引导状态//
	public void NewTeachEnterPort()
	{
		if(!mIsOpenTeach){return;}

		
		NewRefreshAllTeach();
		
//		Globals.Instance.MPushDataManager.RefreshPushUI("EnterPort");
	}
	
	public void NewTeachEnterCopy()
	{
		if(!mIsOpenTeach){return;}
		int copyID = Globals.Instance.MTaskManager.GetCurTaskCopyId();
		
	}
	
	
	
	// 根据errorCode，开启对应引导//
	public void NewErrorCodeEvent(int errorCode)
	{

	}


	
	// 根据等级条件，开启对应引导//
	public bool NewLevelUpEvent(int _level)
	{
		if(!mIsOpenTeach){return false;}

		return false;
	}
	
	public bool NewGUISingleDropFinishedEvent()
	{
		if(!mIsOpenTeach){return false;}
		
			

		
		return false;	
	}
	
	public bool NewTaskPassCopy(int taskId)
	{
		if(!mIsOpenTeach){return false;}
		return false;
	}
	
	public bool  NewBattleEndEvent()
	{
		if(!mIsOpenTeach){return false;}

		return false;
	}
	
	// 根据接收任务条件，开启对应引导//
	public bool NewTaskAcceptedEvent(int _taskId)
	{
		if(!mIsOpenTeach){return false;}
		
		Debug.Log("NewTaskAcceptedEvent_taskID: "+_taskId);
		
		if(_taskId == 10000401 || _taskId == 10000403 || _taskId == 10000412)
		{
			NewOpenTeach("x03");
		}
		if(_taskId == 10000300)
		{
			if(NewGetTeachStep("x01") == 0)
			{
				NewWriteSaveData("x01", TeachFinishedValue);
			}
		}
		return false;
	}
	
	// 根据完成任务条件，开启对应引导//
	
	public bool NewTaskFinishedEvent(int _taskId)
	{
		if(!mIsOpenTeach){return false;}
		
		Debug.Log("NewTaskFinishedEvent_taskID: "+_taskId);
		if (_taskId == 10000500)
		{
			NewOpenTeach("x04");
		}

		if (_taskId == 10000600)
		{
			NewOpenTeach("x06");
		}

//		if (_taskId == 10000700)
//		{
//			NewOpenTeach("x09");
//		}
		return false;
	}


	public bool NewDateChangeEvent(int date)
	{
		if(!mIsOpenTeach){return false;}
		
		Debug.Log("NewDateChangeEvent: "+date);
		if (date == 1)
		{
			NewOpenTeach("x07");
		}
		if (date == 2)
		{
			NewOpenTeach("x08");
		}
		if(date == 3)
		{
			NewOpenTeach("x10");
		}
		if (date == 7)
		{
			NewOpenTeach("x09");
		}
	
		return false;
	}

	public bool NewEnterBattleEvent(int _copyId)
	{
		if(!mIsOpenTeach){return false;}
		

		return false;
	}
	
	///拍照细节的引导///
	private string CMD_X03_Path = "UINoAtlas/Beizhu0"; 
	private string CMD_X03_1 = "GUIPhotoGraph/BgInfo/ScoreNum";
	private string CMD_X03_2 = "GUIPhotoGraph/BgInfo/conditionGirl/IngrObject/Ingr1";
	private string CMD_X03_3 = "GUIPhotoGraph/BgInfo/Information";
	private string CMD_X03_4 = "GUIPhotoGraph/BgInfo/LeftStep";
	private string CMD_X03_5 = "GUIPhotoGraph/PropInfo/UIGrid/PropItemJacket";
	private string CMD_X03_6 = "GUIPhotoGraph/ReadyInfo/StartBtn";


	private void NewTeachX03()
	{
		string key = "x03";
		int value = NewGetTeachStep(key);

		if(value == 1)
		{
			GUIPhotoGraph photo = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph>();
			if(photo != null)
			{
				ClearTextureGuideDic();
				NewSetTextureGuidePos(key,1,CMD_X03_1,20f,-140f,0f,mNewJianTouC,CMD_X03_Path+"1",322,191);
			}

		}
		else if(value == 2)
		{
			NewSetTextureGuidePos(key,2,CMD_X03_2,150f,-140f,0f,mNewJianTouC,CMD_X03_Path+"2",322,191);
		}
		else if(value == 3)
		{
			NewSetTextureGuidePos(key,3,CMD_X03_3,-10f,-140f,0f,mNewJianTouC,CMD_X03_Path+"3",409,191,false,true);
		}
		else if(value == 4)
		{
			NewSetTextureGuidePos(key,4,CMD_X03_4,45f,-140f,0f,mNewJianTouC,CMD_X03_Path+"4",332,191);
		}
		else if(value == 5)
		{
			NewSetTextureGuidePos(key,5,CMD_X03_5,240f,200f,0f,mNewJianTouC,CMD_X03_Path+"5",758,310,false,true);
		}
		else if(value == 6)
		{
			NewSetJianTouPos(key,TeachFinishedValue,CMD_X03_6,400f,150f,new Vector3(0f,-405f,0f),new Vector3(-250f,80f,0f),new Vector3(0f,0f,-20f));
		}
		else if(value == 7)
		{
//			NewSetTextureGuidePos(key,7,CMD_X03_2,430f,15f,0f,mNewJianTouC,CMD_X03_Path+"5",683,127);
		}
		else if(value == 8)
		{

		}
		else if(TeachFinishedValue == value)
		{

		}
	}
	
	
	///服装店 -- //
	private string CMD_X04_1 = "GUIMain/RightControl/TweenObj/ButtonFuzhaungdian"; 
	private string CMD_X04_2 = "GUIClothShop/ShopClothView/DrawClothingStore/ClothGameObject/ClothTypeInfo/ClothTypeIconScroll/ClothTypeIconGrid/ClothTypeItem1";
	private string CMD_X04_3 = "GUIClothShop/ShopClothView/DrawClothingStore/ClothGameObject/ASingleTypeOfClothInfo/ASingleTypeOfClothScroll/ClothGrid/";
	private string CMD_X04_4 = "GUIClothShop/ShopClothView/DrawClothingStore/ClothGameObject/ClothInfoMation/BuyClothBtn";
	private string CMD_X04_5 = "GUIClothShop/ShopClothView/DrawClothingStore/HomeBtn";
	private void NewTeachX04()
	{
		string key = "x04";
		int value = NewGetTeachStep(key);
		if (value > 0)
		{
			
		}
		if(value == 1)
		{
			NewSetJianTouPos(key,1,CMD_X04_1,220f,220f,new Vector3(460f,560f,0f),new Vector3(-150f,50f,0f),new Vector3(0f,0f,-20f),false);
		}
		else if(value == 2)
		{
			GUIClothShop guiClothShop = Globals.Instance.MGUIManager.GetGUIWindow<GUIClothShop>();
			if(guiClothShop != null && guiClothShop.IsVisible)
			{
				NewSetJianTouPos(key,2,CMD_X04_2,120f,170f,new Vector3(-280f,30f,0f),new Vector3(170f,70f,0f),new Vector3(0f,180f,-30f),false);
			}
		}
		else if(value == 3)
		{
			GUIClothShop guiClothShop = Globals.Instance.MGUIManager.GetGUIWindow<GUIClothShop>();
			if(guiClothShop != null && guiClothShop.IsVisible)
			{
				Transform tr = guiClothShop.getTeachClothFromLogicID(11202405);
				NewSetJianTouPos(key,3,CMD_X04_3 + tr.name,200f,350f,new Vector3(-400f + tr.localPosition.x,-292f + tr.localPosition.y,0f),new Vector3(170f,30f,0f),new Vector3(0f,180f,-25f),false);
			}
		}
		else if(value == 4)
		{
			NewSetJianTouPos(key,4,CMD_X04_4 ,210f,140f,new Vector3(350f,240f,0f),new Vector3(140f,30f,0f),new Vector3(0f,180f,-25f));
		}
		else if(value == 5)
		{
			NewSetJianTouPos(key,5,CMD_X04_5 ,210f,210f,new Vector3(425f,740f,0f),new Vector3(-160f,30f,0f),new Vector3(0f,0f,-25f),false);
		}
		else if(value == 6)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				ShowTalkObj(7006,key,TeachFinishedValue,true);
			}
		}
		else if(TeachFinishedValue == value)
		{
			NewOpenTeach("x05");
		}
	}
	
	///换装///

	private string CMD_X05_1 = "GUIMain/RightControl/TweenObj/ButtonHuanzhuang";
	private string CMD_X05_2 = "GUIChangeCloth/RightFrame/ClothingCategoriesFrame/CFMove/Scroll View/Table/1";
	private string CMD_X05_3 = "GUIChangeCloth/RightFrame/ClothingListFrame/CLFMove/Scroll View/Table/";
	private string CMD_X05_4 = "GUIChangeCloth/BackBtn";
	private string CMD_X05_5 = "GUIChangeCloth/LeftFrame/SaveBtn";
	private string CMD_X05_6 = "GUIChangeCloth/RightFrame/ExitBtn";
	private string CMD_X05_7 = "GUIChangeCloth/RightFrame/ClothingCategoriesFrame/CFMove/Scroll View/Table/3";

	private void NewTeachX05()
	{
		string key = "x05";
		int value = NewGetTeachStep(key);

		if(value == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
				NewSetJianTouPos(key,1,CMD_X05_1 ,210f,210f,new Vector3(461f,771f,0f),new Vector3(-200f,60f,0f),new Vector3(0f,0f,-25f),false);
		}
		else if(value == 2)
		{
			GUIChangeCloth guiChangeCloth = Globals.Instance.MGUIManager.GetGUIWindow<GUIChangeCloth>();
			if(guiChangeCloth != null && guiChangeCloth.IsVisible)
			{
				NewSetJianTouPos(key,2,CMD_X05_2 ,240f,110f,new Vector3(430f,346f,0f),new Vector3(-250f,30f,0f),new Vector3(0f,0f,-27f),false);
			}
		}
		else if(value == 3)
		{
			GUIChangeCloth guiChangeCloth = Globals.Instance.MGUIManager.GetGUIWindow<GUIChangeCloth>();
			if(guiChangeCloth != null && guiChangeCloth.IsVisible)
			{
				Transform tr = guiChangeCloth.getTeachClothFromLogicID(11202405);

				NewSetJianTouPos(key,3,CMD_X05_3 + tr.name,260f,260f,new Vector3(330f + tr.localPosition.x,600f + tr.localPosition.y,0f),new Vector3(-250f,30f,0f),new Vector3(0f,0f,-20f),false);
			}
		}
		else if(value == 4)
		{
			NewSetJianTouPos(key,4,CMD_X05_4,560f,300f,new Vector3(-80f,-920f,0f),new Vector3(-250f,30f,0f),new Vector3(0f,0f,-20f),false);
		}
		else if(value == 5)
		{
			GUIChangeCloth guiChangeCloth = Globals.Instance.MGUIManager.GetGUIWindow<GUIChangeCloth>();
			if(guiChangeCloth != null && guiChangeCloth.IsVisible)
			{
				NewSetJianTouPos(key,5,CMD_X05_7 ,240f,110f,new Vector3(430f,40f,0f),new Vector3(-250f,30f,0f),new Vector3(0f,0f,-27f),false);
			}
		}
		else if(value == 6)
		{
			GUIChangeCloth guiChangeCloth = Globals.Instance.MGUIManager.GetGUIWindow<GUIChangeCloth>();
			if(guiChangeCloth != null && guiChangeCloth.IsVisible)
			{
				Transform tr = guiChangeCloth.getTeachClothFromLogicID(11203408);
				
				NewSetJianTouPos(key,6,CMD_X05_3 + tr.name,260f,260f,new Vector3(330f + tr.localPosition.x,600f + tr.localPosition.y,0f),new Vector3(-250f,30f,0f),new Vector3(0f,0f,-20f),false);
			}
		}
		else if(value == 7)
		{
			NewSetJianTouPos(key,7,CMD_X05_5,200f,200f,new Vector3(-465f,-685f,0f),new Vector3(220f,30f,0f),new Vector3(0f,-180f,-20f));
		}
		else if(value == 8)
		{
			NewSetJianTouPos(key,TeachFinishedValue,CMD_X05_6,200f,200f,new Vector3(435f,740f,0f),new Vector3(-220f,30f,0f),new Vector3(0f,0f,-20f));
		}
		else if(TeachFinishedValue == value)
		{

		}
	}
	
	///指向训练的引导///
	private string CMD_X06_1 = "GUIMain/DownControl/FunctionBtn/TrainingBtn";
	private string CMD_X06_2 = "GUITrain/BaseInformation/UIScrollView/UIGrid/TrainItem1/TrainBtn";
	private string CMD_X06_3 = "GUITrain/BaseInformation/HomeBtn";
	private void NewTeachX06()
	{
		string key = "x06";
		int value = NewGetTeachStep(key);
			

		if(value == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				NewSetJianTouPos(key,1,CMD_X06_1,240f,180f,new Vector3(-425f,-912f,0f),new Vector3(200f,80f,0f),new Vector3(0f,-180f,-25f),false);
			}
		}
		else if(value == 2)
		{
			GUITrain guiTrain = Globals.Instance.MGUIManager.GetGUIWindow<GUITrain>();
			if(guiTrain != null && guiTrain.IsVisible)
			{
				NewSetJianTouPos(key,2,CMD_X06_2,300f,160f,new Vector3(323f,70f,0f),new Vector3(-200f,30f,0f),new Vector3(0f,0f,-15f));
			}

		}
		else if(value == 3)
		{
			// 在训练  流程显示结束 来设置  -- //
		}
		else if(value == 4)
		{
			ShowTalkObj(7009,key,4,true);
		}
		else if(value == 5)
		{
			NewSetJianTouPos(key,TeachFinishedValue,CMD_X06_3,230f,220f,new Vector3(434f,740f,0f),new Vector3(-200f,30f,0f),new Vector3(0f,0f,-20f));
		}
		else if(TeachFinishedValue == value)
		{

		}
	}
	
	///打工///
	private string CMD_X07_1 = "GUIMain/DownControl/FunctionBtn/JobBtn";
	private string CMD_X07_2 = "GUIJob/BaseInformation/JobPlaceUIScrollView/UIGrid/JobPlaceItem1/GoIntoBtn";
	private string CMD_X07_3 = "GUIJob/BaseInformation/HomeBtn";
	private string CMD_X07_4 = "GUIJob/BaseInformation/JobInformation/JobUIScrollView/UIGrid/";
	private void NewTeachX07()
	{
		string key = "x07";
		int value = NewGetTeachStep(key);
				
		if(value == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				NewSetJianTouPos(key,1,CMD_X07_1,240f,180f,new Vector3(-143f,-912f,0f),new Vector3(200f,80f,0f),new Vector3(0f,-180f,-25f),false);
			}
		}
		else if (value == 2)
		{
			GUIJob guiJob = Globals.Instance.MGUIManager.GetGUIWindow<GUIJob>();
			if(guiJob != null && guiJob.IsVisible)
			{
				NewSetJianTouPos(key,2,CMD_X07_2,300f,1500f,new Vector3(300f,64f,0f),new Vector3(-200f,30f,0f),new Vector3(0f,0f,-20f),false);
			}
		}
		else if(value == 3)
		{
			GUIJob guiJob = Globals.Instance.MGUIManager.GetGUIWindow<GUIJob>();
			if(guiJob != null && guiJob.IsVisible)
			{
				Transform tr = guiJob.getTeachJobFromJobID(1001);
				NewSetJianTouPos(key,3,CMD_X07_4 + tr.name + "/WorkBtn",250f,120f,new Vector3(308f+tr.localPosition.x,-5f + tr.localPosition.y,0f),new Vector3(-200f,30f,0f),new Vector3(0f,0f,-20f));
			}
		}
		else if(value == 4)
		{
			
		}
		else if(value == 5)
		{
			ShowTalkObj(7010,key,5,true);
		}
		else if(value == 6)
		{
			NewSetJianTouPos(key,TeachFinishedValue,CMD_X07_3,180f,180f,new Vector3(434f,740f,0f),new Vector3(-200f,30f,0f),new Vector3(0f,0f,-20f));
		}
		else if (TeachFinishedValue == value)
		{

		}
	}
	
	///休息///
	private string CMD_X08_1 = "GUIMain/DownControl/FunctionBtn/RestBtn";
	private void NewTeachX08()
	{
		string key = "x08";
		int value = NewGetTeachStep(key);
		if(value == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				ShowTalkObj(7007,key,1);
			}
		}
		else if(value == 2)
		{
			NewSetJianTouPos(key,TeachFinishedValue,CMD_X08_1,240f,180f,new Vector3(423f,-912f,0f),new Vector3(-200f,80f,0f),new Vector3(0f,0f,-25f));
		}
	}
	
	///旅游///
	private string CMD_X09_1 = "GUIMain/DownControl/FunctionBtn/TravelBtn";

	private void NewTeachX09()
	{
		string key = "x09";
		int value = NewGetTeachStep(key);
		
		if(value == 1)
		{	
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				ShowTalkObj(7008,key,TeachFinishedValue , true);
			}
		}
		else if(value == TeachFinishedValue)
		{
			NewOpenTeach("x21");
		}
	}

	// 备忘录引导//
	private string CMD_X10_1 = "GUIMain/LeftControl/TweenObj/TopLabel/InfoBtn";

	private void NewTeachX10()
	{
		string key = "x10";
		int value = NewGetTeachStep(key);
		if(value  == 1)
		{
			GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			if(guiMain != null && guiMain.IsVisible)
			{
				NewSetJianTouPos(key,1,CMD_X10_1,300,100f,new Vector3(-408f,720f,0f),new Vector3(200f,30f,0f),new Vector3(0f,-180f,-25f));
			}
		}
		else if(value == 2)
		{
			GUIPlayer guiPlayer = Globals.Instance.MGUIManager.GetGUIWindow<GUIPlayer>();
			if(guiPlayer != null && guiPlayer.IsVisible)
			{
				ShowTalkObj(7013,key,TeachFinishedValue , true);
			}
		}
	}
	

	private void NewTeachX21()
	{
		string key = "x21";
		int value = NewGetTeachStep(key);
		if(value > 0)
		{
			mIsOpenTeach = false;
			GUIBeginnersGuide.Hide();
			if(NewGetTeachStep("x01") == 0)
			{
				NewWriteSaveData("x01", TeachFinishedValue);
			}
		}
	}
	
	// 处理按钮点击//
	public  void NewTriggerSave(string _fullName, int type, float val = 0.0f)
	{
		string key = "";
		int curStep = 0;
		int nextStep = 0;
		JianTouObject tCurJianTouObject = null;
		
		for(int i = 0; i < mJianTouNameList.Count; i++)
		{
			JianTouObject tJianTouObject = mJianTouNameList[i];
			
			if(tJianTouObject.JianTouName == _fullName+tJianTouObject.Key+tJianTouObject.Value)
			{
				tCurJianTouObject = tJianTouObject;

				if(getGUIBeginnerGuide() != null&&!tJianTouObject.IsCloseGuide)
					getGUIBeginnerGuide().HideArrowBootMode();
				else 
					GUIBeginnersGuide.Hide();
				
				key = tJianTouObject.Key;
				curStep = tJianTouObject.Value;
				if(curStep == TeachFinishedValue)
				{
					curStep = TeachFinishedValue - 1;
					nextStep = TeachFinishedValue;
					if(getGUIBeginnerGuide() != null)
					{
						if(tJianTouObject.IsCloseGuide)
							getGUIBeginnerGuide().HideArrowBootMode();
						else
							GUIBeginnersGuide.Hide();
					}
					
				}
				else if(curStep == TeachXunHuanValue)
				{
					NewWriteSaveData(key, 0);
					mJianTouNameList.RemoveAt(i);
					return;
				}
				else
				{
					nextStep = curStep + 1;
				}
				
				mJianTouNameList.RemoveAt(i);
				
				break;
			}
		}
		
		if(nextStep == 0 || curStep == 0) return;
		
		if(nextStep > curStep)
		{
			NewWriteSaveData(key, nextStep, tCurJianTouObject.IsSaveToServer);
			NewRefreshAllTeach();
		}
	}
	
	
	// 获得引导的当前步骤-1 不存在 0 未开启 >0开启了//
	public int NewGetTeachStep(string key)
	{
		if(!mNewTeachDataDic.ContainsKey(key) || !mIsOpenTeach)
		{
			return -1;
		}
		return mNewTeachDataDic[key];
	}
	
	// 开启引导//
	public void NewOpenTeach(string key,int step = 1)
	{
		int newValue = NewGetTeachStep(key);
		if(newValue == 0)
		{
			NewWriteSaveData(key, step);
			
			NewRefreshAllTeach();
		}
	}
	
	// 刷新全部引导//
	private void NewRefreshAllTeach()
	{
		if(!mIsOpenTeach){return;}

		mGUIMainDir = ETeachGUIMainDir.ListNo;

		NewTeachX03();
		NewTeachX04();
		NewTeachX05();
		NewTeachX06();
		NewTeachX07();
		NewTeachX08();
		NewTeachX09();
		NewTeachX10();
		NewTeachX21();

		RefreshMainFunctionBtn();
	}
	
	/// <summary>
	/// The mMiJiNum attribute value
	/// </summary>
	public static readonly string MI_JI_TEACH_OPEN	= "Teach";
	public static readonly string MI_JI_TEACH_CLOSE = "NoTeach";
	
	public static string mMiJiNum = "...";
	
	public bool checkAcceptCondition()
	{
		return true;
	}

	
	public void RefreshMainFunctionBtn ()
	{
		GUIMain guiMain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
		if(guiMain != null)
		{
			guiMain.FunctionButtonState();
		}
	}

	public void SetTeachStep(string key, int newValue , bool isSave = false)
	{
		if(isSave)
		{
			NewWriteSaveData(key,newValue,true);
		}
		else
		{
			mNewTeachDataDic[key] = newValue;
		}

		NewRefreshAllTeach();
	}
	
	// 响应按钮等触发，引导至下一步//
	public void NewButtonTapEvent(GameObject _btn)
	{		
		if(!mIsOpenTeach){return;}
		
		Debug.Log("NewButtonTapEvent" + _btn.name);
		
		string name = GetFullPathFrmGO(_btn.gameObject);
		
		Debug.Log("NewButtonTapEvent" + name);
		
		// 1 单击触发类型//
		NewTriggerSave(name, 1);
	}
	
	public void NewBuildingClickedEvent(string _name)
	{
		if(!mIsOpenTeach){return;}
		
		// 0 建筑物触发类型//
		NewTriggerSave("EZ3DItemParent/"+_name, 0);
	}
	
	public void NewButtonnDoubleEvent(GameObject obj)
	{
		if(!mIsOpenTeach){return;}
		
		//if(obj.enabled)
		//{
		//	string name = GetFullPathFrmGO(obj.gameObject);
		//	// 2 双击触发类型//
		//	NewTriggerSave(name, 2);
		// }
	}
	
	public void NewBtnDragEvent(GameObject obj, float val)
	{
		if(!mIsOpenTeach){return;}
		
		//if(obj.enabled)
		//{
		//	string name = GetFullPathFrmGO(obj.gameObject);
		//	// 2 双击触发类型//
		//	NewTriggerSave(name, 2, val);
		//}
	}
	
	// 隐藏显示Obj//
	private bool NewSetVisableObj(string _fullName, bool _isShow)
	{
		Transform goTran = mRootTran.Find(_fullName); 
		if(null != goTran)
		{
			NGUITools.SetActive(goTran.gameObject,_isShow);
		}
		return false;
	}
	
	
	public  void NewSetAllColliderDisabledExceptTeach(string name)
	{	
//		BoxCollider[] bcs = FindObjectsOfType(typeof(BoxCollider)) as BoxCollider[];
//		
//		if(bcs == null)
//			return;
//		
//		
//		for(int i = 0; i < bcs.Length; i++)
//		{
//			if(bcs[i].name == name)
//			{
//				bcs[i].enabled = true;
//
//			}
//			else{
//				bcs[i].enabled = false;
//			}
//		}
	}
	
	public  void NewSetColliderState(string name, bool state)
	{	
//		BoxCollider[] bcs = FindObjectsOfType(typeof(BoxCollider)) as BoxCollider[];
//		
//		if(bcs == null)
//			return;
//		
//		for(int i = 0; i < bcs.Length; i++)
//		{
//			if(bcs[i].name == name)
//			{
//				bcs[i].enabled = state;
//			}
//		}
	}
	
	private void NewSetAllColliderEnabled()
	{	
//		BoxCollider[] bcs = FindObjectsOfType(typeof(BoxCollider)) as BoxCollider[];
//		
//		if(bcs == null)
//			return;
//		
//		for(int i = 0; i < bcs.Length; i++)
//		{
//			bcs[i].enabled = true;
//		}
	}
	
	// 隐藏显示Collider//
	private void NewSetUIColliderEnabled(string name, bool _isShow)
	{	
		GameObject go= GameObject.Find(name);
		if(null != go)
		{
			BoxCollider boxCollider = go.GetComponent<BoxCollider>();
			boxCollider.enabled = _isShow;
		}
	}
	
	/// <summary>
	/// 在背包,换装，衣服商店的界面需要将场景的图标隐藏
	/// </summary>
	public void NewSetAllBuildingColliderEnabled(bool isEnabled)
	{
		Dictionary<int, Building> holdBuildingList =  GameStatusManager.Instance.MPortStatus.GetHoldBuildingList();
		if(holdBuildingList == null)
			return ;
		foreach(int key in holdBuildingList.Keys)
		{
			if (isEnabled)
				 holdBuildingList[key].U3DGameObject.transform.parent.localScale = Vector3.one;
			else
				holdBuildingList[key].U3DGameObject.transform.parent.localScale = Vector3.zero;
		}
	}
	
	
	private delegate void CreateJianTouDelEvent(JianTouObject tJianTouObject);
	
	// 创建引导箭头//
	private bool NewSetJianTouPos(string _key, int _step, string _fullName,float curWidth,float curHeight,Vector3 correction,Vector3 arrowPositon,Vector3 arrowRotate, bool IsSaveToServer = true,int zoomType = 0,bool IsCloseGuide = false,bool isScroll = false ,float correctionScale = 1, CreateJianTouDelEvent _CreateJianTouDelEvent = null)
	{
		if(!mIsOpenTeach){return false;}
		
		if(null == mRootTran)
		{
			return false;
		}
		WaitFind(_key,_step,_fullName,curWidth,curHeight,arrowPositon,arrowRotate,zoomType,IsSaveToServer ,isScroll , _CreateJianTouDelEvent, correctionScale, correction, IsCloseGuide);
		return true;
	}
	
	void WaitFind(string _key, int _step, string _fullName,float curWidth,float curHeight,Vector3 arrowPositon,Vector3 arrowRotate, int zoomType, bool IsSaveToServer ,bool isScroll , CreateJianTouDelEvent _CreateJianTouDelEvent,float correctionScale,Vector3 correction,bool IsCloseGuide)
	{
		Transform targetTran = null;
		targetTran = mRootTran.Find(_fullName);
		if(targetTran != null)
		{
			for(int i = 0; i < mJianTouNameList.Count; i++)
			{
				if(mJianTouNameList[i].JianTouName == (_fullName+_key+_step))
				{
					mJianTouNameList.RemoveAt(i);
				}
			}
			
			if(isScroll)
			{
				UIDragScrollView scroll = targetTran.GetComponent<UIDragScrollView>();
				if(scroll != null)
				{
					scroll.enabled = false;
				}
			}

			getGUIBeginnerGuide().ShowArrowBootMode(correction,correctionScale,curWidth,curHeight,arrowPositon,arrowRotate,zoomType);
	
			JianTouObject tJianTouObject = new JianTouObject();
			tJianTouObject.JianTouName = _fullName+_key+_step;
			tJianTouObject.Key = _key;
			tJianTouObject.Value = _step;

			tJianTouObject.IsCloseGuide = IsCloseGuide;
			tJianTouObject.IsSaveToServer = IsSaveToServer;
			mJianTouNameList.Add(tJianTouObject);
			
			NGUITools.SetActive(mTalkTargetObj,false);

			if(_CreateJianTouDelEvent != null)
			{
				_CreateJianTouDelEvent(tJianTouObject);
			}
		}
	}
	
	private GUIMain NewGetGUIMain()
	{
		Transform guiMainTran = mRootTran.Find("GUIMain");
		if(guiMainTran != null)
		{
			GUIMain gui = guiMainTran.GetComponent<GUIMain>();
			return gui;
		}
		return null;
	}
	
	private string GetFullPathFrmGO(GameObject go)
	{
		string t_name = null;
		if (go.tag == "Gfan_Npc")
		{
				t_name = "Building:" +  go.transform.name;
		}
		else
		{
			Transform t_transform = go.transform;
			
			while(t_transform.name != "UICamera")
			{
				if(t_name != null)
				{
					t_name = t_transform.name + "/" + t_name;
				}else
				{
					t_name = t_transform.name;
				}
				
				t_transform = t_transform.parent;
				
				if(t_transform == null)
				{
					break;
				}
			}
		}
		return t_name;
	}
	
	public void ReceiveOK(string key, int newValue)
	{
		mNewTeachDataDic[key] = newValue;
	}
	
	// 本地数据的更改//
	public void NewWriteSaveData(string detkey, int newValue, bool IsSaveToServer = true)
	{	
		if(!mNewTeachDataDic.ContainsKey(detkey) || mNewTeachDataDic[detkey] != TeachFinishedValue)
		{
			mNewTeachDataDic[detkey] = newValue;
			if(IsSaveToServer)
			{
				NetSender.Instance.RequestPlayerBeginnerFinish(detkey, newValue);
			}
		}
	}

	
	private Building NewGetBuildingObj(string name)
	{
		Dictionary<int, Building> holdBuildingList =  GameStatusManager.Instance.MPortStatus.GetHoldBuildingList();
		if(holdBuildingList == null)
			return null;
		foreach(int key in holdBuildingList.Keys)
		{
			if(holdBuildingList[key].U3DGameObject.name == name)
			{
				return holdBuildingList[key] ;
			}
		}
		return null;
	}
	
	struct TextureTeachStepInfo
	{
		public string teachID;
		public int stepID;
		public bool delObj;
		public string objName;
		public bool saveToserver;
	};
	Dictionary<string,GameObject> TextureGuideDic = new Dictionary<string, GameObject>();
	GameObject guideTextureObj = null;
	private bool NewSetTextureGuidePos(string _key, int _step, string _fullName, float _offX, float _offY, float _offZ, GameObject _srcPrefab, string _texturePath,int _width,int _height, bool isSaveToServer = false,bool _isDel = false)
	{
		if(null == mRootTran)
		{
			return false;
		}
		UIButton btn = null;
		guideTextureObj = null;
		
		if(guideTextureObj == null)
		{
			guideTextureObj = (GameObject)Instantiate(_srcPrefab) as GameObject;
			guideTextureObj.transform.parent = mRootTran.Find(_fullName);
			guideTextureObj.transform.localPosition = new Vector3(_offX,_offY,_offZ);
			guideTextureObj.transform.localScale = Vector3.one;
			btn = guideTextureObj.GetComponent<UIButton>();
			UIEventListener.Get(btn.gameObject).onClick  += OnClickGuideTexture;
		}
		if(btn == null)
		{
			btn = guideTextureObj.GetComponent<UIButton>();
		}

		guideTextureObj.name = _fullName+_key+_step;
	
		TextureTeachStepInfo teachInfo  = new TextureTeachStepInfo();
		teachInfo.teachID = _key;
		teachInfo.stepID = _step;
		teachInfo.delObj = _isDel;
		teachInfo.objName = guideTextureObj.name;
		teachInfo.saveToserver = isSaveToServer;
		btn.Data = teachInfo;
		
		UITexture tex = guideTextureObj.GetComponent<UITexture>();
		tex.mainTexture = Resources.Load(_texturePath,typeof(Texture2D)) as Texture2D;
		tex.width = _width;
		tex.height = _height;
		bool isHas = false;
		foreach(KeyValuePair<string,GameObject> item in TextureGuideDic)
		{
			if(item.Key == guideTextureObj.name)
			{
				isHas = true;
			}
		}
		if(!isHas)
		{
			TextureGuideDic.Add(guideTextureObj.name,guideTextureObj);
		}
		return true;
	}
	
	private void OnClickGuideTexture(GameObject obj)
	{
		BoxCollider box = obj.GetComponent<BoxCollider>();
		Destroy(box);
		TextureTeachStepInfo teachInfo  = (TextureTeachStepInfo)obj.GetComponent<UIButton>().Data;
		int curStep = teachInfo.stepID;
		string 	key = teachInfo.teachID;
		int nextStep = 1;
		
		if(teachInfo.delObj)
		{
			ClearTextureGuideDic();
		}
		
		if (curStep == TeachFinishedValue+1)
		{
			return;
		}
		if(curStep == TeachFinishedValue)
		{
			curStep = TeachFinishedValue - 1;
			nextStep = TeachFinishedValue;
		}
		else if(curStep == TeachXunHuanValue)
		{
			NewWriteSaveData(key, 0);
			return;
		}
		else
		{
			nextStep = curStep + 1;
		}
			
		if(nextStep == 0 || curStep == 0) return;
		
		if(nextStep > curStep)
		{
			NewWriteSaveData(key, nextStep,teachInfo.saveToserver);
			NewRefreshAllTeach();
		}
	}
	
	private void ClearTextureGuideDic()
	{
		foreach(KeyValuePair<string,GameObject> item in TextureGuideDic)
		{
			GameObject.DestroyObject(item.Value);
		}
		TextureGuideDic.Clear();
	}
	
	struct TeachStepInfo
	{
		public string teachID;
		public int stepID;
		public bool saveToserver;
	};
	
	private void ShowTalkObj(int wordID,string teachID,int stepID,bool saveToserver=false)
	{
		Transform targetTran = Globals.Instance.M3DItemManager.EZ3DItemParent;
		
		UIButton dialogBtn = null;
		if(mTalkTargetObj ==  null)
		{
			mTalkTargetObj = (GameObject)Instantiate(mTalkObjectPrefab) as GameObject;
			mTalkTargetObj.transform.parent = targetTran;
			mTalkTargetObj.transform.localPosition = new Vector3(0,0,10);
			mTalkTargetObj.transform.localScale = Vector3.one;
			dialogBtn = mTalkTargetObj.transform.Find("BG").GetComponent<UIButton>();
			UIEventListener.Get(dialogBtn.gameObject).onClick += OnTalkBtnClick;
		}
		
		if (dialogBtn == null)
			dialogBtn = mTalkTargetObj.transform.Find("BG").GetComponent<UIButton>();
		TeachStepInfo teachInfo  = new TeachStepInfo();
		teachInfo.teachID = teachID;
		teachInfo.stepID = stepID;
		teachInfo.saveToserver = saveToserver;
		dialogBtn.Data = teachInfo;
		
		
		NGUITools.SetActive(mTalkTargetObj,true);
		UILabel uiLabel = mTalkTargetObj.transform.Find("UILable").GetComponent<UILabel>();
		uiLabel.text = Globals.Instance.MDataTableManager.GetWordText(wordID);

		GUIBeginnersGuide.Hide();
	}
	
	private void OnTalkBtnClick(GameObject obj)
	{
		NGUITools.SetActive(mTalkTargetObj,false);
		
		TeachStepInfo teachInfo = (TeachStepInfo)obj.GetComponent<UIButton>().Data;
		int curStep = teachInfo.stepID;
		string 	key = teachInfo.teachID;
		int nextStep = 1;
		
		if (curStep == TeachFinishedValue+1)
		{
			NewTalkBtnTeachFinished(key);
			return;
		}
		
		if(curStep == TeachFinishedValue)
		{
			curStep = TeachFinishedValue - 1;
			nextStep = TeachFinishedValue;
		}
		else if(curStep == TeachXunHuanValue)
		{
			NewWriteSaveData(key, 0);
			return;
		}
		else
		{
			nextStep = curStep + 1;
		}
			
		if(nextStep == 0 || curStep == 0) return;
		
		if(nextStep > curStep)
		{
			NewWriteSaveData(key, nextStep,teachInfo.saveToserver);
			NewRefreshAllTeach();
		}
		
		
	}

	private GUIBeginnersGuide getGUIBeginnerGuide()
	{
		if (guiBeginnersGuide == null)
		{
			guiBeginnersGuide = Globals.Instance.MGUIManager.GetGUIWindow<GUIBeginnersGuide>();
		}
		return guiBeginnersGuide;
	}

	private void NewTalkBtnTeachFinished(string keyStr)
	{
		
	
	}
	
	public void NewCheckExceptionKey()
	{
		if(!mIsOpenTeach){return;}
		
		mHasCheckException = true;
		int stepValueNext = this.NewGetTeachStep("x03");
		if(stepValueNext >= 4)
		{
			NewWriteSaveData("x03",TeachFinishedValue);
		}

		stepValueNext = this.NewGetTeachStep("x04");
		if(stepValueNext >= 4)
		{
			NewWriteSaveData("x04",TeachFinishedValue);
		}

		stepValueNext = this.NewGetTeachStep("x05");
		if(stepValueNext >= 6)
		{
			NewWriteSaveData("x05",TeachFinishedValue);
		}

		stepValueNext = this.NewGetTeachStep("x06");
		if(stepValueNext >= 3)
		{
			NewWriteSaveData("x06",TeachFinishedValue);
			NewOpenTeach("x07");
		}

		stepValueNext = this.NewGetTeachStep("x07");
		if(stepValueNext >= 3)
		{
			NewWriteSaveData("x07",TeachFinishedValue);
			NewOpenTeach("x08");
		}
		stepValueNext = this.NewGetTeachStep("x10");
		if(stepValueNext > 1)
		{
			NewWriteSaveData("x10",TeachFinishedValue);
		}

		NewRefreshAllTeach();
		
	}

	public void ClearCacheTeachDataDic()
	{
		mNewTeachDataDic.Clear();
//		GameDefines.gReconnectTaskID = -1;
//		GameDefines.gReconnectBeginners = false;
//		GameDefines.gReconnectUnfinishList = false;
		if(mTalkTargetObj != null)
		{
			NGUITools.SetActive(mTalkTargetObj,false);
		}
		
	}

	private void MoveCameraToTargetBuilding(string name)
	{		
		Dictionary<int, Building> holdBuildingList =  GameStatusManager.Instance.MPortStatus.GetHoldBuildingList();
		if(holdBuildingList == null)
			return;
		foreach(int key in holdBuildingList.Keys)
		{
			if(holdBuildingList[key].U3DGameObject.name == name)
			{
				GameStatusManager.Instance.MPortStatus.MainCameraMoveTo(holdBuildingList[key].U3DGameObject,null);
				break;
			}
		}
	}
	
	/**
	 * tzz added
	 * first enter game done (read data from fileSystem)
	 * 
	 * @return bool		return true if first enter teaching is done
	 */ 
	public bool IsFirstEnterGameDone(){

		if(!m_firstEnterGameTeachingDone){
			
			// read from filesystem
			try{
				string tFilename = Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + FIRST_ENTER_GAME_TEACHING_DONE_FILE;
				FileStream t_file = new FileStream(tFilename,FileMode.Open,FileAccess.Read);
				t_file.Close();
				m_firstEnterGameTeachingDone = true;
				
			}catch(System.Exception ex){
				m_firstEnterGameTeachingDone = false;
				Debug.Log("IsFirstEnterGameDone is false " + ex.Message);
			}
		}
		
		return m_firstEnterGameTeachingDone;
	}
	
	public bool IsFirstEnterGame
	{
		get{return IsFirstEnterGameDone();}
		set
		{
			m_firstEnterGameTeachingDone = value;
			SetFirstEnterGameDone();
		}
	}
	
	
	/**
	 * tzz added
	 * set the first enter game done (write data to fileSystem)
	 */ 
	public void SetFirstEnterGameDone(){
		// S filesystem
		try{
			string tFilename = Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + FIRST_ENTER_GAME_TEACHING_DONE_FILE;
			FileStream t_file = new FileStream(tFilename,FileMode.Create,FileAccess.Write);
			t_file.Close();
		}catch(System.Exception ex){
			Debug.LogError( FIRST_ENTER_GAME_TEACHING_DONE_FILE + " create failed! Error: " + ex.Message);
		}
		
		m_firstEnterGameTeachingDone = true;
	}
	
	//! has read data from filesystem for first enter game teaching
	private bool m_readFirstEnterGameData 		= true;
	
	//! is first enter game teaching done
	private bool m_firstEnterGameTeachingDone	= false;
	
	//! the first-enter-game-teaching-done filename
	private static readonly string FIRST_ENTER_GAME_TEACHING_DONE_FILE = "EnterGameTeachingDone.txt";
	
	private List<JianTouObject> mJianTouNameList = new List<JianTouObject>();
	
	//! tzz added for buffer Enter game packet
	[HideInInspector]public Packet m_bufferEnterGamePacket = null;
	[HideInInspector]public Packet m_bufferCreatePlayerPacket = null;
	[HideInInspector]public Packet m_bufferRoleLevelUpPacket = null;
	
	private int mCopyKillMounsterCount = 0;

	private GUIBeginnersGuide guiBeginnersGuide = null;
	
}
