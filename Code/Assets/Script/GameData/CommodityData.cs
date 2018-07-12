using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CurrencyType
{
	// Rmb,
	// Bean,
	// GfanCoupon,
	// Diamond,
	
	Diamond,
	RmbYuan = 1,
	RmbJiao = 2,
	RmbFen = 3,
}

public enum CommodityType
{
	Recharge = 1,
	GameInner = 2,
	PromotionGift = 3,
}

public enum CommodityPayState
{
	Failed = -1,
	Paying = 1,
	Success = 2,
}

public enum CommodityState
{
	OnStore,
	OffStore,
}

public class CommodityEvent
{
	public enum CommodityEventId
	{
		None,
		Discount,
		Handsel,
	}
	
	public CommodityEventId eventId = CommodityEventId.None;
	public string beginTime;
	public string endTime;
	// public string eventDescription;
}

public class CommodityData : ItemData
{
	public bool IsEquals(CommodityData other)
	{
		if( (comType == CommodityType.Recharge && ItemID == other.ItemID)
			|| (comType == CommodityType.GameInner && BasicData.LogicID == other.BasicData.LogicID )
			)
		{
			return true;
		}
		
		return false;
	}
	
	public bool IsDiscounted
	{
		get { return currPrice < originalPrice;}
	}
	
	public string orderId = "";
	
	public CurrencyType currency = CurrencyType.Diamond;
	public CommodityType comType = CommodityType.GameInner;
	
	public int currPrice = 0;
	public int originalPrice = 0;
	public float discoutedRate = 1.0f;
	
	public int recvIgnotCnt;
	
	public CommodityState stateId = CommodityState.OffStore;
	public string buyTime;

	public string CommodityStr;
	public List<ItemMultData> freeGoodsList = new List<ItemMultData>();
	// public List<CommodityEvent> eventList = new List<CommodityEvent>();
	public CommodityEvent bargainEvent = null;
	public CommodityEvent handselEvent = null;
	
	// Transfer data from Server data struction
	public void UpdateFromServerData(sg.GS2C_Pay_Get_Goods_Res.PayGoodInfo source)
	{
		currency = (CurrencyType)source.amountUnit;
		comType = CommodityType.Recharge;
		
		base.ItemID = source.id;
		
		BasicData.Name = source.name;
		BasicData.Description = source.summary;
		BasicData.Count = 1;
		BasicData.Icon = source.icon;
		BasicData.IsFirstDouble = source.isFirstDouble;
		CommodityStr = source.commodityStr;
		currPrice = source.rebateAmount;
		originalPrice = source.amount;
		
		buyTime = source.payTime;
		recvIgnotCnt = source.virtualAmount;
		
		foreach (sg.BI_Pay_HandselResources_Mes info in source.handselResourcesArray)
		{
			ItemMultData data = new ItemMultData();
			
			data.LogicID = info.handselType;
			data.ItemCount = info.handselValue;
			
			// Just for show the free goods icon
			freeGoodsList.Add(data);
		}
		
		if (!string.IsNullOrEmpty(source.rebateBeginTime))
		{
			bargainEvent = new CommodityEvent();
			bargainEvent.eventId = CommodityEvent.CommodityEventId.Discount;
			bargainEvent.beginTime = source.rebateBeginTime;
			bargainEvent.endTime = source.rebateEndTime;
		}
		
		if (!string.IsNullOrEmpty(source.handselBeginTime))
		{
			handselEvent = new CommodityEvent();
			handselEvent.eventId = CommodityEvent.CommodityEventId.Handsel;
			handselEvent.beginTime = source.handselBeginTime;
			handselEvent.endTime = source.handselEndTime;
		}
	}
	
	public void UpdateFromServerData(sg.GS2C_ItemMall_Get_Goods_Res.BuyGoodsInfo source)
	{
		currency = CurrencyType.Diamond;
		comType = CommodityType.GameInner;
		
		BasicData.LogicID = source.id;
		BasicData.FillDataFromConfig();
		
		BasicData.Name = source.name;
		BasicData.Description = source.summary;
		BasicData.Icon = source.icon;
		
		currPrice = source.rebateAmount;
		originalPrice = source.amount;
		
		buyTime = source.buyTime;
		
		if (0 != source.handselItemId || -1 != source.handselItemId)
		{
			ItemMultData data = new ItemMultData();
			data.LogicID = source.handselItemId;
			data.ItemCount = source.handselItemNumber;
			// source.handselBeginTime;
			// source.handselEndTime;
				
			freeGoodsList.Add(data);
		}
		
		if (!string.IsNullOrEmpty(source.rebateBeginTime))
		{
			bargainEvent = new CommodityEvent();
			bargainEvent.eventId = CommodityEvent.CommodityEventId.Discount;
			bargainEvent.beginTime = source.rebateBeginTime;
			bargainEvent.endTime = source.rebateEndTime;
		}
		
		if (!string.IsNullOrEmpty(source.handselBeginTime))
		{
			handselEvent = new CommodityEvent();
			handselEvent.eventId = CommodityEvent.CommodityEventId.Handsel;
			handselEvent.beginTime = source.handselBeginTime;
			handselEvent.endTime = source.handselEndTime;
		}
	}
}

// public enum OrderState
// {
// 	PayFailed = -1,
// 	Paying = 1,
// 	PaySuccessed = 2,
// }
