using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class PublishConfig : ConfigBase
{
	public class MagazineObj
	{
		public int Publish_ID; 
		public int Style;
		public int Type;
		public int Type_Small;
		public string Type_SmallName;
		public int Talk_ID;
		public string TemplateName;
		public string Background;
		public string Foreground;
		public string IssueNum;
		public string LOGO;
		public string MainAD;
		public string SecondaryAD;
		public string Name;
		public string Retain;
		public Vector3 Position;
		public string Icon;
		public string IconCover;
		public List<int>Talk_IDList;
		public string Publish_Scene;

	}
	
	private bool LoadItemElement(SecurityElement element, out MagazineObj itemElement)
	{
		itemElement = new MagazineObj();
		string attribute = element.Attribute("Publish_ID");
		if (attribute != null)
			itemElement.Publish_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style");
		if (attribute != null)
			itemElement.Style = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type");
		if (attribute != null)
			itemElement.Type = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type_Small");
		if (attribute != null)
			itemElement.Type_Small = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Type_SmallName");
		if (attribute != null)
			itemElement.Type_SmallName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Talk_ID");
		if (attribute != null)
			itemElement.Talk_ID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("TemplateName");
		if (attribute != null)
			itemElement.TemplateName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Background");
		if (attribute != null)
			itemElement.Background = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Foreground");
		if (attribute != null)
			itemElement.Foreground = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("IssueNum");
		if (attribute != null)
			itemElement.IssueNum = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("LOGO");
		if (attribute != null)
			itemElement.LOGO = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("MainAD");
		if (attribute != null)
			itemElement.MainAD = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Name");
		if (attribute != null)
			itemElement.Name = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Retain");
		if (attribute != null)
			itemElement.Retain = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("SecondaryAD");
		if (attribute != null)
			itemElement.SecondaryAD = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Position");
		if (attribute != null)
			itemElement.Position = StrParser.ParseVec3(attribute);
		
		attribute = element.Attribute("Icon");
		if (attribute != null)
			itemElement.Icon = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("IconCover");
		if (attribute != null)
			itemElement.IconCover = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Publish_Scene");
		if (attribute != null)
			itemElement.Publish_Scene = StrParser.ParseStr(attribute, "");
		
		
		
		itemElement.Talk_IDList = new List<int> ();
		for(int i = 0; i < 6; ++i)
		{
			attribute = element.Attribute("Talk_ID" + (i + 1).ToString());
			itemElement.Talk_IDList.Add(StrParser.ParseDecInt(attribute, 0));
		}
		
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
					MagazineObj itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mMagazineObjectDic[itemElement.Publish_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	public string GetPublishName(int iPublishID)
	{
		MagazineObj magazineObj;
		if(_mMagazineObjectDic.TryGetValue(iPublishID,out magazineObj))
			return magazineObj.Type_SmallName;
		return null;
	}
	public string GetName(int iPublishID)
	{
		MagazineObj magazineObj;
		if(_mMagazineObjectDic.TryGetValue(iPublishID,out magazineObj))
			return magazineObj.Name;
		return null;
	}
	
	public bool GetMagazineObject(int taskID, out MagazineObj taskObject)
	{

		taskObject = null;
		
		if (!_mMagazineObjectDic.TryGetValue(taskID, out taskObject))
		{
			return false;
		}
		return true;
	}
	

	

	protected Dictionary<int, MagazineObj> _mMagazineObjectDic = new  Dictionary<int, MagazineObj>();
	

	
}
