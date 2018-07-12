using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Www loader class is a wrapper to www class.
	/// </summary>
	public class WwwLoader : RefCountObject
	{
		public AssetBundle ResAssetBundle 
		{ 
			get { 
				return _www != null ? _www.assetBundle : null; 
			} 
		}

		public string AssetPath { get { return _path; } }

		public bool Started { get { return _www != null; } }

		public bool Success	{ get { return IsDone ? _www.assetBundle != null : false; } }

		public float Progress { get { return _www == null ? 0.0f : _www.progress; } }

		public string Error { get { return _www == null ? null : _www.error; } }

		public bool IsDone { get { return _www == null ? false : _www.isDone; } }
		
		public WwwLoader (string path, int version, bool useCaching)
		{
			_path = path;
			_version = version;
			_useCache = useCaching;
		}
		
		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Start ()
		{
			if (_www == null) {
				StringBuilder url = new StringBuilder();
				url.AppendFormat("{0}_{2}.{1}", _path, Defines.ASSET_BUNDLE_EXTENSION,_version);
				if (_useCache) {
					Debug.Log("DownLoad Asset@"+ "Name:"+url.ToString()+" Version:" + _version);
					if (Caching.IsVersionCached (url.ToString(), _version)) {
						Debug.Log("The Asset Has Be Caching!");
					}
					_www = WWW.LoadFromCacheOrDownload (url.ToString(), _version);
					if (_version != 0) {
						_version -= 1;
						url = new StringBuilder();
						url.AppendFormat("{0}_{2}.{1}", _path, Defines.ASSET_BUNDLE_EXTENSION,_version);
						if (Caching.IsVersionCached (url.ToString(), _version)) {
							Debug.Log("The Asset of Last Version Has Be Caching@"+"Version:" + _version);
							WWW www = WWW.LoadFromCacheOrDownload (url.ToString(), _version);
							www.assetBundle.Unload (true);
						}
					}
				} else {
					Debug.Log("DownLoad Asset Unuse Cache");
					_www = new WWW (url.ToString());
				}
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
		
		protected string _path;
		protected int _version;
		protected bool _useCache;
		protected WWW _www;
	}
}
