
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class LoadingInfoPublisher : EventManager.Publisher
{
    public override string Name
    {
        get { return "loading"; }
    }

    public void NotifyLoadingName(string s)
    {
        Notify("name", s);
    }

    public void NotifyPhase(int current, int total)
    {
        Notify("phase", current, total);
    }

    public void NotifyProgress(float prog)
    {
        Notify("progress", prog);
    }

    public void NotifyInitOk()
    {
        Notify("init_ok");
    }
}


public class BundleManager : MonoBehaviour
{

    [HideInInspector]
    public Dictionary<string, AssetBundle> m_mBundle;
    public Dictionary<string, int> m_mCacheID;
    public Dictionary<string, string> m_mCachePath;
	//local level and gui level
	public Dictionary<string, string> m_mLocalLevel;
	[HideInInspector] public List<string> m_resourceList;
	
    public string[] m_preLoadList;
    public List<string> m_charLoadList;
    public bool m_bIsLoadingCharacter;
    public enum LoadResultEnum { SUCC = 0, LOAD = 1, FAIL = 2 };
    public LoadResultEnum m_bIsDone;
    public bool m_CommonBundleReady;
    LoadingInfoPublisher m_loadingPub = new LoadingInfoPublisher();

    public delegate void LoaderCallback(Object obj);

    //const string
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

    void Awake()
    {
		Debug.Log("Application.persistentDataPath is " + Application.persistentDataPath);
		
        m_mBundle = new Dictionary<string, AssetBundle>();
        m_mCacheID = new Dictionary<string, int>();
        m_mCachePath = new Dictionary<string, string>();
		m_mLocalLevel = new Dictionary<string, string>();
		m_resourceList = new List<string>();
		
        m_bIsDone = LoadResultEnum.LOAD;
        m_preLoadList = null;
        m_charLoadList = new List<string>();
        m_bIsLoadingCharacter = false;
        m_CommonBundleReady = false;
    }

    // Use this for initialization
    void Start()
    {
        // StartCoroutine(DoInit());
    }

    IEnumerator DoInit()
    {
        //load version Data
        string[] versionData = new string[1] { BUND_VERSION };
        LoadBundles(versionData, false);
        while (true)
        {
            if (m_bIsDone == LoadResultEnum.SUCC)
            {
                ParseVersionData();
				//ParseDownloadData();
				
                break;
            }
            else if (m_bIsDone == LoadResultEnum.FAIL)
            {
                Debug.LogError("Fail to load version data");
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }
		
		//load download strategy Data
        string[] strategyData = new string[1] { BUND_DOWNLOAD };
        LoadBundles(strategyData, false);
        while (true)
        {
            if (m_bIsDone == LoadResultEnum.SUCC)
            {
				ParseDownloadStrategy();
                break;
            }
            else if (m_bIsDone == LoadResultEnum.FAIL)
            {
                Debug.LogError("Fail to download data");
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }
		
		//load local file Data
        string[] localData = new string[1] { BUND_LOCAL_LEVEL };
        LoadBundles(localData, false);
        while (true)
        {
            if (m_bIsDone == LoadResultEnum.SUCC)
            {
				ParseLocalLevel();
                break;
            }
            else if (m_bIsDone == LoadResultEnum.FAIL)
            {
                Debug.LogError("Fail to local setting data");
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }
		
        // //load preResource Data
        // string[] preResources = new string[] { BUND_UISHADER };
        // LoadBundles(preResources, false);
        // while (true)
        // {
        //     if (m_bIsDone == LoadResultEnum.SUCC)
        //     {
        //         break;
        //     }
        //     else if (m_bIsDone == LoadResultEnum.FAIL)
        //     {
        //         Debug.LogError("Fail to load preResources data");
        //         yield break;
        //     }
        //     yield return new WaitForSeconds(0.05f);
        // }
		// 
        // string[] pakages = new string[] { BUND_UITEXTURE };
        // LoadBundles(pakages, true);
        // while (true)
        // {
        //     if (m_bIsDone == LoadResultEnum.SUCC)
        //     {
        //         try
        //         {
        //             //Globals.Instance.InitData();
        //         }
        //         catch (System.Exception ex)
        //         {
        //             Debug.LogError(ex.ToString());
        //         }
		// 
        //         m_CommonBundleReady = true;
		// 
        //         break;
        //     }
        //     else if (m_bIsDone == LoadResultEnum.FAIL)
        //     {
        //         Debug.LogError("Fail to load start pakages");
        //         yield break;
        //     }
        //     yield return new WaitForSeconds(0.05f);
        // }

        m_loadingPub.NotifyInitOk();

    }

    void ParseVersionData()
    {
        if (GameDefines.IsUseStream())
        {
            AssetBundle ab = GetBundle(BUND_VERSION);
            TextAsset ta = ab.mainAsset as TextAsset;
            MemoryStream stream = new MemoryStream(ta.bytes);
            StreamReader sr = new StreamReader(stream);
            m_mCacheID.Clear();
            m_mCachePath.Clear();
            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                char[] spiltChar = new char[1] { '$' };
                string[] strings = s.Split(spiltChar);
                long verData = long.Parse(strings[2]);
                long baseData = long.Parse("201201010000");
                m_mCacheID.Add(strings[0], (int)(verData - baseData));
                string path = strings[2] + "_" + strings[1];
                m_mCachePath.Add(strings[0], path);
            }
        }
		
    }
	
	void ParseDownloadStrategy()
    {
        if (GameDefines.IsUseStream())
        {
			AssetBundle ab = GetBundle(BUND_DOWNLOAD);
            TextAsset ta = ab.mainAsset as TextAsset;
            MemoryStream stream = new MemoryStream(ta.bytes);
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
				string bundName = sr.ReadLine();
				m_resourceList.Add(bundName);
				
				/*
                int cacheID = GetCacheID(bundName);
				string url = GameDefines.GetUrlBase() + GetCachePath(bundName) + bundName + ".assetbundle";
				if (Caching.IsVersionCached (url, cacheID)) {
					Debug.Log("----url=" + url + " cached");
				} else {
				    WwwLoaderManager.CreateLoader(url, cacheID);
				}
				*/
            }
			
		}
	}
	
	void ParseLocalLevel()
    {
        if (GameDefines.IsUseStream())
        {
			AssetBundle ab = GetBundle(BUND_LOCAL_LEVEL);
            TextAsset ta = ab.mainAsset as TextAsset;
            MemoryStream stream = new MemoryStream(ta.bytes);
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                string sLocalLevel = sr.ReadLine();
				m_mLocalLevel.Add(sLocalLevel, sLocalLevel);
            }
		}
	}
	
    // Update is called once per frame
    void Update()
    {
		WwwLoaderManager.Update();
    }

    public LoadResultEnum GetIsDone()
    {
        return m_bIsDone;
    }

    public bool CheckBundleExist(string bundleName)
    {
        if (GameDefines.IsUseStream())
            return m_mBundle.ContainsKey(bundleName);
        else
            return true;

    }
	
	public bool CheckLocalLevelExist(string levelName)
    {
		string ret = "";
        return m_mLocalLevel.TryGetValue(levelName, out ret);
    }

    public Object LoadResource(string bundleName, string res, bool bIgnoreBundle, System.Type type)
    {
        if (GameDefines.IsUseStream())
        {
            Debug.Log("LoadResource:bundle:" + bundleName + "res:" + res);
            AssetBundle ab = GetBundle(bundleName);
            Object o = ab.LoadAsset(res, type);
            return o;
        }
        else
        {
            if (!bIgnoreBundle)
            {
                res = bundleName + "/" + res;
            }
            return Resources.Load(res);
        }
    }

    public T LoadResource<T>(string bundleName, string res, bool bIgnoreBundle) where T : class
    {
        return LoadResource(bundleName, res, bIgnoreBundle, typeof(T)) as T;
    }
	
	public Object Load(string resourceName)
	{
		Object res = Resources.Load(resourceName);
		if (res != null) 
		{
			return res;
		}
		else
		{
		    string bundleName = "";
		    char[] spiltChar = new char[1] { '/' };
		    if (resourceName.IndexOf("/") > 0)
		    {
			    string[] strings = resourceName.Split(spiltChar);
			    bundleName = strings[strings.Length - 1];
		    } 
		    else
		    {
			    bundleName = resourceName;
		    }
			
			List<string> resList = new List<string>();
            resList.Add(bundleName);
			LoadBundles(resList.ToArray(), true);
			AssetBundle ab = GetBundle(bundleName);
			Object o = ab.LoadAsset(bundleName);
            return o;
		}
		
	}

    public AssetBundle GetBundle(string bundName)
    {
        AssetBundle ab = null;
        m_mBundle.TryGetValue(bundName, out ab);
        return ab;
    }
	
    private string GetCachePath(string bundName)
    {
        if (bundName == BUND_VERSION || bundName == BUND_DOWNLOAD || bundName == BUND_LOCAL_LEVEL)
            return "";
        string ret = "";
        if(m_mCachePath.TryGetValue(bundName, out ret))
            return ret.ToString() + "/";
        return "error999";
    }

    private int GetCacheID(string bundName)
    {
        if (bundName == BUND_VERSION)
            return -1;
        int ret = -1;
        m_mCacheID.TryGetValue(bundName, out ret);
        return ret;
    }

    public bool LoadBundles(string[] nameLists, bool bUseCache = true)
    {
        if (m_preLoadList == null)
        {
            m_preLoadList = nameLists;
            m_bIsDone = LoadResultEnum.LOAD;
            StartCoroutine("DoLoadBundle", bUseCache);
            return true;
        }
        return false;
    }

    IEnumerator DoLoadBundle(bool bUseCache)
    {

        Debug.Log("Start Load Bundle...");
        bool bSuccess = true;

        int c = 1;
        foreach (string bundName in m_preLoadList)
        {
            m_loadingPub.NotifyLoadingName(bundName);
            m_loadingPub.NotifyPhase(c++, m_preLoadList.Length);
            if (GameDefines.IsUseStream())
            {
                string url = GameDefines.GetUrlBase() + GetCachePath(bundName) + bundName + ".assetbundle";
                Debug.Log("url:" + url);
                WWW www = null;


                int cacheID = GetCacheID(bundName);
                if (cacheID >= 0 && bUseCache)
                    www = WWW.LoadFromCacheOrDownload(url, cacheID);
                else
                    www = new WWW(url);
				
                while (!www.isDone)
                {
                    m_loadingPub.NotifyProgress(www.progress);
                    yield return 0;
                }
                if (www.error != null)
                {
                    m_loadingPub.NotifyProgress(1.0f);
                    Debug.Log("Error: " + bundName + " " + www.error);
                    bSuccess = false;
                    break;
                }
                else
                {
                    Debug.Log("OK: " + bundName + "" + www.assetBundle.ToString());
                    m_mBundle.Add(bundName, www.assetBundle);
                }
            }
            else
            {
                m_loadingPub.NotifyProgress(1.0f);
                Debug.Log("Downloading Local " + bundName.ToLower());
                yield return new WaitForSeconds(0.05f);
            }
        }

        Debug.Log("work flow end");
        m_preLoadList = null;
        if (bSuccess)
        {
            m_bIsDone = LoadResultEnum.SUCC;
        }
        else
        {
            m_bIsDone = LoadResultEnum.FAIL;
        }

        Debug.Log("Load Result is " + m_bIsDone);
    }

    public IEnumerator GetUILoader(string name, System.Action<string> callback)
    {
        if (GameDefines.IsUseStream())
        {
            string url = "";
            string bundle = "GUI_" + name;
            AssetBundle ab = null;
            m_loadingPub.NotifyLoadingName(bundle);
            m_loadingPub.NotifyPhase(1, 1);
            if (!m_mBundle.TryGetValue(bundle, out ab))
            {
                url = GameDefines.GetUrlBase() + GetCachePath(bundle) + bundle + ".assetbundle";
                Debug.Log("url:" + url);

                WWW www;
                int cacheID = GetCacheID(bundle);
                if (cacheID >= 0)
                    www = WWW.LoadFromCacheOrDownload(url, cacheID);
                else
                    www = new WWW(url);

                while (!www.isDone)
                {
                    m_loadingPub.NotifyProgress(www.progress);
                    yield return 0;
                }
                if (www.error != null)
                {
                    m_loadingPub.NotifyProgress(1.0f);
                    Debug.Log("Error: " + www.error);
                    yield break;
                }
                else
                {
                    Debug.Log("OK: " + bundle + "" + www.assetBundle.ToString());
                    m_mBundle.Add(bundle, www.assetBundle);
                    ab = www.assetBundle;
                }
            }

            m_loadingPub.NotifyProgress(1.0f);
            if (callback != null)
                callback(name);
        }
        else
        {
            AssetBundle ab = null;
            if (!m_mBundle.TryGetValue(name, out ab))
            {
                m_mBundle.Add(name, ab);
                yield return null;
            }
            if (callback != null)
                callback(name);
        }

    }


    public IEnumerator GetLoader(string bundName, string name, bool useMainAsset, LoaderCallback callback)
    {
        if (GameDefines.IsUseStream())
        {
            string url = "";
            string bundle = bundName;
            if (useMainAsset)
                bundle += "_" + name;
            AssetBundle ab = null;
            if (!m_mBundle.TryGetValue(bundle, out ab))
            {
                url = GameDefines.GetUrlBase() + GetCachePath(bundle) + bundle + ".assetbundle";
                Debug.Log("url:" + url);

                WWW www;
                int cacheID = GetCacheID(bundle);
                if (cacheID >= 0)
                    www = WWW.LoadFromCacheOrDownload(url, cacheID);
                else
                    www = new WWW(url);

                while (!www.isDone)
                {
                    yield return 0;
                }
                if (www.error != null)
                {
                    Debug.Log("Error: " + www.error);
                    if (callback != null)
                        callback(null);
                    yield break;
                }
                else
                {
                    Debug.Log("OK: " + bundle + "" + www.assetBundle.ToString());
                    m_mBundle.Add(bundle, www.assetBundle);
                    ab = www.assetBundle;
                }
            }
            if (useMainAsset)
            {
                if (callback != null)
                    callback(ab.mainAsset);
            }
            else
            {

                if (callback != null)
					callback(ab.LoadAsset(name));
            }
        }
        else
        {
            string res = bundName + "/" + name;
            Object o = Resources.Load(res);
            if (callback != null)
                callback(o);
            yield break;
        }
    }
	
	public void LoadAllAssetBundles()
	{
		foreach (string name in m_resourceList)
		{
			int cacheID = GetCacheID(name);
			string url = GameDefines.GetUrlBase() + GetCachePath(name) + name + ".assetbundle";
			if (Caching.IsVersionCached (url, cacheID)) 
			{
				Debug.Log("----url=" + url + " cached");
			} 
			else 
			{
			    LoadAssetBundle(name, null);
			}
		}
	}
	
	
	public void LoadAssetBundle(string name, LoaderCallback callback)
	{
		StartCoroutine( DoLoadAssetBundle(name, callback) );
	}
	
	public void UnloadAssetBundle(string name)
	{
		AssetBundle ab = null;
        if (m_mBundle.TryGetValue(name, out ab))
        {
			ab.Unload(true);
		}
	}
	
	IEnumerator DoLoadAssetBundle(string name, LoaderCallback callback)
	{
		if (GameDefines.IsUseStream())
        {
            AssetBundle ab = null;
            if (!m_mBundle.TryGetValue(name, out ab))
            {
				string url = GameDefines.GetUrlBase() +  name + ".assetbundle";
                Debug.Log("url:" + url);
				
				WWW www;
                //int cacheID = GetCacheID(name);
				int cacheID = GameDefines.AssetBundleVersion;
                if (cacheID >= 0)
                    www = WWW.LoadFromCacheOrDownload(url, cacheID);
                else
                    www = new WWW(url);
				
				while (!www.isDone)
                {
                    yield return 0;
                }
                if (www.error != null)
                {
                    Debug.Log("Error: " + name + " " + www.error);
                    if (callback != null)
                        callback(null);
                    yield break;
                }
                else
                {
                    Debug.Log("OK: " + name + "" + www.assetBundle.ToString());
					
					if (!m_mBundle.ContainsKey(name))
                    	m_mBundle.Add(name, www.assetBundle);
					
					if (callback != null)
					{
						ab = www.assetBundle;
						bool useMainAsset = true;
						if (useMainAsset)
                    		callback(ab.mainAsset);
						else
						{
							callback( ab.LoadAsset(name) );
						}
					}
					
                    yield break;
				}
			}
			else
			{
				if (callback != null)
				{
					bool useMainAsset = true;
					if (useMainAsset)
                		callback(ab.mainAsset);
					else
					{
						callback( ab.LoadAsset(name) );
					}
				}
               	yield break;
			}
		}
		else
        {
            Object o = Resources.Load(name);
            if (callback != null)
                callback(o);
            yield break;
        }
	}
}
