using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RepeateEventManager : MonoBehaviour 
{
	public enum EventArgsType
	{
		TIMING,
		NETWORK,
	}
	
	/// <summary>
	/// Event arguments.
	/// </summary>
	public class EventArgs
	{
		public EventArgs(string module, string name)
		{
			Module = module;
			Name = name;
			LimitTime = 2.0f;
			ArgsType = EventArgsType.TIMING;
			IsEnabled = true; // Default is enabled
			CurrTime = 0.0f;
		}
		
		public EventArgs(string module, string name, float limitTime, RepeateEventManager.EventArgsType type)
		{
			Module = module;
			Name = name;
			LimitTime = limitTime;
			ArgsType = type;
			
			IsEnabled = true;
			CurrTime = 0.0f;
		}
		
		public EventArgsType ArgsType;
		public bool IsEnabled;
		
		public string Name;
		public string Module;
		public float CurrTime;
		public float LimitTime;
	}
	
	/// <summary>
	/// Event arguments list.
	/// </summary>
	public class EventArgsList
	{
		//---------------------------------------------
		public EventArgsType ArgsType
		{
			get { return _mArgsType; }
		}
		//---------------------------------------------
		
		public EventArgsList()
		{
			_mEventArgsList = new Dictionary<string, EventArgs>();
		}
		
		~EventArgsList()
		{
			Clear();
		}
		
		public void Add(EventArgs args)
		{
			_mArgsType = args.ArgsType;
			_mEventArgsList.Add(args.Name, args);
		}
		
		public void Remove(EventArgs args)
		{
			_mEventArgsList.Remove(args.Name);
		}
		
		public void Clear()
		{
			_mEventArgsList.Clear();
		}
		
		public void Update()
		{
			foreach (EventArgs args in _mEventArgsList.Values)
			{
				args.CurrTime += Time.deltaTime;
				if (args.CurrTime >= args.LimitTime)
				{
					args.IsEnabled = true;
					args.CurrTime = 0.0f;
				}
			}
		}
		
		public void SetEnabled(string eventName, bool enabled)
		{
			EventArgs args = null;
			if (_mEventArgsList.TryGetValue(eventName, out args))
			{
				args.IsEnabled = enabled;
			}
		}
		
		public bool IsEnabled(string eventName)
		{
			EventArgs args = null;
			if (_mEventArgsList.TryGetValue(eventName, out args))
			{
				// When check the enabled and the enabled is true, set it false
				if (args.IsEnabled)
				{
					args.IsEnabled = false;
					return true;
				}
				
				return args.IsEnabled;
			}
			
			return false;
		}
		
		private EventArgsType _mArgsType;
		private Dictionary<string, EventArgs> _mEventArgsList;
	}
	
	void Awake()
	{
		_mModuleEventArgsList = new Dictionary<string, EventArgsList>();
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void OnDestroy()
	{
		foreach (EventArgsList list in _mModuleEventArgsList.Values)
		{
			list.Clear();
		}
		_mModuleEventArgsList.Clear();
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (EventArgsList list in _mModuleEventArgsList.Values)
		{
			if (list.ArgsType == EventArgsType.TIMING)
				list.Update();
		}
	}
	
	public void AddEventArgs(EventArgs args)
	{
		EventArgsList list = null;
		if ( _mModuleEventArgsList.TryGetValue(args.Module, out list) )
		{
			list.Add(args);
		}
		else
		{
			list = new EventArgsList();
			list.Add(args);
			_mModuleEventArgsList.Add(args.Module, list);
		}
		
	}
	
	public void RemoveEventArgs(EventArgs args)
	{
		EventArgsList list = null;
		if ( _mModuleEventArgsList.TryGetValue(args.Module, out list) )
		{
			list.Remove(args);
		}
	}
	
	public void ClearModule(string module)
	{
		EventArgsList list = null;
		if ( _mModuleEventArgsList.TryGetValue(module, out list) )
		{
			list.Clear();
		}
	}
	
	public void SetEnabled(string module, string eventName, bool enabled)
	{
		EventArgsList list = null;
		if ( _mModuleEventArgsList.TryGetValue(module, out list) )
		{
			list.SetEnabled(eventName, enabled);
		}
	}
	
	public bool IsEnabled(string module, string eventName)
	{
		EventArgsList list = null;
		if ( _mModuleEventArgsList.TryGetValue(module, out list) )
		{
			return list.IsEnabled(eventName);
		}
		
		Debug.Log("Please register the event first...");
		return false;
	}
	
	public Dictionary<string, EventArgsList > _mModuleEventArgsList;
}
