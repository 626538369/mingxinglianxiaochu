using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class MovieDialogConfig : ConfigBase
{
	public class MovieDialogObj
	{
		public int TalkID; 
		public int Num;
		public int Type;
		public string Picture;
		public int Model_SceneID;
		public int Character_ID1;
		public string Act1;
		public int Character_ID2;
		public string Act2;
		public int  CGID;
		public string Name;
		public string Dialog;
		public string Sound;
		public string SoundInfo;
		public string EffectIDHead;
		public string EffectIDTail;


	}
	
	private bool LoadItemElement(SecurityElement element, out MovieDialogObj itemElement)
	{
		itemElement = new MovieDialogObj();
		string attribute = element.Attribute("TalkID");
		if (attribute != null)
			itemElement.TalkID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Num");
		if (attribute != null)
			itemElement.Num = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type");
		if (attribute != null)
			itemElement.Type = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Picture");
		if (attribute != null)
			itemElement.Picture = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Model_SceneID");
		if (attribute != null)
			itemElement.Model_SceneID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Character_ID1");
		if (attribute != null)
			itemElement.Character_ID1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Act1");
		if (attribute != null)
			itemElement.Act1 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Character_ID2");
		if (attribute != null)
			itemElement.Character_ID2 = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Act2");
		if (attribute != null)
			itemElement.Act2 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("CGID");
		if (attribute != null)
			itemElement.CGID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Name");
		if (attribute != null)
			itemElement.Name = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Dialog");
		if (attribute != null)
			itemElement.Dialog = StrParser.ParseStr(attribute, "");
		
		
		attribute = element.Attribute("Sound");
		if (attribute != null)
			itemElement.Sound = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("SoundInfo");
		if (attribute != null)
			itemElement.SoundInfo = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("EffectIDHead");
		if (attribute != null)
			itemElement.EffectIDHead = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("EffectIDTail");
		if (attribute != null)
			itemElement.EffectIDTail = StrParser.ParseStr(attribute, "");
		
		
		return true;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		int i = 0;
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Item")
				{
					MovieDialogObj itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mMovieDialogObjectDic[i] = itemElement;
					++i;
				}
			}
			
			return true;
		}
		return false;
	}
	
	
	public bool GetMovieDialogObject(int taskID, out MovieDialogObj taskObject)
	{

		taskObject = null;
		
		if (!_mMovieDialogObjectDic.TryGetValue(taskID, out taskObject))
		{
			return false;
		}
		return true;
	}
	public List<string> GetDialogueByID(int taskID)
	{
		List<string> Temp = new List<string> ();
		foreach(MovieDialogObj data in _mMovieDialogObjectDic.Values)
		{
			if(data.TalkID == taskID)
			{
				//Temp.Add(data.Name + ":" + data.Dialog);
				Temp.Add( data.Dialog);
			}
		}
		return Temp;
	}
	

	protected Dictionary<int, MovieDialogObj> _mMovieDialogObjectDic = new  Dictionary<int, MovieDialogObj>();
	

	
}
