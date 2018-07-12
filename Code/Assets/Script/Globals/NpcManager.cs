using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcManager : MonoBehaviour 
{
	private int  mCurrentInteractBuildingLogicID ;
	// Use this for initialization
	void Start () 
	{
	}
	
	#region Building	
	public Building CreateBuilding(BuildingData data)
	{
		Building building = new Building2D(data);
		building.Initialize();
		
		_mBuildingList.Add(data.LogicID, building);
		int buildID = Globals.Instance.MGameDataManager.MActorData.BuildID;
		if(data.LogicID==buildID)
		{
			Vector3 vPos = building.U3DGameObject.transform.position;
			Quaternion qRot = building.U3DGameObject.transform.rotation;
			mGameObjPreGoWhereClone =  GameObject.Instantiate(mPrefabGoWhereClone,vPos,qRot)as GameObject;
			mGameObjPreGoWhereClone.transform.parent = building.U3DGameObject.transform.parent;
			mGameObjPreGoWhereClone.transform.localScale = Vector3.one;
			mGameObjPreGoWhereClone.SetActive(true);
			string strFaceName = Globals.Instance.MGameDataManager.MActorData.BasicData.AvatarName;
			mStrGoWhere= "Camera"+building.U3DGameObject.name;
			mNpcBuild = building;
			mNpcPreBuild = mNpcBuild;
		}
		
		return building;
	}
	
	public void RemoveBuilding(int buildingID)
	{
		Building building = null;
		
		if (!_mBuildingList.TryGetValue(buildingID, out building))
			return;
		
		RemoveBuilding(building);
	}
	
	public void Update()
	{
		if (mMainCameraAnimation != null && !mMainCameraAnimation.IsPlaying(mStrGoWhere))
		{    
			//zhifayicixiaoxi
			if(mbPlayingAni)
			{
				if(mbDatingPlay)
				{
					Globals.Instance.MNpcManager.mbDatingPlay = false;
					int girlID = Globals.Instance.MTaskManager.mCurDatingGirid;
					int storyID = Globals.Instance.MTaskManager.mCurTaskId;
//				    NetSender.Instance.RequestTaskAccept(storyID,girlID,true);			    
				}
				else
				{
					if(!mbDatingPlay)
				  		GameObject.Destroy(mGameObjPreGoWhere);
				
				    mNpcBuild.U3DGameObject.SetActive(true);
				    int nBuild = Globals.Instance.MGameDataManager.MActorData.BuildID;
				    NetSender.Instance.RequestCanMove(nBuild,mCurrentInteractBuildingLogicID);
				}
			
				mbPlayingAni = false;
			}
			
			if(mbPlayingAniReturn)
			{
				mbSuoPing = false;
			}
			
		}
	}
	public void RemoveBuilding(Building building)
	{
		if (null == building)
			return;
		
		building.Release();
		_mBuildingList.Remove(building.InstanceID);
	}
	
	public void RemoveAllBuilding()
	{
		foreach (Building building in _mBuildingList.Values)
		{ 
			building.Release();
		}
		_mBuildingList.Clear();
	}
	public bool InteractWithPickObject(GameObject npc)
	{
//		if (null == npc.Property)
//			return false;
		if (npc.name == "GoWhere")
		{
			if(Globals.Instance.MGameDataManager.MActorData.WealthData.Oil>=1)
			{   
				GameObject.Destroy(mGameObjPreGoWhere);
				mNpcBuild.U3DGameObject.SetActive(true);
				PlayCameraAnimation();
			   return true;
			}
			else{
				Globals.Instance.MGUIManager.ShowErrorTips(6901,true);
			}
			return false;
		}
		return false;
	}
	public void SetPlayerFace(bool bSucess)
	{
		if(bSucess)
		{
		    GameObject.Destroy(mGameObjPreGoWhereClone);
			Vector3 vPos = mNpcBuild.U3DGameObject.transform.position;
			Quaternion qRot = mNpcBuild.U3DGameObject.transform.rotation;
		    mGameObjPreGoWhereClone =  GameObject.Instantiate(mPrefabGoWhereClone,vPos,qRot)as GameObject;
			mGameObjPreGoWhereClone.transform.parent = mNpcBuild.U3DGameObject.transform.parent;
			mGameObjPreGoWhereClone.transform.localScale = Vector3.one;
			string strFaceName = Globals.Instance.MGameDataManager.MActorData.BasicData.AvatarName;

		}
		else
		{
			GameObject.Destroy(mGameObjPreGoWhereClone);
			Vector3 vPos = mNpcPreBuild.U3DGameObject.transform.position;
			Quaternion qRot = mNpcPreBuild.U3DGameObject.transform.rotation;
		    mGameObjPreGoWhereClone =  GameObject.Instantiate(mPrefabGoWhereClone,vPos,qRot)as GameObject;
			mGameObjPreGoWhereClone.transform.parent = mNpcPreBuild.U3DGameObject.transform.parent;
			mGameObjPreGoWhereClone.transform.localScale = Vector3.one;
			string strFaceName = Globals.Instance.MGameDataManager.MActorData.BasicData.AvatarName;

		}
	}
	public void setmbSuoPing(bool bTure)
	{
		mbSuoPing = bTure;
	}
	public bool getSuoPingState()
	{
		return mbSuoPing;
	}
	public void PlayCameraAnimation()
	{
		mbSuoPing = true;
		mbPlayingAni = true;
		mMainCameraAnimation = Globals.Instance.MSceneManager.mMainCamera.gameObject.GetComponent<Animation>();
		mMainCameraAnimation.Play(mStrGoWhere);
	}
	
	public void PlayeCameraAnimatonReturn()
	{   
		mbSuoPing = false;
//		mbPlayingAniReturn = true;
//		mMainCameraAnimation = Globals.Instance.MSceneManager.mMainCamera.gameObject.GetComponent<Animation>();
//		mMainCameraAnimation[mStrGoWhere].speed = -1;
//		mMainCameraAnimation[mStrGoWhere].time = mMainCameraAnimation[mStrGoWhere].length; 
//	    mMainCameraAnimation.Play(mStrGoWhere);

	}
	
	public bool InteractWith(Building npc)
	{
		if (null == npc.Property)
			return false;
		if(mbSuoPing)
			return false;
		if (mGameObjPreGoWhere != null)
		{
			GameObject.Destroy(mGameObjPreGoWhere);
			mGameObjPreGoWhere = null;
		}
		Globals.Instance.MTeachManager.NewBuildingClickedEvent(npc.U3DGameObject.name);
		
		if ((int)npc.Property.LogicID == 108 || (int)npc.Property.LogicID == 115)
		{
			
//			NetSender.Instance.RequestBeautyBillBoardShow();//
//			GUIRadarScan.Show();
			Globals.Instance.mShopDataManager.ShopPushGoodsID = Globals.Instance.mShopDataManager.XUANMEI;
			NetSender.Instance.RequestShopPushGoods(5201);	
			
			if (mNpcBuild != null && mNpcBuild.U3DGameObject != null)
			{
				mNpcBuild.U3DGameObject.SetActive(true);
				mNpcPreBuild = mNpcBuild;
			}
			
			return false;
		}
		
		int nBuild = Globals.Instance.MGameDataManager.MActorData.BuildID;
		if((int)npc.Property.LogicID == nBuild)
		{
			TaskManager.BuildTaskInfo buildTaskInfo = null;
			Globals.Instance.MTaskManager.BuildTaskInfoList.TryGetValue(nBuild,out buildTaskInfo);
		
			if (buildTaskInfo != null && buildTaskInfo.TaskID != 0)
			{
				int girlID = Globals.Instance.MTaskManager.mCurDatingGirid;
				//NetSender.Instance.RequestTaskGetAccept(girlID,(int)TaskManager.TaskTriggerEvent.STROLL ,(int)npc.Property.LogicID);
//				NetSender.Instance.RequestTaskAccept(buildTaskInfo.TaskID,girlID,false);
				Globals.Instance.MTaskManager.hasExploreTask = false;
				Globals.Instance.MTaskManager.exploreFinished = false;
			}
			else
			{
				NetSender.Instance.C2GSRequestExploreTask((int)npc.Property.LogicID);
			}

			Globals.Instance.MTaskManager.BuildingLablesList.Clear();
			Globals.Instance.MTaskManager.currentExplorePlaceID  = 0;
			mCurrentInteractBuildingLogicID = (int)npc.Property.LogicID;
			///当主角在一个建筑上,点了另一个建筑在出现了go的时候,不点go,返回来点主角所在的建筑的情况///
			if (nBuild != mNpcBuild.Property.LogicID)
			{
				mNpcBuild.U3DGameObject.SetActive(true);
			}
			mNpcBuild = npc;
			mNpcPreBuild = mNpcBuild;
			//SetPlayerFace(false);
			GUIRadarScan.Show();
		}
		else
		{		
			//show go
			Vector3 vPos = npc.U3DGameObject.transform.position;
			Quaternion qRot = npc.U3DGameObject.transform.rotation;
			mGameObjPreGoWhere =  GameObject.Instantiate(mPrefabGoWhere,vPos,qRot)as GameObject;
			mGameObjPreGoWhere.transform.parent = npc.U3DGameObject.transform.parent;
			mGameObjPreGoWhere.transform.localScale = Vector3.one;;
			npc.U3DGameObject.SetActive(false);
			
//			if (!Globals.Instance.MTeachManager.NewIsTeachFinished())
//			{
//				Globals.Instance.MTeachManager.NewSetAllColliderDisabledExceptTeach("GoWhere");
//			}
			if (mNpcBuild != null && mNpcBuild.U3DGameObject != null)
			{
				mNpcBuild.U3DGameObject.SetActive(true);
				mNpcPreBuild = mNpcBuild;
			}
			mNpcBuild = npc;
			mGameObjPreGoWhere.SetActive(true);
			mStrGoWhere= "Camera"+npc.U3DGameObject.name;
			mCurrentInteractBuildingLogicID = (int)npc.Property.LogicID;	
		}
		
		return true;
	}
	
	public EZ3DSimipleText GetBuildingName(string name)
	{
		Transform ez3dItemParent = Globals.Instance.M3DItemManager.EZ3DItemParent;
		Transform tf = ez3dItemParent.Find(name);
		if (null == tf)
			return null;
		
		EZ3DSimipleText text  = tf.GetComponent<EZ3DSimipleText>() as EZ3DSimipleText;
		
		return text;
	}
	
	public void ShowBuildingTips(Building building, string desc)
	{
		string portName = Globals.Instance.MGameDataManager.MCurrentPortData.BasicData.PortName;
		string title = building.U3DGameObject.name + "(" + portName + ")";
		
		string sFormat = Globals.Instance.MDataTableManager.GetWordText(11030058);
		string belongTo = string.Format(sFormat, GUIFontColor.LimeGreen089210000 + Globals.Instance.MGameDataManager.MCurrentPortData.BasicData.CampName);
		
		//Globals.Instance.MGUIManager.ShowAttributeTips(GetBuildingName(building.U3DGameObject.name).BtnText, 
			//title, belongTo + "\n" + GUIFontColor.White255255255 + desc);
	}
	
	public int getCurrentInteractBuildingLogicID()
	{
		return mCurrentInteractBuildingLogicID;
	}
	
	public void MapMoveAnimation()
	{
		nTarMapMoveID = 0;
		
		int nTarBuildID = -1;
		BuildingsExploreConfig config = Globals.Instance.MDataTableManager.GetConfig<BuildingsExploreConfig>();
		BuildingsExploreConfig.BuildingsExploreElement element = null;
		if (config.GetItemElement(mnPlaceID, out element))
		{
			nTarBuildID = element.nBuildingID;
		} 
		
		PortsBuildingConfig cfg = Globals.Instance.MDataTableManager.GetConfig<PortsBuildingConfig>();
		int nTarMapID = -1;
		if(!cfg.GetMapID(nTarBuildID,out nTarMapID))
			return;
		int nSurBuildID = Globals.Instance.MGameDataManager.MActorData.BuildID;
		int nSurMapID = Globals.Instance.MGameDataManager.MCurrentPortData.BasicData.LogicID;
		if(nTarMapID != nSurMapID)
		{
			///如果前往地图和所在地图不一致 播放地图动画//
			if(Globals.Instance.MTaskManager.mbDatingRole)
			{
				 nTarMapMoveID = nTarMapID;
				 long ActorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
				 NetSender.Instance.RequestMapInfo(ActorID);
				Globals.Instance.MNpcManager.mbDatingMapPlay = true;
			}
			 
		}
		else
		{
			if(nSurBuildID != nTarBuildID)
			{
				///如果前往地图和所在地图一致 并且 前往建筑与所在建筑一致 播放建筑移动动画//
				BuildDatingMove();
			}
			else
			{
				///如果前往地图和所在地图一致 并且 前往建筑与所在建筑一致 直接发送任务//
				//if (Globals.Instance.MNpcManager.mbDatingPlay)
				{
					int girlID = Globals.Instance.MTaskManager.mCurDatingGirid;
					int storyID = Globals.Instance.MTaskManager.mCurTaskId;
//					NetSender.Instance.RequestTaskAccept(storyID,girlID,true);
					Globals.Instance.MNpcManager.mbDatingPlay = false;
				}
			}
		}
	}
	
	public void BuildDatingMove()
	{
		int nTarBuildID = -1;
		BuildingsExploreConfig config = Globals.Instance.MDataTableManager.GetConfig<BuildingsExploreConfig>();
		BuildingsExploreConfig.BuildingsExploreElement element = null;
		if (config.GetItemElement(mnPlaceID, out element))
		{
			nTarBuildID = element.nBuildingID;
		} 
		mbDatingPlay = true;
		Building npc = null;
		_mBuildingList.TryGetValue(nTarBuildID, out npc);
		mNpcBuild = npc;
		mNpcPreBuild = mNpcBuild;
		mStrGoWhere= "Camera"+npc.U3DGameObject.name;
		_mBuildingList.TryGetValue(nTarBuildID, out npc);
		
		PlayCameraAnimation();
	
		
	}
	
	public void UpdateBuildMainTaskIcon()
	{
		foreach (Building building in _mBuildingList.Values)
		{
			Transform taskIconTransform = building.U3DGameObject.transform.parent.Find("MainTaskIcomObj");
			if (taskIconTransform != null)
			{
				GameObject.DestroyObject(taskIconTransform.gameObject);
			}
		}
		foreach(TaskManager.BuildTaskInfo buildTaskInfo  in Globals.Instance.MTaskManager.BuildTaskInfoList.Values)
		{
			if (buildTaskInfo.IsMainTask)
			{
				Building npc = null;
				_mBuildingList.TryGetValue(buildTaskInfo.BuildID, out npc);
				if (npc != null)
				{
					GameObject mainTaskIcon =  GameObject.Instantiate(mMainTaskPrefab)as GameObject;
					mainTaskIcon.transform.parent = npc.U3DGameObject.transform.parent;
					mainTaskIcon.transform.localPosition = new Vector3(0,0,0);
					mainTaskIcon.transform.localScale = Vector3.one;
					mainTaskIcon.transform.name = "MainTaskIcomObj";
				}
			}
		}
	}

	public void ResetState()
	{
		mbPlayingAni = false;
		mbDatingPlay = false;
		mbDatingMapPlay = false;
		GameStatusManager.Instance.MPortStatus.MoveCameraDefault(false);
		Globals.Instance.MSceneManager.mTaskCamera.transform.localPosition = new Vector3(0f,1f,-8f);
		Globals.Instance.MSceneManager.mTaskCamera.transform.localEulerAngles = Globals.Instance.MSceneManager.initTaskCameraLocalEulerAngles;
		Globals.Instance.MSceneManager.mTaskCamera.fieldOfView = 15f;
	}

	
	#endregion

	Dictionary<int, Building> _mBuildingList = new Dictionary<int, Building>();
	public GameObject mPrefabGoWhere;
	public GameObject mPrefabGoWhereClone;
	private GameObject mGameObjPreGoWhere;
	private GameObject mGameObjPreGoWhereClone;
	public GameObject mMainTaskPrefab;
	private Animation mMainCameraAnimation = null;
	private string mStrGoWhere;
	private Building mNpcBuild; ///当前交互建筑 /// 
	private Building mNpcPreBuild ;///当前交互建筑///
	private bool     mbSuoPing = false;
	private bool     mbPlayingAni = false;
	private bool     mbPlayingAniReturn = false;
	public  int      mnPlaceID; /// 要去约会的地点ID//
	public  bool     mbDatingPlay = false;/// 标记这是约会动画//
	public  bool     mbDatingMapPlay = false;
	
	///new map 修改//
	public int nTarMapMoveID = 0;
}
