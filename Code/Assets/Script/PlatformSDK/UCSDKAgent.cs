using UnityEngine;
using System.Collections;

public class UCSDKAgent : MonoBehaviour 
{
	//测试环境参数
//	static bool debugMode = true;
//	static int cpid = 1;
//	static int gameid = 435;
//	static int serverid = 1419;
//	static int channelId = 2;
//	static string servername = "";
//	static string apiKey = "54520eb3c61318c120052da361684207";
	
	// //正式环境参数
	 static bool debugMode = false;
	 static int cpid = 23078;
	 static int gameid = 506648;
	 static int serverid = 1897;
	 static int channelId = 2;
	 static string servername = "";
	 static string apiKey = "bbc4c35d73b443256d571f03add98d42";
	
	public static void InitSDK()
	{
#if UNITY_ANDROID
		Debug.Log("UCSDKAgent::InitSDK");
		
		//设置日志级别
		UCGameSdk.setLogLevel (UCConstants.LOGLEVEL_DEBUG);
		
		//设置屏幕方向
		UCGameSdk.setOrientation (UCConstants.ORIENTATION_LANDSCAPE);			
		
		//调用初始化
		// UCGameSdk.initSDK (true, 2, 1, 20, 24, "firstService", false, false);
		UCGameSdk.initSDK(debugMode, UCConstants.LOGLEVEL_ERROR, 
			cpid, gameid, serverid, "firstService", 
			true, true);
#elif UNITY_IPHONE
#endif
	}
	
	public static void CloseSDK()
	{
#if UNITY_ANDROID
		UCGameSdk.exitSDK ();
#elif UNITY_IPHONE
#endif
	}
	
	public static void Login()
	{
#if UNITY_ANDROID
		UCGameSdk.login(false, "");
#elif UNITY_IPHONE
#endif
	}
	
	public static void Logout()
	{
#if UNITY_ANDROID
		UCGameSdk.logout ();
#elif UNITY_IPHONE
#endif
	}
	
	public static void EnterUserCenter()
	{
#if UNITY_ANDROID
		UCGameSdk.enterUserCenter ();
#elif UNITY_IPHONE
#endif
		// UCGameSdk.enterUI ("user_center");
	}
	
	// public static void IsUCVip()
	// {
	// 	UCGameSdk.isUCVip ();
	// }
	// 
	// public static void GetUCVipInfo()
	// {
	// 	UCGameSdk.getUCVipInfo ();
	// }
	
	public static void OpenCharge()
	{
#if UNITY_ANDROID
		UCGameSdk.uPointCharge ();
#elif UNITY_IPHONE
#endif
	}
	
	public static readonly float RmbYuan2UCbi = 1.0f;
	public static void PayForCommodity(CommodityData data)
	{
		long roleId = Globals.Instance.MGameDataManager.MActorData.PlayerID;
		string roleName = Globals.Instance.MGameDataManager.MActorData.BasicData.Name;
		int level = Globals.Instance.MGameDataManager.MActorData.BasicData.Level;
		
		float exchangeRatio = RmbYuan2UCbi;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2UCbi;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2UCbi;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2UCbi;
		}
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		int productCnt = data.BasicData.Count;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		int ucbi = (int)Mathf.CeilToInt(productPrice);
		
		// Send to our pay server
		string customInfo = cpid + "," + data.orderId;
#if UNITY_ANDROID
		//调用支付接口
		UCGameSdk.pay(true, ucbi, serverid, roleId.ToString(), roleName, level.ToString(), customInfo);
#elif UNITY_IPHONE
#endif
	}
}
