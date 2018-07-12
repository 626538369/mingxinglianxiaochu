using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buff 
{
	public void Init()
	{
		_mBuffDataList = new List<BuffData>();
		_mBuffEffectList = new List<GameObject>();
	}
	
	public void UnInit()
	{
		_mBuffDataList.Clear();
		
		foreach (GameObject go in _mBuffEffectList)
		{
			GameObject.DestroyImmediate(go);
		}
		_mBuffEffectList.Clear();
	}
	
	private List<BuffData> _mBuffDataList;
	private List<GameObject> _mBuffEffectList;
}
