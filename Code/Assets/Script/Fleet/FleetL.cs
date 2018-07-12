using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FleetL
{
	//-----------------------------------------
	public Vector3 Position
	{
		get 
		{
			if (lastFrameNum == Time.frameCount)
				return position;
			
			position = Vector3.zero;
			foreach (WarshipL ws in _mWarshipList)
			{
				position += ws.U3DGameObject.transform.position;
			}
			position /= _mWarshipList.Count;
			
			return position;
		}
	}
	//-----------------------------------------
	
	//-----------------------------------------
	public bool IsMoving
	{
		get { return isMoving; }
	}
	//-----------------------------------------
	
	public FleetL(long fleetID)
	{
		_fleetID = fleetID;
	}
	
	public void Initialze()
	{
		isMoving = false;
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
		UnityEngine.Profiling.Profiler.BeginSample("FleetL.Update");
		
		Dictionary<long, WarshipL> tempRemoveList = new Dictionary<long, WarshipL>();
		
		// Update and Check the death warship
		isMoving = true;
		foreach (WarshipL ws in _mWarshipList)
		{
			ws.Update ();
			
			if (ws._isDeath)
			{
				tempRemoveList.Add(ws._warshipID, ws);
			}
			
			isMoving = ws._isMoving;
		}
		
		// Destroy death warship
		foreach (WarshipL ws in tempRemoveList.Values)
		{
			RemoveWarship(ws);
		}
		tempRemoveList.Clear();
		
		// // Recalculate the position information
		// LocateFleetPosition();
		// CalculateFleetRect();
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
	public WarshipL CreateWarship(GirlData data, WarshipL.CreateCallback callback)
	{
		WarshipL ws = GetWarship(data.roleCardId);
		
		if (ws != null)
			return ws;
		
		ws = new WarshipL(data);
		ws.Initialize(delegate(WarshipL ws1) 
		{
			_mWarshipList.Add(ws);
			if (null != callback)
				callback(ws);
		});
		
		
		// LocateFleetPosition();
		// CalculateFleetRect();
		
		return ws;
	}
	
	public void RemoveWarship(WarshipL ws)
	{
		_mWarshipList.Remove(ws);
		
		if (null != ws)
			ws.Release();
		if (_mWarshipList.Count == 0)
		{
			_isDeath = true;
			return;
		}
		
		// LocateFleetPosition();
		// CalculateFleetRect();
	}
	
	public WarshipL GetWarship(GameObject go)
	{
		foreach (WarshipL ws in _mWarshipList)
		{
			if (ws.IsHasGameObject(go))
			{
				return ws;
			}
		}
		
		return null;
	}
	
	public WarshipL GetWarship(long warshipID)
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
	
	public void UpdateProperty()
	{}
	
	public void LocateFleetPosition()
	{
		if (_mWarshipList.Count == 0)
			return;
		
		// 2012.04.01 LiHaojie Calculate the center position of this fleet
		_theFleetFocusPosition = Vector3.zero;
		foreach (WarshipL warship in _mWarshipList)
		{
			if (!warship._isDeath)
			{
				_theFleetFocusPosition += warship.U3DGameObject.transform.position;
			}
		}
		_theFleetFocusPosition /= _mWarshipList.Count;
			
		_fleetRect3DCenterPosition = _theFleetFocusPosition;
		
		// Calculate the Actor fleet end warship position
		if (_isActorFleet)
		{
			_theFleetEndPosition = Vector3.one * 10000.0f;
			
			bool firstSet = true;
			foreach (WarshipL warship in _mWarshipList)
			{
				if (!warship._isDeath)
				{
					if (firstSet)
					{
						firstSet = false;
						
						_theFleetEndPosition.x = warship.U3DGameObject.transform.position.x;
					}
					else
					{
						// Get the min x
						if (_theFleetEndPosition.x > warship.U3DGameObject.transform.position.x)
							_theFleetEndPosition.x = warship.U3DGameObject.transform.position.x;
					}
				}
			}	
		}
		else // Other fleet
		{
			_theFleetEndPosition = Vector3.one * -10000.0f;
			
			
			bool firstSet = true;
			foreach (WarshipL warship in _mWarshipList)
			{
				if (!warship._isDeath)
				{
					if (firstSet)
					{
						firstSet = false;			
						_theFleetEndPosition.x = warship.U3DGameObject.transform.position.x;
					}
					else
					{
						// Get the max x
						if (_theFleetEndPosition.x < warship.U3DGameObject.transform.position.x)
							_theFleetEndPosition.x = warship.U3DGameObject.transform.position.x;
					}
				}
			}
		}
		
		// Calculate the Other fleet end warship position
	}
	
	private void CalculateFleetRect()
	{
		bool firstSet = true;
		foreach (WarshipL warship in _mWarshipList)
		{
			if (warship._isDeath)
				continue;
			
				if (firstSet)
				{
					firstSet = false;
						
					_fleetRect3D.xMin = warship.U3DGameObject.transform.position.x;
					_fleetRect3D.yMin = warship.U3DGameObject.transform.position.z;
					_fleetRect3D.xMax = warship.U3DGameObject.transform.position.x;
					_fleetRect3D.yMax = warship.U3DGameObject.transform.position.z;
				}
				else
				{						
					if (_fleetRect3D.xMin > warship.U3DGameObject.transform.position.x)
						_fleetRect3D.xMin = warship.U3DGameObject.transform.position.x;
					if (_fleetRect3D.yMin > warship.U3DGameObject.transform.position.z)
						_fleetRect3D.yMin = warship.U3DGameObject.transform.position.z;
						
					if (_fleetRect3D.xMax < warship.U3DGameObject.transform.position.x)
						_fleetRect3D.xMax = warship.U3DGameObject.transform.position.x;
					if (_fleetRect3D.yMax < warship.U3DGameObject.transform.position.z)
						_fleetRect3D.yMax = warship.U3DGameObject.transform.position.z;	
				}
		}
	}
	
	// The lineup, the warship order
	// Update the fleet logic data
	
	
	public long _fleetID;
	public bool _isActorFleet = false;
	public bool _isAttachFleet = false;
	
	public Vector3 _theFleetFocusPosition;
	public Vector3 _theFleetEndPosition;
	
	public Rect _fleetRect3D;
	public Vector3 _fleetRect3DCenterPosition;
	
	public bool _isDeath = false;
	
	public List<WarshipL> _mWarshipList = new List<WarshipL>();
	
	protected Vector3 position;
	private int lastFrameNum;
	private bool isMoving;
}
