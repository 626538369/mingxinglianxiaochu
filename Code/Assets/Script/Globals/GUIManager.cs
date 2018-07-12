using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GUIFontColor
{
		
	public static string HightLight = "[BC9303]";
	
	public static string White = "[FFFFFF]";
	public static string MilkyWhite = "[FEFEFE]"; // 255 138 0
	public static string OffWhite = "[B6B4B0]"; //182 180 176
	public static string Black = "[000000]"; //182 180 176
	public static string PureRed = "[FF0000]";
	//public static string PureRed = "[FF2B4A]";
	public static string PureGreen = "[00FF00]";
	public static string PureBlue = "[0000FF]";
	
	public static string Gray = "[625F2C]";
	public static string Orange = "[FFA500]";
	public static string Purple = "[D016F2]"; // 208 22 242
	public static string Yellow = "[FFEA00]"; // 255 234 0
	public static string Yellowish = "[FF8A00]"; // 255 138 0
	public static string DarkBlue = "[0F6000]"; // 15 96 0
	
	public static string Color1H = "[F4DA9F]"; // 244 218 159
	public static string Color2H = "[BC9543]"; // 188 149 67
	
	public static string Color5H = "[59D200]"; // 89 210 0
	public static string LightBlue = "[147DFF]"; // 20 125 255
	public static string BloodRed = "[FF1C0C]"; // 255 28 12
	
	public static string Color9H = "[E243FF]"; // 226 67 255
	public static string Color10H = "[4493FF]"; // 68 147 255
	public static string Color11H = "[DC4747]"; // 220 71 71
	public static string Color12H = "[E8D38C]"; // 232 211 140
	public static string Color13H = "[E0B457]"; // 224 180 87
	public static string Color14H = "[FFF8C0]"; // 255 248 192
	public static string Color15H = "[51AB81]"; // 18 171 129
	public static string Color16H = "[FFF075]"; // 255 240 117  
	public static string Color17H = "[8A8DFD]"; // 138 141 253
	
	// public static string Purple = "[#FFFFFF]"; // 208 22 242
	// public static string Yellow = "[#FFFFFF]"; // 255 234 0
	// public static string Yellowish = "[#FFFFFF]"; // 255 138 0
	// public static string Color2H = "[#FFFFFF]"; // 188 149 67
	
	public static string NothingColor = "[FDFDCC]";
	public static string FloralWhite247246220 = "[FEF6DC]"; // 247 246 220
	// public static string NewColor2H = "[#000000FF]"; // 0 0 0
	public static string Red255000000 = "[FF0000]"; // 255 0 0
	public static string Green000255000 = "[00FF00]"; // 0 255 0
	public static string Gold254221001 = "[FEDD01]"; // 254 221 1
	public static string Purple219039252 = "[DB27FC]"; // 219 39 252
	public static string Blue014213249 = "[0ED5F9]"; // 14 213 249
	
	public static string DarkRed210000005 = "[D20005]"; // 210 0 5
	public static string LimeGreen089210000 = "[59D200]"; // 89 210 0
	public static string SeaGreen068199120 = "[44C778]"; // 68 199 120
	public static string Tan188149067 = "[BC9543]"; // 188 149 67
	public static string Cyan000255204 = "[00FFCC]"; // 0 255 204
	
	public static string LightBlueSky000216255 = "[00D8FF]"; // 0 216 255
	
	public static string White255255255 = "[FFFFFF]"; // 255 255 255
	public static string Black000000000 = "[000000]"; // 0 0 0
	public static string Gray182182182 = "[B6B6B6]"; // 182 182 182
	
	public static string NewColorEnd = "[-]"; // 171 168 168
	
	public static Color ConvertColor(string str)
	{
		char[] c = str.ToCharArray();
		float r = StrParser.ParseHexInt(str.Substring(2,2),0) / 255f;
		float g = StrParser.ParseHexInt(str.Substring(4,2),0) / 255f;
		float b = StrParser.ParseHexInt(str.Substring(6,2),0) / 255f;
		float a = StrParser.ParseHexInt(str.Substring(8,2),0) / 255f;
		return new Color(r,g,b,a);
	}
	
	public static string ConvertFrom(Color col)
	{
		string result = "[#";
		
		int val = (int)(col.r * 255);
		result += val.ToString("X2");
		
		val = (int)(col.g * 255);
		result += val.ToString("X2");
		
		val = (int)(col.b * 255);
		result += val.ToString("X2");
		
		val = (int)(col.a * 255);
		result += val.ToString("X2");
		
		result += "]";
		
		return result;
	}
	
	public static string ConvertFrom(SeaClientColorType seaColType)
	{
		if (seaColType == SeaClientColorType.NothingColor)
			return NothingColor;
		
		else if (seaColType == SeaClientColorType.FloralWhite247246220)
			return FloralWhite247246220;
		// else if (seaColType == SeaClientColorType.NewColor2H)
		//	return NewColor2H;
		else if (seaColType == SeaClientColorType.Red255000000)
			return Red255000000;
		else if (seaColType == SeaClientColorType.Green000255000)
			return Green000255000;
		else if (seaColType == SeaClientColorType.Gold254221001)
			return Gold254221001;
		else if (seaColType == SeaClientColorType.Purple219039252)
			return Purple219039252;
		else if (seaColType == SeaClientColorType.Blue014213249)
			return Blue014213249;
		else if (seaColType == SeaClientColorType.DarkRed210000005)
			return DarkRed210000005;
		else if (seaColType == SeaClientColorType.LimeGreen089210000)
			return LimeGreen089210000;
		else if (seaColType == SeaClientColorType.SeaGreen068199120)
			return SeaGreen068199120;
		else if (seaColType == SeaClientColorType.Tan188149067)
			return Tan188149067;
		else if (seaColType == SeaClientColorType.Cyan000255204)
			return Cyan000255204;
		else if (seaColType == SeaClientColorType.LightBlueSky000216255)
			return LightBlueSky000216255;
		
		else if (seaColType == SeaClientColorType.White255255255)
			return White255255255;
		else if (seaColType == SeaClientColorType.Black000000000)
			return Black000000000;
		else if (seaColType == SeaClientColorType.Gray182182182)
			return Gray182182182;
		
		return NothingColor;
	}
	
	/// <summary>
	/// Res the build format.
	/// </summary>
	/// <returns>
	/// The build format.
	/// </returns>
	/// <param name='destText'>
	/// Destination text.
	/// </param>
	/// <param name='sFormat'>
	/// S format.
	/// </param>
	public static string ReBuildFormat(UILabel destText, string sFormat)
	{
		string[] parses = new string[]{"{0}", "{1}", "{2}", "{3}", "{4}"};
		
		string origTextCol = "";
		//if (destText.SeaColor == SeaClientColorType.NothingColor)
		//{
		//	origTextCol = GUIFontColor.ConvertFrom(destText.color);
		//}
		//else
		//{
		//	origTextCol = GUIFontColor.ConvertFrom(destText.SeaColor);
		//}
			
		string[] results = sFormat.Split(parses, StringSplitOptions.None);
		
		bool isInHead = 0 == sFormat.IndexOf(parses[0]);
		bool isInEnd = false;
		for (int i = 0; i < parses.Length; i++)
		{
			isInEnd = sFormat.IndexOf(parses[i]) == (sFormat.Length - 3);
		}
		
		sFormat = "";
		for (int i = 0; i < results.Length; i++)
		{
			if (isInHead && i == 0)
			{
				sFormat += (parses[i] + origTextCol + results[i]);
				continue;
			}
			
			sFormat += (origTextCol + results[i] + parses[i]);
		}
		
		if (!isInEnd)
			sFormat = sFormat.Remove(sFormat.Length - 3);
		
		return sFormat;
	}
}

public class GUIControlState
{
	public static int RADIO_BTN_TRUE = 0;
	public static int RADIO_BTN_FALSE = 1;
	public static int RADIO_BTN_DISABLED = 2;
	
	public static int BTN_NORMAL = 0;
	public static int BTN_OVER = 1;
	public static int BTN_ACTIVE = 2;
	public static int BTN_DISABLE = 3;
	
	public static int STATE_TOGGLE_BTN_NORMAL = 0;
	public static int STATE_TOGGLE_BTN_SELECT = 1;
}

public class GUIManager : MonoBehaviour 
{
	
	//public static float DEFAULT_SCREEN_WIDTH = 2048f;
	//public static float DEFAULT_SCREEN_HEIGHT = 1536f;
	
	public static float DEFAULT_SCREEN_WIDTH = 1152;
	public static float DEFAULT_SCREEN_HEIGHT = 2048;
	
	// ------------------------------------------
	public const float GUI_NEAREST_Z = 20.0f;
	public const float GUI_FARTHEST_Z = 500.0f;
	public const float GUI_ORDER_SPACE = 20.0f;
	// ------------------------------------------
	
	// ------------------------------------------
	public delegate void CreateGUICallback<T> (T guiPage) where T: GUIWindow;
	public delegate void GUICallback<T> (T guiPage) where T: GUIWindow;
	// ------------------------------------------
	
	// ------------------------------------------
	public GUIRadarScan WatchLoading
	{
		get { return radarScaning; }
	}
	// ------------------------------------------
	
	void Awake()
	{
		MGUILayer = LayerMaskDefine.GUI;
		MIsHitUIObject = false;
		
		MGUICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		DontDestroyOnLoad(MGUICamera);
		
		DontDestroyOnLoad(MGUICamera.transform.parent);
	


		// Calculate EZGUI 3D Bound
		Vector3 screenCenter = MGUICamera.transform.position;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		
		string t_machineType = "";
//		if(t_machineType.StartsWith("iPhone4")
//		|| t_machineType.StartsWith("iPhone5")
//		|| t_machineType.StartsWith("iPhone6")
//		|| t_machineType.StartsWith("iPhone7")
//		|| t_machineType.StartsWith("iPad")
//		|| t_machineType.StartsWith("iPod5")
//		|| t_machineType.StartsWith("iPod6")
//		|| t_machineType.StartsWith("iPod7")){
			
		Debug.Log("Device is " + t_machineType + " Screen.width:" + Screen.width + " Screen.height:" + Screen.height);
		
		// auto resize 
		MGUICamera.orthographicSize = screenHeight * 0.5f;
		widthRatio = screenWidth / GUIManager.DEFAULT_SCREEN_WIDTH;
		heightRatio = screenHeight / GUIManager.DEFAULT_SCREEN_HEIGHT;
		
		//calculate the minimum ratio by lsj
		if(widthRatio <= heightRatio)
		{
			windowFormHRatio = windowFormWRatio = widthRatio;
		}
		else
		{
			windowFormHRatio = windowFormWRatio = heightRatio;
		}
		
	// comment by lsj		
	//		widthRatioInv = 1 / widthRatio;
	//		heightRatioInv = 1 / heightRatio;
		
	// 	comment by lsj
	//	if (height < 2 * MGUICamera.orthographicSize)
	//	{
	//		height = 2 * MGUICamera.orthographicSize;
	//	}
		
		
		
		
		Vector3 topLeft = screenCenter;
		topLeft.x -= 0.5f * screenWidth;
		topLeft.y += 0.5f * screenHeight;
		
		Vector3 topRight = screenCenter;
		topRight.x += 0.5f * screenWidth;
		topRight.y += 0.5f * screenHeight;
		
		Vector3 bottomLeft = screenCenter;
		bottomLeft.x -= 0.5f * screenWidth;
		bottomLeft.y -= 0.5f * screenHeight;
		
		M3DScreenRect = new Rect3D(topLeft, topRight, bottomLeft);
		
		_mModalWindow = null;
		_mTopWindow = null;
		
		_mGUIWindows = new List<GUIWindow>();
		_mCreateGUIQueue = new Queue<IEnumerator>();
		
		// read the culling mask
		//mMainCameraCullMask = Camera.main.cullingMask;
	}
	
	// Use this for initialization
	void Start () 
	{
	
		
		CreateWindow<GUIRadarScan>(delegate(GUIRadarScan gui)
		{
			radarScaning = gui;
			radarScaning.SetVisible(false);
		}
		);
				
		
		CreateWindow<GUIGuoChang>(delegate(GUIGuoChang gui)
		{
			gui.SetVisible(false);
		}
		);

		CreateWindow<GUIBeginnersGuide>(delegate(GUIBeginnersGuide gui)
		                          {
			gui.SetVisible(false);
		}
		);
		
		// tzz added 
		// create tool tips first otherwise show two tips in one frame will be un-order
		CreateWindow<GUIToolTips>(delegate(GUIToolTips gui){
			gui.SetVisible(false);
		});
		
		// Add GUIWindow listener
		_mGUICreate = EventManager.Subscribe(GUIWindowPublisher.NAME + ":" + GUIWindowPublisher.EVENT_CREATE);
		_mGUICreate.Handler = delegate (object[] args)
		{
			GUIWindow wnd = (GUIWindow)(args[0]);
		};
		
		_mGUIDestroy = EventManager.Subscribe(GUIWindowPublisher.NAME + ":" + GUIWindowPublisher.EVENT_DESTROY);
		_mGUIDestroy.Handler = delegate (object[] args)
		{
			GUIWindow wnd = (GUIWindow)(args[0]);
		};
		
		_mChangeModal = EventManager.Subscribe(GUIWindowPublisher.NAME + ":" + GUIWindowPublisher.EVENT_CHANGE_MODAL);
		_mChangeModal.Handler = delegate (object[] args)
		{
			GUIWindow wnd = (GUIWindow)(args[0]);
			_mModalWindow = wnd;
			UpdateGUIWindowOrder();
		};
		
		_mChangeGUILevel = EventManager.Subscribe(GUIWindowPublisher.NAME + ":" + GUIWindowPublisher.EVENT_CHANGE_LEVEL);
		_mChangeGUILevel.Handler = delegate (object[] args)
		{
			GUIWindow wnd = (GUIWindow)(args[0]);
			int level = (int)(args[1]);
			
			UpdateGUIWindowOrder();
		};
		
		_mChangeVisible = EventManager.Subscribe(GUIWindowPublisher.NAME + ":" + GUIWindowPublisher.EVENT_CHANGE_VISIBLE);
		_mChangeVisible.Handler = delegate (object[] args)
		{
			GUIWindow wnd = (GUIWindow)(args[0]);
			bool visible = (bool)(args[1]);
		};
	}
	
	void OnDestroy()
	{	
		_mGUIWindows.Clear();
		_mCreateGUIQueue.Clear();
		_mTempWindowList.Clear();
		
		if (null != _mGUICreate)
			_mGUICreate.Unsubscribe();
		if (null != _mGUIDestroy)
			_mGUIDestroy.Unsubscribe();
		if (null != _mChangeModal)
			_mChangeModal.Unsubscribe();
		if (null != _mChangeGUILevel)
			_mChangeGUILevel.Unsubscribe();
		if (null != _mChangeVisible)
			_mChangeVisible.Unsubscribe();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_mCreateGUIQueue.Count != 0)
		{
			IEnumerator iterator = _mCreateGUIQueue.Peek();
			if (!iterator.MoveNext())
			{
				_mCreateGUIQueue.Dequeue();
			}
		}
	}
	
	public bool guiPointerInfoListener(Vector2 fingerPos )
	{
		MIsHitUIObject = false;
		
		Ray ray = MGUICamera.ScreenPointToRay(fingerPos);//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
         if(Physics.Raycast(ray,out hitInfo))
		{
             GameObject gameObj = hitInfo.collider.gameObject;
             Debug.Log("click object name is " + gameObj.name);
			 if (gameObj != null && gameObj.active)
			{
				MIsHitUIObject = true;
				return true;
				Debug.Log("MIsHitUIObject is true   !!!!!!!!!!!!! ");
			}
		}
		return false;	
	}
	
	private void CreateGUIWindow<T>(string name, bool showLoading, CreateGUICallback<T> callback) where T : GUIWindow
	{
		_mCreateGUIQueue.Enqueue(DoCreateGUIWindow<T>(name, showLoading, callback));
		
		// tzz added for requireing window create
		Statistics.INSTANCE.GUIWndRequireCreateWnd(name);
	}
	
	IEnumerator DoCreateGUIWindow<T> (string name, bool showLoading, CreateGUICallback<T> callback) where T : GUIWindow
	{
		if (GameDefines.USE_ASSET_BUNDLE)
		{
			if (!Globals.Instance.MBundleManager.CheckLocalLevelExist(name)) 
			{
			    if (Globals.Instance.MBundleManager.GetBundle(name) == null)
	            {
				    List<string> resList = new List<string>();
	                resList.Add(name);
	                while (!Globals.Instance.MBundleManager.LoadBundles(resList.ToArray(), true))
	                {
	                    yield return new WaitForSeconds(0.1f);
	                }
	                while (true)
	                {
	                    if (Globals.Instance.MBundleManager.GetIsDone() == BundleManager.LoadResultEnum.SUCC)
	                    {
	                        break;
	                    }
	                    else if (Globals.Instance.MBundleManager.GetIsDone() == BundleManager.LoadResultEnum.FAIL)
	                    {
	                        Debug.LogError("load scene error" + name);
	                    }
	                    yield return new WaitForSeconds(0.1f);
	                }
	            }
			}
		}
		
		// tzz modified for showloading parameter
		if (null != radarScaning && showLoading){
			radarScaning.SetVisible(true);
		}

		AsyncOperation asynOp = Application.LoadLevelAdditiveAsync(name);
		while (!asynOp.isDone)
		{
			yield return null;
		}
		
		// tzz modified to comment following statement
		// the GUIRadarScan.Hide() was called in GUIWindow.Start function
		//
//		if (null != radarScaning)
//			radarScaning.SetVisible(false);
		
		// Resolve the bug: the gui GameObject maybe delete when change different level.
		// yield return null;
		
		// Call callback
		T gui = null;
		GameObject go = GameObject.Find(name);	
		if (null != go)
		{
			gui = go.GetComponent<T>() as T;
		}
		else
		{
			Debug.Log("[GUIManager:] Cann't find the gui " + name);
		}
		
		if (null != gui){
			
			callback(gui);
			
			if(Globals.Instance.MTeachManager != null){
				Globals.Instance.MTeachManager.NewOpenWindowEvent(gui.name);
			}			
		}
		
		// tzz added for requireing window create
		Statistics.INSTANCE.GUIWndCreatedWnd(name);
	}
	
	public void CloseGUIWindow(GUIWindow window)
	{
		window.Close();
	}
	
	public void CloseGUIWindowAboveLevel(int level)
	{
		_mGUIWindows.RemoveAll(delegate(GUIWindow window)
        {
            if (window.GUILevel >= level)
            {
				window.Close();
                return true;
            }
			
            return false;
        }
		);
	}
	
	public T GetGUIWindow<T>() where T : GUIWindow
	{
		
		System.Type type = typeof (T);
		UnityEngine.Profiling.Profiler.BeginSample("GUIManager.GetGUIWindow<" + type.Name + ">");		
				
		foreach (GUIWindow window in _mGUIWindows)
		{
			if (window.GetType() == type)
			{
				UnityEngine.Profiling.Profiler.EndSample();
				return (T)(window);
			}
		}
		
		UnityEngine.Profiling.Profiler.EndSample();
		return (T)null;
	}
	
	public List<GUIWindow> GetGUIWindowList()
	{
		return _mGUIWindows;
	}
	
	public void UpdateGUIWindowOrder()
	{
		float zAxisPos = GUI_NEAREST_Z;
		if (null != _mModalWindow)
		{
			Vector3 pos = _mModalWindow.transform.localPosition;
			pos.z = zAxisPos;
			_mModalWindow.transform.localPosition = pos;
			
			zAxisPos += GUI_ORDER_SPACE;
		}
		
		if (null != _mTopWindow)
		{
			Vector3 pos = _mTopWindow.transform.localPosition;
           	pos.z = zAxisPos;
			_mTopWindow.transform.localPosition = pos;
			
			zAxisPos += GUI_ORDER_SPACE;
		}
		
		// General Window
		_mGUIWindows.Sort();
		
		// Setting the GUIWindow z position
		foreach (GUIWindow wnd in _mGUIWindows)
		{
			if (wnd.GUILevel < 0)
			{
                continue;
			}
			
			if (wnd == _mModalWindow || wnd == _mTopWindow)
				continue;
			
            Vector3 pos = wnd.transform.localPosition;
            pos.z = zAxisPos + wnd.GUILevel * GUI_ORDER_SPACE;
			if (pos.z > GUI_FARTHEST_Z)
				pos.z = GUI_FARTHEST_Z;
			
			wnd.transform.localPosition = pos;
		}
		
		// if (null != _mFullScreenBG)
		// {
		// 	Vector3 pos = _mFullScreenBG.transform.localPosition;
		// 	pos.z = GUI_FARTHEST_Z;
		// 	_mFullScreenBG.transform.localPosition = pos;
		// }
	}
	
	public bool IsTopWindow(GUIWindow window)
	{
		return _mTopWindow == window;
	}
	
	public void SetTopWindow(GUIWindow window)
	{
		_mTopWindow = window;
		
		// Flush it
	}
	
	public void AddWindow(GUIWindow window)
	{		
		if (window.IsModal)
			_mModalWindow = window;
		
		_mGUIWindows.Add(window);
		NotifyFullScreenShiled();
		
		EnableDisableMainCamera();
	}
	
	public void RemoveWindow(GUIWindow window)
	{
		Globals.Instance.MGUILayoutManager.RemoveFromLayout(window);
			
		if (window == _mModalWindow)
			_mModalWindow = null;
		
		_mGUIWindows.Remove(window);
		NotifyFullScreenShiled();
		
		EnableDisableMainCamera();
	}
	
	public void DoubleLayoutGUI(GUIWindow w1, GUIWindow w2, float duration, float delay)
	{
		Globals.Instance.MGUILayoutManager.CreateDoubleLayout(w1, w2, duration, delay);
	}
	
	/// <summary>
	/// tzz added
	/// Enables or disable the main camera when GUIWindowForm is create or close
	/// </summary>
	public void EnableDisableMainCamera(bool forceEnable = false){
		
	
	}
	
	public bool HandleNotEnoughMoney(int needVal)
	{
		int currVal = Globals.Instance.MGameDataManager.MActorData.WealthData.Money;
		if (currVal < needVal)
		{
			this.CreateGUIDialog(delegate(GUIDialog gui) 
			{
				gui.SetTextAnchor(ETextAnchor.MiddleCenter, false);
				gui._mTipsTextST.text = Globals.Instance.MDataTableManager.GetWordText(22600096);
				gui.GetOKBtn().GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(23800003);
			}, EDialogStyle.DialogOkCancel, delegate() {
				NetSender.Instance.RequestHasGift();
				NetSender.Instance.RequestHasGift();
				GUIRadarScan.Show();
				Globals.Instance.MGUIManager.mReserveOpenYuanbaoDuiHuan = true;
			});
			
			return true;
		}
		
		return false;
	}
	
	public void PopupNotEnoughMoney()
	{
		this.CreateGUIDialog(delegate(GUIDialog gui) 
		{
			gui.SetTextAnchor(ETextAnchor.MiddleCenter, false);
			gui._mTipsTextST.text =  Globals.Instance.MDataTableManager.GetErrorCodeText(20006);
			//gui.GetOKBtn().guiText.text = Globals.Instance.MDataTableManager.GetWordText(23800003);
		}, EDialogStyle.DialogOkCancel, delegate() {
			Globals.Instance.MGUIManager.CreateWindow<GUIPurchase>(delegate(GUIPurchase guiPage) {
				guiPage.ShowPurshaseMoneyInfor();
			});
		});
	}
	
	public void PopupNotEnoughDiamond()
	{
		this.CreateGUIDialog(delegate(GUIDialog gui) 
		{
			gui.SetTextAnchor(ETextAnchor.MiddleCenter, false);
			gui._mTipsTextST.text = Globals.Instance.MDataTableManager.GetErrorCodeText(20007);
			//gui.GetOKBtn().guiText.text = Globals.Instance.MDataTableManager.GetWordText(22600101);
		}, EDialogStyle.DialogOkCancel, delegate() {
			Globals.Instance.MGUIManager.CreateWindow<GUIPurchase>(delegate(GUIPurchase guiPage) {
				guiPage.ShowPurshaseDiamondInfor();
			});
		});
	}
	
	public void PopupNotEnoughXingdong()
	{
		mReserveNotEnoughXingDongLi = true;
		NetSender.Instance.RequestBuyOilPrice();
	}
	
	public void PopupNotEnoughExchange()
	{
		this.CreateGUIDialog(delegate(GUIDialog gui) 
		{
			gui.SetTextAnchor(ETextAnchor.MiddleCenter, false);
			gui._mTipsTextST.text = Globals.Instance.MDataTableManager.GetWordText(22600099);
			gui.GetOKBtn().GetComponent<GUIText>().text = Globals.Instance.MDataTableManager.GetWordText(22600102);
		}, EDialogStyle.DialogOkCancel, delegate() {
	
		});
	}
	
	public void ShowErrorTips(int errorCode,bool Isopen = true)
	{
		if (errorCode == 20006) // Not enough Money
		{
			PopupNotEnoughMoney();
			return;
		}
		else if (errorCode == 20007) // Not enough GoldIgnot
		{
			PopupNotEnoughDiamond();
			return;
		}
		else if(errorCode == 6901 ||
			  errorCode == 68003) // Not enough Xingdongli
		{
			PopupNotEnoughXingdong();
			return;
		}
		else if(errorCode == 20025 // Xingdongli allowed exchange count is 0
			|| errorCode == 63001) // GoldIgnot allowed exchange count is 0
		{
			PopupNotEnoughExchange();
			return;
		}
		
		CreateWindow<GUIToolTips>(delegate(GUIToolTips gui)
		{
			gui.IsAutoHide = true;
			gui.Duration = 2.0f;
			
			string text = Globals.Instance.MDataTableManager.GetErrorCodeText(errorCode);
			
			// tzz added for empty error code
			if(text == null || text.Length == 0){
				text = "Unknown Error Code " + errorCode;
			}
			
			gui.ShowTips(typeof(SimpleTipsContent), Vector3.zero, Vector2.zero,Isopen, GUIFontColor.White + text);
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/NormalFail");
		}
		);
	}
	
	public void ShowSimpleCenterTips(int wordconfig,bool Isopen = true,float durationTime = 2.0f)
	{
		ShowSimpleCenterTips(Globals.Instance.MDataTableManager.GetWordText(wordconfig),Isopen,durationTime);
	}
	
	public void ShowSimpleCenterTips(string text,bool Isopen = true, float durationTime = 2.0f)
	{
		CreateWindow<GUIToolTips>(delegate(GUIToolTips gui){
			gui.IsAutoHide = true;
			gui.Duration = durationTime;
			
			gui.ShowTips(typeof(SimpleTipsContent), Vector3.zero, Vector2.zero,Isopen,text);
			Globals.Instance.MSoundManager.PlaySoundEffect("Sounds/UISounds/NormalFail");
		});
	}
	
	public void PlayLevelUpEffect(EZ3DItemManager.EffectEndDelegate callback)
	{
		Globals.Instance.M3DItemManager.PlayLevelUpEffect(Vector3.zero, delegate(){
			if (null != callback){
				callback();
			}
		});
		
		// GUIMain gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
		// if (null != gui && gui.IsVisible)
		// {
		// 	Globals.Instance.M3DItemManager.PlayLevelUpEffect(gui.GetButtonLevelPosition(), delegate(){
		// 		if (null != callback){
		// 			callback();
		// 		}
		// 	});
		// }
	}
	
	public void PlayRankUpEffect(EZ3DItemManager.EffectEndDelegate callback)
	{
		GUIMain gui = Globals.Instance.MGUIManager.GetGUIWindow<GUIMain>();
		if (null != gui && gui.IsVisible)
		{
			Globals.Instance.M3DItemManager.PlayRankUpEffect(gui.GetButtonRankPosition(), delegate() 
			{
				if (null != callback)
					callback();
			});
		}
	}
		
	/// <summary>
	/// Creates the GUI dialog to show
	/// </summary>
	/// <param name='callback'>
	/// Callback. when intialize is over
	/// </param>
	/// <param name='style'>
	/// Style.
	/// </param>
	/// <param name='_dele'>
	/// _dele function callback when dialog confirm button is clicked
	/// </param>
	public void CreateGUIDialog(GUICallback<GUIDialog> callback,EDialogStyle style,GUIDialog.ConfirmDelegate _dele = null)
	{
		CreateWindow<GUIDialog>(delegate(GUIDialog gui){
			
			gui.InitializeGUI(style);
			gui.SetConfirmBtnDelegate(_dele);
			
			if(callback != null){
				callback(gui);
			}			
		});
	}
	
	/// <summary>
	/// Creates the window.
	/// </summary>
	/// <returns>
	/// return true if the window create otherwise false because creating or created
	/// </returns>
	/// <param name='callback'>
	/// If set to <c>true</c> callback.
	/// </param>
	/// <typeparam name='T'>
	/// the window type (GUIWindow's child class)
	/// </typeparam>
	public bool CreateWindow<T>(GUICallback<T> callback) where T:GUIWindow
	{
		if(_mTempWindowList.IndexOf(typeof(T)) != -1){
			return false;
		}
		
		T ui = GetGUIWindow<T>();
		if (null != ui)
		{
			if (null != callback){
				callback(ui);
			}
			
			return false;
		}
		_mTempWindowList.Add(typeof(T));
				
		CreateGUIWindow<T>
		(typeof(T).Name, false, 
			delegate(T gui)
			{
				_mTempWindowList.Remove(typeof(T));
			
				gui.InitializeGUI();
				
				if (null != callback){
					callback(gui);
				}
			}
		);
		
		return true;
	}
	
	/// <summary>
	/// Determines whether this instance is created window.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is created window; otherwise, <c>false</c>.
	/// </returns>
	/// <typeparam name='T'>
	/// The 1st type parameter.
	/// </typeparam>
	public bool IsCreatedWindow<T>()where T:GUIWindow{
		if(_mTempWindowList.IndexOf(typeof(T)) != -1){
			return true;
		}
		
		return GetGUIWindow<T>() != null;
	}
	
	/// <summary>
	/// Removes the state of the GUI window loading.
	/// </summary>
	/// <param name='_type'>
	/// _type of GUIWindow's child class
	/// </param>
	public void RemoveGUIWindowLoadingState(System.Type _type){
		_mTempWindowList.Remove(_type);
	}
	
	private void NotifyFullScreenShiled()
	{
		return;
		
		// foreach (GUIWindow wnd in _mGUIWindows)
		// {
		// 	if (null != _mFullScreenBG)
		// 		_mFullScreenBG.SetVisible(false);
		// 	
		// 	if (wnd.GetType() == typeof(GUILoading) 
		// 			|| wnd.GetType() == typeof(GUIFullScreenShield)
		// 			|| wnd.GetType() == typeof(GUIMain)
		// 			|| wnd.GetType() == typeof(GUIBattle)
		// 			|| wnd.GetType() == typeof(GUITaskTrack)
		// 			)
		// 			continue;
		// 	
		// 	if (!wnd.IsVisible)
		// 		continue;
		// 	
		// 	if (null != _mFullScreenBG)
		// 	{
		// 		_mFullScreenBG.SetVisible(true);
		// 		_mFullScreenBG.transform.localPosition = new Vector3(0.0f, 0.0f, wnd.transform.localPosition.z + 1.0f);
		// 	}
		// 	
		// 	break;
		// }
	}
	
	/// <summary>
	/// Shows the attribute tips.
	/// </summary>
	/// <param name='icon'>
	/// Icon.
	/// </param>
	public void ShowAttributeTips(AttributeIcon icon){
		if(mAttributeTipInstance == null){
			mAttributeTipInstance = (AttributeTipBubble)Instantiate(AttributeTipPrefab);
		}
		
		mAttributeTipInstance.ShowAttrTips(icon);
	}
	
	/// <summary>
	/// Shows the attribute tips by the SpriteRoot (PackedSprite\UIButton\....)
	/// </summary>
	/// <param name='sprite'>
	/// Sprite.
	/// </param>
	/// <param name='title'>
	/// Title.
	/// </param>
	/// <param name='desc'>
	/// Desc.
	/// </param>
	public void ShowAttributeTips(SpriteRoot sprite,string title,string desc){
		if(mAttributeTipInstance == null){
			mAttributeTipInstance = (AttributeTipBubble)Instantiate(AttributeTipPrefab);
		}
		
		mAttributeTipInstance.ShowAttrTips(sprite,title,desc);
	}
	
	//-----------------------------------------------------------------------------
	public static Vector3 WorldToGUIPoint(Vector3 worldPos)
	{
		// tzz added
		// judge whether this worldPos is behind the camera		
		if(IsBehindCamera(worldPos)){
			return new Vector3(0,3000,0);
		}
		
		worldPos = Globals.Instance.MSceneManager.mMainCamera.WorldToScreenPoint(worldPos);
        worldPos = ScreenToGUIPoint(worldPos);
        return worldPos;
	}
	
	/// <summary>
	/// Determines whether this instance is behind camera the specified worldPos.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is behind camera the specified worldPos; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='worldPos'>
	/// If set to <c>true</c> world position.
	/// </param>
	public static bool IsBehindCamera(Vector3 worldPos){
		
		// tzz added
		// judge whether this worldPos is behind the camera
		Vector3 tDir = (worldPos - Globals.Instance.MSceneManager.mMainCamera.transform.position);
		tDir.Normalize();
		
		if(Vector3.Dot(tDir,Globals.Instance.MSceneManager.mMainCamera.transform.forward) < 0){
			return true;
		}
		
		return false;
	}
	
	public static Vector3 GUIToWorldPoint(Vector3 guiPos)
	{
        guiPos = GUIToScreenPoint(guiPos);
		guiPos = Globals.Instance.MSceneManager.mMainCamera.ScreenToWorldPoint(guiPos);
        return guiPos;
	}
	
	public static Vector3 ScreenToGUIPoint(Vector3 screenPos)
	{
		screenPos.x -= Screen.width * 0.5f;
        screenPos.y -= Screen.height * 0.5f;
        return screenPos;
	}
	
	public static Vector3 GUIToScreenPoint(Vector3 guiPos)
	{
		guiPos.x += Screen.width * 0.5f;
        guiPos.y += Screen.height * 0.5f;
        return guiPos;
	}
	
	public static Vector3 WorldToGUIPoint(Camera cam, Vector3 worldPos)
	{
		worldPos = cam.WorldToScreenPoint(worldPos);
        return ScreenToGUIPoint(worldPos);
	}
	
	public void ReceiveMessage(GameObject go, string funcName)
	{
		return;
		if (funcName == "OnPress")
		{
			Debug.Log("OnPress is " + UICamera.currentTouch.pos);
			ParticleObjs.transform.localPosition = new Vector3((UICamera.currentTouch.pos.x - Screen.width/2)/widthRatio,(UICamera.currentTouch.pos.y - Screen.height/2)/heightRatio,-500f);
			ParticleSystem[] systems  = ParticleObjs.GetComponentsInChildren<ParticleSystem>();
			foreach(ParticleSystem Part in systems)
			{
				Part.Play();
			}
		}
	}
	
	//-----------------------------------------------------------------------------
	
	public int MGUILayer;
	public bool MIsHitUIObject;
	public Camera MGUICamera;
	
	public float widthRatio;
	public float heightRatio;
	public float windowFormWRatio;
	public float windowFormHRatio;
	public int screenWidth;
	public int screenHeight;
//	public float widthRatioInv;
//	public float heightRatioInv;
	
	// Use EZGUI Rect3D
	public Rect3D M3DScreenRect;
	
	/// <summary>
	/// The attribute tip bubble prefab
	/// </summary>
	public AttributeTipBubble		AttributeTipPrefab;
		
	/// <summary>
	/// The m attribute tip instance.
	/// </summary>
	public AttributeTipBubble		mAttributeTipInstance = null;
	
	private GUIWindow _mModalWindow;
	private GUIWindow _mTopWindow;
	private GUIWindow _mFullScreenBG;
	private List<GUIWindow> _mGUIWindows;
	
	Queue<IEnumerator> _mCreateGUIQueue;	 
	
	private ISubscriber _mChangeModal;
	private ISubscriber _mChangeGUILevel;
	private ISubscriber _mChangeVisible;
	private ISubscriber _mGUICreate;
	private ISubscriber _mGUIDestroy;
	
	/// <summary>
	/// The m main camera cull mask.
	/// </summary>
	private int			mMainCameraCullMask;
	
	// tzz added for loading state of GUIWindow's child class
	List<System.Type> _mTempWindowList = new List<System.Type>();
	
	private GUIRadarScan radarScaning;
	public bool mReserveOpenYuanbaoDuiHuan = false;
	public bool mReserveOpenXingDongLiDuiHuan = false;
	public bool mReserveNotEnoughXingDongLi = false;
	
	public GameObject ParticleObjs;
	
}

