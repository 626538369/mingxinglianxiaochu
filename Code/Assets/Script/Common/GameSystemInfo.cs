using UnityEngine;
using System.Collections;

public class GameSystemInfo 
{
	public static string PersistentDataPath
	{
		get { return Application.persistentDataPath; }
	}
	
	public static string DataPath
	{
		get { return Application.dataPath; }
	}
	
	public static string CacheDataPath
	{
		get { return Application.temporaryCachePath; }
	}
	
	
	public static bool RunInBackground
	{
		get { return Application.runInBackground; }
	}
	
	public static string DeviceName
	{
		get { return SystemInfo.deviceName; }
	}
	
	public static string DeviceCpuModel
	{
		get { return SystemInfo.deviceModel; }
	}
	
	public static SystemLanguage DeciveLanguage
	{
		get { return Application.systemLanguage; }
	}
	
	/// <summary>
	/// Gets or sets the target frame rate.
	/// </summary>
	/// <value>
	/// The target frame rate.
	/// </value>
	public static int TargetFrameRate
	{
		get { return Application.targetFrameRate; }
		set { Application.targetFrameRate = value; }
	}
	
	// Use this for initialization
	public static void CollectSystemInfo() 
	{
		Debug.Log("SystemInfo.deviceName: " + SystemInfo.deviceName);
		Debug.Log("SystemInfo.deviceModel: " + SystemInfo.deviceModel);
		Debug.Log("SystemInfo.deviceType: " + SystemInfo.deviceType);
		Debug.Log("SystemInfo.deviceUniqueIdentifier: " + SystemInfo.deviceUniqueIdentifier);
		
		Debug.Log("SystemInfo.graphicsDeviceID: " + SystemInfo.graphicsDeviceID);
		Debug.Log("SystemInfo.graphicsDeviceName: " + SystemInfo.graphicsDeviceName);
		Debug.Log("SystemInfo.graphicsDeviceVendor: " + SystemInfo.graphicsDeviceVendor);
		Debug.Log("SystemInfo.graphicsDeviceVendorID: " + SystemInfo.graphicsDeviceVendorID);
		Debug.Log("SystemInfo.graphicsDeviceVersion: " + SystemInfo.graphicsDeviceVersion);
		Debug.Log("SystemInfo.graphicsMemorySize: " + SystemInfo.graphicsMemorySize);
		Debug.Log("SystemInfo.graphicsPixelFillrate: " + SystemInfo.graphicsPixelFillrate);
		Debug.Log("SystemInfo.graphicsShaderLevel: " + SystemInfo.graphicsShaderLevel);
		
		Debug.Log("SystemInfo.supportedRenderTargetCount: " + SystemInfo.supportedRenderTargetCount);
		Debug.Log("SystemInfo.supportsAccelerometer: " + SystemInfo.supportsAccelerometer);
		Debug.Log("SystemInfo.supportsGyroscope: " + SystemInfo.supportsGyroscope);
		Debug.Log("SystemInfo.supportsImageEffects: " + SystemInfo.supportsImageEffects);
		Debug.Log("SystemInfo.supportsLocationService: " + SystemInfo.supportsLocationService);
		Debug.Log("SystemInfo.SupportsRenderTextureFormat[ARGB32]: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32));
		Debug.Log("SystemInfo.SupportsRenderTextureFormat[ARGB1555]: " + SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB1555));
		Debug.Log("SystemInfo.supportsRenderTextures: " + SystemInfo.supportsRenderTextures);
		Debug.Log("SystemInfo.supportsShadows: " + SystemInfo.supportsShadows);
		Debug.Log("SystemInfo.supportsVertexPrograms: " + SystemInfo.supportsVertexPrograms);
		Debug.Log("SystemInfo.supportsVibration: " + SystemInfo.supportsVibration);
		
		Debug.Log("SystemInfo.operatingSystem: " + SystemInfo.operatingSystem);
		Debug.Log("SystemInfo.processorCount: " + SystemInfo.processorCount);
		Debug.Log("SystemInfo.processorType: " + SystemInfo.processorType);
		Debug.Log("SystemInfo.systemMemorySize: " + SystemInfo.systemMemorySize);
	}
}
