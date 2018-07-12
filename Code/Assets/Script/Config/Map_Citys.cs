using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Map_Citys : ConfigBase
{

	// <Item Citys_ID="1" Citys_Name="北京"/>
	public class Map_CitysObject
	{
		public int Citys_ID
		{
			get{return _Citys_ID;}
			set{_Citys_ID = value;}
		}
//		
//		public string Citys_Name
//		{
//			get{return _Citys_Name;}
//			set{_Citys_Name = value;}
//		}
//		
		public string Citys_Name
		{
			get{return _Citys_Name;}
			set{_Citys_Name = value;}
		}
		public int Citys_PosX
		{
			get{return _Citys_PosX;}
			set{_Citys_PosX = value;}
		}public int Citys_PosY
		{
			get{return _Citys_PosY;}
			set{_Citys_PosY = value;}
		}public int Citys_PosZ
		{
			get{return _Citys_PosZ;}
			set{_Citys_PosZ = value;}
		}
		public static Map_CitysObject Load (SecurityElement element)
		{
			Map_CitysObject mapcitysObject = new Map_CitysObject();
			mapcitysObject.Citys_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Citys_ID"),""),-1);
			
			mapcitysObject.Citys_Name = StrParser.ParseStr(element.Attribute ("Citys_Name"), "");
		    mapcitysObject.Citys_PosX = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Citys_PosX"),""),-1);
			mapcitysObject.Citys_PosY = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Citys_PosY"),""),-1);
			mapcitysObject.Citys_PosZ = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Citys_PosZ"),""),-1);
			return mapcitysObject;
		}
		
		protected int _Citys_ID;
		protected string _Citys_Name ;
	    protected int _Citys_PosX;
		protected int _Citys_PosY;
		protected int _Citys_PosZ;
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				Map_CitysObject mapcitysObject = Map_CitysObject.Load(childrenElement);
				
				if (!_Map_CitysObjectDict.ContainsKey(mapcitysObject.Citys_ID))
					_Map_CitysObjectDict[mapcitysObject.Citys_ID] = mapcitysObject;
			}
		}
		return true;
	}

	public Map_CitysObject GetMap_CitysObjectByID(int iSMap_CitysID)
	{
		foreach(Map_CitysObject obj in _Map_CitysObjectDict.Values)
		{
			if(obj.Citys_ID == iSMap_CitysID )
			{
				return obj;
			}
		}
		return null;
	}
	
	public string GetCityNameByID(int id)
	{
		foreach(Map_CitysObject data in _Map_CitysObjectDict.Values)
		{
			if(data.Citys_ID == id)
			{
				return data.Citys_Name;
			}
		}
		return null;
	}
		
	protected Dictionary<int, Map_CitysObject> _Map_CitysObjectDict = new Dictionary<int, Map_CitysObject>();
}


