using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Security;

public class SoundConfig : ConfigBase
{
	public class SceneMusicElement
	{
		public string sceneName;
		public List<MusicElement> usableMusicList = new List<MusicElement>();
		public List<MusicElement> usableSoundEffectList = new List<MusicElement>();
	}
	
	public class MusicElement
	{
		public string name;
		public float startDelay;
		public float delay;
		public float volume;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Sounds")
			return false;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "LoadingMusic")
				{
					SceneMusicElement obj = null;
					if (!LoadSceneMusicElement(childrenElement, out obj))
						continue;
					
					_mSceneMusicElement.Add(obj.sceneName, obj);
				}
				else if (childrenElement.Tag == "SceneMusic")
				{
					SceneMusicElement obj = null;
					if (!LoadSceneMusicElement(childrenElement, out obj))
						continue;
					
					_mSceneMusicElement.Add(obj.sceneName, obj);
				}
			}
			
			return true;
		}
		return false;
	}
	
	public SceneMusicElement GetSceneMusicElement(string name)
	{
		SceneMusicElement element = null;
		_mSceneMusicElement.TryGetValue(name, out element);
		
		return element;
	}
	
	private bool LoadSceneMusicElement(SecurityElement element, out SceneMusicElement obj)
	{
		obj = new SceneMusicElement();
		
		string attribute = element.Attribute("Name");
		if (attribute != null)
			obj.sceneName = attribute;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Music")
				{
					MusicElement obj1 = null;
					if (!LoadMusicElement(childrenElement, out obj1))
						continue;
					
					obj.usableMusicList.Add(obj1);
				}
				else if (childrenElement.Tag == "SoundEffect")
				{
					MusicElement obj1 = null;
					if (!LoadMusicElement(childrenElement, out obj1))
						continue;
					
					obj.usableSoundEffectList.Add(obj1);
				}
			}
		}
		
		return true;
	}
	
	private bool LoadMusicElement(SecurityElement element, out MusicElement obj)
	{
		obj = new MusicElement();
		
		string attribute = element.Attribute("Name");
		if (attribute != null)
			obj.name = attribute;
		
		attribute = element.Attribute("StartDelayTime");
		if (attribute != null)
			obj.startDelay = StrParser.ParseFloat(attribute, 0.0f);
		
		attribute = element.Attribute("DelayTime");
		if (attribute != null)
			obj.delay = StrParser.ParseFloat(attribute, 0.0f);
		
		attribute = element.Attribute("Volume");
		if (attribute != null)
			obj.volume = StrParser.ParseFloat(attribute, 100.0f);
		
		return true;
	}
	
	private Dictionary<string, SceneMusicElement> _mSceneMusicElement = new Dictionary<string, SceneMusicElement>();
}
