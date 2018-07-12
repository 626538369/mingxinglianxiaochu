using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class RoomConfig : ConfigBase
{
	public class RoomNeedData
	{
		public int NeedID;
		public int NeedNum;
	}
	public class RoomObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public string Room_Describe
		{
			get{return _room_Describe;}
			set{_room_Describe = value;}
		}
		public List<RoomNeedData> NeedList
		{
			get{return _needList;}
			set{_needList = value;}
		}
		public int Need_Role_Rank
		{
			get{return _need_Role_Rank;}
			set{_need_Role_Rank = value;}
		}
		public int Effect_Energy
		{
			get{return _effect_Energy;}
			set{_effect_Energy = value;}
		}
		public int Effect_LovePoint
		{
			get{return _effect_LovePoint;}
			set{_effect_LovePoint = value;}
		}
		public int Flowerpot_Count
		{
			get{return _flowerpot_Count;}
			set{_flowerpot_Count = value;}
		}
			public int FunctionId
		{
			get{return _functionId;}
			set{_functionId = value;}
		}

		
		
		public static RoomObject Load (SecurityElement element)
		{
			RoomObject roomObject = new RoomObject();
			roomObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Room_LV"),""),-1);
			roomObject.Room_Describe =  StrParser.ParseStr(element.Attribute ("Room_Describe"), "");
			string  Condition = StrParser.ParseStr(element.Attribute ("Lv_Up_Condition"), "");
			if(Condition != null && Condition != "")
			{
				string[] vecs =  Condition.Split(',');
				roomObject._needList.Clear();
				foreach(string Conditionstring in vecs)
				{
					string []datastring = Conditionstring.Split(':');
					RoomNeedData roomNeedData = new RoomNeedData();	
					roomNeedData.NeedID = StrParser.ParseDecInt(datastring[0],-1);
					roomNeedData.NeedNum = StrParser.ParseDecInt(datastring[1],-1);
					roomObject._needList.Add(roomNeedData);
				}
			}
			roomObject._need_Role_Rank = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Role_Rank"),""),-1);
			roomObject._effect_Energy = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Effect_Energy"),""),-1);
			roomObject._effect_LovePoint = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Effect_LovePoint"),""),-1);
			roomObject._flowerpot_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Flowerpot_Count"),""),-1);
			roomObject._functionId = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("functionId"),""),-1);

			return roomObject;
		}
		
		protected int _id;
		protected string  _room_Describe;
		//protected string _lv_Up_Condition;
		protected List<RoomNeedData> _needList = new List<RoomNeedData> ();
		protected int _need_Role_Rank;
		protected int _effect_Energy;
		protected int _effect_LovePoint;
		protected int _flowerpot_Count;
		protected int _functionId;
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				RoomObject roomObject = RoomObject.Load(childrenElement);
				
				if (!_roomObjectDict.ContainsKey(roomObject.ID))
					_roomObjectDict[roomObject.ID] = roomObject;
			}
		}
		return true;
	}

	public RoomObject GetRoomObjectByRoomLev(int iRoomLev)
	{
		foreach(RoomObject obj in _roomObjectDict.Values)
		{
			if(obj.ID == iRoomLev )
			{
				return obj;
			}
		}
		return null;
	}
	public int GetMaxRoomLev()
	{
		return _roomObjectDict.Count;
	}
		
	protected Dictionary<int, RoomObject> _roomObjectDict = new Dictionary<int, RoomObject>();
}
