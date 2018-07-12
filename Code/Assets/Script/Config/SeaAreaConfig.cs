using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
/// <summary>
/// Sea area config.
/// </summary>
public class SeaAreaConfig : ConfigBase
{
	public class SeaAreaObject
	{
		public int SeaAreaID
		{
			get{ return _seaAreaID; }
			set{ _seaAreaID = value;}
		}
		
		public int SeaAreaNameID
		{
			get{ return _seaAreaNameID; }
			set{ _seaAreaNameID = value;}
		}
		
		public int PortNameID
		{
			get{ return _portNameID; }
			set{ _portNameID = value;}
		}
		
		public List<int> CopyIdList
		{
			get{ return _copyIdList; }
			set{ _copyIdList = value;}
		}
		
		public int FlourishValue
		{
			get{ return _flourishValue; }
			set{ _flourishValue = value;}
		}
		
		public int FlourishLevel
		{
			get{ return _flourishLevel; }
			set{ _flourishLevel = value;}
		}
		
		public int DefenseValue
		{
			get{ return _defenseValue; }
			set{ _defenseValue = value;}
		}
		
		public int DefenseLevel
		{
			get{ return _defenseLevel; }
			set{ _defenseLevel = value;}
		}
		
		public string SceneName
		{
			get{ return _portSceneName; }
			set{ _portSceneName = value;}
		}
		
		public string PortPosition
		{
			get{ return _portPosition;}
			set{ _portPosition = value;}
		}
			
		public int CountryID
		{
			get{ return _countryID; }
			set{ _countryID = value;}
		}
		
		public string CountryFlagIcon
		{
			get{ return _countryFlagIcon; }
			set{ _countryFlagIcon = value;}
		}
		
		public List<int> BuildingIdList
		{
			get{ return _buildingIdList; }
			set{ _buildingIdList = value;}
		}
		
		public int CampID
		{
			get{ return _campID; }
			set{ _campID = value;}
		}
		
		public int CampChange
		{
			get{ return _campChange; }
			set{ _campChange = value;}
		}
		
		public int AlliesReputation
		{
			get{ return _alliesReputation; }
			set{ _alliesReputation = value;}
		}
		
		public int AxisReputation
		{
			get{ return _axisReputation; }
			set{ _axisReputation = value;}
		}
		
		public int NeutralReputation
		{
			get{ return _neutralReputation; }
			set{ _neutralReputation = value;}
		}
		
		public int WinnerPlayerNameID
		{
			get{ return _winnerPlayerNameID; }
			set{ _winnerPlayerNameID = value;}
		}
		
		public int WinnerTime
		{
			get{ return _winnerTime; }
			set{ _winnerTime = value;}
		}
		
		public int LevelRequire
		{
			get{ return _levelRequire; }
			set{ _levelRequire = value;}
		}
		
		public int FeatRequire
		{
			get{ return _featRequire; }
			set{ _featRequire = value;}
		}
		
		public int TaskRequire
		{
			get{ return _taskRequire; }
			set{ _taskRequire = value;}
		}
		
		public int CampRequire
		{
			get{ return _campRequire; }
			set{ _campRequire = value;}
		}
		
		public static SeaAreaObject Load(SecurityElement element)
		{
			SeaAreaObject seaAreaObject = new SeaAreaObject();
			
			seaAreaObject.SeaAreaID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Sea_ID"),""),-1);
			seaAreaObject.SeaAreaNameID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Sea_Name"),""),-1);
			seaAreaObject.PortNameID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Port_Name"),""),-1);
			seaAreaObject.CopyIdList = StrParser.ParseDecIntList(StrParser.ParseStr(element.Attribute("Copy_List"),""),-1);
			seaAreaObject.FlourishValue = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("flourish_Value"),""),-1);
			seaAreaObject.FlourishLevel = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("flourish_Level"),""),-1);
			seaAreaObject.DefenseValue = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Defense_Value"),""),-1);
			seaAreaObject.DefenseLevel = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Defense_Level"),""),-1);
			seaAreaObject.SceneName = StrParser.ParseStr(element.Attribute("Port_Map"),"");
			seaAreaObject.PortPosition = StrParser.ParseStr(element.Attribute("Port_Pos"), "");
//			seaAreaObject.SeaAreaPosition = StrParser.ParseStr(StrParser.ParseStr(element.Attribute("Sea_Pos"), ""),"");
			seaAreaObject.CountryID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Country_ID"),""),-1);
			seaAreaObject.CountryFlagIcon = element.Attribute("Country_Flag_Icon");
				
			seaAreaObject.BuildingIdList = StrParser.ParseDecIntList(StrParser.ParseStr(element.Attribute("Build_ID"),""),-1);
			seaAreaObject.CampID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Camp_ID"),""),-1);
			seaAreaObject.CampChange = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Camp_Change"),""),-1);
			seaAreaObject.AlliesReputation = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Allies_Reputation"),""),-1);
			seaAreaObject.AxisReputation = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Axis_Reputation"),""),-1);
			seaAreaObject.NeutralReputation = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Neutral_Reputation"),""),-1);
			seaAreaObject.WinnerPlayerNameID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Winner_Player_Name"),""),-1);
			seaAreaObject.WinnerTime = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Winner_Time"),""),-1);
			seaAreaObject.LevelRequire = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Level_Require"),""),-1);
			seaAreaObject.FeatRequire = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Feat_Require"),""),-1);
			seaAreaObject.TaskRequire = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Require"),""),-1);
			seaAreaObject.CampRequire = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Camp_Require"),""),-1);
			seaAreaObject.BlueprintSort	= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("BluePrints"),""),0);
			
			return seaAreaObject;
		}
		
		protected int _seaAreaID;
		protected int _seaAreaNameID;
		protected int _portNameID;
		protected List<int> _copyIdList;
		protected int _flourishValue;
		protected int _flourishLevel;
		protected int _defenseValue;
		protected int _defenseLevel;
		protected string _portSceneName;
		protected string _portPosition;
		protected string _seaAreaPosition;
		protected int _countryID;
		protected string _countryFlagIcon;
		protected List<int> _buildingIdList;
		protected int _campID;
		protected int _campChange;
		protected int _alliesReputation;
		protected int _axisReputation;
		protected int _neutralReputation;
		protected int _winnerPlayerNameID;
		protected int _winnerTime;
		protected int _levelRequire;
		protected int _featRequire;
		protected int _taskRequire;
		protected int _campRequire;
		
		public int BlueprintSort;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
		{
			return false;
		}
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if(childrenElement.Tag == "Item")
				{
					SeaAreaObject seaAreaObject = SeaAreaObject.Load(childrenElement);
					
					if(!_seaAreaObjectDict.ContainsKey(seaAreaObject.SeaAreaID))
					{
						_seaAreaObjectDict[seaAreaObject.SeaAreaID] = seaAreaObject;
					}
				}
			}  
			return true;
		}
		else      
		{
			return false;
		}
	}
	
	/// <summary>
	/// Gets the sea area which has designer building.
	/// </summary>
	/// <returns>
	/// The sea area has designer building.
	/// </returns>
	/// <param name='campID'>
	/// Camp ID player's camp id
	/// </param>
	public List<SeaAreaObject> GetSeaAreaHasDesignerBuilding(int campID){
		
		List<SeaAreaObject> tList = new List<SeaAreaObject>();
		foreach(SeaAreaObject sao in _seaAreaObjectDict.Values){
			if(sao.BlueprintSort != 0 // has designer
			&& (sao.CampID == 3 /* Neutral */|| sao.CampID == campID)){
				
				bool inserted = false;
				for(int i = 0;i < tList.Count;i++){
					if(tList[i].BlueprintSort < sao.BlueprintSort){
						
						tList.Insert(i,sao);
						
						inserted = true;						
						break;
					}					
				}
				
				if(!inserted){
					tList.Add(sao);	
				}				
			}
		}
		
		return tList;
	}
	
	public bool GetSeaAreaElement(int seaID, out SeaAreaObject element)
	{
		element = null;
		if ( !_seaAreaObjectDict.TryGetValue(seaID, out element) )
		{
			return false;
		}
		
		return true;
	}
	public Dictionary<int,SeaAreaConfig.SeaAreaObject> GetSeaAreaMap()
	{
		return _seaAreaObjectDict;
	}
	
	protected Dictionary<int,SeaAreaObject> _seaAreaObjectDict = new Dictionary<int, SeaAreaObject>();
}
