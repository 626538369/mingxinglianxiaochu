using UnityEngine;
using System.Collections;

using System.IO;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;

public enum GameLanguage
{
	LANGUAGE_CN = 0,
	LANGUAGE_EN = 1,
	LANGUAGE_JP = 2,
	LANGUAGE_OTHER = 3,
}

// public enum CardAttribute
// {
//     LIFE = 0,
//     SEA_ATK,
//     SUB_ATK,
//     DEF,
//     SKILL_ATK,
//     RANGE,
//     HIT_RATE,
//     CRIT_RATE,
//     DODGE_RATE,
// }

public class DataTableManager : MonoBehaviour 
{
	void Awake()
	{
	}
	
	// Use this for initialization
	IEnumerator Start () 
	{
		if (!useAssetBundle)
			yield break;
		// Config files.
		Debug.Log("DataTableManager Start");
		List<string> configList = new List<string>();
		configList.Add("WarshipConfig");
		configList.Add("SkillConfig");
		configList.Add("SkillEffectConfig");
		configList.Add("CharacterConfig");
		configList.Add("PortsBuildingConfig");
		configList.Add("GirlActionandSoundConfig");
		configList.Add("GirlCGConfig");
		//configList.Add("Girl_SkillConfig");
		configList.Add("SeedConfig");
		configList.Add("RoomFlowerpotConfig");
		configList.Add("RoomConfig");
		configList.Add("Task_Daily");
		configList.Add("WordConfig");	
		configList.Add("ErrorCodeConfig");
		configList.Add("SeaAreaConfig");
		configList.Add("ItemConfig");
		configList.Add("NatureAddConfig");
		configList.Add("SubjectConfig");
		
		configList.Add("PushDataConfig");
		configList.Add("PlayerAttributeIconConfig");
		configList.Add("TrainConfig");
		configList.Add("JobConfig");
		configList.Add("JobPlaceConfig");
		configList.Add("TourismConfig");
		configList.Add("SubjectExtraRewardConfig");
		configList.Add("AccessoryConfig");
		configList.Add("TweenGroupConfig");
		configList.Add("ExplorePlaceConfig");
		configList.Add("TaskConfig");
		configList.Add("TaskDialogConfig");
		configList.Add("SoundConfig");
		configList.Add("ConstellationConfig");
		configList.Add("IdentityConfig");
		configList.Add("DateWordConfig");
		configList.Add("DatingPlaceConfig");
		configList.Add("BuildingsExploreConfig");
		configList.Add("BeautyThemeConfig");
		configList.Add("JuryTalk");
		configList.Add("Task_Play");
		configList.Add("TouchGirlConfig");
		//WorldMap//
		configList.Add("Map_Area");
		configList.Add("Map_Citys");
		configList.Add("Map_News");
		configList.Add("PublishConfig");
		configList.Add("MapFlyConfig");
		//Artist//
		configList.Add("Warship_Evolution");
		configList.Add("WarShip_Rank_ExpConfig");
		configList.Add("Artist_SkillConfig");
		foreach (string configName in configList)
		{
			if (!_configWWWs.ContainsKey(configName))
			{				
				Debug.Log("WWW Start Load config is " + configName);
				yield return StartCoroutine(LoadConfigWWW(configName));
			}
		}
		yield break;	
	}
	
	void Dispose()
	{
	}
	
	IEnumerator LoadConfigWWW(string configName)
	{
		WWW www = WWW.LoadFromCacheOrDownload(CharacterGenerator.AssetbundleBaseURL + configName +".assetbundle" ,GameDefines.AssetBundleVersion);
		yield return  www;
		_configWWWs.Add(configName,www);
		yield break;
	}
	
	bool useAssetBundle = false;
	SecurityParser mXmlParser = new SecurityParser();
	
	public  T   GetConfig<T>() where T : ConfigBase
	{	
		if (useAssetBundle)
		{
			System.Type t = typeof(T);
			Debug.Log("GetConfig Load config is " + t.Name);
			if (!_configs.ContainsKey(t.Name))
			{
				_configs[t.Name] = System.Activator.CreateInstance<T>();
				
				
				TextAsset textAsset = _configWWWs[t.Name].assetBundle.LoadAsset(t.Name) as TextAsset;
				mXmlParser.LoadXml(textAsset.text);
				textAsset = null;
				
				_configWWWs[t.Name].Dispose();
				_configs[t.Name].Load(mXmlParser.ToXml());
			}
			
			return (T) _configs[t.Name];
		}
		else
		{
			if (null == _mSystemConfig)
					return null;
			return _mSystemConfig.GetConfig<T>();
		}
		
	}
	

	
	public string GetWordText(int wordID)
	{
		WordConfig config = GetConfig<WordConfig>();
		return config.GetWordByID( wordID, (int)MCurrentLanguage );
	}
	
	
	public string GetWordText(string wordID)
	{
		int id = StrParser.ParseDecInt(wordID, -1);
		return GetWordText(id);
	}
	
	public string GetErrorCodeText(int wordID)
	{
		ErrorCodeConfig config = GetConfig<ErrorCodeConfig>();
		return config.GetWordByID(wordID, (int)MCurrentLanguage);
	}
	
	/*public string GetJuruTalkText(int wordId)
	{
		JuryTalk config = GetConfig<JuryTalk>();
		return config.GetTalkObjectByID();
		
	}*/

 
	// Temporary code, use SystemConfig
	private SystemConfig _mSystemConfig = new SystemConfig();
		
	public GameLanguage MCurrentLanguage = GameLanguage.LANGUAGE_CN;
	
	// System modules.
	protected Dictionary<string, ConfigBase> _configs = new Dictionary<string, ConfigBase>();
	protected Dictionary<string, WWW> _configWWWs = new Dictionary<string, WWW>();
}
