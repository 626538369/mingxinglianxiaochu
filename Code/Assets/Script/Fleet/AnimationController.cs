using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationController
{
	public delegate void OnAnimationEventFunction(System.Object data);
	
	/// <summary>
	/// Animation event callback.
	/// </summary>
	public class AnimationEventCallback
	{
		public bool _isTriggered = false;
		public bool _isEndEvent = false;
		public float _time = 0.0f;
		public float _triggerTime = 0.0f;
		public OnAnimationEventFunction _onAnimationEventFunction;
		public System.Object _userData;
	}
	
	/// <summary>
	/// Animation state slot.
	/// </summary>
	public class AnimationStateSlot
	{
		public AnimationStateSlot(AnimationState state)
		{
			_animationState = state;
			Initialize();
		}
		
		public void Initialize()
		{
			if (_animationState == null)
				return;
			
			_time = 0.0f;
			_name = _animationState.name;
			_speed = _animationState.speed;
			_isLoop = _animationState.wrapMode == WrapMode.Loop;
			_isEnd = false;
			
			_animationEventCallbackList = new Dictionary<float, AnimationEventCallback>();
		}
		
		public void Release()
		{
			_animationState = null;
			RemoveAllEventCallbacks();
		}
		
		public void AddEventCallback(float time, AnimationEventCallback callback)
		{
			// Only one key one value
			if (_animationEventCallbackList.ContainsKey(time))
				return;
			
			_animationEventCallbackList[time] = callback;
		}
		
		public void RemoveEventCallback(float time)
		{
			if (!_animationEventCallbackList.ContainsKey(time))
				return;
			
			_animationEventCallbackList.Remove(time);
		}
		
		public void RemoveAllEventCallbacks()
		{
			_animationEventCallbackList.Clear();
		}
		
		public string _name;
		public float _time;
		public float _speed;
		public bool _isLoop;
		public bool _isEnd;
		public AnimationState _animationState;
		
		public Dictionary<float, AnimationEventCallback> _animationEventCallbackList;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="AnimationController"/> class.
	/// </summary>
	/// <param name='am'>
	/// Am.
	/// </param>
	public AnimationController(Animation am)
	{
		_currentAnimationState =  null;
		_currentAnimationClip = null;
		
		_animationComponent = am;
	}
	
	public void Initialize()
	{
		
	}
	
	public void PlayAnimation(string animationName, float speed, bool loop, bool autoStop)
	{
		if (_animationComponent == null)
		{
			return;
		}
		
		if (_animationComponent[animationName] == null)
		{
			return;
		}
		
		AnimationState animState = _animationComponent[animationName];
		// The default layer value, the 0 is the low layer,the top layer is cross first
		animState.layer = 0;
		animState.time = 0.0f;
		animState.weight = 1.0f;
		animState.speed = speed;
		// The Loop animation control
		animState.wrapMode = loop ? WrapMode.Loop : WrapMode.Once;
		
		if (_animationStateSlotList.ContainsKey(animationName))
		{
			_animationStateSlotList.Remove(animationName);
		}
		
		AnimationStateSlot animationStateSlot = new AnimationStateSlot(animState);
		_animationStateSlotList.Add(animationName, animationStateSlot);
		
		_animationComponent.CrossFade(animationName);
		// _animationComponent.Play(animationName);
	}
	
	public void StopAnimation(string name)
	{
		if (_animationComponent == null)
			return;
		
		_animationComponent.Stop(name);
		_animationStateSlotList.Remove(name);
	}
	
	public void StopAnimation()
	{
		if (_animationComponent == null)
			return;
		
		_animationComponent.Stop();
		_animationStateSlotList.Clear();
	}
	
	public void AddAnimationEvent(string animationName, float time, OnAnimationEventFunction callbackFunction)
	{
		if (_animationComponent == null)
			return;
		
		if (_animationComponent[animationName] == null)
			return;
		
//		AnimationEvent animEvent = new AnimationEvent();
//		animEvent.time = time;
//		animEvent.functionName = functionName;
//		_currentAnimationState.clip.AddEvent(animEvent);
		
		// Is same with
		// this.gameObject.SendMessage(animEvent.functionName, animEvent);
		
		if (!_animationStateSlotList.ContainsKey(animationName))
			return;
		
		AnimationStateSlot animStateSlot = _animationStateSlotList[animationName];
		
		// Create new event callback
		AnimationEventCallback callback = new AnimationEventCallback();
		callback._triggerTime = time;
		callback._onAnimationEventFunction = callbackFunction;
		callback._userData = animStateSlot;
		callback._isTriggered = false;
		callback._isEndEvent = false;
		
		// Check is animation end event
		if (_animationComponent[animationName].length == time)
		{
			callback._isEndEvent = true;
		}
		
		// Add the time event into callback list
		animStateSlot.AddEventCallback(callback._triggerTime, callback);
	}
	
	public void RemoveAnimationEvent(string animationName, float time)
	{
		if (_animationComponent == null)
			return;
		
		if (_animationComponent[animationName] == null)
			return;
		
		if (!_animationStateSlotList.ContainsKey(animationName))
			return;
		
		_animationStateSlotList[animationName].RemoveEventCallback(time);
	}
	
	public void RemoveAnimationAllEvents(string animationName)
	{
		if (_animationComponent == null)
			return;
		
		if (_animationComponent[animationName] == null)
			return;
		
		if (!_animationStateSlotList.ContainsKey(animationName))
			return;
		
		_animationStateSlotList[animationName].RemoveAllEventCallbacks();
	}
	
	public void addAnimationFinishEvent(string animationName, OnAnimationEventFunction callbackFunction)
	{
		if (_animationComponent == null)
			return;
		
		if (_animationComponent[animationName] == null)
			return;
		
		AddAnimationEvent(animationName, _animationComponent[animationName].length, callbackFunction);
	}
	
	public void RemoveAnimationFinishEvent(string animationName)
	{
		if (_animationComponent == null)
			return;
		
		if (_animationComponent[animationName] == null)
			return;
		
		RemoveAnimationEvent(animationName, _animationComponent[animationName].length);
	}
	
	public void Update()
	{
		UnityEngine.Profiling.Profiler.BeginSample("AnimationController.Update");
		
		if (_animationComponent != null
		&& _animationStateSlotList.Count != 0){
					
			List<string> tempNameList = new List<string>();
			List<float> tempTimeList = new List<float>();
			
			// Just update current the doing animations
			foreach (AnimationStateSlot animStateSlot in _animationStateSlotList.Values)
			{
				if (_animationComponent[animStateSlot._name] == null)
					continue;
				
				// Time increase
				animStateSlot._time += Time.deltaTime;
				
				// Get the callback list
				foreach (AnimationEventCallback callback in animStateSlot._animationEventCallbackList.Values)
				{
					if (callback._isTriggered)
						continue;
					
					// Early quit the circle
					if (animStateSlot._time < callback._triggerTime)
						break;
					
					// Only animation wrapMode == Once or wrapMode == Clamp, can send onAnimationFinish event
					if (animStateSlot._isLoop)
					{
						// Loop animation has no animation end event
						if (callback._isEndEvent)
							break;
						
						// The animation is End now
						callback._isTriggered = true;
						callback._onAnimationEventFunction(callback._userData);
								
					}
					else
					{
						// Once animation
						callback._isTriggered = true;
						callback._onAnimationEventFunction(callback._userData);
						
						if (callback._isEndEvent)
						{
							animStateSlot._isEnd = true;
							
							// Remove no useful animation cache
							tempNameList.Add(animStateSlot._name);
							break;
						}
						else
						{
							// Remove no useful time event cache
							tempTimeList.Add(callback._triggerTime);
						}
					} // End if (animStateSlot._isLoop)
				} // End foreach single animation event callback
				
				// If animation is at the end time, Clear All Events
				if (animStateSlot._isEnd)
					animStateSlot.RemoveAllEventCallbacks();
				else
				{
					// Remove the dispatched event
					foreach (float time in tempTimeList)
					{
						animStateSlot.RemoveEventCallback(time);
					}
				}
			} // End foreach all current animation state slots
			
			if (tempNameList.Count == 0){
				UnityEngine.Profiling.Profiler.EndSample();
				return;
			}
			
			// Check if is the once animation, and now is end
			foreach (string name in tempNameList)
			{
				_animationStateSlotList.Remove(name);
			}
		}
		
		UnityEngine.Profiling.Profiler.EndSample();		
	}
	
	public void OnAnimationFinishiEvent(AnimationEvent animEvent)
	{
		// animEvent.animationState;
		if (_animationComponent == null)
			return;
	}
	
	public Dictionary<string, AnimationStateSlot> _animationStateSlotList = new Dictionary<string, AnimationStateSlot>();
	
	// Obtain u3d Animation Component
	public Animation _animationComponent;
	public AnimationState _currentAnimationState;
	public AnimationClip _currentAnimationClip;
	
	// one animation property defination
	public string _animationName;
	// >0 forward animation, <0 backward animation
	public float _animationPlaySpeed;
	public bool _isLoop;
	public bool _isAutoStop;
	
	// AnimationEvent class
	// public class AnimationEvent
	//{}
	// u3d have itself AnimationEvent class, but the use technique is ???
}

