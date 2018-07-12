using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResAssetBundle;
using Mono.Xml;
using System.Text;

/// <summary>
/// Download delegate to call back when downloads resources.
/// <param name='progress'>
/// Prgogress value is between 0 and 1. 1 means finished.
/// </param>
/// <param name='userData'>
/// User data is the user specified call back data.
/// </param>
/// </summary>
public delegate void DownloadDelegate (float progress,System.Object userData);

/// <summary>
/// Resource manager class is used to download or create assets.
/// usage:
/// It notice outside through progress delegate after assets are downloaded.
/// you will use load method in your update thread.
/// </summary>
public class ResourceManager:MonoBehaviour
{
	public static void Download (DownloadDelegate prgDelegate, params string []assetFiles)
	{
		Download (null, prgDelegate, null, true, assetFiles);
	}
	
	/// <summary>
	/// Download the specified assets.
	/// </summary>
	/// <param name='prgDelegate'>
	/// Prgogress delegate to be callback after downloaded.
	/// </param>
	/// <param name='assetFiles'>
	/// Asset files.
	/// </param>
	public static void Download (DownloadDelegate prgDelegate, bool isReturnPerFrame, params string []assetFiles)
	{
		Download (null, prgDelegate, null, isReturnPerFrame, assetFiles);
	}
	
	/// <summary>
	/// Download the specified assets.
	/// </summary>
	/// <param name='prgDelegate'>
	/// Prgogress delegate to be callback after downloaded.
	/// </param>
	/// <param name='prgUserData'>
	/// Prgogress delegate user data.
	/// </param>
	/// <param name='assetFiles'>
	/// Asset files.
	/// </param>
	public static void Download (DownloadDelegate prgDelegate, System.Object prgUserData,bool isReturnPerFrame, params string []assetFiles)
	{
		Download (null, prgDelegate, prgUserData, isReturnPerFrame, assetFiles);
	}
	
	/// <summary>
	/// Download the specified assets.
	/// </summary>
	/// <param name='dlObject'>
	/// The download object which affact the downloading task.
	/// The task can be canceled by destroying this download object.
	/// </param>
	/// <param name='prgDelegate'>
	/// Prgogress delegate to be callback after downloaded.
	/// </param>
	/// <param name='prgUserData'>
	/// Prgogress delegate user data.
	/// </param>
	/// <param name='assetFiles'>
	/// Asset files.
	/// </param>
	public static void Download (GameObject dlObject, DownloadDelegate prgDelegate, System.Object prgUserData, bool isReturnPerFrame, params string []assetFiles)
	{
		if (assetFiles.Length == 0)
			return;
		
		if (_initialzed != EInializedState.Initialized) {
			DownloadTask dlTask = new DownloadTask ();
			dlTask.DownloadObect = dlObject;
			dlTask.ProgressFunction = prgDelegate;
			dlTask.ProgressUserData = prgUserData;
			
			foreach (string assetFile in assetFiles) {
				dlTask.AssetFiles.Add (assetFile);
			}
			
			_pendingTask.Add (dlTask);
			
			return;
		}
		
		if (dlObject == null)
			dlObject = _downloadObject;
		
		ResourceLoader resDl = dlObject.AddComponent<ResourceLoader> ();
		resDl.SetProgressDelegate (prgDelegate, prgUserData ,isReturnPerFrame);
		
		foreach (string assetFile in assetFiles) {
			resDl.AddAssetLoader (AssetLoaderManager.CreateLoader (ToManifestPath (assetFile), _manifest));
		}
	}
	
	public static GameObject LoadNowTmp (string assetFile)
	{
		GameObject go = Instantiate (Resources.Load (assetFile), new Vector3 (0f, 0f, 0f), new Quaternion (0f, 0f, 0f, 0f)) as GameObject;
		//go.animation.playAutomatically = false;
		return go;
	}
	
	public static Object LoadNow (string assetFile)
	{
		return Resources.Load (assetFile);
	}
	
	private static AssetLoader _assetLoader = null;
	
	/// <summary>
	/// Load the specified asset object.
	/// The Load Method will be invoke in update thread
	/// </summary>
	/// <param name='assetFile'>
	/// Asset file.
	/// </param>
	public static void Load (string assetFile,out float progress,out Object loadObj,out bool isDone)
	{
		if (_initialzed != EInializedState.Initialized)
		{
			progress = 0F;
			loadObj = null;
			isDone = false;
			return;
		}
			
		/*
		if (assetFile.StartsWith ("resources/")) {
			assetFile = PathUtil.RemoveLocalFlag (assetFile);
			progress =1.0F;
			loadObj = Resources.Load (assetFile);
			return;
		}
		*/
		
		progress = 0F;
		loadObj = null;
		isDone = false;
		if (_assetLoader != null) {
			/*
			if (_assetLoader.Progress >= 1.0F) {
				loadObj = _assetLoader.Load ();
				_assetLoader = null;
			}else{
				loadObj = null;
			}
			progress = _assetLoader.Progress;
			*/
			progress = _assetLoader.Progress;
			isDone = _assetLoader.IsDone;
			if(isDone)
			{
				loadObj = _assetLoader.Load ();
				_assetLoader = null;
			}
		} else {
			assetFile = PathUtil.UnifyPath (assetFile);
			_assetLoader = AssetLoaderManager.CreateLoader (ToManifestPath (assetFile), _manifest);
		}
	}
	
	
	public static Object LoadAfterDownLoad(string assetFile)
	{
		if (_initialzed != EInializedState.Initialized)
			return null;
		
		assetFile = PathUtil.UnifyPath (assetFile);
		AssetLoader assetLoader = AssetLoaderManager.CreateLoader (ToManifestPath (assetFile), _manifest);
		if(assetLoader.Progress>=1.0F)
		{
			return assetLoader.Load();
		}
		else
		{
			return null;
		}
	}
	
	public static Object Load(string assetFile)
	{
		return Resources.Load (assetFile);
	}
	
	//private static List<string> _currentAssetPriList = new List<string> ();
	//private static string _currentAssetPriFileName = "";
	//private static int _currentAssetPriLevel;
	
	/*
	public static void DownLoadPriCallBack (float progress, System.Object userData)
	{
		if (progress < 1.0f)
			return;
		
		Debug.Log("DOWNLOAD CALLBACK");
		_currentAssetPriList.Remove (_currentAssetPriFileName);
		_currentAssetPriFileName = "";
		if (_currentAssetPriList.Count == 0) {
			Debug.Log("ONE COLLECTION DOWNLOAD OVER");
			_assetPriList.RemoveFirst ();
			
			//PlayerLevelXX == 1  this collection download over
			PlayerPrefs.SetInt ("PlayerLevel" + _currentAssetPriLevel, 1);
		}
	}*/
	
	/// <summary>
	/// Update this instance.
	/// Manager needs to be called during every frame to be drive downloading tasks.
	/// </summary>
	public static void Update ()
	{
		
		WwwLoaderManager.Update ();
		
		if (_initialzed != EInializedState.Initialized) {
			return;
		}
		
		
		
		/*
		if (_assetPriList.Count != 0 && _currentAssetPriList.Count == 0) {
			Debug.Log("BEGIN DOWN COLLECTION");
			Debug.Log("_assetPriList.Count:"+_assetPriList.Count);
			Dictionary<int, List<string>> tmpDict = _assetPriList.First.Value;
			foreach (KeyValuePair<int, List<string>> tmpK in tmpDict) {
				_currentAssetPriLevel = tmpK.Key;
				if(PlayerPrefs.GetInt("PlayerLevel"+_currentAssetPriLevel) == 1 ){
					Debug.Log("THE COLLECTION DOWNLOAD ALREADY OVER");
					_assetPriList.RemoveFirst();
					break;
				}
				List<string> ss = tmpK.Value;
				if(ss.Count!=0)
				{
					_currentAssetPriList = tmpK.Value;
				}else{
					_assetPriList.RemoveFirst();
					break;
				}
				
			}
		}
		
		if(_currentAssetPriList.Count != 0 && _currentAssetPriFileName.Equals(""))
		{
			Debug.Log("BEGIN DOWN COLLECTION FILE");
			_currentAssetPriFileName = _currentAssetPriList.ToArray()[0];
			Download (DownLoadPriCallBack, new string []{_currentAssetPriFileName});
		}
		*/
	}
	
	/// <summary>
	/// Initialize the manager with pecified basePath, manifestPackage and manifestFile.
	/// Before download asset, manager needs to be initialzed firstly.
	/// 
	/// </summary>
	/// <param name='basePath'>
	/// Game resources base url path.
	/// </param>
	/// <param name='manifestPackage'>
	/// Manifest package file name.
	/// </param>
	/// <param name='manifestFile'>
	/// Manifest file name.
	/// </param>
	public static void Initialize (string basePath, string manifestFile, string manifestPackage, string assetPackageFile, string assetPackagePackage, string assetWeightFile, string assetWeightPackage)
	{
		Debug.Log("Resource Manager Init ");
		if (_initialzed != EInializedState.NotInitialized)
			return;
		
		//PlayerPrefs.DeleteAll();
		//PlayerPrefs.SetInt("PlayerLevel",1);
		
		_initialzed = EInializedState.Initializing;
		
		
		WwwLoaderManager.BasePath = basePath;
		_manifestFile = ToManifestPath (manifestFile);
		_manifestPackage = manifestPackage;
		
		_assetPackageFile = assetPackageFile;
	 	_assetPackagePackage = assetPackagePackage;
	 	_assetWeightFile = assetWeightFile;
	 	_assetWeightPackage = assetWeightPackage;
		
		//_assetPriFile = ToManifestPath (assetPriFile);
		//_assetPriPackage = assetPriPackage;
		
		_downloadObject = new GameObject ("ResourceLoader");
		_resourceLoader = _downloadObject.AddComponent<ResourceLoader> ();
		
		AssetLoader assetLoaderManifest = AssetLoaderManager.CreateLoader (_manifestFile, _manifestPackage);
		//AssetLoader assetLoaderAssetPri = AssetLoaderManager.CreateLoader (_assetPriFile, _assetPriPackage);
		
		List<AssetLoader> assetLoaderList = new List<AssetLoader>();
		assetLoaderList.Add(assetLoaderManifest);
		//assetLoaderList.Add(assetLoaderAssetPri);
		
		_resourceLoader.SetProgressDelegate (InitializeCallback,assetLoaderList,true);
		_resourceLoader.AddAssetLoader (AssetLoaderManager.CreateLoader (_manifestFile, _manifestPackage));
		//_resourceLoader.AddAssetLoader (AssetLoaderManager.CreateLoader (_assetPriFile, _assetPriPackage));
		
	}
	
	/// <summary>
	/// Releases all resource used by the <see cref="ResourceManager"/> object.
	/// </summary>
	/// <remarks>
	/// Call <see cref="Dispose"/> when you are finished using the <see cref="ResourceManager"/>. The <see cref="Dispose"/>
	/// method leaves the <see cref="ResourceManager"/> in an unusable state. After calling <see cref="Dispose"/>, you must
	/// release all references to the <see cref="ResourceManager"/> so the garbage collector can reclaim the memory that
	/// the <see cref="ResourceManager"/> was occupying.
	/// </remarks>
	public static void Dispose ()
	{
		_initialzed = EInializedState.NotInitialized;
		GameObject.Destroy (_downloadObject);
		_manifest = null;
		_manifestFile = "";
		_manifestPackage = "";
		//_assetPriFile = "";
		//_assetPriPackage = "";
//		_assetPri = null;
		_pendingTask.Clear ();
		AssetLoaderManager.DestroyAllLoaders ();
	}
	
	protected static void InitializeCallback (float progress, System.Object userData)
	{
		Debug.Log("PROCESS:"+progress);
		if (progress < 1.0f) {
			return;
		}
		List<AssetLoader> assetLoaderList = (List<AssetLoader>)userData;
		
		_manifest = new Manifest ();
		
		//AssetLoader assetLoader = AssetLoaderManager.CreateLoader (_manifestFile, _manifestPackage);
		AssetLoader assetLoader =assetLoaderList[0];
		TextAsset manifestText = assetLoader.Load () as TextAsset;
		SecurityParser xmlParser = new SecurityParser ();
		xmlParser.LoadXml (manifestText.text);
		_manifest.Load (xmlParser.ToXml ());
		
		/*
		//AssetLoader assetLoader1 = AssetLoaderManager.CreateLoader (_assetPriFile, _assetPriPackage);
		AssetLoader assetLoader1 = assetLoaderList[1];
		TextAsset assetPriText = assetLoader1.Load () as TextAsset;
		SecurityParser xmlParser1 = new SecurityParser ();
		xmlParser1.LoadXml (assetPriText.text);
		_assetPri = new AssetPri ();
		_assetPri.Load (xmlParser1.ToXml ());
		
		//get pending AssetPri list
		//_assetPriList = _assetPri.GetAssetsInLevelAndNetwork (PlayerPrefs.GetInt ("PlayerLevel"), GetNetworkType (), _assetPriList);
	
		//_assetPri.GetAssetsInLevelAndNetwork (PlayerPrefs.GetInt ("PlayerLevel"), GetNetworkType (), _assetPriList);
		*/
		
		AssetPriManager assetPriManager = _downloadObject.AddComponent<AssetPriManager> ();
		assetPriManager.Init(_assetPackageFile, _assetPackagePackage, _assetWeightFile, _assetWeightPackage, _downloadObject, _manifest);
		
		_initialzed = EInializedState.Initialized;
		
		// Process pending task.
		foreach (DownloadTask dltask in _pendingTask) {
			Download (dltask.DownloadObect, dltask.ProgressFunction, dltask.ProgressUserData, true, dltask.AssetFiles.ToArray ());
		}
		
		_pendingTask.Clear ();
	}
	
	protected static int GetNetworkType ()
	{
		int netWorkType = 0;
		return netWorkType;
	}
	
	protected static string ToManifestPath (string assetFile)
	{
		if (assetFile.StartsWith ("assets/")) {
			return PathUtil.UnifyPath (assetFile);
		}
		StringBuilder sb = new StringBuilder ();
		sb.AppendFormat ("assets/{0}", assetFile);
		return PathUtil.UnifyPath (sb.ToString ());
	}
	
	protected class DownloadTask
	{
		public DownloadDelegate ProgressFunction {
			get { return _prgDelegate; }
			set { _prgDelegate = value; }
		}
		
		public System.Object ProgressUserData {
			get { return _prgDelegateData; }
			set { _prgDelegateData = value; }
		}
		
		public List<string> AssetFiles {
			get { return _assetFiles; }
		}
		
		public GameObject DownloadObect {
			get { return _downladObject; }
			set { _downladObject = value; }
		}
		
		protected GameObject _downladObject;
		protected DownloadDelegate _prgDelegate;
		protected System.Object _prgDelegateData;
		protected List<string> _assetFiles = new List<string> ();
	}
	
	protected enum EInializedState
	{
		NotInitialized,
		Initializing,
		Initialized,
	}

	
	protected static ResourceLoader _resourceLoader;
	protected static string _manifestPackage;
	protected static string _manifestFile;
	//protected static string _assetPriPackage;
	//protected static string _assetPriFile;
	protected static Manifest _manifest;
	//protected static AssetPri _assetPri;
	//protected AssetPriManager assetPriManager;
	protected static LinkedList<Dictionary<int, List<string>>> _assetPriList = new LinkedList<Dictionary<int, List<string>>> ();
	protected static GameObject _downloadObject;
	protected static EInializedState _initialzed = EInializedState.NotInitialized;
	protected static List<DownloadTask> _pendingTask = new List<DownloadTask> ();
	
	protected static string _assetPackageFile;
	protected static string _assetPackagePackage;
	protected static string _assetWeightFile;
	protected static string _assetWeightPackage;
}
