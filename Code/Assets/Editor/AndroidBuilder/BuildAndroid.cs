using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class BuildAndroid : MonoBehaviour 
{
	static List<string> buildSettinglevels = new List<string>();
	static void PrepareBuildLevels()
	{
		buildSettinglevels.Clear();
		
		// Prepare all levels
		EditorBuildSettingsScene scene = null;
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			scene = EditorBuildSettings.scenes[i];
			if (scene.enabled)
			{
				buildSettinglevels.Add(scene.path);
			}
		}
	}
	
	public static void ModifyAndroidManifestMeta(string name,string val)	
	{
		string ManifestPath = Path.Combine(Application.dataPath,"Plugins/Android/AndroidManifest.xml");
		//UnityEngine.Debug.Log(ManifestPath);
		XmlDocument document = new XmlDocument();
		document.Load(ManifestPath);
		XmlNodeList nodes = document.SelectNodes("/manifest/application/meta-data");
		
		foreach(XmlNode node in nodes)
		{
			if(node.Attributes["android:name"].Value == name)
				node.Attributes["android:value"].Value = val;
		}

		document.Save(ManifestPath);
	}
	
	public static readonly string ApkBaseName = "qiaojianghu";
    public static readonly string ManifestEntryPackageName = "com.mingyou.mingxinglianxiaochuTW";
	public static readonly string IpaBaseName = "com.mingyou.mingxinglianxiaochuTW";
	public static readonly string IpaBaseNameHD = "com.Ariesgames.QJHHD";
	
//    [MenuItem("Build/BuildSplitApk")]
//    public static void BuildSplitGame ()
//    {
//		BuildSplitGame(GetCombineApkName("gfan"));
//    }
//	
//	[MenuItem("Build/BuildFullApk")]
//    public static void BuildFullGame ()
//    {
//		//! step 1 . get all .unity files
//		string path  = Application.dataPath;
//		
//		// Prepare all levels
//		PrepareBuildLevels();
//		
//        //! step 2. set path
//		string apkName = GetCombineApkName("gfan");
//		
//        string buildpath = Path.Combine(path,"../BuildFull");
//		buildpath = Path.GetFullPath(buildpath);
//		string apkFullPath = Path.Combine(buildpath,apkName);
//		
//		System.IO.Directory.CreateDirectory(buildpath);
//				
//		//! step 3. modify config file
//		PreProcessToBuildFullApk();		
//
//        // Build player.
//        BuildPipeline.BuildPlayer(buildSettinglevels.ToArray(), apkFullPath, BuildTarget.Android, BuildOptions.None);
//    }
	
	public static string GetCombineApkName(string platform, string subChannelid = "",string postfix = ".apk")
	{
		string apkName = ApkBaseName;
		apkName += "_" + platform;
		
		string version = "v" + PlayerSettings.bundleVersion;
		apkName += "_" + version;
		
		// Output date
		string date = System.DateTime.Now.ToString("yyyyMMdd");
		apkName += "_" + date;
		if (subChannelid != "")
			apkName += "_" + subChannelid;
		apkName += postfix;
		
		return apkName;
	}
	
	public static void PreProcessDigitalSignature()
	{
		if (string.IsNullOrEmpty(PlayerSettings.Android.keystoreName))
		{
			// Application.dataPath is Assets directory in current Unity3D project
			PlayerSettings.Android.keystoreName = Application.dataPath + "/android.keystore";
			// throw new System.Exception("Please create or select a keystoreName for your digital signature! " +
			// 	"where it is in PlayerSetting->Publishing Settings");
		}
		
		// our key digital signature
		if(string.IsNullOrEmpty(PlayerSettings.keyaliasPass)
			|| string.IsNullOrEmpty(PlayerSettings.keystorePass))
		{
			PlayerSettings.Android.keystorePass = "123456"; 
			PlayerSettings.Android.keyaliasName = "android";
			PlayerSettings.Android.keyaliasPass = "123456";
			Debug.LogWarning("Android KeyStore password is null!! will auto set it!!");
		}
	}
	
	/// <summary>
	/// Pre-process to build full apk.
	/// change AndroidMenifest file and copy the base jar file to right folder
	/// </summary>
	public static void PreProcessToBuildFullApk(){// tzz added
		// to change the install spliter
		BuildAndroid.ModifyAndroidManifestMeta("install_split","false");
		File.Copy(Path.Combine(Application.dataPath,"Editor/AndroidBuilder/androidGfanPay.jar"),Path.Combine(Application.dataPath,"Plugins/Android/libs/androidGfanPay.jar"),true);
	}
	
	public static void BuildSplitGame(string fileName)
    {
		PreProcessDigitalSignature();
		
		//! step 1 . get all .unity files
		string path  = Application.dataPath;
		
		// Prepare all levels
		PrepareBuildLevels();
		
        //! step 2. set path
		string apkName = fileName;
		
        string buildpath = Path.Combine(path,"../BuildSplit");
		buildpath = Path.GetFullPath(buildpath);
		string apkFullPath = Path.Combine(buildpath,apkName);
		
		System.IO.Directory.CreateDirectory(buildpath);
		
		//! step 3. modify config file
		ModifyAndroidManifestMeta("install_split","true");
		File.Copy(Path.Combine(Application.dataPath,"Editor/AndroidBuilder/androidGfanPay_NR.jar"),Path.Combine(Application.dataPath,"Plugins/Android/libs/androidGfanPay.jar"),true);		
		
        // Build player.
        BuildPipeline.BuildPlayer(buildSettinglevels.ToArray(), apkFullPath, BuildTarget.Android, BuildOptions.None);
		
		//! 
		ModifyAndroidManifestMeta("install_split","false");
		File.Copy(Path.Combine(Application.dataPath,"Editor/AndroidBuilder/androidGfanPay.jar"),Path.Combine(Application.dataPath,"Plugins/Android/libs/androidGfanPay.jar"),true);
		
		string AndroidBuilderPath = Path.Combine(path,"Editor/AndroidBuilder");
		string AndroidBuilder = Path.Combine(path,"Editor/AndroidBuilder/AndroidBuilder.jar");
		
		if(Path.DirectorySeparatorChar == '\\'){
			// in windows platform
			AndroidBuilderPath 	= AndroidBuilderPath.Replace("/","\\");
			AndroidBuilder 		= AndroidBuilder.Replace("/","\\");
			apkFullPath 		= apkFullPath.Replace("/","\\");
		}	
		
		Process proc = new Process();
        proc.StartInfo.FileName = "java";
		proc.StartInfo.Arguments = "-jar " + AndroidBuilder + " " + AndroidBuilderPath + " " + apkFullPath;
        proc.Start();
	}
}