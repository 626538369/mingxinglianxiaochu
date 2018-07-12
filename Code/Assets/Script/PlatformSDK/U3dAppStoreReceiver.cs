using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
//using cn.sharesdk.unity3d;

public class U3dAppStoreReceiver : MonoBehaviour {
	
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
	
	public static readonly string KeyImageData =  "imageData";
	
	public static readonly string KeyPushToken = "pushToken";
	
	public static readonly string KeyMACAdress = "macAdress";
	
	public static readonly string KeyIDAF = "idaf";
	
		//-------------------------------------------------------------------------------
	const string majorDelimiter = "&&&&&";
	const string assignDelimiter = "#####";
	//-------------------------------------------------------------------------------
	
	//-------------------------------------------------------------------------------
	Dictionary<string, string> receiveParams = new Dictionary<string, string>();
	bool ParseReceiveParams(string argvs)
	{
		receiveParams.Clear();
		
		Debug.Log("[ParseReceiveParams]: the argvs is " + argvs);
		
		
		string[] sections = argvs.Split(majorDelimiter.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
		foreach (string section in sections)
		{
			Debug.Log("section is : " + section);
			string[] keyValues = section.Split(assignDelimiter.ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
			if (2 != keyValues.Length)
			{
				Debug.Log("[U3d2NdReceiver]: The parameters has some invalid data,early break.");
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
	
	public void paymentNotify(string args)
	{
		ParseReceiveParams(args);
		
		string result = GetReceiveParam(KeyResult);
		string error = GetReceiveParam(KeyError);
		
		Debug.Log("[U3dAppReceiver]: paymentNotify------------------- the result is " + result);
		Debug.Log("[U3dAppReceiver]: paymentNotify------------------- the error is " + error);
		
		if ("Success" == error)
		{
			string productID  = GetReceiveParam(KeyProductId);
			string changeIdentity = GetReceiveParam(KeyOrderId);
			string changeReceipt = GetReceiveParam(KeyPayDescription);
			
			Debug.Log("[U3dAppReceiver]: productID is  " + productID);
			Debug.Log("[U3dAppReceiver]: changeIdentity is  " + changeIdentity);
			Debug.Log("[U3dAppReceiver]: changeReceipt is  " + changeReceipt);
			
			ShopDataManager.AddPendingOrderId(ShopDataManager.PayCommodityData.orderId,GameDefines.PlatformApp,changeIdentity,changeReceipt);
			
			NetSender.Instance.RequestAppStoreChargeConfirm(ShopDataManager.PayCommodityData.orderId, changeReceipt,changeIdentity);
		}
		else{
			GUIRadarScan.Hide();
			//GUIVipStore.inRechargeing = false;
		}
	}

	public void SendProductsInfoList(string args)
	{
		ParseReceiveParams(args);
		
		foreach(String key in receiveParams.Keys)
		{
			Debug.Log("key is :" + key);
			string valueStr = "";
			receiveParams.TryGetValue(key,out valueStr);
			Debug.Log("value is :" + valueStr);

			ShopDataManager.CommodityToCurrencyDicts[key] = valueStr;
		}
	
	}


	public void pushNotify(string args)
	{
		ParseReceiveParams(args);
		
		string result = GetReceiveParam(KeyResult);
		string error = GetReceiveParam(KeyError);
		Debug.Log("[U3dAppReceiver]: paymentNotify------------------- the result is " + result);
		
		if ("Success" == error)
		{
			string tokenStr   = GetReceiveParam(KeyPushToken);
			Debug.Log("[U3dAppReceiver]: pushtoken is  " + tokenStr);
			//GUIVipStore.devicePushToken = tokenStr;
		}
	}
	
	
	public void checkisJailBroken(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		//if (result == "true")
		{
			//GUIVipStore.isJailBroken = true;
			Debug.Log("checkisJailBroken");
		}
	}
	
	public void checkisIPAFree(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		//if (result == "true")
		{
			//GUIVipStore.isIPAFree = true;
			Debug.Log("checkisIPAFree");
		}
	}
	
	public void SavePhoto(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("SavePhoto result is " + result);
		GUIRadarScan.Hide();
		if (result == "true" || result == "1")
		{
			Debug.Log("ShowSimpleCenterTips 4011" );
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4011);
		}
	}
//	TaskManager.HeadURL mHeadURL = new TaskManager.HeadURL();
	public void SaveURL(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("SavePhoto result is " + result);
		GUIRadarScan.Hide();
		if (result == "true" || result == "1")
		{
			Debug.Log("ShowSimpleCenterTips 4011" );
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4011);
		}
		StartCoroutine(URLTexture(result));
	}
	TaskManager.HeadURL mHeadURL = new TaskManager.HeadURL();
	IEnumerator URLTexture(string url)
	{
		Texture mTex;
		WWW www = new WWW(url);
		yield return www;
		mTex = www.texture;
//		if (mTex != null)
//		{
//			mHeadURL.URLTexture = mTex;
//			Globals.Instance.MTaskManager.urlTexture = mHeadURL;
//		}
//		GUIMain mmain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
//		mmain.HeadURL();
		if(Globals.Instance.MTaskManager.urlTexture == null)
		{
			Debug.Log("url is null");
			if (mTex != null)
			{
				mHeadURL.URLTexture = mTex;
				mHeadURL.URL = url;
				Globals.Instance.MTaskManager.urlTexture = mHeadURL;
			}
			GUIMain mmain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
			mmain.HeadURL();
		}
		else
		{
			if(Globals.Instance.MTaskManager.urlTexture.URL == url)
			{
				Debug.Log("The same pictrue");
			}
			else
			{
				Debug.Log("The different pictrue");
				if (mTex != null)
				{
					mHeadURL.URLTexture = mTex;
					mHeadURL.URL = url;
					Globals.Instance.MTaskManager.urlTexture = mHeadURL;
				}
				GUIMain mmain = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
				mmain.HeadURL();
			}
		}
//		www.Dispose();
	}
	public void SharePhoto(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("SavePhoto result is " + result);
		GUIRadarScan.Hide();
		if (result == "true" || result == "1")
		{
			Debug.Log("ShowSimpleCenterTips 4012" );
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4012);
			if(Globals.Instance.MTaskManager.IsGetShareReward && Globals.Instance.MGameDataManager.MActorData.starData.appStoreTapJoyState)
			{
				GUIRadarScan.Show();
				NetSender.Instance.ShareCountInfoReq(1);
			}
		}
		else
		{
			Debug.Log("ShowSimpleCenterTips 4013" );
			string error = GetReceiveParam(KeyError);
			//Globals.Instance.MGUIManager.ShowSimpleCenterTips(error);
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4013);
		}
	}

	public void ShareGameUrl(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("SavePhoto result is " + result);
		GUIRadarScan.Hide();
		if (result == "true" || result == "1")
		{
			Debug.Log("ShowSimpleCenterTips 4012" );
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4012);
//			GUIPhotoGraph guiPhotoGraph = Globals.Instance.MGUIManager.GetGUIWindow<GUIPhotoGraph>();
//			if(guiPhotoGraph != null)
//			{
//				guiPhotoGraph.SetTaskShare();
//			}
		}
		else
		{
			Debug.Log("ShowSimpleCenterTips 4013" );
			string error = GetReceiveParam(KeyError);
			//Globals.Instance.MGUIManager.ShowSimpleCenterTips(error);
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4013);
		}
	}

	
	public void macAdressIDAF(string args)
	{
		ParseReceiveParams(args);
		string macAdress  = GetReceiveParam(KeyMACAdress);
		string idafstr  = GetReceiveParam(KeyIDAF);
		GameDefines.systemMacAdress = macAdress ;
		if ("empty" != idafstr)
		{
			GameDefines.systemIFAD = idafstr;
			GameDefines.systemUDID = idafstr;
			VerificationIdaf();
		}
		
		Debug.Log("macAdress is " + macAdress );
		Debug.Log("idafstr is " + idafstr );

	}

	public void adVideoPlayFinished(string args)
	{
		ParseReceiveParams(args);
		string result = GetReceiveParam(KeyResult);
		Debug.Log("adVideoPlayFinished result is " + result);

		if (result == "true" || result == "1")
		{
			GUIRadarScan.Show();
			NetSender.Instance.AdvertisingAwardsReq();
		}
		else
		{
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(4023);
		}
		SoundManager.CurrentPlayingMusicAudio.Play();
	}
	
	public void selectImageDataAsAvatar(string args)
	{
		ParseReceiveParams(args);
		
		//string result = GetReceiveParam(KeyResult);
		string error = GetReceiveParam(KeyError);
		Debug.Log("[U3dAppReceiver]: selectImageDataAsAvatar------------------- the error is " + error);
		if ("Success" == error)
		{
			string iamgeData = GetReceiveParam(KeyImageData);
			
			Debug.Log("[U3dAppReceiver]: imageData is  " + iamgeData);
			GUICreateRole guicreateRole = Globals.Instance.MGUIManager.GetGUIWindow<GUICreateRole>();
			if (guicreateRole)
			{
				guicreateRole.updateAvatar(iamgeData);
			}
			

		}
		else{
			GUIRadarScan.Hide();
			//GUIVipStore.inRechargeing = false;
		}
	}
	
	// Use this for initialization
	void Start () {
		
		//ShareSDK.setCallbackObjectName("U3dAppStoreReceiver");
		//ShareSDK.open ("api20");
		//Hashtable sinaWeiboConf = new Hashtable();
		//sinaWeiboConf.Add("app_key", "1152995477");
		//sinaWeiboConf.Add("app_secret", "e37d1b97d3bca278613beea3c0a095a8");
		//sinaWeiboConf.Add("redirect_uri", "http://www.sharesdk.cn");
		//ShareSDK.setPlatformConfig (PlatformType.SinaWeibo, sinaWeiboConf);
		
				//WeChat
		//Hashtable wcConf = new Hashtable();
		//wcConf.Add ("app_id", "wx8edf7598338c31cf");
		//ShareSDK.setPlatformConfig (PlatformType.WeChatSession, wcConf);
		//ShareSDK.setPlatformConfig (PlatformType.WeChatTimeline, wcConf);
		//ShareSDK.setPlatformConfig (PlatformType.WeChatFav, wcConf);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	bool isReVerification = false; //  是否重新验证//
	private void VerificationIdaf() // 验证Idaf //
	{
		StartCoroutine(RequestIdafResult());
	}
	
	private IEnumerator RequestIdafResult()
	{
		string http = "http://106.75.198.104:18000/advice?idfa="+GameDefines.systemIFAD+"&idfv=null&stage=0";
		WWW ret = new WWW(http);
		yield return ret;
		if (ret.error != null)
		{
			Debug.LogError("error:" + ret.error);
			yield break;
		}
		string result = ret.text;
		if (string.IsNullOrEmpty(result))
		{
			yield break;
		}
		JsonData jd = JsonMapper.ToObject(result);
		bool isSuccess = StrParser.ParseBool(jd[0].ToString() , false);
		if(!isSuccess&&!isReVerification)
		{
			isReVerification = true;
			VerificationIdaf();
		}
	}

}
