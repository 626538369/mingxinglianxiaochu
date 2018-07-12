using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPartyPlatform 
{
	// Cache the user information
	public static string CacheUserUniqId = "";
	public static string UserUniqId = "";
		
	public static string SessionId = "";
	public static string NickName = "";
	
	// public static bool IsGuestLogined = false;
	public static bool mIs360Logining = false;
	
	public enum ErrorCode
	{
		Success = 0,
		
		Login = 100,
		NotLogin = 101,
		
		PayFailure = 200,
		PayCancle,
		PayAsynSMSSend,
		PayOrderSubmitted,
		
		SearchNonOrder = 300,
		SearchPaySuccess,
		SearchPayFailure,
		
		LeavePlatform = 400,
	}
	
	static bool hasInitSdk = false;
	public static bool isNdCheckintVer = false;
	public static void InitSDK()
	{
		Debug.Log("InitSDK called [" + GameDefines.OutputVerDefs + "] hasInitSdk [" + hasInitSdk + "]");
		
		if (hasInitSdk){
			// on init SDK called
			//
			OnInitSDK(true);			
			return;
		}
		
		nonePlatformBeginConnect ();
//		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
//		{
//			// Do nth. call OnInitSDK
//			nonePlatformBeginConnect();
//			//ThirdPartyPlatform.OnLogin(true);
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.None)
//		{
//			nonePlatformBeginConnect();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
//		{
//			nonePlatformBeginConnect();
//			//ThirdPartyPlatform.OnLogin(true);
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
//		{
//			nonePlatformBeginConnect();
//		}
		hasInitSdk = true;
	}
	
	public static void OnInitSDK(bool isSuccess)
	{
		Debug.Log("ThirdPartyPlatform::OnInitSDK!!!!!!");
		//if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone
		//	|| GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		//{
			//ThirdPartyPlatform.NdCheckAppVersion();
		//}
		//else
		 if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			Globals.Instance.MGUIManager.CreateWindow<GUIPreLogin>(delegate(GUIPreLogin gui)
			{
					gui.enterAccountGUI();
			});
		}
		{
			ThirdPartyPlatform.Login(true);
		}
	}
	
	public static void CloseSDK()
	{
//		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
//		{
//			U3dNdSender.CloseNdSDK();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
//		{
//			NdSDKAgent.CloseSDK();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
//		{
//			U3dGfanSender.CloseGfanSDK();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone
//			|| GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
//		{
//			U3dGfaniOSSender.CloseNdSDK();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
//			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone)
//		{
//			UCSDKAgent.CloseSDK();
//		}
//		else if (GameDefines.OutputVerDefs == OutputVersionDefs.MiAndroid
//			|| GameDefines.OutputVerDefs == OutputVersionDefs.MiiPhone)
//		{
//			XiaomiSDKAgent.CloseSDK();
//			
//		}

		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			//U3dAppStoreSender.CloseSDK();
		}

		hasInitSdk = false;
	}
	
	public static void OnCloseSDK(bool isSuccess)
	{
		
	}
	
	public static void Login(bool supportGuest)
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows 
			|| GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			Globals.Instance.MGUIManager.CreateWindow<GUIPreLogin>(delegate(GUIPreLogin gui)
			{
				Debug.Log("GUIPreLogin::UpdateGUI");
				gui.UpdateGUI();
			});
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			if (isNdCheckintVer)
			{
				// Please wait
			}
			else
			{
				U3dNdSender.Login(true);
			}
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			if (isNdCheckintVer)
			{
				// Please wait
			}
			else
			{
				NdSDKAgent.Login();
			}
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			U3dGfanSender.Login(true);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone)
		{
			U3dGfaniOSSender.Login(true);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone)
		{
			UCSDKAgent.Login();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.MiAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.MiiPhone)
		{
			XiaomiSDKAgent.Login();
			
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.PPiPhone){
			
			PPSDKAgent.Login();
		}
	}
	
	public static void OnLogin(bool isSuccess,string failedMsg = null)
	{
		string userUniId = GetUserUniId();
		Debug.Log("[ThirdPartyPlatform]: GetUserUniId is " + userUniId);
		
		if (isSuccess)
		{
			AtlanticLogin(IsGuestLogined(), userUniId, "");
		}
		else
		{
			// Request quit the gfan sdk
			//if (Globals.Instance.MGUIManager)
			//{
			//	string wordText = (!string.IsNullOrEmpty(failedMsg)) ? failedMsg : Globals.Instance.MDataTableManager.GetWordText(23200001); // "Confirm quit game";
			//	
			//	Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
			//	{
			//		gui.SetTextAnchor(ETextAnchor.MiddleCenter,false);
			//		gui.SetDialogType(EDialogType.QUIT_GAME, wordText);
			//		
			//	},EDialogStyle.DialogOkCancel);
			//}
			
			Debug.Log("[ThirdPartyPlatform]: OnLogin failed...");
		}
	}
	
	public static void AtlanticLogin(bool isGuest, string account, string password = "")
	{		
		GameDefines.Setting_IsGuest = isGuest;
		GameDefines.Setting_LoginName = account;
		GameDefines.Setting_LoginPass = password;
		
		//Globals.Instance.MStarter.PlayTitleMovie();
		
		Globals.Instance.MLSNetManager.Disconnect();
		Globals.Instance.MLSNetManager.BeginConnect(GameDefines.LOGIN_SERVER_IP, GameDefines.LOGIN_SERVER_PORT, delegate(NetManager.ConnectionState state) 
		{
			if(state == NetManager.ConnectionState.Connected)
			{
				// Check the version
				Debug.Log("RequestCheckChannelAndVersion::Send");
				Globals.Instance.MLSNetManager.RequestCheckChannelAndVersion(GameDefines.GameChannel, GameDefines.GameVersion);
			}
			else
			{
				if (Application.internetReachability == NetworkReachability.NotReachable
					|| Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) // 
				{
					Debug.Log("[ThirdPartyPlatform]: Cann't Connect the login server In AtlanticLogin");
						
					// The login server is not available or in maintenance
					Globals.Instance.PopupNetworkProblemDlg();
				}
				else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
				{
					// User network is ok, but the game is in maintaining
					// Application.OpenURL(GameDefines.OfficialSkipToUrl);
				}
			}
		});
	}
	
	public static void Register()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.Register();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			NdSDKAgent.GuestRegister();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			U3dGfanSender.Register();
		}
		else {
			
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void OnRegister(bool isSuccess)
	{}
	
	public static bool IsLogined()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
		{
			return !string.IsNullOrEmpty(GameDefines.Setting_LoginName);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			return U3dNdSender.IsLogined();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			return U3dGfanSender.IsLogined();
		}
		else
		{
			return !string.IsNullOrEmpty(UserUniqId);
		}
		
		return false;
	}
	
	public static bool IsGuestLogined()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
		{
			return GameDefines.Setting_IsGuest;
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			return U3dNdSender.IsGuestLogined();
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			return NdSDKAgent.IsGuestLogined();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			return U3dGfanSender.IsGuestLogined();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone)
		{
			return U3dGfaniOSSender.IsGuestAccount;
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.MiAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.MiiPhone)
		{
			return XiaomiSDKAgent.IsGuestLogined();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone)
		{
			// the UC don't has guest account
			return false;
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.PPiPhone
			|| GameDefines.OutputVerDefs == OutputVersionDefs.PPAndroid){
			
			// the PP don't has guest account
			return false;
			
		}else if (GameDefines.OutputVerDefs == OutputVersionDefs.None)
		{
			return GameDefines.Setting_IsGuest;
		}
		else
		{
			return !string.IsNullOrEmpty(UserUniqId);
		}
		
		return false;
	}
	
	public static string GetUserUniId()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			return U3dNdSender.GetUserUniqId();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			return NdSDKAgent.GetUserUniqId();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			return U3dGfanSender.GetUserUniqId();
		}
		else
		{
			return UserUniqId;
		}
		
		return "";
	}
	
	public static string GetUserNickName()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			return U3dNdSender.GetUserNickName();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			return U3dGfanSender.GetUserNickName();
			
		}else
		{
			return NickName;
		}
		
		return "";
	}
	
	
	public static void SwitchAccount()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.SwitchAccount();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			U3dGfanSender.SwitchAccount();
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone){
			U3dGfaniOSSender.Logout(false);
		}
		else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void Guest2Official()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.Guest2Official();
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			NdSDKAgent.GuestRegister();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			// Only guest use
			U3dGfanSender.ModifyUserInfo();
			
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone){
			
			U3dGfaniOSSender.Guest2Official();
			
		}else {
				
			
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void ModifyUserInfo()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.EnterUserCenter();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			// Only guest use
			U3dGfanSender.ModifyUserInfo();
			
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone){
			
			UCSDKAgent.EnterUserCenter();
			
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone){
			
			U3dGfaniOSSender.EnterUserCenter();
			
		} else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void OnModifyUserInfo(bool isSuccess)
	{}
	
	public static void Logout(bool cancelAutoLogin)
	{
		Debug.Log("ThirdPartyPlatform.Logout called " + cancelAutoLogin);
		
		if(GameDefines.OutputVerDefs == OutputVersionDefs.PPiPhone){
			PPSDKAgent.EnterPPCenter();
		}else{
			Logout_impl(cancelAutoLogin);
		}
	}
	
	public static void Logout_impl(bool cancelAutoLogin)
	{
		Debug.Log("ThirdPartyPlatform.Logout_impl called " + cancelAutoLogin);
			
		GameDefines.Setting_IsAutoLogin = !cancelAutoLogin;
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows) 
		{
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.Logout(cancelAutoLogin);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
		{
			U3dGfanSender.Logout(cancelAutoLogin);
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android){
			NdSDKAgent.Logout(cancelAutoLogin);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone
			|| GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			U3dGfaniOSSender.Logout(cancelAutoLogin);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone)
		{
			UCSDKAgent.Logout();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.MiAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.MiiPhone)
		{
			XiaomiSDKAgent.Logout();
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.PPiPhone){
			// nothing because the account logout will be callback from PPSDK
			//
		}else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
			
		TalkingDataGA.Logout();
		Globals.Instance.QuitGame();
		
		// Reset some variables
		CacheUserUniqId = "";
		UserUniqId = "";
		SessionId = "";
		NickName = "";
	}
	
	public static void OnLogout(bool isSuccess)
	{
	}
	
	public static void OnRecharge(bool isSuccess)
	{
		
	}
	
	public static void EnterTradeRecord()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.EnterTradeRecord();
			
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android){
			
			NdSDKAgent.EnderUserSettings();
			
		}else{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void PayForCommodity(CommodityData data)
	{
		Statistics.INSTANCE.CustomEventCall(Statistics.CustomEventType.WantToPay,"ItemID",data.BasicData.LogicID,"Amount",data.BasicData.Count,"Price",data.currPrice);
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows) {
			GUIRadarScan.Hide();
		}else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			U3dIOSSendToSdk.BuyProductClick(data.CommodityStr,data.orderId);
			Debug.Log("CommodityStr is " + data.CommodityStr);
		}else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
		{
            GooglePayment.Instance.BuyProductID(data.CommodityStr);
			Debug.Log("orderId is " + data.orderId);
			Debug.Log("CommodityStr is " + data.CommodityStr);
		}else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void OnPayForCommodity(bool isSuccess, string orderId = "")
	{
		if (isSuccess)
		{
			Debug.Log("[GUIVipStore]: OnPayForCommodity orderId " + orderId);
			
			// // Temporary write orderId into file, Move to NetReceiver
			// GUIVipStore.AddPendingOrderId(orderId);
			
			if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
			{
				// NetSender.Instance.RequestGfanSearchOrderState(orderId);
			}
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
			{
				// Do nth. Game Server will auto send packet to Client, see
				// NetReceiver.Instance.ReceivePayNdSendStatusNotify
			}
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfanAndroid)
			{
				// Do nth. Game Server will auto send packet to Client, see NetReceiver.ReceivePaySendStatusNotify
				// U3dGfanSender.ShowToast("Receive from gfan sdk, orderId is " + orderId);
				// NetSender.Instance.RequestGfanSearchOrderState(orderId);
			}
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.GfaniPhone)
			{
				// Do nth. Game Server will auto send packet to Client
			}
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
			{
			}
		}
		else
		{
			
		}
	}
	
	public static void SearchPayResult(string orderId)
	{
		
	}
	 
	public static void OnSearchPayResult(bool isSuccess, bool isPaySuccess, string orderId)
	{
		
	}
	
	
	public static void EnterGameCenter()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			U3dNdSender.EnterNdPlatform();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			NdSDKAgent.EnterNdPlatform();
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.UCAndroid
			|| GameDefines.OutputVerDefs == OutputVersionDefs.UCiPhone)
		{
			UCSDKAgent.EnterUserCenter();
		}else if(GameDefines.OutputVerDefs == OutputVersionDefs.PPiPhone){
			
			PPSDKAgent.EnterPPCenter();
		}
		else {
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(20000019,true);
		}
	}
	
	public static void OnLeavePlatform(bool isSuccess)
	{
	}
	
	// Only Nd91iPhone use the sdk updateApp method
	public enum NdAppUpdateResult
	{
		ND_APP_UPDATE_NO_NEW_VERSION = 0,				/**< 没有新版本 */
		ND_APP_UPDATE_FORCE_UPDATE_CANCEL_BY_USER = 2,	/**< 用户取消下载强制更新 */
		ND_APP_UPDATE_NORMAL_UPDATE_CANCEL_BY_USER = 3,	/**< 用户取消下载普通更新 */
		ND_APP_UPDATE_NEW_VERSION_DOWNLOAD_FAIL = 4,	/**< 下载新版本失败 */
		ND_APP_UPDATE_CHECK_NEW_VERSION_FAIL = 7,		/**< 检测新版本失败 */
	}
	
	public static void NdCheckAppVersion()
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			isNdCheckintVer = true;
			U3dNdSender.CheckAppVersion();
		}
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91Android)
		{
			isNdCheckintVer = true;
			NdSDKAgent.AppVersionUpdate();
		}
		else
		{
			// Gfan and Windows's version check run in our login server
		}
	}
	
	public static void OnNdCheckAppVersion(string updateResult)
	{
		if (GameDefines.OutputVerDefs == OutputVersionDefs.Nd91iPhone)
		{
			isNdCheckintVer = false;
			
			NdAppUpdateResult result = (NdAppUpdateResult)int.Parse(updateResult);
			switch (result)
			{
			case NdAppUpdateResult.ND_APP_UPDATE_NO_NEW_VERSION:
			case NdAppUpdateResult.ND_APP_UPDATE_NEW_VERSION_DOWNLOAD_FAIL:
			case NdAppUpdateResult.ND_APP_UPDATE_CHECK_NEW_VERSION_FAIL:
				Login(true);
				break;
			case NdAppUpdateResult.ND_APP_UPDATE_FORCE_UPDATE_CANCEL_BY_USER:
				Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
				{
					gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
					gui.SetDialogType(EDialogType.ND_FORCE_UPDATE);
				}, EDialogStyle.DialogOk, delegate() 
				{
					Globals.Instance.QuitGame();
				});
				break;
			case NdAppUpdateResult.ND_APP_UPDATE_NORMAL_UPDATE_CANCEL_BY_USER:
				Login(true);
				// Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
				// {
				// 	gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
				// 	gui.SetDialogType(EDialogType.ND_NORMAL_UPDATE);
				// }, EDialogStyle.DialogOkCancel, delegate() 
				// {
				// 	Login(true);
				// });
				break;
			}
			
			
		}
	}
	
	
	/// <summary>
	/// 没有账号系统的平台登录，登录自己的账号系统
	/// </summary>
	public static void nonePlatformBeginConnect()
	{
		Globals.Instance.MGUIManager.CreateWindow<GUIRadarScanCY>(delegate(GUIRadarScanCY guiRaderCY){
			guiRaderCY.SetVisible(true);
			Globals.Instance.MLSNetManager.Disconnect();
			Globals.Instance.MLSNetManager.BeginConnect(GameDefines.LOGIN_SERVER_IP, GameDefines.LOGIN_SERVER_PORT, delegate(NetManager.ConnectionState state) 
			{
				if(state == NetManager.ConnectionState.Connected)
				{
					// Check the version
					Debug.Log("RequestCheckChannelAndVersion::Send");
					Globals.Instance.MLSNetManager.RequestCheckChannelAndVersion(GameDefines.GameChannel, GameDefines.GameVersion);
				}
				else
				{
					if (Application.internetReachability == NetworkReachability.NotReachable
						|| Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) // 
					{
						Debug.Log("[ThirdPartyPlatform]: Cann't Connect the login server In AtlanticLogin");
							
						// The login server is not available or in maintenance
						Globals.Instance.PopupNetworkProblemDlg();
					}
					else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
					{
						// User network is ok, but the game is in maintaining
						// Application.OpenURL(GameDefines.OfficialSkipToUrl);
					}
				}
			});
		});
	}
}
