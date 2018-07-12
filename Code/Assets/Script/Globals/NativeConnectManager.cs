using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class NativeConnectManager : MonoBehaviour 
{
	void Start()
	{
		Debug.Log("NativeConnectManager");
	}

	
	//[DllImport ("__Internal")]
	//private static extern void FlurryCustomEvent_internal(string type,string param);
	//@}
		
	/// <summary>
	/// Puts the flurry custom event.
	/// </summary>
	/// <param name='_type'>
	/// _type.
	/// </param>
	/// <param name='args'>
	/// Arguments.	names and values parameters
	/// </param>
	/// <exception cref='System.Exception'>
	/// Is thrown when the exception.
	/// </exception>
	public void PutFlurryEvent(string _type,params object[] args){
		return;
		try{
				
			if (args != null && args.Length % 2 != 0) {
				throw new System.Exception("Tween Error: Hash requires an even number of arguments!");
			}
			
			Dictionary<string,object> tListParams = new Dictionary<string,object>();
			
			int currentLevel = 1;
			if(Globals.Instance.MGameDataManager != null && Globals.Instance.MGameDataManager.MActorData != null){
				tListParams.Add("RoleID",Globals.Instance.MGameDataManager.MActorData.PlayerID.ToString());
				tListParams.Add("RoleLevel",Globals.Instance.MGameDataManager.MActorData.GetHighestLevelGeneral().ToString());
				
				currentLevel = Globals.Instance.MGameDataManager.MActorData.GetHighestLevelGeneral();
			}
			
			if(args != null){
				int i = 0;
				while(i < args.Length - 1) {
					tListParams.Add(args[i].ToString(), args[i+1].ToString());
					i += 2;
				}
			}
			
			if(tListParams.Count > 0){
				TalkingDataGA.CustomEvent(_type,_type,tListParams,currentLevel);	
			}
			
			// add the channel to flurry
			tListParams.Add("Channel",GameDefines.GameChannel);
			
			// compose the string for native flurry event
			StringBuilder sb = new StringBuilder();
			foreach(string key in tListParams.Keys){
				
				if(sb.Length != 0){
					sb.Append("&");
				}
				
				object obj = tListParams[key];
				sb.Append(key + "=" + obj.ToString());
			}
			
			if(!Application.isEditor){
#if UNITY_ANDROID
			
				using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
		        {  
		            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
		            {  
		                jo.Call("flurryCustomEvent",_type,sb.ToString());
		            }  
		        }
				
#elif UNITY_IPHONE
				
				//FlurryCustomEvent_internal(_type,sb.ToString());
				
#endif			
			}

		}catch(System.Exception e){
			Debug.LogError(e.Message + "\n" + e.StackTrace);			
		}
					
	}
	
	
	// cache the network information to avoid call native function per frame
	private string NetworkInfo = null;
	
	/**
	 * tzz added for get current network state
	 * 
	 */ 
	public string GetCurrNetworkInfo(){
		
		if(Application.isEditor){
			return "DesktopNetwork";
		}
		return "DesktopNetwork";
	}
	
	public static void OpenVewView(string url){
		
		if(Application.isEditor){
			return ;
		}
		
#if UNITY_ANDROID
		using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
        {  
            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
            {  
                jo.Call("showWebView",url);
            }
        }
#elif UNITY_IPHONE
		
#endif
		
	}
	
	
	// cache the machine type to avoid call native function per frame
	private static string MachineType = null;

	
	public string GetAvailMemSize()
	{
		if(!Application.isEditor){
			
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
	        {  
	            using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
	            {  
	               return jo.Call<string>("getAvailMemSize");
	            }  
	        }  	
#endif
		}else{
			return SystemInfo.systemMemorySize.ToString() + "MB";
		}

		return "";
	}
	
	public string GetTotalMemSize()
	{
		if(!Application.isEditor){
#if UNITY_ANDROID
			using(AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))  
		    {  
		        using( AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))  
		        {  
		           return jo.Call<string>("getTotalMemSize");
		        }  
		    }  	
#endif
		}else{
			return SystemInfo.systemMemorySize.ToString() + "MB";
		}
		
		return "";
	}
}

