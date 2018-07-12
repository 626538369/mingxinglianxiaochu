using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Task : ConfigBase
{
	public class TaskObject
	{
		public int Task_ID; 
		public string Name;
		public int Talk_ID;
		public string Task_Desc;
		public int City_ID;
		public int Task_Category; // 任务类别; 
		public int Task_Type;//任务类型; 
		public int Publish_Type;//发布类型;
		public int Artist_ID;//艺术家的ID; 
		public int Is_Branch;//为分公司; 
		public int Parent_Task_ID;//父任务的ID; 
		public int Next_Task_ID;//下一个任务ID; 
		
		public string Require_Condition;//要求的条件; 
		public string Visible_Condition;//可见条件; 
		public int Need_Energy;//需要能量; 
		public int Need_Acting;//需要代理; 
		public int Need_Score;//需要的分数;
		public int Progress_Count;//进度计数; 
		public int Can_Use_Items;//可以使用的物品; 
		public int Is_Single;	//单人; 
		public int Need_Friend_Sex;//需要朋友的行为; 
		
		public string Posture_Group;//姿势组; 
		public string Male_Appearance_Effect;//男性外观效果; 
		public string Female_Appearance_Effect;//女貌的影响;
		public int Max_Dress_Style_Score;//最大着装风格得分; 
		public int Can_Invite_Npc;//可以邀请的NPC; 
		public string Rewards;// 奖励; 
		public int CountDownTime;
		public string Type_Icon;
		public string Sex_Icon;
		public string Tex_Background;
		public string Scene_Background;
		public string CameraAnimition;
		public string CharacterOneAnimition;
		public string CharacterTwoAnimition;
		
		public string Style_Effect;//风格的影响; 
		public int Style_Percent;//风格百分比; 
        public string Material_Effect;//重大影响; 
		public int Material_Percent;//材料百分比;
		public string Theme_Effect;//主题效果;
		public int Theme_Percent;//主题百分比; 
		public string Dress_Part_Effect;//服装的一部分影响; 
		public int Dress_Part_Percent;//礼服部分的百分比;
		
		public string MusicName;

	}
	
	private bool LoadItemElement(SecurityElement element, out TaskObject itemElement)
	{
		itemElement = new TaskObject();
		string attribute = element.Attribute("Task_ID");
		if (attribute != null)
			itemElement.Task_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Name");
		if (attribute != null)
			itemElement.Name = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Talk_ID");
		if (attribute != null)
		itemElement.Talk_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Task_Desc");
		if (attribute != null)
			itemElement.Task_Desc = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("City_ID");
		if (attribute != null)
		itemElement.City_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Task_Category");
		if (attribute != null)
		itemElement.Task_Category = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Task_Type");
		if (attribute != null)
		itemElement.Task_Type = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Publish_Type");
		if (attribute != null)
		itemElement.Publish_Type = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Artist_ID");
		if (attribute != null)
		itemElement.Artist_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Is_Branch");
		if (attribute != null)
		itemElement.Is_Branch = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Parent_Task_ID");
		if (attribute != null)
		itemElement.Parent_Task_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Next_Task_ID");
		if (attribute != null)
		itemElement.Next_Task_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Require_Condition");
		if (attribute != null)
			itemElement.Require_Condition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Visible_Condition");
		if (attribute != null)
			itemElement.Visible_Condition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Need_Energy");
		if (attribute != null)
			itemElement.Need_Energy = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Need_Acting");
		if (attribute != null)
			itemElement.Need_Acting = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Need_Score");
		if (attribute != null)
			itemElement.Need_Score = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Progress_Count");
		if (attribute != null)
			itemElement.Progress_Count = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Can_Use_Items");
		if (attribute != null)
			itemElement.Can_Use_Items = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Is_Single");
		if (attribute != null)
			itemElement.Is_Single = StrParser.ParseDecInt(attribute, 0);	
		attribute = element.Attribute("Need_Friend_Sex");
		if (attribute != null)
			itemElement.Need_Friend_Sex = StrParser.ParseDecInt(attribute, 0);	
		attribute = element.Attribute("Posture_Group");
		if (attribute != null)
			itemElement.Posture_Group = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Male_Appearance_Effect");
		if (attribute != null)
			itemElement.Male_Appearance_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Female_Appearance_Effect");
		if (attribute != null)
			itemElement.Female_Appearance_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Max_Dress_Style_Score");
		if (attribute != null)
			itemElement.Max_Dress_Style_Score = StrParser.ParseDecInt(attribute, 0);	
		attribute = element.Attribute("Rewards");
		if (attribute != null)
			itemElement.Rewards = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Time");
		if (attribute != null)
			itemElement.CountDownTime = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type_Icon");
		if (attribute != null)
			itemElement.Type_Icon = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Sex_Icon");
		if (attribute != null)
			itemElement.Sex_Icon = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Tex_Background");
		if (attribute != null)
			itemElement.Tex_Background = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Scene_Background");
		if (attribute != null)
			itemElement.Scene_Background = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("CameraAnimition");
		if (attribute != null)
			itemElement.CameraAnimition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("CharacterOneAnimition");
		if (attribute != null)
			itemElement.CharacterOneAnimition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("CharacterTwoAnimition");
		if (attribute != null)
			itemElement.CharacterTwoAnimition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("MusicName");
		if (attribute != null)
			itemElement.MusicName = StrParser.ParseStr(attribute, "");
		
		
		attribute = element.Attribute("Style_Effect");
		if (attribute != null)
			itemElement.Style_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Style_Percent");
		if (attribute != null)
			itemElement.Style_Percent = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Material_Effect");
		if (attribute != null)
			itemElement.Material_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Material_Percent");
		if (attribute != null)
			itemElement.Material_Percent = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Theme_Effect");
		if (attribute != null)
			itemElement.Theme_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Theme_Percent");
		if (attribute != null)
			itemElement.Theme_Percent = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Dress_Part_Effect");
		if (attribute != null)
			itemElement.Dress_Part_Effect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Dress_Part_Percent");
		if (attribute != null)
			itemElement.Dress_Part_Percent = StrParser.ParseDecInt(attribute, 0);
		return true;
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
					TaskObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mTaskObjectDic[itemElement.Task_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
// 	public override bool Load (SecurityElement element)
//	{
//		if(element.Children !=null)
//		{
//			foreach(SecurityElement childrenElement in element.Children)
//			{
//				TaskObject taskObject = new TaskObject();
//				taskObject.Talk_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_ID"),""),-1);
//				taskObject.Name = StrParser.ParseStr(element.Attribute("Name"),"");
//				taskObject.Talk_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Talk_ID"),""),-1);
//				taskObject.Task_Desc = StrParser.ParseStr(element.Attribute("Task_Desc"),"");
//				taskObject.City_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("City_ID"),""),-1);
//				taskObject.Task_Category = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Category"),""),-1);
//				taskObject.Task_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_Type"),""),-1);
//				taskObject.Publish_Type = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Publish_Type"),""),-1);
//				taskObject.Artist_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Artist_ID"),""),-1);
//				taskObject.Is_Branch = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Is_Branch"),""),-1);
//				taskObject.Parent_Task_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Parent_Task_ID"),""),-1);
//				taskObject.Next_Task_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Next_Task_ID"),""),-1);
//				taskObject.Require_Condition = StrParser.ParseStr(element.Attribute("Require_Condition"),"");
//				taskObject.Visible_Condition = StrParser.ParseStr(element.Attribute("Visible_Condition"),"");
//				taskObject.Need_Energy = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Energy"),""),-1);
//				taskObject.Need_Acting = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Acting"),""),-1);
//				taskObject.Need_Score = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Score"),""),-1);
//				taskObject.Progress_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Progress_Count"),""),-1);
//				taskObject.Can_Use_Items = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Can_Use_Items"),""),-1);
//				taskObject.Is_Single = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Is_Single"),""),-1);
//				taskObject.Need_Friend_Sex = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Friend_Sex"),""),-1); 
//				taskObject.Posture_Group = StrParser.ParseStr(element.Attribute("Posture_Group"),"");
//				taskObject.Style_Effect = StrParser.ParseStr(element.Attribute("Style_Effect"),"");
//				taskObject.Style_Percent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Style_Percent"),""),-1); 
//				taskObject.Material_Effect = StrParser.ParseStr(element.Attribute("Material_Effect"),"");
//				taskObject.Material_Percent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Material_Percent"),""),-1); 
//				taskObject.Theme_Effect = StrParser.ParseStr(element.Attribute("Theme_Effect"),"");
//				taskObject.Theme_Percent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Theme_Percent"),""),-1); 
//				taskObject.Dress_Part_Effect = StrParser.ParseStr(element.Attribute("Dress_Part_Effect"),"");
//				taskObject.Dress_Part_Percent = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Dress_Part_Percent"),""),-1); 
//				taskObject.Male_Appearance_Effect = StrParser.ParseStr(element.Attribute("Male_Appearance_Effect"),"");
//				taskObject.Female_Appearance_Effect = StrParser.ParseStr(element.Attribute("Female_Appearance_Effect"),"");
//				taskObject.Max_Dress_Style_Score = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Max_Dress_Style_Score"),""),-1); 
//				taskObject.Can_Invite_Npc = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Can_Invite_Npc"),""),-1); 
//				taskObject.Rewards = StrParser.ParseStr(element.Attribute("Rewards"),"");
//				
//				_mTaskObjectDic[taskObject.Task_ID] = taskObject;
//			}
//			return true;
//		}else
//		{
//			return false;
//		}
//		
//		return true;
//	}
		
	
	public bool GetTaskObject(int taskID, out TaskObject taskObject)
	{
		taskObject = null;
		
		if (!_mTaskObjectDic.TryGetValue(taskID, out taskObject))
		{
			return false;
		}
		return true;
	}
	
//	public bool GetTaskObject(int taskID, out TaskObject taskObject)
//	{
//		taskObject = null;
//		
//		if (!_mTaskObjectDic.ContainsKey(taskID))
//			return false;
//		
//		taskObject = _mTaskObjectDic[taskID];
//		return true;
//	}
	

	protected Dictionary<int, TaskObject> _mTaskObjectDic = new  Dictionary<int, TaskObject>();
	
//	public static Dictionary<int, int> _taskReNameGirlDict = new Dictionary<int, int>();
	
}
