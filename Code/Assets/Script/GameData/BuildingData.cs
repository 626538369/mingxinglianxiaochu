using UnityEngine;
using System.Collections;

public enum EBuildingType
{
	TTRN = 1,
	DEFENCE_FACILITY,
	MATERIALS_CENTRE,
	DIRECTORATE,
	PLAYER_PORT,
	MARINE_BOARD,
	SHIPYARD,		// 造船厂 更名 设计局
	TECHNOLOGY,
	OIL_REFINERY,
	STATUE,
	BLACK_MARKET,
	
	JuXianLou = SHIPYARD,
	YanWuTang = TECHNOLOGY,
	GuanShiFang = 15,
	YuanBaoLou = 16,
}

public class BuildingData
{
	public int PortID;
	
	public int ID;
	public int LogicID;
	
	public EBuildingType Type;
	
	public int NameID;
	public string Name;
	public string BuildIcon;// port info ui
	
	public string NPCIcon;
	public int NPCNameID;
	public string NPCName;
	
	public int FlourishRequire;
	public int DefenseRequire;
	public int CampRequire;
	
	public Vector3 Position;
	public Quaternion Orientation;
	
	public string ModelName;
	public string TextureName;
	public string EffectName;
	
	public int FunctionID;
	public int BuffID;
	public int CampID;
	
	public string Remark1;
	public string Remark2;
}
