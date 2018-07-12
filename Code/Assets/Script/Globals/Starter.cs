using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;


using System.Reflection;
using System.Reflection.Emit;


/// <summary>
/// Starter.
/// </summary>
public class Starter : MonoBehaviour 
{
	public UILabel textShow;
	// public PackedSprite packedLogo;
	
	public Transform companyLogoRoot;
	public UISpriteAnimation logoAnimitation;
	public UITexture logoName;
	
	//public MovieTexture titleOfVideo = null;
	
	public bool isAnimationOn;
	//! tzz added for buffer Enter game packet
	[HideInInspector]public Packet m_bufferLoginProofPacket = null;
	[HideInInspector]public Packet m_bufferEnterGamePacket = null;
	
		
	// Use this for initialization
	protected void Awake()
	{		
		publisher.NotifyMonoAwake();
		// Don't destroy this script when loading.
		DontDestroyOnLoad(this.gameObject);
		
		GameObject tStaticRes = GameObject.Find("StaticRes");
		if(tStaticRes != null)
		{
			DontDestroyOnLoad(tStaticRes);
		}
				
		// Start time
		mDataTime = new System.DateTime();
		mDataTime = System.DateTime.Now;
		mLastFrameTime = mDataTime;
		
		Input.imeCompositionMode = IMECompositionMode.On;
		//-------------------
		// Set random seed.
		UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
		phoneImei = SystemInfo.deviceUniqueIdentifier;
		//-------------
		
		
		Debug.Log("Application.persistentDataPath : " +Application.persistentDataPath);
		
		// 2012.05.22 LiHaojie Adjust the targetFrameRate is 35 fps
		Application.targetFrameRate = 35 ;
		Application.runInBackground = true;
		
		Globals.Instance.Awake();
		
		GameSystemInfo.CollectSystemInfo();
		//TalkingDataGA.SessionStarted("01BD57E566CD46FF3889ED4A4D547BD4", "gfan");
		
		if (!GameDefines.ToastEnabled)
		{
			HelpUtil.DestroyObject(textShow.gameObject);
			textShow = null;
		}


//		AdaptiveUI ();

	}
	static private void AdaptiveUI()
	{
		int ManualWidth = 1536;
		int ManualHeight = 2048;
		UIRoot uiRoot = GameObject.FindObjectOfType<UIRoot>();
		if (uiRoot != null)
		{
			if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
				uiRoot.manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
			else
				uiRoot.manualHeight = ManualHeight;
		}
	}


	// Use this for initialization
	protected void Start()
	{
		if(!CheckGoogleResource()){
			return;
		}

		if(!TapjoyUnity.Tapjoy.IsConnected){
			TapjoyUnity.Tapjoy.Connect();
		}

		UnityEngine.Profiling.Profiler.enabled = false;
		publisher.NotifyMonoStart();
		// read game defined setting
		GameDefines.ReadConfigFile();

		
		// Unified the login logic, move it to ThirdPartyPlatform.AtlanticLogin
		// FadeInLogo();
		
		if (!Globals.Instance.Initialize())
			return ;		

		Debug.Log("/var/mobile/Applications/3D2A01DB-BDCB-40CE-A537-E04FCAD5EBD0/Documents"); 
		Debug.Log("Application.persistentDataPath is :" + Application.persistentDataPath); 

		//PlayTitleMovie();
		ThirdPartyPlatform.InitSDK();

		GameStatusManager.Instance.Initialize();
		GameStatusManager.Instance.SetGameState(GameState.GAME_STATE_INITIAL);
		
//		AndroidSDKAgent.getMacAddress ();
	}


	public bool CheckGoogleResource(){
		if(Application.isEditor){
			return true;
		}
		if(GameDefines.OutputVerDefs == OutputVersionDefs.WPay){
			if (!GooglePlayDownloader.RunningOnAndroid())
			{
				Debug.Log ("Use GooglePlayDownloader only on Android device!");
				return false;
			}

			string expPath = GooglePlayDownloader.GetExpansionFilePath();
			if (expPath == null)
			{
				Debug.Log ("External storage is not available!");
				return false;
			}
			else
			{
				string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				Debug.Log ("Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
				if (mainPath == null /*|| patchPath == null*/) {
					GooglePlayDownloader.FetchOBB();
					return false;
				}
			}
		}
		return true;
	}

	public void PlayTitleMovie()
	{
		if (Application.isEditor)
		{
			ShowCompanyLogo();
		}
		else
		{
			ShowCompanyLogo();
#if UNITY_ANDROID || UNITY_IPHONE
			//Handheld.PlayFullScreenMovie("JianghuPianTou.mp4", Color.black, 
				//FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFill);
		
			// // When touch screen, the video auto close
			// Handheld.PlayFullScreenMovie("", Color.black, FullScreenMovieControlMode.CancelOnInput);
			// 
			// // Can not close, auto close when playing is end
			// Handheld.PlayFullScreenMovie("", Color.black, FullScreenMovieControlMode.Hidden);
			// 
			// // Can control the play progress
			// Handheld.PlayFullScreenMovie("", Color.black, FullScreenMovieControlMode.Minimal);
			// ToLoading();
#endif
		}
		
		// titleOfVideo.loop = false;
		// GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), titleOfVideo, ScaleMode.StretchToFill);
		// titleOfVideo.Play();
	}
	
	void ShowCompanyLogo()
	{
		companyLogoRoot.gameObject.SetActiveRecursively(true);
		companyLogoRoot.localScale = new Vector3(Screen.width / GUIManager.DEFAULT_SCREEN_WIDTH, Screen.height / GUIManager.DEFAULT_SCREEN_HEIGHT, 1.0f);
		//logoAnimitation.Reset();
		//StartCoroutine(DoPlayLogoAnim());
		
		EventDelegate.Add(TweenAlpha.Begin(logoName.gameObject,2f,0.0f).onFinished, delegate() {
			ToLoading();
			ThirdPartyPlatform.InitSDK();
			Destroy(companyLogoRoot.gameObject);		
		});
	}
	
	IEnumerator DoPlayLogoAnim()
	{
		NGUITools.SetActive(logoName.transform.gameObject,false);
	
		yield return new WaitForSeconds(1.5f);
	
		NGUITools.SetActive(logoName.transform.gameObject,true);
		
		
		Color c_Start = new Color(1,1,1,0.0f);
		Color c_End = new Color(1,1,1,1);
		
		
		EventDelegate.Add(TweenAlpha.Begin(logoName.gameObject,2f,0.0f).onFinished , delegate() {
			ToLoading();
			ThirdPartyPlatform.InitSDK();
			Destroy(companyLogoRoot.gameObject);		
			});
	}
	
	public void FadeInLogo()
	{
		AutoSizeLogo();
		
		Debug.Log("Unity...  FadeInLogo");
		isAnimationOn = true;
		Color c_Start = new Color(1,1,1,0.0f);
		Color c_End = new Color(1,1,1,1);
		// FadeSprite.Do(packedLogo,EZAnimation.ANIM_MODE.FromTo,c_Start,c_End,EZAnimation.backInOut,2,0,null,delegate(EZAnimation anim){  FadeOutLogo(); });
	}
	
	public void FadeOutLogo()
	{
		Debug.Log("Unity...  FadeOutLogo");
		Color c_End = new Color(1,1,1,0f);
		Color c_Start = new Color(1,1,1,1);
		// FadeSprite.Do(packedLogo,EZAnimation.ANIM_MODE.FromTo,c_Start,c_End,EZAnimation.backInOut,2,0,null,delegate(EZAnimation anim){  ToLoading(); });
	}
	
	public void AutoSizeLogo()
	{
		// packedLogo.transform.localScale = new Vector3(Screen.width / 960f, Screen.height / 640f, 1.0f);
		// packedLogo.SetSize(packedLogo.width * Screen.width / 960f,packedLogo.height * Screen.height / 640f);
	}
	
	public void ToLoading()
	{
		Debug.Log("Unity...  ToLoading");
		isAnimationOn = false;
		
		if(m_bufferLoginProofPacket != null){
			Globals.Instance.MLSNetManager.ReceiveLoginProofNew(m_bufferLoginProofPacket);
			m_bufferLoginProofPacket = null;
		}else{
			GUIRadarScan.Show();
		}
	}

	protected void Update()
	{
		// Process escape key.
#if UNITY_ANDROID
		if (Input.GetKey("escape"))
		{
			Debug.Log("Input.GetKey(escape)");
			if (Globals.Instance.MGUIManager)
			{
				string wordText = Globals.Instance.MDataTableManager.GetWordText(1016); // "Confirm quit game";
				Globals.Instance.MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
				{
					gui.SetTextAnchor(ETextAnchor.MiddleCenter, false);
					gui.SetDialogType(EDialogType.QUIT_GAME, wordText);
				}
				, EDialogStyle.DialogOkCancel,delegate() 
				{
					Globals.Instance.QuitGame();
				}
				);
			}
			Debug.Log("Input.GetKey(escape)1111111111");
		}
#endif
		UnityEngine.Profiling.Profiler.BeginSample("GameStatusManager.Update");
		GameStatusManager.Instance.Update();
		UnityEngine.Profiling.Profiler.EndSample();
		
		Statistics.INSTANCE.Update();
		
		
		// check
		if (GameDefines.ToastEnabled)
		{
			System.DateTime currentTime = new System.DateTime();
			currentTime = System.DateTime.Now;
			
			// MilliSecond unit
			long span0 = mDataTime.Hour * 3600000 + mDataTime.Minute * 60000 + mDataTime.Second * 1000 + mDataTime.Millisecond;
			long span1 = mLastFrameTime.Hour * 3600000 + mLastFrameTime.Minute * 60000 + mLastFrameTime.Second * 1000 + mLastFrameTime.Millisecond;
			long span2 = currentTime.Hour * 3600000 + currentTime.Minute * 60000 + currentTime.Second * 1000 + currentTime.Millisecond;
			
			mTimeFromStartup = span2 - span0;
			mDeltaTime = span2 - span1;
			mLastFrameTime = currentTime;
			
			mTimeRecord += mDeltaTime;
			mFrameCount++;
			
			if (mTimeRecord >= 1000)
			{
				mTimeRecord /= 1000;
				mFPS = mFrameCount / mTimeRecord;
				
				mTimeRecord = 0;
				mFrameCount	= 0;
			}
			
			if (Globals.Instance != null)
			{
				// Remove Some DebugShow for Look in test by hxl
				StringBuilder t_sb = new StringBuilder();
				t_sb.Append("Fps:").Append(mFPS).Append("\n")
				    .Append("Playerid:").Append(Globals.Instance.MGameDataManager.MActorData.PlayerID).Append("\n")
				//	.Append("Time.deltaTime:").Append(Time.deltaTime).Append("\n\n")
				//	.Append("RoleID:").Append(Globals.Instance.MGameDataManager.MActorData.PlayerID).Append("\n")
				//	.Append("MachineType:").Append(NativeConnectManager.GetMachineType()).Append("\n")
				//	.Append("NetworkInfo:").Append(Globals.Instance.MConnectManager.GetCurrNetworkInfo()).Append("\n")
				//	.Append("ConnectState:").Append(Globals.Instance.MGSNetManager.State).Append("\n")
				//	.Append("ServerIP:").Append(GameDefines.GAME_SERVER_IP).Append("\n")
				//	.Append(TeachManager.mMiJiNum);
						;
				
				if (null != Globals.Instance.MConnectManager)
				{
					//t_sb.Append("\n").Append("AvailMemSize:").Append(Globals.Instance.MConnectManager.GetAvailMemSize());
					//t_sb.Append("\n").Append("TotalMemSize:").Append(Globals.Instance.MConnectManager.GetTotalMemSize());
				}
				
				if (null != textShow)
					textShow.text = t_sb.ToString();
			}
		}
		else
		{
			if (null != textShow)
				textShow.transform.localScale = Vector3.one;
		}
		
		LowPerformencePromptUpdate();
	}
	
	/// <summary>
	/// Lows the performence prompt update.
	/// </summary>
	private void LowPerformencePromptUpdate(){
		
		if(!StartLowPerformenceStat){
			return;
		}
			
		if(!mLoadPromptFile){
			mLoadPromptFile = true;
			
			string tFilename = Application.persistentDataPath + "/" + Globals.Instance.MLSNetManager.CurrGameServer.id + LowPerformencePromptFile;
			try{			
				FileStream t_file = new FileStream(tFilename,FileMode.Open,FileAccess.Read);
				t_file.Close();
				mHasPrompted = true;
			}catch{
				try{
					FileStream t_file = new FileStream(tFilename,FileMode.Create,FileAccess.Write);
					t_file.Close();
				}catch{}
			}
		}
		
		if(!mHasPrompted){
			if(Globals.Instance.MSceneManager.mMainCamera.cullingMask == 0){
				// empty port 
				return;
			}
			
			if(mFPS > 20){
				mHasPrompted = true;
			}
			
			mLowPerformenceStatTimer += Time.deltaTime;
			if(mLowPerformenceStatTimer > 60){
				mHasPrompted = true;
				
				//Globals.Instance.MGUIManager.CreateWindow<GUIConfig>(delegate(GUIConfig gui){
				//	gui.HideLowPerformenceDlg(false);
				//});
			}
		}
	}
	
	void OnApplicationQuit()
	{
		publisher.NotifyMonoQuit();
		// release Debug information
		Debug.Release();
		
		Globals.Instance.QuitGame();
		TalkingDataGA.SessionStoped();
		ThirdPartyPlatform.CloseSDK();
	}

	
	void OnApplicationPause(bool pause)
	{
		//Debug.Log("OnApplicationPause    !!!!!!!" + pause.ToString());
		long timeSpan = 0;
		if(pause)
		{
			lt = System.DateTime.Now;
			isPause = true;
		}
		else
		{
			if(lt == null)
				lt = System.DateTime.Now;
			timeSpan = HelpUtil.GetTimeSpan(lt);
			if(timeSpan < 0)
				timeSpan = 0;
			

			if (isPause )
			{
				isPause = false;
				
				Debug.Log("OnApplicationPause    !!!!!!!" + timeSpan);
				Debug.Log("Application is back form pause!!!!!!!");
				Debug.Log("ThirdPartyPlatform.mIs360Logining is :" + ThirdPartyPlatform.mIs360Logining); 
				//if (!ThirdPartyPlatform.mIs360Logining)
				//{
				//	if (timeSpan > 4 * 60 * 1000)
				//	{
				//		ThirdPartyPlatform.CloseSDK();
				//		Globals.Instance.IsStarterRestart = true;
				//		StartCoroutine(InvokeStaterRestartDelegate());
				//		return ;
				//	}
				//	else if (timeSpan > 60 * 1000)
				//	{
				//		Globals.Instance.MGSNetManager.clearPendingQueueAndReconnect();
				//	}
				//}
			}
		}
		publisher.NotifyMonoPause(pause,timeSpan);
	}
	
	private bool isPause = false;
	
	void OnApplicationFocus(bool focus)
	{
#if UNITY_ANDROID || UNITY_IPHONE		
		if (!Application.isEditor)
		{
			long timeSpan = 0;
			if(!focus)
			{
				lt = System.DateTime.Now;
			}
			else
			{
				if(lt == null)
					lt = System.DateTime.Now;
				timeSpan = HelpUtil.GetTimeSpan(lt);
				if(timeSpan < 0)
					timeSpan = 0;
			
				//Debug.Log("OnApplicationFocus timeSpan " + timeSpan.ToString());
			}
			
			publisher.NotifyMonoFocus(focus,timeSpan);
		}
#endif
	}
	
	public void TestNetwork(Globals.TestNetDelegate del)
	{
		StartCoroutine(DoTestNetwork(del));
	}

	IEnumerator InvokeStaterRestartDelegate()
	{
		yield return new WaitForSeconds(1.0f);
		Globals.Instance.Restart();	
	}
	
	IEnumerator DoTestNetwork(Globals.TestNetDelegate del)
	{
		float testNetTimeout = 1; // second
		float connectingTime = 0;
			
		// [220.181.111.85] is www.baidu.com ip address
		UnityEngine.Ping ping = new UnityEngine.Ping("220.181.111.85");
		while (!ping.isDone && connectingTime < testNetTimeout)
		{
			connectingTime += Time.deltaTime;
			yield return 0;
		}
		
		Debug.Log(ping.ip);
		Debug.Log(ping.time);
		
		bool isSuccessed = ping.isDone;
		ping.DestroyPing();
		if (null != del)
		{
			del(isSuccessed);
		}
		
		yield return 0;
	}
	
	float mTime = 0.0f;
	int mFrameCount = 0;
	float mFPS = 0.0f;
	
	System.DateTime mDataTime;
	System.DateTime mLastFrameTime;
	
	long mTimeFromStartup = 0;
	long mDeltaTime = 0;
	
	long mTimeRecord = 0;
	System.DateTime lt;
	MonoEventPublisher publisher = new MonoEventPublisher();
	//----test imei by lsj
	[HideInInspector]public string phoneImei;
	
	private static readonly string LowPerformencePromptFile = "LowPerformence.a";
	private bool mLoadPromptFile			= false;
	private bool mHasPrompted				= false;
	[HideInInspector]public bool StartLowPerformenceStat	= false;
	
	private float mLowPerformenceStatTimer = 0;
}
