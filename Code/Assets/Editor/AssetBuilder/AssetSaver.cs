using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset saver class pack the inputed objects to one asset bundle.
	/// </summary>
	public class AssetSaver
	{
		/// <summary>
		/// Saves the object.
		/// </summary>
		/// <returns>
		/// The object.
		/// </returns>
		/// <param name='fileName'>
		/// If set to <c>true</c> file name.
		/// </param>
		/// <param name='asset'>
		/// If set to <c>true</c> asset.
		/// </param>
		/// <param name='mainAsset'>
		/// If set to <c>true</c> main asset.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static bool SaveObject<T>(string fileName, T asset, bool mainAsset) where T : Object
		{
			return SaveObject<T>(fileName, new T[] { asset }, new string[] { fileName }, mainAsset);
		}
		
		/// <summary>
		/// Saves the object.
		/// </summary>
		/// <returns>
		/// The object.
		/// </returns>
		/// <param name='fileName'>
		/// If set to <c>true</c> file name.
		/// </param>
		/// <param name='assets'>
		/// If set to <c>true</c> assets.
		/// </param>
		/// <param name='assetNames'>
		/// If set to <c>true</c> asset names.
		/// </param>
		/// <param name='mainAsset'>
		/// If set to <c>true</c> main asset.
		/// </param>
		/// <typeparam name='T'>
		/// The 1st type parameter.
		/// </typeparam>
		public static bool SaveObject<T>(string fileName, T[] assets, string[] assetNames, bool mainAsset) where T : Object
		{
			// Objects to be packed in the asset bundle
			List<Object> objsToPack = new List<Object>();
			List<string> namesToPack = new List<string>();
			
			// Objects need to be deleted after saving
			List<Object> objsToDestroy = new List<Object>();
			List<Object> objsToDelete = new List<Object>();
					
			// Go through all game objects.
			bool hasErr = false;
			
			for (int i = 0; i < assets.Length; ++i)
			{
				Object obj = AssetShrinker.ShrinkAsset(assetNames[i], assets[i], objsToDestroy, objsToDelete);
				objsToPack.Add(obj);
				namesToPack.Add(assetNames[i]);
			}
			
			// Save object
			if (!hasErr)
			{
				// Build asset bundle
				Debug.Log(namesToPack.ToArray()[0] + "@@@#" + fileName);
				hasErr = !BuildPipeline.BuildAssetBundleExplicitAssetNames(objsToPack.ToArray(), namesToPack.ToArray(), fileName, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android);
			}
	
			// Delete temp assets.
			foreach (Object obj in objsToDestroy)
			{
				Object.DestroyImmediate(obj);
			}
	
			foreach (Object obj in objsToDelete)
			{
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(obj));
			}
			
			if (hasErr)
			{
				Debug.LogErrorFormat("Create asset bundle failed: {0}", fileName);
			}
	
			return !hasErr;
		}
	}
}
