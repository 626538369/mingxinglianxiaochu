#define OUTER_INTERNET

using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System.Security.Cryptography;


// The output platform defines
public enum OutputVersionDefs
{
	Windows,
	
	Nd91Android,
	Nd91iPhone, // 91 iPhone sdk
	
	GfanAndroid, // gfan Android sdk
	GfaniPhone, // gfan iPhone sdk
	
	UCAndroid,	
	UCiPhone,
	
	PPAndroid,
	PPiPhone,
	
	MiAndroid,
	MiiPhone,
	
	AppStore,
	
	WPay,
	
	None = -1,
}

public class GameDefines
{
	public static string GameVersion = "1.0.6";
	
	// channel information
	private static string mSDKPlatform = null;
	
	private static void ReadSDKPlatform(){
		if(mSDKPlatform ==  null){
			// Read the current sdk platform;
			TextAsset tx = Resources.Load("SDKPlatform") as TextAsset;
			mSDKPlatform = tx.text;				
			Debug.Log("Current sdkPlatform is " + mSDKPlatform);
		}	
	}
	
	public static string GameSDKPlatform{
		get
		{
			ReadSDKPlatform();
			return mSDKPlatform;
		}
	}
	
	public static string GameChannel{
		get{
			
			ReadSDKPlatform();
			
			if(mSDKPlatform.StartsWith(PlatformGfan)){
#if UNITY_IPHONE
				return "Gios6";
#endif
			}
			
			int idx = mSDKPlatform.IndexOf('_');
			if(idx != -1){
				return mSDKPlatform.Substring(idx + 1);
			}
			
			return mSDKPlatform;
		}
	}
	
	public const string PlatformGfan = "gfan";
	public const string PlatformNd91 = "91";
	public const string PlatformUC = "uc";
	public const string PlatformPP = "pp";
	public const string PlatformMi = "mi";
	public const string PlatformApp = "appstore";
	public const string PlatformNone = "none";
	public const string PlatformWPay = "wpay";
	
	private string sdkPlatform;
	
	private static OutputVersionDefs outputPlatform = OutputVersionDefs.None;
	public static OutputVersionDefs OutputVerDefs{
		get{
			if(Application.isEditor)
			{
				return OutputVersionDefs.Windows;
			}
			else
			{	
				if (outputPlatform != OutputVersionDefs.None)
					return outputPlatform;
				
				outputPlatform = ParsePlatform();
			}
			
			return outputPlatform;
		}
	}
	
	private static string outputhannelsIdentity = "B10001";
	public static string OutPutChannelsIdentity
	{
		get{
			if (OutputVerDefs == OutputVersionDefs.WPay)
			{
				outputhannelsIdentity = "B10003";
			}
			else if (OutputVerDefs == OutputVersionDefs.AppStore)
			{
					outputhannelsIdentity = "B10001";
			}
			return outputhannelsIdentity;
		}
	}
	
	public static OutputVersionDefs ParsePlatform(){
		
		ReadSDKPlatform();
		
#if UNITY_ANDROID
		if (mSDKPlatform.StartsWith(PlatformGfan))
			return OutputVersionDefs.GfanAndroid;
		else if (mSDKPlatform.StartsWith(PlatformNd91))
			return OutputVersionDefs.Nd91Android;
		else if (mSDKPlatform.StartsWith(PlatformUC))
			return OutputVersionDefs.UCAndroid;
		else if (mSDKPlatform.StartsWith(PlatformPP))
			return  OutputVersionDefs.PPAndroid;
		else if (mSDKPlatform.StartsWith(PlatformMi))
			return  OutputVersionDefs.MiAndroid;
		else if (mSDKPlatform.StartsWith(PlatformWPay))
			return OutputVersionDefs.WPay;
#elif UNITY_IPHONE
		if (mSDKPlatform.StartsWith(PlatformGfan))
			return OutputVersionDefs.GfaniPhone;
		else if (mSDKPlatform.StartsWith(PlatformNd91))
			return OutputVersionDefs.Nd91iPhone;
		else if (mSDKPlatform.StartsWith(PlatformUC))
			return OutputVersionDefs.UCiPhone;
		else if (mSDKPlatform.StartsWith(PlatformPP))
			return OutputVersionDefs.PPiPhone;
		else if (mSDKPlatform.StartsWith(PlatformMi))
			return  OutputVersionDefs.MiiPhone;
		else if (mSDKPlatform.StartsWith(PlatformApp))
			return OutputVersionDefs.AppStore;
		else if (mSDKPlatform.StartsWith(PlatformWPay))
			return OutputVersionDefs.WPay;
#endif
		
		return OutputVersionDefs.Windows;
	}
	
	public static string GameHomepageUrl = "http://115.28.59.189/GamePort/";
	public static string DownloadSkipToUrl = "http://115.28.59.189/GamePort/";
	public static string OfficialSkipToUrl = "http://115.28.59.189/GamePort/";
	
	public static readonly bool ToastEnabled = false;
	
	
	// Manifest.
	public static string Manifest{ get { return "Artist/manifest.xml"; } }
	
	// Manifest package.
	public static string ManifestPackage{ get { return "00004"; } }
	
	// Asset Package File.
	public static string AssetPackageFile{ get { return "Artist/AssetPackage.xml"; } }
	
	// Asset Package Package.
	public static string AssetPackagePackage{ get { return "00001"; } }
	
	// Asset Weight File.
	public static string AssetWeightFile{ get { return "Artist/AssetWeight.xml"; } }
	
	// Asset Weight Package.
	public static string AssetWeightPackage{ get { return "00003"; } }
	
	/*
	public static string AssetPri{ get { return "Artist/assetpri.xml"; } }
	public static string AssetPriPackage{ get { return "00002"; } }
	*/
	
	// Resource Path.
	public static string ConfigPath { get { return "Config"; } }
	
	// Loading steps.
	public const int LOADING_MAX_STEP = 100;
	public const int LOADING_DELTA_STEP = 1;
	public static readonly int []LOADING_BATTLE_STEP = { 1, 5, 20, 30, 31, 32, 33, 97, 98, 99, LOADING_MAX_STEP };
	
	// Animation.
	public const float ANIM_CROSS_TIME = 0.1f;
	
	// Battle step time.
	public const float BATTLE_STEP_TIME = 2.5f;
	// quick btn visible step ratio
	public const float QUICK_BTN_ENABLED_RATIO = 0.6F;
	
	// Battle Grid size
	public const float BATTLE_GRID_WIDTH = 120.0f;
	public const float BATTLE_GRID_HEIGHT = 60.0f;
	public const int BATTLE_GRID_HORIZONTAL_MAX_NUM = 15;
	public const int BATTLE_GRID_VERTICALMAX_NUM = 8;

	public const float BATTLE_STEP_PARTICLE_TIME = 4f;
	
	//Socket connect timeout
	public const int SOCKET_CONNECT_TIME_OUT = 10;
		
#if OUTER_INTERNET
	public const string LOGIN_SERVER_IP = "mingxing.forace.com.cn";
	public const int LOGIN_SERVER_PORT = 10006;		

#else
	//login server
	//han_lei//
	//public const string LOGIN_SERVER_IP = "192.168.1.104";
//	public const string LOGIN_SERVER_IP = "192.168.1.112";
//	public const string LOGIN_SERVER_IP = "192.168.1.87";
//	public const string LOGIN_SERVER_IP = "118.186.136.98";
//	public const string LOGIN_SERVER_IP = "114.215.123.170";	
	//public const string LOGIN_SERVER_IP = "172.16.12.135";
	public const string LOGIN_SERVER_IP = "111.198.66.109";
	public const int LOGIN_SERVER_PORT = 10006;
	
	//game server
	public const int GAME_SERVER_PORT = 10017;
	
	// TianFengShuo Desktop server
    public const string GAME_SERVER_IP = "127.0.0.1";
	
    //public const string GAME_SERVER_IP = "192.168.100.56";
	//public const string GAME_SERVER_IP = "10.16.17.114"; 
#endif
	   
	//chat server
	public const string CHAT_SERVER_IP = "10.16.8.11";
	public const int CHAT_SERVER_PORT = 10010;
	
	//public static string SERVER_URL_BASE = "http://192.168.100.211:8080/seaGameDownload/ios/assetbundles/";
	public static string SERVER_URL_BASE = "http://115.28.59.189/AssetBundles/";
	
	public static string LOCAL_FILE_FLAG = "resources/";
	
	/**
	 * tzz added
	 * is this actor(is player newbie?)
	 */ 
	public static bool IsNewPlayer(){
		if(Globals.Instance.MGameDataManager.MActorData != null){
			return Globals.Instance.MGameDataManager.MActorData.BasicData.Level <= 20;
		}else{
			return false;
		}
	}
	
	public enum BundleType 
    {
        BUNDLE_SERVER,
        BUNDLE_LOCAL,
        BUNDLE_NONE
    }
	
	public static bool USE_ASSET_BUNDLE = false;
    public static BundleType m_bundleType = BundleType.BUNDLE_SERVER;
	
	public static bool IsUseStream()
    {
		return false;
        #if !UNITY_EDITOR && UNITY_WEBPLAYER
            return true;
        #endif
        return m_bundleType == BundleType.BUNDLE_SERVER || m_bundleType == BundleType.BUNDLE_LOCAL;
    }

    public static string  GetUrlBase()
    {
        if (m_bundleType == BundleType.BUNDLE_SERVER)
        {
			string baseUrl = SERVER_URL_BASE;
			
			if (GameDefines.OutputVerDefs == OutputVersionDefs.Windows)
				//return "file://" + Application.dataPath + "/assetbundles/";
				return baseUrl += "IPhone/";
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
				return baseUrl += "Android/";
			else if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
				return baseUrl += "IPhone/";
			return baseUrl += "IPhone/";
	   }
      return "file://" + Application.dataPath + "/assetbundles/";;
    }

	/// <summary>
	/// 存放由.zip解压的assetbundle的地方
	/// </returns>
	public static string  GetUnZipAssetBundlePath()
	{
		return Application.persistentDataPath + "/Resources/";
	}
	
	#region GameSetting
	/// <summary>
	/// The low performence system.
	/// </summary>
	public static bool HighPerformenceSystem 
	{
		get{

			if (Application.isEditor)
				return true;

			#if UNITY_ANDROID
			
			return  SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures && SystemInfo.systemMemorySize > 1024 && !SystemInfo.graphicsDeviceVersion.Contains("OpenGL ES 2.0") ;
			
			#endif
			
			#if UNITY_IPHONE
			
			return  SystemInfo.supportsImageEffects && SystemInfo.supportsRenderTextures && SystemInfo.systemMemorySize > 768 ;
			#endif

			return true;
		}
	}
	
	
	public static bool Setting_IsAutoLogin  = false;
	public static bool Setting_IsRemberPassword = false;
	public static bool Setting_IsGuest		= false;
	public static string System_GuestAccount = "";
	public static string Setting_LoginName		= "";
	public static string Setting_LoginPass		= "";
	public static string Setting_LogingServer	= "";
	
	public static bool	Setting_ScreenQuality	= true;
	public static bool	Setting_Gravity			= false;
	public static bool  Setting_UserDefined     = false;
	public static int	Setting_MusicVol		= 100;
	public static int	Setting_SoundVol		= 100;
	public static int	Setting_Language		= 0;
	
	public static bool	Setting_SkipCopyCameraTrack = true;
	public static bool	Setting_ShakeEnable		= true;
	
	public static string systemMacAdress = "";
	public static string systemIFAD = "";
	public static string systemUDID = "";
	
	
	public static bool   TempSetting_IsGuest		= false;
	public static string TempSystem_GuestAccount    = "";
	public static string TempSetting_LoginName		= "";
	public static string TempSetting_LoginPass		= "";
	public static string TempSetting_LogingServer	= "";

	public static string GameStartDateTime = "2018-1-1";

	public static float EliminationFallingSpeed = 2f;

	/// <summary>
	/// The config filename.
	/// </summary>
	private static readonly string ConfigFilename = "/configure.set";
	
	
	
	/// <summary>
	/// The config file version.
	/// </summary>
	private static readonly int		ConfigFileVersion = 3;
	
	public static readonly int   AssetBundleVersion = 3;
	
		
	/// <summary>
	/// Reads the config file.
	/// </summary>
	public static void ReadConfigFile(){
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath + ConfigFilename,FileMode.Open,FileAccess.Read)){
				using(StreamReader t_sr = new StreamReader(t_file)){
					int t_version 		= int.Parse(t_sr.ReadLine());
					
					Setting_IsAutoLogin = bool.Parse(t_sr.ReadLine());
					Setting_IsAutoLogin = false;
					Setting_IsGuest = bool.Parse(t_sr.ReadLine());
					Setting_IsRemberPassword = bool.Parse(t_sr.ReadLine());
					Setting_LoginName	= t_sr.ReadLine();
					Debug.Log(Setting_LoginName);
					Setting_LoginPass	= CryptHelper.AESDecrypt(t_sr.ReadLine());
					Debug.Log(Setting_LoginPass);
					Setting_LogingServer= t_sr.ReadLine();
					
					Setting_ScreenQuality = bool.Parse(t_sr.ReadLine());
					Setting_Gravity		= bool.Parse(t_sr.ReadLine());
					Setting_UserDefined = bool.Parse(t_sr.ReadLine());
					Setting_MusicVol	= int.Parse(t_sr.ReadLine());
					Setting_SoundVol	= int.Parse(t_sr.ReadLine());
					
					Setting_Language	= int.Parse(t_sr.ReadLine());
					
					if(ConfigFileVersion >= 2){
						Setting_SkipCopyCameraTrack = bool.Parse(t_sr.ReadLine());
					}
					
					if(ConfigFileVersion >= 3){
						Setting_ShakeEnable		= bool.Parse(t_sr.ReadLine());
					}
					
				}
			}
		}catch(System.Exception ex){
			Debug.LogWarning("Open config file failed, Exception:" + ex.Message);
		}
		
	}
	
	/// <summary>
	/// Writes the config file.
	/// </summary>
	public static void WriteConfigFile(){
		try{
			using(FileStream t_file = new FileStream(Application.persistentDataPath + ConfigFilename,FileMode.Create,FileAccess.Write)){
				using(StreamWriter t_sw = new StreamWriter(t_file)){
					t_sw.WriteLine(ConfigFileVersion);
					
					t_sw.WriteLine(Setting_IsAutoLogin); 		// auto login
					t_sw.WriteLine(Setting_IsGuest); 		// auto login
					t_sw.WriteLine(Setting_IsRemberPassword);
					t_sw.WriteLine(Setting_LoginName); 		// username
					Debug.Log(Setting_LoginName);
					t_sw.WriteLine(CryptHelper.AESEncrypt(Setting_LoginPass)); 		// password (must be crypted)
					t_sw.WriteLine(Setting_LogingServer);			// server
					
					t_sw.WriteLine(Setting_ScreenQuality);		// screen high/low quanlity
					t_sw.WriteLine(Setting_Gravity);		// 	gravity enabled
					t_sw.WriteLine(Setting_UserDefined);	// is user changed
					t_sw.WriteLine(Setting_MusicVol);		// music volume
					t_sw.WriteLine(Setting_SoundVol);		// sound volume
					t_sw.WriteLine(Setting_Language);		// language
					
					t_sw.WriteLine(Setting_SkipCopyCameraTrack);		// language
					t_sw.WriteLine(Setting_ShakeEnable);
					
				}
			}
		}catch(System.Exception ex){
			Debug.LogError("Write config file failed, Exception:" + ex.Message);
		}
	}
	#endregion
}

