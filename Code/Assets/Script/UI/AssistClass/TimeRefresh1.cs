using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class TimeRefresh1 : MonoBehaviour 
{
	public UILabel   RefreshTimeHour;
	public UILabel   RefreshTimeMinute;
	public UILabel   RefreshTimeSecond;
	private int    mLeftTimeSecond;
	
	void Awake()
	{
		
		InvokeRepeating("TimerTickNotify",0,1);
	}
	
	void OnDestroy()
	{

	}
	
	public void setLeftTime(int leftSecondTime)
	{
		mLeftTimeSecond = leftSecondTime;
		TimerTickNotify();
	}
	
	
	private void TimerTickNotify()
	{
		mLeftTimeSecond--;
		if (mLeftTimeSecond < 0)
			return;
		if (mLeftTimeSecond < 60)
		{
			RefreshTimeHour.text = "00";
			RefreshTimeMinute.text = "00";
			RefreshTimeSecond.text = mLeftTimeSecond.ToString();
		}
		else if (mLeftTimeSecond >= 60 && mLeftTimeSecond < 3600)
		{
			int minute = mLeftTimeSecond / 60;
			int second = mLeftTimeSecond % 60;
			RefreshTimeHour.text = "00"; 
			RefreshTimeMinute.text = minute.ToString();
			RefreshTimeSecond.text = second.ToString();
		}
		else if (mLeftTimeSecond >= 3600)
		{
			int minuteTime = mLeftTimeSecond / 60;
			int hourStr = minuteTime/60 ;
			int minuteStr = minuteTime%60;
			int secondStr = mLeftTimeSecond % 60;
			RefreshTimeHour.text = hourStr.ToString(); 
			RefreshTimeMinute.text = minuteStr.ToString();
			RefreshTimeSecond.text = secondStr.ToString();
		}
	}
	
	
}