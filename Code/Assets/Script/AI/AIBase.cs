using UnityEngine;
using System.Collections;

public enum AIStyle
{
	PATH_PATROL,
	CHASE,
}

public class AIBase : MonoBehaviour 
{
	//-------------------------------------
	public GameObject ControlGameObj
	{
		get { return controlGameObj; }
		set { controlGameObj = value; }
	}
	//-------------------------------------
	
	protected virtual void Awake()
	{
		isEnabled = true;
	}
	
	protected virtual void OnDestroy()
	{}
	
	protected virtual void Update()
	{
	}
	
	public virtual void Interrupt()
	{
		isEnabled = false;
	}
	
	public virtual void Restart()
	{
		isEnabled = true;
	}
	
	public virtual void Resume()
	{
		isEnabled = true;
	}
	
	protected bool isEnabled;
	protected GameObject controlGameObj;
}
