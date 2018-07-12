using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security;

namespace ResAssetBundle
{
	/// <summary>
	/// Manifest is all assets' information table. We can get all package or asset information from it.
	/// It's the base class to build or load the assets.
	/// </summary>
	public class Manifest
	{
		/// <summary>
		/// Load the manifest from element.
		/// </summary>
		/// <param name='e'>
		/// E. the security element.
		/// </param>
		public void Load (SecurityElement e)
		{
			if (e.Tag != "Packages")
				return;
			
			if (e.Children != null) {
				foreach (SecurityElement c in e.Children) {
					Package package = Package.Load (c);
					
					if (package == null)
						continue;
					
					if (!_packageDict.ContainsKey (package.File))
						_packageDict [package.File] = package;
				}
			}
		}
		
		/// <summary>
		/// Tos the XML string.
		/// </summary>
		/// <returns>
		/// The XML string.
		/// </returns>
		public string ToXMLString ()
		{
			StringBuilder sb = new StringBuilder ();
			
			sb.AppendLine ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendLine ("<Packages>");
			
			foreach (Package package in _packageDict.Values) {
				sb.AppendLine (package.ToXMLString ());
			}
			
			sb.AppendLine ("</Packages>");
			
			return sb.ToString ();
		}
		
		/// <summary>
		/// Gets all package files.
		/// </summary>
		/// <returns>
		/// The all package files.
		/// </returns>
		public List<string> GetAllPackageFiles ()
		{
			return new List<string> (_packageDict.Keys);
		}
		
		public int GetPackageVersion(string package)
		{
			return _packageDict[package].Version;
		}
		
		/// <summary>
		/// Gets all asset file in package.
		/// </summary>
		/// <returns>
		/// The all asset file in package.
		/// </returns>
		/// <param name='packageFile'>
		/// Package file.
		/// </param>
		public List<string> GetAllAssetFileInPackage (string packageFile)
		{
			if (!_packageDict.ContainsKey (packageFile)) {
				return new List<string> ();
			}
			
			return _packageDict [packageFile].GetAllAssetFile ();
		}
		
		/// <summary>
		/// Gets from asset flag.
		/// </summary>
		/// <returns>
		/// The from asset flag.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='assetFlag'>
		/// If set to <c>true</c> asset flag.
		/// </param>
		public bool GetFromAssetFlag (string assetFile, out string assetFlag)
		{
			Asset asset = GetAsset (assetFile);
			assetFlag = "";
		
			if (asset == null)
				return false;
			
			assetFlag = asset.FromAsset;
			
			return true;
		}
		
		public bool GetAssetPackage(string assetFile, out string packageFile, out int packageVersion)
		{
			double createTime;
			return GetAssetPackage( assetFile, out  packageFile, out  packageVersion,out createTime);
		}
		
		/// <summary>
		/// Gets the asset package.
		/// </summary>
		/// <returns>
		/// The asset package.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='packageFile'>
		/// If set to <c>true</c> package file.
		/// </param>
		/// <param name='packageVersion'>
		/// If set to <c>true</c> package version.
		/// </param>
		public bool GetAssetPackage(string assetFile, out string packageFile, out int packageVersion,out double createTime)
		{
			packageFile = "";
			packageVersion = 0;
			createTime = 0;
			
			Package package = GetAssetPackage (assetFile);
			
			if (package == null)
				return false;
			
			packageFile = package.File;
			packageVersion = package.Version;
			createTime = package.CreateTime;
			
			return true;
		}
		
		/// <summary>
		/// Removes the package.
		/// </summary>
		/// <returns>
		/// The package.
		/// </returns>
		/// <param name='packageName'>
		/// If set to <c>true</c> package name.
		/// </param>
		public bool RemovePackage (string packageName)
		{
			if (!_packageDict.ContainsKey (packageName)) {
				return false;
			}
	
			_packageDict.Remove (packageName);
			
			return true;
		}
		
		/// <summary>
		/// Updates the package version.
		/// </summary>
		/// <returns>
		/// The package version.
		/// </returns>
		/// <param name='packageName'>
		/// If set to <c>true</c> package name.
		/// </param>
		/// <param name='version'>
		/// If set to <c>true</c> version.
		/// </param>
		public bool UpdatePackageVersion (string packageName, int version)
		{
			if (!_packageDict.ContainsKey (packageName) || version > _packageDict [packageName].Version) {
				_packageDict [packageName].Version = version;
			}
			
			return true;
		}
		
		/// <summary>
		/// Determines whether this instance has asset the specified assetFile.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has asset the specified assetFile; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		public bool HasAsset (string assetFile)
		{
			return GetAssetPackage (assetFile) != null;
		}
		
		/// <summary>
		/// Gets the asset depend material count.
		/// </summary>
		/// <returns>
		/// The asset depend material count.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		public int GetAssetDependMatCount (string assetFile)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return 0;
			
			return asset.GetDependMatCount ();
		}
		
		/// <summary>
		/// Gets the index of the asset depend material by index.
		/// </summary>
		/// <returns>
		/// The asset depend material by index.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='index'>
		/// If set to <c>true</c> index.
		/// </param>
		/// <param name='matAsset'>
		/// If set to <c>true</c> material asset.
		/// </param>
		/// <param name='matIndex'>
		/// If set to <c>true</c> material index.
		/// </param>
		/// <param name='matTransform'>
		/// If set to <c>true</c> material transform.
		/// </param>
		public bool GetAssetDependMatByIndex (string assetFile, int index, out string matAsset, out int matIndex, out string matTransform)
		{
			matAsset = "";
			matIndex = 0;
			matTransform = "";
			
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			DependentMaterial depMat = asset.GetDependMatByIndex (index);
			
			if (depMat == null)
				return false;
			
			matAsset = depMat.Asset;
			matIndex = depMat.MaterialIndex;
			matTransform = depMat.TransformPath;
			
			return true;
		}
		
		/// <summary>
		/// Gets the asset dependent texture count.
		/// </summary>
		/// <returns>
		/// The asset dependent texture count.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		public int GetAssetDependTexCount (string assetFile)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return 0;
			
			return asset.GetDependTexCount ();
		}
		
		/// <summary>
		/// Gets the index of the asset dependent texture by index.
		/// </summary>
		/// <returns>
		/// The asset dependent texture by index.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='index'>
		/// If set to <c>true</c> index.
		/// </param>
		/// <param name='texAsset'>
		/// If set to <c>true</c> texture asset.
		/// </param>
		/// <param name='texPropertyName'>
		/// If set to <c>true</c> texture property name.
		/// </param>
		public bool GetAssetDependTexByIndex (string assetFile, int index, out string texAsset, out string texPropertyName)
		{
			texAsset = "";
			texPropertyName = "";
			
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			DependentTexture depTex = asset.GetDependTexByIndex (index);
			
			if (depTex == null)
				return false;
			
			texAsset = depTex.Asset;
			texPropertyName = depTex.PropertyName;
			
			return true;
		}
		
		/// <summary>
		/// Gets the asset dependent animation count.
		/// </summary>
		/// <returns>
		/// The asset dependent animation count.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		public int GetAssetDependAnimCount (string assetFile)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return 0;
			
			return asset.GetDependAnimCount ();
		}
		
		/// <summary>
		/// Gets the index of the asset dependent animation by.
		/// </summary>
		/// <returns>
		/// The asset dependent animation by index.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='index'>
		/// If set to <c>true</c> index.
		/// </param>
		/// <param name='animAsset'>
		/// If set to <c>true</c> animation asset.
		/// </param>
		/// <param name='animName'>
		/// If set to <c>true</c> animation name.
		/// </param>
		/// <param name='animTransform'>
		/// If set to <c>true</c> animation transform.
		/// </param>
		public bool GetAssetDependAnimByIndex (string assetFile, int index, out string animAsset, out string animName, out string animTransform)
		{
			animAsset = "";
			animName = "";
			animTransform = "";
			
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			DependentAnimation depAnim = asset.GetDependAnimByIndex (index);
			
			if (depAnim == null)
				return false;
			
			animAsset = depAnim.Asset;
			animName = depAnim.AnimationName;
			animTransform = depAnim.TransformPath;
			
			return true;
		}
		
		/// <summary>
		/// Gets the asset dependent animation.
		/// </summary>
		/// <returns>
		/// The asset dependent animation.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='animAssetFile'>
		/// If set to <c>true</c> animation asset file.
		/// </param>
		/// <param name='animAsset'>
		/// If set to <c>true</c> animation asset.
		/// </param>
		/// <param name='animName'>
		/// If set to <c>true</c> animation name.
		/// </param>
		/// <param name='animTransform'>
		/// If set to <c>true</c> animation transform.
		/// </param>
		public bool GetAssetDependAnim (string assetFile, string animAssetFile, out string animAsset, out string animName, out string animTransform)
		{
			animAsset = "";
			animName = "";
			animTransform = "";
			
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			DependentAnimation depAnim = asset.GetDependAnimation (animAssetFile);
			
			if (depAnim == null)
				return false;
			
			animAsset = depAnim.Asset;
			animName = depAnim.AnimationName;
			animTransform = depAnim.TransformPath;
			
			return true;
		}
		
		/// <summary>
		/// Gets the type of the dependent.
		/// </summary>
		/// <returns>
		/// The dependent type.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='dependAssetFile'>
		/// If set to <c>true</c> depend asset file.
		/// </param>
		/// <param name='dependType'>
		/// If set to <c>true</c> depend type.
		/// </param>
		public bool GetDependentType(string assetFile, string dependAssetFile, out string dependType)
		{
			dependType = "";
			
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			if (asset.GetDependTexture(dependAssetFile) != null)
			{
				dependType = "DependentTexture";
			}
			else if(asset.GetDependAnimation(dependAssetFile) != null)
			{
				dependType = "DependentAnimation";
			}
			else if(asset.GetDependMaterial(dependAssetFile) != null)
			{
				dependType = "DependentMaterial";
			}
			else
			{
				dependType = "";
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// Create one package.
		/// </summary>
		/// <returns>
		/// The package.
		/// </returns>
		public string CreateNewPackage (double createTime)
		{
			int packMaxNumber = 0;
			
			foreach (Package package in _packageDict.Values) {
				int packNumber = StrParser.ParseDecInt (package.File, 0);
				
				if (packNumber > packMaxNumber)
					packMaxNumber = packNumber;
			}
			
			packMaxNumber ++;
			
			Package newPackage = new Package ();
			newPackage.File = packMaxNumber.ToString ("D5");
			newPackage.Version = 0;
			newPackage.CreateTime = createTime;
			
			_packageDict [newPackage.File] = newPackage;
			
			return newPackage.File;
		}
		
		/// <summary>
		/// Adds the asset to package.
		/// </summary>
		/// <returns>
		/// The asset to package.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='version'>
		/// If set to <c>true</c> version.
		/// </param>
		/// <param name='fromAsset'>
		/// If set to <c>true</c> from asset.
		/// </param>
		/// <param name='packageFile'>
		/// If set to <c>true</c> package file.
		/// </param>
		public bool AddAssetToPackage (string assetFile, int version, string fromAsset, string packageFile)
		{
			if (HasAsset (assetFile))
				return false;
			
			if (!_packageDict.ContainsKey (packageFile)) {
				return false;
			}
	
			return _packageDict [packageFile].AddAsset (assetFile, version, fromAsset);
		}
		
		/// <summary>
		/// Adds the dependent material to asset.
		/// </summary>
		/// <returns>
		/// The dependent material to asset.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='matAssetFile'>
		/// If set to <c>true</c> material asset file.
		/// </param>
		/// <param name='transformPath'>
		/// If set to <c>true</c> transform path.
		/// </param>
		/// <param name='matIndex'>
		/// If set to <c>true</c> material index.
		/// </param>
		public bool AddDependMatToAsset (string assetFile, string matAssetFile, string transformPath, int matIndex)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			return asset.AddDependMaterial (matAssetFile, transformPath, matIndex);
		}
		
		/// <summary>
		/// Adds the dependent texture to asset.
		/// </summary>
		/// <returns>
		/// The dependent texture to asset.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='texAssetFile'>
		/// If set to <c>true</c> texture asset file.
		/// </param>
		/// <param name='texPropertyName'>
		/// If set to <c>true</c> texture property name.
		/// </param>
		public bool AddDependTexToAsset (string assetFile, string texAssetFile, string texPropertyName)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			return asset.AddDependTexture (texAssetFile, texPropertyName);
		}
		
		/// <summary>
		/// Adds the dependent animation to asset.
		/// </summary>
		/// <returns>
		/// The dependent animation to asset.
		/// </returns>
		/// <param name='assetFile'>
		/// If set to <c>true</c> asset file.
		/// </param>
		/// <param name='animAssetFile'>
		/// If set to <c>true</c> animation asset file.
		/// </param>
		/// <param name='animName'>
		/// If set to <c>true</c> animation name.
		/// </param>
		/// <param name='transformPath'>
		/// If set to <c>true</c> transform path.
		/// </param>
		public bool AddDependAnimToAsset (string assetFile, string animAssetFile, string animName, string transformPath)
		{
			Asset asset = GetAsset (assetFile);
			
			if (asset == null)
				return false;
			
			return asset.AddDependAnimaion (animAssetFile, animName, transformPath);
		}
		
		protected Package GetAssetPackage (string assetFile)
		{
			foreach (Package package in _packageDict.Values) {
				if (package.HasAsset (assetFile)) {
					return package;
				}
			}
			
			return null;
		}
		
		protected Asset GetAsset (string assetFile)
		{
			Package package = GetAssetPackage (assetFile);
			
			if (package == null)
				return null;
			
			return package.GetAsset (assetFile);
		}
		
		protected class DependentTexture
		{
			public string Asset { get { return _asset; } set { _asset = value; } }
	
			public string PropertyName { get { return _propertyName; } set { _propertyName = value; } }
			
			public static DependentTexture Load (SecurityElement e)
			{
				if (e.Tag != "DependentTexture")
					return null;
				
				DependentTexture depTex = new DependentTexture ();
				
				depTex._asset = StrParser.ParseStr (e.Attribute ("Asset"), "");
				depTex._propertyName = StrParser.ParseStr (e.Attribute ("PropertyName"), "");
				
				return depTex;
			}
			
			public string ToXMLString ()
			{
				StringBuilder sb = new StringBuilder ();
				
				return sb.AppendFormat ("			<DependentTexture Asset=\"{0}\" PropertyName=\"{1}\"/>", _asset, _propertyName).ToString ();
			}
			
			public bool IsSame (DependentTexture depTex)
			{
				return Asset.Equals (depTex.Asset) && PropertyName.Equals (depTex.PropertyName);
			}
			
			protected string _asset;
			protected string _propertyName;
		}
		
		protected class DependentMaterial
		{
			public string Asset { get { return _asset; } set { _asset = value; } }
	
			public int MaterialIndex { get { return _materialIndex; } set { _materialIndex = value; } }
	
			public string TransformPath { get { return _transformPath; } set { _transformPath = value; } }
			
			public static DependentMaterial Load (SecurityElement e)
			{
				if (e.Tag != "DependentMaterial")
					return null;
				
				DependentMaterial depMat = new DependentMaterial ();
				
				depMat._asset = StrParser.ParseStr (e.Attribute ("Asset"), "");
				depMat._materialIndex = StrParser.ParseDecInt (e.Attribute ("MaterialIndex"), 0);
				depMat._transformPath = StrParser.ParseStr (e.Attribute ("TransformPath"), "");
				
				return depMat;
			}
			
			public string ToXMLString ()
			{
				StringBuilder sb = new StringBuilder ();
				
				return sb.AppendFormat ("			<DependentMaterial Asset=\"{0}\" MaterialIndex=\"{1}\" TransformPath=\"{2}\"/>", _asset, _materialIndex, _transformPath).ToString ();
			}
			
			public bool IsSame (DependentMaterial depMat)
			{
				return Asset.Equals (depMat.Asset) && MaterialIndex == depMat.MaterialIndex && TransformPath.Equals (depMat.TransformPath);
			}
			
			protected string _asset;
			protected int _materialIndex;
			protected string _transformPath;
		}
		
		protected class DependentAnimation
		{
			public string Asset { get { return _asset; } set { _asset = value; } }
	
			public string AnimationName { get { return _animationName; } set { _animationName = value; } }
	
			public string TransformPath { get { return _transformPath; } set { _transformPath = value; } }
			
			public static DependentAnimation Load (SecurityElement e)
			{
				if (e.Tag != "DependentAnimation")
					return null;
				
				DependentAnimation depAnim = new DependentAnimation ();
				
				depAnim._asset = StrParser.ParseStr (e.Attribute ("Asset"), "");
				depAnim._animationName = StrParser.ParseStr (e.Attribute ("AnimationName"), "");
				depAnim._transformPath = StrParser.ParseStr (e.Attribute ("TransformPath"), "");
				
				return depAnim;
			}
			
			public string ToXMLString ()
			{
				StringBuilder sb = new StringBuilder ();
				
				return sb.AppendFormat ("			<DependentAnimation Asset=\"{0}\" AnimationName=\"{1}\" TransformPath=\"{2}\"/>", _asset, _animationName, _transformPath).ToString ();
			}
			
			public bool IsSame (DependentAnimation depAnim)
			{
				return Asset.Equals (depAnim.Asset) && AnimationName.Equals (depAnim.AnimationName) && TransformPath.Equals (depAnim.TransformPath);
			}
			
			protected string _asset;
			protected string _animationName;
			protected string _transformPath;
		}
		
		protected class Asset
		{
			public string File { get { return _file; } set { _file = value; } }
	
			public int Version { get { return _version; } set { _version = value; } }
	
			public string FromAsset { get { return _fromAsset; } set { _fromAsset = value; } }
			
			public int GetDependTexCount ()
			{
				return _dependTexList.Count;
			}
			
			public DependentTexture GetDependTexByIndex (int index)
			{
				if (index < 0 || index >= _dependTexList.Count)
					return null;
				
				return _dependTexList [index];
			}
			
			public int GetDependMatCount ()
			{
				return _dependMatList.Count;
			}
			
			public DependentMaterial GetDependMatByIndex (int index)
			{
				if (index < 0 || index >= _dependMatList.Count)
					return null;
				
				return _dependMatList [index];
			}
			
			public int GetDependAnimCount ()
			{
				return _dependMatList.Count;
			}
			
			public DependentAnimation GetDependAnimByIndex (int index)
			{
				if (index < 0 || index >= _dependAnimList.Count)
					return null;
				
				return _dependAnimList [index];
			}
			
			public DependentAnimation GetDependAnimation (string animAssetFile)
			{
				foreach (DependentAnimation depAnim in _dependAnimList) {
					if (animAssetFile.Equals (depAnim.Asset))
						return depAnim;
				}
				
				return null;
			}
			
			public DependentTexture GetDependTexture (string texAssetFile)
			{
				foreach (DependentTexture depTex in _dependTexList) {
					if (texAssetFile.Equals (depTex.Asset))
						return depTex;
				}
				
				return null;
			}
			
			public DependentMaterial GetDependMaterial (string matAssetFile)
			{
				foreach (DependentMaterial depMat in _dependMatList) {
					if (matAssetFile.Equals (depMat.Asset))
						return depMat;
				}
				
				return null;
			}
			
			public bool AddDependMaterial (string matAssetFile, string transformPath, int matIndex)
			{
				DependentMaterial depMat = new DependentMaterial ();
				
				depMat.Asset = matAssetFile;
				depMat.TransformPath = transformPath;
				depMat.MaterialIndex = matIndex;
				
				if (!HasDependMaterial (depMat))
					_dependMatList.Add (depMat);
				
				return true;
			}
			
			public bool AddDependTexture (string texAssetFile, string texPropertyName)
			{
				DependentTexture depTex = new DependentTexture ();
				
				depTex.Asset = texAssetFile;
				depTex.PropertyName = texPropertyName;
				
				if (!HasDependTexture (depTex))
					_dependTexList.Add (depTex);
				
				return true;
			}
			
			public bool AddDependAnimaion (string animAssetFile, string animName, string transformPath)
			{
				DependentAnimation depAnim = new DependentAnimation ();
				
				depAnim.Asset = animAssetFile;
				depAnim.AnimationName = animName;
				depAnim.TransformPath = transformPath;
				
				if (!HasDependAnimation (depAnim))
					_dependAnimList.Add (depAnim);
				
				return true;
			}
			
			public bool HasDependMaterial (DependentMaterial dstDepMat)
			{
				foreach (DependentMaterial depMat in _dependMatList) {
					if (depMat.IsSame (dstDepMat))
						return true;
				}
				
				return false;
			}
			
			public bool HasDependAnimation (DependentAnimation dstDepAnim)
			{
				foreach (DependentAnimation depAnim in _dependAnimList) {
					if (depAnim.IsSame (dstDepAnim))
						return true;
				}
				
				return false;
			}
			
			public bool HasDependTexture (DependentTexture dstDepTex)
			{
				foreach (DependentTexture depTex in _dependTexList) {
					if (depTex.IsSame (dstDepTex))
						return true;
				}
				
				return false;
			}
			
			public static Asset Load (SecurityElement e)
			{
				if (e.Tag != "Asset")
					return null;
				
				Asset asset = new Asset ();
				
				asset._file = StrParser.ParseStr (e.Attribute ("File"), "");
				asset._version = StrParser.ParseDecInt (e.Attribute ("Version"), 0);
				asset._fromAsset = StrParser.ParseStr (e.Attribute ("FromAsset"), "");
				
				if (e.Children != null) {
					foreach (SecurityElement c in e.Children) {
						if (c.Tag == "DependentAnimation") {
							DependentAnimation depAnim = DependentAnimation.Load (c);
							
							if (depAnim != null && !asset.HasDependAnimation (depAnim)) {
								asset._dependAnimList.Add (depAnim);
							}
						} else if (c.Tag == "DependentMaterial") {
							DependentMaterial depMat = DependentMaterial.Load (c);
							
							if (depMat != null && !asset.HasDependMaterial (depMat)) {
								asset._dependMatList.Add (depMat);
							}
						} else if (c.Tag == "DependentTexture") {
							DependentTexture depTex = DependentTexture.Load (c);
							
							if (depTex != null && !asset.HasDependTexture (depTex)) {
								asset._dependTexList.Add (depTex);
							}
						}
					}
				}
				
				return asset;
			}
			
			public string ToXMLString ()
			{
				StringBuilder sb = new StringBuilder ();
				sb.AppendFormat ("		<Asset File=\"{0}\" Version=\"{1}\" FromAsset=\"{2}\">", _file, _version, _fromAsset);
				sb.AppendLine ();
				
				foreach (DependentTexture depTex in _dependTexList) {
					sb.AppendLine (depTex.ToXMLString ());
				}
				
				foreach (DependentMaterial depMat in _dependMatList) {
					sb.AppendLine (depMat.ToXMLString ());
				}
				
				foreach (DependentAnimation depAnim in _dependAnimList) {
					sb.AppendLine (depAnim.ToXMLString ());
				}
				
				sb.AppendLine ("		</Asset>");
				
				return sb.ToString ();
			}
			
			protected string _file;
			protected int _version;
			protected string _fromAsset;
			protected List<DependentTexture> _dependTexList = new List<DependentTexture> ();
			protected List<DependentMaterial> _dependMatList = new List<DependentMaterial> ();
			protected List<DependentAnimation> _dependAnimList = new List<DependentAnimation> ();
		}
		
		protected class Package
		{
			public string File { get { return _file; } set { _file = value; } }
			public double CreateTime { get { return _createTime; } set { _createTime = value; } }
			public int Version  { get { return _version; } set { _version = value; } }
			
			public bool HasAsset (string assetFile)
			{
				return _assetDict.ContainsKey (assetFile);
			}
			
			public bool AddAsset (string assetFile, int version, string fromAsset)
			{
				if (HasAsset (assetFile))
					return false;
				
				Asset asset = new Asset ();
				asset.File = assetFile;
				asset.Version = version;
				asset.FromAsset = fromAsset;
				
				_assetDict [asset.File] = asset;
				
				return true;
			}
			
			public Asset GetAsset (string assetFile)
			{
				if (_assetDict.ContainsKey (assetFile))
					return _assetDict [assetFile];
				
				return null;
			}
			
			public List<string> GetAllAssetFile ()
			{
				return new List<string> (_assetDict.Keys);
			}
			
			public static Package Load (SecurityElement e)
			{
				if (e.Tag != "Package")
					return null;
				
				Package package = new Package ();
				
				package._file = StrParser.ParseStr (e.Attribute ("File"), "");
				package._version = StrParser.ParseDecInt (e.Attribute ("Version"), 0);
				package._createTime = StrParser.ParseDouble(e.Attribute ("CreateTime"), 0);
				
				if (e.Children != null)
				{
					foreach (SecurityElement c in e.Children) {
						Asset asset = Asset.Load (c);
						
						if (asset == null)
							continue;
						
						if (!package._assetDict.ContainsKey (asset.File))
							package._assetDict [asset.File] = asset;
					}
				}
				
				return package;
			}
			
			public string ToXMLString ()
			{
				StringBuilder sb = new StringBuilder ();
				sb.AppendFormat ("	<Package File=\"{0}\" Version=\"{1}\" CreateTime=\"{2}\">", _file, _version,_createTime);
				sb.AppendLine ();
				
				foreach (Asset asset in _assetDict.Values) {
					sb.AppendLine (asset.ToXMLString ());
				}
				
				sb.AppendLine ("	</Package>");
				
				return sb.ToString ();
			}
			
			protected string _file;
			protected double _createTime;
			protected int _version;
			protected Dictionary<string, Asset> _assetDict = new Dictionary<string, Asset> ();
		}
		
		protected Dictionary<string, Package> _packageDict = new Dictionary<string, Package> ();
	}
}