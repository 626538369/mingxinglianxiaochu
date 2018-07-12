using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopyDropData
{
	public struct DropItem
	{
		public int ItemID;
		public int LogicID;
		public int ItemNum;
		public bool IsInTempBag;
	}
	
	public int BasicExp;
	public int BasicMoney;
	public int BasicContribution;
	public int CopyScore;
	
	public List<DropItem> DropItemList = new List<DropItem>();
}


public enum DropType
{
	BATTLE,
	COPY,
}

public class DropData
{
	public DropType Type;
	
	public int CopyScore;
	
	public int Technolegy;
	
	public int BasicMoney;
	public int BasicContribution;
	public int CopyID;
	
	
	public Dictionary<string, int> WarShipTypeList = new Dictionary<string, int>();
	public List<int> PickupItemList = new List<int>();
	
	// Give Up
	public int BasicExp;
	public int Ingot;
	
	// Rank relative
	public int Feat;
	public int Oil;
	
	// 
	
	
	public List<ItemSlotData> MItemSlotDataList = new List<ItemSlotData>();
}
