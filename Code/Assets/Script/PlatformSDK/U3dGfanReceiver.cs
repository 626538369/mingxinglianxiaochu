using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class U3dGfanReceiver : MonoBehaviour 
{
	/// <summary>
	/// Copy the constants define from Android project
	/// </summary>
	//----------------------------------------------------------------------
	public static readonly string KEY_RESULT = "Result";
	public static readonly string KEY_ERROR_CODE = "ErrorCode";
	
	public static readonly string KEY_GUI = "Gui";
	
	public static readonly string KEY_USER_NAME = "UserName";
	public static readonly string KEY_USER_ID = "UserId";
	
	public static readonly string KEY_ORDER_ID = "OrderId";
	public static readonly string KEY_PAY_NAME = "PayName";
	public static readonly string KEY_PAY_DESCRIPTION = "PayDescription";
	public static readonly string KEY_PAY_MONEY = "PayMoney";
	public static readonly string KEY_PRODUCT_COUNT = "ProductCnt";

	public static readonly string KEY_GOOGLE_PURCHASE_DATA = "GooglePurchaaseData";
	public static readonly string KEY_GOOGLE_PURCHASE_SIGNATURE = "GooglePurchaaseSignature";

	// Code values
	public static readonly bool B_SUCCESS = true;
	public static readonly bool B_FAILURE = false;
	
	public static readonly int GUI_LOGIN = 100;
	public static readonly int GUI_REGISTER = 101;
	public static readonly int GUI_MODIFY = 102;
	public static readonly int GUI_PAYMENT = 103;
	public static readonly int GUI_RECHARGE = 104;
	
	public static readonly int ERR_UNKNOW = 1001;
	public static readonly int ERR_NOT_LOGINED = 1002;
	//----------------------------------------------------------------------
	
	//----------------------------------------------------------------------
	public static ThirdPartyPlatform.ErrorCode Convert2ResultCode(int code)
	{
		if (code == ERR_NOT_LOGINED)
			return ThirdPartyPlatform.ErrorCode.NotLogin;
		
		return ThirdPartyPlatform.ErrorCode.Success;
	}
	//----------------------------------------------------------------------
	
	
	//----------------------------------------------------------------------
	const string majorDelimiter = "&";
	const string assignDelimiter = "#";
	
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
			return bool.Parse(receiveParams[KEY_RESULT]);
		} 
		catch (Exception e)
		{
			Debug.Log("[U3dGfanReceiver]: The parameters has some invalid data,early break.");
			return false;
		}
		
		return false;
	}
	//----------------------------------------------------------------------
	
	public void initSdkNotify(string args)
	{
		U3dGfanSender.ShowToast("initSdkNotify finishi...");
		ThirdPartyPlatform.OnInitSDK(true);
	}
	
	public void closeSdkNotify(string args)
	{
		U3dGfanSender.ShowToast("closeSdkNotify finishi...");
		ThirdPartyPlatform.OnCloseSDK(true);
	}

	public void SavePhotoSucess(string args)
	{
		GUIRadarScan.Hide();
		
		if (args.Contains("failed"))
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(10013);
		}
		else
		{
			Debug.Log("SavePhotoSucess!!!!!!!!!!!!!");
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4011);
		}
	}
	
	public void SavePhotoFailed(string args)
	{
		GUIRadarScan.Hide();
		Globals.Instance.MGUIManager.ShowSimpleCenterTips(10012);
	}
	
	public void loginNotify(string args)
	{
		if (!ParseReceiveParams(args))
		{
			U3dGfanSender.ShowToast("loginNotify ParseReceiveParams error...");
		}
		
		U3dGfanSender.ShowToast("loginNotify finishi...IsSuccess " + IsSuccess());
		ThirdPartyPlatform.OnLogin(IsSuccess());
	}
	
	public void registerNotify(string args)
	{
		ParseReceiveParams(args);
		
		U3dGfanSender.ShowToast("registerNotify finishi...IsSuccess " + IsSuccess());
		ThirdPartyPlatform.OnRegister(IsSuccess());
	}
	
	public void modifyUserInfoNotify(string args)
	{
		ParseReceiveParams(args);
		
		U3dGfanSender.ShowToast("modifyUserInfoNotify finishi...IsSuccess " + IsSuccess());
		ThirdPartyPlatform.OnModifyUserInfo(IsSuccess());
	}
	
	public void logoutNotify(string args)
	{
		ParseReceiveParams(args);
		
		U3dGfanSender.ShowToast("logoutNotify finishi...IsSuccess " + IsSuccess());
		ThirdPartyPlatform.OnLogout(IsSuccess());
	}
	
	public void paymentNotify(string args)
	{
		ParseReceiveParams(args);
		
		U3dGfanSender.ShowToast("paymentNotify finishi...IsSuccess " + IsSuccess());
		if (IsSuccess())
		{
			string orderId = receiveParams[KEY_ORDER_ID];
			string payName = receiveParams[KEY_PAY_NAME];
			string payDescription = receiveParams[KEY_PAY_DESCRIPTION];
			string gfanMoney = receiveParams[KEY_PAY_MONEY];
			string count = receiveParams[KEY_PRODUCT_COUNT];
			
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), orderId);
			
			Statistics.INSTANCE.ChargeEvent(orderId,"GFan",payName,count,gfanMoney);
		}
		else
		{
			string error = receiveParams[KEY_ERROR_CODE];
			
			ThirdPartyPlatform.OnPayForCommodity(IsSuccess(), "");
		}
	}
	
	public void rechargeNotify(string args)
	{
		ParseReceiveParams(args);
		
		U3dGfanSender.ShowToast("rechargeNotify finishi...IsSuccess " + IsSuccess());
		ThirdPartyPlatform.OnRecharge(IsSuccess());
	}

	public void AliPaySuccess(){

		GUIRadarScan.Hide ();
	}

	public void leavePlatformNotify(string args)
	{
		ParseReceiveParams(args);
		ThirdPartyPlatform.OnLeavePlatform(IsSuccess());
	}
	
	public void appVersionUpdateDidFinish(string args)
	{
		ParseReceiveParams(args);
	}
	
	public void searchPayResultDidFinish(string args)
	{
		ParseReceiveParams(args);
		// ThirdPartyPlatform.OnLeavePlatform(IsSuccess());
	}

	public void ReceiveGooglePurchase(string args)
	{
		//Debug.Log("ReceiveGooglePurchase args is :" + args);
		ParseReceiveParams(args);
		
		string orderId = ShopDataManager.PayCommodityData.orderId;
		string purchase_data = receiveParams[KEY_GOOGLE_PURCHASE_DATA];
		string purchase_signature=receiveParams[KEY_GOOGLE_PURCHASE_SIGNATURE];

		NetSender.Instance.C2GSPayGooglePlayReq(orderId, purchase_data, purchase_signature);
	}

	public void SharePhotoSuccess(string args)
	{
		GUIRadarScan.Hide();
		Globals.Instance.MGUIManager.ShowSimpleCenterTips(4012);

		Debug.Log ("BOOL" + Globals.Instance.MTaskManager.IsGetShareReward);
		Debug.Log ("BOOL" + Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState);
		if(Globals.Instance.MTaskManager.IsGetShareReward && Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState)
		{
			GUIRadarScan.Show();
			NetSender.Instance.ShareCountInfoReq(1);
		}
	}

	public void ShareURLSuccess(string args)
	{
		GUIRadarScan.Hide();
		Globals.Instance.MGUIManager.ShowSimpleCenterTips(4012);
		
//		GUIPhotoGraph guiPhotoGraph = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph>();
//		if(guiPhotoGraph != null)
//		{
//			guiPhotoGraph.SetTaskShare();
//		}
	}
	
	public void SharePhotoFail(string args)
	{
		GUIRadarScan.Hide();
		Globals.Instance.MGUIManager.ShowSimpleCenterTips(4013);
	}

	public void macAdressIDAF(string idafstr)
	{
		if ("empty" != idafstr)
		{
			GameDefines.systemIFAD = idafstr;
			GameDefines.systemUDID = idafstr;
		}
	}

}
