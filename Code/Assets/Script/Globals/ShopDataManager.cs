using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

	
public class ShopDataManager : MonoBehaviour
{
		enum ShopType
		{
			 MARKET = 1,
			 FOOD = 2,
			 BOOK = 3,
			 SUPERMARKET = 4,
		     CYCLE = 5,
		};
	
		public class ShopInfo
		{
			public int shopID;
		    public long refreshTime;
		    public int refreshMoney;
			public  List<GoodsInfo> mShopGoodsList ;
		}
	
		public class GoodsInfo
		{
			public  int nId;			
			public int nCurrencyType;
			public int nPrice;
			public int  nPriceDiscount;
			public int nDiscount;
			public bool nIsNew; 
			public bool nHaveBuy;
			public float nSkillDiscount;
		    public int nLimitCount;
			public int nBoughtCount;
			public bool isDream;
			public string nGoodRegId;
		};
	public class ShopListInfo
	{
		public string type;
		public string goods_register_id;
		public string goods_price;
		public string goods_number;
		public string goods_describe;
		public string goods_id;
		public string goods_icon;
		public string goods_name;
		
	};
//		public int ShopPushGoods;
	
		public int ShopID = 510;
		public int MallID = 5140;
		
		public int ShopPushGoodsID; // 推送的位置 ID 以这个数字ID来判断是在哪个节点触发的推送系统//
		public int ShopPushGoodsVL; //  储存一下在触发节点、原有逻辑发送协议需要的数据、以便推送系统结束后、发送协议//
		
	public int goodsNum; // 
	public int goodsPrice; //

		// ----------推送系统2个触发的节点//
		public int JOB = 1 ;	
		public int XUANMEI = 2; 	
		public int SHOP = 3;  
 		// --------------------------------//
	
	
		public int InStudy; //进入学习界面时、记录一下ID；//
		public bool TermStudyEnd = false; //  学期学习结束 //
	
		public ShopDataManager ()
		{
		}
		
	    public  Dictionary<int, ShopInfo > shopToGoodList
		{
			get {return _mShopsToGoodsList;}	
		}
	
	    public void updateShopGoodInfo(int shopID,int goodId,int itemBoughtCount)
		{
			ShopInfo shopInfo ;
			_mShopsToGoodsList.TryGetValue(shopID,out shopInfo);
		    if (shopInfo != null)
			{
				for (int i=0; i<shopInfo.mShopGoodsList.Count; i++)
				{
					if (shopInfo.mShopGoodsList[i].nId == goodId)
					{
						GoodsInfo goodInfo = shopInfo.mShopGoodsList[i];
						goodInfo.nBoughtCount = itemBoughtCount;
						shopInfo.mShopGoodsList[i] = goodInfo;
						_mShopsToGoodsList[shopID] = shopInfo;
						break;
					}
				}
			}
		}
	
		public GoodsInfo getGoodInfo(int shopID,int goodId)
		{
			ShopInfo shopInfo ;
			GoodsInfo goodInfo = null;
			_mShopsToGoodsList.TryGetValue(shopID,out shopInfo);
		    if (shopInfo != null)
			{
				for (int i=0; i<shopInfo.mShopGoodsList.Count; i++)
				{
					if (shopInfo.mShopGoodsList[i].nId == goodId)
					{
						goodInfo = shopInfo.mShopGoodsList[i];
						return goodInfo;
					}
				}
			}
			return goodInfo;
		}
	
		private List<GoodsInfo> mShopGoodsList = new List<GoodsInfo>();
	
	
	    Dictionary<int, ShopInfo > _mShopsToGoodsList = new Dictionary<int,  ShopInfo >();
	  
	public static CommodityData PayCommodityData = new CommodityData();
	
	
	public struct PendingOrderInfo
	{
		public string platform;
		public string changeOrder;
		public string changeReceipt;
	};
	
	static List<string> sPendingOrderIds = new List<string>();
	static Dictionary<CommodityType, List<CommodityData>> sVipCommodityDicts = new Dictionary<CommodityType, List<CommodityData>>();
	static Dictionary<string ,PendingOrderInfo> sPendingOrderIdsInfo = new Dictionary<string, PendingOrderInfo>();
	public static Dictionary<string ,string> CommodityToCurrencyDicts = new Dictionary<string, string>();
	
	public static void ClearPendingOrderIds()
	{
		sPendingOrderIds.Clear();
		sPendingOrderIdsInfo.Clear();
	}
	
	public static void AddPendingOrderId(string orderId,string platformstr ,string changeOrder,string changeReceipt)
	{
		bool isHas = false;
		for (int i = 0; i < sPendingOrderIds.Count; i++)
		{
			if (sPendingOrderIds[i].Equals(orderId))
			{
				isHas = true;
				break;
			}
		}
		
		if (!isHas)
		{
			sPendingOrderIds.Add(orderId);
			PendingOrderInfo aPendingOrderInfo = new PendingOrderInfo();
			aPendingOrderInfo.changeOrder = changeOrder;
			aPendingOrderInfo.platform = platformstr;
			aPendingOrderInfo.changeReceipt = changeReceipt;
			sPendingOrderIdsInfo.Add(orderId,aPendingOrderInfo);
			// Record this order
			ShopDataManager.FlushPendingOrderId();
		}
	}
	
	public static void RemovePendingOrderId(string orderId)
	{
		if (!sPendingOrderIds.Remove(orderId))
		{
			Debug.Log("[GUIVipStore]: Remove a invalid order id...");
		}
		
		if (!sPendingOrderIdsInfo.Remove(orderId))
		{
			Debug.Log("[GUIVipStore]: Remove a invalid order id...");
		}
		// Record this order
		ShopDataManager.FlushPendingOrderId();
	}
	
	public static void ConfirmPendingOrderIds()
	{
		ReadCommOrderFile();
		
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			for (int i = 0; i < sPendingOrderIds.Count; i++)
			{
				NetSender.Instance.RequestAppStoreChargeConfirm(sPendingOrderIds[i],sPendingOrderIdsInfo[sPendingOrderIds[i]].changeReceipt, sPendingOrderIdsInfo[sPendingOrderIds[i]].changeOrder);
			}
		}
		//for (int i = 0; i < sPendingOrderIds.Count; i++)
		//{
		//	NetSender.Instance.RequestSearchOrderState(sPendingOrderIds[i]);
		//}
	}
	
	//------------------------------------------------------------------------------------
	public static void ClearCommodityDatas(CommodityType comType)
	{
		List<CommodityData> list = null;
		if (sVipCommodityDicts.TryGetValue(comType, out list))
		{
			list.Clear();
		}
	}
	
	public static List<CommodityData> GetSpecialCommodityList()
	{
		List<CommodityData> specList = new List<CommodityData>();
		foreach (List<CommodityData> list in sVipCommodityDicts.Values)
		{
			for (int i = 0; i < list.Count; ++i)
			{
				if (list[i].IsDiscounted)
				{
					specList.Add(list[i]);
				}
			}
		}
		
		return specList;
	}
	
	public static List<CommodityData> GetCommodityList(CommodityType comType)
	{
		List<CommodityData> list = null;
		if (sVipCommodityDicts.TryGetValue(comType, out list))
		{
			return list;
		}
		
		return list;
	}
	
	public static void AddCommodityData(CommodityData data)
	{
		List<CommodityData> list = null;
		if (!sVipCommodityDicts.TryGetValue(data.comType, out list))
		{
			list = new List<CommodityData>();
		}
		
		bool tFind = false;
		for(int i = 0; i < list.Count;i++)
		{
			if(list[i].IsEquals(data))
			{
				list[i] = data;
				tFind = true;
				break;
			}
		}
		
		if (!tFind)
		{
			list.Add(data);
		}
		
		sVipCommodityDicts[data.comType] = list;
	}
	
	public static void RemoveCommodityData(CommodityData data)
	{
		List<CommodityData> list = null;
		if (!sVipCommodityDicts.TryGetValue(data.comType, out list))
		{
			return;
		}
		
		for (int i = 0; i < list.Count; ++i)
		{
			if(list[i].IsEquals(data))
			{
				list.Remove(data);
				break;
			}
		}
		
		sVipCommodityDicts[data.comType] = list;
	}
	
	public static void UpdateCommodityData(CommodityType comType,CommodityData data)
	{
		List<CommodityData> list = null;
		if (!sVipCommodityDicts.TryGetValue(comType, out list))
		{
			list = new List<CommodityData>();
		}
		
		bool tFind = false;
		for(int i = 0; i < list.Count;i++)
		{
			if(list[i].IsEquals(data))
			{
				list[i] = data;
				tFind = true;
				break;
			}
		}
		
		if (!tFind)
		{
			list.Add(data);
		}
		
		sVipCommodityDicts[comType] = list;
	}
	
	//------------------------------------------------------------------------------------
	
	//------------------------------------------------------------------------------------
	private static FileStream smCommOrderFile = null;
	private static StreamWriter smCommOrderWriter = null;
	private static StreamReader smCommOrderReader = null;
	public static string GetCommOrderFileName()
	{
		return (Application.persistentDataPath + "/" + GameDefines.Setting_LoginName +  "QiaojianghuOrder.sc");
	}
	
	public static void CloseFile()
	{
		if(smCommOrderWriter != null)
		{
			smCommOrderWriter.Close();
		}
		smCommOrderWriter = null;
		
		if(smCommOrderReader != null)
		{
			smCommOrderReader.Close();
		}
		smCommOrderReader = null;
		
		if(smCommOrderFile != null)
		{
			smCommOrderFile.Close();
		}
		smCommOrderFile = null;
	}
	
	public static void FlushPendingOrderId()
	{
		if (null == smCommOrderFile)
		{
			try
			{
				smCommOrderFile = new FileStream(GetCommOrderFileName(), FileMode.Create, FileAccess.Write);
				smCommOrderWriter = new StreamWriter(smCommOrderFile);
			}
			catch(System.Exception ex)
			{
				Debug.LogWarning("NotFound the file:" + GetCommOrderFileName() + "\n" + ex.Message);
			}
		}
		
		smCommOrderFile.SetLength(0);
		smCommOrderFile.Flush();
		
		for (int i = 0; i < sPendingOrderIds.Count; i++)
		{
			smCommOrderWriter.WriteLine(sPendingOrderIds[i]);
			smCommOrderWriter.WriteLine(sPendingOrderIdsInfo[sPendingOrderIds[i]].platform);
			smCommOrderWriter.WriteLine(sPendingOrderIdsInfo[sPendingOrderIds[i]].changeOrder);
			smCommOrderWriter.WriteLine(sPendingOrderIdsInfo[sPendingOrderIds[i]].changeReceipt);
		}
		smCommOrderWriter.Flush();
	}
	
	public static void WritePendingOrderIds()
	{
		if (null == smCommOrderFile)
		{
			try
			{
				smCommOrderFile = new FileStream(GetCommOrderFileName(), FileMode.Create, FileAccess.Write);
				smCommOrderWriter = new StreamWriter(smCommOrderFile);
			}
			catch(System.Exception ex)
			{
				Debug.LogWarning("NotFound the file:" + GetCommOrderFileName() + "\n" + ex.Message);
			}
		}
		
		FlushPendingOrderId();
		CloseFile();
	}
	
	public static void ReadCommOrderFile()
	{
		try
		{
			using (smCommOrderFile = new FileStream(GetCommOrderFileName(), FileMode.Open, FileAccess.Read))
			{
				sPendingOrderIds.Clear();
				sPendingOrderIdsInfo.Clear();
				using (smCommOrderReader = new StreamReader(smCommOrderFile))
				{
					string orderId = "";
					string changeOredeID = "";
					string changeRecipt = "";
					string platform = "";
					
					while (!smCommOrderReader.EndOfStream)
					{
						orderId = smCommOrderReader.ReadLine();
						Debug.Log("PendingOrederis orderId" + orderId);
						platform = smCommOrderReader.ReadLine();
						Debug.Log("PendingOrederis platform" + platform);
						changeOredeID = smCommOrderReader.ReadLine();
						Debug.Log("PendingOrederis changeOredeID" + changeOredeID);
						changeRecipt = smCommOrderReader.ReadToEnd();
						Debug.Log("PendingOrederis changeRecipt" + changeRecipt);
						if (!string.IsNullOrEmpty(orderId))
						{
							sPendingOrderIds.Add(orderId);
							PendingOrderInfo aPendingOrderInfo = new PendingOrderInfo();
							aPendingOrderInfo.platform = platform;
							aPendingOrderInfo.changeOrder = changeOredeID;
							aPendingOrderInfo.changeReceipt = changeRecipt;
							sPendingOrderIdsInfo.Add(orderId,aPendingOrderInfo);
						}
					} // End while (!smCommOrderReader.EndOfStream)
				} // End using (smCommOrderReader = new StreamReader(smCommOrderFile))
			}
		}
		catch(System.Exception ex)
		{
			smCommOrderFile = null;
			Debug.LogWarning("NotFound the file:" + GetCommOrderFileName() + "\n" + ex.Message);
		}
		
		CloseFile();
	}


	}

