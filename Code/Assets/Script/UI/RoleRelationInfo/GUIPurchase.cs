using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIPurchase : GUIWindow {
	
	public NewWealthGroup newWealthGroup;
	public UIButton BackHomeBtn;
	public GameObject ExchangeBtn;
	public GameObject BuyBtn;
	public GameObject WatchVideo;
	public UIButton WacthBtn;
	
	public UIScrollView PurshaseUIScrollView;
	public UIGrid PurshaseUIGrid;
	public GameObject PurchaseMoneyItem;
	public GameObject PurchaseDiamondItem;

	public GameObject BuyDiamondLabel;
	public GameObject BuyMoneyLabel;
	private PlayerData playerData;
	
	private DiamondToGoldConfig.DiamondToGoldElement diamondToGoldelement;
	private CommodityData moneyToDiamondCommodityData;

	private List<CommodityData> commodityDataList ;

	private int mCurrentShowView = 0;

	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();		
		base.enabled = true;		


		playerData = Globals.Instance.MGameDataManager.MActorData;

		UIEventListener.Get(BackHomeBtn.gameObject).onClick += delegate(GameObject go) {

			this.Close();
		};

		UIEventListener.Get(WacthBtn.gameObject).onClick += OnClickWacthBtn;
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
		
		this.GUILevel = 2;

		commodityDataList = ShopDataManager.GetCommodityList(CommodityType.Recharge);

//		if(!Globals.Instance.MGameDataManager.MActorData.starData.appStoreWatchVideoState)
//		{
			WatchVideo.transform.localScale = Vector3.zero;
//		}
	}
	 

	public void ShowPurshaseMoneyInfor()
	{
		mCurrentShowView = 1;
		NGUITools.SetActive(BuyBtn , true);
//		NGUITools.SetActive(ExchangeBtn , false);
		NGUITools.SetActive(WatchVideo , true);
		BuyDiamondLabel.transform.localScale = Vector3.zero;
		BuyMoneyLabel.transform.localScale = Vector3.one;
		List<CommodityData> list = new List<CommodityData>();
		if(commodityDataList != null)
		{
			foreach(CommodityData data in commodityDataList)
			{
				if(data!=null && data.currency == CurrencyType.RmbYuan)
				{
					list.Add(data);
				}
			}
		}

		if(list.Count <= 0)
		{
			return;
		}
		DiamondToGoldConfig config = Globals.Instance.MDataTableManager.GetConfig<DiamondToGoldConfig>();
		Dictionary<int, DiamondToGoldConfig.DiamondToGoldElement> moneytoDiamond = config.GetDiamondToGoldElementList();
		HelpUtil.DelListInfo(PurshaseUIGrid.transform);
		for(int i = 0; i < (list.Count+moneytoDiamond.Count) ; i++)
		{
			if(i % 2 == 0)
			{
				GameObject purchaseItem = GameObject.Instantiate(PurchaseDiamondItem)as GameObject;
				purchaseItem.transform.parent = PurshaseUIGrid.transform;
				purchaseItem.transform.localScale = Vector3.one;

				purchaseItem.transform.localPosition = new Vector3(-260f,334f-(i/2)*360f,0);
				CommodityData commodityItem = list[i/2];

				UISprite iconSprite = purchaseItem.transform.Find("IconSprite").GetComponent<UISprite>();
				UILabel diamondLabel = purchaseItem.transform.Find("DiamondLabel").GetComponent<UILabel>();
				UILabel rmbLabel = purchaseItem.transform.Find("RmbLabel").GetComponent<UILabel>();

				UILabel rechargeLabel = iconSprite.transform.Find("RechargeLabel").GetComponent<UILabel>();

				if(commodityItem.BasicData.IsFirstDouble)
				{
					NGUITools.SetActive(rechargeLabel.gameObject , true);
				}
				else
				{
					NGUITools.SetActive(rechargeLabel.gameObject , false);
				}
				UISprite diamondIcon = diamondLabel.transform.Find("Sprite").GetComponent<UISprite>();
				diamondIcon.spriteName = "IconJinqian";

				iconSprite.spriteName = commodityItem.BasicData.Icon;
				
				diamondLabel.text = commodityItem.recvIgnotCnt.ToString();

				String currencyText = commodityItem.originalPrice.ToString("N2");
				Debug.Log("commodityItem.CommodityStr:" + commodityItem.CommodityStr);
				if (ShopDataManager.CommodityToCurrencyDicts.ContainsKey(commodityItem.CommodityStr))
				{
					ShopDataManager.CommodityToCurrencyDicts.TryGetValue(commodityItem.CommodityStr,out currencyText);
				}

				rmbLabel.text = currencyText;

				UIToggle btn = purchaseItem.transform.GetComponent<UIToggle>();
				btn.Data = commodityItem;
				
				UIEventListener.Get(btn.gameObject).onClick += OnClickDiamondBtn;
			}
			else
			{
				GameObject purchaseItem = GameObject.Instantiate(PurchaseMoneyItem)as GameObject;
				purchaseItem.transform.parent = PurshaseUIGrid.transform;
				purchaseItem.transform.localScale = Vector3.one;

				purchaseItem.transform.localPosition = new Vector3(260f,334f-(i/2)*360f,0);

				DiamondToGoldConfig.DiamondToGoldElement element = moneytoDiamond[/*i-((i+1)/2)*/  i ]; 

				UISprite iconSprite = purchaseItem.transform.Find("IconSprite").GetComponent<UISprite>();
				UILabel moneyLabel = purchaseItem.transform.Find("MoneyLabel").GetComponent<UILabel>();
				UILabel diamondLabel = purchaseItem.transform.Find("DiamondLabel").GetComponent<UILabel>();
				iconSprite.spriteName = element.Icon;
				moneyLabel.text = element.Get_Gold.ToString();
				diamondLabel.text = element.Need_Diamond.ToString();
				UIToggle btn = purchaseItem.transform.GetComponent<UIToggle>();
				btn.Data = element;
				UIEventListener.Get(btn.gameObject).onClick += OnClickMoneyBtn;
			}
		}
	}

	public void ShowPurshaseDiamondInfor()
	{
		mCurrentShowView = 2;
		NGUITools.SetActive(BuyBtn , true);
//		NGUITools.SetActive(ExchangeBtn , false);
		NGUITools.SetActive(WatchVideo , false);
		BuyDiamondLabel.transform.localScale = Vector3.one;
		BuyMoneyLabel.transform.localScale = Vector3.zero;
		HelpUtil.DelListInfo(PurshaseUIGrid.transform);

		List<CommodityData> list = new List<CommodityData>();
		if(commodityDataList != null)
		{
			foreach(CommodityData data in commodityDataList)
			{
				if(data!=null && data.currency == CurrencyType.RmbJiao)
				{
					list.Add(data);
				}
			}
		}
		
		if(list.Count <= 0)
		{
			return;
		}

		for (int i=0; i<list.Count; i++)
		{
			CommodityData commodityItem = list[i];
			GameObject purchaseItem = GameObject.Instantiate(PurchaseDiamondItem)as GameObject;
			purchaseItem.transform.parent = PurshaseUIGrid.transform;
			purchaseItem.transform.localScale = Vector3.one;
			if(i % 2 == 0)
			{
				purchaseItem.transform.localPosition = new Vector3(-260f,334f-(i/2)*360f,0);
			}
			else
			{
				purchaseItem.transform.localPosition = new Vector3(260f,334f-(i/2)*360f,0);
			}

			UISprite iconSprite = purchaseItem.transform.Find("IconSprite").GetComponent<UISprite>();
			UILabel diamondLabel = purchaseItem.transform.Find("DiamondLabel").GetComponent<UILabel>();
			UILabel rmbLabel = purchaseItem.transform.Find("RmbLabel").GetComponent<UILabel>();
			UILabel rechargeLabel = iconSprite.transform.Find("RechargeLabel").GetComponent<UILabel>();
			if(commodityItem.BasicData.IsFirstDouble)
			{
				NGUITools.SetActive(rechargeLabel.gameObject , true);
			}
			else
			{
				NGUITools.SetActive(rechargeLabel.gameObject , false);
			}
			iconSprite.spriteName = commodityItem.BasicData.Icon;

			diamondLabel.text = commodityItem.recvIgnotCnt.ToString();

			String currencyText = commodityItem.originalPrice.ToString("N2");
			if (ShopDataManager.CommodityToCurrencyDicts.ContainsKey(commodityItem.CommodityStr))
			{
				ShopDataManager.CommodityToCurrencyDicts.TryGetValue(commodityItem.CommodityStr,out currencyText);
			}

			rmbLabel.text = currencyText;
			
			UIToggle btn = purchaseItem.transform.GetComponent<UIToggle>();
			btn.Data = commodityItem;
			
			UIEventListener.Get(btn.gameObject).onClick += OnClickDiamondBtn;
		}
	}

	private void OnClickMoneyBtn(GameObject obj)
	{
		UIToggle btn = obj.transform.GetComponent<UIToggle>();
		diamondToGoldelement = (DiamondToGoldConfig.DiamondToGoldElement)btn.Data;
	
//		NGUITools.SetActive(BuyBtn , false);
//		NGUITools.SetActive(ExchangeBtn , true);

		OnClickExchangeBtn();
	}

	private void OnClickDiamondBtn(GameObject obj)
	{
		UIToggle btn = obj.transform.GetComponent<UIToggle>();
		moneyToDiamondCommodityData = (CommodityData)btn.Data;

//		NGUITools.SetActive(BuyBtn , true);
//		NGUITools.SetActive(ExchangeBtn , false);

		OnClickBuyBtn();
	}

	private void OnClickWacthBtn(GameObject obj)
	{
		NetSender.Instance.WatchVideoStateReq();
	}

	public void getWatchVideoState(int remainingTime)
	{
		if(remainingTime <= 0)
		{
			SoundManager.CurrentPlayingMusicAudio.Pause();
			
			//U3dAppStoreSender.AppStoreTapJoyEvent("offerwall_unit");
		}
		else
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips( string.Format( Globals.Instance.MDataTableManager.GetWordText(4024),(int)(remainingTime/60) + 1));
		}
	}

	private void OnClickBuyBtn()
	{
		if(moneyToDiamondCommodityData == null)
		{
			return;
		}
		
		long itemID = moneyToDiamondCommodityData.ItemID;
		ShopDataManager.PayCommodityData.ItemID = itemID;
		ShopDataManager.PayCommodityData.CommodityStr = moneyToDiamondCommodityData.CommodityStr;
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore){
			NetSender.Instance.RequestPayAddOrder(moneyToDiamondCommodityData, 3, "appstore");
		}else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay){
			NetSender.Instance.RequestPayAddOrder(moneyToDiamondCommodityData, 12,GameDefines.OutputVerDefs.ToString());
		}
	}
	private void OnClickExchangeBtn()
	{
		
		if(diamondToGoldelement == null)
		{
			return;
		}
		
		Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui){
			
			gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
			gui.SetDialogType(EDialogType.CommonType,null);
			string flag = string.Format( Globals.Instance.MDataTableManager.GetWordText(4001) , diamondToGoldelement.Need_Diamond,diamondToGoldelement.Get_Gold);
			gui.SetText(flag);
			
		},EDialogStyle.DialogOkCancel,delegate() {
			
			if(diamondToGoldelement.Need_Diamond > playerData.WealthData.GoldIngot)
			{
				Globals.Instance.MGUIManager.ShowErrorTips(20007);
				return ;
			}
			NetSender.Instance.C2GSDiamondToGoldReq(diamondToGoldelement.ID);
		});
	}


	public void RefreshSurface()
	{
		if(mCurrentShowView == 1)
		{
			ShowPurshaseMoneyInfor();
		}
		else if(mCurrentShowView == 2)
		{
			ShowPurshaseDiamondInfor();
		}
	}

	void OnDestroy()
	{
		base.OnDestroy();	
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
}


