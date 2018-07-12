using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class BeautyThemeConfig : ConfigBase
{
	public class BeautyThemeObject
	{
		public int ThemeID;
		public string ThemeName;
		public int NatureType1;
		public int NatureType2;
		public int NaturePercent1;
		public int NaturePercent2;
		public int StylePercent;
		public int Style1;
		public int Style1Weight;
		public int Style2;
		public int Style2Weight;
		public int Style3;
		public int Style3Weight;
		public int Style4;
		public int Style4Weight;
		public int Style5;
		public int Style5Weight;
		public int Style6;
		public int Style6Weight;
		public int Style7;
		public int Style7Weight;
		public int Style8;
		public int Style8Weight;
		public int Style9;
		public int Style9Weight;
		public int Style10;
		public int Style10Weight;
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
					BeautyThemeObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mItemElementList[itemElement.ThemeID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadItemElement(SecurityElement element, out BeautyThemeObject itemElement)
	{
		itemElement = new BeautyThemeObject();
		
		string attribute = element.Attribute("Theme_ID");
		if (attribute != null)
			itemElement.ThemeID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Theme_Name");
		if (attribute != null)
			itemElement.ThemeName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Style_Percent");
		if (attribute != null)
			itemElement.StylePercent = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Nature_Type_1");
		if (attribute != null)
			itemElement.NatureType1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Percent_1");
		if (attribute != null)
			itemElement.NaturePercent1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Type_2");
		if (attribute != null)
			itemElement.NatureType2 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Nature_Percent_2");
		if (attribute != null)
			itemElement.NaturePercent2 = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Style1");
		if (attribute != null)
			itemElement.Style1 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style1_Weight");
		if (attribute != null)
			itemElement.Style1Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style2");
		if (attribute != null)
			itemElement.Style2 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style2_Weight");
		if (attribute != null)
			itemElement.Style2Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style3");
		if (attribute != null)
			itemElement.Style3 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style3_Weight");
		if (attribute != null)
			itemElement.Style3Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style4");
		if (attribute != null)
			itemElement.Style4 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style4_Weight");
		if (attribute != null)
			itemElement.Style4Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style5");
		if (attribute != null)
			itemElement.Style5 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style5_Weight");
		if (attribute != null)
			itemElement.Style5Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style6");
		if (attribute != null)
			itemElement.Style6 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style6_Weight");
		if (attribute != null)
			itemElement.Style6Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style7");
		if (attribute != null)
			itemElement.Style7 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style7_Weight");
		if (attribute != null)
			itemElement.Style7Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style8");
		if (attribute != null)
			itemElement.Style8 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style8_Weight");
		if (attribute != null)
			itemElement.Style8Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style9");
		if (attribute != null)
			itemElement.Style9 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style9_Weight");
		if (attribute != null)
			itemElement.Style9Weight = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style10");
		if (attribute != null)
			itemElement.Style10 = StrParser.ParseDecInt(attribute, 0);
		attribute = element.Attribute("Style10_Weight");
		if (attribute != null)
			itemElement.Style10Weight = StrParser.ParseDecInt(attribute, 0);
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out BeautyThemeObject itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	private Dictionary<int, BeautyThemeObject> _mItemElementList = new Dictionary<int, BeautyThemeObject>();
}
