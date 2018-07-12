using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class FlashConfig : ConfigBase {
	
	public class FlashObject
	{
		public int FlashID
		{
			get{ return flashId;}
			set{ flashId = value;}
		}
	
		public string FlashPrefabName
		{
			get{ return flashName;}
			set{ flashName = value;}
		}
		public static FlashObject Load (SecurityElement element)
		{
		FlashObject flashObject = new FlashObject();
		flashObject.FlashID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Flash_ID"),""),-1);
		flashObject.FlashPrefabName =  StrParser.ParseStr(element.Attribute ("FlashPrefabName"), "");
		return flashObject;
		}
		protected int flashId;
		protected string flashName;
	
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				
				FlashObject flash = FlashObject.Load(childrenElement);
				if(! FlashObjectDic.ContainsKey(flash.FlashID))
				{
					FlashObjectDic[flash.FlashID] = flash;
				}
			}
		}
		return true;
	}
	
	public bool GetFlashObj(int flashID,out FlashObject flashName)
	{
		flashName = null;
		foreach (FlashObject flash in FlashObjectDic.Values)
		{
			if (flash.FlashID == flashID)
			{
				flashName = flash;
				return true;
			}
		}
		
		return false;
		
	}
	

	Dictionary<int,FlashObject> FlashObjectDic = new Dictionary<int, FlashObject>(); 

}