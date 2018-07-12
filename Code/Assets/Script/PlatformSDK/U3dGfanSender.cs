using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class U3dGfanSender 
{
	public static readonly float RmbYuan2GfanQExchangeRatio = 10.0f;
	
	// GfanSdk Appkey(620954647), CryptographicKey(1e92e078ec13d966)
	
	public static string GetAppVersion()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				return jo.Call<string>("getAppVersion");
            }  
        }  
#endif
		return null;
	}
	
	public static string GetAppGfanKey()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				return jo.Call<string>("getAppGfanKey");
            }  
        }  
#endif
		return null;
	}
	
	public static string GetAppChannelId()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				return jo.Call<string>("getAppChannelId");
            }  
        }  
#endif
		return null;
	}
	
	public static void ShowToast(string msg)
	{
		if (!GameDefines.ToastEnabled)
			return;
		
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("showToast", msg);
            }  
        } 
#endif
	}

	public static void InitGfanSDK()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("initGfanSDK");
            }  
        } 
#endif
	}
	
	public static void CloseGfanSDK()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("closeGfanSDK");
            }  
        } 
#endif
	}
	
	// The Result form UnitySendMessage("Object", "Method", "Params"), params is combined as the format "key1=value1&key2=value2"
	public static void Login(bool supportGuest = false)
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startGfanLogin");
            }  
        }  
#endif
	}
	
	public static void Register()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startGfanRegister");
            }  
        }  	
#endif
	}
	
	public static bool IsLogined()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                // jo.Call("startGfanRegister");
				return jo.Call<bool>("isGfanLoined");
            }  
        }  
#endif
		return false;
	}
	
	public static bool IsGuestLogined()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                // jo.Call("startGfanRegister");
				return jo.Call<bool>("isGfanGuestLoined");
            }  
        }  
#endif
		return false;
	}
	
	public static string GetUserUniqId()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				return jo.Call<long>("getGfanUserUniqId").ToString();
            }  
        } 
#endif
		
		return "";
	}
	
	public static string GetUserNickName()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				return jo.Call<string>("getGfanUserName");
            }  
        }  	
#endif
		return string.Empty;
	}
	
	public static void ModifyUserInfo()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startModfiyUserInfo");
            }  
        }  	
#endif
	}
	
	public static void SwitchAccount()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startGfanLogout");
            }  
        }  	
#endif
	}
	
	public static void Logout(bool cancelAutoLogin)
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
				if (cancelAutoLogin)
				{
					jo.Call("startGfanLogout");
				}
				else
				{
					// Do nth.
				}
            }  
        }  	
#endif
	}
	
	public static void Recharge()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startGfanRecharge");
            }  
        }  
#endif
	}
	
	public static void PayForCommodity(CommodityData data)
	{
#if UNITY_ANDROID
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
		string userName = GetUserUniqId();
		
		productPrice *= productCnt;
		productOriginalPrice *= productCnt;
		
		// Gfan pay's mininum is 1 * multiple
		int gfanPayVal = (int)Mathf.CeilToInt(productPrice);
		
		Debug.Log("[GUIVipStore]: PayForCommodity GfanPlatform " + gfanPayVal.ToString() + " and count " + productCnt);
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                // jo.Call("startGfanPay", orderId, productName, payDescription, productPrice, userName);
				jo.Call("startGfanPay", orderId, productName, payDescription, gfanPayVal);
            }  
        }  
#endif
	}
	
	public static void GetLeakedOrders()
	{
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("startGfanGetLeakedOrders");
            }  
        }  
#endif
	}
	
	public static void SearchPayResult(string orderId)
	{
#if UNITY_ANDROID
		Debug.Log("[U3dGfanSender]: Not implement yield.");
		
		/*
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
            }  
        } 
        */
#endif
	}
}