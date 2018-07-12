using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PackageUnit : MonoBehaviour {
	
	/// <summary>
	/// Item icon tap delegate.
	/// </summary>
	public delegate void ItemIconTapDelegate(PackageUnit.IconData iData);
	
	/// <summary>
	/// Operation button delegate.
	/// </summary>
	public delegate void OperationBtnDelegate(PackageUnit.IconData iData);
	
	/// <summary>
	/// Sell button delegate.
	/// </summary>
	public delegate void SellBtnDelegate(PackageUnit.IconData iData);
	
	/// <summary>
	/// Package item data update when server send packet to change data
	/// </summary>
	public delegate void PackageItemDataUpdate(PackageUnit unit);
		
	//! item icon template
	public GameObject	ItemIcon;
	
	//! page container
	public GameObject	ListContainer;
	
	//! page indicator
	public GameObject	PageIndicator;
	
	//! item tips text
	public SpriteText	ItemTipsName;
	
	//! item tips for money
	public SpriteText	ItemTipsMoney;
		
	//! item tips text for description
	public SpriteText	ItemTipsDesc;
	
	//! hide the operation button always
	public bool			HideOperationBtnAlways	= false;
	
	/// <summary>
	/// The hide left operation button.
	/// </summary>
	public bool			HideLeftOperationBtn	= false;
	
	/// <summary>
	/// The hide right operation button.
	/// </summary>
	public bool			HideRightOperationBtn	= false;
	
	/// <summary>
	/// is shop sell item package or normail personal package
	/// </summary>
	public bool	ShopSellItemPackage				= false;
	
	//! page indicator parent for adding pageIndicator
	public Transform	PageIndicatorParent		= null;
		
	//! the item list parent
	//public UIGrid	IconItemParentScroll;
	
	/// <summary>
	/// The operation button.
	/// </summary>
	public UIButton		OperationBtn			= null;
	
	/// <summary>
	/// The sell button.
	/// </summary>
	public UIButton		SellBtn					= null;
	
	/// <summary>
	/// The item attribute icon.
	/// </summary>
	// public AttributeIcon[]		ItemAttributeIcon;
	
	//! interval pixel between every items
	private static readonly  float		ItemInterval	= 3;
	
	//! interval pixel between page indicators  
	private static readonly  float		PageIndicatoerInterval	= 24;
	
	//! all item pages' number
	public	int			PageTotalNum					= 2;
	
	//! item number in one page
	private static readonly	int ItemsNumInOnePage		= 20;
	
	//! item number in one row
	private static readonly int ItemsNumInOneRow		= 5;
	
	//! item container size
	private static readonly int ItemContainerSize		= 80;
	
	//! item page size
	private Vector2		ItemPageSize;	
		
	//! current selected item icon data
	private IconData	mCurrSelectedIconData 		= null;
	
	//! the list of UIButton of ItemIcon
	private List<IconData>	mCurrIconItemList		= new List<IconData>();
	
	//! page indicator list
	//private List<UIToggle>	mListPageIndicator = new List<UIToggle>();
	
	//! the List item container to snapped container event
	//private List<UIDragPanelContents> mListItemContainer = new List<UIDragPanelContents>();
		
	//! select page indicator index
	private int		mCurrSelPageIndicatorIdx		= 0;
	
	/// <summary>
	/// The type of the m curr package.
	/// </summary>
	private ItemSlotType				mCurrPackageType;
	
	/// <summary>
	/// The m_op button delegate event.
	/// </summary>
	private OperationBtnDelegate		mOpBtnDelegateEvent = null;
	
	/// <summary>
	/// The m_sel button delegate event.
	/// </summary>
	private SellBtnDelegate				mSellBtnDelegateEvent = null;
	
	/// <summary>
	/// The m icon tap delegate.
	/// </summary>
	private ItemIconTapDelegate			mIconTapDelegate		= null;
	
	/// <summary>
	/// The m package item data delegate to callback when server send packet to change data
	/// </summary>
	private PackageItemDataUpdate		mPackageItemDataDelegate = null;
	
	/// <summary>
	/// The all package unit list.
	/// </summary>
	private static List<PackageUnit>	smPackageUnitList		= new List<PackageUnit>();
	
	// Use this for initialization
	void Awake () {
			
		//ItemPageSize 	= new Vector2(IconItemParentScroll.viewableArea.x - ItemInterval,
								//	IconItemParentScroll.viewableArea.y - ItemInterval);
		
		// create the item
		int t_pageIdx 					= 0;
		GameObject t_containerGO		= null;
		//UIDragPanelContents t_container = null;
		
		int t_addItemSlotNum = PageTotalNum * ItemsNumInOnePage;
		
		for(int i = 0 ;i < t_addItemSlotNum;i++){
			
			ItemSlotData t_data = null;
						
			if (i % ItemsNumInOnePage == 0){
				
				t_containerGO 							= Instantiate(ListContainer) as GameObject;
				t_containerGO.name 						= "Page" + t_pageIdx;
				t_containerGO.transform.localPosition 	= Vector3.zero;
				
				//t_container								= t_containerGO.GetComponent<UIDragPanelContents>() as UIDragPanelContents;
				
				//mListItemContainer.Add(t_container);
				t_pageIdx++;
			}
			
		//	mCurrIconItemList.Add(CreateItem(t_container,t_data,i));
		}
		
		t_pageIdx = 0;
		
		// the EZGUI must fill all item in container and than 
		// addItem in to scroll(UIGrid)
		//
	//foreach(UIDragPanelContents c in mListItemContainer){				
	//	//IconItemParentScroll.AddItem(c);
	//	
	//	// add page indicator
	//	//
	//	GameObject t_indicator 		= Instantiate(PageIndicator) as GameObject;
	//	UIToggle t_checkBtn	= t_indicator.GetComponent<UIToggle>();
	//	
	//	t_indicator.transform.parent	= PageIndicatorParent;				
	//	//t_checkBtn.SetInputDelegate(OnPageIndicatorTapped);
	//	
	//	float t_indicatorWidth		= PageIndicatoerInterval + t_checkBtn.width;
	//	
	//	float t_totalWidth			= t_indicatorWidth * PageTotalNum;
	//	float t_position_x 			= t_pageIdx * t_indicatorWidth - (t_totalWidth - t_indicatorWidth) / 2;
	//	
	//	t_indicator.transform.localPosition = new Vector3(t_position_x,0,-1);
	//	
	//	//mListPageIndicator.Add(t_checkBtn);
	//	
	//	if(t_pageIdx == 0){
	//		t_checkBtn.isChecked = true;
	//	}
	//	
	//	t_pageIdx++;
	//}
		
		// Default Scroll to the first item
		//IconItemParentScroll.ScrollToItem(0, 0.1f);
		//IconItemParentScroll.SetScrollChangeDelegate(ItemScrollChangeDelegateEvent);
		
		if(PageTotalNum == 1){
			PageIndicatorParent.gameObject.SetActiveRecursively(false);
		}
		
		// add the button event
		//OperationBtn.SetInputDelegate(OnOpBtnTapEvent);
		//SellBtn.SetInputDelegate(OnOpBtnTapEvent);
		OperationBtn.transform.localScale = Vector3.zero;
		SellBtn.transform.localScale = Vector3.zero;
		
		smPackageUnitList.Add(this);
		
		// set the default event
		mSellBtnDelegateEvent = delegate(IconData iData) {
			if(iData != null && iData.itemData != null && iData.itemData.MItemData != null){
				if(iData.itemData.MItemData.BasicData.IsPermitSell){
					GUIDialog.PopupSellItemConfigDlg(iData.itemData);	
				}else{
					Globals.Instance.MGUIManager.ShowSimpleCenterTips(21000014,true);
				}				
			}
		};
	}
	
	void Start(){
		// hide the tips in Start after AttributeIcon has Awake
		SetItemDescTips(null);
	}
	
	/// <summary>
	/// Items the scroll change delegate event.
	/// </summary>
	/// <param name='_currPos'>
	/// _curr position.
	/// </param>
	private void ItemScrollChangeDelegateEvent(float _currPos){
		
		_currPos 		= Mathf.Clamp01(_currPos);
		
		//int tIndicatorIdx = (int)(_currPos * mListPageIndicator.Count);
		
		//if(tIndicatorIdx >= mListPageIndicator.Count){
		//	tIndicatorIdx = mListPageIndicator.Count - 1;
		//}
		
		//for(int i = 0;i < mListPageIndicator.Count;i++){
			//mListPageIndicator[i].isChecked = tIndicatorIdx == i;			
		//}	
	}
	
	/// <summary>
	/// Clears the selected icon.
	/// </summary>
	public void ClearSelectedIcon(){
		if(mCurrSelectedIconData != null){
			mCurrSelectedIconData.checkButton.isChecked = false;
			mCurrSelectedIconData = null;			
		}
		
		for(int i = 0;i < mCurrIconItemList.Count;i++){
			IconData t_iconData = mCurrIconItemList[i];
			t_iconData.checkButton.isChecked = false;
		}
		
		SetItemDescTips(null);
		ShowOpBtn(null);
	}
	
	/// <summary>
	/// Ons the destroy to delete the unit which in package list.
	/// </summary>
	void OnDestroy(){
		for(int i = 0;i < smPackageUnitList.Count;i++){
			if(smPackageUnitList[i] == this){
				smPackageUnitList.RemoveAt(i);
				break;
			}			
		}
	}
	
	/// <summary>
	/// Updates the package list if some item of this type was changed
	
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	public static void UpdatePackageList(ItemSlotType type){
		
		for(int i = 0;i < smPackageUnitList.Count;i++){
			
			if(smPackageUnitList[i].mCurrPackageType == type){
				
				smPackageUnitList[i].UpdatePackageItem(type);
				
				if(smPackageUnitList[i].mPackageItemDataDelegate != null){
					
					try{
						smPackageUnitList[i].mPackageItemDataDelegate(smPackageUnitList[i]);
					}catch(System.Exception ex){
						Debug.LogError(ex.Message);
					}					
				}
			}			
		}
	}
	
	
	/// <summary>
	/// Fills the item icon from packages.
	/// </summary>
	/// <param name='itemLogicID'>
	/// Item logic I.
	/// </param>
	/// <param name='list'>
	/// List.
	/// </param>
	public static void FillItemIconFromPackages(int itemLogicID,List<IconData> list){
		for(int i = 0;i < smPackageUnitList.Count;i++){
			smPackageUnitList[i].FillItemIconList(itemLogicID,list);
		}
	}
	
	/// <summary>
	/// Gets the selected icon.
	/// </summary>
	/// <returns>
	/// The selected icon, return null if not selected anything
	/// </returns>
	public IconData GetSelectedIcon(){
		return mCurrSelectedIconData;
	}
		
	// refresh the package item
	public void UpdatePackageItem(ItemSlotType _type){
		
		mCurrPackageType = _type;
		
		// clear the former selected item 
		if(mCurrSelectedIconData != null){
			mCurrSelectedIconData.checkButton.isChecked = false;
		}		
		mCurrSelectedIconData = null;
		
		ItemTipsName.transform.localScale = Vector3.one;
		ItemTipsMoney.transform.localScale = Vector3.one;
		ItemTipsDesc.transform.localScale = Vector3.one;
		
		// foreach(AttributeIcon icon in ItemAttributeIcon){
		// 	icon.transform.localScale = Vector3.one;
		// }
		
		// hide the operation button
		ShowOpBtn(null);
						
		List<ItemSlotData> tDataList = ItemDataManager.Instance.GetItemDataList(_type);
		
		for(int i = 0;i < mCurrIconItemList.Count;i++){
			IconData t_iconData = mCurrIconItemList[i];
			
			t_iconData.checkButton.isChecked = false;
			
			bool t_setIcon = false;
			
			// add Dictionary.Values to List for iterating
			foreach (ItemSlotData data in tDataList){
				if(data.LocationID == i){
					t_setIcon = true;
					t_iconData.itemData = data;
					HelpUtil.SetItemIcon(t_iconData.checkButton.transform,t_iconData.itemData,!ShopSellItemPackage);
					break;
				}
			}
			
			if(!t_setIcon){
				t_iconData.itemData = new ItemSlotData();
				t_iconData.itemData.LocationID	= i;
				t_iconData.itemData.SlotType	= _type;
				t_iconData.itemData.SlotState	= ItemSlotState.LOCK;
				HelpUtil.SetItemIcon(t_iconData.checkButton.transform,null,!ShopSellItemPackage);
			}
		}
		
		mCurrSelPageIndicatorIdx = 0;
		
		// SetPackageHighLight(HighLightRule.HIGHLIGHT_NONE);
	}
	
	/// <summary>
	/// Set Some of ItemList to HighLight by a Rule. by hxl 20121214
	/// Sets the package high light.
	/// </summary>
	/// <param name='rule'>
	/// Rule. Set HighLight by Item Lock UnLock ...
	/// </param>
	/// <param name='logicID'>
	/// Logic I.Set HighLight by a Special Item's logicIDID
	/// </param>
	/// <param name='type'>
	/// Type. Set HighLight by a Special Item's type
	/// </param>
	public void SetPackageHighLight(HighLightRule rule,int logicID = 0,ItemSlotType type = ItemSlotType.NUM){
		
		for(int i = 0;i < mCurrIconItemList.Count;i++){
			IconData t_iconData = mCurrIconItemList[i];
			
			t_iconData.itemIcon.SetItemHighLight(false);
			
			//
			if(t_iconData.checkButton.isChecked)
			{
				continue;
			}
			
			// Logic by logicID
			if(logicID != 0 && t_iconData.itemData.MItemData != null && t_iconData.itemData.MItemData.BasicData.LogicID != logicID){	
				continue;
			}
			
			// Logic by type
			if(type != ItemSlotType.NUM && t_iconData.itemData.SlotType != type){
				continue;
			}
			
			// Logic by rule
			if(rule == HighLightRule.HIGHLIGHT_LOCK){
				if(t_iconData.itemData.IsUnLock()){
					continue;
				}
			}else if(rule == HighLightRule.HIGHLIGHT_UNLOCK){
				if(!t_iconData.itemData.IsUnLock()){
					continue;
				}
			}else if(rule == HighLightRule.HIGHLIGHT_NULL){
				if(t_iconData.itemData.MItemData != null || !t_iconData.itemData.IsUnLock()){
					continue;
				}
			}else if(rule == HighLightRule.HIGHLIGHT_NOTNULL){
				if(t_iconData.itemData.MItemData == null){
					continue;
				}
			}
			else if(rule == HighLightRule.HIGHLIGHT_NONE){
				continue;
			}
			
			t_iconData.itemIcon.SetItemHighLight(true);
		}
	}
	
	/**
	  * add item to container
	  * 
	  * @param _container			scroll list container
	  * @param _item				Item of slot data
	  * @param _idx					total item
	  */ 
//private IconData CreateItem(UIDragPanelContents _container,ItemSlotData _item,int _idx){
//	int t_itemIdx = _idx % ItemsNumInOnePage;
//	
//	float t_page_offset = (_idx < ItemsNumInOnePage)?0:(ItemInterval * 4);
//	
//	// iconSize + iconInterval
//	float t_itemSizeImpl = ItemContainerSize + ItemInterval * 2;
//	
//	// offset y
//	float t_offset_y = ItemPageSize.y / (ItemsNumInOnePage / ItemsNumInOneRow) + t_itemSizeImpl / 2;
//			
//	float t_x = (t_itemIdx % ItemsNumInOneRow) * t_itemSizeImpl + t_itemSizeImpl / 2 + t_page_offset;
//	float t_y = (t_itemIdx / ItemsNumInOneRow) * (-t_itemSizeImpl) + t_offset_y;
//	
//	// item icon template
//	GameObject t_itemIcon 				= Instantiate(ItemIcon) as GameObject;
//	t_itemIcon.name						= "ItemIcon"+_idx;
//	_container.MakeChild(t_itemIcon);
//	t_itemIcon.transform.localPosition	= new Vector3(t_x,t_y,-2);
//	
//	UIToggle t_btn	= t_itemIcon.GetComponent<UIToggle>();
//	t_btn.Data 			= new IconData(_item,t_btn);
//	t_btn.SetInputDelegate(OnItemTapped);
//	
//	HelpUtil.SetItemIcon(t_btn.transform,_item,!ShopSellItemPackage);
//	return (IconData)t_btn.Data;
//}
	
	//! item icon clicked
	//private void OnItemTapped(ref POINTER_INFO ptr){
	//	
	//	if(ptr.evt == POINTER_INFO.INPUT_EVENT.TAP){
	//		
	//		UIButton tBtn 		= (UIButton)ptr.targetObj;
	//		IconData tIconData 	= (IconData)tBtn.Data;
	//		
	//		bool t_hasItem		= tIconData.itemData != null && tIconData.itemData.MItemData != null;
	//		
	//		ItemTipsName.Hide(!t_hasItem);
	//		ItemTipsMoney.Hide(!t_hasItem);
	//		ItemTipsDesc.Hide(!t_hasItem);
	//		
	//		// foreach(AttributeIcon icon in ItemAttributeIcon){
	//		// 	icon.transform.localScale = Vector3.one;
	//		// }
	//		
	//		//! show or hide the operation button
	//		ShowOpBtn(tIconData.itemData);
	//			
	//		tIconData.formerSelect		= mCurrSelectedIconData;
	//		
	//		if(tIconData.itemData != null){
	//			
	//			if(mCurrSelectedIconData != null){
	//				mCurrSelectedIconData.checkButton.isChecked = false;
	//			}
	//			
	//			mCurrSelectedIconData 						= tIconData;
	//			mCurrSelectedIconData.checkButton.isChecked 	= true;
	//			
	//			if(tIconData.itemData.MItemData != null){
	//				ComposeTips(tIconData);
	//			}				
	//		}
	//		
	//		if(mIconTapDelegate != null){
	//			mIconTapDelegate(tIconData);
	//		}			
	//	}
	//}
		
	/// <summary>
	/// Raises the op button tap event event for operation button or sell button
	/// </summary>
	/// <param name='ptr'>
	/// Ptr.
	/// </param>
	//private void OnOpBtnTapEvent(ref POINTER_INFO ptr){
	//	if(ptr.evt == POINTER_INFO.INPUT_EVENT.TAP){
	//		if(ptr.targetObj == OperationBtn){
	//							
	//			if(GetSelectedIcon() != null && !GetSelectedIcon().itemData.IsUnLock()){
	//				
	//				// Unlock the slot
	//				Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
	//				{
	//					gui.SetDialogType(EDialogType.EXTEND_BAG, (int)GetSelectedIcon().itemData.SlotType, GetSelectedIcon().itemData);
	//					gui.SetTextAnchor(ETextAnchor.MiddleLeft,true);
	//				},  EDialogStyle.DialogOkCancel);
	//				
	//			}else{
	//				
	//				if(mOpBtnDelegateEvent != null){
	//					mOpBtnDelegateEvent(GetSelectedIcon());
	//				}
	//			}			
	//			
	//		}else if(ptr.targetObj == SellBtn){
	//			
	//			if(mSellBtnDelegateEvent != null){
	//				mSellBtnDelegateEvent(GetSelectedIcon());
	//			}				
	//		}
	//	}			
	//}
	
	//! compose item data tips to display
	private void ComposeTips(IconData iData){
		SetItemDescTips(iData.itemData);
	}
	
	/// <summary>
	/// Sets the item desc tips.
	/// </summary>
	/// <param name='itemData'>
	/// Item data.
	/// </param>
	/// <param name='clearFormer'>
	/// Clear former item desc
	/// </param>
	public void SetItemDescTips(ItemSlotData itemData,bool clearFormer = true){
		
	
	}
	
	/// <summary>
	/// The color of the rarity.
	/// </summary>
	private static readonly string[] RarityColor = 
	{
		GUIFontColor.White,	
		GUIFontColor.PureGreen,
		GUIFontColor.LightBlueSky000216255,
		GUIFontColor.Purple,
		GUIFontColor.Orange,
		GUIFontColor.PureRed,
	};
	
	private static string[] RarityQuanlityName = 
	{
		"丁",
		"丙",
		"乙",
		"甲",
		"神"
	};
	
	
	/// <summary>
	/// Gets the color of the item data name.
	/// </summary>
	/// <returns>
	/// The item data name color.
	/// </returns>
	/// <param name='itemData'>
	/// Item data.
	/// </param>
	public static string GetItemDataNameColor(ItemData itemData){
		if(itemData == null || itemData.BasicData == null){
			return RarityColor[0];
		}
		
		return GetItemDataNameColor((int)itemData.BasicData.RarityLevel);
	}
		
	/// <summary>
	/// Gets the color of the item data name.
	/// </summary>
	/// <returns>
	/// The item data name color.
	/// </returns>
	/// <param name='rarityLevel'>
	/// Rarity level.
	/// </param>
	public static string GetItemDataNameColor(int rarityLevel){
		
		int tColorIdx = rarityLevel - 1;
		if(tColorIdx < 0){
			return RarityColor[0];
		}
		
		if(tColorIdx >= RarityColor.Length){
			return RarityColor[RarityColor.Length - 1];
		}
		
		return RarityColor[tColorIdx];
	}
	
	/// <summary>
	/// Gets the name of the item data quanlity.
	/// </summary>
	/// <returns>
	/// The item data quanlity name.
	/// </returns>
	public static string GetItemDataQuanlityName(ItemData itemData){
		
				
		if(itemData == null || itemData.BasicData == null){
			return RarityQuanlityName[0];
		}
		
		return GetItemDataQuanlityName((int)itemData.BasicData.RarityLevel);
	}
	
	/// <summary>
	/// Gets the name of the item data quanlity.
	/// </summary>
	/// <returns>
	/// The item data quanlity name.
	/// </returns>
	/// <param name='rarityLevel'>
	/// Rarity level.
	/// </param>
	public static string GetItemDataQuanlityName(int rarityLevel){
				
		int tColorIdx = rarityLevel - 1;
		if(tColorIdx < 0 ){
			return RarityQuanlityName[0];
		}
		
		if(tColorIdx >= RarityColor.Length){
			return RarityQuanlityName[RarityQuanlityName.Length - 1];
		}
		
		return RarityQuanlityName[tColorIdx];
	}
	
	/// <summary>
	/// Shows the op button.
	/// </summary>
	/// <param name='_itemData'>
	/// _item data. hide the all operation button if null
	/// </param>
	private void ShowOpBtn(ItemSlotData iItemData){
		
		if(ShopSellItemPackage || HideOperationBtnAlways){
			// just return if it's shop sell item pacage
			// operate outside this class
			return;
		}
				
		if(iItemData == null || iItemData.MItemData == null){
			
			if(iItemData != null && !iItemData.IsUnLock()){
				OperationBtn.transform.localScale = Vector3.zero;
				OperationBtn.GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(21000006);
			}else{		
				OperationBtn.transform.localScale = Vector3.one;
			}
			
			SellBtn.transform.localScale = Vector3.one;
			
		}else{
			
			if(!HideRightOperationBtn){
				SellBtn.transform.localScale = Vector3.zero;
			}			
			
			if(iItemData.MItemData.BasicData.MajorType == ItemMajorType.MATERIAL || HideLeftOperationBtn){
				// mat type item will not display the operation button
				OperationBtn.transform.localScale = Vector3.one;
			}else{
				OperationBtn.transform.localScale = Vector3.zero;
			}
			
			if(iItemData.MItemData.BasicData.MajorType == ItemMajorType.SHIP_EQUIP
			|| iItemData.MItemData.BasicData.MajorType == ItemMajorType.GENERAL_EQUIP){
				
				OperationBtn.GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(21000007);
				
			}else if(iItemData.MItemData.BasicData.MajorType == ItemMajorType.JUNHUN){
				
				OperationBtn.GetComponent<GUIText>().text	= Globals.Instance.MDataTableManager.GetWordText(23700016);
				SellBtn.GetComponent<GUIText>().text		= Globals.Instance.MDataTableManager.GetWordText(23700001);
				
			}else if(iItemData.MItemData.BasicData.MajorType == ItemMajorType.SHIP_CARD
				|| iItemData.MItemData.BasicData.MajorType == ItemMajorType.EQUIPMENT_CARD
				|| iItemData.MItemData.BasicData.MajorType == ItemMajorType.FEMULAR_CARD){
				
				OperationBtn.GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(21000007);
				// OperationBtn.Text = Globals.Instance.MDataTableManager.GetWordText(21600005);
				
			}else if (iItemData.MItemData.BasicData.MajorType == ItemMajorType.GIFT_PACKAGE
				|| iItemData.MItemData.BasicData.MajorType == ItemMajorType.EXPENDABLE
				|| iItemData.MItemData.BasicData.MajorType == ItemMajorType.ZHENTU){
				OperationBtn.GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(21000007);
			}
		}
	}
	
	/// <summary>
	/// Hides the op button.
	/// </summary>
	/// <param name='hide'>
	/// Hide or show state
	/// </param>
	public void HideOpBtn(bool hide){
		
		if(hide == false){
			
			if(GetSelectedIcon() != null && GetSelectedIcon().itemData != null ){
				if(!GetSelectedIcon().itemData.IsUnLock()){
					
					// lock item
					//
					SellBtn.transform.localScale = Vector3.one;
					OperationBtn.transform.localScale = Vector3.zero;
					
					return;
					
				}else if(GetSelectedIcon().itemData.MItemData == null){
					
					// empty slot
					//
					OperationBtn.transform.localScale = Vector3.one;
					SellBtn.transform.localScale = Vector3.one;
					
					return;
				}
			}	
		}
				
		OperationBtn.transform.localScale =  hide ?  Vector3.zero :Vector3.one;
		SellBtn.transform.localScale =  hide ?  Vector3.zero :Vector3.one;
	}
	
	/// <summary>
	/// Fills the item icon list.
	/// </summary>
	/// <param name='itemLogicID'>
	/// Item logic I.
	/// </param>
	/// <param name='fillList'>
	/// Fill list.
	/// </param>
	public void FillItemIconList(int itemLogicID,List<IconData> fillList){
		
		for(int i = 0;i < mCurrIconItemList.Count;i++){
			IconData t_iconData = mCurrIconItemList[i];
			
			if(t_iconData.itemData != null 
			&& t_iconData.itemData.MItemData != null 
			&& t_iconData.itemData.MItemData.BasicData.LogicID == itemLogicID){
				fillList.Add(t_iconData);
			}
		}
	}

	//! page indicator tapped event
//	private void OnPageIndicatorTapped(ref POINTER_INFO _ptr){
//		
//		if(_ptr.evt == POINTER_INFO.INPUT_EVENT.PRESS){
//			
//			for(int i = 0;i < mListPageIndicator.Count;i++){
//				if(mListPageIndicator[i] == _ptr.targetObj){
//					
//					SetPageIndicatorSelected(i,true);
//					
//					break;
//				}
//			}
//		}
//	}
		
	//! set the page indicator selected
	private void SetPageIndicatorSelected(int _idx,bool _scroll){
		
		mCurrSelPageIndicatorIdx = _idx;
		
		//for(int i = 0;i < mListPageIndicator.Count;i++){
		//	UIToggle t_check = mListPageIndicator[i];
			
		//	if(i == _idx){
		//		t_check.isChecked = true;
		//		
		//		if(_scroll){
		//			//IconItemParentScroll.ScrollPosition = i;
		//		}
		//		
		//	}else{
		//		t_check.isChecked = false;
		//	}
		//}
	}
	
	/// <summary>
	/// Sets the item icon tap delegate.
	/// </summary>
	public void SetItemIconTapDelegate(ItemIconTapDelegate iDele){
		mIconTapDelegate		= iDele;
	}
	
	/// <summary>
	/// Adds the item icon tap delegate.
	/// </summary>
	/// <param name='iDele'>
	/// I dele.
	/// </param>
	public void AddItemIconTapDelegate(ItemIconTapDelegate iDele){
		mIconTapDelegate		+= iDele;
	}
	
	/// <summary>
	/// Sets the op button delegate event when selected icon and tap the operation button
	/// </summary>
	/// <param name='_dele'>
	/// _dele.
	/// </param>
	public void SetOpBtnDelegate(OperationBtnDelegate iDele){
		mOpBtnDelegateEvent = iDele;
	}
	
	/// <summary>
	/// Adds the op button delegate event when selected icon and tap the operation button
	/// </summary>
	/// <param name='iDele'>
	/// I dele.
	/// </param>
	public void AddOpBtnDelegate(OperationBtnDelegate iDele){
		mOpBtnDelegateEvent += iDele;
	}
	
	/// <summary>
	/// Adds the sell button delegate.
	/// </summary>
	/// <param name='iDele'>
	/// I dele.
	/// </param>
	public void AddSellBtnDelegate(SellBtnDelegate iDele){
		mSellBtnDelegateEvent	+= iDele;
	}
	
	/// <summary>
	/// Sets the sell button delegate.
	/// </summary>
	/// <param name='iDele'>
	/// I dele.
	/// </param>
	public void SetSellBtnDelegate(SellBtnDelegate iDele){
		mSellBtnDelegateEvent	= iDele;
	}
	
	/// <summary>
	/// Sets the package item data chanaged delegate.
	/// </summary>
	/// <param name='item'>
	/// Item.
	/// </param>
	public void SetPackageItemDataChanagedDelegate(PackageItemDataUpdate dele){
		mPackageItemDataDelegate = dele;
	}
	
	/// <summary>
	/// Adds the package item data changed delegate.
	/// </summary>
	/// <param name='dele'>
	/// Dele.
	/// </param>
	public void AddPackageItemDataChangedDelegate(PackageItemDataUpdate dele){
		mPackageItemDataDelegate += dele;
	}
	
	/**
	 * Icon data for IconButton
	 */ 
	public class IconData{
		public ItemSlotData 		itemData 		= null;
		public UIToggle		checkButton		= null;
		public PackageItemIcon		itemIcon		= null;
		
		/// <summary>
		/// The former select for ItemTapDelegate
		/// </summary>
		public IconData				formerSelect	= null;
		
		public IconData(ItemSlotData _item,UIToggle _btn){
			itemData 		= _item;
			checkButton		= _btn;
			itemIcon		= _btn.gameObject.GetComponent<PackageItemIcon>();
		}
	}
	
	// HighLight Rule enum by hxl 20121214
	public enum HighLightRule
	{
		HIGHLIGHT_NONE,
		HIGHLIGHT_LOCK,
		HIGHLIGHT_UNLOCK,
		HIGHLIGHT_NULL,
		HIGHLIGHT_NOTNULL,
		HIGHLIGHT_ALL,
	}
}
