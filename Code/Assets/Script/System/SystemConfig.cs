using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;


public class SystemConfig : SystemModule
{
	public override bool Initialize()
	{
		// 启动游戏全部加载Config改为用到的时候再加载 by hxl 20121107
		
		return true;
	}
	
	public override void Dispose()
	{
	}
	
	public override void Start(System.Object startInfo)
	{
	}
	
	public override void Update()
	{
	}
	
	public override void OnGUI()
	{
	}
	
	public T GetConfig<T>() where T : ConfigBase
	{
		System.Type t = typeof(T);
		
		if (!_configs.ContainsKey(t.Name))
		{
			if(!AddConfig<T>(GameDefines.ConfigPath + "/" + t.Name)){
				return null;
			}
		}

		return (T)(_configs[t.Name]);
	}
	
	protected bool AddConfig<T>(string assetFile) where T : ConfigBase
	{
		System.Type t = typeof(T);

		if (_configs.ContainsKey(t.Name))
			return false;
		
		_configs[t.Name] = System.Activator.CreateInstance<T>();

		
		return _configs[t.Name].Load(LoadConfig(assetFile));
	}
	
	
	protected SecurityElement LoadConfig(string assetFile)
	{
		try{
			
			SecurityParser xmlParser = new SecurityParser(); 
			TextAsset textAsset = Resources.Load(assetFile) as TextAsset;
			xmlParser.LoadXml(textAsset.text);
			textAsset = null;
			
			return xmlParser.ToXml();
			
		}catch(System.Exception e){
			Debug.LogError("Load " + assetFile +" config error:" + e.Message);
			throw e;
		}
	}
	

	
	// System modules.
	protected Dictionary<string, ConfigBase> _configs = new Dictionary<string, ConfigBase>();
}
