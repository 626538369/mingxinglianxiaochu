using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EZGUI;

public class Globals : Singleton<Globals>
{
	// Public Access Variable Entrypoint
	public LSNetManager MLSNetManager = null;
	public GSNetManager MGSNetManager = null;
	
	public Starter		MStarter = null;
	
	public DataTableManager MDataTableManager = null;
	public GameDataManager MGameDataManager = null;
	public HQGeneralManager MHQGeneralManager = null;
	
	public GUIManager MGUIManager = null;
	public GUILayoutManager MGUILayoutManager = null;
	public SceneManager MSceneManager = null;
	
	public PlayerManager MPlayerManager = null;
	public NpcManager MNpcManager = null;
	
	public SkillManager MSkillManager = null;
	//public ResourceManager MResourceManager = null;

	public TaskManager MTaskManager = null;
	public EZ3DItemManager M3DItemManager = null;
	public AffectorManager MAffectorManager = null;
	
	//native connnect 
	public NativeConnectManager MConnectManager = null;
	public RepeateEventManager MRepeateEventManager = null;
	
	public EffectManager MEffectManager = null;
	public SoundManager MSoundManager = null;
	
	public CopySweepManager MCopySweepManager = null;
	
	public FingerEvent MFingerEvent = null;
	public CameraController MCameraController = null;
	public CameraTrackController MCamTrackController = null;
	
	// add TeachManager by huxiaolei 2012.8.17
	public TeachManager MTeachManager = null;
	
	public GUIDataManager MGUIDataManager = null;
	
    public BundleManager MBundleManager = null;
    public ResourceMgr MResourceManager = null;
	
	//by lsj port defense manager
	public PortDefenseManager MPortDefenseManager = null;
	public PortVieManager MPortVieManager = null;
	public JunHunManager MJunHunManager = null;
	public JobManager MJobManager = null;
	public ArenaInfoManager mArenaInfoManager = null;
	public ProduceManager   mProduceManager = null;
	public EvolutionManager mEvolutionManager = null;
	public ShopDataManager mShopDataManager = null;
	public PushDataManager MPushDataManager = null;
	// Globals Class
	public Globals()
	{
		
	}
	
	public void Awake()
	{
		GameObject go = GameObject.Find("GlobalScripts");
		if (go != null)
		{
			MLSNetManager = go.GetComponent(typeof(LSNetManager)) as LSNetManager;
			MGSNetManager = go.GetComponent(typeof(GSNetManager)) as GSNetManager;
			
			MDataTableManager = go.GetComponent(typeof(DataTableManager)) as DataTableManager;
			MGameDataManager = go.GetComponent(typeof(GameDataManager)) as GameDataManager;
			MHQGeneralManager = go.GetComponent(typeof(HQGeneralManager)) as HQGeneralManager;
			
			MGUIManager = go.GetComponent(typeof(GUIManager)) as GUIManager;
			MGUILayoutManager = go.GetComponent(typeof(GUILayoutManager)) as GUILayoutManager;
			
			MSceneManager = go.GetComponent(typeof(SceneManager)) as SceneManager;
			
			MPlayerManager = go.GetComponent(typeof(PlayerManager)) as PlayerManager;
			MNpcManager = go.GetComponent(typeof(NpcManager)) as NpcManager;
			MSkillManager = go.GetComponent(typeof(SkillManager)) as SkillManager;
			
			MTaskManager = go.GetComponent<TaskManager>() as TaskManager;
			
			M3DItemManager = go.GetComponent<EZ3DItemManager>() as EZ3DItemManager;
			
			MConnectManager = go.GetComponent<NativeConnectManager>() as NativeConnectManager;
			MRepeateEventManager = go.GetComponent<RepeateEventManager>() as RepeateEventManager;
			
			MAffectorManager = go.GetComponent<AffectorManager>() as AffectorManager;
			MEffectManager = go.GetComponent(typeof(EffectManager)) as EffectManager;
			MSoundManager = go.GetComponent(typeof(SoundManager)) as SoundManager;
			
			MCopySweepManager = go.GetComponent(typeof(CopySweepManager)) as CopySweepManager;
			
			MFingerEvent = go.GetComponent<FingerEvent>() as FingerEvent;
			MCameraController = go.GetComponentInChildren<CameraController>() as CameraController;
			MCamTrackController = go.GetComponentInChildren<CameraTrackController>() as CameraTrackController;
			
			MTeachManager = go.GetComponent(typeof(TeachManager)) as TeachManager;
			MGUIDataManager = go.GetComponent(typeof(GUIDataManager)) as GUIDataManager;
			MPushDataManager = go.GetComponent(typeof(PushDataManager)) as PushDataManager;
			
			MBundleManager = go.GetComponent(typeof(BundleManager)) as BundleManager;
			MResourceManager = go.GetComponent<ResourceMgr>() as ResourceMgr;
			MPortDefenseManager = go.GetComponent<PortDefenseManager>() as PortDefenseManager;
			MPortVieManager = go.GetComponent<PortVieManager>() as PortVieManager;
			MJunHunManager = go.GetComponent<JunHunManager>() as JunHunManager;
			MJobManager = go.GetComponent<JobManager>() as JobManager; 
			mArenaInfoManager = go.GetComponent<ArenaInfoManager> () as ArenaInfoManager;
			mProduceManager = go.GetComponent<ProduceManager>() as ProduceManager;
			mEvolutionManager = go.GetComponent<EvolutionManager>() as EvolutionManager;
			mShopDataManager = go.GetComponent<ShopDataManager>() as ShopDataManager;
			MStarter = go.GetComponent<Starter>();
		}
	}
	
	public bool Initialize()
	{
		if(MConnectManager.GetCurrNetworkInfo() == ""){
			PopupNetworkProblemDlg();
			return false;
		}else{
		} // End if (MConnectManager.GetCurrNetworkInfo() == "")
		
		allDontDestroyObjs.Clear();
		
		UILabel.SetWordConfigDelegate
		(
			delegate(int _code)
			{
				return  MDataTableManager.GetWordText(_code);
			}
		);
		return true;
	}
	
	
	public void Release()
	{
		// Disconnect network
		if (MLSNetManager.Connected)
		{
			MLSNetManager.Disconnect();
		}
		if (MGSNetManager.Connected)
		{
			MGSNetManager.Disconnect();
		}

		CharacterCustomizeOne.ReleaseData();

		MGameDataManager.Release();
		
		GameStatusManager.Instance.Release();
		EventManager.UnsubscribeAll();
		
		System.GC.Collect();
	}
	
	public bool IsSwitchServer = false;
	public bool IsStarterRestart = false;
	public void GameInsideToServerList()
	{
		IsSwitchServer = true;
		Restart();
	}

	
	public void Restart()
	{
		Debug.Log("[Globals]: Restart begin...");
		
		GameDefines.WriteConfigFile();
		//GUIVipStore.WritePendingOrderIds();
		
		ThirdPartyPlatform.CloseSDK();
		
		
		Debug.Log("[Globals]: Restart call Release()...");
		Globals.Instance.Release();
		GameObject dontAutoDelObj = GameObject.Find("GlobalScripts");
		if (null != dontAutoDelObj)
		{
			GameObject.Destroy(dontAutoDelObj);

		}

		dontAutoDelObj = GameObject.Find("StaticRes");
		if (null != dontAutoDelObj)
		{
			GameObject.Destroy(dontAutoDelObj);
		}
		
		dontAutoDelObj = GameObject.Find("UI Root");
		if (null != dontAutoDelObj)
		{
			GameObject.Destroy(dontAutoDelObj);

		}
		
		dontAutoDelObj = GameObject.Find("CameraControl");
		if (null != dontAutoDelObj)
		{
			GameObject.Destroy(dontAutoDelObj);

		}

		dontAutoDelObj = GameObject.Find("TaskCameraControl");
		if (null != dontAutoDelObj)
		{
			GameObject.Destroy(dontAutoDelObj);
//			GameObject.DestroyImmediate(dontAutoDelObj);
		}
		
		Debug.Log("[Globals]: Restart call Application.LoadLevel...");
		Application.LoadLevel("SceneStart");
		
		Debug.Log("[Globals]: Restart End...");
	}
	
	//! popup network problem dialog
	public void PopupNetworkProblemDlg(){
		string wordText = MDataTableManager.GetWordText(1013);
		MGUIManager.CreateGUIDialog(delegate(GUIDialog gui)
		{
			gui.SetTextAnchor(ETextAnchor.MiddleLeft, false);
			gui.SetDialogType(EDialogType.CHECK_NETWORK, wordText);
		},EDialogStyle.DialogOk);
	}
	
	public delegate void TestNetDelegate(bool isDone);
	public void TestNetwork(TestNetDelegate del)
	{
		MStarter.TestNetwork(del);
	}

	public void QuitGame()
	{
		GameDefines.WriteConfigFile();
		//GUIVipStore.WritePendingOrderIds();
		
		// Quit game
		Application.Quit();
		
		// Test the Process state
		if (!Application.isEditor)
		{
			Process currentProcess = Process.GetCurrentProcess();
			if(currentProcess != null) {
				currentProcess.Kill();
			}
		}
	}
	
	void AddDontDestroyObj(string levelName, GameObject obj)
	{
	}
	
	Dictionary<string, List<GameObject>> allDontDestroyObjs = new Dictionary<string, List<GameObject>>();
}
