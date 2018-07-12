using UnityEngine;
using System.Collections;

public abstract class GameStatus
{
	public string MGameStatusName
	{
		get { return _gameStatusName; }
		// set;
	}
	
	public System.Type MGameStatusType
	{
		get { return _gameStatusType; }
		// set;
	}
	
	public bool MEnabled
	{
		get { return _enabled; }
		set { _enabled = value; }
	}
	
	public abstract void Initialize();
	public abstract void Release();
	
	public abstract void Pause();
	public abstract void Resume();
	
	public abstract void Update();
	
	protected string _gameStatusName;
	protected System.Type _gameStatusType;
	protected bool _enabled;
	protected bool _mPaused;
}
