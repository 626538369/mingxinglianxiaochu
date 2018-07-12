using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Task_Play : ConfigBase
{
	public class Task_PlayObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public int Chapter
		{
			get{return _chapter;}
			set{_chapter = value;}
		}
		public string Chapter_Name
		{
			get{return _chapter_Name;}
			set{_chapter_Name = value;}
		}
		public int CG_Type
		{
			get{return _cG_Type;}
			set{_cG_Type = value;}
		}
		public List<int> Task_IDList
		{
			get{return _task_IDList;}
			set{_task_IDList = value;}
		}
		public string Task_Title
		{
			get{return _task_Title;}
			set{_task_Title = value;}
		}
		public int Is_Feature
		{
			get{return _is_Feature;}
			set{_is_Feature = value;}
		}
		public int Is_branch
		{
			get{return _is_branch;}
			set{_is_branch = value;}
		}

		public static Task_PlayObject Load (SecurityElement element)
		{

			Task_PlayObject task_PlayObjectObject = new Task_PlayObject();		
			task_PlayObjectObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ID"),""),-1);
			task_PlayObjectObject.Chapter = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Chapter"),""),-1);
			task_PlayObjectObject.Chapter_Name = StrParser.ParseStr(element.Attribute ("Chapter_Name"), "");
			task_PlayObjectObject.CG_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("CG_Type"),""),-1);
			string  Condition = StrParser.ParseStr(element.Attribute ("Task_ID"), "");
			if("" != Condition && null != Condition)
			{
				string[] vecs =  Condition.Split('|');
				task_PlayObjectObject._task_IDList.Clear();
				foreach(string Conditionstring in vecs)
				{	
					int temp = StrParser.ParseDecInt(Conditionstring,-1);
					task_PlayObjectObject._task_IDList.Add(temp);
				}
			}
			task_PlayObjectObject.Task_Title =  StrParser.ParseStr(element.Attribute ("Task_Title"), "");
			task_PlayObjectObject._is_Feature = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Is_Feature"),""),-1);
			task_PlayObjectObject._is_branch = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Is_branch"),""),-1);

//			
			return task_PlayObjectObject;
		}
		
		protected int _id;
		protected int _chapter;
		protected string _chapter_Name;
		protected int _cG_Type;
		protected List<int> _task_IDList = new List<int>();
		protected string _task_Title;
		protected int _is_Feature;
		protected int _is_branch;

			

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				Task_PlayObject task_PlayObjectObject = Task_PlayObject.Load(childrenElement);
				
				if (!_task_PlayObjectObjectDict.ContainsKey(task_PlayObjectObject.ID))
					_task_PlayObjectObjectDict[task_PlayObjectObject.ID] = task_PlayObjectObject;
			}
		}
		return true;
	}
	public Task_PlayObject GetObjByID(int ID)
	{
		foreach(Task_PlayObject data in _task_PlayObjectObjectDict.Values)
		{
			if(ID == data.ID)
				return data;
		}
		return null;
	}
	public string GetNameByChapter(int Chapter)
	{
		foreach(Task_PlayObject data in _task_PlayObjectObjectDict.Values)
		{
			if(Chapter == data.Chapter)
				return data.Chapter_Name;
		}
		return null;
	}
	public bool IsLastInThisChapter(int ID)
	{
		int TChapter = ID/100;
		foreach(Task_PlayObject data in _task_PlayObjectObjectDict.Values)
		{
			if(TChapter == data.Chapter && ID <  data.ID)
				return false;
		}
		return true;
	}
	
	public bool IsLastInAllChapter(int ID)
	{
		int TChapter = ID/100;
		foreach(Task_PlayObject data in _task_PlayObjectObjectDict.Values)
		{
			if(ID <  data.ID)
				return false;
		}
		return true;
	}
	public int GetCGTypeByID(int ID)
	{
		if(_task_PlayObjectObjectDict.ContainsKey(ID))
		{
			return _task_PlayObjectObjectDict[ID].CG_Type;
		}
		return -1;
	}

	
	protected Dictionary<int, Task_PlayObject> _task_PlayObjectObjectDict = new Dictionary<int, Task_PlayObject>();
}
