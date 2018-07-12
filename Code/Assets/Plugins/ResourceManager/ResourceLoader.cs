using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResAssetBundle;

/// <summary>
/// Resource loader class is used to load some assets through asset loader.
/// It provides downloading progress infromation for outside through callback.
/// </summary>
public class ResourceLoader : MonoBehaviour
{
	/// <summary>
	/// Sets the progress delegate.
	/// </summary>
	/// <param name='prgDlgt'>
	/// Prg dlgt.
	/// </param>
	/// <param name='userData'>
	/// User data.
	/// </param>
	public void SetProgressDelegate (DownloadDelegate prgDlgt, System.Object userData,bool isReturnPerFrame)
	{
		_progressDelegate = prgDlgt;
		_prgDlgtUserData = userData;
		_isReturnPerFrame = isReturnPerFrame;
	}
	
	public AssetLoader GetAssetLoader(string assetFile)
	{
		return _assetLoaders [assetFile];
	}
	
	/// <summary>
	/// Adds the asset loader.
	/// </summary>
	/// <param name='assetLoader'>
	/// Asset loader.
	/// </param>
	public void AddAssetLoader (AssetLoader assetLoader)
	{
		if (_assetLoaders.ContainsKey (assetLoader.AssetFile))
			return;
		
		_assetLoaders [assetLoader.AssetFile] = assetLoader;
	}
	
	protected void Start ()
	{
	}

	protected void Update ()
	{
		if (Error () != null) {
			Debug.LogError (Error ());
			GameObject.Destroy (this);
			return;
		}
		
		float progress = Progress ();
		
		if (_isReturnPerFrame) {
			if (_progressDelegate != null && progress < 1.0f) {
				try {
					_progressDelegate (progress, _prgDlgtUserData);
				} catch (System.Exception e) {
					Debug.LogError (e.StackTrace);
				}
			}
		}
		
		if (progress >= 1.0f && isDone ()) {
			if(!_isCoroutineRun)
			{
				StartCoroutine (InvokeDelegate (progress));
			}
			
			
			/*
			_progressDelegate (progress, _prgDlgtUserData);
			foreach (AssetLoader assetLoader in _assetLoaders.Values) {
				AssetLoaderManager.DestroyLoader (assetLoader);
			}
			//GameObject.Destroy (this);
			GameObject.Destroy (this);
			*/
		}
	}
	
	
	IEnumerator InvokeDelegate (float progress)
	{
		_isCoroutineRun = true;
		yield return new WaitForSeconds(0.1f);
		_progressDelegate (progress, _prgDlgtUserData);
		
		yield return new WaitForSeconds(0.1f);
		foreach (AssetLoader assetLoader in _assetLoaders.Values) {
			AssetLoaderManager.DestroyLoader (assetLoader);
		}
		GameObject.Destroy (this);
		
	}
	
	
	protected string Error ()
	{
		/*
		if (_assetLoaders.Count == 0)
			return null;
		
		foreach (AssetLoader assetLoader in _assetLoaders.Values)
		{
			if (assetLoader.Error != null)
				return assetLoader.Error;
		}
		*/	
		return null;
	}
	
	public float Progress ()
	{
		if (_assetLoaders.Count == 0)
			return 0.0f;
		
		float progress = 0.0f;
		
		foreach (AssetLoader assetLoader in _assetLoaders.Values) {
			progress += assetLoader.Progress;
		}
		
		return progress / (float)_assetLoaders.Count;
	}
	
	public int Count {
		get{ return _assetLoaders.Count;}
	}
	
	public bool isDone ()
	{
		foreach (AssetLoader assetLoader in _assetLoaders.Values) {
			
			if(!assetLoader.IsDone)
				return false;
			
		}
		return true;
		
	}
	
	protected bool _isCoroutineRun = false;
	
	protected bool _isReturnPerFrame;
	protected DownloadDelegate _progressDelegate;
	protected System.Object _prgDlgtUserData;
	protected Dictionary<string, AssetLoader> _assetLoaders = new Dictionary<string, AssetLoader> ();
}
