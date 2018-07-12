using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class InterludeContent
{
	public InterludeContent(InterludeStatus mgr) 
	{ 
		controller = mgr; 
	}
	
	public abstract void DoContent();
	
	protected InterludeStatus controller;
}

public class InterludePutIntoPort : InterludeContent
{
	public InterludePutIntoPort(InterludeStatus mgr) : base(mgr)
	{
	}
	
	public override void DoContent()
	{
		controller.IsInterludeEnd = true;
	}
}

public class InterludeBattleEnd : InterludeContent
{
	public InterludeBattleEnd(InterludeStatus mgr) : base(mgr)
	{
		
	}
	
	public override void DoContent()
	{
		controller.IsInterludeEnd = true;
	}
}

public class InterludeStatus : GameStatus, IFingerEventListener
{
	//------------------------------------------------
	public InterludeContent InterludeContent
	{
		get { return interludeContent; }
		set { interludeContent = value; }
	}
	
	public bool IsInterludeEnd
	{
		get { return isEnd; }
		set 
		{
			isEnd = value;
			
			// Notify
		}
	}
	//------------------------------------------------
	
	public override void Initialize()
	{
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		SetFingerEventActive(false);
	}
	
	public override void Release()
	{}
	
	public override void Pause()
	{}
	
	public override void Resume()
	{}
	
	public override void Update()
	{}
	
	#region Handle Finger Event
	private bool _mIsFingerEventActive = false;
	public bool IsFingerEventActive()
	{
		return _mIsFingerEventActive;
	}
	
	public void SetFingerEventActive(bool active)
	{
		_mIsFingerEventActive = active;
	}
	public bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	
   	public bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		return true;
	}
	
	public bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerStationaryBeginEvent (int fingerIndex, Vector2 fingerPos)
	{
		return false;
	}
	public bool OnFingerStationaryEndEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}
	public bool OnFingerStationaryEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
		return false;
	}

	public bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;	
	}
	
	public bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{	
		return true;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		return true;
	}
	public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{
		return true;
	}
	public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		return true;
	}
	public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		return true;
	}
	#endregion
	
	private InterludeContent interludeContent = null;
	private bool isEnd = false;
}
