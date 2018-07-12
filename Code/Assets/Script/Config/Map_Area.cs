using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class Map_Area : ConfigBase
{


	public class Map_AreaObject
	{
		public int Area_ID
		{
			get{return _AreaId;}
			set{_AreaId = value;}
		}
		public List<int> Citys
		{
			get{return _Citys;}
			set{_Citys = value;}
		}
		public int Populace
		{
			get{return _Populace;}
			set{_Populace = value;}
		}
		public string  Publication
		{
			get{return _Publication;}
			set{_Publication = value;}
		}
		public int Lv_Consume
		{
			get{return _Lv_Consume;}
			set{_Lv_Consume = value;}
		}
		public int Lv_Populace
		{
			get{return _Lv_Populace;}
			set{_Lv_Populace = value;}
		}
		public int Map_Index
		{
			get{return _Map_Index;}
			set{_Map_Index = value;}
		}
		public string Area_Name
		{
			get{return _Area_Name;}
			set{_Area_Name = value;}
		}
		public string Area_Pic
		{
			get{return _Area_Pic;}
			set{_Area_Pic = value;}
		}
	//	<Item Area_ID="1" Citys="1|2|3|4" Populace="1370536875" Publication="1:0.2,2:0.3,3:0.4" Lv_Consume="3" Map_Index="2" Area_Name="中国" Area_Pic=""/>	
		public static Map_AreaObject Load (SecurityElement element)
		{
			Map_AreaObject mapareaObject = new Map_AreaObject();
			mapareaObject.Area_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Area_ID"),""),-1);
//			mapareaObject.Citys =  StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Citys"), ""),-1);
			string  Condition = StrParser.ParseStr(element.Attribute ("Citys"), "");
			if("" != Condition && null != Condition)
			{
				string[] vecs =  Condition.Split('|');
				mapareaObject._Citys.Clear();
				foreach(string Conditionstring in vecs)
				{	
					int temp = StrParser.ParseDecInt(Conditionstring,-1);
					mapareaObject._Citys.Add(temp);
				}
			}
			mapareaObject.Populace = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Populace"), ""),-1);
			mapareaObject.Publication =  StrParser.ParseStr(element.Attribute ("Publication"), "");
			mapareaObject.Lv_Consume = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Lv_Consume"), ""),-1);
			mapareaObject.Lv_Populace = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Lv_Populace"), ""),-1);			
			mapareaObject.Map_Index =  StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Map_Index"), ""),-1);
			mapareaObject.Area_Name = StrParser.ParseStr(element.Attribute("Area_Name"),"");
			mapareaObject.Area_Pic = StrParser.ParseStr(element.Attribute("Area_Pic"),"");
			
			return mapareaObject;
		}
		
		protected int _AreaId;
		protected List<int> _Citys = new List<int>();
		protected int _Populace;
		protected string _Publication;
		protected int _Lv_Consume; 
		protected int _Lv_Populace;		
		protected int _Map_Index;
		protected string _Area_Name;
		protected string _Area_Pic;
	
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				Map_AreaObject mapareaObject = Map_AreaObject.Load(childrenElement);
				
				if (!_Map_AreaObjectDict.ContainsKey(mapareaObject.Area_ID))
					_Map_AreaObjectDict[mapareaObject.Area_ID] = mapareaObject;
			}
		}
		return true;
	}

	public Map_AreaObject GetMap_AreaObjectByID(int iSMap_AreaID)
	{
		foreach(Map_AreaObject obj in _Map_AreaObjectDict.Values)
		{
			if(obj.Area_ID == iSMap_AreaID )
			{
				return obj;
			}
		}
		return null;
	}
	public Map_AreaObject GetMap_AreaObjectByCityID(int iSMap_CityID)
	{
		foreach(Map_AreaObject obj in _Map_AreaObjectDict.Values)
		{
			if(obj.Citys.Contains(iSMap_CityID))
			{
				return obj;
			}
		}
		return null;
	}
		
	protected Dictionary<int, Map_AreaObject> _Map_AreaObjectDict = new Dictionary<int, Map_AreaObject>();
}

