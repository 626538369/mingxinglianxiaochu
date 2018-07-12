using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResAssetBundle
{
	/// <summary>
	/// Asset shrinker class shrink asset object when build into bundle.
	/// </summary>
	public class AssetShrinker
	{
		/// <summary>
		/// Shrinks the asset.
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
		/// <param name='objsToDestroy'>
		/// Objects to destroy.
		/// </param>
		/// <param name='objsToDelete'>
		/// Objects to delete.
		/// </param>
		public static Object ShrinkAsset(string assetFile, Object assetObject, List<Object> objsToDestroy, List<Object> objsToDelete)
		{
			Initialize();
			
			return _processor.Process(new ProcessData(assetFile, assetObject, objsToDestroy, objsToDelete));
		}
		
		protected static void Initialize()
		{
			if (_initialized)
				return;
			
			_initialized = true;
			
			_processor.Register("GameObject", ShrinkGameObject);
		}
		
		/// <summary>
		/// Shrinks the game object.
		/// </summary>
		/// <returns>
		/// The game object.
		/// </returns>
		/// <param name='processData'>
		/// Process data.
		/// </param>
		protected static Object ShrinkGameObject(ProcessData processData)
		{
			GameObject gameObject = Object.Instantiate(processData.AssetObject) as GameObject;
			processData.ObjsToDestroy.Add(gameObject);
	
			Renderer []renderers = gameObject.GetComponentsInChildren<Renderer>(true);
			
			foreach (Renderer renderer in renderers)
			{
				renderer.sharedMaterials = new Material[0];
			}
		
			Animation []animations = gameObject.GetComponentsInChildren<Animation>(true);
			List<GameObject> animGameObjects = new List<GameObject>();
			
			foreach (Animation animation in animations)
			{
				animGameObjects.Add( animation.gameObject );
			}
			
			foreach (GameObject animObject in animGameObjects)
			{
				GameObject.DestroyImmediate(animObject.GetComponent<Animation>());
				animObject.AddComponent<Animation>();
			}
			
			// Save the game object to a temp prefab.
			string prefabPath = Path.ChangeExtension(processData.AssetFile, ".prefab");
			prefabPath = BuilderHelper.GetTemporaryFilePath(prefabPath);
			Object gameObjPrefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
			gameObjPrefab = PrefabUtility.ReplacePrefab(gameObject, gameObjPrefab);
			processData.ObjsToDelete.Add(gameObjPrefab);
			
			return gameObjPrefab;
		}
		
		/// <summary>
		/// Process data.
		/// </summary>
		protected class ProcessData : IProcessData
		{
			public ProcessData(string assetFile, Object assetObject, List<Object> objsToDestroy, List<Object> objsToDelete)
			{
				_assetFile = assetFile;
				_assetObject = assetObject;
				_objsToDestroy = objsToDestroy;
				_objsToDelete = objsToDelete;
			}
			
			public string Type{ get { return AssetObject.GetType().Name; } }
			public Object DefaultReturn{ get { return AssetObject; } }
			public string LogString
			{ 
				get 
				{
					StringBuilder sb = new StringBuilder();
					sb.AppendFormat("AssetShrinker: {0}", AssetFile);
					return sb.ToString();
				}
			}
			
			public string AssetFile{ get {return _assetFile;} }
			public Object AssetObject{ get {return _assetObject;} }
			public List<Object> ObjsToDestroy{ get {return _objsToDestroy;} }
			public List<Object> ObjsToDelete { get {return _objsToDelete;} }
			
			protected string _assetFile;
			protected Object _assetObject;
			protected List<Object> _objsToDestroy;
			protected List<Object> _objsToDelete;
		}
		
		protected static AssetProcessor<ProcessData> _processor = new AssetProcessor<ProcessData>();
		protected static bool _initialized;
	}
}
