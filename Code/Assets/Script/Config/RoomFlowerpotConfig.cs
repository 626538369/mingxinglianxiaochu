using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class RoomFlowerpotConfig : ConfigBase
{
	public class SeedObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public string Seed_Describe
		{
			get{return _seed_Describe;}
			set{_seed_Describe = value;}
		}
		public string  Seed_Icon
		{
			get{return _seed_Icon;}
			set{_seed_Icon = value;}
		}
		public string  Growth_Icon
		{
			get{return _growth_Icon;}
			set{_growth_Icon = value;}
		}
		public string Blossom_Icon
		{
			get{return _blossom_Icon;}
			set{_blossom_Icon = value;}
		}
		public string Wilt_Icon
		{
			get{return _wilt_Icon;}
			set{_wilt_Icon = value;}
		}
		public int Grow_Time
		{
			get{return _grow_Time;}
			set{_grow_Time = value;}
		}
		public int Life_Time
		{
			get{return _life_Time;}
			set{_life_Time = value;}
		}
		public int Reward_Item
		{
			get{return _reward_Item;}
			set{_reward_Item = value;}
		}
		public int Reward_Item_Count
		{
			get{return _reward_Item_Count;}
			set{_reward_Item_Count = value;}
		}
		public int Need_Room_Lv
		{
			get{return _need_Room_Lv;}
			set{_need_Room_Lv = value;}
		}
		
		
		public static SeedObject Load (SecurityElement element)
		{
			SeedObject seedObject = new SeedObject();
			seedObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Seed_ID"),""),-1);
			seedObject.Seed_Describe =  StrParser.ParseStr(element.Attribute ("Seed_Describe"), "");
			seedObject.Seed_Icon =  StrParser.ParseStr(element.Attribute ("Seed_Icon"), "");
			seedObject.Growth_Icon =  StrParser.ParseStr(element.Attribute ("Growth_Icon"), "");
			seedObject.Blossom_Icon =  StrParser.ParseStr(element.Attribute ("Blossom_Icon"), "");
			seedObject.Wilt_Icon =  StrParser.ParseStr(element.Attribute ("Wilt_Icon"), "");
			seedObject.Grow_Time = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Grow_Time"),""),-1);
			seedObject.Life_Time = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Life_Time"),""),-1);
			seedObject._reward_Item = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Item"),""),-1);
			seedObject._reward_Item_Count = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Reward_Item_Count"),""),-1);
			seedObject.Need_Room_Lv = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Room_Lv"),""),-1);
		
			return seedObject;
		}
		
		protected int _id;
		protected string _seed_Describe ;
		protected string _seed_Icon;
		protected string _growth_Icon;
		protected string _blossom_Icon  ;     
		protected string _wilt_Icon;
		protected int _grow_Time;
		protected int _life_Time;
		protected int _reward_Item;
		protected int _reward_Item_Count ;
		protected int _need_Room_Lv;
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				SeedObject seedObject = SeedObject.Load(childrenElement);
				
				if (!_seedObjectDict.ContainsKey(seedObject.ID))
					_seedObjectDict[seedObject.ID] = seedObject;
			}
		}
		return true;
	}

	public SeedObject GetSeedObjectByID(int iSeedID)
	{
		foreach(SeedObject obj in _seedObjectDict.Values)
		{
			if(obj.ID == iSeedID )
			{
				return obj;
			}
		}
		return null;
	}
		
	protected Dictionary<int, SeedObject> _seedObjectDict = new Dictionary<int, SeedObject>();
}
