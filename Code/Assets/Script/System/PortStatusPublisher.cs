using UnityEngine;
using System.Collections;

public class PortStatusPublisher : EventManager.Publisher
{
	public static string NAME = "PortStatus";
	public static string EVENT_ENTER_PORT = "EnterPort";
	public static string EVENT_LEAVE_PORT = "LeavePort";
	
	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyEnterPort()
	{
		base.Notify(EVENT_ENTER_PORT);
	}
	
	public void NotifyLeavePort()
	{
		base.Notify(EVENT_LEAVE_PORT);
	}
}
