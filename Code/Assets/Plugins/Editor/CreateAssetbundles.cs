using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Mono.Xml;
using System.Security;

class CreateAssetbundles
{
	//public static  BuildTarget AssetBundleTarget =   EditorUserBuildSettings.activeBuildTarget;
	
	public static  BuildTarget AssetBundleTarget =   BuildTarget.iOS;
	
	// This method creates an assetbundle of each SkinnedMeshRenderer
	// found in any selected character fbx, and adds any materials that
	// are intended to be used by the specific SkinnedMeshRenderer.
	[MenuItem("Character Generator/Create A001 Assetbundles")]
	static void Execute()
	{
		bool createdBundle = false;
		string objPath = "Character/A001";
		foreach (Object o in Selection.GetFiltered(typeof (Object), SelectionMode.DeepAssets))
		{
			if (!(o is GameObject)) continue;
			if (o.name.Contains("@")) continue;
			if (!AssetDatabase.GetAssetPath(o).Contains("/Character/")) continue;
			
			GameObject characterFBX = (GameObject)o;
			string name = characterFBX.name;
			
			
			if (name != "A001" && name != "Dog001" )
				continue;
			
			Debug.Log("******* Creating assetbundles for: " + name + " *******");
			
			// Create a directory to store the generated assetbundles.
			if (!Directory.Exists(AssetbundlePath))
				Directory.CreateDirectory(AssetbundlePath);
			
			if (!Directory.Exists(AssetbundlePath + objPath))
				Directory.CreateDirectory(AssetbundlePath + objPath);
			
			// Delete existing assetbundles for current character.
			string[] existingAssetbundles = Directory.GetFiles(AssetbundlePath);
			foreach (string bundle in existingAssetbundles)
			{
				if (bundle.EndsWith(".assetbundle") && bundle.Contains("/assetbundles/" + name))
					File.Delete(bundle);
			}
			
			// Save bones and animations to a seperate assetbundle. Any 
			// possible combination of CharacterElements will use these
			// assets as a base. As we can not edit assets we instantiate
			// the fbx and remove what we dont need. As only assets can be
			// added to assetbundles we save the result as a prefab and delete
			// it as soon as the assetbundle is created.
			GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);
			
			// postprocess animations: we need them animating even offscreen
			foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
				anim.animateOnlyIfVisible = false;
			
			foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
				Object.DestroyImmediate(smr.gameObject);
			
			foreach (MeshRenderer smr in characterClone.GetComponentsInChildren<MeshRenderer>())
				Object.DestroyImmediate(smr.gameObject);
			
			
			
			characterClone.AddComponent<SkinnedMeshRenderer>();
			CreatePrefab(characterClone, "characterbase"+name);
			
			//string path = AssetbundlePath + objPath + "/" + name + "_characterbase.assetbundle";
			//BuildPipeline.BuildAssetBundle(characterBasePrefab, null, path, BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.UncompressedAssetBundle,AssetBundleTarget);
			//AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(characterBasePrefab));
			
			//ZipUtil.CompressFileLZMA(path,AssetbundlePath + objPath + "/" + name + "_characterbase.zip");
			
			//File.Delete(path);
			
			
			// Collect materials.
			//List<Material> materials = EditorHelpers.CollectAll<Material>(GenerateMaterials.MaterialsPath(characterFBX));
			List<Material> materials = EditorHelpers.CollectAll<Material>("Assets/Resources/Character/Materials");
			Dictionary<string,List<string> > skinMeshBoneDic = new Dictionary<string, List<string> >();
			// Create assetbundles for each SkinnedMeshRenderer.
			List<CharacterElement> characterElements = new List<CharacterElement>();
			foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				
				// Save the current SkinnedMeshRenderer as a prefab so it can be included
				// in the assetbundle. As instantiating part of an fbx results in the
				// entire fbx being instantiated, we have to dispose of the entire instance
				// after we detach the SkinnedMeshRenderer in question.
				GameObject rendererClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
				GameObject rendererParent = rendererClone.transform.parent.gameObject;
				rendererClone.transform.parent = null;
				Object.DestroyImmediate(rendererParent);
				CreatePrefab(rendererClone, smr.gameObject.name);
				
				
				List<string> boneNames = new List<string>();
				foreach (Transform t in smr.bones)
					boneNames.Add(t.name);
				
				skinMeshBoneDic.Add(smr.gameObject.name,boneNames);
			}
			
			const string majorDelimiter = "&";
			const string assignDelimiter = "=";
			string filePath = Application.dataPath + "/Resources/Config/SkinMeshedBones.txt";
			try 
			{
				using(FileStream t_file = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.Write))
				{
					t_file.Seek(0,SeekOrigin.Begin);
					using(StreamWriter t_sw = new StreamWriter(t_file))
					{
						foreach(string skinMeshName in skinMeshBoneDic.Keys)
						{
							List<string> boneNames = skinMeshBoneDic[skinMeshName];
							string oneLineStr = skinMeshName + majorDelimiter ;
							for (int i=0; i<boneNames.Count; i++)
							{
								oneLineStr += boneNames[i] + assignDelimiter;
							}
							t_sw.WriteLine(oneLineStr);
						}
					}
				}
			}
			catch(System.Exception ex){
				Debug.LogError("Write config file failed, Exception:" + ex.Message);
			}
		}
	}
	
	static Object GetPrefab(GameObject go, string name)
	{
		Object tempPrefab = EditorUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
		tempPrefab = EditorUtility.ReplacePrefab(go, tempPrefab);
		Object.DestroyImmediate(go);
		return tempPrefab;
	}
	
	static void CreatePrefab(Object go,string name)
	{
		Object tempPrefab = EditorUtility.CreateEmptyPrefab("Assets/Resources/Character/Prefabs/" + name + ".prefab");
		tempPrefab = EditorUtility.ReplacePrefab((GameObject)go, tempPrefab);
		Object.DestroyImmediate(go);
	}
	
	public static string AssetbundlePath
	{
		get { return "Assets" + Path.DirectorySeparatorChar + "assetbundles" + Path.DirectorySeparatorChar; }
	}
	
	
	[MenuItem("Character Generator/BuildSelectionFolderBundle")]
	static void BuildSelectionFolderBundle()
	{
		foreach (Object obj in Selection.GetFiltered(typeof (UnityEngine.Object), SelectionMode.DeepAssets))
		{
			//if (!(obj is Texture2D)) continue;
			if (!(obj is UnityEngine.AudioClip) && !(obj is Texture2D) && !(obj is TextAsset) && 
			    !(obj is UnityEngine.RuntimeAnimatorController) && !(obj is UnityEngine.GameObject) && !(obj is UnityEngine.AnimationClip)) continue;
			
			string fullPath = AssetDatabase.GetAssetOrScenePath(obj);
			
			string fliterStr = "Resources";
			int beginIndex = fullPath.IndexOf(fliterStr)+fliterStr.Length+1;
			string objPath = fullPath.Substring(beginIndex,fullPath.LastIndexOf("/") - beginIndex);
			
			string dataPath = Application.dataPath;
			string outPath = dataPath + "/AssetBundles/" + objPath;
			
			if (!Directory.Exists(outPath))
			{
				DirectoryInfo dirInfo = Directory.CreateDirectory(outPath);
				Debug.Log("Create a new direcory: " + dirInfo.FullName);
			}
			
			
			BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle|BuildAssetBundleOptions.UncompressedAssetBundle|BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
			string fileName = outPath + "/" + obj.name + ".assetbundle";
			BuildPipeline.PushAssetDependencies();
			BuildPipeline.BuildAssetBundle(obj, null, fileName, options, AssetBundleTarget);
			BuildPipeline.PopAssetDependencies();
			
			ZipUtil.CompressFileLZMA(fileName,outPath + "/"  + obj.name + ".zip");
			
			File.Delete(fileName);
		}
		
	}
	
	
	[MenuItem("Character Generator/CopyClientFolderBundle")]
	static void CopyClientFolderBundle()
	{
		string assetFile = "Config/AssetBundleConfig";
		SecurityParser xmlParser = new SecurityParser(); 
		TextAsset textAsset = Resources.Load(assetFile) as TextAsset;
		xmlParser.LoadXml(textAsset.text);
		textAsset = null;
		
		SecurityElement securityElement =  xmlParser.ToXml();
		foreach(SecurityElement childrenElement in securityElement.Children)
		{
			string assetBundleResourceName = StrParser.ParseStr(childrenElement.Attribute("ResourceName"),"");
			string resourceOriganlName = StrParser.ParseStr(childrenElement.Attribute("OrignalName"),"");
			int assetBundleResourceType = StrParser.ParseDecInt(StrParser.ParseStr(childrenElement.Attribute("Location"),""),0);
			if (assetBundleResourceType == 0)
			{
				string srcFileName = Application.dataPath + "/assetbundles/" + assetBundleResourceName;
				
				string destFileName = Application.dataPath + "/assetbundlesLocalCopy/Resources/" + assetBundleResourceName;
				
				string path = destFileName.Substring(0,destFileName.LastIndexOf("/"));
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				
				File.Copy(srcFileName,destFileName,true);
			}
			else if (assetBundleResourceType == 1)
			{
				string srcFileName = Application.dataPath + "/assetbundles/" + assetBundleResourceName;
				
				string destFileName = Application.dataPath + "/assetbundlesServerCopy/Resources/" + assetBundleResourceName;
				
				string path = destFileName.Substring(0,destFileName.LastIndexOf("/"));
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				
				if (srcFileName.Contains("TWXR017b.zip"))
				{
					Debug.Log("!!!!!!!!!!!!!!!!!");
				}
				
				File.Copy(srcFileName,destFileName,true);
				if (File.Exists(Application.dataPath + "/Resources/" + resourceOriganlName))
				{
					File.Delete(Application.dataPath + "/Resources/" + resourceOriganlName);
				}
			}
		}
		
		Debug.Log("CopyClientFolderBundle Finished !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		
	}
}