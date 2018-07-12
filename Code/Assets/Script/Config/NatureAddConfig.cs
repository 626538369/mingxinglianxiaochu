using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;



public class NatureAddConfig : ConfigBase
{
		
	public class ItemNatureObject
	{
		public int ItemNatureID;
		public Dictionary<int ,float > natureItemInfoDic = new Dictionary<int ,float>();
	}
	
    public override bool Load (SecurityElement element)
	{
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				ItemNatureObject itemNatureObject = new ItemNatureObject();
				
				string attribute = childrenElement.Attribute("Item_Nature");
				
				if (attribute != null)
					itemNatureObject.ItemNatureID = StrParser.ParseDecInt(attribute, -1);
				
				int keyInt = 0;
				float valueInt = 1.0f;
				attribute = childrenElement.Attribute("Girl_Nature_Type1");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight1");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
					
				attribute = childrenElement.Attribute("Girl_Nature_Type2");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight2");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type3");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight3");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);

				attribute = childrenElement.Attribute("Girl_Nature_Type4");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight4");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type5");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight5");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type6");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight6");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type7");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight7");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type8");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight8");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type9");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight9");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
	
				
				attribute = childrenElement.Attribute("Girl_Nature_Type10");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight10");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type11");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = element.Attribute("Girl_Nature_Weight11");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type12");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight12");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				attribute = childrenElement.Attribute("Girl_Nature_Type13");
				if (attribute != null)
					keyInt = StrParser.ParseDecInt(attribute, -1);
				
				attribute = childrenElement.Attribute("Girl_Nature_Weight13");
				if (attribute != null)
					valueInt = StrParser.ParseFloat(attribute, -1);
				
				itemNatureObject.natureItemInfoDic.Add(keyInt,valueInt);
				
				_mNatureObjectDic[itemNatureObject.ItemNatureID] = itemNatureObject;
				

			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out ItemNatureObject itemElement)
	{
		itemElement = null;
		if (!_mNatureObjectDic.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	protected Dictionary<int, ItemNatureObject> _mNatureObjectDic = new  Dictionary<int, ItemNatureObject>();
	
}
