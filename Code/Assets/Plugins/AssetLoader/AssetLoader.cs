using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset loader class is used to load asset form package.
	/// </summary>
	public class AssetLoader 
	{
		public string AssetFile
		{
			get { return _assetFile; }
		}
		
		/// <summary>
		/// Gets the error.
		/// </summary>
		/// <value>
		/// The error. If no error, will return null.
		/// </value>
		public string Error
		{
			get 
			{
				if (_wwwLoader == null)
					return null;
				
				if (_wwwLoader.Error != null)
					return _wwwLoader.Error;
				
				foreach (AssetLoader dependLoader in _dependLoaders.Values)
				{
					if (dependLoader.Error != null)
						return dependLoader.Error;
				}
				
				return null;
			}
		}
		
		public bool IsDone
		{
			get
			{
				bool flag = false;
				if(_wwwLoader.Progress<1.0f)
				{
					return flag = false;
				}
					
				
				foreach (AssetLoader dependLoader in _dependLoaders.Values)
				{
					if(dependLoader._wwwLoader.Progress<1.0f){
						return flag = false;
					}
				}
				
				return flag = true;
			}
		}
		
		/// <summary>
		/// Gets the progress.
		/// </summary>
		/// <value>
		/// The downloading progress.
		/// </value>
		public float Progress
		{
			get
			{
				if (_wwwLoader == null || _wwwLoader.Error != null)
					return 0.0f;
				
				float progress = _wwwLoader.Progress;
				
				foreach (AssetLoader dependLoader in _dependLoaders.Values)
				{
					progress += dependLoader.Progress;
				}
				
				return progress / ((float)_dependLoaders.Count + 1.0f );
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ResAssetBundle.AssetLoader"/> class.
		/// This asset loader will not check dependent assets.
		/// </summary>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='packageFile'>
		/// Package file.
		/// </param>
		public AssetLoader(string assetFile, string packageFile)
		{
			_assetFile = assetFile;
			_packageFile = packageFile;
			_packageVersion = 0;
			_useCache = false;
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="ResAssetBundle.AssetLoader"/> class.
		/// </summary>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		public AssetLoader(string assetFile, Manifest manifest)
		{
			_assetFile = assetFile;
			_manifest = manifest;
			_useCache = true;
			
			if (!_manifest.GetAssetPackage(assetFile, out _packageFile, out _packageVersion))
				return;
			
			// Check dependent loader.
			int dependMatCount = _manifest.GetAssetDependMatCount(assetFile);
			
			for (int index = 0; index < dependMatCount; index ++)
			{
				string matAsset;
				int matIndex;
				string matTransform;
				
				if (!_manifest.GetAssetDependMatByIndex(assetFile, index, out matAsset, out matIndex, out matTransform))
					continue;
				
				if (!_dependLoaders.ContainsKey(matAsset))
					_dependLoaders.Add(matAsset, new AssetLoader(matAsset, _manifest));
			}
			
			int dependAnimCount = _manifest.GetAssetDependAnimCount(assetFile);
			
			for (int index = 0; index < dependAnimCount; index ++)
			{
				string animAsset;
				string animName;
				string animTransform;
				
				if (!_manifest.GetAssetDependAnimByIndex(assetFile, index, out animAsset, out animName, out animTransform))
					continue;
				
				if (!_dependLoaders.ContainsKey(animAsset))
					_dependLoaders.Add(animAsset, new AssetLoader(animAsset, _manifest));
			}
			
			int dependTexCount = _manifest.GetAssetDependTexCount(assetFile);
			
			for (int index = 0; index < dependTexCount; index ++)
			{
				string texAsset;
				string texPropertyName;
				
				if (!_manifest.GetAssetDependTexByIndex(assetFile, index, out texAsset, out texPropertyName))
					continue;
					
				if (!_dependLoaders.ContainsKey(texAsset))
					_dependLoaders.Add(texAsset, new AssetLoader(texAsset, _manifest));
			}
		}
		
		/// <summary>
		/// Load this asset object. It will begin download the package first.
		/// </summary>
		public Object Load()
		{
			if (_assetObject != null)
				return _assetObject;
			
			if (_wwwLoader==null)
				CreateWWW();
			
			if (Progress<1.0f)
				return null;
			
			AssetBundle ab = _wwwLoader.ResAssetBundle;
			
			_assetObject = ab.LoadAsset(_assetFile);
			if (_manifest == null)
				return _assetObject;
			
			Dictionary<string, Object> dependObjects = new Dictionary<string, Object>();
			foreach (AssetLoader assetLoader in _dependLoaders.Values)
			{
				dependObjects[assetLoader.AssetFile] = assetLoader.Load();
			}
			
			_assetObject = AssetCreator.CreateAsset(_assetFile, _assetObject, dependObjects, _manifest, out _clonedObject);
			
			return _assetObject;
		}
		
		/// <summary>
		/// Release this asset loader.
		/// It will destroy the loaded object.
		/// </summary>
		public void Release()
		{
			if (_wwwLoader == null)
				return;
			
			ReleaseWWW();
			
			if (_clonedObject)
				Object.DestroyObject(_assetObject);
		}
		
		protected void CreateWWW()
		{
			if (_wwwLoader != null)
				return;
			_wwwLoader = WwwLoaderManager.CreateLoader(_packageFile, _packageVersion, _useCache);
			
			foreach (AssetLoader dependLoader in _dependLoaders.Values)
			{
				dependLoader.CreateWWW();
			}
		}
		
		protected void ReleaseWWW()
		{
			if (_wwwLoader == null)
				return;
			
			WwwLoaderManager.DestroyLoader(_wwwLoader);
			
			foreach (AssetLoader dependLoader in _dependLoaders.Values)
			{
				dependLoader.ReleaseWWW();
			}
		}
		
		protected string _assetFile;
		protected string _packageFile;
		protected int _packageVersion;
		protected bool _useCache;
		protected Object _assetObject;
		protected bool _clonedObject;
		protected Manifest _manifest;
		protected WwwLoader _wwwLoader;
		protected Dictionary<string, AssetLoader> _dependLoaders = new Dictionary<string, AssetLoader>();
	}
}