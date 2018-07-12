using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimKeyFrameInfo
{
	public Vector3 Position;
	public Vector3 EulerAngles;
	// public Vector3 Scale;
	
	// The time cost from last key frame to this key frame
	public float TransformTime;
	public float DelayTime;
}

// [ExecuteInEditMode]
public class RelAnimControl : MonoBehaviour 
{
	public int Index
	{
		get { return currentIndex; }
		set { currentIndex = value; }
	}
	
	void Awake()
	{
		origPos = gameObject.transform.position;
		origEulerAngles = gameObject.transform.rotation.eulerAngles;
		if (null == keyFrameInfos)
			keyFrameInfos = new AnimKeyFrameInfo[0];
		
		// if (null == targetGameObj)
		targetGameObj = this.gameObject;
		
		currentIndex = 0;
		isTrackEnalbed = true;
		trackEnumerator = DoMoveTrack();
	}
	
	void Start()
	{
		Restart();
	}
	
	/// <summary>
	/// tzz added for 
	/// Copies to other RelAnimControl
	/// </summary>
	/// <param name='_dst'>
	/// _dst.
	/// </param>
	public void CopyTo(RelAnimControl _dst){
		_dst.keyFrameInfos		= keyFrameInfos;
		_dst.origPos			= origPos;
		_dst.origEulerAngles	= origEulerAngles;
	}
	
	void OnDestroy()
	{
	}
	
	public void Pause()
	{
		ITweenStop();
	}
	
	public AnimKeyFrameInfo Stop(bool playDoneDelegate = false)
	{
		ITweenStop();
		
		if(playDoneDelegate){
			PlayDoneDelegate();
		}
		
		return keyFrameInfos[keyFrameInfos.Length - 1];
	}
	
	public void Resume()
	{
		ITweenStart();
	}
	
	public void Restart()
	{
		currentIndex = 0;
		ITweenStart();
	}
	
	void Test()
	{
		if (null == keyFrameInfos || keyFrameInfos.Length == 0)
		{
			Debug.Log("");
			return;
		}
		
		StartCoroutine( "DoMoveTrack" );
	}
	
	IEnumerator DoMoveTrack()
	{
		yield return null;
	}
	
	/// <summary>
	/// raise the done delegate.
	/// </summary>
	private void PlayDoneDelegate(){
		// tzz added for play down delegate event
		if(m_playDoneDelegate != null){
			try{
				m_playDoneDelegate();
			}catch(System.Exception e){
				Debug.LogError (e.Message + "\n" + e.StackTrace);
			}
		}
	}
	
	private void ITweenStart()
	{
		if (currentIndex >= keyFrameInfos.Length)
		{
			isTrackEnalbed = false;
			PlayDoneDelegate();			
			return;
		}
		
		AnimKeyFrameInfo info = keyFrameInfos[currentIndex];
		ITweenMoveTo(info, delegate()
		{
			currentIndex++;
			ITweenStart();
		}
		);
	}
	
	/// <summary>
	/// Gets the length time of the play.
	/// </summary>
	/// <returns>
	/// The play length.
	/// </returns>
	public float GetPlayLength(){
		
		float tLength = 0;
		
		foreach(AnimKeyFrameInfo key in keyFrameInfos ){
			tLength += key.DelayTime + key.TransformTime;
		}
		
		return tLength;
	}
	
	private void ITweenStop()
	{
		// tzz modified for avoid Stop shake type of itween of camera
		iTween.Stop(targetGameObj,"move");
		iTween.Stop(targetGameObj,"rotate");
	}
	
	private void ITweenMoveTo(AnimKeyFrameInfo _keyInfo, iTween.EventDelegate complete)
	{
		ITweenMoveTo(targetGameObj,_keyInfo, complete,_keyInfo.TransformTime,origPos,origEulerAngles);
	}
	
	//! tzz added for camera move
	public static void ITweenMoveTo(GameObject _obj,AnimKeyFrameInfo _keyInfo,iTween.EventDelegate complete,float time){
		ITweenMoveTo(_obj,_keyInfo,complete,time,Vector3.zero,Vector3.zero);
	}

	public static void ITweenMoveTo(GameObject _obj,AnimKeyFrameInfo _keyInfo,iTween.EventDelegate complete,
									float time,Vector3 refPosition,Vector3 refRotation){	
		
		iTween.MoveTo(_obj, iTween.Hash("position", _keyInfo.Position + refPosition, "time", time, "delay", _keyInfo.DelayTime, "easetype", iTween.EaseType.easeInOutSine),
				null, complete);
		iTween.RotateTo(_obj, iTween.Hash("rotation", _keyInfo.EulerAngles + refRotation, "time", time, "delay", _keyInfo.DelayTime, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	public AnimKeyFrameInfo[] keyFrameInfos;
	
	[HideInInspector]
	public GameObject targetGameObj;
	
	[HideInInspector]
	private int currentIndex;
	
	/// <summary>
	/// The m_play done delegate.
	/// </summary>
	private iTween.EventDelegate	m_playDoneDelegate = null;
	
	/// <summary>
	/// Sets the play done delegate.
	/// </summary>
	/// <param name='_dele'>
	/// _dele.
	/// </param>
	public void SetPlayAnimDoneDelegate(iTween.EventDelegate _dele){
		m_playDoneDelegate = _dele;
	}
	
	public Vector3 origPos;
	public Vector3 origEulerAngles;
	private bool isTrackEnalbed;
	private IEnumerator trackEnumerator;
}

