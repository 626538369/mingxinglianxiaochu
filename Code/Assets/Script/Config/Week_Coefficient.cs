using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

//public enum EType
//{
//	TYPE_TALK = 1,
//	TYPE_KILL = 2,
//	TYPE_PASS = 3,
//	TYPE_TOPOS = 4,
//	TYPE_JuQing = 5
//}

public class Week_Coefficient : ConfigBase
{
	public class WeekObject
	{
		public int Week_Num;
		public float Difficulty_Coefficient;

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				// server
				WeekObject weekObject = new WeekObject();
				weekObject.Week_Num = StrParser.ParseDecInt(childrenElement.Attribute("Week_Num"), -1);
				weekObject.Difficulty_Coefficient = float.Parse( childrenElement.Attribute("Difficulty_Coefficient"));

			
				_mWeekObjectDic[weekObject.Week_Num] = weekObject;	
			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}
	
	
	public bool GetWeekObject(int Week_Num, out WeekObject weekObject)
	{
		weekObject = null;
		
		if (!_mWeekObjectDic.TryGetValue(Week_Num, out weekObject))
		{
			return false;
		}
		return true;
	}
	
	public bool GeWeekObjectList(out Dictionary<int, WeekObject> mWeekObjectDic)
	{
		mWeekObjectDic = _mWeekObjectDic;
		return true;
	}
	
//	public Week_Coefficient()
//	{
//		_taskReNameGirlDict.Clear();
//		_taskReNameGirlDict.Add(20010,1217001000);
//		_taskReNameGirlDict.Add(10260,1217003000);
//		_taskReNameGirlDict.Add(10300,1217004000);	
//		_taskReNameGirlDict.Add(10330,1217005000);	
//	}
	
	protected Dictionary<int, WeekObject> _mWeekObjectDic = new  Dictionary<int, WeekObject>();
	
	//public static Dictionary<int, int> _taskReNameGirlDict = new Dictionary<int, int>();
}
