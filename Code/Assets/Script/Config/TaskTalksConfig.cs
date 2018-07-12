using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class TaskDialog : ConfigBase
{
	public class TaskDialogObject
	{
		public int talkIter;
		public int talkTpye;
		public string talkBgPicture;
		public string talkModlePrefab;
		public int girlID1;
		public string girlAnimation1;
		public int girlID2;
		public string girlAnimation2;
		public int cgGroundID;
		public string talkCaptionName;
		public string talkContent;
		public string talkMusic;
	}
	
	public override bool Load (SecurityElement element)
	{
		//Debug.Log("-----------------------TaskDialogConfig-------------KaiShi------------------");
		if(element.Children !=null)
		{
			int talkID = 0;
			foreach(SecurityElement childrenElement in element.Children)
			{
				int key = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("TalkID"),""), -1);
				if (talkID != key)
				{
					List<TaskDialogObject> taskTalkList = new List<TaskDialogObject>();
					
					_mTaskDialogDic[key] = taskTalkList;
					talkID = key;
				}
				TaskDialogObject talkObject = new TaskDialogObject();
				
				talkObject.talkIter = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Num"),""), -1);
				talkObject.talkTpye = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Type"),""), -1);
				talkObject.talkBgPicture = StrParser.ParseStr(childrenElement.Attribute("Picture"),"");
				talkObject.talkModlePrefab = StrParser.ParseStr(childrenElement.Attribute("Model_SceneID"),"");
				talkObject.girlID1 =  StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Character_ID1"),""), -1);
				talkObject.girlAnimation1 = StrParser.ParseStr(childrenElement.Attribute("Act1"),"");
				talkObject.girlID2 =  StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Character_ID2"),""), -1);
				talkObject.girlAnimation2 = StrParser.ParseStr(childrenElement.Attribute("Act2"),"");
				talkObject.cgGroundID =  StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("CGID"),""), -1);
				talkObject.talkCaptionName = StrParser.ParseStr(childrenElement.Attribute("Name"),"");
				talkObject.talkContent = StrParser.ParseStr(childrenElement.Attribute("Dialog"),"");
				talkObject.talkMusic = StrParser.ParseStr(childrenElement.Attribute("Sound"),"");
				_mTaskDialogDic[key].Add(talkObject);
				
			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}
	
	public bool GeTTaskDialogDic(out Dictionary<int,  List<TaskDialogObject>> mTaskTalksIDDic)
	{
		mTaskTalksIDDic = _mTaskDialogDic;
		return true;
	}
	
	protected Dictionary<int, List<TaskDialogObject>> _mTaskDialogDic = new Dictionary<int, List<TaskDialogObject>> ();
}
