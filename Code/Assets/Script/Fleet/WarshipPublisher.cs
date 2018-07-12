using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WarshipPublisher : EventManager.Publisher
{	
	public static string NAME = "Warship";
	
	public static string EVENT_ATTACK_START = "AttackStart";
	public static string EVENT_ATTACK_END = "AttackEnd";
	public static string EVENT_BE_ATTACKED = "BeAttacked";
	public static string EVENT_CREATE = "Create";
	public static string EVENT_DEATH = "Death";
	
	public override string Name
    {
        get { return NAME; }
    }
	
	public void NotifyCreate(params object[] args)
	{
		base.Notify(EVENT_CREATE, args);
	}
	
	public void NotifyDeath(params object[] args)
	{
		base.Notify(EVENT_DEATH, args);
	}
	
	public void NotifyAttackStart(params object[] args)
	{
		base.Notify(EVENT_ATTACK_START, args);
	}
	
	public void NotifyBeAttacked(params object[] args)
	{
		base.Notify(EVENT_BE_ATTACKED, args);
	}
	
	public void NotifyAttackEnd(params object[] args)
	{
		base.Notify(EVENT_ATTACK_END, args);
	}
	
	public void DispathEvent(string name, params object[] args)
	{
		base.Notify(name, args);
	}
}