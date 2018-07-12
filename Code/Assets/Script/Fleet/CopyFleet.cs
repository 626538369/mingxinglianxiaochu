using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyFleet
{
	public CopyFleet(int fleetID)
	{
		_mID = fleetID;
	}
	
	public void Initialze()
	{
	}
	
	public void Release()
	{
		foreach (WarshipL ws in _mWarshipList)
		{
			ws.Release();
		}
		_mWarshipList.Clear();
	}
	
	public void Update()
	{
		// Update and Check the death warship
		foreach (WarshipL ws in _mWarshipList)
		{
			ws.Update ();
			
			if (ws._isDeath)
			{
				_mWarshipTempList.Add(ws);
			}
		}
		
		// Destroy death warship
		foreach (WarshipL ws in _mWarshipTempList)
		{
			RemoveWarship(ws);
		}
		_mWarshipTempList.Clear();
	}
	
	// public WarshipL CreateWarship(GirlData data)
	// {
	// 	WarshipL ws = GetWarship(data.WarShipID);
	// 	
	// 	if (ws != null)
	// 		return ws;
	// 	
	// 	ws = new WarshipL(data);
	// 	ws.Initialize();
	// 	
	// 	_mWarshipList.Add(data.WarShipID, ws);
	// 	
	// 	return ws;
	// }
	
	public void RemoveWarship(WarshipL ws)
	{
		_mWarshipList.Remove(ws);
		
		if (null != ws)
			ws.Release();
		if (_mWarshipList.Count == 0)
		{
			_mIsDeath = true;
			return;
		}
	}
	
	public WarshipL GetWarship(GameObject go)
	{
		foreach (WarshipL ws in _mWarshipList)
		{
			if (ws.U3DGameObject == go)
			{
				return ws;
			}
		}
		
		return null;
	}
	
	public WarshipL GetWarship(int warshipID)
	{
		
		foreach(WarshipL s in _mWarshipList){
			if(s._warshipID == warshipID){
				return s;
			}
		}
		
		return null;
	}
	
	public void SetActive(bool visible)
	{
		foreach (WarshipL warship in _mWarshipList)
		{
			warship.SetActive(visible);
		}
	}
	
	public void Stop()
	{
		foreach (WarshipL warship in _mWarshipList)
		{
			warship.Stop();
		}
	}
	
	private int _mID;
	private FleetData _mFleetData;
	
	private bool _mIsActor;
	private bool _mIsAttacker;
	
	private bool _mIsDeath;
	private List<WarshipL> _mWarshipList = new List<WarshipL>();
	private List<WarshipL> _mWarshipTempList = new List<WarshipL>();
}
