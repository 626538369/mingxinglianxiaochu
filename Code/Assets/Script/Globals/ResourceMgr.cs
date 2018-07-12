using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ResourceMgr : MonoBehaviour 
{
	// ------------------------------------------------------------
	public delegate void ResourceDelegate(Object obj, string error);
	// ------------------------------------------------------------
	
	// ------------------------------------------------------------
	public const string BUND_VERSION = "version";
	public const string BUND_DOWNLOAD = "DownloadStrategy";
	public const string BUND_LOCAL_LEVEL = "LocalLevel";
    //public const string BUND_CLIENTPROTO = "ClientProto";
    //public const string BUND_SKILL = "SkillPrefab";
    //public const string BUND_PARTICLE = "ParticlesPrefab";
    //public const string BUND_BUFF = "BuffPrefab";
    //public const string BUND_HIT = "HitPrefab";
    //public const string BUND_COMM = "Comm";
    public const string BUND_UITEXTURE = "UITexture";
    public const string BUND_UISHADER = "UIShader";
	// ------------------------------------------------------------
	
	// ------------------------------------------------------------
	public bool IsPreloadLoaded
	{
		get { return _mPreloadLoaded; }
	}
	// ------------------------------------------------------------
	
	void Awake()
	{
		// _mBundleList = new Dictionary<string, AssetBundle>();
		_mCacheID = new Dictionary<string, int>();
		_mCachePath = new Dictionary<string, string>();
		
		_mLocalResList = new Dictionary<string, string>();
		_mStrategyResList = new List<string>();
		
		_mLocalLevelLoaded = _mStrategyLoaded = _mVersionLoaded = _mPreloadLoaded = false;
	}
	
	// Use this for initialization
	void Start () 
	{
		wwwLoaderMgr = this.transform.GetComponent<WwwLoaderMgr>() as WwwLoaderMgr;
		if (null == wwwLoaderMgr){
			Debug.LogError("Miss the WwwLoaderMgr Component...");
			return;
		}
		
		InitPreloadResources();
	}
	
	// // Update is called once per frame
	// void Update () 
	// {
	// 
	// }
	
	public void Load(string name, ResourceDelegate completeDel)
	{
		if (GameDefines.USE_ASSET_BUNDLE)
		{
			// Parse the relative path
			string url = string.Empty;
			int version = -1;
			if (name.IndexOf("file://") == 0)
			{
				// Local download bundle
				url = name;
				version = -1;
			}
			else
			{
				name = name.Substring(name.LastIndexOf("/") + 1);
				url = GameDefines.GetUrlBase() + GetCachePath(name) + name + ".assetbundle";
				version = GetCacheID(name);
			}
			
			LoadBundle(url, version, completeDel);
		}
		else
		{
			// Use the path relative to Resources
			LoadLocal(name, completeDel);
		}
	}

	public UnityEngine.Object Load(string name)
	{
		if (GameDefines.USE_ASSET_BUNDLE)
		{
			if (File.Exists(GameDefines.GetUnZipAssetBundlePath() + name+ ".assetbundle"))
			{
				if (!mResourceWWWs.ContainsKey(name))
				{
					AssetBundle baseBundle = AssetBundle.LoadFromFile(GameDefines.GetUnZipAssetBundlePath() + name+ ".assetbundle");
					if (baseBundle == null) return null; 
					
					mResourceWWWs[name] = baseBundle;
				}
				
				return mResourceWWWs[name].mainAsset;
				
				mResourceWWWs[name].Unload(false);		
			}
			else
			{
				return Resources.Load(name);
			}
		}
		else
		{
			return Resources.Load(name);
		}
		return null;
	}

	
	private void LoadLocal(string name, ResourceDelegate completeDel)
	{
		Object obj = Resources.Load(name);
		string error = string.Empty;
		
		// Check the name is a level name?
		// if (null == obj)
		// {
		// 	error = "Cann't find " + name + " in local.";
		// }
		
		if (null != completeDel)
			completeDel(obj, error);
	}
	
	private void LoadBundle(string url, int version, ResourceDelegate completeDel)
	{
		wwwLoaderMgr.Download(url, version, delegate(WWW www)
		{
			Object obj = null;
			if (string.IsNullOrEmpty(www.error))
			{
				Debug.Log("[LoadBundle:End:], the version is " + version);
				obj = www.assetBundle.mainAsset;
			}
			else
			{
				Debug.Log("[LoadBundle:Error:] " + www.error);
			}
			
			if (null != completeDel)
				completeDel(obj, www.error);
		});
	}
	
	private void InitPreloadResources()
	{
		// WWWForm form = new WWWForm();
		return;
		if (!GameDefines.IsUseStream())
		{
			_mPreloadLoaded = true;
			return;
		}
		
		// string url = GameDefines.GetUrlBase() + "201209120957_0/" + name + ".assetbundle";
		string name = BUND_VERSION;
		Load(name, delegate(Object obj, string error) 
		{
			if (string.IsNullOrEmpty(error))
			{
				ParseVersionData(obj);
				
				_mVersionLoaded = true;
				if (_mVersionLoaded && _mStrategyLoaded && _mLocalLevelLoaded)
					_mPreloadLoaded = true;
			}
			else
			{
				Debug.Log("[PreLoad-Error:] " + name);
			}
		});
		
		name = BUND_DOWNLOAD;
		Load(name, delegate(Object obj, string error) 
		{
			if (string.IsNullOrEmpty(error))
			{
				ParseDownloadStrategy(obj);
				
				_mStrategyLoaded = true;
				if (_mVersionLoaded && _mStrategyLoaded && _mLocalLevelLoaded)
					_mPreloadLoaded = true;
			}
			else
			{
				Debug.Log("[PreLoad-Error:] " + name);
			}
		});
		
		name = BUND_LOCAL_LEVEL;
		Load(name, delegate(Object obj, string error) 
		{
			if (string.IsNullOrEmpty(error))
			{
				ParseLocalLevel(obj);
				
				_mLocalLevelLoaded = true;
				if (_mVersionLoaded && _mStrategyLoaded && _mLocalLevelLoaded)
					_mPreloadLoaded = true;
			}
			else
			{
				Debug.Log("[PreLoad-Error:] " + name);
			}
		});
	}
	
	private string GetCachePath(string bundName)
    {
        if (bundName == BUND_VERSION || bundName == BUND_DOWNLOAD || bundName == BUND_LOCAL_LEVEL)
            return "";
		
        string ret = "";
        if(_mCachePath.TryGetValue(bundName, out ret))
            return ret.ToString() + "/";
		
        return "error999";
    }

    private int GetCacheID(string bundName)
    {
        // if (bundName == BUND_VERSION)
        //     return -1;
		
        int ret = -1;
        _mCacheID.TryGetValue(bundName, out ret);
		
        return ret;
    }
	
	private void ParseVersionData(Object obj)
	{
		TextAsset ta = obj as TextAsset;
        MemoryStream stream = new MemoryStream(ta.bytes);
        StreamReader sr = new StreamReader(stream);
        _mCacheID.Clear();
        _mCachePath.Clear();
        while (!sr.EndOfStream)
        {
            string s = sr.ReadLine();
            char[] spiltChar = new char[1] { '$' };
            string[] strings = s.Split(spiltChar);
            long verData = long.Parse(strings[2]);
            long baseData = long.Parse("201201010000");
            _mCacheID.Add(strings[0], (int)(verData - baseData));
            string path = strings[2] + "_" + strings[1];
            _mCachePath.Add(strings[0], path);
        }
	}
	
	void ParseDownloadStrategy(Object obj)
    {
	    TextAsset ta = obj as TextAsset;
	    MemoryStream stream = new MemoryStream(ta.bytes);
	    StreamReader sr = new StreamReader(stream);
	    while (!sr.EndOfStream)
	    {
			string bundName = sr.ReadLine();
			_mStrategyResList.Add(bundName);
	    }
	}
	
	void ParseLocalLevel(Object obj)
    {
        TextAsset ta = obj as TextAsset;
        MemoryStream stream = new MemoryStream(ta.bytes);
        StreamReader sr = new StreamReader(stream);
        while (!sr.EndOfStream)
        {
            string name = sr.ReadLine();
			_mLocalResList.Add(name, name);
        }
	}
	
	private WwwLoaderMgr wwwLoaderMgr;
	private bool _mVersionLoaded;
	private bool _mStrategyLoaded;
	private bool _mLocalLevelLoaded;
	private bool _mPreloadLoaded;
	
	// public Dictionary<string, AssetBundle> _mBundleList;
   	[HideInInspector] public Dictionary<string, int> _mCacheID;
   	[HideInInspector] public Dictionary<string, string> _mCachePath;
	
	//local level and gui level
	[HideInInspector] public Dictionary<string, string> _mLocalResList;
	[HideInInspector] public List<string> _mStrategyResList;
	[HideInInspector] public List<string> mLoadFinishedList;
	[HideInInspector] public List<string> mToLoadList;
	[HideInInspector] public List<string> _mDownLoadList;
	[HideInInspector] public const int DownLoadMaxNum = 100;
	private Dictionary<string, AssetBundle> mResourceWWWs = new Dictionary<string, AssetBundle>();
}
