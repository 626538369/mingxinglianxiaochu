using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffInfluence
{
	public int ID;
	public int Value;
	
	public float ValueFactor;
}

public class BuffData 
{
	public int ID;
	public int Type;
	
	public int NameID;
	public string Name;
	
	public int DescriptID;
	public string Descript;
	
	public string Icon;
	
	public string PersistEffectName;
	public string ImmediateEffectName;
	
	public float PersistTime;
	public int PersistRound;
	
	public bool IsOfflineRemove;
	public bool IsOfflineFreeze;
	public bool IsCoexistence;
	
	public BuffInfluence Influence1 = new BuffInfluence();
	public BuffInfluence Influence2 = new BuffInfluence();
	
	public void FillDataFromConfig()
	{
	
	}
}
