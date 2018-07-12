using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Mono.Xml;
using System.Text;
using System;

namespace ResAssetBundle
{
	/// <summary>
	/// Manifest builder class is used to build manifest information from assets.
	/// </summary>
	public class ManifestBuilder
	{
		[MenuItem("GfanTools/Build Assets/Build manifest")]
		public static void BuildAllAssets()
		{
			// Get all assets.
			List<string> assetsList = new List<string>();
			BuilderHelper.GetAssetsInFolder(BuilderConfig.AssetBasePath, true, assetsList);
			
			// Todo: Clear useless assets.
			
			// Extract new assets to manifest.
			ExtractAsset(assetsList);
		}
		
		[MenuItem("GfanTools/Build Assets/Build manifest for selected assets")]
		public static void BuildSelectedAssets()
		{
			// Get selected assets.
			List<string> assetsList = BuilderHelper.GetSelectedAssets();
			
			// Extract new assets to manifest.
			ExtractAsset(assetsList);
		}
		
		protected static void ExtractAsset(List<string> assetsList)
		{
			// Todo: Get version.
			Dictionary<string, int> assetsWithVersion = new Dictionary<string, int>();
			
			foreach (string asset in assetsList)
			{
				assetsWithVersion[asset] = Defines.DEF_RES_VERSION;
			}
			
			// Load current manifest.
			Manifest manifest = BuilderHelper.LoadManifest();
			
			// Add new asssets.
			foreach (string assetFile in assetsWithVersion.Keys)
			{
				string packageFile;
				int packageVersion;
				double createTime;
				
				double seconds = BuilderHelper.GetFileCreateTime(assetFile);
				
				// Add new asssets.
				if (!manifest.HasAsset(assetFile))
				{
					packageFile = manifest.CreateNewPackage(seconds);
					//it not use the version of asset
					manifest.AddAssetToPackage(assetFile, 0, "", packageFile);
				}
				// Update assets version.
				// TODO
				else
				{
					manifest.GetAssetPackage(assetFile, out packageFile, out packageVersion,out createTime);
					if(packageFile.Equals("00001"))
					{
						Debug.Log("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
						Debug.Log("createTime:"+createTime);
						Debug.Log("seconds:"+seconds);
					}
					if(createTime < seconds)
					{
						packageVersion += 1;
						manifest.UpdatePackageVersion(packageFile,packageVersion);
					}
					
				}
			}
			
			// Extract asset.
			foreach (string assetFile in assetsWithVersion.Keys)
			{
				AssetManifest.ExtractAsset(assetFile, manifest);
			}
			
			// Todo: Update package versions.
			
			// Update manifest file.
			using (StreamWriter outfile = new StreamWriter(PathUtil.CombinePath(Directory.GetCurrentDirectory(), BuilderConfig.ManifestPath), false))
	        {
	            outfile.Write(manifest.ToXMLString());
	        }
			
			AssetDatabase.Refresh();
			
			Debug.Log("Build manifest finished!");
		}
	}
}