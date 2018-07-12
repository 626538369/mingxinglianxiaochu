// #define OPEN_PPSDK

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PPSDKAgent : MonoBehaviour 
{
	
	#region Sender
	static readonly int AppId = 679;
	static readonly string AppKey = "a97c9bf38ee8145e578d211d39b3b0f1";
	
	static readonly int Landscape = 0;
	static readonly int Portrait = 1;
	
	public static readonly float RmbYuan2NdBeanExchangeRatio = 1.0f;

#if OPEN_PPSDK
	[DllImport("__Internal")]
	private static extern void pp_leavePPPlatform();
    
	[DllImport("__Internal")]
	private static extern void pp_initSDK(int appId, string appKey);
        
    [DllImport("__Internal")]
	private static extern void pp_login();
    [DllImport("__Internal")]
	private static extern void pp_logout();
    
    [DllImport("__Internal")]
	private static extern bool pp_isLogined();
    [DllImport("__Internal")]
	private static extern long pp_getUserId();
    [DllImport("__Internal")]
	private static extern long pp_getSessionId();
    [DllImport("__Internal")]
	private static extern string pp_getUserName();
	
	[DllImport("__Internal")]
	private static extern float pp_SYPPMoneyRequest();
    
    [DllImport("__Internal")]
	private static extern void pp_uniPayAsyn(string orderId, string productId, string productName,
                           float price, int productCnt, string payDescription);
	
    [DllImport("__Internal")]
	private static extern void pp_enterPPCenter();
#endif
	
	public static void InitSDK()
	{
#if OPEN_PPSDK
		pp_initSDK(AppId,AppKey);
#endif
	}
	
	public static void Login()
	{
#if OPEN_PPSDK
		pp_login();
#endif
	}
	#endregion
	
	/// <summary>
	/// Eters the PP center to logout
	/// </summary>
	public static void EnterPPCenter(){
#if OPEN_PPSDK
		pp_enterPPCenter();
#endif
	}
	
	public static float GetBalance(){
#if OPEN_PPSDK
		return pp_SYPPMoneyRequest();
#endif
		return 0;		
	}
	
	public static void PayForCommodity(CommodityData data){
		
#if OPEN_PPSDK
		
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
		
		float productPrice = exchangeRatio * data.currPrice;
		float productOriginalPrice = exchangeRatio * data.originalPrice;
		
		int productCnt = data.BasicData.Count;
		string payDescription = data.BasicData.Description;
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		Debug.Log("[GUIVipStore]: PayForCommodity PPPlatform " + productPrice.ToString() + " and count " + productCnt);
		
		float blanceMoney = pp_SYPPMoneyRequest();
		if(blanceMoney >= productPrice){
			
			string prompt = string.Format(Globals.Instance.MDataTableManager.GetWordText(24700036),blanceMoney.ToString("F2"),productPrice.ToString("F2"));
			
			GUIDialog.PopupYesNoDlg(prompt,delegate() {
				pp_uniPayAsyn(orderId, productId, productName, productPrice, productCnt, payDescription);
			},null);
			
		}else{
			pp_uniPayAsyn(orderId, productId, productName, productPrice, productCnt, payDescription);
		}	
		
#endif
		
	}
	
	#region Receiver
	
	public void OnInitSDK(string args){
		ThirdPartyPlatform.OnInitSDK(true);
	}
	
	public void OnLogin(string args){
		ThirdPartyPlatform.UserUniqId = args;
		ThirdPartyPlatform.OnLogin(true);
	}
	
	public void OnLogout(string args){
		ThirdPartyPlatform.Logout_impl(true);
	}
	
	#endregion
}
