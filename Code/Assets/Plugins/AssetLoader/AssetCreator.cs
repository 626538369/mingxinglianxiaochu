using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset creator class.
	/// </summary>
	public class AssetCreator
	{
		/// <summary>
		/// Creates the asset.
		/// </summary>
		/// <returns>
		/// The asset.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='assetObject'>
		/// Asset object.
		/// </param>
		/// <param name='dependObjects'>
		/// Depend objects.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		/// <param name='clonedObject'>
		/// Cloned object.
		/// </param>
		public static Object CreateAsset(string assetFile, Object assetObject, Dictionary<string, Object> dependObjects, Manifest manifest, out bool clonedObject)
		{
			Initialize();
			
			clonedObject = false;
			
			ProcessData processData = new ProcessData(assetFile, assetObject, dependObjects, manifest);
			
			Object returnObject = _processor.Process(processData);
			
			clonedObject = processData.IsCloned;
				
			return returnObject;
		}
		
		protected static void Initialize()
		{
			if (_initialized)
				return;
			
			_initialized = true;
			
			_processor.Register("GameObject", CreateGameObject);
			_processor.Register("Material", CreateMaterial);
		}
		
		/// <summary>
		/// Creates the game object.
		/// </summary>
		/// <returns>
		/// The game object.
		/// </returns>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		protected static Object CreateGameObject(ProcessData processData)
		{
			GameObject assetGameObject = Object.Instantiate(processData.AssetObject) as GameObject;
			assetGameObject.name = processData.AssetObject.name;
			processData.IsCloned = true;
			
			int dependMatCount = processData.ManifestInst.GetAssetDependMatCount(processData.AssetFile);
			
			for (int index = 0; index < dependMatCount; index ++)
			{
				string matAsset;
				int matIndex;
				string matTransform;
				
				if (!processData.ManifestInst.GetAssetDependMatByIndex(processData.AssetFile, index, out matAsset, out matIndex, out matTransform))
					continue;
				
				if (!processData.DependObjects.ContainsKey(matAsset))
					continue;
				
				Material mat = processData.DependObjects[matAsset] as Material;
				
				Renderer renderer = ObjectUtil.GetTransFromRoot(assetGameObject.transform, matTransform).GetComponent<Renderer>();
				
				int matCount = Mathf.Max(renderer.sharedMaterials.Length, matIndex + 1);
				Material []mats = new Material[matCount];
				
				for (int i = 0; i < renderer.sharedMaterials.Length; i ++)
				{
					mats[i] = renderer.sharedMaterials[i];
				}
				
				mats[matIndex] = mat;
				renderer.sharedMaterials = mats;
			}
			
			int dependAnimCount = processData.ManifestInst.GetAssetDependAnimCount(processData.AssetFile);
			
			for (int index = 0; index < dependAnimCount; index ++)
			{
				string animAsset;
				string animName;
				string animTransform;
				
				if (!processData.ManifestInst.GetAssetDependAnimByIndex(processData.AssetFile, index, out animAsset, out animName, out animTransform))
					continue;
				
				if (!processData.DependObjects.ContainsKey(animAsset))
					continue;
				
				AnimationClip animClip = processData.DependObjects[animAsset] as AnimationClip;
				
				Animation animation = ObjectUtil.GetTransFromRoot(assetGameObject.transform, animTransform).GetComponent<Animation>();
				
				animation.AddClip(animClip, animName);
			}
			
			return assetGameObject;
		}
		
		/// <summary>
		/// Creates the material.
		/// </summary>
		/// <returns>
		/// The material.
		/// </returns>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		protected static Object CreateMaterial(ProcessData processData)
		{
			Material mat = Object.Instantiate(processData.AssetObject) as Material;
			mat.name = processData.AssetObject.name;
			processData.IsCloned = true;
				
			int dependTexCount = processData.ManifestInst.GetAssetDependTexCount(processData.AssetFile);
		
			for (int index = 0; index < dependTexCount; index ++)
			{
				string texAsset;
				string texPropertyName;
				
				if (!processData.ManifestInst.GetAssetDependTexByIndex(processData.AssetFile, index, out texAsset, out texPropertyName))
					continue;
					
				if (!processData.DependObjects.ContainsKey(texAsset))
					continue;
				
				mat.SetTexture(texPropertyName, processData.DependObjects[texAsset] as Texture);
			}
			
			return mat;
		}
		
		/// <summary>
		/// Process data.
		/// </summary>
		protected class ProcessData : IProcessData
		{
			public ProcessData(string assetFile, Object assetObject, Dictionary<string, Object> dependObjects, Manifest manifest)
			{
				_assetFile = assetFile;
				_assetObject = assetObject;
				_dependObjects = dependObjects;
				_manifest = manifest;
			}
			
			public string Type{ get { return AssetObject.GetType().Name; } }
			public Object DefaultReturn{ get { return AssetObject; } }
			public string LogString
			{ 
				get 
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("AssetCreator: {0}", AssetFile);
					return sb.ToString();
				}
			}
			
			public string AssetFile{ get {return _assetFile;} }
			public Object AssetObject{ get {return _assetObject;} }
			public Dictionary<string, Object> DependObjects{ get {return _dependObjects;} }
			public Manifest ManifestInst{ get {return _manifest;} }
			public bool IsCloned{ get {return _isCloned;} set {_isCloned = value;} }
			
			protected string _assetFile;
			protected Object _assetObject;
			protected Dictionary<string, Object> _dependObjects;
			protected Manifest _manifest;
			protected bool _isCloned;
		}
		
		protected static AssetProcessor<ProcessData> _processor = new AssetProcessor<ProcessData>();
		protected static bool _initialized;
	}
}
