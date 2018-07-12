using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class TouchClass
{
	public  string TouchWord;
	public  string TouchSound;
}
public class TouchGirlConfig : ConfigBase
{

	public class TouchGirlObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public int GirlID
		{
			get{return _girlID;}
			set{_girlID = value;}
		}
		public int TouchType
		{
			get{return _touchType;}
			set{_touchType = value;}
		}
		public string  TouchWord
		{
			get{return _touchWord;}
			set{_touchWord = value;}
		}
		public string  TouchSound
		{
			get{return _touchSound;}
			set{_touchSound = value;}
		}

		public static TouchGirlObject Load (SecurityElement element)
		{

			TouchGirlObject touchGirlObject = new  TouchGirlObject();		
			touchGirlObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("TouchID"),""),-1);
			touchGirlObject.GirlID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GirlID"),""),-1);
			touchGirlObject.TouchType = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("TouchType"),""),-1);
			touchGirlObject.TouchWord =  StrParser.ParseStr(element.Attribute ("TouchWord"), "");
			touchGirlObject.TouchSound =  StrParser.ParseStr(element.Attribute ("TouchSound"), "");
			return touchGirlObject;
		}
		
		protected int _id;
		protected int _girlID;
		protected int _touchType;
		protected string _touchWord;
		protected string _touchSound;
	

			

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				TouchGirlObject touchGirlObject = TouchGirlObject.Load(childrenElement);
				
				if (!_touchGirlObjectDict.ContainsKey(touchGirlObject.ID))
					_touchGirlObjectDict[touchGirlObject.ID] = touchGirlObject;
			}
		}
		return true;
	}
	public List<TouchClass> GetWordByGrilIDandPlaceID(long GirlID,int TouchType)
	{
		List<TouchClass > Temp = new List<TouchClass>(); 
		foreach(TouchGirlObject data in _touchGirlObjectDict.Values)
		{
			if(data.GirlID == GirlID && data.TouchType == TouchType)
			{
				TouchClass TempTouchClass = new TouchClass(); 
				TempTouchClass.TouchWord = data.TouchWord;
				TempTouchClass.TouchSound = data.TouchSound;
				Temp.Add(TempTouchClass);
			}
		}
		return Temp;
	}
	protected Dictionary<int, TouchGirlObject> _touchGirlObjectDict = new Dictionary<int, TouchGirlObject> ();
}
