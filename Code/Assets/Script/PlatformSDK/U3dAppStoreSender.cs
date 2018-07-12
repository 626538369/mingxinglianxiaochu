#define OPEN_APPSTORE // Use a define switch

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class U3dAppStoreSender {
	
#if OPEN_APPSTORE
		// SDK设置相关
	//[DllImport("__Internal")]
	//private static extern void U3dAppStoreBuyItem(string CommodityStr);
	
	//[DllImport("__Internal")]
	//private static extern void U3dAppStoreHDBuyItem(int itemID);
	
	//[DllImport("__Internal")]
	//private static extern void U3dAppStoreStartTracePage(string pageName);
	
	//[DllImport("__Internal")]
	//private static extern void U3dAppStoreEndTracePage(string pageName);
	
	//[DllImport("__Internal")]
	//private static extern void U3dUMengRegister();
	
	//[DllImport("__Internal")]
	//private static extern void U3dSavePhoth(string savePath);
	
	//[DllImport("__Internal")]
	//private static extern void U3dSharePhoto(string savePath);

	
	//[DllImport("__Internal")]
	//private static extern void U3dShareGameUrl(string gameurl);
	
	//[DllImport("__Internal")]
	//private static extern void U3dOpenPhoto();
	
	//[DllImport("__Internal")]
	//private static extern void U3dKTPlay();
	
	//[DllImport("__Internal")]
	//private static extern void U3dLoginKTPlay(string str,string str2);

	//[DllImport("__Internal")]
	//private static extern void U3dLoginCYwithuserid(string str);

	//[DllImport("__Internal")]
	//private static extern void U3dTapJoyEvent(string str);
	
#endif

	public static void AppStoreTapJoyEvent(string str)
	{
		#if OPEN_APPSTORE

		//U3dTapJoyEvent(str);
	
		#endif
	}

	public static void AppStoreBuyItem(string CommodityStr)
	{
		#if OPEN_APPSTORE
	
			//U3dAppStoreBuyItem(CommodityStr);
			
		 	Debug.Log("AppStoreBuyItem is " + CommodityStr );
		#endif
	}
	
	public static void AppStoreRegisterUMeng()
	{
		#if OPEN_APPSTORE

		//U3dUMengRegister();

		Debug.Log("AppStore Register UMeng " );
		#endif
	}
	
	public static void UMengStracePage(string pageName)
	{
		#if OPEN_APPSTORE
			//U3dAppStoreStartTracePage(pageName);
		#endif
	}
	
	public static void UMengStopStracePage(string pageName)
	{
		#if OPEN_APPSTORE
			//U3dAppStoreEndTracePage(pageName);
		#endif
	}
	
	public static void AppSavePhoth(string strAddr)
	{
		#if OPEN_APPSTORE
			//GUIRadarScan.Show();
			//U3dSavePhoth(strAddr);
		#endif
	}
	
	public static void ShareMyPhoto(string photoName)
	{
		#if OPEN_APPSTORE
			//U3dSharePhoto(photoName);
		#endif
	}

	public static void ShareGameUrl(string photoName)
	{
		#if OPEN_APPSTORE
			//U3dShareGameUrl(photoName);
		#endif
	}
	
	public static void OpenIphonePhoto()
	{
		#if OPEN_APPSTORE
			//U3dOpenPhoto();
		#endif
		
	}
	public static void OpenKTPlay()
	{
		#if UNITY_IPHONE
			//U3dKTPlay();
		#endif
		
	}
	public static void LoginKTPlay(string str,string str2)
	{
		if(!Application.isEditor)
		{
			#if UNITY_IPHONE
			//U3dLoginKTPlay(str,str2);
			#endif		
		}
	}

	public static void LoginCYwithuserid(string str)
	{
		if(!Application.isEditor)
		{
			#if UNITY_IPHONE
			//U3dLoginCYwithuserid(str);
			#endif		
		}
	}

	// 释放平台内存(destory)
	public static void CloseSDK()
	{
		#if UNITY_IPHONE
		////U3dCYLeavePlatform();
		#endif
	}

}
