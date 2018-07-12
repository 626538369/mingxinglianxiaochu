using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Security;

public class g_level_info : ConfigBase
{
	public class g_level_infoElement
	{
		public string LevelID ;
		public int LevelType;
		public int InitialMood;
		public string Mood1;
		public string Mood2;
		public string Mood3;
		public string Mood4;
		public string Mood5;
		public string InitElement;
		public string BaseElement;
		public string ElementLimit;
		public string LevelBg;
		public int CharacterID;
		public int MosterID;
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
					g_level_infoElement itemElement;
					if (!Loadg_level_infoElement(childrenElement, out itemElement))
						continue;

					_mg_level_infoElementList[itemElement.LevelID] = itemElement;
				}
			}
			return true;
		}
		return false;
	}

	private bool Loadg_level_infoElement(SecurityElement element, out g_level_infoElement itemElement)
	{
		itemElement = new g_level_infoElement();

		string attribute = element.Attribute("LevelID");
		if (attribute != null)
			itemElement.LevelID = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("LevelType");
		if (attribute != null)
			itemElement.LevelType = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("InitialMood");
		if (attribute != null)
			itemElement.InitialMood = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Mood1");
		if (attribute != null)
			itemElement.Mood1 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Mood2");
		if (attribute != null)
			itemElement.Mood2 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Mood3");
		if (attribute != null)
			itemElement.Mood3 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Mood4");
		if (attribute != null)
			itemElement.Mood4 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("Mood5");
		if (attribute != null)
			itemElement.Mood5 = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("InitElement");
		if (attribute != null)
			itemElement.InitElement = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("BaseElement");
		if (attribute != null)
			itemElement.BaseElement = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("ElementLimit");
		if (attribute != null)
			itemElement.ElementLimit = StrParser.ParseStr(attribute, "");
		attribute = element.Attribute("LevelBg");
		if (attribute != null)
			itemElement.LevelBg = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("CharacterID");
		if (attribute != null)
			itemElement.CharacterID = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("MosterID");
		if (attribute != null)
			itemElement.MosterID = StrParser.ParseDecInt(attribute, 0);
		
		return true;
	}


	public Dictionary<string, g_level_infoElement> Getg_level_infoElementList()
	{
		return _mg_level_infoElementList;
	}

	private Dictionary<string, g_level_infoElement> _mg_level_infoElementList = new Dictionary<string, g_level_infoElement>();
}

