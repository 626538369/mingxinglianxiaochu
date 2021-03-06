// #define OPEN_GFAN_SDK

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class U3dGfaniOSSender
{
	public static readonly float RmbYuan2GfanQExchangeRatio = 10.0f;
	
	// UnitySendMessage("被通知的物体名", "函数名", "参数"), 在消息通知中，采用的key1=value1&key2=value2的格式封装多个参数为字符串,在U3D中
	// 需要自己将参数按照格式拆分
	
#if OPEN_GFAN_SDK
	[DllImport("__Internal")]
	private static extern void U3dGfanSetAppInfo(string appKey, string cpId, int timeout);
	
//	[DllImport("__Internal")]
//	private static extern void U3dGfanLeavePlatform();
	
	[DllImport("__Internal")]
	private static extern void U3dGfanLogin();
	
	[DllImport("__Internal")]
	private static extern void U3dGfanVerifyStatus();
	
	// [DllImport("__Internal")]
	// private static extern long U3dGfanGetUserId();
	// 
	// [DllImport("__Internal")]
	// private static extern bool U3dGfanIsLogined();
	// 
	// [DllImport("__Internal")]
	// private static extern bool U3dGfanIsGuest();
	// 
	// [DllImport("__Internal")]
	// private static extern int U3dGfanGetBalance();
	
	[DllImport("__Internal")]
	private static extern void U3dGfanRenameAccount();	
	
	[DllImport("__Internal")]
	private static extern void U3dGfanLogout();
	
	[DllImport("__Internal")]
	private static extern void U3dGfanPayOrder(string appKey, string orderId, string productName, string payDesc, int payValue);
	
	[DllImport("__Internal")]
	private static extern void U3dGfanInAppPayOrder(string appKey, string orderId, string productIdentifier, string productName, string payDesc, int payValue);
#endif	
	
	/*
	 * BattleAtlantic official AppId & AppKey in mAppn company.
	 * */
	public static readonly int OurAppId = 1865019395;
	public static readonly int NetworkTimeout = 200;
	
	public static int GfanUserBalance = 0;
	
	public static bool IsGuestAccount = true;
	
	public enum U3dGfanOrientation
	{
		Protrait,
		ProtraitUpsideDown,
		LandscapeLeft, // Iphone function button is in right hand
		LandscapeRight, // Iphone function button is in left hand
		Auto,
	}
	
	public enum U3dGfanAccountState
	{
		NotLogin,
		GuestLogin,
		NormalLogin,
	}
	
	public enum U3dGfanLogoutFlag
	{
		Normal,
		CancelAutoLogin,
	}
	
	public static string GetAppVersion()
	{
		return "1.0.0";
	}
	
	public static string GetAppChannelId()
	{
		return "GfaniOS";
	}
	
	public static void InitNdSDK()
	{
#if OPEN_GFAN_SDK
		U3dGfanSetAppInfo(OurAppId.ToString(), GetAppChannelId(), NetworkTimeout);
#endif
	}
	
	public static void CloseNdSDK()
	{
#if OPEN_GFAN_SDK
		//U3dGfanLeavePlatform();
#endif
	}
	
	public static void Login(bool supportGuest = false)
	{
#if OPEN_GFAN_SDK
		U3dGfanLogin();
#endif
	}
	
	public static void Guest2Official()
	{
#if OPEN_GFAN_SDK
		U3dGfanRenameAccount();
#endif
	}
	
	public static void EnterUserCenter()
	{
#if OPEN_GFAN_SDK
		U3dGfanRenameAccount();	
#endif
	}
	
	public static void Logout(bool cancelAutoLogin)
	{
#if OPEN_GFAN_SDK
		U3dGfanLogout();
#endif
	}
	
	public static void PayForCommodity(CommodityData data)
	{
#if OPEN_GFAN_SDK
		string orderId = data.orderId;
		
		string productId = data.ItemID.ToString();
		string productName = data.BasicData.Name;
		
		// Convert to GfanQuan unit
		float exchangeRatio = RmbYuan2GfanQExchangeRatio;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2GfanQExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2GfanQExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2GfanQExchangeRatio;
		}
		
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		
		int productCnt = data.BasicData.Count;
		string payDescription = data.BasicData.Description;
		string userName = ThirdPartyPlatform.UserUniqId;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		// Gfan pay's mininum is 1 * multiple
		int gfanPayVal = (int)Mathf.CeilToInt(productPrice);
		
		U3dGfanPayOrder(OurAppId.ToString(), orderId, productName, payDescription, gfanPayVal);
#endif
	}
	
	public static void PayForCommodityInApp(CommodityData data)
	{
#if OPEN_GFAN_SDK
		string orderId = data.orderId;
		
		string productId = data.ItemID.ToString();
		string productName = data.BasicData.Name;
		
		// Convert to GfanQuan unit
		float exchangeRatio = RmbYuan2GfanQExchangeRatio;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2GfanQExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2GfanQExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2GfanQExchangeRatio;
		}
		
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		
		int productCnt = data.BasicData.Count;
		string payDescription = data.BasicData.Description;
		string userName = ThirdPartyPlatform.UserUniqId;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		// Gfan pay's mininum is 1 * multiple
		int gfanPayVal = (int)Mathf.CeilToInt(productPrice);
		
		U3dGfanInAppPayOrder(OurAppId.ToString(), orderId, productName, productName, payDescription, gfanPayVal);
#endif
	}
	
	public static void SearchPayResult(string orderId)
	{
	}

}