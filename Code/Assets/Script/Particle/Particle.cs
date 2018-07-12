using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleEffect
{
	public float MLength
	{
		get { return _length; }
	}
	
	public float MTime
	{
		get { return _time; }
	}
	
	public float MDelayTime
	{
		get { return _delay; }
		set { _delay = value; }
	}
	
	public float MDurationTime
	{
		get { return _duration; }
		set { _duration = value; }
	}
	
	public ParticleEffect(string name, string resourceName)
	{
		_name = name;
		_resourceName = resourceName;
	}
	
	public ParticleEffect(string resourceName)
	{
		_name = string.Empty;
		_resourceName = resourceName;
	}
	
	public void Initialze()
	{
		_thisGameObject = GameObject.Instantiate(Resources.Load(_resourceName), Vector3.zero, Quaternion.identity) as GameObject;
		
		if (_name != string.Empty)
			_thisGameObject.name = _name;
		
		_particleSystem = _thisGameObject.GetComponent<ParticleSystem>() as ParticleSystem;
		_particleEmitter = _thisGameObject.GetComponent<ParticleEmitter>() as ParticleEmitter;
		_particleAnimator = _thisGameObject.GetComponent<ParticleAnimator>() as ParticleAnimator;
		_particleRenderer = _thisGameObject.GetComponent<ParticleRenderer>() as ParticleRenderer;
		
		_time = 0.0f;
		CalculateLength(out _length);
	}
	
	public void Release()
	{
		GameObject.Destroy(_thisGameObject);
		
		_thisGameObject = null;
	}
	
	public void Update()
	{
		if (!_isAlive)
			return;
		
		if (!_isPlaying)
			return;
		
		_time += Time.deltaTime;
	 	_isAlive = CalculateIsAlive();
		
		if (_time >= _delay)
		{
			if (!_isPlaying)
				DoPlay();
		}
		
		if (_time >= _duration)
		{
			_isAlive = false;
		}
	}
	
	public void DoPlay()
	{
		if (_particleSystem != null)
		{
			_particleSystem.Play(true);
		}
		
		if (_particleEmitter != null)
		{
			if (_particleEmitter.GetComponent<ParticleSystem>() != null)
				_particleEmitter.GetComponent<ParticleSystem>().Play(true);
		}
		
		_isPlaying = true;
	}
	
	public void DoPause()
	{
		if (_particleSystem != null)
		{
			_particleSystem.Pause(true);
		}
		
		if (_particleEmitter != null)
		{
			if (_particleEmitter.GetComponent<ParticleSystem>() != null)
				_particleEmitter.GetComponent<ParticleSystem>().Pause(true);
		}
		
		_isPlaying = false;
	}
	
	public void Attach(Transform parent, Vector3 offsetPosition, Quaternion offsetRotation)
	{
		if (null == _thisGameObject)
			return;
		
		if (null != parent)
		{
			_thisGameObject.transform.parent = parent;
			_thisGameObject.transform.localPosition = offsetPosition;
			_thisGameObject.transform.localRotation = offsetRotation;
		}
	}
	
	public void Detach()
	{
		if (null == _thisGameObject)
			return;
		
		_thisGameObject.transform.parent = null;
	}
	
	private void CalculateLength(out float length)
	{
		length = -1.0f;
		if (_particleSystem != null)
		{
			length = _particleSystem.duration;
		}
			
		if (_particleEmitter != null)
		{
			if (_particleEmitter.GetComponent<ParticleSystem>() != null)
			{
				length = length > _particleEmitter.GetComponent<ParticleSystem>().duration ? length 
					: _particleEmitter.GetComponent<ParticleSystem>().duration;
			}
		}
	}
	
	private bool CalculateIsAlive()
	{
		bool result = false;
		if (_particleSystem != null)
		{
			result = _particleSystem.IsAlive();
		}
		
		if (_particleEmitter != null)
		{
			if (_particleEmitter.GetComponent<ParticleSystem>() != null)
			{
				result = result && _particleSystem.IsAlive();
			}
		}
		
		return result;
	}
	
	public GameObject _thisGameObject;
	
	public string _name;
	public string _resourceName;
	
	// In the Unity3D, the Component is Unique
	// ParticleSystem Component
	public ParticleSystem _particleSystem;
	// ParticleEmitter Component
	public ParticleEmitter _particleEmitter;
	// ParticleAnimator Component
	public ParticleAnimator _particleAnimator;
	// ParticleRenderer Component
	public ParticleRenderer _particleRenderer;
	
	public float _time;
	public float _length;
	public float _delay;
	public float _duration;
	
	public bool _isLoop;
	public bool _isAlive;
	public bool _isPlaying;
}
