using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JobManager : MonoBehaviour 
{
	public enum JobState
	{
		WORKING  = 0,
		FINISHEED = 1,
		NOT_START = 2,
	}
	
	public enum InviteType
	{
		FRIEND = 0,
		GIRL = 1,
		NEARBY = 2,
		NPC = 3,
		Pet = 4,
	}
	
	public class JobItemInfo
	{
		public int JobID;
		public int JobState;
		public float JobLeftTime;
		public long JobInviteFriendID;
		public int JobIndustryID;
	};	
	
	private List<JobItemInfo> mJobItemInfoList = new List<JobItemInfo>();
	
	private int mCurrentWorkingJobID;
	private long mCurrentWorkingInviteFrindID;
	public  int ToOpenFinishJobUI = 1;
	
	public int GDiamondJob = 0;

	private int JobRefreshLastTime = -1;
	private Dictionary<int,List<int>> CacheJobPlaceInformation = new Dictionary<int,List<int>>();

	void Awake()
	{
		InvokeRepeating("TimerTickNotify",0,1);
	}
	
	public int CurrentWorkingJobID
    {
        get { return mCurrentWorkingJobID; }
        set { mCurrentWorkingJobID = value; }
    }
	
	public long CurrentWorkingInviteFriendID
    {
        get { return mCurrentWorkingInviteFrindID; }
        set { mCurrentWorkingInviteFrindID = value; }
    }
	
	private void TimerTickNotify()
	{
		for (int i=0; i<mJobItemInfoList.Count; i++)
		{
			if (mJobItemInfoList[i].JobLeftTime > 0)
			{
				JobItemInfo jobItemInfo =  mJobItemInfoList[i];
				jobItemInfo.JobLeftTime -= 1;
				mJobItemInfoList[i] = jobItemInfo;
				
				if (jobItemInfo.JobLeftTime  <= 0 && jobItemInfo.JobState != (int)JobManager.JobState.FINISHEED)
				{
					jobItemInfo.JobState = (int)JobManager.JobState.FINISHEED;
					mJobItemInfoList[i] = jobItemInfo;
				

					
				}
				else
				{
					

					
				}

			}
		}
	}
	
		
	public List<JobItemInfo> getJobItemInfoList()
	{
		return mJobItemInfoList;
	}

	public Dictionary<int,List<int>> getJobPlaceInformationDic
	{
		get
		{
			return CacheJobPlaceInformation;
		}
		set
		{
			CacheJobPlaceInformation = value;
		}
	}

	public int GetJobRefreshLastTime
	{
		get
		{
			return JobRefreshLastTime;
		}
		set
		{
			JobRefreshLastTime = value;
		}
	}

	public void updateJobItemInfo(int jobID, int jobState, float jobLeftTime)
	{

		
	}
	
	public JobManager.JobItemInfo getJobItemInfo(int jobID)
	{
		for (int i=0; i<mJobItemInfoList.Count; i++)
		{
			if (mJobItemInfoList[i].JobID ==  jobID)
			{
				JobItemInfo jobItemInfo =  mJobItemInfoList[i];
				return jobItemInfo;
			}
		}
		return null;
	}
	
	public int getJobLeftTime(int jobID)
	{
		for (int i=0; i<mJobItemInfoList.Count; i++)
		{
			if (mJobItemInfoList[i].JobID ==  jobID)
			{
				int leftTime = (int)mJobItemInfoList[i].JobLeftTime;
				if (leftTime == 0)
					return 0;
				return leftTime;
			}
		}
		return 0;
	}
	
	public void insertJobItemInfo(int jobID, int jobState, float jobLeftTime,long inviteFriendID)
	{

	}
	
	public void removeJobItemInfo(int jobID)
	{

	}
}
