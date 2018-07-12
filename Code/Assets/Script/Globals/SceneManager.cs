using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;

public class ScenePublisher : EventManager.Publisher
{
	public static string NAME = "Scene";
	public static string EVENT_PREPARE_LOAD = "PrepareLoad";
	public static string EVENT_LOADING = "Loading";
	public static string EVENT_LOADED = "Loaded";
	
	public override string Name
	{
		get { return NAME; }
	}
	
	public void NotifyScenePrepareLoad(string levelName)
	{
		base.Notify(EVENT_PREPARE_LOAD, levelName);
	}
	
	public void NotifySceneLoading(string levelName, float progress)
	{
		base.Notify(EVENT_LOADING, levelName, progress);
	}
	
	public void NotifySceneLoaded(string levelName, float progress)
	{
		base.Notify(EVENT_LOADED, levelName, progress);
	}
}

public enum SceneState
{
	STATE_UNKNOW,
	STATE_PREPARE_LOAD,
	STATE_LOADING,
	STATE_LOADED,
}

public class SceneManager  : MonoBehaviour
{
	
	void Awake ()
	{
		mTaskCamera = GameObject.Find("TaskCamera").GetComponent<Camera>();
		DontDestroyOnLoad(mTaskCamera);
	
		mMainCameraControl = GameObject.Find("CameraControl");
		DontDestroyOnLoad(mMainCameraControl);
		
		mTaskCameramControl = GameObject.Find("TaskCameraControl");
		DontDestroyOnLoad(mTaskCameramControl);
		
		mMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		DontDestroyOnLoad(mMainCamera);

		mUICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		DontDestroyOnLoad(mMainCamera);

		
		_mSceneName = string.Empty;
		_mSceneID = -1;
		
		_mSceneState = SceneState.STATE_UNKNOW;
		_mEntryPosition = Vector3.zero;
		
		_mPublisher = new ScenePublisher();
		
		_sceneMainGameObject = UnityEngine.GameObject.Find ("SceneGameObject");
		_isError = false;
		changeSceneState = ChangeSceneState.ChangeInit;

		if (GameDefines.Setting_ScreenQuality)
		{
			mTaskCamera.GetComponent<FastBloom>().fastBloomShader = Shader.Find("Hidden/FastBloom");
			mMainCamera.GetComponent<FastBloom>().fastBloomShader = Shader.Find("Hidden/FastBloom");
			mMainCamera.GetComponent<FastBloom>().enabled = true;
			mTaskCamera.GetComponent<FastBloom>().enabled = true;
		}
		if (Application.isEditor)
		{
			mTaskCamera.GetComponent<FastBloom>().fastBloomShader = Shader.Find("Hidden/FastBloom");
			mMainCamera.GetComponent<FastBloom>().fastBloomShader = Shader.Find("Hidden/FastBloom");
			mMainCamera.GetComponent<FastBloom>().enabled = true;
			mTaskCamera.GetComponent<FastBloom>().enabled = true;
		}
	}
	
	void Start()
	{}
	
	void OnDestroy()
	{}
	
	public enum ChangeSceneState
	{
		ChangeOver = 0,
		Changing = 1,
		ChangeError = 2,
		ChangeInit = 3
		
	}
	
	public enum CameraActiveState
	{
		MAINCAMERA,
		TASKCAMERA,
	}
	
	private void EmptySceneMainGameObject()
	{
		Transform[] childs = _sceneMainGameObject.GetComponentsInChildren<Transform>();
		foreach(Transform child in childs)
		{
			if(child.gameObject.name.Equals("SceneGameObject"))
				continue;
			
			Destroy(child.gameObject);
		}
	}
	
	public string GetSceneName()
	{
		return _mSceneName;
	}
	
	public void ChangeCameraActiveState(CameraActiveState state)
	{
		if (state == CameraActiveState.TASKCAMERA)
		{
			 mTaskCamera.enabled = true;
			 mTaskCamera.targetTexture = null;
			 mMainCamera.enabled = false;
			 GameStatusManager.Instance.MPortStatus.Pause();
		}
		else if (state == CameraActiveState.MAINCAMERA)
		{
			 mTaskCamera.enabled = false;
			 mMainCamera.enabled = true;
			 
			 GameStatusManager.Instance.MPortStatus.Resume();
		}
	}
	
	public void ChangeScene(string levelName, Vector3 enterPosition)
	{
//		if(levelName == "C_SmaTropical"	|| levelName == "C_MidTropical"	|| levelName == "C_BigTropical"){
//			seaPlaneController.iOSForceRepleaceLowPerformanceWater = true;
//		}else{
//		seaPlaneController.iOSForceRepleaceLowPerformanceWater = false;
//		}
		
		_mEntryPosition = enterPosition;
		_mSceneState = SceneState.STATE_PREPARE_LOAD;
		
		// tzz added for destroy all effect font when change scene level
		NewEffectFont.DestroyAllEffectFont();
		
		Globals.Instance.MResourceManager.Load(levelName, delegate(Object obj, string error) 
		{
			if (string.IsNullOrEmpty(error))
			{
				StartCoroutine( DoChangeScene(levelName) );
			}
			else
				Debug.Log("[SceneManger::ChangeScene]-Error: the scene leve is " + levelName);
		}
		);
	}
	
	private IEnumerator DoChangeScene(string levelName)
	{
		_mPublisher.NotifyScenePrepareLoad(levelName);
		
		AsyncOperation ao = Application.LoadLevelAsync(levelName);
		while (!ao.isDone)
		{
			_mSceneState = SceneState.STATE_LOADING;
			
			// Event publisher
			_mPublisher.NotifySceneLoading(levelName, ao.progress);
			
			yield return null;
		}
		
		// Dispose multiple Main Camera
		//UnityEngine.Object[] camObjs = GameObject.FindGameObjectsWithTag(TagMaskDefine.MAIN_CAMERA);
		//List<UnityEngine.Object> tmpObjs = new List<UnityEngine.Object>();
		//for (int i = 0; i < camObjs.Length; i++)
		//{
		//	tmpObjs.Add(camObjs[i]);
		//}
		//while (tmpObjs.Count > 1)
		//{
		//	HelpUtil.DestroyObject(tmpObjs[0]);
		//	tmpObjs.RemoveAt(0);
		//}
		
		// if(levelName.StartsWith("C_")){
		// 	FindWayPoint.Instance.CalcMovePoint();
		// }
		
		_mSceneName = levelName;
		_mSceneState = SceneState.STATE_LOADED;
		_mPublisher.NotifySceneLoaded(levelName, ao.progress);

		if (!GameDefines.Setting_ScreenQuality)
		{
			GameObject mainLight = GameObject.Find("Directional light Main");
			GameObject backLight = GameObject.Find("Directional light Back");
			if (mainLight != null)
			{
				//mainLight.SetActive(false);
				Light mL = mainLight.GetComponent<Light>();
				mL.shadows =  LightShadows.None;
			}
			
			if (backLight != null)
			{
				//backLight.SetActive(false);
			}
		}

		//tzz added for scene fade 
		// SceneFadeInOut.Instance.FadeInScene(3);
		
		yield break;
	}

	private bool _isError;
	private ChangeSceneState changeSceneState;
	private GameObject _sceneMainGameObject;
	
	// 2012.06.01 LiHaojie Refactory the SceneManager class
	private string _mSceneName;
	private int _mSceneID;
	
	private SceneState _mSceneState;
	private Vector3 _mEntryPosition;
	
	// 
	ScenePublisher _mPublisher;
	[HideInInspector]
	public Vector3 initTaskCameraLocalEulerAngles = new Vector3(5,0,0);
	public Camera mTaskCamera;
	public Camera mMainCamera;
	public Camera mUICamera;
	public GameObject mTaskCameramControl;
	public GameObject mMainCameraControl;
	public bool isEmail;
	
}
