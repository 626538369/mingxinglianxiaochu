using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Www loader manager is used to create or destroy www loader.
	/// It manages one loader table to avoid create the reduplicate loader.
	/// </summary>
	public class WwwLoaderManager
	{
		public static string BasePath
		{
			get { return _basePath; }
			set
			{
				_basePath = PathUtil.UnifyPath(value);
				Debug.LogFormat("Set base URL: {0}", _basePath);
			}
		}
		
		public static bool EnableCache
		{
			get { return _enableCache; } 
			set { _enableCache = value; } 
		}
		
		public static uint MaxRunLoaderNumber
		{
			get { return _maxRunLoaderNumber; } 
			set { _maxRunLoaderNumber = value; }
		}
		
		public static bool Paused 
		{
			get { return _paused; }
			set {_paused = value;}
		}
		
		/// <summary>
		/// Creates the loader.
		/// </summary>
		/// <returns>
		/// The loader.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		/// <param name='version'>
		/// Version.
		/// </param>
		/// <param name='useCaching'>
		/// Use caching.
		/// </param>
		public static WwwLoader CreateLoader(string path, int version, bool useCaching)
		{
			// Unify path.
			StringBuilder bd = new StringBuilder();
			bd.AppendFormat("{0}", PathUtil.CombinePath(BasePath, path));
			//bd.AppendFormat("{0}_{2}.{1}", PathUtil.CombinePath(BasePath, path), Defines.ASSET_BUNDLE_EXTENSION,version);
			path = PathUtil.UnifyPathNoLower(bd.ToString());
			
			// Check if the asset in the loader table
			if (_loaders.ContainsKey(path))
			{
				WwwLoader existingLoader = _loaders[path].Value;
				existingLoader.AddRef();
	
				return existingLoader;
			}
	
			// Check if system caching is enabled
			if (EnableCache == false || Caching.enabled == false)
			{
				useCaching = false;
			}
			
			// Create loader
			WwwLoader loader = new WwwLoader(path, version, useCaching);
			loader.AddRef();
	
			// Add to existing loader
			_loaders.Add(path, _pendingLoaders.AddLast(loader));
	
			return loader;
		}
		
		/// <summary>
		/// Destroies the loader.
		/// </summary>
		/// <param name='loader'>
		/// Loader.
		/// </param>
		public static void DestroyLoader(WwwLoader loader)
		{
			if (loader == null)
				return;
					
			loader.ReleaseRef();
	
			if (loader.RefCount == 0)
			{
				Debug.Assert(_loaders.ContainsKey(loader.AssetPath), "Must exist");
	
				LinkedListNode<WwwLoader> loaderNode = _loaders[loader.AssetPath];
				_loaders.Remove(loader.AssetPath);
				loaderNode.List.Remove(loaderNode);
				loaderNode.Value.Destroy();
			}
	
			loader = null;
		}
		
		/// <summary>
		/// Update www loader manager. This will drive all loaders.
		/// </summary>
		public static void Update()
		{
			LinkedListNode<WwwLoader> iter = _startedLoaders.First;	
			while (iter != null)
			{
				LinkedListNode<WwwLoader> nextIter = iter.Next;
	
				if (iter.Value.IsDone)
				{
					_startedLoaders.Remove(iter);
					_finishedLoaders.AddLast(iter);		
				}
	
				iter = nextIter;
			}
	
			if (!Paused)
			{
				iter = _pendingLoaders.First;
				
				while (_startedLoaders.Count < MaxRunLoaderNumber && iter != null)
				{
					LinkedListNode<WwwLoader> nextIter = iter.Next;
	
					iter.Value.Start();
					_pendingLoaders.Remove(iter);
					_startedLoaders.AddLast(iter);
	
					iter = nextIter;
				}
			}
		}
		
		protected static string _basePath;
		protected static bool _enableCache = true;
		protected static bool _paused;
		protected static uint _maxRunLoaderNumber = 10;
		
		protected static Dictionary<string, LinkedListNode<WwwLoader>> _loaders = new Dictionary<string, LinkedListNode<WwwLoader>>();
		protected static LinkedList<WwwLoader> _pendingLoaders = new LinkedList<WwwLoader>();
		protected static LinkedList<WwwLoader> _startedLoaders = new LinkedList<WwwLoader>();
		protected static LinkedList<WwwLoader> _finishedLoaders = new LinkedList<WwwLoader>();
	}
}
