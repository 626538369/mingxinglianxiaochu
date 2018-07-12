using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset builder can pack assets to asset bundles.
	/// It can build all assets in the specified folders.
	/// Or you can selected some assets to build.
	/// </summary>
	public class AssetBuilder
	{
		/// <summary>
		/// Build all assets.
		/// </summary>
		[MenuItem("GfanTools/Build Assets/Build all assets")]
		public static void BuildAllAssets()
		{
			Manifest manifest = BuilderHelper.LoadManifest();
			
			List<string> packageFiles = manifest.GetAllPackageFiles();
			
			BuildPackage(packageFiles, manifest);
		}
		
		/// <summary>
		/// Builds the selected assets.
		/// </summary>
		[MenuItem("GfanTools/Build Assets/Build selected assets")]
		public static void BuildSelectedAssets()
		{
			Manifest manifest = BuilderHelper.LoadManifest();
			
			// Get selected assets.
			List<string> assetFiles = BuilderHelper.GetSelectedAssets();
			
			// Get packages.
			List<string> packageFiles = new List<string>();
			
			// Build packages.
			foreach (string assetFile in assetFiles)
			{
				string packageFile;
				int packageVersion;
				
				if (!manifest.GetAssetPackage(assetFile, out packageFile, out packageVersion))
				{
					Debug.LogWarningFormat("Failed to get package file: {0}", assetFile);
				}
				
				if (!packageFiles.Contains(packageFile))
					packageFiles.Add(packageFile);
			}
			
			BuildPackage(packageFiles, manifest);
		}
		
		/// <summary>
		/// Builds the package.
		/// </summary>
		/// <param name='packageFiles'>
		/// Package files.
		/// </param>
		/// <param name='manifest'>
		/// Manifest infomation.
		/// </param>
		protected static void BuildPackage(List<string> packageFiles, Manifest manifest)
		{
			// Clear temporary folder.
			Directory.Delete(BuilderConfig.TemporaryPath, true);
			
			// Build every package.
			foreach (string packageFile in packageFiles)
			{
				List<string> assetFiles = manifest.GetAllAssetFileInPackage(packageFile);
				List<string> packFiles = new List<string>();
				List<Object> packObjects = new List<Object>();
				
				int version = manifest.GetPackageVersion(packageFile);
				
				foreach (string assetFile in assetFiles)
				{
					string fromAsset;
					
					if (!manifest.GetFromAssetFlag(assetFile, out fromAsset))
					{
						Debug.LogErrorFormat("Failed to load asset flag: {0}", assetFile);
						continue;
					}
					
					// Get asset object.
					Object assetObject;
					if (fromAsset == string.Empty)
					{
						assetObject = AssetDatabase.LoadAssetAtPath(assetFile, typeof(Object));
					}
					else
					{
						assetObject = AssetDerivative.GetDerivative(assetFile, fromAsset, manifest);
					}
					
					if (assetObject == null)
					{
						Debug.LogErrorFormat("Failed to load asset: {0}", assetFile);
						continue;
					}
				
					packFiles.Add( assetFile );
					packObjects.Add( assetObject );
				}
				
				// Save to asset bundle.
				AssetSaver.SaveObject(BuilderHelper.GetAssetBundlePath(packageFile,version), packObjects.ToArray(), packFiles.ToArray(), true);
			}
			
			Debug.Log("Build package finished!");
		}
	}
}
