using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class GirlCGConfig : ConfigBase
{
	public class GirlCGObject
	{
		public int CG_ID
		{
			get{return _cgID;}
			set{_cgID = value;}
		}
		public int Girl_ID
		{
			get{return _girlID;}
			set{_girlID = value;}
		}
		public int Talk_ID
		{
			get{return _talk_ID;}
			set{_talk_ID = value;}
		}
		public string CGIcon
		{
			get{return _cgicon;}
			set{_cgicon = value;}
		}
		public static GirlCGObject Load (SecurityElement element)
		{
			GirlCGObject girlcgobject = new GirlCGObject();		
			girlcgobject.CG_ID= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("CG_ID"),""),-1);
			girlcgobject.Girl_ID= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Girl_ID"),""),-1);	
			girlcgobject.Talk_ID= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Talk_ID"),""),-1);
			girlcgobject.CGIcon = StrParser.ParseStr(element.Attribute ("Icon"), "");
			return girlcgobject;
		}
		
		protected int _cgID;
		protected int _girlID;
		protected int _talk_ID;
		protected string _cgicon;

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				GirlCGObject girlcgObject = GirlCGObject.Load(childrenElement);
				
				if (!_girlcgObjectDict.ContainsKey(girlcgObject.CG_ID))
					_girlcgObjectDict[girlcgObject.CG_ID] = girlcgObject;
			}
		}

		return true;
	}
	public GirlCGObject GetGirlCGObjectByID(int _logicID)
	{
		if (!_girlcgObjectDict.ContainsKey(_logicID))
			return null;
		
		return _girlcgObjectDict[_logicID];
	}
	public List<int> GetGirlCgListByGirlID(int GirlID)
	{
		List<int> Temp = new List<int>();
		foreach(GirlCGObject obj in _girlcgObjectDict.Values)
		{
			if(obj.Girl_ID == GirlID)
			{
				Temp.Add(obj.CG_ID);
			}
		}
		return Temp;
	}
	
	protected Dictionary<int, GirlCGObject> _girlcgObjectDict = new Dictionary<int, GirlCGObject>();
}
