using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArenaInfoManager : MonoBehaviour 
{
	[HideInInspector]public long coolingEndTime;
	[HideInInspector]public bool isTimeGoing;
	[HideInInspector]public bool isEnteringGame;
		
	void Awake()
	{
		isTimeGoing = false;
		isEnteringGame = false;
	}

	public void TimerStart()
	{
		isTimeGoing = true;
		InvokeRepeating("TimerTickNotify",0,1);
		RegisterMonoEventChange();
		if(Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>() != null)
		{

		}
	}
	
	public void TimerEnd()
	{
		isTimeGoing = false;
		CancelInvoke();
		
	
		if(Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>() != null)
		{

		}
	}
	
	
	public void TimerTickNotify()
	{
		if(!isTimeGoing) return;
		coolingEndTime = coolingEndTime - 1;

		if(coolingEndTime <= 0)
		{
			coolingEndTime = 0;
			isTimeGoing = false;
		}
		
		
		if(Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>() != null)
		{

		}
		
		if(!isTimeGoing)
		{
			 TimerEnd();
		}
	}
	
	public string ConvertTime()
	{
		TimeSpan tCD = new TimeSpan(0,0,(int)coolingEndTime);
		string tCDTime = String.Format("{0:0#}:{1:0#}",(int)tCD.Minutes,(int)tCD.Seconds);
		
		return tCDTime;	
	}
		
	void RegisterMonoEventChange()
	{
		ISubscriber subscriber = EventManager.Subscribe(MonoEventPublisher.NAME + ":" + MonoEventPublisher.MONO_FOCUS);
		subscriber.Handler = delegate (object[] args)
		{
			bool focus = (bool)args[0];
			long ms = (long) args[1];
			coolingEndTime -= ms;
			if(coolingEndTime < 0)
			{
				coolingEndTime = 0;
				TimerEnd();
			}
		};
	}
	
	public void UpdateCDTime()
	{
		if(coolingEndTime > 0 && !isTimeGoing)
		{
			TimerStart();
		}
	}
	
}


