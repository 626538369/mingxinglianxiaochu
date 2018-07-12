using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset derivative class is used to extract additional data
	/// from one asset.
	/// </summary>
	public class AssetDerivative
	{
		/// <summary>
		/// Gets the derivative.
		/// </summary>
		/// <returns>
		/// The derivative.
		/// </returns>
		/// <param name='assetFile'>
		/// Asset file.
		/// </param>
		/// <param name='fromAsset'>
		/// From asset.
		/// </param>
		/// <param name='manifest'>
		/// Manifest.
		/// </param>
		public static Object GetDerivative(string assetFile, string fromAsset, Manifest manifest)
		{
			Initialize();
			
			return _processor.Process(new ProcessData(assetFile, fromAsset, manifest));
		}
		
		protected static void Initialize()
		{
			if (_initialized)
				return;
			
			_initialized = true;
			
			_processor.Register("DependentAnimation", ExtractAnimation);			
		}
		
		/// <summary>
		/// Extracts the animation.
		/// </summary>
		/// <returns>
		/// The animation.
		/// </returns>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		protected static Object ExtractAnimation(ProcessData processData)
		{
			string animAsset;
			string animName;
			string animTransform;
			
			if (!processData.ManifestInst.GetAssetDependAnim(processData.FromAsset, processData.AssetFile, out animAsset, out animName, out animTransform))
			{
				Debug.LogErrorFormat("Failed to load asset dependent assets: {0}", processData.LogString);
				return null;
			}
			
			GameObject fromAssetObject = AssetDatabase.LoadAssetAtPath(processData.FromAsset, typeof(Object)) as GameObject;
			
			Transform animTrans = ObjectUtil.GetTransFromRoot(fromAssetObject.transform, animTransform);
			
			if (animTrans == null)
			{
				Debug.LogErrorFormat("Failed to get animation transform: {0}", processData.LogString);
				return null;
			}
			
			Animation animation = animTrans.gameObject.GetComponent<Animation>();
			AnimationState animState = animation[animName];
			
			Object assetObject = ( Object )animState.clip;
			
			return assetObject;
		}
		
		/// <summary>
		/// Process data.
		/// </summary>
		protected class ProcessData : IProcessData
		{
			public ProcessData(string assetFile, string fromAsset, Manifest manifest)
			{
				_assetFile = assetFile;
				_fromAsset = fromAsset;
				_manifest = manifest;
			}
			
			public string Type
			{ 
				get 
				{ 
					string dependType;
			
					if (!ManifestInst.GetDependentType(FromAsset, AssetFile, out dependType))
						return "ErrorType";
					
					return dependType;
				}
			}
			
			public Object DefaultReturn
			{ 
				get 
				{ 
					Debug.LogErrorFormat("Has no process function: {0}", LogString);
					return null; 
				} 
			}
			
			public string LogString
			{ 
				get 
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("AssetDerivative: asset={0} fromasset={1}", AssetFile, FromAsset);
					return sb.ToString();
				}
			}
			
			public string AssetFile{ get {return _assetFile;} }
			public string FromAsset{ get {return _fromAsset;} }
			public Manifest ManifestInst{ get {return _manifest;} }
			
			protected string _assetFile;
			protected string _fromAsset;
			protected Manifest _manifest;
		}
		
		protected static AssetProcessor<ProcessData> _processor = new AssetProcessor<ProcessData>();
		protected static bool _initialized;
	}
}
