using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WwwLoaderPublisher : EventManager.Publisher
{
	public const string BEGIN = "name";
	public const string PHASE = "phase";
	public const string PROGRESS = "progress";
	public const string COMPLETE = "complete";
	
    public override string Name
    {
        get { return "WwwLoader"; }
    }

    public void NotifyLoadingName(string url)
    {
        Notify(BEGIN, url);
		
		Debug.Log("[WWW-Publisher-Begin:] " + url);
    }

    public void NotifyPhase(int current, int total)
    {
        Notify(PHASE, current, total);
    }

    public void NotifyProgress(string url, float prog)
    {
        Notify(PROGRESS, url, prog);
		Debug.Log("[WWW-Publisher-Progress:] " + prog + " " + url);
    }
	
	public void NotifyComplete(string url, string error)
	{
        Notify(COMPLETE, url, error);
		if (string.IsNullOrEmpty(error))
			Debug.Log("[WWW-Publisher-Complete:] " + url);
		else
		{
			Debug.Log("[WWW-Publisher-Error:] " + url + " " + error);
		}
	}
	
    public void NotifyInitOk()
    {
        Notify("init_ok");
    }
}

public class WwwLoaderMgr : MonoBehaviour 
{
	// ------------------------------------------------------
	public delegate void WwwLoaderDelegate(WWW www);
	// ------------------------------------------------------
	
	// ------------------------------------------------------
	public class DownloadTask
	{
		public string url;
		public int cacheVersion;
		public WWW www;
		public event WwwLoaderDelegate callback;
		
		public void DoEventDelegate()
		{
			if (null != callback)
				callback(www);
		}
	}
	// ------------------------------------------------------
	
	[HideInInspector] public const int DownLoadMaxNum = 100;
	
	void Awake()
	{
		_mPubliser = new WwwLoaderPublisher();
		
		_mRequestList = new Dictionary<string, DownloadTask>();
		_mDownLoadList = new Dictionary<string, DownloadTask>();
		_mLoadedList = new Dictionary<string, DownloadTask>();
	}
	
	// Use this for initialization
	void Start () 
	{
	}
	
	void OnDestroy()
	{
		_mRequestList.Clear();
		_mDownLoadList.Clear();
		_mLoadedList.Clear();
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (DownloadTask task in _mRequestList.Values)
		{
			if (_mDownLoadList.Count < DownLoadMaxNum)
			{
				StartCoroutine( DoDownload(task) );
				break;
			}
		}
	}
	
	public void Download(string url, int cacheVersion, WwwLoaderDelegate complete)
	{
		DownloadTask task = null;
		// In loaded list
		if ( _mLoadedList.TryGetValue(url, out task) )
		{
			complete(task.www);
			return;
		}
		// In download list
		else if (_mDownLoadList.TryGetValue(url, out task))
		{
			task.callback += complete;
			return;
		}
		// In request list
		else if (_mRequestList.TryGetValue(url, out task))
		{
			task.callback += complete;
			return; 
		}
		else
		{
			task = new DownloadTask();
			task.url = url;
			task.cacheVersion = cacheVersion;
			task.callback += complete;
			
			_mRequestList.Add(url, task);
		}
	}
	
	IEnumerator DoDownload(DownloadTask task)
	{
		string url = task.url;
		
		Debug.Log("[WWW-Download:] " + url);
		_mPubliser.NotifyLoadingName(url);
		_mRequestList.Remove(url);
		
		int cacheVersion = task.cacheVersion;
		if (cacheVersion >= 0)
			task.www = WWW.LoadFromCacheOrDownload(url, cacheVersion);
		else
			task.www = new WWW(url);
		
		_mDownLoadList.Add(url, task);
		
		if (task.www.isDone)
		{
			Debug.Log("[WWW-Load from cache:] " + url);
			_mLoadedList.Add(url, task);
		}
		else
		{
			while (!task.www.isDone)
			{
				Debug.Log("[WWW-Loading:] " + url + " " + task.www.progress);
				_mPubliser.NotifyProgress(url, task.www.progress);
				yield return 0;
			}
			
			if (!string.IsNullOrEmpty(task.www.error))
			{
				Debug.Log("[WWW-Error:] " + task.www.error);
			}
			else
			{
				Debug.Log("[WWW-Loaded:] " + url);
				_mLoadedList.Add(url, task);
			}
		}
		
		_mPubliser.NotifyComplete(url, task.www.error);
		_mDownLoadList.Remove(url);
		
		// Call DoSomething
		task.DoEventDelegate();
		
		yield break;
	}
	
	protected Dictionary<string, DownloadTask> _mRequestList;
	protected Dictionary<string, DownloadTask> _mDownLoadList;
	protected Dictionary<string, DownloadTask> _mLoadedList;
	
	protected List<string> _mTmpRemoveList;
	
	protected WwwLoaderPublisher _mPubliser;
}
