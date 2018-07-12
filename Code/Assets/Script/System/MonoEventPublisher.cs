using UnityEngine;
using System.Collections;

public class MonoEventPublisher : EventManager.Publisher
{
	public static string NAME = "MonoEvent";
	public static string MONO_AWAKE = "MonoAwake";
	public static string MONO_START = "MonoStart";
	public static string MONO_PAUSE = "MonoPause";
	public static string MONO_FOCUS = "MonoFocus";
	public static string MONO_QUIT = "MonoQuit";

	public override string Name 
	{
		get { return NAME; }
	}
	
	public void NotifyMonoAwake()
	{
		base.Notify(MONO_AWAKE);
	}
	
	public void NotifyMonoStart()
	{
		base.Notify(MONO_START);
	}
	/// <summary>
	/// Notifies the mono pause.
	/// </summary>
	/// <param name='pause'>
	/// Pause.
	/// </param>
	/// <param name='timeSpan'>
	/// Time span. (Millisecond seconds)
	/// </param>
	public void NotifyMonoPause(bool pause,long timeSpan)
	{
		base.Notify(MONO_PAUSE,pause,timeSpan);
	}
	
	public void NotifyMonoFocus(bool focus,long timeSpan)
	{
		base.Notify(MONO_FOCUS,focus,timeSpan);
	}
	
	public void NotifyMonoQuit()
	{
		base.Notify(MONO_QUIT);
	}
}
