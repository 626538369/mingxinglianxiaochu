using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Xml;
using System;

namespace ResAssetBundle
{
	/// <summary>
	/// Builder helper class provide some build functions.
	/// </summary>
	public class BuilderHelper
	{
		/// <summary>
		/// Loads the manifest.
		/// </summary>
		/// <returns>
		/// The manifest.
		/// </returns>
		public static Manifest LoadManifest ()
		{
			Manifest manifest = new Manifest ();
			
			TextAsset manifestText = AssetDatabase.LoadAssetAtPath (BuilderConfig.ManifestPath, typeof(TextAsset)) as TextAsset;
			SecurityParser xmlParser = new SecurityParser ();
			xmlParser.LoadXml (manifestText.text);
			
			manifest.Load (xmlParser.ToXml ());
			
			return manifest;
		}
		
		/// <summary>
		/// Gets the selected assets.
		/// </summary>
		/// <returns>
		/// The selected assets.
		/// </returns>
		public static List<string> GetSelectedAssets ()
		{
			List<string> assetsList = new List<string> ();
			
			if (Selection.objects.Length == 0)
				return assetsList;
			
			foreach (UnityEngine.Object obj in Selection.objects) {
				string assetFile = PathUtil.UnifyPath (AssetDatabase.GetAssetPath (obj));
				
				if (!assetFile.StartsWith (BuilderConfig.AssetBasePath))
					continue;
				
				string assetFullPath = PathUtil.CombinePath (Directory.GetCurrentDirectory (), assetFile);
				
				if (Directory.Exists (assetFullPath)) {
					GetAssetsInFolder (assetFile, true, assetsList);
				} else {
					assetsList.Add (assetFile);
				}
			}
			
			return assetsList;
		}
		
		/// <summary>
		/// Gets the assets in folder.
		/// </summary>
		/// <param name='root'>
		/// Root folder.
		/// </param>
		/// <param name='includeSubFolder'>
		/// Include sub folder.
		/// </param>
		/// <param name='assetsList'>
		/// Assets list.
		/// </param>
		public static void GetAssetsInFolder (string root, bool includeSubFolder, List<string> assetsList)
		{
			// Skip hidden directory.
			DirectoryInfo dirInfo = new DirectoryInfo (root);
			if ((dirInfo.Attributes & FileAttributes.Hidden) != 0) {
				return;
			}
	
			// Process all files in the directory.
			FileInfo[] fileInfos = dirInfo.GetFiles ();
			foreach (FileInfo fileInfo in fileInfos) {
				// Skip unity version file(*.meta).
				if (string.Compare (fileInfo.Extension, ".meta", true) == 0) {
					continue;
				}
				if (string.Compare (fileInfo.Extension, ".unity", true) == 0) {
					continue;
				}
				if (string.Compare (fileInfo.Extension, ".fbx", true) == 0) {
					continue;
				}
				
				//store the str from project root path("client") to file name but no extension
				assetsList.Add (PathUtil.GetPathInFolder (fileInfo.FullName, Directory.GetCurrentDirectory (), true));
			}
			
			if (!includeSubFolder)
				return;
	
			// Process all sub directories.
			DirectoryInfo[] subDirInfos = dirInfo.GetDirectories ();
			
			foreach (DirectoryInfo subDirInfo in subDirInfos) {
				GetAssetsInFolder (PathUtil.CombinePath (root, subDirInfo.Name), includeSubFolder, assetsList);
			}
		}
		
		/// <summary>
		/// Given a path name and make sure it exist on disk.
		/// </summary>
		/// <param name="path">Path to check</param>
		/// <returns>Return the path pass in</returns>
		public static string CheckDirectoryExist (string path)
		{
			if (!Directory.Exists (path)) {
				Directory.CreateDirectory (path);
			}
			
			return path;
		}
		
		/// <summary>
		/// Gets the asset bundle path.
		/// </summary>
		/// <returns>
		/// The asset bundle path.
		/// </returns>
		/// <param name='assetPath'>
		/// Asset path.
		/// </param>
		public static string GetAssetBundlePath (string assetPath, int version)
		{
			StringBuilder bd = new StringBuilder ();
			bd.AppendFormat ("{1}{2}.{0}", Defines.ASSET_BUNDLE_EXTENSION, PathUtil.CombinePath (BuilderConfig.AssetBundleBasePath, assetPath), "_" + version);
			string assetBundlePath = bd.ToString ();
	
			// Create directory
			CheckDirectoryExist (Path.GetDirectoryName (assetBundlePath));
	
			return assetBundlePath;
		}
		
		/// <summary>
		/// Gets the temporary file path.
		/// </summary>
		/// <returns>
		/// The temporary file path.
		/// </returns>
		/// <param name='name'>
		/// Name.
		/// </param>
		public static string GetTemporaryFilePath (string name)
		{
			string path = PathUtil.CombinePath (BuilderConfig.TemporaryPath, name);
			
			CheckDirectoryExist (Path.GetDirectoryName (path));
			
			return path;
		}
		
		public static double GetFileCreateTime (string filename)
		{
			//get file create time;
			string rootPath = Directory.GetCurrentDirectory ();
			DateTime stander = new DateTime (1970, 1, 1);
			string rootFilePath = rootPath + "/" + filename;
			FileInfo fileInfo = new FileInfo (rootFilePath);
			if (fileInfo.Exists) {
				DateTime dt = fileInfo.LastWriteTime;
				TimeSpan ts = dt - stander;
				Debug.Log (dt.ToString ());
				double seconds = ts.TotalMilliseconds;
				return seconds;
			}
			return 0;
		}
	}
}
