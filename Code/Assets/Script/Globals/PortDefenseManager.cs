using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortDefenseManager : MonoBehaviour 
{	
	[HideInInspector]public bool puDefenseOn;
	[HideInInspector]public int puDefenseInspire;
	[HideInInspector]public List<sg.BI_PortDefense_Team_Mes> puListTeams = new List<sg.BI_PortDefense_Team_Mes>();
	[HideInInspector]public List<sg.BI_PortDefense_Team_Member_Mes> puListMembers = new List<sg.BI_PortDefense_Team_Member_Mes>();
	[HideInInspector]public int puJoinTeamID;
	[HideInInspector]public int puJoinWantedTeamID;
	[HideInInspector]public bool puShowPK;
	[HideInInspector]public int puWaveIndex;
	[HideInInspector]public int puBattleIndex;
	[HideInInspector]public bool puIsDefenseBattlingOn;
	[HideInInspector]public bool puIsShowWaveResult;
	[HideInInspector]public bool puIsShowFinalWaveResult;

	
	public enum ETeamState
	{
		AllowJoin = 0,
		TeamFull,
		NotAllowJoin,
		AlreadyJoin,
		TeamDeleted
	}
	
	public enum EPageTurnType
	{
		Backward = 1,
		Forward = 2,
	}
	
	void Awake()
	{
		puDefenseOn = false;
	 	puJoinTeamID = -1;
		puJoinWantedTeamID = -1;
		puShowPK = false;
		puIsDefenseBattlingOn = false;
		
		//isPortDefenseOn = false;
		//isCloseDefenseUI = false;
		//isBreakRequestPortDefense = false;
	}
	
	public void UpdateInspireValue()
	{
		//GUIPortDefense guiPortDef = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
		//if(guiPortDef != null)
		//{
		//	guiPortDef.UpdateInspireText();
		//}
	}
	
	public void UpdateTeams()
	{
		//GUIPortDefense portDefense = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
		//if(portDefense != null)
		//{
		//	portDefense.UpdateTeamData();
		//}
	}
	
	public void UpdateTeamMembers()
	{
		//GUIPortDefense portDefense = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
		//if(portDefense != null)
		//{
		//	portDefense.UpdateJoinTeamData();
		//}
	}
	
	public void BeginDefenseBattle()
	{
		//GUIPortDefense gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
		//if(gui != null)
		//{
		//	GUIRadarScan.Hide();
		//	puShowPK = true;
		//	gui.SetVisible(false);
		//}
		
		puWaveIndex = 0;
		puBattleIndex = 0;
		puIsDefenseBattlingOn = true;
		
		FillBattleData();
		GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_BATTLE);
	}
	
	void FillBattleData()
	{
	
	}
	
	public bool UpdateDefenseBattle()
	{
		return true;	
	}
	
	public void EndDefenseBattle()
	{
		puJoinTeamID = -1;
		puJoinWantedTeamID = -1;
		
		puShowPK = false;
		puIsDefenseBattlingOn = false;
		puIsShowWaveResult = false;
		puIsShowFinalWaveResult = false;
		//update panel data
		if(!puDefenseOn)
		{
			GUIBattle t = Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>();
			if(null != t)
				t.Close();
			//GUIPortDefense t1 = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
			//if(null != t)
			//	t1.Close();
		}
		else
		{			
			int tIndex = 0;
			if(puListTeams != null && puListTeams.Count > 0)
				tIndex = puListTeams[0].teamID;
		}
	}
	
	
	
		
	
	/*
	void Update()
	{
		//Debug.Log("Update");
	}
	
	// ---old-------------------------------
	public int CurrBattleWaveID
	{
		get{ return _currBattleWaveID; }
		set{ _currBattleWaveID = value;}
	}
	
	public bool IsDefBattleInfoUpdate
	{
		get{ return _isDefBattleInfoUpdate; }
		set{ _isDefBattleInfoUpdate = value;}
	}
	
	public bool IsDefBattleDataUpdate
	{
		get{ return _isDefBattleDataUpdate; }
		set{ _isDefBattleDataUpdate = value;}
	}
	
	public bool IsShowPKUI
	{
		set{ _isShowPKUI = value;}
		get{ return _isShowPKUI; }
	}
	
	public enum ETeamActionState
	{
		Team_Create,
		Team_Insert,
		Team_Insert_With_Exit,
		None,
		Team_Exit,
		Team_Remove_Ohter
	}
	
	

	//single port defense team
	public class SinglePortDefenseTeam
	{
		public class TeamMemberInfo
		{   
			public int id;
			public int RoleID;
			public int RoleLevel;
			public string AdmiralName;
			public string AdmiralAvatar;
			public bool IsLeader;
		}
		
		public enum ETeamStatus
		{
			ALLOW_JOIN,
			TEAM_FULL,
			NOT_ALLOW_JOIN,
			IS_IN_TEAM,
			TEAM_REMOVE
		}
		public int TeamID;
		public int PortID;
		public string PortName;
		public int FortDegree;
		public int AttackAddition;
		public int TeamMemberCount;
		public ETeamStatus TeamStatus;
		public List<TeamMemberInfo> listTeamMemberInfo = new List<TeamMemberInfo>();
	}
	
	//port which curr player can access data
	public class PortAccessData
	{
		public int PortID;
		public string PortName;
		public int Flourish;
	}
	
	public string TeamHeaderName(SinglePortDefenseTeam singleTeam)
	{
		foreach(SinglePortDefenseTeam.TeamMemberInfo memberInfo in singleTeam.listTeamMemberInfo)
		{
			if(memberInfo.IsLeader)
			{
				return memberInfo.AdmiralName + "\n(LV"+ memberInfo.RoleLevel + ")";
			}
		}
		return string.Empty;
	}
	
	public bool isTeamHeader(SinglePortDefenseTeam singleTeam)
	{	
		foreach(SinglePortDefenseTeam.TeamMemberInfo memberInfo in singleTeam.listTeamMemberInfo)
		{
			if(memberInfo.IsLeader && memberInfo.RoleID == Globals.Instance.MGameDataManager.MActorData.PlayerID)
			{
				return true;
			}
		}
		return false;
	}
	
	public bool isTeamHeader()
	{
		if(!dictDefenseList.ContainsKey(joinTeamID)) return false;
		SinglePortDefenseTeam singleTeam = dictDefenseList[joinTeamID];
		foreach(SinglePortDefenseTeam.TeamMemberInfo info in singleTeam.listTeamMemberInfo)
		{
			if(info.RoleID == Globals.Instance.MGameDataManager.MActorData.PlayerID && info.IsLeader)
			{
				return true;
			}
		}
		return false;
	}
	
	
	public string TeamStatus(SinglePortDefenseTeam.ETeamStatus status)
	{
		switch(status)
		{
		case SinglePortDefenseTeam.ETeamStatus.ALLOW_JOIN:
			return "";
			break;
		case SinglePortDefenseTeam.ETeamStatus.TEAM_FULL:
			return GUIFontColor.NewColor8H + Globals.Instance.MDataTableManager.GetWordText(23400007);
			break;
		case SinglePortDefenseTeam.ETeamStatus.NOT_ALLOW_JOIN:
			return GUIFontColor.NewColor8H + Globals.Instance.MDataTableManager.GetWordText(23400008);
			break;
		}
		return string.Empty;
	}
	
	public int GetTeamMemberRoleID(int teamID,int id)
	{
		if(!dictDefenseList.ContainsKey(teamID)) return -1;
		SinglePortDefenseTeam singleTeam = dictDefenseList[teamID];
		foreach(SinglePortDefenseTeam.TeamMemberInfo info in singleTeam.listTeamMemberInfo)
		{
			if(info.id == id)
			{
				return info.RoleID;
			}
		}
		return -1;
	}
	
	public int GetMemberID(int teamID)
	{
		if(!dictDefenseList.ContainsKey(teamID)) return -1;
		SinglePortDefenseTeam singleTeam = dictDefenseList[teamID];
		foreach(SinglePortDefenseTeam.TeamMemberInfo info in singleTeam.listTeamMemberInfo)
		{
			if(info.RoleID == Globals.Instance.MGameDataManager.MActorData.PlayerID)
			{
				return info.id;
			}
		}
		return -1;
	}
	
	public bool IsInCertenTeam(int teamID)
	{
		if(!dictDefenseList.ContainsKey(teamID)) return false;
		SinglePortDefenseTeam singleTeam = dictDefenseList[teamID];
		foreach(SinglePortDefenseTeam.TeamMemberInfo info in singleTeam.listTeamMemberInfo)
		{
			if(info.RoleID == Globals.Instance.MGameDataManager.MActorData.PlayerID)
				return true;
		}
		return false;
	}
	
	public bool IsCurrTeamFull(int teamID)
	{
		if(!dictDefenseList.ContainsKey(teamID)) return false;
		SinglePortDefenseTeam singleTeam = dictDefenseList[teamID];
		if(singleTeam.TeamMemberCount <3)
			return false;
		else 
			return true;
	}
	
	
	
	public void ShowPortsListDialog()
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIPortDefList>(delegate(GUIPortDefList gui){gui.SetPlaneVisible(false);});
	}
	
	public void UpdatePortDefenseWindow()
	{
		if(mGUIPortDefense == null)
		{
			Globals.Instance.MGUIManager.CreateWindow<GUIPortDefense>(delegate(GUIPortDefense gui){
				NetSender.Instance.RequestPortDefList();
				Globals.Instance.MPortDefenseManager.currPageFirstTeamID = 1;
				gui.UpdateTeamData();
			});
		}
		else
		{
			//  only notify view update
			mGUIPortDefense.UpdatePageTeamData( PortDefenseManager.EPageTurnType.Forward);
		}
	}
	//----------------------------request battle info
	public void RequestBattleInfo()
	{
		IsDefBattleDataUpdate = false;
		IsDefBattleInfoUpdate = false;
		NetSender.Instance.RequestPortDefBattle(Globals.Instance.MPortDefenseManager.CurrBattleWaveID,Globals.Instance.MPortDefenseManager.joinTeamID);
	}
	
	//decided whether can play the battle animation
	public void PlayDefBattleAnimation()
	{
		if(IsDefBattleDataUpdate && IsDefBattleInfoUpdate)
		{
			IsDefBattleDataUpdate = false;
			IsDefBattleInfoUpdate = false;
			// play the battle animation
			GUIPortDefense gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIPortDefense>();
			if(gui != null)
			{
				GUIRadarScan.Hide();
				// show the Pk infomation
				_isShowPKUI = true;
				mGUIPortDefense.Close();
			}
			
			GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_BATTLE);
		}
	}
	
	//show the battle wave result
	public void ShowBattleWaveResult()
	{
		Globals.Instance.MGUIManager.GetGUIWindow<GUIBattle>().UpdateWaveResultDialogData();
	}
	
	public string AnalysisBattleStatus(sg.Battle_Status status)
	{
		int tTextID = 0;
		switch(status)
		{
		case sg.Battle_Status.BATTLE_DEATH:
			tTextID = 23400028;
			break;
		case sg.Battle_Status.BATTLE_FIGHTING:
			tTextID = 23400027;
			break;
		case sg.Battle_Status.BATTLE_LIVE:
			tTextID = 23400029;
			break;
		case sg.Battle_Status.BATTLE_WAITTING:
			tTextID = 23400026;
			break;
		}
		
		return Globals.Instance.MDataTableManager.GetWordText(tTextID);
	}
	
	//----------------port defense--------------------------
	public bool isCloseDefenseUI;
	public GUIPortDefense mGUIPortDefense;
	public SortedDictionary<int,SinglePortDefenseTeam> dictDefenseList = new SortedDictionary<int, SinglePortDefenseTeam>();
	public List<PortAccessData> listPortAccessData = new List<PortAccessData>();
	
	public bool isBreakRequestPortDefense;
	public bool isPortDefenseOn; // whether the activity port defense is on
	public int currPageFirstTeamID;//the curr page teamID
	public int joinTeamID; // curr player joined team id;
	public int wantJoinTeamID;
	public int JoinTeamMemberID; // centern team member id
	public int SelfCreatePortID;// the port id which curr player created
	public ETeamActionState TeamActionState;
	private float updateTeamListDataInterval = 5;
	public int portDefenseInspire = 0;
	public int isDefenseBattleStart; // battle defense whether start
	private int _currBattleWaveID = 1;
	private bool _isDefBattleInfoUpdate = false;
	private bool _isDefBattleDataUpdate = false;
	private bool _isShowPKUI = false;
	public sg.GS2C_Port_Defense_Battle_Res defenseBattle;
	*/
}
