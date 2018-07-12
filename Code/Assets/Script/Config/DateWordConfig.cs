using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;


enum SAY_TYPE
{
	ACTOR_SAY = 0,
	GIRL_SAY = 1,
};

public class DateWordConfig : ConfigBase 
{

	public class DateWordElement
	{
		public int nID;
		public int nMinIntimacy;
		public int nMaxIntimacy;
		public int bSuccess;
		public int IsNormal;
		public int[] bSays = new int[7]; ///说话人身份 0：主角  , 1: 女孩///
		public string[] strWord = new string[7];
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag!="Items")
			return false;
		if(element.Children !=null)
		{
			foreach(SecurityElement ChirldrenElement in element.Children)
			{
				if(ChirldrenElement.Tag=="Item")
				{
					DateWordElement ItemElement;
					if(!LoatItemElement(ChirldrenElement, out ItemElement))
						continue;
					
					if(_mItemElementList.ContainsKey(ItemElement.nID))
					{
						List<DateWordElement> ListItem = new List<DateWordElement>();
						_mItemElementList.TryGetValue(ItemElement.nID, out ListItem);
						_mItemElementList.Remove(ItemElement.nID);
						ListItem.Add(ItemElement);
						_mItemElementList.Add(ItemElement.nID, ListItem);
					}
					else
					{
						List<DateWordElement> ListItem = new List<DateWordElement>();
						ListItem.Add(ItemElement);
						_mItemElementList.Add(ItemElement.nID, ListItem);
					}
				}
			}
			
			return true;
		}
		
		return false;
	}
	
	private bool LoatItemElement(SecurityElement element,out DateWordElement ItemElement)
	{
		ItemElement = new DateWordElement();
		string attribute = element.Attribute("Girl_ID");
		if(attribute!=null)
			ItemElement.nID = StrParser.ParseDecInt(attribute,-1);
		
		attribute = element.Attribute("Need_Min_Intimacy");
		if(attribute!=null)
			ItemElement.nMinIntimacy = StrParser.ParseDecInt(attribute,-1);
		
		attribute = element.Attribute("Need_Max_Intimacy");
		if(attribute!=null)
			ItemElement.nMaxIntimacy = StrParser.ParseDecInt(attribute,-1);
		
		attribute = element.Attribute("Is_Success");
		if(attribute!=null)
		{
			ItemElement.bSuccess = StrParser.ParseDecInt(attribute,0);//chenggong
		}
		
		attribute = element.Attribute("Is_Normal");
		if(attribute!=null)
		{
			ItemElement.IsNormal = StrParser.ParseDecInt(attribute,0);;//zhengchang
		}
		
		attribute = element.Attribute("Who_Says1");
		if(attribute!=null)
		{
		    ItemElement.bSays[0] =  StrParser.ParseDecInt(attribute,0);//xuejie
		}
		
		attribute = element.Attribute("Who_Says2");
		if(attribute!=null)
		if(attribute!=null)
		{
			ItemElement.bSays[1] = StrParser.ParseDecInt(attribute,0);
		}
		
		attribute = element.Attribute("Who_Says3");
		if(attribute!=null)
			if(attribute!=null)
		{
			ItemElement.bSays[2] = StrParser.ParseDecInt(attribute,0);
		}
		
		attribute = element.Attribute("Who_Says4");
		if(attribute!=null)
			if(attribute!=null)
		{
			ItemElement.bSays[3] = StrParser.ParseDecInt(attribute,0);
		}
		
		attribute = element.Attribute("Who_Says5");
		if(attribute!=null)
			if(attribute!=null)
		{
			ItemElement.bSays[4] = StrParser.ParseDecInt(attribute,0);
		}
		
		attribute = element.Attribute("Who_Says6");
		if(attribute!=null)
				if(attribute!=null)
		{
			ItemElement.bSays[5] =  StrParser.ParseDecInt(attribute,0);;
		}
		
		attribute = element.Attribute("Who_Says7");
		if(attribute!=null)
				if(attribute!=null)
		{
			ItemElement.bSays[6] =  StrParser.ParseDecInt(attribute,0);;
		}
		
		attribute = element.Attribute("Word1");
		if(attribute!=null)
			ItemElement.strWord[0] = StrParser.ParseStr(attribute,"");
		
		attribute = element.Attribute("Word2");
		if(attribute!=null)
			ItemElement.strWord[1] = StrParser.ParseStr(attribute,"");
		
		attribute = element.Attribute("Word3");
		if(attribute!=null)
			ItemElement.strWord[2] = StrParser.ParseStr(attribute,"");
		
		attribute = element.Attribute("Word4");
		if(attribute!=null)
			ItemElement.strWord[3] = StrParser.ParseStr(attribute,"");
		attribute = element.Attribute("Word5");
		if(attribute!=null)
			ItemElement.strWord[4] = StrParser.ParseStr(attribute,"");
		attribute = element.Attribute("Word6");
		if(attribute!=null)
			ItemElement.strWord[5] = StrParser.ParseStr(attribute,"");
		
		attribute = element.Attribute("Word7");
		if(attribute!=null)
			ItemElement.strWord[6] = StrParser.ParseStr(attribute,"");
		return true;
	}
	
	// TgirlID nFavqinmidu
	public bool GetItemElement(int ItemID, int nFav, int bNoFanyao, int Sucess,out DateWordElement ItemElement)
	{
		ItemElement = null;
		List<DateWordElement> ListItem = new List<DateWordElement>();
		if(!_mItemElementList.TryGetValue(ItemID, out ListItem))
			return false;
		for(int i=0;i<ListItem.Count;i++)
		{
			if(nFav>=ListItem[i].nMinIntimacy&&nFav<=ListItem[i].nMaxIntimacy)
			{
				if(ListItem[i].IsNormal==bNoFanyao)
				{
					if(ListItem[i].bSuccess==Sucess)
					{
						ItemElement = ListItem[i];
					}
				}
			}
		}
		return true;
	}
	
	private Dictionary<int,List<DateWordElement>> _mItemElementList = new Dictionary<int, List<DateWordElement>>();
}
