using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class JuryTalk: ConfigBase
{
	public class TalkObject
	{
		public int ID
		{
			get{return _id;}
			set{_id = value;}
		}
		public int JuryID
		{
			get{return _juryID;}
			set{_juryID = value;}
		}
		public string Dialog
		{
			get{return _dialog;}
			set{_dialog = value;}
		}
		public int TypeID
		{
			get{return _type;}
			set{_type = value;}
			
				
		}

		public static TalkObject Load (SecurityElement element)
		{
			TalkObject talkObject = new TalkObject ();
			talkObject.ID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("TalkID"),""),-1);
			talkObject.Dialog =  StrParser.ParseStr(element.Attribute ("Dialogue"), "");
			talkObject.JuryID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("JuryID"),""),-1);
			talkObject.TypeID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("JuryType"),""),-1);
			return talkObject;
		}
		
		protected int _id;
		protected int _juryID;
		protected string _dialog;
		protected int _type;

	}
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				//SeedObject seedObject = SeedObject.Load(childrenElement);
				TalkObject talkobject = TalkObject.Load(childrenElement);
				
				if (!_juryObjectDict.ContainsKey(talkobject.ID))
					_juryObjectDict[talkobject.ID] = talkobject;
				
	
				/*if (! _juryToIDDict.ContainsKey(talkobject.JuryID))
				{
					_juryToIDDict.Add(talkobject.JuryID,new List<int>());
				}
				
				_juryToIDDict[talkobject.JuryID].Add(talkobject.ID);
				*/
			}
		}
		return true;
	}

	public TalkObject GetTalkObjectByID(int ID)
	{
		foreach(TalkObject obj in _juryObjectDict.Values)
		{
			if(obj.ID == ID )
			{
				return obj;
			}
		}
		return null;
	}
	
	public string  GetJuryObjectByID(int judgeID , int judgeType)
	{
		/*List<int> talkIDList ;
		 _juryToIDDict.TryGetValue(judgeID,out talkIDList);
		
		int iter = Random.Range(0,talkIDList.Count);
		*/
		int iter = Random.Range(1,3);
		int ID = judgeID*100+judgeType*10+iter;
		
		TalkObject talkObj = GetTalkObjectByID(ID);
		
		return talkObj.Dialog;
		
	}
		
	protected Dictionary<int, TalkObject> _juryObjectDict = new Dictionary<int, TalkObject>();
	
	protected Dictionary<int, List<int>> _juryToIDDict = new Dictionary<int, List<int>>();
}
