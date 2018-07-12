using UnityEngine;
using System.Collections;

public class XiaomiSDKAgent : MonoBehaviour 
{
	const string JavaClassUnity2MiSDK = "com.gfan.game.battleatlantic_mi.U3dMiSDK";
	
	#region Sender
	static readonly int AppId = 13684;
	static readonly string AppKey = "1b5f3c53-7202-a297-9e66-519b3cbffe2d";
	
	static readonly int Landscape = 0;
	static readonly int Portrait = 1;
	
	
	public static void InitSDK()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2MiSDK,"miInitSDK", AppId, AppKey, Landscape);
	}
	
	public static void CloseSDK()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2MiSDK,"miCloseSDK");
	}
	
	public static void Login()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2MiSDK,"miLogin");
	}
	
	public static void Logout()
	{
		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2MiSDK,"miLogout");
	}
	
	public static bool IsGuestLogined()
	{
		return false;
	}
		
	public static readonly float RmbYuan2Mibi = 1.0f;
	public static void PayForCommodity(CommodityData data)
	{
		string roleName = Globals.Instance.MGameDataManager.MActorData.BasicData.Name;
		int level = Globals.Instance.MGameDataManager.MActorData.BasicData.Level;
		
		float exchangeRatio = RmbYuan2Mibi;
		if (data.currency == CurrencyType.RmbYuan)
		{
			exchangeRatio = RmbYuan2Mibi;
		}
		else if (data.currency == CurrencyType.RmbJiao)
		{
			exchangeRatio = 0.1f * RmbYuan2Mibi;
		}
		else if (data.currency == CurrencyType.RmbFen)
		{
			exchangeRatio = 0.01f * RmbYuan2Mibi;
		}
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		int productCnt = data.BasicData.Count;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		int mibi = (int)Mathf.CeilToInt(productPrice);

		HelpUtil.AndroidNativeCallStatic(JavaClassUnity2MiSDK,"miUniPayOnline", data.orderId, mibi);	
	}
	#endregion
	
	#region Receiver
	void OnSDKCallback(string jsonstr)
	{
		Debug.Log("MiSdk: OnSDKCallback");
		JsonData json = JsonMapper.ToObject(jsonstr);
		
		// Parse the json string
		string callbackType = (string)json[SDKConstants.KEY_CALLBACK_METHOD];
		
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
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		// JsonData data = json[SDKConstants.KEY_JSON_DATA];
		if (code == MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS)
		{
			ThirdPartyPlatform.OnInitSDK(true);
		}
	}
	
	void OnCloseSDK(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		// JsonData data = json[SDKConstants.KEY_JSON_DATA];
		if (code == MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS)
		{
			ThirdPartyPlatform.OnCloseSDK(true);
		}
	}
	
	void OnLogin(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch( code )
		{
		case MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS:
			JsonData data = json[SDKConstants.KEY_JSON_DATA];
			if (null == data)
			{
				Debug.Log("Get error information form Android native OnLogin");
				break;
			}
			// Debug.Log("MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS data.IsObject : " + data.IsObject);
			// Debug.Log("MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS data.IsArray : " + data.IsArray);
			Debug.Log("MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS data.Count : " + data.Count);
			
			string userId = (string)data[SDKConstants.KEY_USER_ID];
			string sessionId = (string)data[SDKConstants.KEY_USER_SESSION_ID];
			string nickName = (string)data[SDKConstants.KEY_USER_NICKNAME];
			
			ThirdPartyPlatform.UserUniqId = userId;
			ThirdPartyPlatform.SessionId = sessionId;
			ThirdPartyPlatform.NickName = nickName;
			
			ThirdPartyPlatform.OnLogin(true);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_LOGIN_FAIL:
			// 登陆失败
			ThirdPartyPlatform.OnLogin(false);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CANCEL:
			// 取消登录
			ThirdPartyPlatform.OnLogin(false);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_ACTION_EXECUTED:
			//登录操作正在进行中
			ThirdPartyPlatform.OnLogin(false);
			break;
		default:
			// 登录失败
			break;
		}
	}
	
	void OnLogout(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch(code)
		{
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_LOGINOUT_FAIL:
			// 注销失败
			ThirdPartyPlatform.OnLogout(false);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_LOGINOUT_SUCCESS:
			// 注销成功
			ThirdPartyPlatform.OnLogout(true);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_ACTION_EXECUTED:
			//操作正在进行中
			break;
		default:
			break;
		}
	}
	
	void OnPayProduct(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch( code ) 
		{
		case MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS:
			//购买成功
			JsonData data = json[SDKConstants.KEY_JSON_DATA];
			
			string orderId = (string)data[SDKConstants.KEY_ORDER_ID];
			int mibi = (int)data[SDKConstants.KEY_PAY_MONEY];
			string customInfo = (string)data[SDKConstants.KEY_PAY_CUSTOM_INFO];
			
			ThirdPartyPlatform.OnPayForCommodity(true, orderId);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_PAY_CANCEL:
			//取消购买
			ThirdPartyPlatform.OnPayForCommodity(false);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_PAY_FAILURE:
			//购买失败
			ThirdPartyPlatform.OnPayForCommodity(false);
			break;
		case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_ACTION_EXECUTED:
			//操作正在进行中
			ThirdPartyPlatform.OnPayForCommodity(false);
			break;
		default:
			//购买失败
			break;
		}
	}
	
	// Not support now
	void OnCardPayProduct(JsonData json)
	{
		int code = (int)json[SDKConstants.KEY_ERROR_CODE];
		switch( code )
		{
			case MiErrorCode.MI_XIAOMI_GAMECENTER_SUCCESS:
				// 成功
			break;
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_PAY_FAILURE: // 失败
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_PAYFORCARD_FAILE: // 充值卡充值失败
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CARDNUMERROR: // 卡号空
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CARDPASSERROR: // 密码空
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CARDCARRISNULL: // 运营商空
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CARDNUMORPASSERROR: // 卡号或密码错误
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CARDMONEYERROR: // 金额错误
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_CREATEORDERFAIL: // 创建充值订单错误
				// 失败 以上错误都可以认为是充值失败
			break;
			case MiErrorCode.MI_XIAOMI_GAMECENTER_ERROR_LOGIN_FAIL:
			break;
			
			default:
			break;
		}
	}
	
	void OnRecharge(JsonData json)
	{
		
	}
	
	void OnImproveInformation(JsonData json)
	{
		
	}
	#endregion
}
