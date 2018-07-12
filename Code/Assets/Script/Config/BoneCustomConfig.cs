using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class BoneCustomConfig : ConfigBase 
{
	public struct BoneItemInfo
	{
		public string boneName;
		public int nAxis;
		public int nPRS; ///Position , Rotation, Scale
		public int nIsWorld;
		public float nValueRange;
		public float nBigValueRange;
	};
	public class CustomElement
	{
		public int nCustomID;
		public string nCustomContent;
		public int nBoneCount;
		public float ValueBegin;
		public List<BoneItemInfo> nBoneList;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		if(element.Children!=null)
		{
			foreach(SecurityElement ChildrenElement in element.Children)
			{
				if(ChildrenElement.Tag=="Item")
				{
					CustomElement itemElement = new CustomElement();
					itemElement.nCustomID = StrParser.ParseDecInt(ChildrenElement.Attribute("CustomID"),-1);
					itemElement.nCustomContent = ChildrenElement.Attribute("CustomContet");
					itemElement.nBoneCount = StrParser.ParseDecInt(ChildrenElement.Attribute("BoneCount"),0);
					itemElement.ValueBegin = StrParser.ParseDecInt(ChildrenElement.Attribute("ValueBegin"),0);
					itemElement.nBoneList = new List<BoneItemInfo>();
					for (int i=0; i<itemElement.nBoneCount; i++)
					{
						BoneItemInfo boneItemInfo = new BoneItemInfo();
						string strIter = "BoneName" + (i+1).ToString();
						boneItemInfo.boneName = ChildrenElement.Attribute(strIter);
						strIter = "Axis" + (i+1).ToString();
						boneItemInfo.nAxis = StrParser.ParseDecInt(ChildrenElement.Attribute(strIter),0);
						strIter = "PRS" + (i+1).ToString();
						boneItemInfo.nPRS = StrParser.ParseDecInt(ChildrenElement.Attribute(strIter),0);
						
						strIter = "IsWorld" + (i+1).ToString();
						if (ChildrenElement.Attribute(strIter) != "")
							boneItemInfo.nIsWorld = StrParser.ParseDecInt(ChildrenElement.Attribute(strIter),0);
						else
							boneItemInfo.nIsWorld = 0;
						
						strIter = "ValueRange" + (i+1).ToString();
						boneItemInfo.nValueRange = StrParser.ParseFloat(ChildrenElement.Attribute(strIter),0);
						
						strIter = "BigValueRange" + (i+1).ToString();
						boneItemInfo.nBigValueRange = StrParser.ParseFloat(ChildrenElement.Attribute(strIter),0);
						
						itemElement.nBoneList.Add(boneItemInfo);
					}
					_mItemElementList[itemElement.nCustomID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}
	
	public bool GetItemElement(int nID, out CustomElement itemElement)
	{
		itemElement = null;
		if(!_mItemElementList.TryGetValue(nID, out itemElement))
			return false;
		return true;
	}
	

	
   private Dictionary<int,CustomElement> _mItemElementList = new Dictionary<int, CustomElement>();

}
