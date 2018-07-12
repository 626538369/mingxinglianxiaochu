using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvolutionManager : MonoBehaviour 
{
	[HideInInspector]public int gFlushConsumeIngot;
	[HideInInspector]public int gFlushConsumeStrength;
	[HideInInspector]public int gChoiceCosumeStrength;
	[HideInInspector]public int gResumeStrength;
	[HideInInspector]public int gMaxStrength;
	[HideInInspector]public int gCurrentStrength;
	[HideInInspector]public int gCurrentEvolutionDegree;
	[HideInInspector]public int gNextEvolutionDegree;
	[HideInInspector]public int gCurrentXiakeMaxDegree;
	[HideInInspector]public int gNextXiakeMaxDegree;
	[HideInInspector]public int gAddAttack;
	[HideInInspector]public int gAddDefence;
	[HideInInspector]public int gAddLife;
	[HideInInspector]public int gAddFillSpeed;
	[HideInInspector]public int gNextSkillID;
	[HideInInspector]public int gCurrentEvolutionXiakeID;
	[HideInInspector]public int gCurrentChoicePlaceID;
	[HideInInspector]public int gChuanGongXiakeID;
	[HideInInspector]public int gBeiChuanGongXiakeID;
	
	public struct RequireInfo
	{
		public int requiretType;
		public bool isHave;
	};
	private List<RequireInfo> evolutionPlaceRequireList = new List<RequireInfo>();
	private List<RequireInfo> evolutionXaikeRequireList = new List<RequireInfo>();
	private List<int> flushPlaceIDList = new List<int>();
	private List<int> reservePlaceIDList = new List<int>();
	
	void Awake()
	{

		
	}
	
	public void modifyEvolutionPlaceListValue(int key,bool isHave)
	{
		for (int i = 0; i < evolutionPlaceRequireList.Count; i++)
		{
			if (evolutionPlaceRequireList[i].requiretType == key)
			{
				RequireInfo requireInfo = new RequireInfo();
				requireInfo.requiretType = key;
				requireInfo.isHave = isHave;
				evolutionPlaceRequireList[i] = requireInfo;
			}
		}
	}
	
	public  List<RequireInfo> EvolutionPlaceRequireList
	{
		get { return evolutionPlaceRequireList; }
	}
	
	public List<RequireInfo> EvolutionXiakeRequireList
	{
		get { return evolutionXaikeRequireList; }
	}
	
	public List<int> FlushPlaceIdList
	{
		get { return flushPlaceIDList;}
	}
	
	public List<int> ReservePlaceIDList
	{
		get {return reservePlaceIDList;		}
	}

	
}
