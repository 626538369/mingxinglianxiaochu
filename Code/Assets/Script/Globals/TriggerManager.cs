using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerManager : MonoBehaviour 
{
	void Awake()
	{}
	
	void Start()
	{}
	
	public void Release()
	{
		_mBattleTriggerList.Clear();
	}
	
	public void AddBattleTrigger(BattleTrigger trigger)
	{
		_mBattleTriggerList.Add(trigger);
	}
	
	public void RemoveBattleTrigger(BattleTrigger trigger)
	{
		_mBattleTriggerList.Remove(trigger);
	}
	
	private List<BattleTrigger> _mBattleTriggerList = new List<BattleTrigger>();
}
