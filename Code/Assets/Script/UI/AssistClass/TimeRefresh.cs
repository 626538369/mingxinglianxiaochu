using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TimeRefresh : MonoBehaviour 
{
	public UILabel   RefreshTimeHour;	
	private DateTime mRefreshTime;

	public int Digit = 1;
	
	public delegate void ReMainingTime();
	[HideInInspector] public event TimeRefresh.ReMainingTime ReMainingTimer  = null;

	
	void Awake()
	{
		InvokeRepeating("TimerTickNotify",0,1);
	}
	
	void OnDestroy()
	{

	}
	
	public void setRefreshTime(long refreshTime)
	{
		System.DateTime SDT = new System.DateTime(0);
		HelpUtil.JsTime2CSharpDateTime(refreshTime,out SDT);
		mRefreshTime = SDT;		
	}
	
	public void setRefreshTimeLength(long refreshTime)
	{
		DateTime nowTime  = DateTime.Now;
		mRefreshTime = nowTime.AddMilliseconds(refreshTime);
	}
	
	private void TimerTickNotify()
	{
	
		DateTime nowTime  = DateTime.Now;
		
		TimeSpan ts  = mRefreshTime.Subtract(nowTime);

		string timeStr = "";
		if(Digit == 2)
		{
			if(0 < ts.Hours)
			{
				timeStr = ts.Hours.ToString() + Globals.Instance.MDataTableManager.GetWordText(5019);
				if(ts.Minutes > 0)
					timeStr += ts.Minutes.ToString() + Globals.Instance.MDataTableManager.GetWordText(5020);
				RefreshTimeHour.text = timeStr;
			}else if(0 < ts.Minutes)
			{
				timeStr = ts.Minutes.ToString() + Globals.Instance.MDataTableManager.GetWordText(5020);
				if(ts.Seconds > 0)
					timeStr += ts.Seconds.ToString() + Globals.Instance.MDataTableManager.GetWordText(5021);
				RefreshTimeHour.text = timeStr;
			}else if(0 < ts.Seconds)
			{
				timeStr = ts.Seconds.ToString() + Globals.Instance.MDataTableManager.GetWordText(5021);
				RefreshTimeHour.text = timeStr;
			}
		}
		else
		{
			if(0 < ts.Hours)
			{
				timeStr = ts.Hours.ToString() + Globals.Instance.MDataTableManager.GetWordText(5019);
				RefreshTimeHour.text = timeStr;
			}else if(0 < ts.Minutes)
			{
				timeStr = ts.Minutes.ToString() + Globals.Instance.MDataTableManager.GetWordText(5020);
				RefreshTimeHour.text = timeStr;
			}else if(0 < ts.Seconds)
			{
				timeStr = ts.Seconds.ToString() + Globals.Instance.MDataTableManager.GetWordText(5021);
				RefreshTimeHour.text = timeStr;
			}
		}
		
		if(ts.TotalSeconds <= 0)
		{
			if(ReMainingTimer != null)
			{
				ReMainingTimer();
				ReMainingTimer = null;
			}
		}

		return;
	}
	
	
}