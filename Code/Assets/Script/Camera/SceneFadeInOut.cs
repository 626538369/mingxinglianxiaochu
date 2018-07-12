using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneFadeInOut : MonoBehaviour {
		
	private static readonly Color fsm_transparent = new Color(1,1,1,0);
	private static readonly Color fsm_opaue = new Color(1,1,1,1);
	
	/// <summary>
	/// The fade background sprite 
	/// </summary>
	private PackedSprite		m_fadeBackground;
	
	private static 	SceneFadeInOut	sm_instance;
	
	private FadeSprite			mCurrFadeSprite = null;
	
	private bool				mFadePlaying = false;
	private bool				mFadeOutState = false;
	
	private float				mFadeTime			= 1.0f;
	private float				mFadeTimeCounter	= 0;

	// instance of this class
	public static SceneFadeInOut Instance{
		get{return sm_instance;}
	}
	
	// Use this for initialization
	protected void Awake () {
		sm_instance = this;
		
		// background
		m_fadeBackground = GetComponent<PackedSprite>();
		
		// hide the background first
		m_fadeBackground.transform.localScale = Vector3.one;
	}
	
	void Update(){
		if(mFadePlaying){
			
			mFadeTimeCounter += Time.deltaTime;
			
			if(mFadeTimeCounter > mFadeTime){
				mFadeTimeCounter = mFadeTime;
				mFadePlaying = false;
			}
			
			float tStart;
			float tEnd;
			
			if(mFadeOutState){
				tStart	= fsm_transparent.a;
				tEnd	= fsm_opaue.a;				
			}else{
				tStart	= fsm_opaue.a;
				tEnd	= fsm_transparent.a;
			}
						
			float tValue = tStart + (tEnd - tStart) * (mFadeTimeCounter / mFadeTime);
			m_fadeBackground.Color = new Color(1,1,1,tValue);			
		}
	}
		
	/// <summary>
	/// Fades the out scene.
	/// </summary>
	/// <param name='_time'>
	/// _time second unit
	/// </param>
	public void FadeOutScene(float _time = 3){
				
		m_fadeBackground.transform.localScale = Vector3.zero;
		m_fadeBackground.Color = fsm_transparent;
		
		mFadePlaying		= true;
		mFadeTimeCounter	= 0;
		mFadeTime			= _time;
		mFadeOutState		= true;
	}
	
	/// <summary>
	/// Fades the in scene.
	/// </summary>
	/// <param name='time'>
	/// Time second unit
	/// </param>
	public void FadeInScene(float _time = 3){
			
		m_fadeBackground.transform.localScale = Vector3.zero;
		m_fadeBackground.Color = fsm_opaue;
		
		mFadePlaying		= true;
		mFadeTimeCounter	= 0;
		mFadeTime			= _time;
		mFadeOutState		= false;
	}
}
