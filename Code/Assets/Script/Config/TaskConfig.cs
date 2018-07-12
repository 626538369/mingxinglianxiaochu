using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public enum EType
{
	TYPE_TALK = 1,
	TYPE_KILL = 2,
	TYPE_PASS = 3,
	TYPE_TOPOS = 4,
	TYPE_JuQing = 5
}

public class TaskConfig : ConfigBase
{
	public class TaskObject
	{
		public int Task_ID;
		public string Name;
		public int Task_Talk_ID;
		public string Task_Desc;
		public int City_ID;
		public int Task_Category;
		public int Task_Type;
		public string Begin_Date; 
		public string End_Date;
		public int Task_tag_Branch;
		public string Require_Condition;
		public int Next_Task_ID;
		public int Get_Memo;
		public int Memo_Time_Start;
		public int Memo_Time_End;
		public string Memo_Desc;
		public int Need_Energy;
		public int Need_Act;
		public int Need_Sport;
		public int Need_Knowledge;
		public int Need_Deportment;
		public int Need_Score;
		public int Progress_Count;
		public int Can_Use_Items;
		public int Is_Single;
		public int Need_Friend_Sex;
		public string Posture_Group;
		public string Style_Effect;
		public int Style_Percent;
		public string Material_Effect;
		public int Material_Percent;
		public string Theme_Effect;
		public int Theme_Percent;
		public string Dress_Part_Effect;
		public int Dress_Part_Percent;
		public string Male_Appearance_Effect;
		public string Female_Appearance_Effect;
		public int Max_Dress_Style_Score;
		public int Can_Invite_Npc;
		public string Rewards;
		public int CountDownTime; 
		public string Type_Icon;
		public string Sex_Icon;
		public string Tex_Background;
		public string Scene_Background;
		public string CameraAnimition;
		public string CharacterOneAnimition;
		public string CharacterTwoAnimition;
		public string MusicName;
		public int Add_Line_Day;
		public int Is_Delay;
		public string Stage_Desc;
		public int Is_End;
		public int Next_Task_Fail;
		public int Is_Perfect_End;
	}
    public override bool Load (SecurityElement element)
	{
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				// server
				TaskObject taskObject = new TaskObject();
				taskObject.Task_ID = StrParser.ParseDecInt(childrenElement.Attribute("Task_ID"), -1);
				taskObject.Name = StrParser.ParseStr(childrenElement.Attribute("Name"),"");
				taskObject.Task_Talk_ID = StrParser.ParseDecInt(childrenElement.Attribute("Talk_ID"), -1);
				taskObject.Task_Desc = StrParser.ParseStr(childrenElement.Attribute("Task_Desc"),"");
				taskObject.City_ID = StrParser.ParseDecInt(childrenElement.Attribute("City_ID"), -1);
				taskObject.Task_Category = StrParser.ParseDecInt(childrenElement.Attribute("Task_Category"), -1);
				taskObject.Task_Type = StrParser.ParseDecInt(childrenElement.Attribute("Task_Type"), -1);
				taskObject.Begin_Date = StrParser.ParseStr(childrenElement.Attribute("Begin_Date"),"");
				taskObject.End_Date = StrParser.ParseStr(childrenElement.Attribute("End_Date"),"");
				taskObject.Task_tag_Branch = StrParser.ParseDecInt(childrenElement.Attribute("Is_Branch"), -1);
				taskObject.Require_Condition = StrParser.ParseStr(childrenElement.Attribute("Require_Condition"),"");
				taskObject.Next_Task_ID = StrParser.ParseDecInt(childrenElement.Attribute("Next_Task_ID"), -1);
				taskObject.Get_Memo = StrParser.ParseDecInt(childrenElement.Attribute("Get_Memo"), -1);

				string str = StrParser.ParseStr(childrenElement.Attribute("Memo_Time"), "");
				if(str != "")
				{
					string[] strPar = str.Split('|');
					taskObject.Memo_Time_Start = StrParser.ParseDecInt(strPar[0],-1);
					taskObject.Memo_Time_End = StrParser.ParseDecInt(strPar[1],-1);
				}

				taskObject.Memo_Desc = StrParser.ParseStr(childrenElement.Attribute("Memo_Desc"),"");
				taskObject.Need_Energy = StrParser.ParseDecInt(childrenElement.Attribute("Need_Energy"), -1);
				taskObject.Need_Act = StrParser.ParseDecInt(childrenElement.Attribute("Need_Act"), -1);
				taskObject.Need_Sport = StrParser.ParseDecInt(childrenElement.Attribute("Need_Sport"), -1);
				taskObject.Need_Knowledge = StrParser.ParseDecInt(childrenElement.Attribute("Need_Knowledge"), -1);
				taskObject.Need_Deportment = StrParser.ParseDecInt(childrenElement.Attribute("Need_Deportment"), -1);
				taskObject.Need_Score = StrParser.ParseDecInt(childrenElement.Attribute("Need_Score"), -1);
				taskObject.Progress_Count = StrParser.ParseDecInt(childrenElement.Attribute("Progress_Count"), -1);
				taskObject.Can_Use_Items = StrParser.ParseDecInt(childrenElement.Attribute("Can_Use_Items"), -1);
				taskObject.Is_Single = StrParser.ParseDecInt(childrenElement.Attribute("Is_Single"), -1);
				taskObject.Need_Friend_Sex = StrParser.ParseDecInt(childrenElement.Attribute("Need_Friend_Sex"), -1);
				taskObject.Posture_Group = StrParser.ParseStr(childrenElement.Attribute("Posture_Group"),"");
				taskObject.Style_Effect = StrParser.ParseStr(childrenElement.Attribute("Style_Effect"),"");
				taskObject.Style_Percent = StrParser.ParseDecInt(childrenElement.Attribute("Style_Percent"), -1);
				taskObject.Material_Effect = StrParser.ParseStr(childrenElement.Attribute("Material_Effect"),"");
				taskObject.Material_Percent = StrParser.ParseDecInt(childrenElement.Attribute("Material_Percent"), -1);
				taskObject.Theme_Effect = StrParser.ParseStr(childrenElement.Attribute("Theme_Effect"),"");
				taskObject.Theme_Percent = StrParser.ParseDecInt(childrenElement.Attribute("Theme_Percent"), -1);
				taskObject.Dress_Part_Effect = StrParser.ParseStr(childrenElement.Attribute("Dress_Part_Effect"),"");
				taskObject.Dress_Part_Percent = StrParser.ParseDecInt(childrenElement.Attribute("Dress_Part_Percent"), -1);
				taskObject.Male_Appearance_Effect = StrParser.ParseStr(childrenElement.Attribute("Male_Appearance_Effect"),"");
				taskObject.Female_Appearance_Effect = StrParser.ParseStr(childrenElement.Attribute("Female_Appearance_Effect"),"");
				taskObject.Max_Dress_Style_Score = StrParser.ParseDecInt(childrenElement.Attribute("Max_Dress_Style_Score"), -1);
				taskObject.Can_Invite_Npc = StrParser.ParseDecInt(childrenElement.Attribute("Can_Invite_Npc"), -1);
				taskObject.Rewards = StrParser.ParseStr(childrenElement.Attribute("Rewards"),"");
				taskObject.CountDownTime = StrParser.ParseDecInt(childrenElement.Attribute("Time"), -1); 
				taskObject.Type_Icon = StrParser.ParseStr(childrenElement.Attribute("Type_Icon"),"");
				taskObject.Sex_Icon = StrParser.ParseStr(childrenElement.Attribute("Sex_Icon"),"");
				taskObject.Tex_Background = StrParser.ParseStr(childrenElement.Attribute("Tex_Background"),"");
				taskObject.Scene_Background = StrParser.ParseStr(childrenElement.Attribute("Scene_Background"),"");
				taskObject.CameraAnimition = StrParser.ParseStr(childrenElement.Attribute("CameraAnimition"),"");
				taskObject.CharacterOneAnimition = StrParser.ParseStr(childrenElement.Attribute("CharacterOneAnimition"),"");
				taskObject.CharacterTwoAnimition = StrParser.ParseStr(childrenElement.Attribute("CharacterTwoAnimition"),"");
				taskObject.MusicName = StrParser.ParseStr(childrenElement.Attribute("MusicName"),"");
				taskObject.Add_Line_Day = StrParser.ParseDecInt(childrenElement.Attribute("Add_Line_Day"), -1); 
				taskObject.Is_Delay = StrParser.ParseDecInt(childrenElement.Attribute("Is_Delay"), -1); 
				taskObject.Stage_Desc = StrParser.ParseStr(childrenElement.Attribute("Stage_Desc"),"");
				taskObject.Is_End = StrParser.ParseDecInt(childrenElement.Attribute("Is_End"), -1); 
				taskObject.Next_Task_Fail = StrParser.ParseDecInt(childrenElement.Attribute("Next_Task_Fail"), -1); 
				taskObject.Is_Perfect_End = StrParser.ParseDecInt(childrenElement.Attribute("Is_Perfect_End"), -1); 

				_mTaskObjectDic[taskObject.Task_ID] = taskObject;	
			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}


	public bool GetTaskObject(int taskID, out TaskObject taskObject)
	{
		taskObject = null;
		
		if (!_mTaskObjectDic.TryGetValue(taskID, out taskObject))
		{
			return false;
		}
		return true;
	}

	public bool GeTaskObjectList(out Dictionary<int, TaskObject> mTaskObjectDic)
	{
		mTaskObjectDic = _mTaskObjectDic;
		return true;
	}
	
	public TaskConfig()
	{
		_taskReNameGirlDict.Clear();
		_taskReNameGirlDict.Add(20010,1217001000);
		_taskReNameGirlDict.Add(10260,1217003000);
		_taskReNameGirlDict.Add(10300,1217004000);	
		_taskReNameGirlDict.Add(10330,1217005000);	
	}
	
	protected Dictionary<int, TaskObject> _mTaskObjectDic = new  Dictionary<int, TaskObject>();
	
	public static Dictionary<int, int> _taskReNameGirlDict = new Dictionary<int, int>();
}
