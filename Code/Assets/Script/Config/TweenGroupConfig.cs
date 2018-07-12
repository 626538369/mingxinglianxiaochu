using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class TweenGroupConfig : ConfigBase {
	
	public struct TweenCellInfo
	{
		public string TweenGameObject;
		public float StartTime;
		public string TweenName;
		public float TweenDuration;
		public int TweenValueType;
		public string TweenValueFrom;
		public string TweenValueTo;
	};
	public class TweenGroupObject
	{
		public int TweenId;
		public int IsAutoJump;
		public List<TweenCellInfo> TweenCellInfoList = new List<TweenCellInfo>();
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				// server
				TweenGroupObject tweenGruopObject = new TweenGroupObject();
				tweenGruopObject.TweenId = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("TweenId"),""), -1);
				
				TweenCellInfo tweenCellItem = new TweenCellInfo();
				tweenCellItem.TweenGameObject = StrParser.ParseStr(childrenElement.Attribute("TweenGameObject"),"");
				if (tweenCellItem.TweenGameObject != "")
				{
					if (!_mSpecialDic.ContainsKey(tweenCellItem.TweenGameObject))
					{
						List<int> specialIDList =  new List<int>();
						_mSpecialDic[tweenCellItem.TweenGameObject] = specialIDList;
					}
					
					_mSpecialDic[tweenCellItem.TweenGameObject].Add(tweenGruopObject.TweenId);
				}
				tweenCellItem.StartTime = StrParser.ParseFloat(StrParser.ParseStr(childrenElement.Attribute("StartTime"),""), 0);
				tweenCellItem.TweenName = StrParser.ParseStr(childrenElement.Attribute("TweenName"),"");
				tweenCellItem.TweenDuration = StrParser.ParseFloat(StrParser.ParseStr(childrenElement.Attribute("TweenDuration"),""), -1);
				tweenCellItem.TweenValueType = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("TweenValueType"),""), -1);
				tweenCellItem.TweenValueFrom = StrParser.ParseStr(childrenElement.Attribute("TweenValueFrom"),"");
				tweenCellItem.TweenValueTo = StrParser.ParseStr(childrenElement.Attribute("TweenValueTo"),"");
				tweenGruopObject.TweenCellInfoList.Add(tweenCellItem);
				
				tweenCellItem = new TweenCellInfo();
				tweenCellItem.TweenGameObject = StrParser.ParseStr(childrenElement.Attribute("TweenGameObject1"),"");
				tweenCellItem.StartTime = StrParser.ParseFloat(StrParser.ParseStr(childrenElement.Attribute("StartTime1"),""), 0);
				tweenCellItem.TweenName = StrParser.ParseStr(childrenElement.Attribute("TweenName1"),"");
				tweenCellItem.TweenDuration = StrParser.ParseFloat(StrParser.ParseStr(childrenElement.Attribute("TweenDuration1"),""), -1);
				tweenCellItem.TweenValueType = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("TweenValueType1"),""), -1);
				tweenCellItem.TweenValueFrom = StrParser.ParseStr(childrenElement.Attribute("TweenValueFrom1"),"");
				tweenCellItem.TweenValueTo = StrParser.ParseStr(childrenElement.Attribute("TweenValueTo1"),"");
				tweenGruopObject.TweenCellInfoList.Add(tweenCellItem);
				
				tweenGruopObject.IsAutoJump = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("IsAutoJump"),""), -1);
				_mTweenObjectDic[tweenGruopObject.TweenId] = tweenGruopObject;
				
			}
			return true;
		}
		else
		{
			return false;
		}
		
		return true;
	}
	
	public bool GetItemElement(int itemLogicID, out TweenGroupObject itemElement)
	{
		itemElement = null;
		if (!_mTweenObjectDic.TryGetValue(itemLogicID, out itemElement))
		{
			return false;
		}
		
		return true;
	}
	
	public bool IsPregroundGroup(string specialObject,int tweenId)
	{
		if (_mSpecialDic.ContainsKey(specialObject))
		{
			for (int i=0; i<_mSpecialDic[specialObject].Count; i++)
			{
				int tweenID = _mSpecialDic[specialObject][i];
				if (tweenID == tweenId)
					return true;
			}
		}
		return false;
	}
	
	protected Dictionary<int, TweenGroupObject> _mTweenObjectDic = new  Dictionary<int, TweenGroupObject>();
	protected Dictionary<string,List<int> > _mSpecialDic = new  Dictionary<string, List<int> >();
}
