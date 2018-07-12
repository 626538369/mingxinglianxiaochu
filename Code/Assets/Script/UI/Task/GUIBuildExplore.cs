using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GUIBuildExplore : GUIWindowForm
{
	public GameObject taksLableItemPrefab;
	
	public UITexture spriteBuildBackgroundScene;
	public UILabel  ExploreBuildName;
	public UILabel  ExploreBuildProgress;
	public InfoProgressBar ExploreBuildProgressBar;
	public GameObject mBuildExploreObj;
	
	private static readonly Vector3[] LabelItem_Positon = 
	{
		new Vector3(0,-639.3398f,-1f),
		new Vector3(0,-275.4668f,-1f),
		new Vector3(0,60.62988f,-1f),
		new Vector3(0,400.62988f,-1f),
		new Vector3(0,740.62988f,-1f),
	};
	
		//---------------------------------------------------------------
	protected override void Awake()
	{
		if (null == Globals.Instance.MGUIManager)
			return;
		
		base.Awake();
		
		base.enabled = true;
	}
	
	protected virtual void Start ()
	{
		base.Start();
	}
	
	public override void InitializeGUI()
	{
		if (base._mIsLoaded)
			return;
		base._mIsLoaded = true;
		

		this.GUILevel = 20;
		
	}
	
	public void updateBuildingPlaceInfo()
	{
		bool isExploreFinished  = false;
		isExploreFinished = Globals.Instance.MTaskManager.exploreFinished;
		PortsBuildingConfig cfg = Globals.Instance.MDataTableManager.GetConfig<PortsBuildingConfig>();
				
		PortBuildingElement element = null;
		int portID = Globals.Instance.MGameDataManager.MActorData.PortID;
		bool isExist = cfg.GetPortBuildingElement(portID, out element);
		if (!isExist)
			return;
				
		int buildLogicID = Globals.Instance.MNpcManager.getCurrentInteractBuildingLogicID();
		BuildingElement bldElement = element.GetBuildingElement(buildLogicID);
		if (null == bldElement)
		{
			if (Globals.Instance.MTaskManager.buildingExploreBG != "")
			{
				setBuildingBackgroudPic(Globals.Instance.MTaskManager.buildingExploreBG);	
			}
			NGUITools.SetActive(mBuildExploreObj,false);
			return;
		}
			
		NGUITools.SetActive(mBuildExploreObj,true);
		
		if (Globals.Instance.MTaskManager.currentExplorePlaceID != 0)
		{
			string currentBuidlingName = Globals.Instance.MDataTableManager.GetWordText(bldElement._buildingNameID);
			if (!isExploreFinished)
			{
				ExploreBuildProgress.text = Globals.Instance.MTaskManager.exploreCount.ToString() + "/" +
										Globals.Instance.MTaskManager.exploreTotalCount.ToString();
				ExploreBuildName.text = currentBuidlingName + "-" + Globals.Instance.MTaskManager.currentTaskExploreName;
				
				ExploreBuildProgressBar.SetMaxValue((long)Globals.Instance.MTaskManager.exploreTotalCount);
				ExploreBuildProgressBar.SetValue((long)Globals.Instance.MTaskManager.exploreCount,(long)Globals.Instance.MTaskManager.exploreCount,0.1f);
			}
			else
			{
							
				ExploreBuildProgressBar.SetMaxValue((long)1.0);
				ExploreBuildProgressBar.SetValue((long)1.0,(long)1.0,0.1f);
				
				ExploreBuildName.text = currentBuidlingName;
				ExploreBuildProgress.text = Globals.Instance.MDataTableManager.GetWordText(11014);
			}

		}
		
		
		if (Globals.Instance.MTaskManager.buildingExploreBG == "")
		{
			setBuildingBackgroudPic(bldElement._buildBG);	
		}
		else
			setBuildingBackgroudPic(Globals.Instance.MTaskManager.buildingExploreBG);

	}
	
	
	private  void setBuildingBackgroudPic(string backgrdName)
	{
		UIWidget uiWidget = spriteBuildBackgroundScene.GetComponentInChildren<UIWidget>();
		UIPanel uiPanel = spriteBuildBackgroundScene.GetComponent<UIPanel>();
		uiWidget.alpha = 1f;
		uiPanel.alpha = 1f;
		
		string atlasPath = "UIAtlas/" + backgrdName;
		spriteBuildBackgroundScene.mainTexture = Resources.Load(atlasPath,typeof(Texture2D)) as Texture2D;
		
		Globals.Instance.MSoundManager.PlaySceneSound(backgrdName);
		
	}
	
	public void showBuildingLables(List<sg.GS2C_Buildings_Res.Buildings_Mes> buildingLablesList)
	{		
		int funcNum = 0;
		GameObject lableItemObj_null = GameObject.Instantiate(taksLableItemPrefab) as GameObject;
		lableItemObj_null.name = "lableItemObj" + funcNum.ToString();
		lableItemObj_null.transform.parent = this.gameObject.transform;
		lableItemObj_null.transform.localPosition = LabelItem_Positon[funcNum];
		funcNum ++;
		lableItemObj_null.transform.localScale = Vector3.one;
		
		UIImageButton lableItemBtn_null =  lableItemObj_null.transform.Find("ButtonRenWuLable").GetComponent<UIImageButton>();
		UIEventListener.Get(lableItemBtn_null.gameObject).onClick += PressedBuildingNullItemButton;
								
		UILabel lableItemContent_null = lableItemObj_null.transform.Find("UILableContent").GetComponent<UILabel>();
		lableItemContent_null.text = Globals.Instance.MDataTableManager.GetWordText(1000);
		UISprite daodaUISprite = lableItemObj_null.transform.Find("SpriteDadao").GetComponent<UISprite>();
		
		UISprite daodaUISprite1 = lableItemObj_null.transform.Find("SpriteDadao1").GetComponent<UISprite>();
		UISprite needAttributeSprite1 = lableItemObj_null.transform.Find("SpriteNeed").GetComponent<UISprite>();
		UILabel needAttributeLabel = lableItemObj_null.transform.Find("UILableNeedCount").GetComponent<UILabel>();
		
		daodaUISprite1.transform.localScale = Vector3.zero;
		needAttributeSprite1.transform.localScale = Vector3.zero;
		needAttributeLabel.transform.localScale = Vector3.zero;
		daodaUISprite.transform.localScale = Vector3.zero;
		
		for (int i=0; i<buildingLablesList.Count; i++,funcNum++)
		{
			GameObject lableItemObj = GameObject.Instantiate(taksLableItemPrefab) as GameObject;
			lableItemObj.name = "lableItemObj" + funcNum.ToString();
			lableItemObj.transform.parent = this.gameObject.transform;
			lableItemObj.transform.localPosition = LabelItem_Positon[funcNum];
			lableItemObj.transform.localScale = Vector3.one;
			
			UIImageButton lableItemBtn =  lableItemObj.transform.Find("ButtonRenWuLable").GetComponent<UIImageButton>();
			UIEventListener.Get(lableItemBtn.gameObject).onClick += PressedBuildingLableItemButton;
			lableItemBtn.Data = buildingLablesList[i];
							
			UILabel lableItemContent = lableItemObj.transform.Find("UILableContent").GetComponent<UILabel>();
			lableItemContent.text = buildingLablesList[i].name;
			
			daodaUISprite = lableItemObj.transform.Find("SpriteDadao").GetComponent<UISprite>();
			daodaUISprite.transform.localScale = Vector3.zero;
			daodaUISprite1 = lableItemObj.transform.Find("SpriteDadao1").GetComponent<UISprite>();
			daodaUISprite1.transform.localScale = Vector3.zero;
			
			UISprite dneedAttributeSprite1 = lableItemObj.transform.Find("SpriteNeed").GetComponent<UISprite>();
			UILabel dneedAttributeLabel = lableItemObj.transform.Find("UILableNeedCount").GetComponent<UILabel>();
			dneedAttributeSprite1.transform.localScale = Vector3.zero;
			dneedAttributeLabel.transform.localScale = Vector3.zero;
			
		}
			
		Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIBuildExplore");
	}
	
	private void PressedBuildingLableItemButton(GameObject obj)
	{
		UIImageButton btn = obj.transform.GetComponent<UIImageButton>();
		sg.GS2C_Buildings_Res.Buildings_Mes userData = (sg.GS2C_Buildings_Res.Buildings_Mes)btn.Data;
		switch (userData.id )
		{
		///学习//
		case 1:
		case 2:
			Globals.Instance.mShopDataManager.InStudy = userData.param;

			break;	
		//打工//
		case 3:
			int buildingID = Globals.Instance.MNpcManager.getCurrentInteractBuildingLogicID();
			GUIRadarScan.Show();
			NetSender.Instance.C2GSRequestIndustryList(buildingID);
			break;
		//商店//
		case 4:
		case 5:
//			Globals.Instance.mShopDataManager.ShopPushGoodsID = Globals.Instance.mShopDataManager.SHOP;
//			Globals.Instance.mShopDataManager.ShopPushGoodsVL = userData.param;
//			NetSender.Instance.RequestShopPushGoods(5021);
			
			NetSender.Instance.C2GSRequestShopItems(userData.param,-1);
			bool isPushData = Globals.Instance.MPushDataManager.GetItemData(Push_Tpye.SHOP_PUSH,userData.param) == null ? false:true;
			if (isPushData)
			{
				NetSender.Instance.RequestReadPushData((int)Push_Tpye.SHOP_PUSH,userData.param);
			}
			
			break;
			///进入宿舍///
		case 6:
			Globals.Instance.MLSNetManager.RequestRoomInfo();
			break;
			//打开世界地图//
		case 8:
		{
		    long ActorID = Globals.Instance.MGameDataManager.MActorData.PlayerID;
			NetSender.Instance.RequestMapInfo(ActorID);
			GUIRadarScan.Show();
			this.Close();
			Globals.Instance.MNpcManager.PlayeCameraAnimatonReturn();
			Globals.Instance.MNpcManager.SetPlayerFace(true);
		}
			break;
		}
			
	}
	
	
	private void PressedBuildingNullItemButton(GameObject obj)
	{
		this.Close();

		Globals.Instance.MNpcManager.PlayeCameraAnimatonReturn();
		Globals.Instance.MNpcManager.SetPlayerFace(true);
		this.SetVisible(false);
		Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIMain");
	}
	
	protected override void OnClose(GameObject obj)
	{
		GameObject backgroundChangeCloth = GameObject.Find("BuildBackground");
		if(null != backgroundChangeCloth)
		{
			GameObject.Destroy(backgroundChangeCloth);
		}
		
		GameObject preGround = GameObject.Find("BuildPreground");
		if(null != preGround)
		{
			GameObject.Destroy(preGround);
		}
		Globals.Instance.MTaskManager.buildingExploreBG = "";
		
		string sceneName = Globals.Instance.MSceneManager.GetSceneName();
		Globals.Instance.MSoundManager.PlaySceneSound(sceneName);
	}
	
	
	
}
