using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Task_Daily : ConfigBase
{
	public class RewardData
	{
		public int ID;
		public int Num;	
	}
	public class TaskDailyObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		
		public int Logic_ID
		{
			get{return _logic_ID;}
			set{_logic_ID = value;}
		}
		public string  Name
		{
			get{return _name;}
			set{_name = value;}
		}
		public string  Des
		{
			get{return _des;}
			set{_des = value;}
		}
		public int Task_Category
		{
			get{return _task_Category;}
			set{_task_Category = value;}
		}
		public int Task_Type
		{
			get{return _task_Type;}
			set{_task_Type = value;}
		}
		public int Need_Count
		{
			get{return _need_Count;}
			set{_need_Count = value;}
		}
		public int Reward_Exp
		{
			get{return _reward_Exp;}
			set{_reward_Exp = value;}
		}
		public int Reward_Money
		{
			get{return _reward_Money;}
			set{_reward_Money = value;}
		}
		public int Reward_Ingot
		{
			get{return _reward_Ingot;}
			set{_reward_Ingot = value;}
		}
		public List<RewardData> RewardItemList
		{
			get{return _rewardItemList;}
			set{_rewardItemList = value;}
		}
		public List<int> RewardSkillList
		{
			get{return _rewardSkillList;}
			set{_rewardSkillList = value;}
		}
		public int Rarity_Level
		{
			get{return _rarity_Level;}
			set{_rarity_Level = value;}
		}
		public int Gift_Level
		{
			get{return _gift_Level;}
			set{_gift_Level = value;}
		}
		
		public static TaskDailyObject Load (SecurityElement element)
		{
			TaskDailyObject taskDailyObject = new TaskDailyObject ();
			taskDailyObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Daily_ID"),""),-1);
			taskDailyObject.Logic_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Logic_ID"),""),-1);
			taskDailyObject.Name = StrParser.ParseStr(element.Attribute ("Name"), "");
			taskDailyObject.Des = StrParser.ParseStr(element.Attribute ("Des"), "");
			taskDailyObject.Task_Category = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Category"),""),-1);
			taskDailyObject.Task_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Type"),""),-1);
			taskDailyObject.Need_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Count"),""),-1);
			taskDailyObject.Reward_Exp = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Exp"),""),-1);
			taskDailyObject.Reward_Money = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Money"),""),-1);
			taskDailyObject.Reward_Ingot = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Ingot"),""),-1);
			
			string  rewardItemList = StrParser.ParseStr(element.Attribute ("Reward_Item"), "");
			if(rewardItemList != null && rewardItemList != "")
			{
				string[] vecs =  rewardItemList.Split(',');
				taskDailyObject.RewardItemList.Clear();
				foreach(string Conditionstring in vecs)
				{
					string []datastring = Conditionstring.Split(':');
					RewardData rewardData = new RewardData();	
					rewardData.ID = StrParser.ParseDecInt(datastring[0],-1);
					rewardData.Num = StrParser.ParseDecInt(datastring[1],-1);
					taskDailyObject.RewardItemList.Add(rewardData);
				}
			}
			
			
			string  RewardskillList = StrParser.ParseStr(element.Attribute ("Reward_Skill"), "");
			if(RewardskillList != null && RewardskillList != "")
			{
				string[] vecs =  rewardItemList.Split('|');
				taskDailyObject.RewardSkillList.Clear();
				foreach(string Conditionstring in vecs)
				{
					int data = StrParser.ParseDecInt(Conditionstring,-1);
					taskDailyObject.RewardSkillList.Add(data);
				}
			}
			taskDailyObject.Rarity_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Rarity_Level"),""),-1);
			taskDailyObject.Gift_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Gift_Level"),""),-1);
			
//			seedObject.Seed_Describe =  StrParser.ParseStr(element.Attribute ("Seed_Describe"), "");
//			seedObject.Seed_Icon =  StrParser.ParseStr(element.Attribute ("Seed_Icon"), "");
//			seedObject.Growth_Icon =  StrParser.ParseStr(element.Attribute ("Growth_Icon"), "");
//			seedObject.Blossom_Icon =  StrParser.ParseStr(element.Attribute ("Blossom_Icon"), "");
//			seedObject.Wilt_Icon =  StrParser.ParseStr(element.Attribute ("Wilt_Icon"), "");
//			seedObject.Grow_Time = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Grow_Time"),""),-1);
//			seedObject.Life_Time = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Life_Time"),""),-1);
//			seedObject._reward_Item = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Item"),""),-1);
//			seedObject._reward_Item_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Item_Count"),""),-1);
			return taskDailyObject;
		}
		protected int _id;
		protected int _logic_ID;
		protected string _name;
		protected string _des;
		protected int _task_Category;
		protected int _task_Type;
		protected int _need_Count;
		protected int _reward_Exp;
		protected int _reward_Money;
		protected int _reward_Ingot;
		protected List<RewardData> _rewardItemList = new List<RewardData> ();
		protected List<int> _rewardSkillList = new List<int> ();
		protected int _rarity_Level;
		protected int _gift_Level;
	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				TaskDailyObject taskDailyObject = TaskDailyObject.Load(childrenElement);
				
				if (!_taskDailyObject.ContainsKey(taskDailyObject.ID))
					_taskDailyObject[taskDailyObject.ID] = taskDailyObject;
			}
		}
		return true;
	}

	public TaskDailyObject GetTaskDailyObjectByID(int iTaskID)
	{
		foreach(TaskDailyObject obj in _taskDailyObject.Values)
		{
			if(obj.ID == iTaskID )
			{
				return obj;
			}
		}
		return null;
	}
		
	protected Dictionary<int, TaskDailyObject> _taskDailyObject = new Dictionary<int, TaskDailyObject>();
}
