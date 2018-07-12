using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FleetProperty : MonoBehaviour 
{
	void Awake()
	{
		FleetID = -1;
		FleetName = null;
		IsNpcFleet = true;
	}
	
	public int FleetID;
	public string FleetName;
	public bool IsNpcFleet;
	// public List<WarshipL> WarshipList = new List<WarshipL>();
}
