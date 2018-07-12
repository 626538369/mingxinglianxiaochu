using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{
	public const string MUSIC_URL_BASE = "http://192.168.1.112/";
	
	//! tzz added for current playing Audio
	public static AudioSource				CurrentPlayingMusicAudio 	= null;
	public static SoundConfig.MusicElement 	CurrentPlayingMusicConfig	= null;
	
	private bool OnAwakeIsPlay = true;
	private bool SoundIsLoop = true;
	
	void Awake()
	{
		_mSceneMusic = null;
		_mSoundObjList = new Dictionary<string, GameObject>();
		_mAudioClipResList = new Dictionary<string, AudioClip>();
	}
	
	void Start()
	{
		// this.Play2DMusic("Sounds/BattleAtlanticBG");
		// this.PlaySoundEffect("Sounds/test");
	}
	
	void OnDestroy()
	{
		if (null != _mSceneMusic)
			Destroy(_mSceneMusic);
		_mSceneMusic = null;
		
		_mSoundObjList.Clear();
		_mAudioClipResList.Clear();
	}
	
	void OnDisable()
	{
		if (null != _mSceneMusic)
			Destroy(_mSceneMusic);
		_mSceneMusic = null;
		
		_mSoundObjList.Clear();
		_mAudioClipResList.Clear();
		
		StopAllCoroutines();
	}
	
	public void PlaySoundEffect(string name)
	{
		AudioSource source = BuildAudioObj(name, false);
		source.playOnAwake = true;
		source.volume = GameDefines.Setting_SoundVol * 0.01f;
		source.Play();
		
		Destroy(source.gameObject, source.clip.length * 1.01f); // a little delay
	}
	
	public AudioSource PlayTaskSoundEffect(string name)
	{
		AudioSource source = BuildAudioObj(name, false);
		source.playOnAwake = true;
		source.volume = GameDefines.Setting_SoundVol * 0.01f;
		source.Play();
		
		Destroy(source.gameObject, source.clip.length * 1.01f); // a little delay
		
		return source;
	}
	
	public void Play2DMusic(string name)
	{
	}
	
	public void Play3DMusic(string name, ref Vector3 worldPos)
	{
		AudioSource source = BuildAudioObj(name, false);
		source.gameObject.transform.position = worldPos;
		source.Play();
		
		Destroy(source.gameObject, source.clip.length * 1.01f); // a little delay
	}
	
	
	
	public void PlayMusic(string name, bool fromNet, bool isStream)
	{
		AudioClip clip = null;
		if (fromNet)
		{
			WWW www = new WWW(MUSIC_URL_BASE + name + ".ogg");
			clip = www.GetAudioClip(false, true, AudioType.OGGVORBIS);
		}
		else
		{
			clip = Resources.Load(name) as AudioClip;
		}
		
		GameObject go = null;
		AudioSource source = null;
		if (_mSoundObjList.TryGetValue(name, out go))
		{
			source = go.GetComponent<AudioSource>() as AudioSource;
		}
		else
		{
			go = new GameObject(name);
			DontDestroyOnLoad(go);
			source = go.AddComponent<AudioSource>() as AudioSource;
			_mSoundObjList.Add(name, go);
		}
		
		source.clip = clip;
		DoPlayMusic(source);
	}
	
	public void CloseMusic(string name)
	{
		GameObject go = null;
		if (_mSoundObjList.TryGetValue(name, out go))
		{
			_mSoundObjList.Remove(name);
			GameObject.Destroy(go);
		}
	}
	
	private IEnumerator DoPlayMusic(AudioSource source)
	{
		while (!source.clip.isReadyToPlay)
		{
			yield return null;
		}
		
		source.Play();
		
		yield break;
	}
	
	
	public void PlaySceneSound(string sceneName,bool isPlay = true,bool isLoop = true)
	{
		OnAwakeIsPlay = isPlay;
		SoundIsLoop = isLoop;
		SoundConfig cfg = Globals.Instance.MDataTableManager.GetConfig<SoundConfig>();
		_mSceneMusicElement = cfg.GetSceneMusicElement(sceneName);
		if (null == _mSceneMusicElement)
		{
			Debug.Log("The sceneName " + sceneName + " cann't find the music config.");
			return;
		}
		
		if (0 == _mSceneMusicElement.usableMusicList.Count)
		{
			Debug.Log("No have the background sound");
			return;
		}
		
		int index = Random.Range(0, _mSceneMusicElement.usableMusicList.Count);
		SoundConfig.MusicElement music = _mSceneMusicElement.usableMusicList[index];
		if (null == music)
		{
		 	return;
		}
		
		if (_mSceneMusic != null &&  "BGM_" + music.name == _mSceneMusic.name)
			return ;
		
		
		// Stop all coroutines running on this behaviour
		StopAllCoroutines();
		
		PlaySceneBGMusic();
		PlaySceneSoundEffect();
		

	}
	
	private void PlaySceneSoundEffect()
	{
		// StopCoroutine("DoPlaySceneBGMusic");
		
		foreach (SoundConfig.MusicElement music in _mSceneMusicElement.usableSoundEffectList)
		{
			StartCoroutine( DoPlaySceneSoundEffect(music) );
		}
	}
	
	private IEnumerator DoPlaySceneSoundEffect(SoundConfig.MusicElement music)
	{
		AudioSource source = BuildAudioObj(music.name, false);
		source.gameObject.name = "S_" + music.name;
		
		source.volume = music.volume * (GameDefines.Setting_SoundVol / 100.0f);
		
		if (music.startDelay > 0)
			yield return new WaitForSeconds(music.startDelay);
		source.Play();
		
		Destroy(source.gameObject, source.clip.length * 1.01f); // a little delay
		
		float delay = source.clip.length;
		if (music.delay.Equals(0.0f))
			delay += 0.2f;
		else
		 	delay += music.delay;
		yield return new WaitForSeconds(delay);
		
		StartCoroutine( DoPlaySceneSoundEffect(music) );
	}
	
	private void PlaySceneBGMusic()
	{
		if (null != _mSceneMusic)
		{
			GameObject.DestroyObject(_mSceneMusic);
			_mSceneMusic = null;
		}
		StopCoroutine("DoPlaySceneBGMusic");
		StartCoroutine("DoPlaySceneBGMusic");
	}
	
	private IEnumerator DoPlaySceneBGMusic()
	{
		Debug.Log("[SoundManager]:Begin DoPlaySceneBGMusic");
		
		// Random.seed = 1000;
		int index = Random.Range(0, _mSceneMusicElement.usableMusicList.Count);
		SoundConfig.MusicElement music = _mSceneMusicElement.usableMusicList[index];
		if (null == music)
		{
			Debug.Log("Cann't find the music " + music.name);
			yield break;
		}
		
		Debug.Log("[SoundManager]:music.startDelay is " + music.startDelay);
		
		if (music.startDelay > 0)
			yield return new WaitForSeconds(music.startDelay);
		
		AudioSource source = BuildAudioObj(music.name, SoundIsLoop);
		source.gameObject.name = "BGM_" + music.name;
		
		_mSceneMusic = source.gameObject;
		while (!source.clip.isReadyToPlay)
		{
			Debug.Log("[SoundManager]:Prepare the sound " + music.name);
			yield return null;
		}
		
		source.priority = 128;
		
		
		source.volume	= music.volume * (GameDefines.Setting_MusicVol / 100.0f);
		//source.volume = 0;
		
		if(OnAwakeIsPlay)
		{
			source.playOnAwake = true;
			source.Play();
		}else
		{
			source.playOnAwake = false;
		}
		
		
		CurrentPlayingMusicAudio = source;
		
		// Debug.Log("[SoundManager]:Begin play the sound " + music.name);
		// Debug.Log("[SoundManager]:source.enabled is " + source.enabled.ToString());
		// Debug.Log("[SoundManager]:source.isPlaying is " + source.isPlaying.ToString());
		// Debug.Log("[SoundManager]:source.loop is " + source.loop.ToString());
		// Debug.Log("[SoundManager]:source.volume is " + source.volume.ToString());
		// Debug.Log("[SoundManager]:source.mute is " + source.mute.ToString());
		// Debug.Log("[SoundManager]:source.pan is " + source.pan.ToString());
		// Debug.Log("[SoundManager]:source.panLevel is " + source.panLevel.ToString());
		// Debug.Log("[SoundManager]:source.pitch is " + source.pitch.ToString());
		// Debug.Log("[SoundManager]:source.priority is " + source.priority.ToString());
		// Debug.Log("[SoundManager]:source.playOnAwake is " + source.playOnAwake.ToString());
		
//		float delay = source.clip.length;
//		if (music.delay.Equals(0.0f))
//			delay += 0.2f;
//		else
//		 	delay += music.delay;
//		yield return new WaitForSeconds(delay);
//		
//		if (null != _mSceneMusic)
//		{
//			GameObject.DestroyObject(_mSceneMusic);
//			_mSceneMusic = null;
//		}
//		
//		PlaySceneBGMusic();
	}
	
	private AudioSource BuildAudioObj(string name, bool loop)
	{
		AudioClip clip = Resources.Load(name) as AudioClip;
		if (null == clip)
		{
			Debug.Log("Cann't search the sound resource " + name);
			return null;
		}
		
		if (!clip.isReadyToPlay)
		{
			Debug.Log("Cann't decompress the sound resource " + name);
		}
		
		clip.name = name;
		
		GameObject go = new GameObject();
		AudioSource source = go.AddComponent<AudioSource>() as AudioSource;

		source.clip = clip;
		source.loop = loop;
		
		return source;
	}
	
	private GameObject _mSceneMusic;
	private SoundConfig.SceneMusicElement _mSceneMusicElement;
	
	private Dictionary<string, GameObject> _mSoundObjList;
	private Dictionary<string, AudioClip> _mAudioClipResList;
	
}
