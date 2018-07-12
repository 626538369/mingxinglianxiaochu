using UnityEngine;
using System.Collections;

public enum ELoginState
{
	NOT_CONNECT,
	CONNECTING,
	CONNECTED,
	
	LOGINNING,
	LOGIN_FINISHED,
	LOGIN_FAILED,
}

public class LoginStatus : GameStatus
{
	public LoginStatus()
	{}
	
	public override void Initialize()
	{}
	
	public override void Release()
	{}
	
	public void Login()
	{
		// StartCoroutine(DoLogin(GameDefine.GetServer(), GameDefine.GetPort()));
	}
	
	private IEnumerator DoLogin()
	{
		while (!Globals.Instance.MLSNetManager.Connected)
		{
			yield return null;
		}
		
		while (!Globals.Instance.MGSNetManager.Connected)
		{
			yield return null;
		}
		
		// Enter other GameState
	}
	
	public override void Pause()
	{}
	
	public override void Resume()
	{}
	
	public override void Update()
	{}
	
	private ELoginState _mLoginState;
}
