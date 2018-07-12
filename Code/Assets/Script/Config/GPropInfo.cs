using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class GPropInfo : ConfigBase
{
	public class PropInfoElement
	{
		public int PropID;
		public string PropName;
		public int PropGrade;
		public string PropEffect;
		public int MagicPoint;
		public string PropDesc;
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
					PropInfoElement itemElement;
					if (!Loadg_level_infoElement(childrenElement, out itemElement))
						continue;

					_mPropInfoElementList[itemElement.PropID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}

	private bool Loadg_level_infoElement(SecurityElement element, out PropInfoElement itemElement)
	{
		itemElement = new PropInfoElement();

		string attribute = element.Attribute("PropID");
		if (attribute != null)
			itemElement.PropID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("PropName");
		if (attribute != null)
			itemElement.PropName = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("PropGrade");
		if (attribute != null)
			itemElement.PropGrade = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("PropEffect");
		if (attribute != null)
			itemElement.PropEffect = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("MagicPoint");
		if (attribute != null)
			itemElement.MagicPoint = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("PropDesc");
		if (attribute != null)
			itemElement.PropDesc = StrParser.ParseStr(attribute, "");
		return true;
	}


	public Dictionary<int, PropInfoElement> GetPropInfoElementList()
	{
		return _mPropInfoElementList;
	}

	private Dictionary<int, PropInfoElement> _mPropInfoElementList = new Dictionary<int, PropInfoElement>();
}

