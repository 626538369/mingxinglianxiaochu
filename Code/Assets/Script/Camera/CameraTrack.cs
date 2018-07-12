using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class KeyFrameInfo
{
	public Vector3 Position;
	
	// tzz fucked
	// this CameraTrack will reuse in CameraTrackController so this function will call more than one time
	// must backup the original position
	//
	[HideInInspector]
	public Vector3 PositionBackup;
	
	public Vector3 EulerAngles;
	// public Vector3 Scale;
	
	// The time cost from last key frame to this key frame
	public float TransformTime;
	public float DelayTime;
	public bool isStartPoint;
	public bool isTargetPoint;
}

// [ExecuteInEditMode]
public class CameraTrack : MonoBehaviour 
{
	public int Index
	{
		get { return currentIndex; }
		set { currentIndex = value; }
	}
	
	void Awake()
	{
		if (null == keyFrameInfos){
			keyFrameInfos = new KeyFrameInfo[0];
		}
		
		// tzz fucked
		// this CameraTrack will reuse in CameraTrackController so this function will call more than one time
		// must backup the original position
		foreach(KeyFrameInfo info in keyFrameInfos){
			info.PositionBackup = info.Position;
		}
		
		// if (null == targetGameObj)
		targetGameObj = Globals.Instance.MSceneManager.mMainCamera.gameObject;
		
		currentIndex = 0;
		isTrackEnalbed = true;
	}
		
	public void Pause()
	{
		ITweenStop();
	}
	
	public KeyFrameInfo Stop()
	{
		ITweenStop();
		
		return keyFrameInfos[keyFrameInfos.Length - 1];
	}
	
	public void Resume()
	{
		ITweenStart();
	}
	
	/// <summary>
	/// Restart the specified refPos and startPosition.
	/// </summary>
	/// <param name='refPos'>
	/// Reference position.
	/// </param>
	/// <param name='startPosition'>
	/// Start position.
	/// </param>
	public void Restart(Vector3 startPosition){
		
		foreach(KeyFrameInfo frameInfo in keyFrameInfos){
			frameInfo.Position = frameInfo.PositionBackup + startPosition;
		}
			
		currentIndex = 0;
		ITweenStart();
	}
	
	/// <summary>
	/// Restart this instance.
	/// </summary>
	public void Restart(){
		
		// restore the position to absolute axes
		foreach(KeyFrameInfo frameInfo in keyFrameInfos){
			frameInfo.Position = frameInfo.PositionBackup;
		}
		
		currentIndex = 0;
		ITweenStart();
	}
	
	/// <summary>
	/// Restarts the nivose track.
	/// </summary>
	/// <param name='refPos'>
	/// Reference position.
	/// </param>
	/// <param name='startPosition'>
	/// Start position.
	/// </param>
	/// <param name='targetPosition'>
	/// Target position.
	/// </param>
	public void RestartNivoseTrack(bool refPos,Vector3 startPosition,Vector3 targetPosition){
		if(refPos){
			
			foreach(KeyFrameInfo frameInfo in keyFrameInfos){
				if(frameInfo.isStartPoint){
					
					frameInfo.Position = frameInfo.PositionBackup + startPosition;
					
				}else if(frameInfo.isTargetPoint){
					
					frameInfo.Position = frameInfo.PositionBackup + targetPosition;
				}
			}
		}

		currentIndex = 0;
		ITweenStart();
	}
		
	private void ITweenStart()
	{
		if (currentIndex >= keyFrameInfos.Length)
		{
			isTrackEnalbed = false;
			return;
		}
				
		ITweenMoveTo(keyFrameInfos[currentIndex], delegate(){
			currentIndex++;
			ITweenStart();
		});
	}
	
	private void ITweenStop()
	{
		// tzz modified for avoid Stop shake type of itween of camera
		iTween.Stop(targetGameObj,"move");
		iTween.Stop(targetGameObj,"rotate");
	}
	
	private void ITweenMoveTo(KeyFrameInfo _keyInfo, iTween.EventDelegate complete)
	{
		ITweenMoveTo(targetGameObj,_keyInfo, complete,_keyInfo.TransformTime);
	}
	
	//! tzz added for camera move
	public static void ITweenMoveTo(GameObject _obj,KeyFrameInfo _keyInfo,iTween.EventDelegate complete,float time){	
	
		iTween.MoveTo(_obj, iTween.Hash("position", _keyInfo.Position, "time", time, "delay", _keyInfo.DelayTime, "easetype", iTween.EaseType.easeInOutSine),
			null, complete);
		iTween.RotateTo(_obj, iTween.Hash("rotation", _keyInfo.EulerAngles, "time", time, "delay", _keyInfo.DelayTime, "easetype", iTween.EaseType.easeInOutSine));
	}
	
	public KeyFrameInfo[] keyFrameInfos;
	
	[HideInInspector]
	public GameObject targetGameObj;
	
	[HideInInspector]
	private int currentIndex;
	
	private bool isTrackEnalbed;
}
