using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset loader manager class is used to create or destroy asset loaders.
	/// </summary>
	public class AssetLoaderManager
	{	
		/// <summary>
		/// Creates the loader with the specified package.
		/// </summary>
		/// <returns>
		/// The loader.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='packageFile'>
		/// Package file.
		/// </param>
		public static AssetLoader CreateLoader(string assetFile, string packageFile)
		{
			if (_assetLoaders.ContainsKey(assetFile))
				return _assetLoaders[assetFile];
			
			AssetLoader assetLoader = new AssetLoader(assetFile, packageFile);
			assetLoader.Load();
			_assetLoaders[assetFile] = assetLoader;
			
			return assetLoader;
		}
		
		/// <summary>
		/// Creates the loader with manifest. It will created dependent assets.
		/// </summary>
		/// <returns>
		/// The loader.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		public static AssetLoader CreateLoader(string assetFile, Manifest manifest)
		{
			if (_assetLoaders.ContainsKey(assetFile))
				return _assetLoaders[assetFile];
			
			AssetLoader assetLoader = new AssetLoader(assetFile, manifest);
			assetLoader.Load();
			_assetLoaders[assetFile] = assetLoader;
			
			return assetLoader;
		}
		
		/// <summary>
		/// Destroies the loader.
		/// </summary>
		/// <param name='assetLoader'>
		/// Asset loader.
		/// </param>
		public static void DestroyLoader(AssetLoader assetLoader)
		{
			if (assetLoader == null)
				return;
			
			_assetLoaders.Remove(assetLoader.AssetFile);
			assetLoader.Release();
		}
		
		/// <summary>
		/// Destroies all loaders.
		/// </summary>
		public static void DestroyAllLoaders()
		{
			foreach (AssetLoader assetLoader in _assetLoaders.Values)
			{
				assetLoader.Release();
			}
			
			_assetLoaders.Clear();
		}
		
		protected static Dictionary<string, AssetLoader> _assetLoaders = new Dictionary<string, AssetLoader>();
	}
}
