using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class GirlActionandSoundConfig : ConfigBase
{
	public class GirlActionandSoundObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public int GirID
		{
			get{return _girID;}
			set{_girID = value;}
		}
		public int PartID
		{
			get{return _partID;}
			set{_partID = value;}
		}
		public bool IsAgree
		{
			get{return _isAgree;}
			set{_isAgree = value;}
		}
		public int SoundDelayTime
		{
			get{return _soundDelayTime;}
			set{_soundDelayTime = value;}
		}
		public int HomeLevNeed
		{
			get{return _homeLevNeed;}
			set{_homeLevNeed = value;}
		}
		public int GoodsIDNeed
		{
			get{return _goodsIDNeed;}
			set{_goodsIDNeed = value;}
		}
		public int IntimacyLevNeed
		{
			get{return _intimacyLevNeed;}
			set{_intimacyLevNeed = value;}
		}
		public string SoundName
		{
			get{return _soundName;}
			set{_soundName = value;}
		}
		public string ActionName
		{
			get{return _actionName;}
			set{_actionName = value;}
		}
		public float  Prob
		{
			get{return _prob;}
			set{_prob = value;}
		}
		public static GirlActionandSoundObject Load (SecurityElement element)
		{
			GirlActionandSoundObject girlActionandSoundObject = new GirlActionandSoundObject();		
			girlActionandSoundObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ID"),""),-1);
			girlActionandSoundObject.GirID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GirID"),""),-1);
			girlActionandSoundObject.PartID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("PartID"),""),-1);
			girlActionandSoundObject.IsAgree = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("IsAgree"),""),-1)== 0 ? false : true ;
			girlActionandSoundObject.HomeLevNeed = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("HomeLevNeed"),""),-1);
			girlActionandSoundObject.GoodsIDNeed = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GoodsIDNeed"),""),-1);
			girlActionandSoundObject.IntimacyLevNeed = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("IntimacyLevNeed"),""),-1);
			girlActionandSoundObject.SoundDelayTime = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SoundDelayTime"),""),-1);
			girlActionandSoundObject.SoundName =  StrParser.ParseStr(element.Attribute ("SoundName"), "");
			girlActionandSoundObject.ActionName =  StrParser.ParseStr(element.Attribute ("ActionName"), "");
			girlActionandSoundObject.Prob = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("Prob"),""),-1.0f);
			
			return girlActionandSoundObject;
		}
		
		protected int _id;
		protected int _girID;
		protected int _partID;
		protected bool _isAgree;
		protected int _homeLevNeed;
		protected int _goodsIDNeed;
		protected int _intimacyLevNeed;
		protected int _soundDelayTime;
		protected string _soundName;
		protected string _actionName;
		protected float _prob;
			

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				GirlActionandSoundObject girlActionandSoundObject = GirlActionandSoundObject.Load(childrenElement);
				
				if (!_girlActionandSoundObjectDict.ContainsKey(girlActionandSoundObject.ID))
					_girlActionandSoundObjectDict[girlActionandSoundObject.ID] = girlActionandSoundObject;
			}
		}
		return true;
	}

	public GirlActionandSoundObject GetGirlActionandSound(int GirlID,int PartID,bool bIsAgree)
	{
		foreach(GirlActionandSoundObject obj in _girlActionandSoundObjectDict.Values)
		{
			if(obj.GirID == GirlID && obj.PartID == PartID && obj.IsAgree == bIsAgree )
			{
				return obj;
			}
		}
		return null;
	}
	
	public GirlActionandSoundObject GetGirlActionandSound(int GirlID,int PartID,int IntimacyLev,bool bTouch = true)
	{
		foreach(GirlActionandSoundObject obj in _girlActionandSoundObjectDict.Values)
		{
			if(obj.GirID == GirlID && obj.PartID == PartID)
			{
				if((IntimacyLev >= obj.IntimacyLevNeed) == obj.IsAgree)
					return obj;
			}
		}
		return null;
	}
	public float GetProbByGirlIDAndPartID(int GirlLogicID, int iPartID)
	{
		foreach(GirlActionandSoundObject obj in _girlActionandSoundObjectDict.Values)
		{
			if(obj.GirID == GirlLogicID && obj.PartID == iPartID)
			{
				return  obj.Prob;
			}
		}
		return -1.0f;
	}
	
	protected Dictionary<int, GirlActionandSoundObject> _girlActionandSoundObjectDict = new Dictionary<int, GirlActionandSoundObject>();
}
