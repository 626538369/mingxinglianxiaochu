using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FleetData
{
	public int FleetID;
	public string FleetName;
	
	public string FleetAvatar;
	
	public int FleetHeaderID;
	
	public int FleetCountryID;
	public int FleetCampID;
	public int FleetFactionID;
	
	public int StrengthList;
	public int FeatList;
	public int Strength;
	public int Feat;
	
	public int Rank;
	
	public int CurrentWarshipCount;
	public int MaxWarshipCount;
	public List<int> WarshipIDList = new List<int>();
	
	public int CurrentFormationID;
	public List<int> FormationIDList = new List<int>();
	
	public int TacticsID;
	public List<int> TacticsIDList = new List<int>();
}