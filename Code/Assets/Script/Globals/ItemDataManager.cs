using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagDefines
{
	public static readonly int ROW_COUNT_PER_PAGE = 3;
	public static readonly int COLUME_COUNT_PER_PAGE = 4;
	public static readonly int MAX_GRID_COUNT_PER_PAGE = ROW_COUNT_PER_PAGE * COLUME_COUNT_PER_PAGE;
	
	public static readonly int MAX_PAGE_COUNT = 4;
	public static readonly int MAX_GRID_COUNT = 48;
	
	// 0-14 id default is unlock
	public static readonly int DEFAULT_BAG_UNLOCK_MAX_LOCATION_ID = 14;
	// The gold ignot cost when turn on the item slot
	public static readonly int UNLOCK_GRID_COST = 2;
	public static int CalculateUnLockIngot(int endLocationID)
	{
		int n = endLocationID -  DEFAULT_BAG_UNLOCK_MAX_LOCATION_ID;
		int cost = UNLOCK_GRID_COST * (n * (n + 1) / 2);
		return cost;
	}
}

public class ItemDataManager
{
	private static readonly ItemDataManager instance = new ItemDataManager();
	
	/// <summary>
	/// Gets the instance of item data manager
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>
    public static ItemDataManager Instance{
        get{return instance;}
    }
	
	// ----------------------------------------------------------
	public DropData BattleDropData
	{
		get { return _mBattleDropData; }
		set { _mBattleDropData = value; }
	}
	
	public DropData CopyDropData
	{
		get { return _mCopyDropData; }
		set { _mCopyDropData = value; }
	}
	
	/// <summary>
	/// Updates the item data by data.LocationID
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='data'>
	/// Data with right locatioID
	/// </param>
	public void UpdateItemData(ItemSlotType type, ItemSlotData data)
	{
		List<ItemSlotData> tList = GetItemDataList(type);
		
		bool t_found = false;
		
		for(int i = 0;i < tList.Count;i++){
			
			ItemSlotData item = tList[i];
			
			if(item.LocationID == data.LocationID){
				tList[i] = data;
				t_found = true;
				break;
			}
		}
		
		if(!t_found){
			tList.Add(data);
		}
		
		PackageUnit.UpdatePackageList(type);
	}
	
	/// <summary>
	/// Adds the item data.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='data'>
	/// Data.
	/// </param>
	public void AddItemData(ItemSlotType type, ItemSlotData data)
	{
		List<ItemSlotData> tList = GetItemDataList(type);
		tList.Add(data);
		
		PackageUnit.UpdatePackageList(data.SlotType);
	}
	
	/// <summary>
	/// Removes the item data by the type and data LocationID
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='data'>
	/// Data.
	/// </param>
	public void RemoveItemData(ItemSlotType type, ItemSlotData data)
	{
		RemoveItemData(type,data.LocationID);
		PackageUnit.UpdatePackageList(data.SlotType);
	}
		
	/// <summary>
	/// Removes the item data.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <param name='locationID'>
	/// Location ID
	/// </param>
	public void RemoveItemData(ItemSlotType type, int locationID)
	{
		List<ItemSlotData> tList = GetItemDataList(type);
		foreach(ItemSlotData item in tList){
			if(item.LocationID == locationID){
				tList.Remove(item);
				PackageUnit.UpdatePackageList(type);
				break;
			}
		}
	}
	
	/// <summary>
	/// Gets the item data.
	/// </summary>
	/// <returns>
	/// The item data.
	/// </returns>
	/// <param name='iType'>
	/// type of 
	/// </param>
	/// <param name='iLocationID'>
	/// I location I.
	/// </param>
	public ItemSlotData GetItemData(ItemSlotType type, int locationID)
	{
		List<ItemSlotData> tList = GetItemDataList(type);
		foreach(ItemSlotData item in tList){
			if(item.LocationID == locationID){
				return item;
			}
		}
		
		return null;
	}
	
	public ItemSlotData GetItemDataByGUID(ItemSlotType type, long guiID)
	{
		List<ItemSlotData> tList = GetItemDataList(type);
		foreach(ItemSlotData item in tList){
			if(item.MItemData != null && item.MItemData.ItemID == guiID){
				return item;
			}
		}
		
		return null;
	}
	
	/// <summary>
	/// The sm find list.
	/// </summary>
	public static readonly ItemSlotType[] smFindList = {
		
		ItemSlotType.GENERAL_BAG,
		ItemSlotType.CLOTH_BAG,
		ItemSlotType.OTHER_BAG,
		ItemSlotType.SHIP_CARD_BAG,
		
		ItemSlotType.EQU_CARD_BAG,
		ItemSlotType.PILL_CARD_BAG,
		ItemSlotType.JUNHUN_BAG,
	};

	/// <summary>
	/// Gets the item number by identifier in follow bag:
	///		ItemSlotType.GENERAL_BAG,
	/// 	ItemSlotType.CLOTH_BAG,
	/// 	ItemSlotType.OTHER_BAG,
	/// 	ItemSlotType.SHIP_CARD_BAG,
	/// 	
	/// 	ItemSlotType.EQU_CARD_BAG,
	/// 	ItemSlotType.PILL_CARD_BAG,
	/// 	ItemSlotType.JUNHUN_BAG,
	/// </summary>
	/// <returns>
	/// The item number by identifier.
	/// </returns>
	/// <param name='_logicID'>
	/// _logic I.
	/// </param>
	/// <param name='_loadList'>
	/// load list, if NOT null, add the item to this list
	/// </param>
	/// <param name='includeGeneralInfo'>
	/// search general's own equiped item if true
	/// </param>
	/// <param name='includeWarshipInfo'>
	/// search warship's own equiped item if true
	/// </param>
	public int GetItemNumById(int _logicID,List<ItemSlotData> _loadList = null,bool includeGeneralInfo = true,bool includeWarshipInfo = true){
		int t_num = 0;
		
		foreach(ItemSlotType type in smFindList){
			
			if(!mItemDataListArray.ContainsKey(type)) return t_num;
			
			List<ItemSlotData> list =  mItemDataListArray[type];
			
			if(list != null){
				
				foreach(ItemSlotData item in list){
					if(item.MItemData != null && item.MItemData.BasicData.LogicID == _logicID){
						
						t_num += item.MItemData.BasicData.Count;
						
						if(_loadList != null){
							_loadList.Add(item);
						}
					}
				}
			}
		}
		
		t_num += Globals.Instance.MGameDataManager.MActorData.GetItemNumById(_logicID,_loadList,includeGeneralInfo,includeWarshipInfo);
		
		return t_num;
	}
	
	
	/// <summary>
	/// Gets the item data list.
	/// </summary>
	/// <returns>
	/// The item data list.
	/// </returns>
	/// <param name='type'>
	/// Type. item solt type
	/// </param>
	public List<ItemSlotData> GetItemDataList(ItemSlotType iType)
	{
		List<ItemSlotData> tList;
		if(!mItemDataListArray.ContainsKey(iType)){
			// new a list 
			//
			tList = new List<ItemSlotData>();
			mItemDataListArray.Add(iType,tList);
			
		}else{
			
			tList = mItemDataListArray[iType];
			
		}
		
		return tList;
	}
	
	/// <summary>
	/// Removes the item data list.
	/// </summary>
	/// <param name='type'>
	/// Type.
	/// </param>
	public void RemoveItemDataList(ItemSlotType type){
		GetItemDataList(type).Clear();
		PackageUnit.UpdatePackageList(type);
	}
			
	public void AddDropData(DropData data)
	{
		_mDropDataList.Add(data);
	}
	
	public void RemoveDropData(DropData data)
	{
		_mDropDataList.Remove(data);
	}
	
	public void RemoveAllDropData()
	{
		_mDropDataList.Clear();
	}

	private Dictionary<ItemSlotType, List<ItemSlotData>> mItemDataListArray = new Dictionary<ItemSlotType, List<ItemSlotData>>();
	
	private DropData _mBattleDropData = new DropData();
	private DropData _mCopyDropData = new DropData();
	private List<DropData> _mDropDataList;
}
