using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JunHunManager : MonoBehaviour 
{
	#region JunHunBag Defines
	public static readonly int JUNHUN_ROW_COUNT_PER_PAGE = 4;
	public static readonly int JUNHUN_COLUME_COUNT_PER_PAGE = 4;
	public static readonly int JUNHUN_GRID_COUNT_PER_PAGE = JUNHUN_ROW_COUNT_PER_PAGE * JUNHUN_COLUME_COUNT_PER_PAGE;
	#endregion
	
	#region TreasureMap Defines
	public static readonly int TREASURE_MAP_ROW_COUNT = 4;
	public static readonly int TREASURE_MAP_COLUME_COUNT = 6;
	public static readonly int TREASURE_MAP_MAX_GRIDS = TREASURE_MAP_ROW_COUNT * TREASURE_MAP_COLUME_COUNT;
	#endregion
	
	
	public class SysPromptInfo
	{
		public int roleID;
		public string roleName;
		public string junHunName;
		public string digTime;
		public int rarityLevel;
		
		public SysPromptInfo(int id, int _rarityLevel, string name, string soulName, string grubTime)
		{
			roleID = id;
			roleName = name;
			junHunName = soulName;
			digTime = grubTime;
			rarityLevel = _rarityLevel;
		}
	}
	
	//--------------------------------------------------
	public int TreasureMapFreeCount
	{
		get;
		set;
	}
	
	public int TreasureMapBuyRemainCount
	{
		get;
		set;
	}
	
	public int TreasureMapCreateCount
	{
		get;
		set;
	}
	
	public int TreasureMapPrice
	{
		get;
		set;
	}
	
	// 免费挖掘总次数
	public int FreeDigCount
	{
		get;
		set;
	}
	
	// 已经挖掘次数
	public int GrubNumber
	{
		get;
		set;
	}
	
	// 挖掘需要的钻石
	public int NextConsumeIngot
	{
		get;
		set;
	}
	
	// 当前剩余免费挖掘次数
	public int CurrentDigCount
	{
		get;
		set;
	}
	
	public List<ItemSlotData> CurrentTreasureMapItemDatas
	{
		get { return currentTreasureMapItemDatas; }
	}
	
	public List<ItemSlotData> CurrentCanbeFoundJunhunDatas
	{
		get { return currentCanbeFoundJunhunDatas; }
	}
	
	public List<SysPromptInfo> SysPromptInfos
	{
		get { return sysPromptInfos; }
	}
	//--------------------------------------------------
		
	void Awake()
	{
		//BuildInitTreasureMapDatas();
	}
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	public void BuildUnkndownTreasureMapDatas()
	{
		currentTreasureMapItemDatas.Clear();
		for (int i = 0; i < TREASURE_MAP_MAX_GRIDS; ++i)
		{
			ItemSlotData data = new ItemSlotData();
			data.LocationID = i;
			data.SlotState = ItemSlotState.LOCK; // Lock is mean a unknown slot in treasure map
			data.SlotType = ItemSlotType.JUNHUN_TREASURE_MAP;
			
			currentTreasureMapItemDatas.Add(data);
		}
	}
	
	public void BuildInitTreasureMapDatas()
	{
		currentTreasureMapItemDatas.Clear();
		for (int i = 0; i < TREASURE_MAP_MAX_GRIDS; ++i)
		{
			ItemSlotData data = new ItemSlotData();
			data.LocationID = i;
			data.SlotState = ItemSlotState.LOCK;
			data.SlotType = ItemSlotType.JUNHUN_TREASURE_MAP;
			
			currentTreasureMapItemDatas.Add(data);
		}
		
		// Test code
		/*currentCanbeFoundJunhunDatas.Clear();
		for (int i = 0; i < 10; ++i)
		{
			ItemSlotData data = new ItemSlotData();
			data.LocationID = TREASURE_MAP_MAX_GRIDS + i;
			data.SlotState = ItemSlotState.LOCK;
			data.SlotType = ItemSlotType.JUNHUN_TREASURE_MAP;
			
			currentCanbeFoundJunhunDatas.Add(data.LocationID, data);
		}*/
	}
	
	public void UpdateTreasureMapData(ItemSlotData data)
	{
		for(int i = 0; i < currentTreasureMapItemDatas.Count; i++)
		{
			if(currentTreasureMapItemDatas[i].LocationID == data.LocationID)
			{
				currentTreasureMapItemDatas[i] = data;
			}
		}
		
	}
	
	private int currentTreasureMapCount = 10;
	private int treasureMapPrice = 20; // Unit is GoldIgnot
	
	private int currentFreeDigCount = 4;
	
	private List<ItemSlotData> currentTreasureMapItemDatas = new List<ItemSlotData>();
	// Current treasure map can dig how and what items
	private List<ItemSlotData> currentCanbeFoundJunhunDatas = new List<ItemSlotData>();
	
	private List<SysPromptInfo> sysPromptInfos = new List<SysPromptInfo>();
}
