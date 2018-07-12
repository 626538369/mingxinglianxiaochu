using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// tzz fucked
/// EZ general avatar.
/// </summary>
public class EZWarshipHeader : EZ3DItem{
	
	/// <summary>
	/// The avatar prefab
	/// </summary>
	public PackedSprite		AvatarPrefab;
	
	/// <summary>
	/// The avatar background.
	/// </summary>
	public PackedSprite		AvatarBg;
	
	/// <summary>
	/// The fire progressbar prefab
	/// </summary>
	public UISlider	FireProgressPrefab;
	
	private WarshipL		mShipData;	
	
	private PackedSprite	mAvatar			= null;
	
	/// <summary>
	/// The fire progress total step time
	/// </summary>
	private float			mFireProgressTotalStepTime	= 0;
	
	/// <summary>
	/// The fire progress counter.
	/// </summary>
	private float			mFireProgressCounter	= 0;
	
	/// <summary>
	/// The m avatar disappear time(1.5-3 second)
	/// </summary>
	private float			mAvatarDisappearTime	= 0;
	
	/// <summary>
	/// The m avatar disappear counter.
	/// </summary>
	private float			mAvatarDisappearCounter	= 0;
	
	/// <summary>
	/// The  Z level to sort with other header
	/// </summary>
	private float			mZLevel					= 0;
	
	private static List<EZWarshipHeader>		smWarshipList = new List<EZWarshipHeader>();
	
	void Awake(){
		smWarshipList.Add(this);
	}
	
	public override void SetValue(params object[] args){
		
		
	}
	
	/// <summary>
	/// Avatar this instance.
	/// </summary>
	public string Avatar{
		get{return mAvatar != null ? mAvatar.GetCurAnim().name : null;}
	}
	
	void LateUpdate(){
		
		
	}
	
	void OnDestroy(){
		if(mAvatar != null){
			Destroy(mAvatar);
			mAvatar = null;
		}
		
		smWarshipList.Remove(this);
	}
	
	/// <summary>
	/// Sets the buffer step interval
	/// </summary>
	/// <param name='stepInterval'>
	/// Step interval.
	/// </param>
	public void SetBufferStepInterval(List<int> bufferList,int currStep){
		
		int tStepInterval	= 0;
		int tIdx 			= bufferList.IndexOf(currStep);
		
		if(tIdx != -1){
			if(bufferList.Count - 1 == tIdx){
				//FireProgressPrefab.transform.localScale = Vector3.one;
				//return ;
				
				tStepInterval = WarshipConfig.MaxFillSpeedAttr * 2;
				
			}else{
				tStepInterval = bufferList[tIdx + 1] - bufferList[tIdx];
			}			
		}else{
			return;
		}
		
		mFireProgressTotalStepTime	= tStepInterval * 2.6f;
		mFireProgressCounter		= 0;
		
		FireProgressPrefab.sliderValue = 0;
		
		
		//if(FireProgressPrefab.IsHidden()){
			FireProgressPrefab.transform.localScale = Vector3.zero;
		//}		
	}
	
	public override void Reposition(Vector3 worldPos){
		// fuck repostion parameters worldpos
	}
}
