using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class PosConfig : ConfigBase
{
	public class PosObject
	{
		public int Pose_ID;
		public string Pose_Icon;
		public string Pose_Controll;
		public string Pose_CameraPosition;
		public string Pose_CameraRotation;
		public int Pose_CameraView;
	}
	
	private bool LoadItemElement(SecurityElement element, out PosObject itemElement)
	{
		itemElement = new PosObject();
		string attribute = element.Attribute("Pose_ID");
		if (attribute != null)
			itemElement.Pose_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Pose_Icon");
		if (attribute != null)
			itemElement.Pose_Icon = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pose_Controll");
		if (attribute != null)
			itemElement.Pose_Controll = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pose_CameraPosition");
		if (attribute != null)
			itemElement.Pose_CameraPosition = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pose_CameraRotation");
		if (attribute != null)
			itemElement.Pose_CameraRotation = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Pose_CameraView");
		if (attribute != null)
			itemElement.Pose_CameraView = StrParser.ParseDecInt(attribute, 0);
		return true;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Item")
				{
					PosObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mPosObjectDic[itemElement.Pose_ID] = itemElement;
				}
				
			}
			return true;
		}
		return false;
	}

	
	public Dictionary<int, PosObject> GetPosObjectDic()
	{
		return _mPosObjectDic;
	}

	protected Dictionary<int, PosObject> _mPosObjectDic = new  Dictionary<int, PosObject>();
}
