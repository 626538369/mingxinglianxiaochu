using UnityEngine;
using System.Collections;

public class GameStatusPublisher : EventManager.Publisher
{
	public static string NAME = "GameStatus";
	public static string GAME_STATE_INVALID = "GameStateInvalid";
	public static string GAME_STATE_INITIAL = "GameStateInitial";
	public static string GAME_STATE_LOGIN = "GameStateLogin";
	public static string GAME_STATE_LOADING = "GameStateLoading";
	public static string GAME_STATE_ROLE_CREATE = "GameStateRoleCreate";
	public static string GAME_STATE_PORT = "GameStatePort";
	public static string GAME_STATE_COPY = "GameStateCopy";
	public static string GAME_STATE_BATTLE = "GameStateBattle";
	public static string GAME_STATE_QUIT = "GameStateQuit";
	
	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyGameStateInvalid()
	{
		base.Notify(GAME_STATE_INVALID);
	}
	
	public void NotifyGameStateInitial()
	{
		base.Notify(GAME_STATE_INITIAL);
	}
	
	public void NotifyGameStateLogin()
	{
		base.Notify(GAME_STATE_LOGIN);
	}
	
	public void NotifyGameStateLoading()
	{
		base.Notify(GAME_STATE_LOADING);
	}
	
	public void NotifyGameStateRoleCreate()
	{
		base.Notify(GAME_STATE_ROLE_CREATE);
	}
	
	public void NotifyGameStatePort()
	{
		base.Notify(GAME_STATE_PORT);
	}
	
	public void NotifyGameStateCopy()
	{
		base.Notify(GAME_STATE_COPY);
	}
	
	public void NotifyGameStateBattle()
	{
		base.Notify(GAME_STATE_BATTLE);
	}
	
	public void NotifyGameStateQuit()
	{
		base.Notify(GAME_STATE_QUIT);
	}
}
