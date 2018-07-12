using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyBasicData
{
	public int SeaAreaID;
	public bool isElitCopy;
	public int CopyID;
	public int CopyNameID;
	
	/// <summary>
	/// The type of the _copy.
	/// 0 : common copy
	/// 1 : Elite copy
	/// 2 : boss copy
	/// </summary>
	public int CopyType;
	
	
	public string CopyName;
	public string CopySceneName;
	public string BattleSceneName;
	
	public string CopyPic;
	
	public int CopyScore = 0;
	public bool CopyIsAllowEnter;
	public bool CopyIsAllowRaids;
	
	public string CopyDropItems;
	
	public string CopyLevelRequire;
	public string CopyFeatRequire;
	public string CopyReputationRequire;
	public string CopyAchieveRequire;
	public string CopyTaskRequire;
	public int CopyOilRequire;
	public int CopyMoneyRequire;
	public int CopyIngotRequire;
	public string CopyCampRequire;
	public string CopyEnterCameraTrack;
	
	
	public Vector3 ActorBirthPosition;
	public string ChestPointsFile;
	
	public void FillDataFromConfig()
	{
		
	}
}

public class CopyMonsterData
{
	public int CopyID;
	public class MonsterData
	{
		public int FleetID;
		public string ModelName;
		
		public int MonsterNameID;
		public string MonsterName;
		
		public int MonsterDialogID;
		public string MonsterDialog;
		
		public Vector3 MonsterPosition;
		
		public string PathPointsFile;
		public float DetectionRangeFactor;
		public float BattleRangeFactor;
	}
	
	public List<CopyMonsterData.MonsterData> MMonsterDataList = new List<CopyMonsterData.MonsterData>();
	
	public void FillDataFromConfig()
	{
	}
}

public class CopyChestData
{
	public List<int> ChestIDList = new List<int>();
}

public class CopyData
{
	public static int INVALID_ID = -1;
	
	public CopyBasicData MCopyBasicData = new CopyBasicData();
	public CopyMonsterData MCopyMonsterData = new CopyMonsterData();
	public CopyChestData MCopyChestData = new CopyChestData();
	
	public void FillDataFromConfig()
	{
		MCopyBasicData.FillDataFromConfig();
		MCopyMonsterData.FillDataFromConfig();
	}
}
