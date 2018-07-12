using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
//<Item News_ID="2" Area_ID="1" Title="北京" Content="北京" Publication="1:02" Prob="0"/>
public class Map_News : ConfigBase
{


	public class Map_NewsObject
	{
		public int News_ID
		{
			get{return _News_ID;}
			set{_News_ID = value;}
		}
		public int Area_ID
		{
			get{return _Area_ID;}
			set{_Area_ID = value;}
		}
		
		public string  Title
		{
			get{return _Title;}
			set{_Title = value;}
		}
		public string Content
		{
			get{return _Content;}
			set{_Content = value;}
		}
		public string Publication
		{
			get{return _Publication;}
			set{_Publication = value;}
		}
		public int Prob
		{
			get{return _Prob;}
			set{_Prob = value;}
		}
		
		//<Item News_ID="2" Area_ID="1" Title="北京" Content="北京" Publication="1:02" Prob="0"/>
	//	<Item Area_ID="1" Citys="1|2|3|4" Populace="1370536875" Publication="1:0.2,2:0.3,3:0.4" Lv_Consume="3" Map_Index="2" Area_Name="中国" Area_Pic=""/>	
		public static Map_NewsObject Load (SecurityElement element)
		{
			Map_NewsObject mapnewsObject = new Map_NewsObject();
			mapnewsObject.News_ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("News_ID"),""),-1);
			mapnewsObject.Area_ID =  StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute ("Area_ID"), ""),-1);
			mapnewsObject.Title = StrParser.ParseStr(element.Attribute ("Title"), "");
		
			mapnewsObject.Content = StrParser.ParseStr(element.Attribute ("Content"), "");
			mapnewsObject.Publication =  StrParser.ParseStr(element.Attribute ("Publication"), "");
			mapnewsObject.Prob = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Prob"),""),-1);
			
			
			return mapnewsObject;
		}
		
		protected int _News_ID;
		protected int _Area_ID ;
		protected string _Title;
		protected string _Content;
		protected string _Publication  ;     
		protected int _Prob;
		
	
		


	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				Map_NewsObject mapnewsObject = Map_NewsObject.Load(childrenElement);
				
				if (!_Map_NewsObjectDict.ContainsKey(mapnewsObject.News_ID))
					_Map_NewsObjectDict[mapnewsObject.News_ID] = mapnewsObject;
			}
		}
		return true;
	}

	public Map_NewsObject GetMap_NewsObjectByID(int iSMap_NewsID)
	{
		foreach(Map_NewsObject obj in _Map_NewsObjectDict.Values)
		{
			if(obj.News_ID == iSMap_NewsID )
			{
				return obj;
			}
		}
		return null;
	}
		
	protected Dictionary<int, Map_NewsObject> _Map_NewsObjectDict = new Dictionary<int, Map_NewsObject>();
}


