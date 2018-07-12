using UnityEngine;
using System.Collections;

public class TimeRefresh2 : MonoBehaviour {
	public UILabel Timerefresh;
	public long time = 0;
	// Use this for initialization
	void Awake()
	{
		
		InvokeRepeating("TimeRefresh",0,1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setRefreshTime(long Times)
	{
		time = Times;
	}
	public void TimeRefresh()
	{
		if(time != 0)
		{
			time = time -1000;
			long Hour = 0;
			long Min =0;
			long scond = 0;
			int Hours = 0;
			int Minutes = 0;
			int Seconds = 0;
			string tiemRefresh = "";
			string Minute ="";
			string Second = "";
			if(time % (1000*60*60) != 0)
			{
				Min = time % (1000*60*60);
				Hours = ((int)time/(1000*60*60));
			}
			else
			{
				tiemRefresh =((int)time/(1000*60*60)).ToString() + ":" + Minutes + ":" + Second;
				
			}
			if(Min % (1000*60) != 0)
			{
				scond = Min % (1000*60);
				Minutes = ((int)Min /(1000*60));
				if(Minutes < 10)
				{
					Minute = "0" + Minutes;
				}
				else
				{
					Minute = Minutes.ToString();
				}
			}
			else
			{
				tiemRefresh =((int)time/(1000*60*60)).ToString() + ":" + Hours + ":" + "00";
			}
			if(scond >=1000)
			{
				Seconds = (int)scond/1000;
				if(Seconds < 10)
				{
					Second = "0" + Seconds;
				}
				else
				{
					Second = Seconds.ToString();
				}
				tiemRefresh =((int)time/(1000*60*60)).ToString() + ":" + Minute + ":" + Second;
			}
			else
			{
				tiemRefresh =((int)time/(1000*60*60)).ToString() + ":" + Minute + ":" + "00";
			}
			
			Timerefresh.text = tiemRefresh;
			
		}
	}
}
