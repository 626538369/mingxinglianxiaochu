using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Security;

/// <summary>
/// Word config. read word xml file
/// </summary>
public class WordConfig : ConfigBase
{
	public class WordStruct
	{
		public int _wordID;
		public string _wordTextCN;
		public string _wordTextEN;
		public string _wordTextJP;
		public string _wordTextOther;
	}
	
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
		{
			return false;
		}
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				WordStruct currStruct = new WordStruct();
				
				currStruct._wordID = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("ID"),""),0);
				currStruct._wordTextCN = StrParser.ParseStr(childrenElement.Attribute("CN"),"");
				currStruct._wordTextEN = StrParser.ParseStr(childrenElement.Attribute("EN"),"");
				currStruct._wordTextJP = StrParser.ParseStr(childrenElement.Attribute("JP"),"");
				currStruct._wordTextOther = StrParser.ParseStr(childrenElement.Attribute("Other"),"");
				
				_wordDictionary.Add(currStruct._wordID,currStruct);
			}
		}
		else    
		{
			return false;
		}
		return true;
	}

	public string GetWordByID(int wordID,int languageIndex)
	{
		string result = "";
		if(!_wordDictionary.ContainsKey(wordID))
		{
			return result;
		}
		WordStruct currStruct = _wordDictionary[wordID];
		
		switch(languageIndex)
		{
		case 0:
			result = currStruct._wordTextCN;
			break;
		case 1:
			result = currStruct._wordTextEN;
			break;
		case 2:
			result = currStruct._wordTextJP;
			break;
		case 3:
			result = currStruct._wordTextOther;
			break;
		}
		
		return result;
	}
	protected Dictionary<int,WordStruct> _wordDictionary = new  Dictionary<int, WordStruct>();
}
