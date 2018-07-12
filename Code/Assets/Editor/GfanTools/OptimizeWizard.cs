using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Threading;

public class OptimizeWizard : ScriptableWizard
{
	// [Serializable]
	public enum SelectContent
	{
		GameInstance,
		ProjectEZGUIPrefab,
		Other,
	}
	
	public enum HandleScope
	{
		All,
		OnlySelection,
		IncludeChildren,
	}
	
	public bool autoSave = false;
	public SelectContent selectContent = SelectContent.GameInstance;
	public HandleScope handleScope = HandleScope.All;
	public bool openShadow = true;
	
	// Scan the EZGUI ISpriteAggregator
#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
	public bool onlyScanProjectFolder = true;	// Scan the entire "Assets" folder for prefabs containing packable sprites.
#else
	[HideInInspector] public bool scanProjectFolder = true;	// Reuse the entire "Assets" folder for prefabs containing packable sprites.
#endif
	System.Type packableType = typeof(ISpriteAggregator);
	
	public void LoadSettings()
	{
		// PlayerSettings.
		string val = PlayerPrefs.GetString("");
	}
	
	public void SaveSettings()
	{
		PlayerPrefs.SetString("", "");
		// RenderTexture texture2D = new RenderTexture();
	}
	
	internal void BuildMeshRenderAtlas()
	{
		UnityEngine.Object[] sel = Selection.GetFiltered(typeof(Material), SelectionMode.ExcludePrefab);
	}
	
	// Copy from BuildAtlases.cs, Finds all packable sprite objects:
	void FindPackableSprites()
	{
		ArrayList sprites = new ArrayList();
		List<Object> objList = new List<Object>();

		// Get all packed sprites in the scene:
		Object[] o = FindObjectsOfType(typeof(AutoSpriteBase));
		objList.AddRange(o);

		o = FindObjectsOfType(typeof(PackableStub));
		objList.AddRange(o);

		for (int i = 0; i < objList.Count; ++i)
		{
#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
			if (onlyScanProjectFolder)
#else
			if (scanProjectFolder)
#endif
			{
				// Check to see if this is a prefab instance,
				// and if so, don't use it since we'll be updating
				// the prefab itself anyway.
				// Don't add it at all if we're in Unity iPhone and
				// we'll be scanning the project folder since in
				// iPhone, we can't tell if a scene instance is a
				// prefab instance and we'll mess up prefab relationships
				// otherwise:
#if !UNITY_IPHONE || (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
	#if UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9
				if (PrefabType.PrefabInstance != PrefabUtility.GetPrefabType(objList[i]))
					sprites.Add(objList[i]);
	#else
				if (PrefabType.PrefabInstance != EditorUtility.GetPrefabType(objList[i]))
					sprites.Add(objList[i]);
	#endif
#endif
			}
			else
				sprites.Add(objList[i]);
		}

		// See if we need to scan the Assets folder for sprite objects
#if UNITY_IPHONE && !(UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_3_6 || UNITY_3_7 || UNITY_3_8 || UNITY_3_9)
		if (onlyScanProjectFolder)
#else
		if (scanProjectFolder)
#endif
		{
			ScanProjectFolder(sprites);
		}

		// Now filter for the types of sprites we want:
		for (int i = 0; i < sprites.Count; ++i)
		{
			if (!packableType.IsInstanceOfType(sprites[i]))
			{
				sprites.RemoveAt(i);
				--i;
			}
		}
		
		ISpriteAggregator sprite = null;
		GameObject gObj = null;
		for (int i = 0; i < sprites.Count; ++i)
		{
			sprite = (ISpriteAggregator)sprites[i];
			gObj = sprite.gameObject;
			
			Renderer rend = gObj.GetComponent<Renderer>() as Renderer;
			if (null == rend)
				continue;
			
			rend.castShadows = openShadow;
			rend.receiveShadows = openShadow;
		}
	}
	
	// Scans the entire Assets folder recursively looking for
	// prefabs that contain packable sprites:
	internal void ScanProjectFolder(ArrayList sprites)
	{
		string[] files;
		GameObject obj;
		Component[] c;
		
		// Stack of folders:
		Stack stack = new Stack();
		// Add root directory:
		stack.Push(Application.dataPath);
		
		// Continue while there are folders to process
		while (stack.Count > 0)
		{
			// Get top folder:
			string dir = (string)stack.Pop();
			
			try
			{
				// Get a list of all prefabs in this folder:
				files = Directory.GetFiles(dir, "*.prefab");
				
				// Process all prefabs:
				for (int i = 0; i < files.Length; ++i)
				{
					// Make the file path relative to the assets folder:
					files[i] = files[i].Substring(Application.dataPath.Length - 6);

					obj = (GameObject)AssetDatabase.LoadAssetAtPath(files[i], typeof(GameObject));

					if (obj != null)
					{
						c = obj.GetComponentsInChildren(typeof(ISpriteAggregator), true);

						for (int j = 0; j < c.Length; ++j)
						{
							Debug.Log(files[i] + "==>" + c[j].name);
							sprites.Add(c[j]);
						}
					}
				}
				
				// Add all subfolders in this folder:
				foreach (string dn in Directory.GetDirectories(dir))
				{
					stack.Push(dn);
				}
			}
			catch
			{
				// Error
				Debug.LogError("Could not access folder: \"" + dir + "\"");
			}
		}
	}
	
	internal void ScanGameScene()
	{
		UnityEngine.GameObject[] allObjs = null;
		
		if (handleScope == HandleScope.All)
			allObjs = GameObject.FindSceneObjectsOfType(typeof(GameObject)) as UnityEngine.GameObject[];
		else if (handleScope == HandleScope.OnlySelection)
		{
			if (null == Selection.activeGameObject)
			{
				Debug.Log("[OptimizeWizard]: Please select a GameObject.");
				return;
			}
			
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject go in Selection.gameObjects)
			{
				list.Add(go);
			}
			
			allObjs = list.ToArray();
		}
		else if (handleScope == HandleScope.IncludeChildren)
		{
			if (null == Selection.activeGameObject)
			{
				Debug.Log("[OptimizeWizard]: Please select a GameObject.");
				return;
			}
			
			List<GameObject> list = new List<GameObject>();
			foreach (Transform tf in Selection.transforms)
			{
				RecursiveScan(tf, list);
			}
			allObjs = list.ToArray();
		}
		
		foreach (UnityEngine.GameObject gObj in allObjs)
		{
			Renderer rend = gObj.GetComponent<Renderer>() as Renderer;
			if (null == rend)
				continue;
			
			rend.castShadows = openShadow;
			rend.receiveShadows = openShadow;
		}
	}
	
	void RecursiveScan(Transform parent, List<GameObject> list)
	{
		list.Add(parent.gameObject);
		foreach (Transform tf in parent)
		{
			RecursiveScan(tf, list);
		}
	}
	
	void OnWizardCreate()
	{
		// string assetPath = Application.dataPath;
		// SelectionMode;
		// Selection;
		// EditorApplication.OpenScene("");
		if (selectContent == SelectContent.GameInstance)
		{
			ScanGameScene();
		}
		else if (selectContent == SelectContent.ProjectEZGUIPrefab)
		{
			FindPackableSprites();
		}
		
		if (autoSave)
		{
			EditorApplication.SaveScene();
		}
		else
		{
			EditorUtility.DisplayDialog("Success", "The handle is complete, please save it.", "Ok");
		}
	}
	
	void OnWizardUpdate()
	{
	}
}

