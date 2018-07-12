using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class MapFlyConfig : ConfigBase
{


	public class MapFlyObject
	{
		public int Task_ID
		{
			get{return _Task_ID;}
			set{_Task_ID = value;}
		}
		public int City
		{
			get{return _City;}
			set{_City = value;}
		}

		public int Area
		{
			get{return _Area;}
			set{_Area = value;}
		}
		
		
		
		
		public static MapFlyObject Load (SecurityElement element)
		{
			MapFlyObject mapFlyObject = new MapFlyObject();
			mapFlyObject.Task_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Task_ID"),""),-1);

			mapFlyObject.City = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("City"), ""),-1);
			mapFlyObject.Area = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Area"), ""),-1);
			
			return mapFlyObject;
		}
		
		protected int _Task_ID;
		protected int _City;
		protected int _Area;

	
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				MapFlyObject mapFlyObject = MapFlyObject.Load(childrenElement);
				
				if (!_MapFlyObjectDict.ContainsKey(mapFlyObject.Task_ID))
					_MapFlyObjectDict[mapFlyObject.Task_ID] = mapFlyObject;
			}
		}
		return true;
	}

	public MapFlyObject GetMapFlyObjectByID(int iSTask_ID)
	{
		foreach(MapFlyObject obj in _MapFlyObjectDict.Values)
		{
			if(obj.Task_ID == iSTask_ID )
			{
				return obj;
			}
		}
		return null;
	}
	public MapFlyObject GetMapFlyObjectByCityID(int iSMap_CityID)
	{
		foreach(MapFlyObject obj in _MapFlyObjectDict.Values)
		{
			if(obj.City==iSMap_CityID)
			{
				return obj;
			}
		}
		return null;
	}
		
	public Dictionary<int, MapFlyObject> _MapFlyObjectDict = new Dictionary<int, MapFlyObject>();
}


