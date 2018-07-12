using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class JobConfig : ConfigBase
{
	public class JobElement
	{
		public int JobID;
		public string Job_Name;
		public int Job_Place_ID;
		public int Need_Act;
		public int Need_Sport;
		public int Need_Knowledge;
		public int Need_Deportment;
		public int Get_Fatigue;
		public int Get_Money;
		public int Get_Fans;
		public int Weight;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Item")
				{
					JobElement itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.JobID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out JobElement itemElement)
	{
		itemElement = new JobElement();
		
		string attribute = element.Attribute("Job_ID");
		if (attribute != null)
			itemElement.JobID = StrParser.ParseDecInt(attribute, -1);
	
		attribute = element.Attribute("Job_Name");
		if (attribute != null)
			itemElement.Job_Name = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Job_Place_ID");
		if (attribute != null)
			itemElement.Job_Place_ID = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Need_Act");
		if (attribute != null)
			itemElement.Need_Act = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Need_Sport");
		if (attribute != null)
			itemElement.Need_Sport = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Need_Knowledge");
		if (attribute != null)
			itemElement.Need_Knowledge = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Need_Deportment");
		if (attribute != null)
			itemElement.Need_Deportment = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Get_Fatigue");
		if (attribute != null)
			itemElement.Get_Fatigue = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Get_Money");
		if (attribute != null)
			itemElement.Get_Money = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Get_Fans");
		if (attribute != null)
			itemElement.Get_Fans = StrParser.ParseDecInt(attribute, -1);

		attribute = element.Attribute("Weight");
		if (attribute != null)
			itemElement.Weight = StrParser.ParseDecInt(attribute, -1);

		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out JobElement itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}

	public JobElement GetSingleElement(int jobID)
	{
		if(_mItemElementList.ContainsKey(jobID))
		{
			return _mItemElementList[jobID];
		}
		return null;
	}

	public Dictionary<int, JobElement> GetJobElementList()
	{
		return _mItemElementList;
	}

	public List<int> GetJobSingleElementList(int mPlaceID)
	{
		int mWeight = 0;
		Dictionary<int, JobElement> mSingleDic = new Dictionary<int, JobElement>();
		foreach(KeyValuePair<int,JobElement> mPair in _mItemElementList)
		{
			if(mPair.Value.Job_Place_ID == mPlaceID)
			{
				mSingleDic.Add(mPair.Key, mPair.Value);
				mWeight += mPair.Value.Weight;
			}
		}

		List<int>  mRandomJobList = new List<int>();
		while(mRandomJobList.Count < 2)
		{
			int ran = Random.Range(0,mWeight);
			
			foreach(KeyValuePair<int,JobElement> mPair in mSingleDic)
			{
				if(ran < mPair.Value.Weight || mPair.Value.Weight >= 100)
				{
					if(!mRandomJobList.Contains(mPair.Key))
					{
						mRandomJobList.Add(mPair.Key);
						break;
					}
				}
				ran -= mPair.Value.Weight;
			}
		}

		return mRandomJobList;
	}
	
	private Dictionary<int, JobElement> _mItemElementList = new Dictionary<int, JobElement>();
}
