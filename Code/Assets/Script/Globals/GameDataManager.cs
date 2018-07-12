using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour 
{
	public PlayerData MActorData = null;
	public PlayerData MEnemyData = null;
	
	public SeaAreaData MCurrentSeaAreaData = null;
	public PortData MCurrentPortData = null;
	public CopyData MCurrentCopyData = null;
	
	public Dictionary<int, SeaAreaData> MSeaAreaDataList = null;
	#region Technology
	ISubscriber playerVipDataSub;
	readonly int TechVipLevelLimit = 2;
	public List<GameData.TechInfo> produceTeches;
	public List<GameData.TechInfo> warTeches;
	public List<GameData.TechInfo> formationTeches;
	public float techStudySpeed;//!studySpeedPoint/studySpeedSecond(point / second)
	public int techStudyPassCnt = 1;
	public int mInvitedCodeReceived;
	public Transform [] A001Source;
	public GameObject BonePhysicObj;
	
	
	protected void Awake()
	{
		MActorData = new PlayerData();
		MEnemyData = new PlayerData();
		
		MCurrentSeaAreaData = new SeaAreaData();
		// Just for easy call
		MCurrentPortData = MCurrentSeaAreaData.MPortData;
		MCurrentCopyData = MCurrentSeaAreaData.MCurrentCopyData;
		
		MSeaAreaDataList = new Dictionary<int, SeaAreaData>();
		
		produceTeches = new List<GameData.TechInfo>();
		warTeches = new List<GameData.TechInfo>();
		formationTeches = new List<GameData.TechInfo>();
	}
	
	void Start()
	{
		playerVipDataSub = EventManager.Subscribe(PlayerDataPublisher.NAME + ":" + PlayerDataPublisher.EVENT_VIP_UPDATE);
		playerVipDataSub.Handler = delegate (object[] args)
		{
			if (MActorData.VipData.Level >= TechVipLevelLimit)
			{
				techStudyPassCnt = 2;
			}
		};
	}
	
	void Update()
	{
		for (int i = 0; i < cdTimings.Count; i++)
		{
			cdTimings[i].Update();
		}
		
		MActorData.Update();
	}
	
	public void Release()
	{
		if (MActorData !=null) {
			MActorData.ClothDatas.Clear();
			MActorData.MainTaskIDList.Clear();
			MActorData.WarshipList.Clear();
			MActorData.EquipmentDatas.Clear();
			MActorData.MHoldFormationDataList.Clear();
			MActorData = null;
		}
	}
	
	/**
	 * tzz added
	 * fill enemy player data by BattleResult it has been used by NetReceiver.cs and TeachFirstEnterGame.cs
	 * 
	 * @param newBattleResult		battle result data
	 */ 
	public void FillEnemyPlayerData(GameData.BattleGameData.BattleResult newBattleResult){
		
		if(MActorData == null || MEnemyData == null){
			throw new System.Exception("MActorData == null || MEnemyData == null");
		}
		
		// Fill in enemy PlayerData
		PlayerData playerData = null;
		foreach (GameData.BattleGameData.Fleet fleetData in newBattleResult.Fleets) 
		{
			// by lsj 
			if(newBattleResult.BattleType == GameData.BattleGameData.BattleType.PORT_VIE_BATTLE &&
				Globals.Instance.MPortVieManager.puFlagReverseFleetPosition)
			{
				if(fleetData.IsAttacker)
				{
					playerData = MEnemyData;
					playerData.ClearWarshipDatas();
				}
				else
				{
					playerData = MActorData;
				}
			}
			else
			{
				if (fleetData.IsAttacker)
				{
					playerData = MActorData;
				}
				else
				{
					playerData = MEnemyData;
					playerData.ClearWarshipDatas();
				}
			}
		
			List<GameData.BattleGameData.Ship> shipDatas = fleetData.Ships;
			foreach (GameData.BattleGameData.Ship shipData in shipDatas) 
			{
				GirlData data = playerData.GetGirlData(shipData.ShipID);
				if (null == data)
				{
					data = new GirlData();
					data.roleCardId = shipData.ShipID;
//					data.BasicData.LogicID = shipData.LogicShipID;
					
					playerData.AddWarshipData(shipData.ShipID, data);
				}
				else
				{
					data.roleCardId = shipData.ShipID;
//					data.BasicData.LogicID = shipData.LogicShipID;
				}
				
				// Is Npc ship
//				data.BasicData.IsNpc = shipData.IsNpc;
				
//				data.BasicData.Name = shipData.ShipName;
//				data.BasicData.TypeID = (EWarshipTypeID)shipData.TypeID;
//				switch (data.BasicData.TypeID)
//				{
//				case EWarshipTypeID.AIRCRAFT_CARRIER:
//					data.BasicData.Type = EWarshipType.CARRIER;
//					break;
//				case EWarshipTypeID.SURFACE_CRAFT:
//				case EWarshipTypeID.SURFACE_CRAFT_1:
//				case EWarshipTypeID.SURFACE_CRAFT_2:
//				case EWarshipTypeID.SURFACE_CRAFT_3:
//				case EWarshipTypeID.SURFACE_CRAFT_4:
//				case EWarshipTypeID.SURFACE_CRAFT_5:
//				case EWarshipTypeID.SURFACE_CRAFT_6:
//					data.BasicData.Type = EWarshipType.SURFACE_SHIP;
//					break;
//				case EWarshipTypeID.UNDER_WATER_CRAFT:
//					data.BasicData.Type = EWarshipType.SUBMARINE;
//					break;
//				}
//				data.BasicData.FillDataFromConfig();
				
				data.PropertyData.Life = shipData.InitialCurrentLife;
				//data.PropertyData.MaxLife = shipData.MaxLife;
				data.PropertyData.Power = 0;
				data.PropertyData.MaxPower = GirlData.MAX_POWSER;
			}
		}
	}


	public List<GameData.TechInfo> GetStudyingList()
	{
		List<GameData.TechInfo> list = new List<GameData.TechInfo>();
		for (int i = 0; i < produceTeches.Count; i++)
		{
			if (produceTeches[i].IsResearch)
			{
				list.Add(produceTeches[i]);
			}
		}
		
		for (int i = 0; i < warTeches.Count; i++)
		{
			if (warTeches[i].IsResearch)
			{
				list.Add(warTeches[i]);
			}
		}
		
		for (int i = 0; i < formationTeches.Count; i++)
		{
			if (formationTeches[i].IsResearch)
			{
				list.Add(formationTeches[i]);
			}
		}
		
		list.Sort(CompareByStartTime);
		return list;
	}
	
	int CompareByStartTime(GameData.TechInfo a, GameData.TechInfo b)
	{
		bool isAscendingOrder = false;
		if (isAscendingOrder)
			return (int)(a.StudyStart - b.StudyStart);
		else
			return (int)(b.StudyStart - a.StudyStart);
		
		return 1;
	}

	//public int 					   techStudyPoints;
	
	#endregion

	private static List<object>	mServerDataList = new List<object>();
	
	/// <summary>
	/// Gets the server data.
	/// </summary>
	/// <returns>
	/// The server data.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public static T GetServerData<T>(){
		foreach(object data in mServerDataList){
			if(data.GetType() == typeof(T)){
				return (T)data;
			}
		}
		
		return default(T);
	}
	
	/// <summary>
	/// Sets the server data.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public  static void SetServerData(object obj){
		
		for(int i = 0;i < mServerDataList.Count;i++){
			if(mServerDataList[i].GetType() == obj.GetType()){
				mServerDataList[i] = obj;
				return;
			}
		}
		
		mServerDataList.Add(obj);
	}
	
	// LiHaojie 2013.01.17 Unify all cd time calculate
	List<CDTiming> cdTimings = new List<CDTiming>();
	public void AddCDTiming(CDTiming timing)
	{
		for (int i = 0; i < cdTimings.Count; i++)
		{
			if (cdTimings[i].EventName.Equals(timing.EventName))
			{
				cdTimings.Remove(cdTimings[i]);
				break;
			}
		}
		
		cdTimings.Add(timing);
	}
	
	public void RemoveCDTiming(CDTiming timing)
	{
		cdTimings.Remove(timing);
	}
	
	public List<CDTiming> GetCDTimings()
	{
		return cdTimings;
	}
	
	// BagData
	// AchieveData
	// MailData
	// ...
}

/// <summary>
/// CD timing.
/// </summary>
public delegate void CDTimingDelegate(long remainTime);
public class CDTiming
{
	// public enum TimingUnit
	// {
	// 	Second,
	// 	MilliSecond,
	// }
	
	public string EventName
	{
		get { return eventName.ToLower(); }
	}
	
	public CDTiming(string name)
	{
		eventName = name;
	}
	
	string eventName = "";
	long coolingEndTime; // unit is Millisecond
	bool isEndTiming = false;
	// TimingUnit timingUnit = TimingUnit.MilliSecond;
	
	CDTimingDelegate updateDel = null;
	CDTimingDelegate endDel = null;
	
	float cdStartRealTime = 0.0f;
	ISubscriber appFocusSub = null;
	
	// coolingTime must is Millisecond
	public void BeginCDTiming(long coolingTime, CDTimingDelegate iUpdateDel = null, CDTimingDelegate iEndDel = null)
	{
		coolingEndTime = coolingTime;
		isEndTiming = false;
		cdStartRealTime = Time.realtimeSinceStartup;
		
		updateDel = iUpdateDel;
		endDel = iEndDel;
		
		RegisterAppFocusSubscribes();
	}
	
	public void ActiveCDTiming(long coolingTime)
	{
		coolingEndTime = coolingTime;
		isEndTiming = false;
		
		cdStartRealTime = Time.realtimeSinceStartup;
	}
	
	void RegisterAppFocusSubscribes()
	{
		appFocusSub = EventManager.Subscribe(MonoEventPublisher.NAME + ":" + MonoEventPublisher.MONO_FOCUS);
		appFocusSub.Handler = delegate (object[] args)
		{
			bool focus = (bool)args[0];
			long ms = (long) args[1];
			
			if (focus)
			{
				coolingEndTime -= ms;
				
				if (!isEndTiming)
					TimeTicks();
			}
		};
	}
	
	public void Update()
	{
		if (isEndTiming)
			return;
		
		float remain = (Time.realtimeSinceStartup - cdStartRealTime);
		if (remain >= 1.0f)
		{
			// Hold the remain time
			remain -= 1.0f;
			cdStartRealTime = Time.realtimeSinceStartup - remain;
			
			coolingEndTime -= 1000;
			TimeTicks();
		}
	}
	
	void TimeTicks()
	{
		if (coolingEndTime <= 0)
		{
			coolingEndTime = 0;
		}
			
		if (null != updateDel)
		{
			updateDel(coolingEndTime);
		}
		
		if (coolingEndTime == 0 && null != endDel)
		{
			isEndTiming = true;
			endDel(coolingEndTime);
		}
		
	}
}
