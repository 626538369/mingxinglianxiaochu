using UnityEngine;
using System.Collections;

public class BattleStatusPublisher : EventManager.Publisher
{
	public static string NAME = "BattleStatus";
	public static string EVENT_ENTER_BATTLE = "EnterBattle";
	public static string EVENT_ON_STEP = "OnStep";
	public static string EVENT_BATTLE_DELAY = "BattleDelay";
	public static string EVENT_LEAVE_BATTLE = "LeaveBattle";
	
	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyEnterBattle()
	{
		base.Notify(EVENT_ENTER_BATTLE);
	}
	
	public void NotifyOnStep()
	{
		base.Notify(EVENT_ON_STEP);
	}
	
	public void NotifyBattleDelay()
	{
		base.Notify(EVENT_BATTLE_DELAY);
	}
	
	public void NotifyLeaveBattle(GameData.BattleGameData.BattleResult result)
	{
		base.Notify(EVENT_LEAVE_BATTLE, result);
	}
	
	
}
