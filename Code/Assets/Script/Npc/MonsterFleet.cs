using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterFleet
{
	//-----------------------------------------
	public Vector3 Position
	{
		get 
		{
			if (lastFrameNum == Time.frameCount)
				return position;
			
			position = Vector3.zero;
			foreach (MonsterWarship ws in _monsterWarshipList)
			{
				position += ws.U3DGameObject.transform.position;
			}
			position /= _monsterWarshipList.Count;
			
			return position;
		}
	}
	//-----------------------------------------
	
	public MonsterFleet(int fleetID)
	{
		_fleetID = fleetID;
	}
	
	public void Initialize()
	{
	}
	
	public void Release()
	{
		foreach (MonsterWarship ws in _monsterWarshipList)
		{
			ws.Release();
		}
		_monsterWarshipList.Clear();
	}
	
	public MonsterWarship CreateMonsterWarship(CopyMonsterData.MonsterData data, WarshipL.CreateCallback callback)
	{
		MonsterWarship monster = new MonsterWarship(data);
		monster.Initialize( delegate(WarshipL ws) 
		{
			_monsterWarshipList.Add(monster);
			if (null != callback)
				callback(ws);
		});
		
		return monster;
	}
	
	public void RemoveMonsterWarship(MonsterWarship monster)
	{
		if (monster == null)
			return;
		
		monster.Release();
		_monsterWarshipList.Remove(monster);
	}
	
	public MonsterWarship GetFirstWarship()
	{
		foreach (MonsterWarship monster in _monsterWarshipList)
		{
			return monster;
		}
		
		return null;
	}
	
	public void SetActive(bool visible)
	{
		foreach (MonsterWarship monster in _monsterWarshipList)
		{
			monster.SetActive(visible);
			
			if (visible)
			{
				monster.StartPathPatrolAI();
			}
		}
	}
	
	public void Stop()
	{
		foreach (MonsterWarship monster in _monsterWarshipList)
		{
			monster.Stop();
		}
	}
	
	public void Update()
	{
		lastFrameNum = Time.frameCount;
		
		if (_monsterWarshipList.Count == 0)
			return;
		
		foreach (MonsterWarship monster in _monsterWarshipList)
		{
			monster.Update();
		}
	}
	
	public int _fleetID;
	public List<MonsterWarship> _monsterWarshipList = new List<MonsterWarship>();
	protected Vector3 position;
	private int lastFrameNum;
	
	//! CopyStatus using battle trigger GameObject
	public GameObject	m_battleTrigger = null;
	public GameObject	m_copyStatusGameObject = null;
}
