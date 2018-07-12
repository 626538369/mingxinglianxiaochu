using UnityEngine;
using System.Collections;

namespace ResAssetBundle
{
	/// <summary>
	/// Builder config.
	/// </summary>
	public class BuilderConfig
	{
		public static string AssetBundleBasePath { get { return PathUtil.UnifyPath(BuilderHelper.CheckDirectoryExist("AssetBundle")); } }
		public static string WebPlayerAssetBundleBasePath { get { return PathUtil.UnifyPath(BuilderHelper.CheckDirectoryExist("WebPlayerAssetBundle")); } }
		
		public static string AssetBasePath { get { return PathUtil.CombinePath("Assets", "Artist"); } }
		public static string ManifestPath { get { return PathUtil.CombinePath(AssetBasePath, "Manifest.xml"); } }
		public static string TemporaryPath { get { return BuilderHelper.CheckDirectoryExist(PathUtil.CombinePath("Assets", "PackTemp")); } }
	}
}
