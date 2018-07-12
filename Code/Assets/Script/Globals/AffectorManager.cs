using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AffectorType
{
	DELAY_FLY_UP,
	DELAY_FADE,
}

public class AffectorManager : MonoBehaviour 
{
	void Awake()
	{
		_mFreeAffectorList = new Dictionary<AffectorType, Affector>();
	}
	
	void Start()
	{}
	
	void OnDisable()
	{
		_mFreeAffectorList.Clear();
	}
	
	void OnDestroy()
	{
		_mFreeAffectorList.Clear();
	}
	
	public Affector GetAffector(AffectorType type)
	{
		// See if we have a list of this type:
		if (_mFreeAffectorList.ContainsKey(type))
		{
			return _mFreeAffectorList[type];
		}
		
		return CreateNewAffector(type);
	}
	
	private Affector CreateNewAffector(AffectorType type)
	{
		GameObject go = new GameObject();
		go.name = "Affector_" + (int)type;
		Affector affector = AddAffectorComp(go, type);
		
		_mFreeAffectorList.Add(type, affector);
		return affector;
	}
	
	private Affector AddAffectorComp(GameObject go, AffectorType type)
	{
		Affector affector = null;
		switch(type)
		{
		case AffectorType.DELAY_FLY_UP:
			affector = go.AddComponent<AffectorDelayFlyUp>() as AffectorDelayFlyUp;
			break;
		case AffectorType.DELAY_FADE:
			affector = go.AddComponent<AffectorDelayFade>() as AffectorDelayFade;
			break;
		}
		
		return affector;
	}
	
	private Dictionary<AffectorType, Affector> _mFreeAffectorList;
}
