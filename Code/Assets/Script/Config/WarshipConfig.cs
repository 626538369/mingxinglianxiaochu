using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class WarshipConfig : ConfigBase
{
	public class WarshipObject
	{
		public int WarshipID
		{
			get{ return _warshipID; }
			set{ _warshipID = value;}
		}	
		public int TypeID
		{
			get{ return _TypeID; }
			set{ _TypeID = value;}
		}
				
		public string Name
		{
			get{ return _Name; }
			set{ _Name = value;}
		}
		
		public int Rare
		{
			get{ return _Rare; }
			set{ _Rare = value;}
		}
		
		public int Rank
		{
			get{ return _Rank; }
			set{ _Rank = value;}
		}
				
		public int Evolution_Level
		{
			get{ return _Evolution_Level; }
			set{ _Evolution_Level = value;}
		}

		public int Exp
		{
			get{ return _Exp;}
			set{_Exp = value;}
		}

		public string Fans_Icon
		{
			get{return _Fans_Icon;}
			set{_Fans_Icon = value;}
		}
		
		public string Art_Describe
		{
			get{return _Art_Describe;}
			set{_Art_Describe = value;}
		}
		
		public string Fans_Say
		{
			get{ return _Fans_Say; }
			set{ _Fans_Say = value;}
		}
		
				
		public static WarshipObject Load (SecurityElement element)
		{
			WarshipObject warshipObject = new WarshipObject();

			warshipObject.WarshipID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("WarShip_ID"),""),-1);
			warshipObject.TypeID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Type_ID"),""),-1);
			warshipObject.Name = StrParser.ParseStr(element.Attribute("Name"),"");
			warshipObject.Rank = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Rank"),""),-1);
			warshipObject.Rare= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Rare"),""),-1);
			warshipObject.Evolution_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Evolution_Level"),""),-1);
			warshipObject.Exp = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Exp"),""),-1);
			warshipObject.Fans_Icon = StrParser.ParseStr(element.Attribute("Fans_Icon"),"");
			warshipObject.Art_Describe = StrParser.ParseStr(element.Attribute("Art_Describe"),"");
			warshipObject.Fans_Say = StrParser.ParseStr(element.Attribute("Fans_Say"),"");
			return warshipObject;
		}		
		protected int _warshipID;
		protected int _TypeID;
		protected string _Name;
		protected int _Rare;
		protected int _Rank;
		protected int _Evolution_Level;		
		protected int _Exp;
		protected string  _Art_Describe;
		protected string  _Fans_Icon;		
		protected string _Fans_Say;
	}
	
	public static readonly int 	MaxFillSpeedAttr = 3;
	
	/// <summary>
	/// tzz added for fill speed
	/// Gets the actual fill speed.
	/// </summary>
	/// <returns>
	/// The actual fill speed.
	/// </returns>
	/// <param name='xmlFillSpeed'>
	/// Xml fill speed.
	/// </param>
	public static int GetActualFillSpeed(int xmlFillSpeed){
		int tMaxSpeed = 1;	
			
		for(int i = 2;i <= MaxFillSpeedAttr;i++){
			tMaxSpeed *= i;
		}
		
		return tMaxSpeed / xmlFillSpeed;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				WarshipObject warshipObject = WarshipObject.Load(childrenElement);
				
				if (!_warshipObjectDict.ContainsKey(warshipObject.WarshipID))
					_warshipObjectDict[warshipObject.WarshipID] = warshipObject;
			}
		}

		return true;
	}
	
	public bool GetWarshipElement(int warshipID, out WarshipObject element)
	{
		element = null;
		
		if ( !_warshipObjectDict.TryGetValue(warshipID, out element) )
			return false;
		
		return true;
	}
	
	public string GetgirlobjectByID(int Id)
	{
		return _girlStringDict[Id];
	}
	
	public WarshipObject GetWarshipObjectByID(int iSWarshipID)
	{
		foreach(WarshipObject obj in _warshipObjectDict.Values)
		{
			if(obj.WarshipID == iSWarshipID )
			{
				return obj;
			}
		}
		return null;
	}
	public WarshipObject GetWarshipObjectByName(string iSName)
	{
		foreach(WarshipObject obj in _warshipObjectDict.Values)
		{
			if(obj.Name== iSName )
			{
				return obj;
			}
		}
		return null;
	}

	public Dictionary<int, WarshipObject> getWarshipDic()
	{
		return _warshipObjectDict;
	}

	protected Dictionary<int, WarshipObject> _warshipObjectDict = new Dictionary<int, WarshipObject>();
	
	public static Dictionary<int, string> _girlStringDict = new Dictionary<int, string>();
}
