using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour 
{
	void Awake()
	{
		_mBuffList = new List<Buff>();
		_mRemoveBuffList = new List<Buff>();
	}
	
	void OnDisable()
	{
		Cleanup();
	}
	
	void OnDestroy()
	{
		Cleanup();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_mRemoveBuffList.Count > 0)
		{
			foreach (Buff buff in _mRemoveBuffList)
			{
				_mBuffList.Remove(buff);
			}
			_mRemoveBuffList.Clear();
		}
	}
	
	public void CreateBuff(int buffID)
	{
		Buff buff = new Buff();
		buff.Init();
		
		_mBuffList.Add(buff);
	}
	
	public void DestroyBuff(Buff buff)
	{
		buff.UnInit();
		_mRemoveBuffList.Add(buff);
	}
	
	private void Cleanup()
	{
		foreach (Buff buff in _mBuffList)
		{
			DestroyBuff(buff);
		}
		_mBuffList.Clear();
	}
	
	private List<Buff> _mBuffList;
	private List<Buff> _mRemoveBuffList;
}
