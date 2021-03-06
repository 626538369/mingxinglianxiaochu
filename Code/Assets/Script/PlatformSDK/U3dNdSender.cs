//#define OPEN_ND91 // Use a define switch

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class U3dNdSender
{
	public static readonly float RmbYuan2NdBeanExchangeRatio = 1.0f;
	
	// UnitySendMessage("被通知的物体名", "函数名", "参数"), 在消息通知中，采用的key1=value1&key2=value2的格式封装多个参数为字符串,在U3D中
	// 需要自己将参数按照格式拆分
	
#if OPEN_ND91
	// SDK设置相关
	[DllImport("__Internal")]
	private static extern void U3dNdLeavePlatform();
	[DllImport("__Internal")]
	private static extern void U3dNdInitSDK();
	
	[DllImport("__Internal")]
	private static extern bool U3dNdSetAppId (int appID);
	[DllImport("__Internal")]
	private static extern bool U3dNdSetAppKey (string appKey);
	
	[DllImport("__Internal")]
	private static extern string U3dNdGetPlatformVersion();
	
	[DllImport ("__Internal")]
	private static extern void U3dNdSetScreenOrientation (int orientation);//orientation 0:竖屏 1:横屏朝左 2:竖屏朝下 3:横屏朝右;
	[DllImport ("__Internal")]
	private static extern void U3dNdSetAutoRotation (bool isAuto);
	
	[DllImport ("__Internal")]
	private static extern void U3dNdSetDebugMode (int nFlag);
	
	// 登录相关
	[DllImport ("__Internal")]
	private static extern int U3dNdLogin (int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdLoginEx(int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdLogout (int nFlag);//nFlag 标识（按位标识）0,表示注销；1，表示注销，并清除自动登录;
	[DllImport ("__Internal")]
	private static extern bool U3dNdIsLogined();
	[DllImport ("__Internal")]
	private static extern long U3dNdGetUin(); // 获取用户登录ID
	[DllImport ("__Internal")]
	private static extern string U3dNdGetNickName(); // 获取用户昵称
	
	[DllImport ("__Internal")]
	private static extern int U3dNdGetLoginState();
	[DllImport ("__Internal")]
	private static extern void U3dNdGuestRegister(int nFlag); // 游客登录或者游客账号转为正式
	
	[DllImport ("__Internal")]
	private static extern string U3dNdGetSessionId(); // 获取回话ID
	
	[DllImport ("__Internal")]
	private static extern string U3dNdSwitchAccount();
	
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterAccountSetting(); // 账号设置
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterPersonalInfo(); // 个人信息
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterTradeRecord(); // 查看充值消费记录
	
	// 版本更新相关
	[DllImport ("__Internal")]
	private static extern int U3dNdCheckUpdate (int nFlag); // nFlag传0
	[DllImport ("__Internal")]
	private static extern int U3dNdAppVersionUpdate (int nFlag);
	
	// 充值支付相关
	[DllImport ("__Internal")]
	private static extern int U3dNdEnterRecharge(int nFlag, string content); // 91豆充值， 1元=1个91豆，最小支持0.01个91豆
	[DllImport ("__Internal")]
	private static extern int U3dNdUniPaySyn(string cooOrderSerial, string productID, string productName, 
		double productPrice, double productOriginalPrice, int productCount, string payDescription); // 同步支付
	[DllImport ("__Internal")]
	private static extern int U3dNdUniPayAsyn(string cooOrderSerial, string productID, string productName, 
		double productPrice, double productOriginalPrice, int productCount, string payDescription); // 异步支付
	[DllImport ("__Internal")]
	private static extern int U3dNdSearchPayResult(string orderSerial); // 检测订单支付情况
	[DllImport ("__Internal")]
	private static extern int U3dNdUniPayForCoin (string cooOrderSerial,int needPayCoins,string payDescription); // 代币支付
	
	// 更多内容
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterPlatform (int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterAppCenter(int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdUserFeedback (int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterAppBBS(int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdPause(int nFlag);
	
	[DllImport ("__Internal")]
	private static extern int U3dNdGetNetWork();
	
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterFriendCenter(int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterAchievement(int nFlag);
	[DllImport ("__Internal")]
	private static extern void U3dNdEnterLeaderBoard(int param1, int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdShare2ThirdPlatform(string content, string image); // 分享到新浪微博/腾讯微博/人人网
	[DllImport ("__Internal")]
	private static extern void U3dNdSendTemplateActivity(int nTemplateId, long uin);
	[DllImport ("__Internal")]
	private static extern void U3dNdInviteFriend(string content); // 邀请好友
	
	[DllImport ("__Internal")]
	private static extern int U3dNdBindPhoneAccount(int nFlag, string number); // 绑定手机
#endif
	
	/*
	 * BattleAtlantic official AppId & AppKey in nd91 company.
	 * */
	public static readonly int OurNdAppId = 107535;
	public static readonly string OurNdAppKey = "e9fdf257feae9c6ec60bd060f61f7ef76bac65f065e9ca0d";	
	public enum U3dNdOrientation
	{
		Protrait,
		ProtraitUpsideDown,
		LandscapeLeft, // Iphone function button is in right hand
		LandscapeRight, // Iphone function button is in left hand
		Auto,
	}
	
	public enum U3dNdAccountState
	{
		NotLogin,
		GuestLogin,
		NormalLogin,
	}
	
	public enum U3dNdLogoutFlag
	{
		Normal,
		CancelAutoLogin, // 登出时取消自动登录
	}
	
	// public class U3dNdBuyInfo
	// {
	// 	public string orderSerial;
	// 	public int productID;
	// 	public int productName;
	// 	public int productPrice;
	// 	public int productOriginalPrice;
	// 	public int productCount;
	// 	public string productDescription;
	// }
	
	public static string GetAppVersion()
	{
		return "1.0.0";
	}
	
	public static string GetAppChannelId()
	{
		return "Nd91";
	}
	
	// 必须接入的：更新、登陆、支付、91社区入口
	public static void InitNdSDK()
	{
#if OPEN_ND91
		// Call plugin only when running on real device
		// if (Application.platform != RuntimePlatform.WindowsEditor
		// 	&& Application.platform != RuntimePlatform.OSXEditor
		// 	&& Application.platform != RuntimePlatform.Android)
		U3dNdSetAppId(OurNdAppId);
		U3dNdSetAppKey(OurNdAppKey);
		//U3dNdSetDebugMode(1);
		U3dNdInitSDK();
		
		U3dNdSetScreenOrientation((int)U3dNdOrientation.LandscapeRight);
		U3dNdSetAutoRotation(false);
#endif
	}
	
	// 释放平台内存(destory)
	public static void CloseNdSDK()
	{
#if OPEN_ND91		
		U3dNdLeavePlatform();
#endif
	}
	
	public static void Login(bool supportGuest = false)
	{
#if OPEN_ND91		
		// Force account login, when not the case many bad UE on IPad
		int nError = 0;
		nError = U3dNdLogin(0); // No support the guest login
		return;
		
		if (supportGuest)
		{
			nError = U3dNdLoginEx(0);
		}
		else
		{
			nError = U3dNdLogin(0);
		}
		
		if (-1 == nError)
		{
			Debug.Log("[U3dNdSender]: -----------------------------");
		}
#endif
	}
	
	public static bool IsLogined()
	{
#if OPEN_ND91
		return U3dNdIsLogined();
#endif
		
		return false;
	}
	
	public static bool IsGuestLogined()
	{
#if OPEN_ND91
		return U3dNdGetLoginState() == (int)U3dNdAccountState.GuestLogin;
#endif
		return false;
	}
	
	public static string GetUserUniqId()
	{
#if OPEN_ND91
		return U3dNdGetUin().ToString();
#endif
		return "";
	}
	
	public static string GetUserNickName()
	{
#if OPEN_ND91
		return U3dNdGetNickName();
#endif
		return "";
	}
	
	public static void Register()
	{
#if OPEN_ND91
		U3dNdGuestRegister(0);
#endif
	}
	
	public static void Guest2Official()
	{
#if OPEN_ND91
		U3dNdEnterAccountSetting();
		// U3dNdGuestRegister(0);
#endif
	}
	
	public static void SwitchAccount()
	{
#if OPEN_ND91
		U3dNdSwitchAccount();
#endif
	}
	
	public static void Logout(bool cancelAutoLogin)
	{
#if OPEN_ND91
		if (cancelAutoLogin)
		{
			U3dNdLogout((int)U3dNdLogoutFlag.CancelAutoLogin);
		}
		else
		{
			U3dNdLogout((int)U3dNdLogoutFlag.Normal);
		}
#endif
	}
	
	public static void CheckAppVersion()
	{
#if OPEN_ND91
		int nError = U3dNdAppVersionUpdate(0);
		if (-1 == nError)
		{
			// Do something
		}
#endif
	}
	
	public static void Recharge()
	{
#if OPEN_ND91
		U3dNdEnterRecharge(0, "");
#endif
	}
	
	public static void PayForCommodity(CommodityData data)
	{
#if OPEN_ND91
		string orderId = data.orderId;
		
		string productId = data.ItemID.ToString();
		string productName = data.BasicData.Name;
		
		// Convert to 91Bean unit
		float exchangeRatio = RmbYuan2NdBeanExchangeRatio;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2NdBeanExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2NdBeanExchangeRatio;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2NdBeanExchangeRatio;
		}
		
		double productPrice = exchangeRatio * data.currPrice;
		double productOriginalPrice = exchangeRatio * data.originalPrice;
		
		int productCnt = data.BasicData.Count;
		string payDescription = data.BasicData.Description;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		Debug.Log("[GUIVipStore]: PayForCommodity NdPlatform " + productPrice.ToString() + " and count " + productCnt);
		int nError = U3dNdUniPayAsyn(orderId, productId, productName, productPrice, productOriginalPrice, productCnt, payDescription);
		if (-1 == nError)
		{
			Debug.Log("[U3dNdSender]: ");
		}
#endif
	}
	
	public static void SearchPayResult(string orderId)
	{
	}
	
	public static void EnterNdPlatform()
	{
#if OPEN_ND91
		U3dNdEnterPlatform(0);
#endif
	}
	
	public static void EnterUserCenter()
	{
#if OPEN_ND91
		U3dNdEnterPersonalInfo();
#endif
	}
	
	public static void EnterTradeRecord()
	{
#if OPEN_ND91
		U3dNdEnterTradeRecord();
#endif
	}
	
	public static void Share2ThirdPlatform(string content, string imageFileName)
	{
#if OPEN_ND91
		U3dNdShare2ThirdPlatform(content, imageFileName);
#endif
	}
	
	public static void LeavePlatform()
	{
#if OPEN_ND91
		U3dNdLeavePlatform();
#endif
	}
	
	public static void EnterAppBBS()
	{
#if OPEN_ND91		
		U3dNdEnterAppBBS(0);	
#endif
	}
	
	public static void NdPauseGame()
	{
#if OPEN_ND91
		U3dNdPause(0);	
#endif
	}
	
	public static void NdUserFeedback()
	{
#if OPEN_ND91
		U3dNdUserFeedback(0);
#endif
	}
	
	public static string NdGetNetWork()
	{
		#if OPEN_ND91
		int netWorkCondition = U3dNdGetNetWork();
		if (netWorkCondition == -1)
			return "";
		else if (netWorkCondition == 0)
			return "Connected";
		#endif
		return "Connected";
	}
}