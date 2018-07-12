using UnityEngine;
using System.Collections;

public class CopyStatusPublisher : EventManager.Publisher
{
	public static string NAME = "CopyStatus";
	public static string EVENT_ENTER_COPY = "EnterCopy";
	public static string EVENT_LEAVE_COPY = "LeaveCopy";
	public static string EVENT_NPC_FLEET_COUNT = "NpcFleetCount";
	
	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyEnterCopy()
	{
		base.Notify(EVENT_ENTER_COPY);
	}
	
	public void NotifyLeaveCopy()
	{
		base.Notify(EVENT_LEAVE_COPY);
	}
	
	public void NotifyNPCFleetCount(int count)
	{
		base.Notify(EVENT_NPC_FLEET_COUNT,count);
	}
}
