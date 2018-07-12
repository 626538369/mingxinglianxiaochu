using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class U3dGfaniOSReceiver : MonoBehaviour 
{
	// The Gfan ios sdk return value's key name
	// return bool value
	public static readonly string KeyResult = "result";
	public static readonly string KeyErrorCode = "StatusCode";
	public static readonly string KeyErrorMsg = "Message";
	public static readonly string KeyError = "error";
	
	public static readonly string KeyIsGuest = "isGuest";
	public static readonly string KeyUserName = "UserName";
	public static readonly string KeyUserId = "UID";
	public static readonly string KeyPassword = "PassWord";
	public static readonly string KeyEmail = "Email";
	public static readonly string KeyUserToken = "Token";
	public static readonly string KeyBalance = "Coupon";
	
	public static readonly string KeyOrderId = "OrderID";
	public static readonly string KeyPayName = "PayName";
	public static readonly string KeyPayDesc = "PayDesc";
	public static readonly string KeyPayValue = "PayCoupon";
	public static readonly string KeyCount = "Count";
	public static readonly string KeyPayAppKey = "PayAppKey";
	public static readonly string KeyInsideOrderID = "InsideOrderID";
	public static readonly string KeyAliPayOrderID = "AliPayOrderID";
	
	public static readonly string KeyiOSReceiptMD5 = "PayReceiptMD5";
	
	/*
	 * 
	 * KeyErrorCode = -1002 user status is wrong
	 * KeyErrorCode = -1009 user status is wrong
	 * KeyErrorCode = 10426 client appId is wrong
	 * */
	//----------------------------------------------------------------------
	const string majorDelimiter = "&";
	const string assignDelimiter = "=";
	
	Dictionary<string, string> receiveParams = new Dictionary<string, string>();
	bool ParseReceiveParams(string argvs)
	{
		try
		{
			receiveParams.Clear();
			
			string[] sections = argvs.Split(majorDelimiter.ToCharArray());
			foreach (string section in sections)
			{
				string[] keyValues = section.Split(assignDelimiter.ToCharArray());
				if (2 != keyValues.Length)
				{
					Debug.Log("[U3dGfanReceiver]: The parameters has some invalid data,early break.");
					return false;
				}
				else
				{
					receiveParams.Add(keyValues[0], keyValues[1]);
				}
			}
			
			return true;
		}
		catch (Exception e)
		{
			Debug.Log("[U3dGfanReceiver]: The parameters has some invalid data,early break.");
			return false;
		}
		
		return false;
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
			return bool.Parse(GetReceiveParam(KeyResult));
		} 
		catch (Exception e)
		{
			Debug.Log("[U3dGfanReceiver]: The parameters has some invalid data,early break.");
			return false;
		}
		
		return false;
	}
	//----------------------------------------------------------------------
	
	void OnInitSDK(string args)
	{
		ThirdPartyPlatform.OnInitSDK(true);
	}
	
	void OnCloseSDK(string args)
	{
		ThirdPartyPlatform.OnCloseSDK(true);
	}
	
	void OnLogin(string args)
	{
		ParseReceiveParams(args);
		
		Debug.Log("U3dGfaniSDK OnLogin args: " + args);
		
		long userId = -1;
		long.TryParse(GetReceiveParam(KeyUserId), out userId);
		
		ThirdPartyPlatform.UserUniqId = userId.ToString();
		ThirdPartyPlatform.SessionId = "";
		ThirdPartyPlatform.NickName = GetReceiveParam(KeyUserName);
		
		// get the guest state
		string keyGuestValue = GetReceiveParam(KeyIsGuest);
		U3dGfaniOSSender.IsGuestAccount = keyGuestValue.Equals("1") || string.Compare(keyGuestValue,"true",true) == 0;
		
		ThirdPartyPlatform.OnLogin(IsSuccess(),GetReceiveParam(KeyResult));
	}
	
	void OnLogout(string args)
	{
		ParseReceiveParams(args);
		ThirdPartyPlatform.OnLogout(IsSuccess());
	}
	
	void OnPayForOrder(string args)
	{
		ParseReceiveParams(args);
		
		string orderId = GetReceiveParam(KeyOrderId);
		string payName = GetReceiveParam(KeyPayName);
		string payDescription = GetReceiveParam(KeyPayDesc);
		string gfanMoney = GetReceiveParam(KeyPayValue);
		string count = "1"; // GetReceiveParam(KeyCount);
		
		if (IsSuccess())
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), orderId);
			Statistics.INSTANCE.ChargeEvent(orderId, "GFanApple", payName, count, gfanMoney);
		}
		else
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), "");
		}
	}
	
	void OnPayForOrderIAP(string args)
	{
		ParseReceiveParams(args);
		
		string orderId = GetReceiveParam(KeyOrderId);
		string payName = GetReceiveParam(KeyPayName);
		string receiptMD5 = GetReceiveParam(KeyiOSReceiptMD5);
		string count = "1"; // GetReceiveParam(KeyCount);
		
		if (IsSuccess())
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), orderId);
			// Statistics.INSTANCE.ChargeEvent(orderId, "GFanApple", payName, count, gfanMoney);
		}
		else
		{
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), "");
		}
	}
}
