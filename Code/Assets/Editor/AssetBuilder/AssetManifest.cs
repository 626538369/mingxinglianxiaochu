using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset manifest class is used to extract the asset package information.
	/// </summary>
	public class AssetManifest
	{
		/// <summary>
		/// Extracts the asset.
		/// </summary>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		public static void ExtractAsset(string assetFile, Manifest manifest)
		{
			Initialize();
			
			_processor.Process(new ProcessData(assetFile, manifest));
		}
		
		protected static void Initialize()
		{
			if (_initialized)
				return;
			
			_initialized = true;
			
			_processor.Register("GameObject", ExtractGameObject);
			_processor.Register("Material", ExtractMaterial);
		}
		
		/// <summary>
		/// Extracts the game object.
		/// </summary>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='assetObject'>
		/// Asset object.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		protected static Object ExtractGameObject(ProcessData processData)
		{
			Object assetObject = AssetDatabase.LoadAssetAtPath(processData.AssetFile, typeof(Object));
			GameObject assetGameObject = assetObject as GameObject;
					
			Renderer []renderers = assetGameObject.GetComponentsInChildren<Renderer>(true);
			
			foreach (Renderer renderer in renderers)
			{
				for (int matIndex = 0; matIndex < renderer.sharedMaterials.Length; matIndex ++)
				{
					Material material = renderer.sharedMaterials[matIndex];
					string transformPath = ObjectUtil.GetTransRootPath(renderer.transform);
					string matAssetFile = PathUtil.UnifyPath(AssetDatabase.GetAssetPath(material));
					bool b = processData.ManifestInst.HasAsset(matAssetFile);
					Debug.Log(b);
					if (processData.ManifestInst.HasAsset(matAssetFile))
					{
						// Add dependent material.
						processData.ManifestInst.AddDependMatToAsset(processData.AssetFile, matAssetFile, transformPath, matIndex);
					}
				}
			}
			
			Animation []animations = assetGameObject.GetComponentsInChildren<Animation>(true);
			
			foreach (Animation animation in animations)
			{
				string transformPath = ObjectUtil.GetTransRootPath(animation.transform);
				
				// Build each animation clip.
				foreach (AnimationState aniState in animation)
				{
					if (aniState.clip != null )
					{
						string aniAssetFile = PathUtil.UnifyPath(AssetDatabase.GetAssetPath(aniState.clip));
						
						// It's a binding animation.
						if (aniAssetFile.Equals(processData.AssetFile))
						{
							StringBuilder bd = new StringBuilder(Path.GetDirectoryName(processData.AssetFile));
							bd.AppendFormat("/{0} {1}", Path.GetFileNameWithoutExtension(processData.AssetFile), aniState.name);
							
							aniAssetFile = bd.ToString();
							
							if (!processData.ManifestInst.HasAsset(aniAssetFile))
							{
								double seconds = BuilderHelper.GetFileCreateTime(aniAssetFile);
								string packageFile = processData.ManifestInst.CreateNewPackage(seconds);
								processData.ManifestInst.AddAssetToPackage(aniAssetFile, 0, processData.AssetFile, packageFile);
							}
						}
						
						if (processData.ManifestInst.HasAsset(aniAssetFile))
						{
							// Add dependent animation.
							processData.ManifestInst.AddDependAnimToAsset(processData.AssetFile, aniAssetFile, aniState.name, transformPath );
						}
					}
				}
			}
			return null;
		}
		
		/// <summary>
		/// Extracts the material.
		/// </summary>
		/// <returns>
		/// The material.
		/// </returns>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		protected static Object ExtractMaterial(ProcessData processData)
		{
			Object assetObject = AssetDatabase.LoadAssetAtPath(processData.AssetFile, typeof(Object));
			Material material = assetObject as Material;
			
			foreach(string propertyName in Defines.MAT_TEX_PROPERTY_NAMES)
			{
				if (material.HasProperty(propertyName))
				{
					Texture texture = material.GetTexture(propertyName);
					
					if (texture != null)
					{
						string texAssetFile = PathUtil.UnifyPath(AssetDatabase.GetAssetPath(texture));
						
						if (processData.ManifestInst.HasAsset(texAssetFile))
						{
							// Add dependent texture.
							processData.ManifestInst.AddDependTexToAsset(processData.AssetFile, texAssetFile, propertyName);
						}
					}
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Process data.
		/// </summary>
		protected class ProcessData : IProcessData
		{
			public ProcessData(string assetFile, Manifest manifest)
			{
				_assetFile = assetFile;
				_manifest = manifest;
			}
			
			public string Type
			{ 
				get 
				{ 
					Object assetObject = AssetDatabase.LoadAssetAtPath(AssetFile, typeof(Object));
					return assetObject.GetType().Name;
				}
			}
			
			public Object DefaultReturn{ get { return null; } }
			
			public string LogString
			{ 
				get 
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("AssetManifest: asset={0}", AssetFile);
					return sb.ToString();
				}
			}
			
			public string AssetFile{ get {return _assetFile;} }
			public Manifest ManifestInst{ get {return _manifest;} }
			
			protected string _assetFile;
			protected Manifest _manifest;
		}
		
		protected static AssetProcessor<ProcessData> _processor = new AssetProcessor<ProcessData>();
		protected static bool _initialized;
	}
}