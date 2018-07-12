using UnityEngine;
using System.Collections;

public class ObjMoveControl : MonoBehaviour 
{
	private Vector3 mDestPosition;
	private float mTime;
	public bool ControlEnabled
	{
		get;
		set;
	}
	
    public bool Stoped
	{
		get;
		set;
	}
	
	public Vector3 MoveSpeed
	{
		get;
		set;
	}
	
	
	void Awake()
	{
		ControlEnabled = true;
		MoveSpeed = Vector3.one;
		Stoped = false;
		mTime = 1.0f;
	}
	
	public void MoveTo(Vector3 dest, iTween.EventDelegate updateDel = null, iTween.EventDelegate completeDel = null)
	{
		if (!ControlEnabled)
			return;
		
		mDestPosition = dest;
				
		//Vector3 dist = dest - transform.position;
		//float time = Mathf.Abs(dist.x / MoveSpeed.x);
		//time = Mathf.Max(time, Mathf.Abs(dist.y / MoveSpeed.y));
		// time = Mathf.Max(time, Mathf.Abs(dist.z / MoveSpeed.z));
		// time = 2.0f;
		
		iTween.MoveTo(gameObject,iTween.Hash("position",dest,"time",mTime,"easetype", iTween.EaseType.linear), updateDel, completeDel);
	}
	
	public void Stop()
	{
		iTween.Stop(gameObject);
		Stoped = true;
	}
	
	public Vector3 getDestPosition()
	{
		return mDestPosition;
	}
	
	public void setTime(float time)
	{
		mTime = time;
	}
}
