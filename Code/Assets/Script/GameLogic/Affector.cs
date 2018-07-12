using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Affector : MonoBehaviour
{
	public delegate void AffectorDelegate(Affector affector);
	
	void Awake()
	{
		_mGameObjectList = new List<GameObject>();
	}
	
	void OnDestroy()
	{
		_mGameObjectList.Clear();
	}
	
	public void Clear()
	{
		_mGameObjectList.Clear();
	}
	
	public void AddItem(GameObject go)
	{
		_mGameObjectList.Add(go);
	}
	
	// public void RemoveItem(GameObject go)
	// {
	// 	_mGameObjectList.Remove(go);
	// }
	
	public virtual void DoAffect(AffectorDelegate startDel, AffectorDelegate endDel, params object[] args)
	{
		_mStartDelegate = startDel;
		_mEndDelegate = endDel;
	}
	
	protected List<GameObject> _mGameObjectList;
	
	protected AffectorDelegate _mStartDelegate;
	protected AffectorDelegate _mEndDelegate;
}

public class AffectorDelayFlyUp : Affector
{
	void Start()
	{
		_mAnimEndCounter = 0;
	}
	
	public override void DoAffect(AffectorDelegate startDel, AffectorDelegate endDel, params object[] args)
	{
		base.DoAffect(startDel, endDel, args);
		
		_mAnimEndCounter = 0;
		_mDestPosition = (Vector3)args[0];
		_mScale = (Vector3)args[1];
		
		if (_mGameObjectList.Count != 4)
			return;
		foreach (GameObject go in _mGameObjectList)
		{
			go.SetActiveRecursively(false);
		}
		
		StartCoroutine(DoCoroutineAffector());
	}
	
	IEnumerator DoCoroutineAffector()
	{
		_mGameObjectList[0].SetActiveRecursively(true);
		yield return new WaitForSeconds(0.3f);
		
		_mGameObjectList[1].SetActiveRecursively(true);
		yield return new WaitForSeconds(0.3f);
		
		_mGameObjectList[2].SetActiveRecursively(true);
		_mGameObjectList[3].SetActiveRecursively(true);
		yield return new WaitForSeconds(0.3f);
		
		foreach (GameObject go in _mGameObjectList)
		{
			AnimatePosition.Do(go, EZAnimation.ANIM_MODE.To, _mDestPosition, 
				EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear), 1.0f, 0, 
				EZAnimationStart, EZAnimationEnd);
			
			AnimateScale.Do(go, EZAnimation.ANIM_MODE.To, _mScale, 
				EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear), 1.0f, 0, 
				EZAnimationStart, EZAnimationEnd);
		}
	}
	
	public void EZAnimationStart(EZAnimation anim)
	{
		// if (anim.GetType() == typeof(AnimatePosition))
		// {}
		// else if (anim.GetType() == typeof(AnimateScale))
		// {}
	}
	
	public void EZAnimationEnd(EZAnimation anim)
	{
		_mAnimEndCounter++;
		if (_mAnimEndCounter == _mGameObjectList.Count * 2)
		{
			foreach (GameObject go in _mGameObjectList)
			{
				go.SetActiveRecursively(false);
			}
			_mGameObjectList.Clear();
			
			_mEndDelegate(this);
			return;
		}
	}
	
	private Vector3 _mDestPosition;
	private Vector3 _mScale;
	private float _mAnimEndCounter;
}

public class AffectorDelayFade : Affector
{
	public override void DoAffect(AffectorDelegate startDel, AffectorDelegate endDel, params object[] args)
	{
		base.DoAffect(startDel, endDel, args);
		//modify laraft 2012-9-26
		Globals.Instance.MCameraController.StartCoroutine(DoCoroutineAffector());
	}
	
	protected IEnumerator DoCoroutineAffector()
	{
		// yield return null;
		yield return new WaitForSeconds(1.0f);
		
		foreach (GameObject go in _mGameObjectList)
		{
			go.SetActiveRecursively(false);
		}
		
		_mGameObjectList.Clear();
		_mEndDelegate(this);
	}
}
