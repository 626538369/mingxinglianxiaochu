using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipShopPanel : RoleBasePanel 
{
	public GameObject ItemInfoTp;
	public GameObject VirtualItemInfoTp;
	public GameObject VipDetailTp;
	
	public UIToggle[] shopTabBtns;
	public Transform[] shopTabRoots;
	public UIGrid[] shopTabSLists;
	
	public UIButton buyGenericBtn;
	public UIButton buyIAPBtn;
	public UIButton buyDiamonBtn;
	public UIButton fast2RechargeBtn;
	
	
	[HideInInspector] public static readonly int MaxVipLevel = 10;
	[HideInInspector] public static readonly int WordCfgBaseId = 24200000;
	public enum TabPageIndex
	{
		GenericTab,
		IAPTab,
		RechargeTab,
		VipDetailTab,
		
		Count,
	}
	
	protected override void Awake()
	{
		base.Awake();
		
	//for (TabPageIndex i = 0; i < TabPageIndex.Count; i++)
	//{
    // UIEventListener.Get(	shopTabBtns[(int)i].gameObject).onClick += OnClickRadioBtn;
	//	shopTabBtns[(int)i].Data = i;
	//}
	//
    // UIEventListener.Get(		buyGenericBtn.gameObject).onClick += OnClickBuyItemBtn;
    // UIEventListener.Get(		buyIAPBtn.gameObject).onClick += OnClickBuyDiamonBtn;
    // UIEventListener.Get(		buyDiamonBtn.gameObject).onClick += OnClickBuyDiamonBtn;
	//
    //IEventListener.Get(fast2RechargeBtn.gameObject).onClick += (delegate(GameObject obj) {
	//	shopTabBtns[(int)TabPageIndex.RechargeTab].Value = true;
	//});
	//
	//RegisterSubscribers();
	}
	
	void OnDestroy()
	{
		//UnregisterSubscribers();
	}
	
	//void RegisterSubscribers()
	//{
	//	reqShopInfoSub = EventManager.Subscribe(NetReceiverPublisher.NAME + ":" + NetReceiverPublisher.EVENT_REQ_SHOP_INFO);
	//	reqShopInfoSub.Handler = delegate (object[] args)
	//	{
	//		CreateGenericItemList();
	//		GUIRadarScan.Hide();
	//	}; // End reqShopInfoSub.Handler
	//	
	//	reqIAPInfoSub = EventManager.Subscribe(NetReceiverPublisher.NAME + ":" + NetReceiverPublisher.EVENT_REQ_IAP_INFO);
	//	reqIAPInfoSub.Handler = delegate (object[] args)
	//	{
	//		CreateCommodityList(TabPageIndex.IAPTab);
	//		GUIRadarScan.Hide();
	//	}; // End reqIAPInfoSub.Handler
	//	
	//	reqRechargeInfoSub = EventManager.Subscribe(NetReceiverPublisher.NAME + ":" + NetReceiverPublisher.EVENT_REQ_RECHARGE_INFO);
	//	reqRechargeInfoSub.Handler = delegate (object[] args)
	//	{
	//		CreateCommodityList(TabPageIndex.RechargeTab);
	//		GUIRadarScan.Hide();
	//	}; // End reqRechargeInfoSub.Handler
	//	
	//	buyCommoditySub = EventManager.Subscribe(NetReceiverPublisher.NAME + ":" + NetReceiverPublisher.EVENT_BUY_COMMODITY);
	//	buyCommoditySub.Handler = delegate (object[] args)
	//	{
	//		int error = (int)args[0];
	//		if (error == (int)PacketErrorCode.SUCCESS)
	//		{
	//		}
	//	}; // End buyCommoditySub.Handler
	//	
	//	vipChangeSub  = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_VIP_UPDATE);
	//	vipChangeSub.Handler = delegate (object[] args)
	//	{
	//		UpdateRoleVipInfo();
	//	}; // End vipChangeSub.Handler
	//}
	//
	//void UnregisterSubscribers()
	//{
	//	if (null != reqShopInfoSub)
	//	{
	//		reqShopInfoSub.Unsubscribe();
	//	}
	//	reqShopInfoSub = null;
	//	
	//	if (null != reqIAPInfoSub)
	//	{
	//		reqIAPInfoSub.Unsubscribe();
	//	}
	//	reqIAPInfoSub = null;
	//	
	//	if (null != reqRechargeInfoSub)
	//	{
	//		reqRechargeInfoSub.Unsubscribe();
	//	}
	//	reqRechargeInfoSub = null;
	//	
	//	if (null != buyCommoditySub)
	//	{
	//		buyCommoditySub.Unsubscribe();
	//	}
	//	buyCommoditySub = null;
	//	
	//	if (null != vipChangeSub)
	//		vipChangeSub.Unsubscribe();
	//	vipChangeSub = null;
	//}
	
	public override void UpdateGUI()
	{
		//if (IsNeedCreate)
		//{
		//	IsNeedCreate = false;
		//	shopTabBtns[(int)TabPageIndex.GenericTab].Value = true;
		//}
		//else
		//{
		//}
	}
	
	public override void HideGUI()
	{
		//for (TabPageIndex i = 0; i < TabPageIndex.Count; i++)
		//{
		//	shopTabBtns[(int)i].Value = false;
		//}
		//
		//shopTabSLists[(int)TabPageIndex.GenericTab].ClearList(true);
		//shopTabSLists[(int)TabPageIndex.IAPTab].ClearList(true);
		//shopTabSLists[(int)TabPageIndex.RechargeTab].ClearList(true);
	}
	
	public override void OnClickNonFunctionalArea(GameObject obj) 
	{
	}
	
	public void SwitchTo(TabPageIndex index)
	{
		shopTabBtns[(int)index].isChecked = true;
	}
	
//void OnClickRadioBtn(GameObject obj)
//{
//	UIToggle btn = obj as UIToggle;
//	if (!btn.Value)
//		return;
//	
//	ClearSelGenericItem();
//	ClearSelDiamondItem();
//	
//	TabPageIndex index = (TabPageIndex)obj.Data;
//	ShowTabPage((int)index);
//	
//	switch (index)
//	{
//	case TabPageIndex.GenericTab:
//		if (0 == shopTabSLists[(int)index].Count)
//		{
//			GUIRadarScan.Show();
//			
//			int seaID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
//			NetSender.Instance.RequestShopInfo(seaID, 0);
//		}
//		break;
//	case TabPageIndex.IAPTab:
//		if (0 == shopTabSLists[(int)index].Count)
//		{
//			GUIRadarScan.Show();
//			NetSender.Instance.RequestVipStoreGameItems();
//		}
//		break;
//	case TabPageIndex.RechargeTab:
//		if (0 == shopTabSLists[(int)index].Count)
//		{
//			// GUIRadarScan.Show();
//			NetSender.Instance.RequestVipStoreRechargeInfo();
//		}
//		break;
//	case TabPageIndex.VipDetailTab:
//		CreateVipDetailList();
//		break;
//	}
//}
//
//void ShowTabPage(int index)
//{
//	currIndex = index;
//	
//	for (int i = 0; i < shopTabBtns.Length; i++)
//	{
//		if (i == index)
//		{
//			shopTabRoots[i].localPosition = 
//				new Vector3(shopTabRoots[i].localPosition.x, 0.0f, shopTabRoots[i].localPosition.z);
//		}
//		else
//		{
//			shopTabRoots[i].localPosition =
//				new Vector3(shopTabRoots[i].localPosition.x, 10000.0f, shopTabRoots[i].localPosition.z);
//		}
//	}
//}
//
//void OnClickBuyItemBtn(GameObject obj)
//{
//	Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");
//	
//	// Request buy something
//	if (null == selectGenericItem)
//		return;
//	
//	int needMoney = selectGenericItem.ItemData.MItemData.BasicData.BuyPrice * selectGenericItem.ItemData.MItemData.BasicData.Count;
//	if (!Globals.Instance.MGUIManager.HandleNotEnoughMoney(needMoney))
//	{
//		int seadID = Globals.Instance.MGameDataManager.MCurrentSeaAreaData.SeaAreaID;
//		NetSender.Instance.RequestBuyItem(seadID, 
//			selectGenericItem.ItemData.MItemData.BasicData.LogicID, selectGenericItem.ItemData.MItemData.BasicData.Count);
//	}
//}
//
//
//
//void OnClickBuyDiamonBtn(GameObject obj)
//{
//	Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/Button");
//	
//	// Request buy
//	if (null == selectCommodity)
//		return;
//	
//	if (selectCommodity.ItemData.comType == CommodityType.GameInner)
//	{
//		// Internal business
//		NetSender.Instance.RequestVipStoreBuyGameItem(selectCommodity.ItemData);
//	}
//	else if (selectCommodity.ItemData.comType == CommodityType.Recharge)
//	{
//		if (ThirdPartyPlatform.IsGuestLogined())
//		{
//			Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
//			{
//				gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
//				gui.SetDialogType(EDialogType.GUEST_PAY);
//			}, EDialogStyle.DialogOkGoto);
//			return;
//		}
//		
//		// 1 - gfan         2 - 91sdk
//		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android
//			|| GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
//		{
//			NetSender.Instance.RequestPayAddOrder(selectCommodity.ItemData, 2, "91");
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid
//			|| GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone)
//		{
//			NetSender.Instance.RequestPayAddOrder(selectCommodity.ItemData, 1, "Gfan");
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
//		{
//			NetSender.Instance.RequestPayAddOrder(selectCommodity.ItemData, 3, "appstore");
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
//		{
//			//NetSender.Instance.RequestPayAddOrder(selectCommodity.ItemData, 1, "Gfan");
//			NetSender.Instance.RequestPayAddOrder(selectCommodity.ItemData, 3, "appstore");
//		}
//		GUIVipStore.PayCommodityData = selectCommodity.ItemData;
//		GUIRadarScan.Show();
//	}
//}
//
//void OnClickGenericItem(GameObject obj)
//{
//	if (null != selectGenericItem)
//		selectGenericItem.SetChecked(false);
//		
//	selectGenericItem = obj.transform.GetComponent<ItemInfoSlot>() as ItemInfoSlot;
//	selectGenericItem.SetChecked(true);
//}
//
//void OnClickDiamondItem(GameObject obj)
//{
//	if (null != selectCommodity)
//		selectCommodity.SetChecked(false);
//		
//	selectCommodity = obj.transform.GetComponent<CommoditySlot>() as CommoditySlot;
//	selectCommodity.SetChecked(true);
//}
//
//void ClearSelGenericItem()
//{
//	if (null != selectGenericItem)
//		selectGenericItem.SetChecked(false);
//	selectGenericItem = null;
//}
//
//void ClearSelDiamondItem()
//{
//	if (null != selectCommodity)
//		selectCommodity.SetChecked(false);
//	selectCommodity = null;
//}
//
//void CreateGenericItemList()
//{
//	List<ItemSlotData> list = ItemDataManager.Instance.GetItemDataList(ItemSlotType.SHOP);
//	
//	if (null == list)
//	{
//		shopTabSLists[(int)TabPageIndex.GenericTab].ClearList(true);
//		return;
//	}
//	
//	if (shopTabSLists[(int)TabPageIndex.GenericTab].Count != 0)
//	{
//		//shopTabSLists[(int)TabPageIndex.GenericTab].ScrollToItem(0, 0);
//		return;
//	}
//	
//	for (int i = 0; i < list.Count; i++)
//	{
//		GameObject contObj = new GameObject("container" + i.ToString());
//		UIDragPanelContents container = contObj.AddComponent<UIDragPanelContents>() as UIDragPanelContents;
//		
//		GameObject go = Instantiate(ItemInfoTp, Vector3.zero, Quaternion.identity) as GameObject;
//		container.MakeChild(go);
//		go.transform.localPosition = new Vector3(0.0f, 0.0f, -1.0f);
//		
//		ItemInfoSlot slot = go.GetComponent<ItemInfoSlot>() as ItemInfoSlot;
//		slot.UpdateSlot(list[i]);
//       UIEventListener.Get(	slot.eventBtn.gameObject).onClick += OnClickGenericItem;
//		
//		//shopTabSLists[(int)TabPageIndex.GenericTab].AddItem(contObj);
//	}
//	
//	//shopTabSLists[(int)TabPageIndex.GenericTab].ScrollToItem(0, 0);
//}
//
//void CreateCommodityList(TabPageIndex index)
//{
//	List<CommodityData> list = null;
//	
//	if (index == TabPageIndex.IAPTab)
//		list = GUIVipStore.GetCommodityList(CommodityType.GameInner);
//	else if (index == TabPageIndex.RechargeTab)
//		list = GUIVipStore.GetCommodityList(CommodityType.Recharge);
//	
//	if (null == list)
//	{
//		shopTabSLists[(int)index].ClearList(true);
//		return;
//	}
//	
//	if (shopTabSLists[(int)index].Count != 0)
//	{
//		//shopTabSLists[(int)index].ScrollToItem(0, 0);
//		return;
//	}
//	
//	for (int i = 0; i < list.Count; i++)
//	{
//		GameObject contObj = new GameObject("container" + i.ToString());
//		UIDragPanelContents container = contObj.AddComponent<UIDragPanelContents>() as UIDragPanelContents;
//		
//		GameObject go = Instantiate(VirtualItemInfoTp, Vector3.zero, Quaternion.identity) as GameObject;
//		container.MakeChild(go);
//		go.transform.localPosition = new Vector3(0.0f, 0.0f, -1.0f);
//		
//		CommoditySlot slot = go.GetComponent<CommoditySlot>() as CommoditySlot;
//		slot.UpdateGUI(list[i]);
//     UIEventListener.Get(	slot.itemCheckedBtn.gameObject).onClick += OnClickDiamondItem;
//		
//		//shopTabSLists[(int)index].AddItem(contObj);
//	}
//	
//	//shopTabSLists[(int)index].ScrollToItem(0, 0);
//}
//
//void UpdateRoleVipInfo()
//{
//	PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
//	
//	UIGrid slist = shopTabSLists[(int)TabPageIndex.VipDetailTab];
//	for (int i = 0; i < slist.Count; i++)
//	{
//		IUIListObject obj = slist.GetItem(i);
//		Transform tf = obj.transform.GetChild(0);
//		
//		SpriteText lvlupBalance = tf.FindChild("LevelUpBalance").GetComponent<SpriteText>() as SpriteText;
//		if (actorData.VipData.Level == i)
//		{
//			lvlupBalance.transform.localScale = Vector3.one;
//			
//			string textFormat = "还需充值{0}";
//			lvlupBalance.Text = string.Format(textFormat, 
//				GUIFontColor.NewColor9H + (actorData.VipData.NextLevelRechargeVal - actorData.VipData.RechargeVal).ToString());
//		}
//		else
//		{
//			lvlupBalance.transform.localScale = Vector3.zero;
//		}
//	}
//}
//
//void CreateVipDetailList()
//{
//	if (shopTabSLists[(int)TabPageIndex.VipDetailTab].Count != 0)
//	{
//		return;
//	}
//	
//	shopTabSLists[(int)TabPageIndex.VipDetailTab].ClearList(true);
//	for (int i = 1; i <= MaxVipLevel; i++)
//	{
//		GameObject contObj = new GameObject("container");
//		UIDragPanelContents container = contObj.AddComponent<UIDragPanelContents>() as UIDragPanelContents;
//		
//		GameObject go = CreateVipDetailInfo(i);
//		container.MakeChild(go);
//		go.transform.localPosition = new Vector3(0.0f, 0.0f, -1.0f);
//		
//		//shopTabSLists[(int)TabPageIndex.VipDetailTab].AddItem(container);
//	}
//	
//	//shopTabSLists[(int)TabPageIndex.VipDetailTab].ScrollToItem(0, 0);
//}
//
//GameObject CreateVipDetailInfo(int level)
//{
//	GameObject go = Instantiate(VipDetailTp, Vector3.zero, Quaternion.identity) as GameObject;
//	
//	SpriteText vipLevel = go.transform.FindChild("VipLevel").GetComponent<SpriteText>() as SpriteText;
//	SpriteText detailInfo = go.transform.FindChild("DetailText").GetComponent<SpriteText>() as SpriteText;
//	
//	vipLevel.Text = "Vip" + level.ToString();
//	
//	// Build content
//	int wordId = WordCfgBaseId + level;
//	string rawString = Globals.Instance.MDataTableManager.GetWordText(wordId);
//	detailInfo.Text = rawString;
//	
//	SpriteText lvlupBalance = go.transform.FindChild("LevelUpBalance").GetComponent<SpriteText>() as SpriteText;
//	lvlupBalance.transform.localScale = Vector3.zero;
//	
//	return go;
//}
	
	int currIndex = 0;
	ItemInfoSlot selectGenericItem = null;
	CommoditySlot selectCommodity = null;
	
	ISubscriber reqShopInfoSub = null;
	ISubscriber reqIAPInfoSub = null;
	ISubscriber reqRechargeInfoSub = null;
	ISubscriber buyCommoditySub = null;
	ISubscriber vipChangeSub = null;
}
