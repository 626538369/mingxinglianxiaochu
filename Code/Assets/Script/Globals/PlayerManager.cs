using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 2012.04.02 LiHaojie Add PlayerManager controller
public class PlayerManager : MonoBehaviour
{
	void Start()
	{
		Initialize();
	}
	
	void Dispose()
	{
		Release();
	}
	
//	// tzz modified
//	// replace the Update with FixedUpdate to avoid shake with camera (frame_move before the camera moving)
//	void FixedUpdate()
//	{
//		UpdateFleet();
//		UpdateMonsterFleet();
//	}
	
	protected void Initialize()
	{
	}
	
	protected void Release()
	{
		RemoveAllFleet();
		RemoveAllMonsterFleet();
	}
	
	#region General Fleet
	public FleetL GetFleet(long fleetID)
	{
		FleetL fleet = null;
		_fleetList.TryGetValue(fleetID, out fleet);
		
		// Debug.Log ("Find the fleet, id = " + fleetID.ToString());
		return fleet;
	}
	
	// An useful global assistant method
	public WarshipL GetWarship(long warshipID)
	{
		WarshipL ws = null;
		
		foreach (FleetL fleet in _fleetList.Values)
		{
			ws = fleet.GetWarship(warshipID);
			
			if (ws != null)
				return ws;
		}
		
		return null;
	}
	
	public WarshipL GetWarship(GameObject go)
	{
		WarshipL ws = null;
		
		foreach (FleetL fleet in _fleetList.Values)
		{
			ws = fleet.GetWarship(go);
			if (ws != null)
				return ws;
		}
		
		return null;
	}
	
	public FleetL CreateFleet(long fleetID)
	{
		FleetL fleet = GetFleet(fleetID);
		
		if (fleet != null)
			return fleet;
		
		fleet = new FleetL(fleetID);
		fleet.Initialze();
		
		_fleetList.Add(fleetID, fleet);
		return fleet;
	}
	
	public void RemoveFleet(FleetL fleet)
	{
		fleet.Release();
		_fleetList.Remove(fleet._fleetID);
	}
	
	public void RemoveAllFleet()
	{
		foreach (FleetL fleet in _fleetList.Values)
		{
			fleet.Release();
		}
		_fleetList.Clear();
	}
	
	public void UpdateFleet()
	{
		UnityEngine.Profiling.Profiler.BeginSample("PlayerManager.UpdateFleet");
		
		Dictionary<long, FleetL> tempRemoveList = new Dictionary<long, FleetL>();
		foreach (FleetL fleet in _fleetList.Values)
		{	
			fleet.Update();
				
			// If the fleet dead, clean relative data
			if (fleet._isDeath)
			{
				tempRemoveList.Add(fleet._fleetID, fleet);
			}
		}
		
		foreach (FleetL fleet in tempRemoveList.Values)
		{
			RemoveFleet(fleet);
		}
		tempRemoveList.Clear();
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	#endregion
	
	#region Monster
	public MonsterFleet CreateMonsterFleet(int fleetID)
	{
		MonsterFleet monster = new MonsterFleet(fleetID);
		monster.Initialize();
		
		_monsterFleetList.Add(monster);
		
		return monster;
	}
	
	public void RemoveMonsterFleet(MonsterFleet monster)
	{
		if (monster == null)
			return;
		
		monster.Release();
		_monsterFleetList.Remove(monster);
	}
	
	public void RemoveAllMonsterFleet()
	{
		foreach (MonsterFleet fleet in _monsterFleetList)
		{
			fleet.Release();
		}
		_monsterFleetList.Clear();
	}
	
	public void UpdateMonsterFleet()
	{}
	#endregion

	public Dictionary<long, FleetL> _fleetList = new Dictionary<long, FleetL>();
	
	// Specifc, do not allocate a id value
	private List<MonsterFleet> _monsterFleetList = new List<MonsterFleet>();
}
