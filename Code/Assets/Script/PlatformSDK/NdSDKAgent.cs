using UnityEngine;
using System.Collections;

public class NdSDKAgent : MonoBehaviour 
{
	const string JavaClassUnity2NdSDK = "com.xiamiyou.qiaojianghu_91.U3dNdSDK";
	const string JavaClassUnityPlayer = "com.unity3d.player.UnityPlayer";
	const string CurrActivityField = "currentActivity";
	
	static readonly int AppId = 107535;
	static readonly string AppKey = "e9fdf257feae9c6ec60bd060f61f7ef76bac65f065e9ca0d";
	
	public static long CacheUserUniqId = -1;
	
	public enum NdScreenOrientation
	{
		Protrait,
		Landscape,
	}
	
	public enum NdAccountState
	{
		NotLogin,
		GuestLogin,
		NormalLogin,
	}
		
	public static string GetAppChannelId()
	{
		return "Nd91";
	}
	
	public static void InitSDK()
	{
		Debug.Log("NdSDKAgent:InitSDK");
		ThirdPartyPlatform.OnInitSDK(true);
		
		// init SDK before in java native
		// com.gfan.game.battleatlantic_91.U3dNdSDK
		//
		//Debug.Log("NdSDKAgent:InitSDK");
		//AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndInitSDK", AppId, AppKey, DebugMode, (int)NdScreenOrientation.Landscape);
	}
	
	public static void CloseSDK()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndCloseSDK");
	}
	
	public static void Login()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndLogin");
	}
	
	public static void Logout(bool cancelAutoLogin)
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndLogout",cancelAutoLogin);
	}
	
	public static bool IsLogined()
	{
		return HelpUtil.AndroidNativeCallStatic<bool>(JavaClassUnity2NdSDK,"ndIsLogined");
	}
	
	public static bool IsGuestLogined()
	{
		// status 
		// 0: not login
		// 1: account logined
		// 2: geust logined
		//
		int status = HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndGetLoginStatus");
		Debug.Log("ndGetLoginStatus == " + status);
		return status == 2;
	}
	
	public static string GetUserUniqId()
	{
		return HelpUtil.AndroidNativeCallStatic<string>(JavaClassUnity2NdSDK,"ndGetUserUniqId");
	}
	
	public static void AppVersionUpdate()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndAppVersionUpdate");
	}
	
	public static void GuestRegister()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndGuestRegister");
	}
	
	public static void EnterUserCenter()
	{
	}
	
	public static void OpenCharge()
	{
		HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndEnterRecharge");
	}
	
	public static readonly float RmbYuan2NdBean = 1.0f;
	public static void PayForCommodity(CommodityData data)
	{
		string orderId = data.orderId;
		string productId = data.ItemID.ToString();
		string productName = data.BasicData.Name;
		string payDescription = data.BasicData.Description;
		
		float exchangeRatio = RmbYuan2NdBean;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2NdBean;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2NdBean;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2NdBean;
		}
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		
		int productCnt = data.BasicData.Count;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		

		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndUniPayAysn", orderId, productId, productName, productPrice, productOriginalPrice, productCnt, payDescription);	
	}
	
	public static void EnterNdPlatform()
	{
		HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndEnterPlatform");
	}
	
	public static void EnterNdGameZone()
	{
		HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndEnterAppCenter", AppId);
	}
	
	public static void EnterNdGameBBS()
	{
		HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndEnterAppBBS");
	}
	
	public static void EnterNdPauseApp()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndPauseApp");
	}
	
	public static void EnterNdUserFeedback()
	{
		HelpUtil.AndroidNativeCallStatic<int>(JavaClassUnity2NdSDK,"ndUserFeedback");
	}
	
	public static void EnderUserSettings(){
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2NdSDK,"ndEnterUserSetting",0);
	}
	
	
	void OnSDKCallback(string jsonstr)
	{
		Debug.Log("NdSDKAgent:OnSDKCallback - jsonstr " + jsonstr);
		JsonData json = JsonMapper.ToObject(jsonstr);
		
		// Parse the json string
		string callbackType = (string)json[SDKConstants.KEY_CALLBACK_METHOD];
		Debug.Log("NdSDKAgent: OnSDKCallback/callbackType is " + callbackType);
		
		switch (callbackType) {
		case SDKConstants.CALLBACK_METHOD_OnInitSDK:
			OnInitSDK(json);
			break;

		case SDKConstants.CALLBACK_METHOD_OnCloseSDK:
			OnCloseSDK(json);
			break;

		case SDKConstants.CALLBACK_METHOD_OnLogin:
			OnLogin(json);
			break;

		case SDKConstants.CALLBACK_METHOD_OnLogout:
			OnLogout(json);
			break;
			
		case SDKConstants.CALLBACK_METHOD_OnRegister:
			OnRegister(json);
			break;	
		case SDKConstants.CALLBACK_METHOD_OnAppVersionUpdate:
			OnAppVersionUpdate(json);
			break;	
			
		case SDKConstants.CALLBACK_METHOD_OnPayProduct:
			OnPayProduct(json);
			break;

		case SDKConstants.CALLBACK_METHOD_OnRecharge:
			OnRecharge(json);
			break;

		case SDKConstants.CALLBACK_METHOD_OnImproveInformation:
			OnImproveInformation(json);
			break;
		}
	}
	
	void OnInitSDK(JsonData json)
	{
		Debug.Log("NdSDKAgent: OnInitSDK");
		ThirdPartyPlatform.OnInitSDK(true);
	}
	
	void OnCloseSDK(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		ThirdPartyPlatform.OnCloseSDK(true);
	}
	
	void OnLogin(JsonData json)
	{
		Debug.Log("NdSDKAgent: OnLogin");
		
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch(code){
		case NdErrorCode.ND_COM_PLATFORM_SUCCESS:

			JsonData data = json[SDKConstants.KEY_JSON_DATA];
			string userId = (string)data[SDKConstants.KEY_USER_ID];
			string sessionId = (string)data[SDKConstants.KEY_USER_SESSION_ID];
			string nickName = (string)data[SDKConstants.KEY_USER_NICKNAME];
			
			ThirdPartyPlatform.UserUniqId = userId;
			ThirdPartyPlatform.SessionId = sessionId;
			ThirdPartyPlatform.NickName = nickName;
			
			ThirdPartyPlatform.OnLogin(true);
			// Log.e("91","登陆成功 " + userId);
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_LOGIN_FAIL:
			// Log.e("91","登陆失败");
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_CANCEL:
			// Log.e("91","用户取消登陆");
			break;
		default:
			break;
		}
	}
	
	void OnLogout(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		if (code == NdErrorCode.ND_COM_PLATFORM_SUCCESS)
		{
			ThirdPartyPlatform.OnLogout(true);
		}
	}
	
	void OnRegister(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch(code){
		case NdErrorCode.ND_COM_PLATFORM_SUCCESS:

			JsonData data = json[SDKConstants.KEY_JSON_DATA];
			string userId = (string)data[SDKConstants.KEY_USER_ID];
			string sessionId = (string)data[SDKConstants.KEY_USER_SESSION_ID];
			string nickName = (string)data[SDKConstants.KEY_USER_NICKNAME];
			
			ThirdPartyPlatform.OnRegister(true);
			// Log.e("91","登陆成功 " + userId);
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_LOGIN_FAIL:
			// Log.e("91","登陆失败");
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_CANCEL:
			// Log.e("91","用户取消登陆");
			break;
		default:
			break;
		}
	}
	
	void OnAppVersionUpdate(JsonData json)
	{
		ThirdPartyPlatform.isNdCheckintVer = false;
		
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		Debug.Log("NdSDKAgent: OnAppVersionUpdate/code " + code);
		switch(code)
		{
		case NdErrorCode.UPDATESTATUS_NONE:
			// Log.e("91","没有更新");
			ThirdPartyPlatform.Login(true);
			break;
		case NdErrorCode.UPDATESTATUS_UNMOUNTED_SDCARD:
			// Log.e("91","没有SD卡");
			break;
		case NdErrorCode.UPDATESTATUS_CANCEL_UPDATE:
			// Log.e("91","用户取消普通更新");
			Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
			{
				gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
				gui.SetDialogType(EDialogType.ND_NORMAL_UPDATE);
			}, EDialogStyle.DialogOkCancel, delegate() 
			{
				ThirdPartyPlatform.Login(true);
			});
			break;
		case NdErrorCode.UPDATESTATUS_CHECK_FAILURE:
			// Log.e("91","新版本检测失败");
			break;
		case NdErrorCode.UPDATESTATUS_FORCES_LOADING:
			// Log.e("91","强制更新正在下载");
			break;
		case NdErrorCode.UPDATESTATUS_RECOMMEND_LOADING:
			// Log.e("91","普通更新正在下载");
			break; 
		default:
			// Log.e("91","检查更新失败DEFAULT");
			break;
		}
	}
	
	void OnPayProduct(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch(code)
		{
		case NdErrorCode.ND_COM_PLATFORM_SUCCESS:
			JsonData data = json[SDKConstants.KEY_JSON_DATA];
			
			string orderId = (string)data[SDKConstants.KEY_ORDER_ID];
			string product = (string)data[SDKConstants.KEY_PRODUCT_ID];
			string productName = (string)data[SDKConstants.KEY_PRODUCT_NAME];
			float price = (float)data[SDKConstants.KEY_PAY_MONEY];
			int count = (int)data[SDKConstants.KEY_PRODUCT_COUNT];
			
			ThirdPartyPlatform.OnPayForCommodity(true, orderId);
			// Toast.makeText(UnityPlayer.currentActivity, "购买成功", Toast.LENGTH_SHORT).show();
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_FAILURE:
			// Toast.makeText(UnityPlayer.currentActivity, "购买失败", Toast.LENGTH_SHORT).show();
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_CANCEL:
			// Toast.makeText(UnityPlayer.currentActivity, "取消购买", Toast.LENGTH_SHORT).show();
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_ASYN_SMS_SENT:
			// Toast.makeText(UnityPlayer.currentActivity, "订单已提交，充值短信已发送", Toast.LENGTH_SHORT).show();
			break;
		case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_REQUEST_SUBMITTED:
			// Toast.makeText(UnityPlayer.currentActivity, "订单已提交", Toast.LENGTH_SHORT).show();
			break;
		default:
			// Toast.makeText(UnityPlayer.currentActivity, "购买失败", Toast.LENGTH_SHORT).show();
			break;
		}
	}
		
	// Not support now
	void OnCardPayProduct(JsonData json)
	{
	}
	
	void OnRecharge(JsonData json)
	{
		ThirdPartyPlatform.OnRecharge(true);
	}
	
	void OnImproveInformation(JsonData json)
	{
		
	}
}
