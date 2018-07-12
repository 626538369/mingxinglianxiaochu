using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


class BonePhysics
{
	static List<string> listname = new List<string>();
	static List<string> listname1 = new List<string>();
	static List<string> listname2 = new List<string>();
	static Dictionary<string,ConfigurableJoint> dic = new Dictionary<string,ConfigurableJoint>();
    [MenuItem("m/test")]
    static void test()
    {
		listname.Clear();
		listname1.Clear();
		listname2.Clear();
		dic.Clear();
		
		GameObject go = GameObject.Find("GlobalScripts");
	
		Globals.Instance.MGameDataManager = go.GetComponent(typeof(GameDataManager)) as GameDataManager;
		GameObject A001 = Globals.Instance.MGameDataManager.BonePhysicObj;
		Transform a001 =  Globals.Instance.MGameDataManager.A001Source[0];		
		
		foreach(Transform a001obj in a001)
		{
			listname.Add(a001obj.name);			
		}
		foreach(Transform A001obj in A001.transform)
		{
			listname1.Add(A001obj.name);
		}
		foreach(string st in listname)
		{
			if(listname1.Contains(st))
			{
				listname1.Remove(st);
			}
		}
//		listname1.Add("Bip001 L Hair05");
//		listname1.Add("Bip001 R Hair05");
		Transform[] transobjs2 = A001.GetComponentsInChildren<Transform>();
		Transform[] transobjs = a001.gameObject.GetComponentsInChildren<Transform>();
		BoxCollider bc = null;
		ConfigurableJoint cj = null;
		CapsuleCollider cap = null;
		Rigidbody rBody = null;
//		GameObject tObject = new GameObject(); 
		foreach(string st in listname1)
		{
			foreach(Transform transf in transobjs2)
			{
				if(transf !=null)
				{
					if(transf.name == st)
					{
//						tObject = transf.gameObject;
						bc = transf.GetComponent<BoxCollider>();
//						 cj = transf.GetComponent<ConfigurableJoint>();
						cap = transf.GetComponent<CapsuleCollider>();
						rBody = transf.GetComponent<Rigidbody>();
						if(st == "Bip001 L Hair05"||st == "Bip001 R Hair05")
						{
							GameObject objj = GameObject.Instantiate(transf.gameObject)as GameObject;
							objj.name = st;
							objj.transform.parent = a001;
							if(bc!=null)
							{
								BoxCollider bc1 = objj.transform.GetComponent<BoxCollider>();
								bc1.sharedMaterial = bc.sharedMaterial;
								bc1.center = bc.center;
								bc1.size = bc.size;
							}
							if(cap!=null)
							{
								CapsuleCollider cap1 = objj.transform.GetComponent<CapsuleCollider>();
								cap1.center = cap.center;
								cap1.radius = cap.radius;
								cap1.height = cap.height;
								cap1.direction = cap.direction;
								cap1.sharedMaterial = cap.sharedMaterial;
							}
						}
					}
				}				
			}
			foreach(Transform transf in transobjs)
			{
				if(transf !=null)
				{
					if(transf.name == st)
					{
						transf.parent = a001 ;
						if(rBody!=null)
						{
							Rigidbody rb = transf.gameObject.AddComponent<Rigidbody>();
							rb.mass = rBody.mass;
							rb.drag = rBody.drag;
							rb.useGravity = rBody.useGravity;
							rb.isKinematic = rBody.isKinematic;
							rb.angularDrag = rBody.angularDrag;
						}
						
						if(bc!=null)
						{
							BoxCollider bc1 = transf.gameObject.AddComponent<BoxCollider>();
							bc1.sharedMaterial = bc.sharedMaterial;
							bc1.center = bc.center;
							bc1.size = bc.size;
						}
						if(cap!=null)
						{
							CapsuleCollider cap1 = transf.gameObject.AddComponent<CapsuleCollider>();
							cap1.center = cap.center;
							cap1.radius = cap.radius;
							cap1.height = cap.height;
							cap1.direction = cap.direction;
							cap1.sharedMaterial = cap.sharedMaterial;
						}
						
//						if(cj!=null)
//						{
//							ConfigurableJoint cj1 = transf.gameObject.AddComponent<ConfigurableJoint>();
////						cj1.transform = cj.transform;
//							foreach(Transform transf2 in transobjs)
//							{
//								if(transf2.name == cj.connectedBody.name)
//								{
//									cj1.connectedBody = transf2.gameObject.GetComponent<Rigidbody>();
//								}
//							}
//						}						
					}
				}				
			}
				
		}
		listname2.Add("Bip001 Pelvis");
		listname2.Add("Bip001 Head");
		listname2.Add("Bip001 Spine");
		listname2.Add("Bip001 Spine1");
		listname2.Add("Bip001 Spine2");
		listname2.Add("Bip001 L Thigh");
		listname2.Add("Bip001 L Calf");
		listname2.Add("Bip001 R Thigh");
		listname2.Add("Bip001 R Calf");
		listname2.Add("Bip001 L Clavicle");
		listname2.Add("Bip001 L UpperArm");
		listname2.Add("Bip001 L Forearm");
		listname2.Add("Bip001 Neck");
		listname2.Add("Bip001 R Clavicle");
		listname2.Add("Bip001 R UpperArm");
//		listname2.Add("Bip001 R Forearm");
		listname2.Add("Bip001 L Sternum Con");
		listname2.Add("Bip001 R Sternum Con");
		

		
		foreach(string strg in listname2)
		{
			CapsuleCollider cc = null;
			foreach(Transform transf in transobjs2)
			{
				if(transf !=null)
				{
					if(transf.name == strg)
					{
						 cc = transf.GetComponent<CapsuleCollider>();				
					}
				}				
			}
			foreach(Transform transf in transobjs)
			{
				if(transf !=null)
				{
					if(transf.name == strg)
					{
						CapsuleCollider ccl = transf.gameObject.AddComponent<CapsuleCollider>();
						ccl.center = cc.center;
						ccl.radius = cc.radius;
						ccl.height = cc.height;
						ccl.direction = cc.direction;
						ccl.sharedMaterial = cc.sharedMaterial;
						if(strg =="Bip001 Pelvis"||strg =="Bip001 Head"||strg =="Bip001 Spine")
						{
							Rigidbody rb1 = transf.gameObject.AddComponent<Rigidbody>();
							rb1.angularDrag = 0f;
							rb1.isKinematic = true;
						}
					}
					
				}				
			}
		}
		foreach(Transform transf in transobjs2)
		{
			cj = transf.GetComponent<ConfigurableJoint>();
			if(cj!=null)
			{
				dic.Add(transf.name,cj);
			}			
		}
		
		foreach(Transform tra in transobjs)
		{
			if(dic.ContainsKey(tra.name))
			{
//				tra.gameObject.GetComponent<Rigidbody>();
				ConfigurableJoint cj1 = tra.gameObject.AddComponent<ConfigurableJoint>();
				foreach(Transform tra2 in transobjs)
				{
					if(tra2.name == dic[tra.name].connectedBody.name)
					{
						cj1.connectedBody = tra2.gameObject.GetComponent<Rigidbody>();
						cj1.xMotion = dic[tra.name].xMotion;
						cj1.yMotion = dic[tra.name].yMotion;
						cj1.zMotion = dic[tra.name].zMotion;
						cj1.angularXMotion = dic[tra.name].angularXMotion;
						cj1.angularYMotion = dic[tra.name].angularYMotion;
						cj1.angularZMotion = dic[tra.name].angularZMotion;
						cj1.lowAngularXLimit =  dic[tra.name].lowAngularXLimit;
						cj1.highAngularXLimit =  dic[tra.name].highAngularXLimit;
						cj1.angularYLimit =  dic[tra.name].angularYLimit;
						cj1.angularZLimit =  dic[tra.name].angularZLimit;
					}
					
				}
			}
		}
		ConfigurableJoint conj1 = a001.Find("Bip001 L Hair05").GetComponent<ConfigurableJoint>();
		ConfigurableJoint conj2 = a001.Find("Bip001 R Hair05").GetComponent<ConfigurableJoint>();
		conj1.connectedBody = a001.Find("Bip001 L Hair04").GetComponent<Rigidbody>();
		conj2.connectedBody = a001.Find("Bip001 R Hair04").GetComponent<Rigidbody>();
//		foreach(Transform tran in transobjs)
//		{
//			foreach(Transform transf in transobjs2)
//			{
//				if(transf !=null)
//				{
//					if(transf.name == tran.name)
//					{
//						 cj = transf.GetComponent<ConfigurableJoint>();
//					}
//				}				
//			}
//			foreach(Transform transf in transobjs)
//			{
//				if(transf !=null)
//				{
//					if(transf.name == tran.name)
//					{
//						if(cj!=null)
//						{
//							ConfigurableJoint cj1 = transf.gameObject.AddComponent<ConfigurableJoint>();
////						cj1.transform = cj.transform;
//							foreach(Transform transf2 in transobjs)
//							{
//								if(transf2.name == cj.connectedBody.name)
//								{
//									cj1.connectedBody = transf2.gameObject.GetComponent<Rigidbody>();
//								}
//							}
//						}						
//					}
//				}				
//			}				
//		}
				
	} 
}
public class BuildAssetStruct
{
	public void BuildBundle(Object obj, BuildTarget buildTarget)
	{
        string dataPath = Application.dataPath;
		string outPath = dataPath + "/AssetBundles";
		if (!Directory.Exists(outPath))
		{
			DirectoryInfo dirInfo = Directory.CreateDirectory(outPath);
			Debug.Log("Create a new direcory: " + dirInfo.FullName);
		}
		
		string fullPath = AssetDatabase.GetAssetOrScenePath(obj);
		if (fullPath.Contains("Level"))
		{
			string[] levels = new string[]{fullPath};
			string fileName = outPath + "/" + obj.name + ".assetbundle";
			BuildPipeline.BuildPlayer(levels, fileName, buildTarget, BuildOptions.BuildAdditionalStreamedScenes);
			// BuildPipeline.BuildPlayer(levels, fileName, BuildTarget.StandaloneWindows, BuildOptions.BuildAdditionalStreamedScenes);
			// BuildPipeline.BuildStreamedSceneAssetBundle(levels, fileName, BuildTarget.Android);
		}
		else
		{
			BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
			string fileName = outPath + "/" + obj.name + ".assetbundle";
		    BuildPipeline.PushAssetDependencies();
	        BuildPipeline.BuildAssetBundle(obj, null, fileName, options, buildTarget);
		    BuildPipeline.PopAssetDependencies();
		}
		
	}
	
    public void BuildBundles(BuildTarget buildTarget)
    {
		Md5Manager.Instance.Start();
        string assetRootPath = Application.dataPath;


        //BuildPipeline.BuildPlayer(new string[] { "Assets/Map/Start.unity" }, "web/mw", buildTarget, BuildOptions.Development | BuildOptions.AllowDebugging);
        //return;

        var options =
            BuildAssetBundleOptions.CollectDependencies;

        string path;
		string srcPath;
		string bundleName;
        List<string> dependNames = new List<string>();
		Object[] objs;
       
		//building
        srcPath = Path.Combine(assetRootPath, "Resources/TempArtist/Prefab/Build");
        objs = GetAllAssets(srcPath, false);
        foreach (Object o in objs)
        {
			if (o != null){
			bundleName = o.name;
			if(Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, new Object[]{o}))
			{
				path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
	            BuildPipeline.PushAssetDependencies();
	            BuildPipeline.BuildAssetBundle(o, null, path, options, buildTarget);
	            BuildPipeline.PopAssetDependencies();
			}
			}
        }

        //ship
        srcPath = Path.Combine(assetRootPath, "Resources/TempArtist/Prefab/Ship");
        objs = GetAllAssets(srcPath, false);
        foreach (Object o in objs)
        {
			bundleName = o.name;
			if(Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, new Object[]{o}))
			{
				path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
	            BuildPipeline.PushAssetDependencies();
	            BuildPipeline.BuildAssetBundle(o, null, path, options, buildTarget);
	            BuildPipeline.PopAssetDependencies();
			}
        }
		
		//Particle
        srcPath = Path.Combine(assetRootPath, "Resources/TempArtist/Prefab/Particle");
        objs = GetAllAssets(srcPath, false);
        foreach (Object o in objs)
        {
			bundleName = o.name;
			if(Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, new Object[]{o}))
			{
				path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
	            BuildPipeline.PushAssetDependencies();
	            BuildPipeline.BuildAssetBundle(o, null, path, options, buildTarget);
	            BuildPipeline.PopAssetDependencies();
			}
        }
		
		 //Scene
        srcPath = Path.Combine(assetRootPath, "Resources/TempArtist/Prefab/Scene");
        objs = GetAllAssets(srcPath, false);
        foreach (Object o in objs)
        {
			bundleName = o.name;
			if(Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, new Object[]{o}))
			{
				path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
	            BuildPipeline.PushAssetDependencies();
	            BuildPipeline.BuildAssetBundle(o, null, path, options, buildTarget);
	            BuildPipeline.PopAssetDependencies();
			}
        }
		
		//TagPoint
        srcPath = Path.Combine(assetRootPath, "Resources/TempArtist/Prefab/TagPoint");
        objs = GetAllAssets(srcPath, false);
        foreach (Object o in objs)
        {
			bundleName = o.name;
			if(Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, new Object[]{o}))
			{
				path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
	            BuildPipeline.PushAssetDependencies();
	            BuildPipeline.BuildAssetBundle(o, null, path, options, buildTarget);
	            BuildPipeline.PopAssetDependencies();
			}
        }
  
        Debug.Log("map start");
        //map
        string mapPath = Path.Combine(assetRootPath, "Level");
        objs = GetAllAssets(mapPath, false);
        foreach (Object o in objs)
        {
            if(o != null){
			if (o.name == "Start")
            {
                continue;
            }
            string[] scenePath = new string[] { "Assets/Level/" + o.name + ".unity" };
			if(Md5Manager.Instance.CheckMd5SceneDirty(o.name, scenePath[0]))
			{
	            path = AssetbundlePath(buildTarget) + o.name + ".assetbundle";
	            BuildPipeline.BuildPlayer(scenePath, path, buildTarget, BuildOptions.BuildAdditionalStreamedScenes);				
			}
			}

        }

        //uishader
        objs = GetUICommShader();
        bundleName = "UIShader";
        BuildPipeline.PushAssetDependencies();  ////1
        if (Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, objs))
        {
            path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
            BuildPipeline.BuildAssetBundle(null, objs, path, options, buildTarget);
        }

        //ui comm texture
        objs = GetUICommtexture();
        bundleName = "UITexture";
        BuildPipeline.PushAssetDependencies();  ////2
        if (Md5Manager.Instance.CheckMd5PrefabDirty(bundleName, objs))
        {
            path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
            BuildPipeline.BuildAssetBundle(null, objs, path, options, buildTarget);
        }

        //ui map
        string uimapPath = Path.Combine(assetRootPath, "GUIMap");
        objs = GetAllAssets(uimapPath, false);
        foreach (Object o in objs)
        {
            bundleName = "GUI_" + o.name;
            string[] scenePath = new string[] { "Assets/GUIMap/" + o.name + ".unity" };
            if (Md5Manager.Instance.CheckMd5UIMapDirty(bundleName, o.name, scenePath[0]))
            {
                path = AssetbundlePath(buildTarget) + bundleName + ".assetbundle";
                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildPlayer(scenePath, path, buildTarget, BuildOptions.BuildAdditionalStreamedScenes);
                BuildPipeline.PopAssetDependencies();
            }
        }
        BuildPipeline.PopAssetDependencies();   ////2
        BuildPipeline.PopAssetDependencies();   ////1
		
	
		if(Md5Manager.Instance.CheckVerion())
		{
            AssetDatabase.Refresh();
			//version
			srcPath = Path.Combine(assetRootPath, "VersionData");
            objs = GetAllPrefixAssets(srcPath, "versionFile");
            if (objs.Length == 1)
            {
				bundleName = "version";
                path = VersionPath(buildTarget) + bundleName + ".assetbundle";

                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(objs[0], null, path, options, buildTarget);
                BuildPipeline.PopAssetDependencies();

            }
			
			//download
            objs = GetAllPrefixAssets(srcPath, "DownloadStrategy");
            if (objs.Length == 1)
            {
				bundleName = "DownloadStrategy";
                path = VersionPath(buildTarget) + bundleName + ".assetbundle";

                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(objs[0], null, path, options, buildTarget);
                BuildPipeline.PopAssetDependencies();

            }
			
			//local level
            objs = GetAllPrefixAssets(srcPath, "LocalLevel");
            if (objs.Length == 1)
            {
				bundleName = "LocalLevel";
                path = VersionPath(buildTarget) + bundleName + ".assetbundle";

                BuildPipeline.PushAssetDependencies();
                BuildPipeline.BuildAssetBundle(objs[0], null, path, options, buildTarget);
                BuildPipeline.PopAssetDependencies();

            }
		}
		
		Md5Manager.Instance.End();
		

        return;

    }

    static public void GetAllAssetsString(string path, List<string> assets, bool bSearchChild)
    {
        GetAllAssetsString(path, assets, bSearchChild, "*.*");
    }
	
    static public void GetAllAssetsString(string path, List<string> assets, bool bSearchChild, string searchPattern)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles(searchPattern, bSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        foreach (FileInfo fi in files)
        {
            if (fi.FullName.EndsWith(".meta"))
                continue;
            assets.Add(fi.FullName);
        }

        //if (bSearchChild)
        //{
        //    DirectoryInfo[] dirs = di.GetDirectories();
        //    foreach (DirectoryInfo d in dirs)
        //    {
        //        GetAllAssetsString(d.FullName, assets, bSearchChild);
        //    }
        //}

    }

    public string GetAssetPath(string filePath)
    {
        string assetRootPath = System.IO.Path.GetFullPath(Application.dataPath);
        return "Assets" + filePath.Substring(assetRootPath.Length).Replace("\\", "/");
    }

    public UnityEngine.Object[] GetAllPrefixAssets(string path, string prefix)
    {
        List<string> assets = new List<string>();

        DirectoryInfo di = new DirectoryInfo(path);
        FileInfo[] files = di.GetFiles();
        foreach (FileInfo fi in files)
        {
            if (fi.FullName.Contains(prefix))
            {
                if (fi.FullName.EndsWith(".meta"))
                    continue;
                assets.Add(fi.FullName);
            }
        }
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        foreach (string asset in assets)
        {
            string s = GetAssetPath(asset);
            objs.Add(AssetDatabase.LoadAssetAtPath(s, typeof(UnityEngine.Object)));
        }
        return objs.ToArray();
    }

    public UnityEngine.Object[] GetUICommtexture()
    {
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        //objs.Add(AssetDatabase.LoadAssetAtPath("Assets/GUI/Materials/Mat_GUICommon.mat", typeof(UnityEngine.Object)));
        objs.Add(AssetDatabase.LoadAssetAtPath("Assets/GUISource/Font/songti.mat", typeof(UnityEngine.Object)));
        objs.Add(AssetDatabase.LoadAssetAtPath("Assets/GUISource/Font/songti.txt", typeof(UnityEngine.Object)));
        return objs.ToArray();
    }

    public UnityEngine.Object[] GetUICommShader()
    {
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        objs.Add(AssetDatabase.LoadAssetAtPath("Assets/Plugins/AnBSoft Common/Standard Shaders/Sprite Vertex Colored, Fast.shader", typeof(UnityEngine.Object)));
        //objs.Add(AssetDatabase.LoadAssetAtPath("Assets/Shaders/VertexColorNoTex.shader", typeof(UnityEngine.Object)));
        return objs.ToArray();
    }


    public string[] GetAllDirectory(string path)
    {
        List<string> retDirs = new List<string>();
        DirectoryInfo di = new DirectoryInfo(path);
        DirectoryInfo[] dirs = di.GetDirectories();
        foreach (DirectoryInfo d in dirs)
        {
            retDirs.Add(d.Name);
        }
        return retDirs.ToArray();
    }

    public UnityEngine.Object[] GetAllAssets(string path, bool bSearchChild)
    {
        return GetAllAssets(path, bSearchChild, "*.*");
    }

    public UnityEngine.Object[] GetAllAssets(string path, bool bSearchChild, string searchPattern)
    {
        List<string> assets = new List<string>();
        BuildAssetStruct.GetAllAssetsString(path, assets, bSearchChild, searchPattern);
        List<UnityEngine.Object> objs = new List<UnityEngine.Object>();
        foreach (string asset in assets)
        {
            string s = GetAssetPath(asset);
            objs.Add(AssetDatabase.LoadAssetAtPath(s, typeof(UnityEngine.Object)));
        }
        return objs.ToArray();
    }

    public string VersionPath(BuildTarget buildTarget)
    {
        string retStr;
        if (buildTarget == BuildTarget.iOS)
        {
            retStr = "iphone" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + System.IO.Path.DirectorySeparatorChar;
        }
        else if (buildTarget == BuildTarget.Android)
        {
            retStr = "android" + System.IO.Path.DirectorySeparatorChar + "assetbundles" +  System.IO.Path.DirectorySeparatorChar;
        }
        else if (buildTarget == BuildTarget.WebPlayer)
        {
            retStr = "web" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + System.IO.Path.DirectorySeparatorChar;
        }
        else
        {
            retStr = "unknown" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + System.IO.Path.DirectorySeparatorChar;
        }
        return retStr;
    }

    public string AssetbundlePath(BuildTarget buildTarget)
    {
		string retStr;
		string versionPath = System.IO.Path.DirectorySeparatorChar + Md5Manager.Instance.GetNewVersionName();
        if (buildTarget == BuildTarget.iOS)
        {
            retStr = "iphone" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + versionPath + System.IO.Path.DirectorySeparatorChar;
        }
        else if (buildTarget == BuildTarget.Android)
        {
            retStr = "android" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + versionPath + System.IO.Path.DirectorySeparatorChar;
        }
        else if (buildTarget == BuildTarget.WebPlayer)
        {
            retStr = "web" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + versionPath + System.IO.Path.DirectorySeparatorChar;
        }
        else
        {
            retStr = "unknown" + System.IO.Path.DirectorySeparatorChar + "assetbundles" + versionPath + System.IO.Path.DirectorySeparatorChar;
        }
		if (!Directory.Exists(retStr))
            Directory.CreateDirectory(retStr);
		return retStr;
    }


    public void test()
    {
        Md5Manager.Instance.Start();
        string mapName = "Town_01";
        string mapPath = "Map/" + mapName + ".unity";
        string path = Path.Combine(Application.dataPath, mapPath);
        EditorApplication.OpenScene(path);
        GameObject scene = GameObject.Find("Scene");
		
        if (scene)
        {
            BundleDependency bd = Md5Manager.Instance.CreateBundleDependency(mapName);
            Object[] objs = new Object[1] { scene };

            //dependency
            Object[] rets = EditorUtility.CollectDependencies( objs );
            Debug.Log("rets:" + rets.Length);
            foreach (Object oc in rets)
            {
                Debug.Log("dep:" + oc.ToString());
                string s = AssetDatabase.GetAssetPath(oc);
                bd.AddFileName(s);
                Debug.Log("path:" + s);
            }

            //child prefab
            Transform[] trans = scene.GetComponentsInChildren<Transform>();
            foreach(Transform t in trans)
            {
                GameObject child = t.gameObject;
                if (PrefabType.None == EditorUtility.GetPrefabType(child))
                    continue;
                if ((Object)child != EditorUtility.FindPrefabRoot(child))
                    continue;
                Debug.Log("prefab:" + child.ToString());
                string s = AssetDatabase.GetAssetPath( EditorUtility.GetPrefabParent( child ) );
                bd.AddFileName(s);
                Debug.Log("prefabPath:" + s);
                
            }
            bd.AddFileName(path);
            List<string> assets = new List<string>();
            string lightMapPath = Path.Combine(Application.dataPath, "Map/" + mapName);
            BuildAssetStruct.GetAllAssetsString(lightMapPath, assets, true);
            foreach(string s in assets)
            {
                bd.AddFileName(s);
            }
            Debug.Log("result:" + mapName + bd.GetDirty().ToString());
            bd.PrintDirtyFile();

        }
        
      
		Md5Manager.Instance.CheckVerion();
        Md5Manager.Instance.End();
    }
}