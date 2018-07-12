using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

/**
/Applications/Unity/Unity.app/Contents/MacOS/Unity -quit -batchmode -projectPath $PROJECT_PATH -executeMethod CommandBuild.BuildAndroid
/ D:\0Work\1Work\sea_client\ClientTestin
*/

/// <summary>
/// Command build. one key build different channelId's apk
/// </summary>
public class CommandBuild 
{
	public static string projectPath = "D:/0Work/1Work/sea_client/ClientTestin/";
	public static readonly string channelFilePath = "Assets/Plugins/Android/ChannleIds.txt";
	public static readonly string manifestFilePath = "Assets/Plugins/Android/AndroidManifest.xml";
	
	[MenuItem("BuildInstallPackage/BuildMultCPsAndroid")]
	public static void BuildMultCPsAndroid()
    {
		// tzz added for preprocessing for build full apk
		// change AndroidMenifest file and copy the base jar file to right folder
		BuildAndroid.PreProcessToBuildFullApk();
		
		int assetIdx = Application.dataPath.IndexOf("/Assets");
		projectPath = Application.dataPath.Substring(0, assetIdx) + "/";
		
		ReadChannelIdsFile(projectPath + channelFilePath);
		ReadManifestContents();
		
		for (int i = 0; i < channelIds.Count; i++)
		{
			BuildOneChannelApk(channelIds[i]);
		}
		
		ResetManifestChannelId();
    }
	
	static List<string> channelIds = new List<string>();
	static List<string> manifestContents = new List<string>();
	static void BuildOneChannelApk(string channelId)
	{
		// EditorSettings;
		// EditorUserSettings;
		// EditorUserBuildSettings;
		// PlayerSettings;
		
		// Prepare modify AndroidManifest.xml
		ModifyManifestChannelId(channelId);
		BuildAndroidPackage(channelId);
	}
	
	static void ReadChannelIdsFile(string fullPath)
	{
		channelIds.Clear();
		using (FileStream fs = new FileStream(fullPath, FileMode.Open))
		{
			using (StreamReader reader = new StreamReader(fs))
			{
				while (reader.Peek() != -1)
		        {
		            channelIds.Add(reader.ReadLine());
		        }
				reader.Close();
			}
			fs.Close();
		}
	}
	
	
	static void ReadManifestContents()
	{
		manifestContents.Clear();
		
		string manifestFullPath = projectPath + manifestFilePath;
		using (FileStream fs = new FileStream(manifestFullPath, FileMode.Open))
		{
			using (StreamReader reader = new StreamReader(fs))
			{
				// Read all
		        while (reader.Peek() != -1)
		        {
		            manifestContents.Add(reader.ReadLine());
		        }
       			reader.Close();
			}
			fs.Close();
		}
	}
	
	static void ModifyManifestChannelId(string channelId)
	{
		string manifestFullPath = projectPath + manifestFilePath;
		
		string[] list = new string[manifestContents.Count];
		manifestContents.CopyTo(list); // Copy to a temporary list
		
		// Modify
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Contains("gfan_cpid"))
            {
                string[] s = list[i + 1].Split(new char[] { '\"' });
                if (s.Length != 3)
                {
                    Debug.LogError(" String's length is Error ");
                    break;
                }
				
				// Find "android:value="
                if (string.Compare(list[i + 1].Substring(0, s[0].Length).Trim(), "android:value=") != 0)
                {
                    Debug.LogError(" Start string is Error");
                    break;
                }
				
				// Replace it use new channelId
                list[i + 1] = s[0] + "\"" + s[1].Replace(s[1], channelId) + "\"" + s[2];
                break;
            }
        }
		
		// Write
		using (FileStream fs = new FileStream(manifestFullPath, FileMode.Create))
		{
			using (StreamWriter writer = new StreamWriter(fs))
			{
				for (int i = 0; i < list.Length; i++)
		        {
		            writer.WriteLine(list[i]);
		        }
		        writer.Close();
			}
			fs.Close();
		}
	}
	
	static void ResetManifestChannelId()
	{
		string manifestFullPath = projectPath + manifestFilePath;
		
		// Write
		using (FileStream fs = new FileStream(manifestFullPath, FileMode.Create))
		{
			using (StreamWriter writer = new StreamWriter(fs))
			{
				for (int i = 0; i < manifestContents.Count; i++)
		        {
		            writer.WriteLine(manifestContents[i]);
		        }
		        writer.Close();
			}
			fs.Close();
		}
	}
	
#region Multiple Sdk
	/** Oncekey build install package
	 * */
	public const string PlatformGfan = "gfan";
	public const string PlatformNd91 = "91";
	public const string PlatformUC = "uc";
	public const string PlatformPP = "pp";
	public const string PlatformMi = "mi";
	public const string PlatformApp = "appstore";
	public const string PlatformNone = "none";
	public const string PlatformWPay = "wpay";
	
	[MenuItem("BuildInstallPackage/FullApk/Gfan")]
	public static void BuildFullGfanApk()
    {
		BuildGfanInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/Nd91")]
	public static void BuildFullNd91Apk()
    {
		BuildNd91InstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/None")]
	public static void BuildFullNoneApk()
    {
		BuildNoneInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/WPay")]
	public static void BuildFullWPayApk()
    {
		//for (int i=200001; i<=200011; i++)
		//{
			BuildWPayInstallPackage("200001",true);
		//}
	}
	
	[MenuItem("BuildInstallPackage/FullApk/PP")]
	public static void BuildFullPPApk()
    {
		BuildPPInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/UC")]
	public static void BuildFullUCApk()
    {
		BuildUCInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/Mi")]
	public static void BuildFullMiApk()
    {
		BuildMiInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/Android Gfan+Nd91+UC+Mi")]
	public static void BuildFullSDKApk()
    {
		BuildGfanInstallPackage(true);
		BuildNd91InstallPackage(true);
		BuildUCInstallPackage(true);
		BuildMiInstallPackage(true);
	}
	
	[MenuItem("BuildInstallPackage/FullApk/Android Gfan ManyChannel")]
	public static void BuildGFanChannelsApk()
    {
		string[] channels = 
		{
			"_aaa",
			"_aax",
			"_aaj",
			"_abg",
			"_aai",
			"_aag",
			"_abf",
			"_aan",
			"_aas",
			"_abn",
		};
		
		foreach(string ch in channels){
			string path = BuildGfanInstallPackage(true,ch);
			
			// rename
			string newPath = path.Replace(PlatformGfan,PlatformGfan + ch);
			if(System.IO.File.Exists(newPath)){
				System.IO.File.Delete(newPath);
			}
			System.IO.File.Move(path,newPath);
		}
		
	}
	
	//-------------------------------------------------------------------
	[MenuItem("BuildInstallPackage/SplitApk/Gfan")]
	public static void BuildSplitGfanApk()
    {
		BuildGfanInstallPackage(false);
	}
	
	[MenuItem("BuildInstallPackage/SplitApk/Nd91")]
	public static void BuildSplitNd91Apk()
    {
		BuildNd91InstallPackage(false);
	}
	
	[MenuItem("BuildInstallPackage/SplitApk/PP")]
	public static void BuildSplitPPApk()
    {
		BuildPPInstallPackage(false);
	}
	
	[MenuItem("BuildInstallPackage/SplitApk/UC")]
	public static void BuildSplitUCApk()
    {
		BuildUCInstallPackage(false);
	}
	
	[MenuItem("BuildInstallPackage/SplitApk/Mi")]
	public static void BuildSplitMiApk()
    {
		BuildMiInstallPackage(false);
	}
	
	[MenuItem("BuildInstallPackage/AppStoreIpa")]
	public static void BuildAppStoreInstallPackage()
    {
		// BundleID = com.Ariesgames.QJH
		// AppId = 673015105
		string platform = PlatformApp;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier = BuildAndroid.IpaBaseName;
		PlayerSettings.bundleVersion = GameDefines.GameVersion;
//		PlayerSettings.shortBundleVersion = GameDefines.GameVersion;
		string outputPath = "AppStoreXCode";
				
		BuildXcodeProject(outputPath);
    }

	[MenuItem("BuildInstallPackage/AppStore64Ipa")]
	public static void BuildAppStore64InstallPackage()
	{
		// BundleID = com.Ariesgames.QJH
		// AppId = 673015105
		string platform = PlatformApp;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		
		PlayerSettings.applicationIdentifier = BuildAndroid.IpaBaseName;
		PlayerSettings.bundleVersion = GameDefines.GameVersion;
//		PlayerSettings.shortBundleVersion = GameDefines.GameVersion;
		
		string outputPath = "AppStoreXCode64";
		
		BuildXcodeProject(outputPath);
	}
	
	[MenuItem("BuildInstallPackage/AppStoreIpaHD")]
	public static void BuildAppStoreHDInstallPackage()
    {
		// BundleID = com.Ariesgames.QJH
		// AppId = 673015105
		string platform = PlatformApp;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier = BuildAndroid.IpaBaseNameHD;
				
		string outputPath = "AppStoreXCode";
				
		BuildXcodeProject(outputPath);
    }
	
	static void ModifyManifestPackageName(string newName)
	{
		string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
		XmlDocument document = new XmlDocument();
		document.Load(manifestPath);
		XmlNodeList nodes = document.SelectNodes("/manifest");
		
		foreach(XmlNode node in nodes)
		{
			node.Attributes["package"].Value = newName;
			break;
		}

		document.Save(manifestPath);
	}
	
	public static string BuildGfanInstallPackage(bool fullPackage = true,string subChannel = "")
    {
		SetLauncherIconSetting(new string[]{
#if UNITY_ANDROID
		
			"Assets/SDKIcon/gfan/ic_launcher144.png",
			"Assets/SDKIcon/gfan/ic_launcher72.png",
			"Assets/SDKIcon/gfan/ic_launcher48.png",
			"Assets/SDKIcon/gfan/ic_launcher48.png",

#elif UNITY_IPHONE

			"Assets/SDKIcon/gfan/ic_launcher57.png",
			"Assets/SDKIcon/gfan/ic_launcher114.png",
			"Assets/SDKIcon/gfan/ic_launcher72.png",
			"Assets/SDKIcon/gfan/ic_launcher144.png",
#endif
		});
		string path = "";
		
		string platform = PlatformGfan;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform + subChannel);
		CleanupBundleIdentifier();
		// PlayerSettings.bundleIdentifier += suffix;
#if UNITY_ANDROID
		// modify the manifest.xml file
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			path = BuildAndroidPackage(platform);
			
		}else
		{
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
		}
#elif UNITY_IPHONE
		BuildXcodeProject("GfanSDK");
#endif
		return path;
	}
	
	static void BuildGfaniPhone()
    {
		CleanupBundleIdentifier();
		
		string outputPath = "./ProjectXcode/GfanSdk";
		if (!Directory.Exists(outputPath))
		{
			Directory.CreateDirectory(outputPath);
		}
		
		BuildXcodeProject(outputPath);
    }
	
	public static void BuildNd91InstallPackage(bool fullPackage = true)
    {
		SetLauncherIconSetting(new string[]{
#if UNITY_ANDROID
		
			"Assets/SDKIcon/91/ic_launcher144.png",
			"Assets/SDKIcon/91/ic_launcher72.png",
			"Assets/SDKIcon/91/ic_launcher48.png",
			"Assets/SDKIcon/91/ic_launcher48.png",

#elif UNITY_IPHONE

			"Assets/SDKIcon/91/ic_launcher57.png",
			"Assets/SDKIcon/91/ic_launcher114.png",
			"Assets/SDKIcon/91/ic_launcher72.png",
			"Assets/SDKIcon/91/ic_launcher144.png",
#endif
		});
		
		string platform = PlatformNd91;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
		BuildXcodeProject("ND91SDK");
#endif
	}
	
	static void BuildNoneInstallPackage(bool fullPackage = true)
	{
		SetLauncherIconSetting(new string[]{
#if UNITY_ANDROID
		
			"Assets/SDKIcon/none/ic_launcher144.png",
			"Assets/SDKIcon/none/ic_launcher72.png",
			"Assets/SDKIcon/none/ic_launcher48.png",
			"Assets/SDKIcon/none/ic_launcher48.png",

#elif UNITY_IPHONE

			"Assets/SDKIcon/91/ic_launcher57.png",
			"Assets/SDKIcon/91/ic_launcher114.png",
			"Assets/SDKIcon/91/ic_launcher72.png",
			"Assets/SDKIcon/91/ic_launcher144.png",
#endif
		});
		
		string platform = PlatformNone;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
		BuildXcodeProject("ND91SDK");
#endif	
	}
	
	static void BuildWPayInstallPackage(string subChannnelID,bool fullPackage = true)
	{
			SetLauncherIconSetting(new string[]{
#if UNITY_ANDROID
		
			"Assets/SDKIcon/wpay/ic_launcher144.png",
			"Assets/SDKIcon/wpay/ic_launcher72.png",
			"Assets/SDKIcon/wpay/ic_launcher48.png",
			"Assets/SDKIcon/wpay/ic_launcher48.png",

#elif UNITY_IPHONE

			"Assets/SDKIcon/91/ic_launcher57.png",
			"Assets/SDKIcon/91/ic_launcher114.png",
			"Assets/SDKIcon/91/ic_launcher72.png",
			"Assets/SDKIcon/91/ic_launcher144.png",
#endif
		});
		
		string platform = PlatformWPay;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		RecordSubChannel2Resouces(subChannnelID);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform,subChannnelID);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
		BuildXcodeProject("ND91SDK");
#endif	
	}
	
	static void RecordChannel2Resouces(string sdkPlatform)
	{
		using (FileStream fs = new FileStream("Assets/Resources/SDKPlatform.txt", FileMode.OpenOrCreate))
		{
			fs.SetLength(0);
			fs.Flush();
			
			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.Write(sdkPlatform);
				writer.Close();
			}
			fs.Close();
		}
		
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
	
	static void RecordSubChannel2Resouces(string subChannedId)
	{
		using (FileStream fs = new FileStream("Assets/Resources/SDKSubChannel.txt", FileMode.OpenOrCreate))
		{
			fs.SetLength(0);
			fs.Flush();
			
			using (StreamWriter writer = new StreamWriter(fs))
			{
				writer.Write(subChannedId);
				writer.Close();
			}
			fs.Close();
		}
		
		BuildAndroid.ModifyAndroidManifestMeta("WIIPAY_CHANNEL_CODE",subChannedId);
		BuildAndroid.ModifyAndroidManifestMeta("Channel_ID",subChannedId);
		
		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
		
	public static void BuildUCInstallPackage(bool fullPackage = true)
    {
		SetLauncherIconSetting(new string[]{
			"Assets/SDKIcon/UC/ic_launcher144.png",
			"Assets/SDKIcon/UC/ic_launcher72.png",
			"Assets/SDKIcon/UC/ic_launcher48.png",
			"Assets/SDKIcon/UC/ic_launcher48.png",
		});
		
		string platform = PlatformUC;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
#endif
	}
	
	public static void BuildPPInstallPackage(bool fullPackage = true)
    {
		SetLauncherIconSetting(new string[]
		{
#if UNITY_ANDROID
		
			"Assets/SDKIcon/PP/ic_launcher144.png",
			"Assets/SDKIcon/PP/ic_launcher72.png",
			"Assets/SDKIcon/PP/ic_launcher48.png",
			"Assets/SDKIcon/PP/ic_launcher48.png",

#elif UNITY_IPHONE

			"Assets/SDKIcon/PP/ic_launcher57.png",
			"Assets/SDKIcon/PP/ic_launcher114.png",
			"Assets/SDKIcon/PP/ic_launcher72.png",
			"Assets/SDKIcon/PP/ic_launcher144.png",
#endif
		});
		
		string platform = PlatformPP;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
		BuildXcodeProject("PPSDK");
#endif
	}
	
	public static void BuildMiInstallPackage(bool fullPackage = true)
    {
		SetLauncherIconSetting(new string[]{
			"Assets/SDKIcon/gfan/ic_launcher144.png",
			"Assets/SDKIcon/gfan/ic_launcher72.png",
			"Assets/SDKIcon/gfan/ic_launcher48.png",
			"Assets/SDKIcon/gfan/ic_launcher48.png",
		});
		
		string platform = PlatformMi;
		string suffix = GetBundleIdentifierSplitChar() + platform;
		
		RecordChannel2Resouces(platform);
		CleanupBundleIdentifier();
		PlayerSettings.applicationIdentifier += suffix;
#if UNITY_ANDROID
		ModifyManifestPackageName(BuildAndroid.ManifestEntryPackageName + suffix);
		if (fullPackage)
		{
			// tzz added for preprocessing for build full apk
			// change AndroidMenifest file and copy the base jar file to right folder
			BuildAndroid.PreProcessToBuildFullApk();
			BuildAndroidPackage(platform);
		}
		else
			BuildAndroid.BuildSplitGame(BuildAndroid.GetCombineApkName(platform));
#elif UNITY_IPHONE
#endif
	}
	
	static string GetBundleIdentifierSplitChar()
	{
		// iOS and Android is different in using "_" & "-"
		string splitChar = "_";
#if UNITY_ANDROID
		splitChar = "_";
#elif UNITY_IPHONE
		splitChar = "-";
		splitChar = "";
#endif
		return splitChar;
	}
	
	static void CleanupBundleIdentifier()
	{
		PlayerSettings.applicationIdentifier = BuildAndroid.ManifestEntryPackageName;
	}
	
	static string BuildAndroidPackage(string channelId,string subChannelId="", string postfix = ".apk")
	{
		BuildAndroid.PreProcessDigitalSignature();
		//@{{ CAUTION!!
		//
		// delete the other SDK alipay_msp.jar to avoid conflict with the one in gfansdk_pay.jar 
		//
		string alipayFile;
		string backupAlipay;
		
		if(channelId.StartsWith(PlatformGfan)){
			alipayFile = Application.dataPath + "/Plugins/Android/libs/alipay_msp.jar";
			backupAlipay = Application.dataPath + "/alipay_msp.jar";	
		}else{
			alipayFile = Application.dataPath + "/Plugins/Android/libs/gfansdk_pay.jar";
			backupAlipay = Application.dataPath + "/gfansdk_pay.jar";
		}
		
		if(File.Exists(alipayFile)){
			if(File.Exists(backupAlipay)){
				File.Delete(backupAlipay);	
			}
			
			File.Move(alipayFile,backupAlipay);
		}else{
			throw new System.Exception("Failed!! Please Update SVN to restore : " + alipayFile);
		}
		//@}}
		
		
		// Prepare all levels
		List<string> levels = new List<string>();
		EditorBuildSettingsScene scene = null;
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			scene = EditorBuildSettings.scenes[i];
			if (scene.enabled)
			{
				levels.Add(scene.path);
			}
		}
		
		string path = BuildAndroid.GetCombineApkName(channelId,subChannelId, postfix);
        BuildPipeline.BuildPlayer(levels.ToArray(), path, BuildTarget.Android, BuildOptions.None);
		
		//@{{ CAUTION!!
		//
		// copy back 
		//
		if(!File.Exists(alipayFile)){
			if(File.Exists(backupAlipay)){
				File.Move(backupAlipay,alipayFile);
			}else{
				throw new System.Exception("Failed!! Please Update SVN to restore : " + alipayFile);
			}
		}
		//@}}
		
		return path;
	}
	
	static void SetLauncherIconSetting(string[] iconPaths)
	{
		string[] original =
		{
#if UNITY_ANDROID
			"Assets/Sprite Atlases/ic_launcher144.png",
			"Assets/Sprite Atlases/ic_launcher72.png",
			"Assets/Sprite Atlases/ic_launcher48.png",
			"Assets/Sprite Atlases/ic_launcher48.png",
		
#elif UNITY_IPHONE
			"Assets/Sprite Atlases/ic_launcher48.png",
			"Assets/Sprite Atlases/ic_launcher48.png",
			"Assets/Sprite Atlases/ic_launcher72.png",
			"Assets/Sprite Atlases/ic_launcher144.png",
#endif
		};	
		
		Texture2D[] iconTexs = new Texture2D[iconPaths.Length];
		for (int i = 0; i < iconPaths.Length; i++)
		{
			iconTexs[i] = AssetDatabase.LoadMainAssetAtPath(iconPaths[i]) as Texture2D;
			
			// copy file to replace
			File.Copy(iconPaths[i], original[i], true);
		}
		
#if UNITY_ANDROID
		// copy it to Android resource ic
		string[] android =
		{
			"Assets/Plugins/Android/res/drawable-xhdpi/ic_launcher.png",
			"Assets/Plugins/Android/res/drawable-hdpi/ic_launcher.png",
			"Assets/Plugins/Android/res/drawable-mdpi/ic_launcher.png",
			"Assets/Plugins/Android/res/drawable-ldpi/ic_launcher.png",
		};
		
		File.Copy(iconPaths[0], android[0], true);
		File.Copy(iconPaths[0], android[1], true);
		File.Copy(iconPaths[1], android[2], true);
		File.Copy(iconPaths[2], android[3], true);
		
		PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, iconTexs);
#elif UNITY_IPHONE
		PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, iconTexs);
#endif
	}
	
	static void BuildXcodeProject(string subSDKFolder)
	{
		string projectPath = Application.dataPath.Substring(0,Application.dataPath.Length - "Assets".Length);
		string locationPath  = projectPath + "ProjectXcode/" + subSDKFolder;
		
		if (!Directory.Exists(locationPath))
			Directory.CreateDirectory(locationPath);

		
		// Prepare all levels
		List<string> levels = new List<string>();
		EditorBuildSettingsScene scene = null;
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			scene = EditorBuildSettings.scenes[i];
			if (scene.enabled)
			{
				Debug.Log("Scene " + scene.path);
				levels.Add(scene.path);
			}
		}
		
        //BuildPipeline.BuildPlayer(levels.ToArray(), locationPath, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
        BuildPipeline.BuildPlayer(levels.ToArray(), locationPath, BuildTarget.iOS, BuildOptions.None);
		
		// post process
		//XCodePostProcess.OnPostProcessBuild(BuildTarget.iPhone,locationPath);
	}
#endregion
}
