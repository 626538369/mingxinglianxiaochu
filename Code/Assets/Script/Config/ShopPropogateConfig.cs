using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class ShopPropogateConfig : ConfigBase
{
	public class ShopPropoGateObject
	{
		public int Shop_ID;
		public int Propogate_ID;
		public int Add_Exp; 
		public string Propogate_Desc;
		public int Cost_Type;
		public int Cost_Num;
	}
	
	private bool LoadItemElement(SecurityElement element, out ShopPropoGateObject itemElement)
	{
		itemElement = new ShopPropoGateObject();
		string attribute = element.Attribute("Shop_ID");
		if (attribute != null)
			itemElement.Shop_ID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Propogate_ID");
		if (attribute != null)
		itemElement.Propogate_ID = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Add_Exp");
		if (attribute != null)
		itemElement.Add_Exp = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Propogate_Desc");
		if (attribute != null)
			itemElement.Propogate_Desc = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Cost_Type");
		if (attribute != null)
			itemElement.Cost_Type = StrParser.ParseDecInt(attribute, 0);
		
		attribute = element.Attribute("Cost_Num");
		if (attribute != null)
			itemElement.Cost_Num = StrParser.ParseDecInt(attribute, 0);
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
					ShopPropoGateObject itemElement;
					if (!LoadItemElement(childrenElement, out itemElement))
						continue;
					
					_mShopPropoGateDic[itemElement.Propogate_ID] = itemElement;
				}
				
			}
			
			return true;
		}
		return false;
	}	
	
	public ShopPropoGateObject GetPropoGateObject(int PropoGateId)
	{
		return _mShopPropoGateDic[PropoGateId];
	}
	

	protected Dictionary<int, ShopPropoGateObject> _mShopPropoGateDic = new  Dictionary<int, ShopPropoGateObject>();
	
}
