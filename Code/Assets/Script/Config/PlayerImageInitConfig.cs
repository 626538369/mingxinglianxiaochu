using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;


public class PlayerImageInitConfig : ConfigBase 
{

	public class ImageInitData
	{
		public int nID;
		public string type;
		public string icon;
		public string initValue;
		public int nGener;
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
					ImageInitData ItemElement;
					if(!LoatItemElement(ChirldrenElement, out ItemElement))
						continue;
					_mItemElementList.Add(ItemElement.nID,ItemElement);
				}
			}
			
			return true;
		}
		
		return false;
	}
	
	private bool LoatItemElement(SecurityElement element,out ImageInitData ItemElement)
	{
		ItemElement = new ImageInitData();
		string attribute = element.Attribute("Id");
		if(attribute!=null)
			ItemElement.nID = StrParser.ParseDecInt(attribute,-1);
		
		attribute = element.Attribute("Gender");
		if(attribute!=null)
			ItemElement.nGener = StrParser.ParseDecInt(attribute,0);
		
		attribute = element.Attribute("type");
		if(attribute!=null)
			ItemElement.type = attribute;
		
		attribute = element.Attribute("Icon");
		if(attribute!=null)
			ItemElement.icon = attribute;
		
		attribute = element.Attribute("Value");
		if(attribute!=null)
			ItemElement.initValue = attribute;
		
		
		return true;
	}
	
	// TgirlID nFavqinmidu
	public bool GetItemElement(int id, out ImageInitData itemElement)
	{
		itemElement = null;
		if (!_mItemElementList.TryGetValue(id, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	public Dictionary<int,ImageInitData> GetItemElementList()
	{
		return _mItemElementList;
	}
	
	private Dictionary<int,ImageInitData> _mItemElementList = new Dictionary<int, ImageInitData>();
}
