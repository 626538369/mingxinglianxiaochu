using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Text;

public class AssetBundleWizard: ScriptableWizard
{
	// All the settings
	public BuildTarget buildTarget = BuildTarget.Android;
	public BuildAssetBundleOptions[] buildOptions = new BuildAssetBundleOptions[]{BuildAssetBundleOptions.CompleteAssets};
	
	public string[] filterExts = new string[]{".meta", ".unityBundle", ".log", ".db"};
	public bool buildDirectory = false;
	
	[HideInInspector] public string outputPath;
	[HideInInspector] public List<Object> buildObjs = new List<Object>();
	
	public void LoadSettings()
	{
		buildDirectory = 1 == PlayerPrefs.GetInt("AssetBundleWizard.buildDirectory", buildDirectory ? 1 : 0);
	}
	
	public void SaveSettings()
	{
		PlayerPrefs.SetInt("AssetBundleWizard.buildDirectory", buildDirectory ? 1 : 0);
		// PlayerPrefs.SetString("", "");
		// RenderTexture texture2D = new RenderTexture();
	}
	
	// internal void BuildMeshRenderAtlas()
	// {
	// 	Object[] sel = Selection.GetFiltered(typeof(Material), SelectionMode.ExcludePrefab);
	// }
	
	internal void ScanProjectFolder()
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
						c = obj.GetComponentsInChildren(typeof(MeshRenderer), true);

						for (int j = 0; j < c.Length; ++j)
						{}
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
	
	// Response when user click the "Ok" Button
	void OnWizardCreate()
	{
		string dataPath = Application.dataPath;
		string persistentPath = Application.persistentDataPath;
		
		// SelectionMode;
		// Selection;
		Debug.Log("OnWizardCreate");
		
		BuildFromSelectTrackDep();
	}
	
	// This is called when the wizard is opened or whenever the user changes something in the wizard
	void OnWizardUpdate()
	{
		Debug.Log("OnWizardUpdate");
	}
	
	// Response when user click the "Other" Button
	void OnWizardOtherButton()
	{
		base.Close();
		Debug.Log("OnWizardOtherButton");
	}
	
	void Close()
	{
		Debug.Log("Close");
	}
	
	void BuildBundle()
	{
		if (null == Selection.activeObject)
			return;
		
		if (string.IsNullOrEmpty(outputPath))
		{
			outputPath = "Assets/Bundles";
		}
		
		string objPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		if (GfanTools.Utils.IsDirectory(objPath))
		{
			BuildAllDirectory(objPath, true);
		}
		
		Object[] buildObjs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
	}
	
	void BuildAllDirectory(string path, bool searchChild)
	{
		string[] fileEnts = GetFileNames(path, searchChild, "*.*");
		UnityEngine.Object[] objs = GetAssets(fileEnts);
		for (int i = 0; i < objs.Length; i++)
        {
            BuildPipeline.PushAssetDependencies();
			
			if (GfanTools.Utils.IsUnityLevel(fileEnts[i]))
			{
            	string errorMsg = BuildPipeline.BuildPlayer(
				new string[]{fileEnts[i]}, 
				// The path where the application will be built. Must include any necessary file extensions, like .exe, .app, .unity3d or .wdgt
				outputPath + objs[i].name + ".unityBundle", 
				buildTarget, 
				BuildOptions.BuildAdditionalStreamedScenes);
			}
			else
			{
				// Collect the Options
				BuildAssetBundleOptions opts = BuildAssetBundleOptions.CompleteAssets;
				foreach (BuildAssetBundleOptions op in buildOptions)
				{
					opts |= op;
				}
				StringBuilder sbl = new StringBuilder(256);
				sbl.Append(outputPath).Append("/").Append(objs[i].name).Append(".unityBundle");
				BuildPipeline.BuildAssetBundle(objs[i], null, sbl.ToString(), opts, buildTarget);
			}
			
            BuildPipeline.PopAssetDependencies();
			
        }
	}
	
	// [MenuItem("GfanTools/BuildFromSelect")]
	void BuildFromSelectTrackDep()
	{
		if (null == Selection.activeObject)
			return;
		
		// // Get the path which relative Assets folder(include)
		// string path = AssetDatabase.GetAssetOrScenePath(Selection.activeObject);
		// // Get the path which relative to the project folder (Assets/Levels/)
		// path = AssetDatabase.GetAssetPath(Selection.activeObject);
		// string[] labels = AssetDatabase.GetLabels(Selection.activeObject);
		// 
		// Get this path array all relative dependencies 
		// string[] deps = AssetDatabase.GetDependencies(new string[]{path});
		
		// Bring up save panel
		string savePath = EditorUtility.SaveFilePanel("BuildAssetBundle", "AssetBundles", Selection.activeObject.name, "bundle");
		// string savePath = EditorUtility.SaveFolderPanel();
		if (savePath.Length != 0)
		{
			// Build the resource file from the active selection.
			Object[] buildObjs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			
			Object mainAsset = Selection.activeObject;
			BuildPipeline.BuildAssetBundle(mainAsset, buildObjs, savePath, 
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
			Selection.objects = buildObjs;
			
			FileStream fs = File.Open(savePath + ".log", FileMode.OpenOrCreate);
			StreamWriter sw = new StreamWriter(fs);
			sw.WriteLine("File: " + savePath + " and Content: ");
			foreach (Object obj in Selection.objects)
			{
				sw.WriteLine("Name: " + obj.name + "\t\t\tType:" + obj.GetType());
				if (obj.GetType() == typeof(Object))
				{
					Debug.LogWarning("Name: " + obj.name + ", Type: " + obj.GetType() + ". the unity3d cannot recognize this file.");
				}
			}
			
			sw.Flush();
			fs.Flush();
			sw.Close();
			fs.Close();
			
			//GC
			System.GC.Collect();
		}
	}
	
	void BuildSelectDir()
	{
		if (null == Selection.activeObject)
			return;
		
		// Relative the Assets directory
		string selectPath = AssetDatabase.GetAssetPath(Selection.activeObject);
		Debug.Log("Select folder is " + selectPath);
		if (!string.IsNullOrEmpty(selectPath) && GfanTools.Utils.IsDirectory(selectPath))
		{
			string[] fileEnts = GetFileNames(selectPath, true, "*.*");
			UnityEngine.Object[] objs = GetAssets(fileEnts);
			foreach (Object obj in objs)
	        {
                BuildPipeline.PushAssetDependencies();
				
				BuildAssetBundleOptions opts = BuildAssetBundleOptions.CompleteAssets;
				foreach (BuildAssetBundleOptions op in buildOptions)
				{
					opts |= op;
				}
				BuildPipeline.BuildAssetBundle(obj, null, "AssetBundles/" + obj.name + ".unityBundle", opts, buildTarget);
				
                BuildPipeline.PopAssetDependencies();
				
                // BuildPipeline.BuildPlayer(scenePath, path, buildTarget, BuildOptions.BuildAdditionalStreamedScenes);
	        }
			
			// int npos = selectPath.LastIndexOf("/");
			// 
			// string fileName = selectPath.Substring(npos + 1, selectPath.Length - npos - 1);
			// Debug.Log("Select fileName is " + fileName);
			
			// // 
			// bool searchChildren = true;
			// string[] fileEnts = Directory.GetFiles(selectPath, SearchOption.AllDirectories);
			// foreach (string file in fileEnts)
			// {
			// 	file = file.Replace("\\", "/");
			// 	int pos = file.LastIndexOf("/");
			// 	file = file.Substring(pos + 1, file.Length - pos - 1);
			// 	string localPath = "Assets/" + file;
			// 	Object obj = AssetDatabase.LoadMainAssetAtPath(localPath);
			// }
		}
	}
	
	UnityEngine.Object[] GetAssets(string[] paths)
	{
		UnityEngine.Object[] objs = new UnityEngine.Object[paths.Length];
		for (int i = 0; i < paths.Length; i++)
		{
			objs[i] = AssetDatabase.LoadAssetAtPath(paths[i], typeof(UnityEngine.Object));
		}
		
		return objs;
	}
	
	string[] GetFileNames(string path, bool searchChild, string searchPattern)
	{
        List<string> fileNames = new List<string>();
		
		StringBuilder sb = new StringBuilder(256);
		
		DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fiInfos = dirInfo.GetFiles(searchPattern, searchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        foreach (FileInfo fi in fiInfos)
        {
			bool isFilter = false;
			for (int i = 0; i < filterExts.Length; i++)
			{
				if (fi.FullName.EndsWith(filterExts[i]))
				{
					isFilter = true;
					break;
				}
			}
			if (isFilter)
				continue;
			
			string fullName = fi.FullName;
			fullName = fullName.Replace("\\", "/");
			sb.Append("Assets/").Append(fullName);
			fullName = sb.ToString();
			
            fileNames.Add(fullName);
        }
		
		return fileNames.ToArray();
	}
}