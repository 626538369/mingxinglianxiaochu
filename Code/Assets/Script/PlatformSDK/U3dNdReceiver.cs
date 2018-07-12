using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class U3dNdReceiver : MonoBehaviour 
{
	//-------------------------------------------------------------------------------
	// return value type is int or bool
	public static readonly string KeyResult = "result";
	
	// If the error return value is 0, then API is callback success, return value type is int
	public static readonly string KeyError = "error";
	
	// An enum value, return value type is int
	public static readonly string KeyUpdateResult = "updateResult";
	
	// bSuccess return this order payment is success? return value type is bool
	public static readonly string KeyIsPaySuccess = "isPaySuccess";
	
	// return value type is string
	public static readonly string KeyOrderId = "cooOrderSerial";
	// return value type is string
	public static readonly string KeyProductId = "productId";
	// return value type is string
	public static readonly string KeyProductName = "productName";
	
	// return value type is float
	public static readonly string KeyProductPrice = "productPrice";
	// return value type is float
	public static readonly string KeyProductOrigPrice = "productOrignalPrice";
	
	// return value type is int
	public static readonly string KeyProductCount = "productCount";
	// return value type is string
	public static readonly string KeyPayDescription = "payDescription";
	//-------------------------------------------------------------------------------
	
	public enum U3dNdErrorCode
	{
		Success = 1,
		
		Login = 100,
		LoginGuest,
		LoginGuestRegister,
		
		PayFailure = 200,
		PayCancle,
		PayAsynSMSSend,
		PayOrderSubmitted,
		
		SearchNonOrder = 300,
		SearchPaySuccess,
		SearchPayFailure,
		
		LeavePlatform = 400,
	}
	
	public static ThirdPartyPlatform.ErrorCode Convert2ErrorCode(int code)
	{
		switch ((U3dNdErrorCode)code)
		{
		case U3dNdErrorCode.Success:
			return ThirdPartyPlatform.ErrorCode.Success;
		case U3dNdErrorCode.Login:
			return ThirdPartyPlatform.ErrorCode.Success;
		default:
			return ThirdPartyPlatform.ErrorCode.Success;
		}
	}
	
	//-------------------------------------------------------------------------------
	const string majorDelimiter = "&";
	const string assignDelimiter = "=";
	//-------------------------------------------------------------------------------
	
	//-------------------------------------------------------------------------------
	Dictionary<string, string> receiveParams = new Dictionary<string, string>();
	bool ParseReceiveParams(string argvs)
	{
		receiveParams.Clear();
		
		if (string.IsNullOrEmpty(argvs))
		{
			Debug.Log("[U3d2NdReceiver]: The parameters has some invalid data,early break.");
			return false;
		}
		
		string[] sections = argvs.Split(majorDelimiter.ToCharArray());
		foreach (string section in sections)
		{
			string[] keyValues = section.Split(assignDelimiter.ToCharArray());
			if (2 != keyValues.Length)
			{
				Debug.Log("[U3d2NdReceiver]: The parameters has some invalid data,early break.");
				return false;
			}
			else
			{
				receiveParams.Add(keyValues[0], keyValues[1]);
			}
		}
		
		return true;
	}
	
	string GetReceiveParam(string key)
	{
		string val = "";
		receiveParams.TryGetValue(key, out val);
		
		return val;
	}
	
	bool IsSuccess()
	{
		try
		{
			string result = GetReceiveParam(KeyResult);
			return (int)U3dNdErrorCode.Success ==  int.Parse(result);
		} 
		catch (Exception e)
		{
			Debug.Log("[U3dNdReceiver]: The parameters has some invalid data,early break.");
			
			return false;
		}
		
		return false;
	}
	
	/*
	 * All callback methods from 91SDK
	 * */
	public void initSdkNotify(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("[U3d2NdReceiver]: initSdkNotify------------------- the result is " + result);
		
		ThirdPartyPlatform.OnInitSDK(IsSuccess());
	}
	
	public void closeSdkNotify(string args)
	{
	}
	
	public void loginNotify(string args)
	{
		ParseReceiveParams(args);
		
		// result it 1 = success or 0 = fail
		string result = GetReceiveParam(KeyResult);
		string error = GetReceiveParam(KeyError);
		Debug.Log("[U3d2NdReceiver]: loginNotify------------------- the result is " + result);
		Debug.Log("[U3d2NdReceiver]: loginNotify------------------- the error is " + error);
		
		if (U3dNdSender.IsLogined())
		{
			if (string.IsNullOrEmpty(ThirdPartyPlatform.CacheUserUniqId))
			{
				// First login
				ThirdPartyPlatform.OnLogin(IsSuccess());
				Debug.Log("first login id is " + ThirdPartyPlatform.CacheUserUniqId);
			}
			else if (!U3dNdSender.GetUserUniqId().Equals(ThirdPartyPlatform.CacheUserUniqId))
			{
				// User switch account
				TalkingDataGA.Logout();
				Globals.Instance.Restart();
				
				Debug.Log("switch account login id is " + ThirdPartyPlatform.CacheUserUniqId);
			}
			else
			{
				ThirdPartyPlatform.OnLogin(IsSuccess());
			}
			ThirdPartyPlatform.CacheUserUniqId = U3dNdSender.GetUserUniqId();
		}
		else
		{
			if (!string.IsNullOrEmpty(ThirdPartyPlatform.CacheUserUniqId))
			{
				// Logout event
				TalkingDataGA.Logout();
				Globals.Instance.Restart();
				
				Debug.Log("Log out id is " + ThirdPartyPlatform.CacheUserUniqId);
			}
			else
			{
				// Not login
				// if (Globals.Instance.MGUIManager)
				// {
				// 	string wordText = Globals.Instance.MDataTableManager.GetWordText(23200001); // "Confirm quit game";
				// 	Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
				// 	{
				// 		gui.SetTextAnchor(ETextAnchor.MiddleCenter,false);
				// 		gui.SetDialogType(EDialogType.QUIT_GAME, wordText);
				// 	}
				// 	, EDialogStyle.DialogOkCancel
				// 	);
				// }
				
				// Logout event
				TalkingDataGA.Logout();
				Globals.Instance.Restart();
			}
			
			ThirdPartyPlatform.CacheUserUniqId = "";
			ThirdPartyPlatform.UserUniqId = "";
			ThirdPartyPlatform.SessionId = "";
			ThirdPartyPlatform.NickName = "";
		}
	}
	
	public void registerNotify(string args)
	{
		ParseReceiveParams(args);
		ThirdPartyPlatform.OnRegister(IsSuccess());
	}
	
	public void logoutNotify(string args)
	{
		ParseReceiveParams(args);
		ThirdPartyPlatform.OnLogout(IsSuccess());
	}
	
	public void paymentNotify(string args)
	{
		ParseReceiveParams(args);
		
		string result = GetReceiveParam(KeyResult);
		string error = GetReceiveParam(KeyError);
		
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the result is " + result);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the error is " + error);
		
		string orderId = GetReceiveParam(KeyOrderId);
		string productId = GetReceiveParam(KeyProductId);
		string productName = GetReceiveParam(KeyProductName);
		string price = GetReceiveParam(KeyProductPrice);
		string origPrice = GetReceiveParam(KeyProductOrigPrice);
		string count = GetReceiveParam(KeyProductCount);
		string payDescription = GetReceiveParam(KeyPayDescription);
		
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the orderId is " + orderId);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the productId is " + productId);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the productName is " + productName);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the price is " + price);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the origPrice is " + origPrice);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the count is " + count);
		
		if (IsSuccess())
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), orderId);
			
			Statistics.INSTANCE.ChargeEvent(orderId,"91",productName,count,price);
		}
		else
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), orderId);
		}
		
		
		/*
		 * switch(code){
					case NdErrorCode.ND_COM_PLATFORM_SUCCESS:
					Toast.makeText(mCtx, "购买成功", Toast.LENGTH_SHORT).show();
					break;
				case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_FAILURE:
					Toast.makeText(mCtx, "购买失败", Toast.LENGTH_SHORT).show();
					break;
				case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_CANCEL:
					Toast.makeText(mCtx, "取消购买", Toast.LENGTH_SHORT).show();
					break;
				case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_ASYN_SMS_SENT:
					Toast.makeText(mCtx, "订单已提交，充值短信已发送", Toast.LENGTH_SHORT).show();
					break;
				case NdErrorCode.ND_COM_PLATFORM_ERROR_PAY_REQUEST_SUBMITTED:
					Toast.makeText(mCtx, "订单已提交", Toast.LENGTH_SHORT).show();
					break;
				default:
					Toast.makeText(mCtx, "购买失败", Toast.LENGTH_SHORT).show();
				}
		 * */
	}
	
	public void rechargeNotify(string args)
	{
		// ParseReceiveParams(args);
		// ThirdPartyPlatform.OnRecharge(IsSuccess());
	}
	
	public void leavePlatformNotify(string args)
	{
		// ParseReceiveParams(args);
		// ThirdPartyPlatform.OnLeavePlatform(IsSuccess());
	}
	
	public void sessionInvalidNotify(string args)
	{
		ParseReceiveParams(args);
		// IsSuccess();
	}
	
	public void appVersionUpdateNotify(string args)
	{
		ParseReceiveParams(args);
		
		// 自己处理更新接口回调的逻辑
		string updateResult = GetReceiveParam(KeyUpdateResult);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the updateResult is " + updateResult);
		
		ThirdPartyPlatform.OnNdCheckAppVersion(updateResult);
	}
	
	public void searchPayResultNotify(string args)
	{
		ParseReceiveParams(args);
		
		string isPaySuccess = GetReceiveParam(KeyIsPaySuccess);
		bool bIsPaySuccess = string.IsNullOrEmpty(isPaySuccess) ? false : bool.Parse(isPaySuccess);
		
		string orderId = GetReceiveParam(KeyOrderId);
		string productId = GetReceiveParam(KeyProductId);
		string productName = GetReceiveParam(KeyProductName);
		string price = GetReceiveParam(KeyProductPrice);
		string origPrice = GetReceiveParam(KeyProductOrigPrice);
		string count = GetReceiveParam(KeyProductCount);
		string payDescription = GetReceiveParam(KeyPayDescription);
		
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the orderId is " + orderId);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the productId is " + productId);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the productName is " + productName);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the price is " + price);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the origPrice is " + origPrice);
		Debug.Log("[U3d2NdReceiver]: paymentNotify------------------- the count is " + count);
		
		if (IsSuccess())
		{
			ThirdPartyPlatform.OnSearchPayResult(IsSuccess(), bIsPaySuccess, orderId);
		}
		else
		{
			ThirdPartyPlatform.OnSearchPayResult(IsSuccess(), bIsPaySuccess, orderId);
		}
		
		// ThirdPartyPlatform.ErrorCode resultCode = U3dNdSender.Convert2ResultCode(0);
		// 
		// string orderId = "";
		// ThirdPartyPlatform.OnSearchPayResult(resultCode, orderId);
		/*
		 * switch(responseCode){
			case NdErrorCode.ND_COM_PLATFORM_SUCCESS://查询成功
				if(isPay){
					//购买成功
				}else{
				//购买失败
				}
				break;
			case NdErrorCode.ND_COM_PLATFORM_ERROR_UNEXIST_ORDER:
				//无此订单
				break;
			default :
				//查询失败
			}
		 * */
	}
	
	
}
