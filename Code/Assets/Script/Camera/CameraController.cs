using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CameraBehaviourMode
{
	BEHAVIOUR_SMOOTH_FOLLOW = 1 << 0,
	BEHAVIOUR_SMOOTH_LOOKAT = 1 << 1,
	BEHAVIOUR_SMOOTH_MOVE = 1 << 2,
	BEHAVIOUR_SMOOTH_ZOOM = 1 << 3,
	BEHAVIOUR_SMOOTH_TRACK_MOTION = 1 << 4,
}

public abstract class CameraBehaviour : MonoBehaviour
{
	public abstract void LateUpdate();
	public bool isUseCoroutine;
}

public class CameraController : MonoBehaviour
{
	protected void Awake()
	{
		_mCameraBehaviourList.Clear();
		
		//CameraBehaviour behaviour = new CameraSmoothFollow();
		// _mCameraBehaviourList.Add(CameraBehaviourMode.BEHAVIOUR_SMOOTH_FOLLOW, behaviour);
		// 
		// behaviour = new CameraSmoothLookAt();
		// _mCameraBehaviourList.Add(CameraBehaviourMode.BEHAVIOUR_SMOOTH_LOOKAT, behaviour);
		// 
		// behaviour = new CameraSmoothMove();
		// _mCameraBehaviourList.Add(CameraBehaviourMode.BEHAVIOUR_SMOOTH_MOVE, behaviour);
		// 
		// behaviour = new CameraSmoothZoom();
		// _mCameraBehaviourList.Add(CameraBehaviourMode.BEHAVIOUR_SMOOTH_ZOOM, behaviour);
		// 
		// CameraBehaviour behaviour = new CameraSmoothTrackMotion();
		// _mCameraBehaviourList.Add(CameraBehaviourMode.BEHAVIOUR_SMOOTH_TRACK_MOTION, behaviour);
		
		// Disable all behaviours
		DisableAllCameraBehaviours();
	}
	
	void Start()
	{}
	
	void LateUpdate()
	{
		if (0 == _mCameraBehaviourList.Count)
			return;
		
		foreach (CameraBehaviour behaviour in _mCameraBehaviourList.Values)
		{
			if (!behaviour.enabled)
				continue;
			
			behaviour.LateUpdate();
		}
	}
	
	public void AddCameraBehaviour(CameraBehaviourMode mode, CameraBehaviour behaviour)
	{
		_mCameraBehaviourList.Add(mode, behaviour);
	}
	
	public T GetCameraBehaviour<T>(CameraBehaviourMode mode) where T : CameraBehaviour
	{
		CameraBehaviour behaviour = null;
		if ( !_mCameraBehaviourList.TryGetValue(mode, out behaviour) )
		{
			return null;
		}
		
		return (T)behaviour;
	}
	
	public void EnableCameraBehaviour(CameraBehaviourMode mode)
	{
		_mBehaviourMode += (int)mode;
		
		CameraBehaviour behaviour = null;
		if ( _mCameraBehaviourList.TryGetValue(mode, out behaviour) )
		{
			behaviour.enabled = true;
		}
	}
	
	public void DisableCameraBehaviour(CameraBehaviourMode mode)
	{
		_mBehaviourMode -= (int)mode;
		
		CameraBehaviour behaviour = null;
		if ( _mCameraBehaviourList.TryGetValue(mode, out behaviour) )
		{
			behaviour.enabled = false;
		}
	}
	
	public void DisableAllCameraBehaviours()
	{
		_mBehaviourMode = 0;
		foreach (CameraBehaviour behaviour in _mCameraBehaviourList.Values)
		{
			behaviour.enabled = false;
		}
	}
	
	public bool isInCameraBehaviour(CameraBehaviourMode mode)
	{
		int result = _mBehaviourMode & (int)mode;
		return result != 0;
	}
	
	private int _mBehaviourMode = 0;
	private Dictionary<CameraBehaviourMode, CameraBehaviour> _mCameraBehaviourList = new Dictionary<CameraBehaviourMode, CameraBehaviour>();
}
