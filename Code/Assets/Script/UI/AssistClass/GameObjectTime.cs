using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class GameObjectTime : MonoBehaviour 
{
		
	public UILabel RefreshDate;
	public UILabel RefreshTime;
	private DateTime mRefreshTime;
	
	void Awake()
	{
		
		//InvokeRepeating("TimerTickNotify",0,1);
	}
	
	void OnDestroy()
	{

	}
	
	public void setRefreshTime(long refreshTime)
	{
		System.DateTime SDT = new System.DateTime(0);
		HelpUtil.JsTime2CSharpDateTime(refreshTime,out SDT);
		mRefreshTime = SDT;		
		string Hour ="";
		string Minute = "";
		string Second = "";
		if(SDT.Hour < 10)
		{
			Hour = "0" + SDT.Hour;
		}
		else
		{
			Hour = SDT.Hour.ToString();
		}
		if(SDT.Minute < 10)
		{
			Minute = "0" + SDT.Minute;
		}
		else
		{
			Minute = SDT.Minute.ToString();
		}
		if(SDT.Second < 10)
		{
			Second = "0" + SDT.Second;
		}
		else
		{
			Second = SDT.Second.ToString();
		}
		RefreshDate.text = SDT.Year + "/" + SDT.Month + "/" + SDT.Day;
		RefreshTime.text = Hour + ":" + Minute + ":" + Second;
	 
	}
	
	public void setRefreshTimeLength(long refreshTime)
	{
		DateTime nowTime  = DateTime.Now;
		mRefreshTime = nowTime.AddMilliseconds(refreshTime);
	}
	
	private void TimerTickNotify()
	{
		
	}
}