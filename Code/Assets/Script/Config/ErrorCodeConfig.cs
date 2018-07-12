using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Security;

public class ErrorCodeConfig : ConfigBase
{
	public class WordElement
	{
		public int WordID;
		public string WordTextCN;
		public string WordTextEN;
		public string WordTextJP;
		public string WordTextOther;
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
					WordElement wordElement = null;
					if (!LoadWordElement(childrenElement, out wordElement))
						continue;
					
					_mWordElementList[wordElement.WordID] = wordElement;
				}
			}
			
			return true;
		}
		return false;
	}
	
	private bool LoadWordElement(SecurityElement element, out WordElement wordElement)
	{
		wordElement = new WordElement();
		
		string attribute = element.Attribute("ID");
		if (attribute != null)
			wordElement.WordID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("CN");
		if (attribute != null)
			wordElement.WordTextCN = attribute;
		
		attribute = element.Attribute("EN");
		if (attribute != null)
			wordElement.WordTextEN = attribute;
		
		attribute = element.Attribute("JP");
		if (attribute != null)
			wordElement.WordTextJP = attribute;
		
		attribute = element.Attribute("Other");
		if (attribute != null)
			wordElement.WordTextOther = attribute;
		
		return true;
	}
	
	public string GetWordByID(int wordID, int languageIndex)
	{
		string result = "";
		if(!_mWordElementList.ContainsKey(wordID))
		{
			return result;
		}
		
		switch(languageIndex)
		{
		case 0:
			result = _mWordElementList[wordID].WordTextCN;
			break;
		case 1:
			result = _mWordElementList[wordID].WordTextEN;
			break;
		case 2:
			result = _mWordElementList[wordID].WordTextJP;
			break;
		case 3:
			result = _mWordElementList[wordID].WordTextOther;
			break;
		}
		
		return result;
	}
	
	private Dictionary<int, WordElement> _mWordElementList = new Dictionary<int, WordElement>();
}
