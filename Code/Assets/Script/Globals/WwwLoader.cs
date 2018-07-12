using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/// <summary>
	/// Www loader class is a wrapper to www class.
	/// </summary>
public class WwwLoader : RefCountObject
{
	public AssetBundle ResAssetBundle { get { return _www != null ? _www.assetBundle : null; } }

	public string AssetPath { get { return _url; } }

	public bool Started { get { return _www != null; } }

	public bool Success	{ get { return IsDone ? _www.assetBundle != null : false; } }

	public float Progress { get { return _www == null ? 0.0f : _www.progress; } }

	public string Error { get { return _www == null ? null : _www.error; } }

	public bool IsDone { get { return _www == null ? false : _www.isDone; } }
	
	protected string _url;
	
    protected int _version;
	
	protected WWW _www;
	
	public WwwLoader (string url, int version)
	{
		_url = url;
		_version = version;
	}
		
	/// <summary>
		/// Start this instance.
		/// </summary>
	public void Start ()
	{
		if (_www == null) {
			Debug.Log("download start");
			_www = WWW.LoadFromCacheOrDownload (_url, _version);
			Debug.Log("download end");
			if(_www == null)
			{
				Debug.Log("DownLoad Asset Is Not Exits");
			}
		}
	}
		
	/// <summary>
		/// Destroy this instance.
		/// </summary>
	public void Destroy ()
	{
		if (_www != null && _www.isDone && (_www.error == null || _www.error == "")) {
			_www.assetBundle.Unload (false);
		}
	}
		

}
