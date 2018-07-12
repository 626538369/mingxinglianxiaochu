using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ResAssetBundle;
using System.Text;
using Mono.Xml;

public class AssetPriManager : MonoBehaviour
{
	
	public void Init (string assetPackageFile, string assetPackagePackage, string assetWeigthFile, string assetWeightPackage, GameObject resourceLoader, Manifest manifest)
	{
	/*
	public void Init (ResourceLoader resourceLoader, Manifest manifest)
	{
	*/
		Debug.Log("AssetPriManager Init");
		_initialzed = InializedState.Initializing;
		isStartCoroutine = false;
		
		this._resourceLoaderObj = resourceLoader;
		this._manifest = manifest;
		
		this._assetPackageFile = ToAssetPriPath (assetPackageFile);
		this._assetPackagePackage = assetPackagePackage;
		this._assetWeightFile = ToAssetPriPath (assetWeigthFile);
		this._assetWeightPackage = assetWeightPackage;
		
		ResourceLoader resDl = _resourceLoaderObj.AddComponent<ResourceLoader> ();
		
		_assetPackageLoader = AssetLoaderManager.CreateLoader (_assetPackageFile, _assetPackagePackage);
		_assetWeightLoader = AssetLoaderManager.CreateLoader (_assetWeightFile, _assetWeightPackage);
		
		resDl.AddAssetLoader (_assetPackageLoader);
		resDl.AddAssetLoader (_assetWeightLoader);
		resDl.SetProgressDelegate (InitCallBack, null, true);
		
	}
	
	private void InitCallBack (float progress, System.Object userData)
	{
		Debug.Log("AssetPriManager Init Callback");
		//Debug.Log("PROCESS:"+progress);
		if (progress < 1.0f) {
			return;
		}
		Debug.Log("AssetPriManager Init Callback Over");
		
		//init assetPackage
		TextAsset assetText = _assetPackageLoader.Load () as TextAsset;
		SecurityParser xmlParser = new SecurityParser ();
		xmlParser.LoadXml (assetText.text);
		_assetPackageConfig = new AssetPackageConfig();
		_assetPackageConfig.Load(xmlParser.ToXml ());
		//_assetPackage = _assetPackageConfig.AssetPackages;
		
		//init assetWeight
		assetText = _assetWeightLoader.Load() as TextAsset;
		xmlParser = new SecurityParser ();
		xmlParser.LoadXml (assetText.text);
		_assetWeightConfig = new AssetWeightConfig();
		_assetWeightConfig.Load(xmlParser.ToXml());
		
		int currentMissionNode = PlayerPrefs.GetInt("currentMissionNode" , 0);
		AssetWeigth currentAssetWeigth = _assetWeightConfig.assetWeigthDict[currentMissionNode];
		_assetPackageConfig.nodeValueUpdate(currentAssetWeigth);
		_assetPackage = _assetPackageConfig.AssetPackages;
		
		_initialzed = InializedState.Initialized;
	}
	
	public void AssetPackageDownLoadReStart(int missNode)
	{
		StopCoroutine("AssetPackageDownLoadReStart");
		
		AssetWeigth currentAssetWeigth = _assetWeightConfig.assetWeigthDict[missNode];
		_assetPackageConfig.nodeValueUpdate(currentAssetWeigth);
		_assetPackage = _assetPackageConfig.AssetPackages;
		
		isStartCoroutine = false;
		
	}
	
	void Update ()
	{
		
		if (_initialzed == InializedState.NotInitialized) {
			//this.Init();
			return;
		}
		
		if (_initialzed == InializedState.Initializing) {
			return;
		}
		
		if(_initialzed == InializedState.Initialized)
		{
			if(!isStartCoroutine)
			{
				Debug.Log("StartCoroutine: AssetPackageDownLoad");
				StartCoroutine(AssetPackageDownLoad(_assetPackage));
				isStartCoroutine = true;
			}
		}
		
	}
	
	
	
	private IEnumerator AssetPackageDownLoad(AssetPackage assetPackage)
	{
		Debug.Log("AssetPackageDownLoad" + assetPackage.name);
		if(PlayerPrefs.HasKey(assetPackage.name))
		{
			int version = PlayerPrefs.GetInt(assetPackage.name);
			if(version == assetPackage.version)
			{
				Debug.Log("The Asset Package Has Being DownLoaded ...");
				yield break;
			}
		}
		
		List<string> assetList =  assetPackage.assetList;
		
		/*
		if(assetPackage.name == "")
		{
			
		}
		*/
		
		foreach(string asset in assetList)
		{
			Debug.Log("Ready To DownLoad One Asset ...... + Asset Name:"+asset);
			ResourceManager.Download(AssetPackageDownLoadCallBack, new string[]{asset});
			isDownLoaded = false;
			while(true)
			{
				if(isDownLoaded)
				{
					break;
				}
				yield return 0;
			}
			
		}
		
		List<AssetPackage> assetPackageList =  assetPackage.packageList;
		foreach(AssetPackage childAssetPackage in assetPackageList)
		{
			yield return StartCoroutine(AssetPackageDownLoad(childAssetPackage));

		}
		PlayerPrefs.SetInt(assetPackage.name,assetPackage.version);
	}
	
	private void AssetPackageDownLoadCallBack(float progress, System.Object userData)
	{
		Debug.Log("AssetPackageDownLoadCallBack" + progress);
		if(progress < 1.0f)
		{
			Debug.Log("AssetPackageDownLoadCallBack:"+progress);
			return;
		}
		isDownLoaded = true;
	}
	
	
	
	private string ToAssetPriPath (string assetFile)
	{
		if (assetFile.StartsWith ("assets/")) {
			return PathUtil.UnifyPath (assetFile);
		}
		StringBuilder sb = new StringBuilder ();
		sb.AppendFormat ("assets/{0}", assetFile);
		return PathUtil.UnifyPath (sb.ToString ());
	}
	
	private enum InializedState
	{
		NotInitialized,
		Initializing,
		Initialized,
	}
	
	private GameObject _resourceLoaderObj;
	private AssetWeightConfig _assetWeightConfig;
	private AssetPackageConfig _assetPackageConfig;
	private AssetPackage _assetPackage;
	private InializedState _initialzed = InializedState.NotInitialized;
	
	private AssetLoader _assetPackageLoader;
	private string _assetPackageFile;
	private string _assetPackagePackage;
	
	private AssetLoader _assetWeightLoader;
	private string _assetWeightFile;
	private string _assetWeightPackage;
	
	private Manifest _manifest;
	private bool isDownLoaded = false;
	
	private bool isStartCoroutine = false;
	
	
}
