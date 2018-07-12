using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class AttrGradeConfig : ConfigBase
{
	public class AttrGradeElement
	{
		public int ID;
		public int AttrType;
		public int AttrGrade;
		public int NeedAttrNum;
		public int ScoreRatio;
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
					AttrGradeElement itemElement;
					if (!Loadg_level_infoElement(childrenElement, out itemElement))
						continue;

					_mAttrGradeElementList[itemElement.ID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}

	private bool Loadg_level_infoElement(SecurityElement element, out AttrGradeElement itemElement)
	{
		itemElement = new AttrGradeElement();

		string attribute = element.Attribute("AttrType");
		if (attribute != null)
			itemElement.AttrType = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("AttrGrade");
		if (attribute != null)
			itemElement.AttrGrade = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("NeedAttrNum");
		if (attribute != null)
			itemElement.NeedAttrNum = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("ScoreRatio");
		if (attribute != null)
			itemElement.ScoreRatio = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("ID");
		if (attribute != null)
			itemElement.ID = StrParser.ParseDecInt(attribute, 0);
		return true;
	}


	public Dictionary<int, AttrGradeElement> GetAttrGradeElementList()
	{
		return _mAttrGradeElementList;
	}

	private Dictionary<int, AttrGradeElement> _mAttrGradeElementList = new Dictionary<int, AttrGradeElement>();
}

