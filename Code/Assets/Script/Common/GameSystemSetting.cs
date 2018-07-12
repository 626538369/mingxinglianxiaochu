using UnityEngine;
using System.Collections;
using System.IO;

public class GameSystemSetting
{
	//----------------------------------------------------------
	public static VersionSetting VersionSetting
	{
		get { return verSetting; }
	}
	
	// public static ImageSetting ImageSetting
	// {
	// 	get { return imageSetting; }
	// }
	// 
	// public static SoundSetting SoundSetting
	// {
	// 	get { return soundSetting; }
	// }
	// 
	// public static FunctionSetting FunctionSetting
	// {
	// 	get { return funcSetting; }
	// }
	//----------------------------------------------------------
	
	public static void InitConfigs()
	{
		PersistentPath = GameSystemInfo.PersistentDataPath + "/Config/";
		if (!Directory.Exists(PersistentPath))
		{
			DirectoryInfo dirInfo = Directory.CreateDirectory(PersistentPath);
			Debug.Log(dirInfo);
		}
		else
		{
			Debug.Log(PersistentPath);
		}
		
		// VersionSetting
		ParseVersionSetting();
		// ParseImageSetting();
	}
	
	private static void ParseVersionSetting()
	{
		verSetting = new VersionSetting();
		verSetting.LoadSettings();
	}
	
	// private static void ParseImageSetting()
	// {
	// 	imageSetting = new ImageSetting();
	// 	imageSetting.LoadSettings();
	// 	imageSetting.SaveSettings();
	// }
	
	public static string PersistentPath;
	
	private static VersionSetting verSetting;	
	// private static ImageSetting imageSetting;	
	// private static SoundSetting soundSetting;	
	// private static FunctionSetting funcSetting;	
}

public abstract class PreferSetting
{
	public string FileName
	{
		get;
		set;
	}
	
	public string FullPath
	{
		get;
		set;
	}
	
	public abstract void LoadSettings();
	public abstract void SaveSettings();
}

public class VersionSetting : PreferSetting
{
	public string SectionVal
	{
		get;
		set;
	}
	
	public string Version
	{
		get;
		set;
	}
	
	public VersionSetting()
	{
		FileName = "Version";
		FullPath = "GameSetting/" + FileName;
		SectionVal = "Version";
	}
	
	public override void LoadSettings()
	{
		UnityEngine.Profiling.Profiler.BeginSample("VersionSetting::LoadSettings");
		
		TextAsset text = Resources.Load(FullPath) as TextAsset;
		if (null == text)
		{
			Debug.Log("[VersionSetting:]Cannot find the file " + FullPath + " in the Resources directory.");
			return;
		}
		
		IniFileIO.OpenBuffer(text.bytes);
		Version = IniFileIO.ReadString(SectionVal, "Version");
		IniFileIO.Close();
		
		UnityEngine.Profiling.Profiler.EndSample();
	}
	
	public override void SaveSettings(){}
}

// public class ImageSetting : PreferSetting
// {
// 	public bool IsWindowed;
// 	public string ResolutionRatio;
// 	public string Frequency;
// 	public string CameraControl;
// 	
// 	public ImageSetting()
// 	{
// 		FileName = "ImageSetting";
// 		FullPath = GameSystemSetting.PersistentPath + FileName + ".ini";
// 	}
// 	
// 	public override void LoadSettings()
// 	{
// 		CheckExist(FullPath);
// 		
// 		// Open the fullPath file
// 		if (IniFileIO.OpenFile(FullPath, false))
// 		{
// 			// Read the setting.
// 			IniFileIO.Close();
// 		}
// 		else
// 		{
// 			return;
// 		}
// 	}
// 	
// 	public override void SaveSettings()
// 	{
// 		if (!IniFileIO.OpenFile(FullPath, true))
// 		{
// 			IniFileIO.Close();
// 			return;
// 		}
// 		
// 		IniFileIO.BeginWrite(false);
// 		
// 		// IniFileIO.Write("1111111");
// 		// IniFileIO.Write("1111111");
// 		// IniFileIO.Write("Section", "Key1", 111);
// 		// IniFileIO.Write("Section", "Key1", 111);
// 		
// 		IniFileIO.EndWrite();
// 		IniFileIO.Close();
// 	}
// 	
// 	private void CheckExist(string fullPath)
// 	{
// 		bool exist = File.Exists(fullPath);
// 		if (!exist)
// 		{
// 			string localPath = FileName;
// 			localPath = "GameSetting/" + localPath;
// 			TextAsset text = Resources.Load(localPath) as TextAsset;
// 			if (null == text)
// 			{
// 				Debug.Log("[ImageSetting:]Cannot find the file " + localPath + " in the Resources directory.");
// 				return;
// 			}
// 			
// 			if (IniFileIO.CreateFile(fullPath))
// 			{
// 				IniFileIO.Write(text.bytes);
// 				// IniFileIO.Write(text.text);
// 				IniFileIO.Close();
// 			}
// 		}
// 	}
// }
// 
// public class SoundSetting
// {
// 	public bool IsOpened;
// 	public bool IsOpenSoundEffect;
// 	public bool IsOpenMusic;
// 	
// 	public float SoundEffectVolume;
// 	public float MusicVolume;
// }
// 
// public class FunctionSetting
// {
// 	
// }