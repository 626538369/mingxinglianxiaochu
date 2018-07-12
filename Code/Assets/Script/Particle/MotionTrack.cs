using UnityEngine;
using System.Collections;


public abstract class MotionTrack
{
	//--------------------------------------------------
	public object Data
	{
		get { return _mUserData; }
		set { _mUserData = value; }
	}
	
	public Vector3 CurrentPosition
	{
		get { return _mCurrentPosition; }
		set { _mCurrentPosition = value; }
	}
	
	public Vector3 MEulerAngle
	{
		get { return _mEulerAngle; }
		set { _mEulerAngle = value; }
	}
	//--------------------------------------------------
	
	public MotionTrack(System.Object data)
	{
		_mEnabled = true;
		_mUserData = data;
	}
	
	public MotionTrack()
	{
		_mEnabled = true;
		_mUserData = null;
	}
	
	public abstract void OnStart();
	public abstract void OnEnd();
	public abstract void Update();
	
	protected bool _mEnabled;
	protected System.Object _mUserData;
	
	protected Vector3 _mStartPosition;
	protected Vector3 _mEndPosition;
	protected Vector3 _mCurrentPosition;
	protected Vector3 _mEulerAngle;
}

// Parabolic track
public class ParabolicTrack : MotionTrack
{
	public ParabolicTrack(Vector3 startPosition, Vector3 endPosition, float horizontalSpeed, float verticalSpeed)
	{
		_mStartPosition = startPosition;
		_mEndPosition = endPosition;
		_mHorizontalSpeed = horizontalSpeed;
		_mVerticalSpeed = verticalSpeed;
		
		_mCurrentPosition = _mStartPosition;
		
		_mGravityAcceleration = -200.8f;
		_mIsEnd = false;
	}
	
	public ParabolicTrack(Vector3 startPosition, Vector3 endPosition, float missileHorzSpeed, float targetHorzSpeed, float gravityAcceleration)
	{
		_mStartPosition = startPosition;
		_mEndPosition = endPosition;
		
		_mHorizontalSpeed = missileHorzSpeed;
		_mGravityAcceleration = gravityAcceleration;
		
		// Prepare some useful data
		float xDistance = _mEndPosition.x - _mStartPosition.x;
		float runningTime = Mathf.Abs(xDistance) / (missileHorzSpeed + targetHorzSpeed);
		
		// Calculate the vertical up Initial velocity
		// Assume the Y axis value is 200.0f
		// float vertInitialSpeed = maxHeight / (runningTime * 0.5f) - 0.25f * _mGravityAcceleration * runningTime;
		float vertInitialSpeed = Mathf.Abs(0.5f * _mGravityAcceleration * runningTime);
		
		// Calculate the z axis velocity
		float zDistance = _mEndPosition.z - _mStartPosition.z;
		float zAxisSpeed = zDistance / runningTime;
		
		_mHorizontalSpeed = _mEndPosition.x > _mStartPosition.x ? missileHorzSpeed : -missileHorzSpeed;
		_mVerticalSpeed = vertInitialSpeed;
		_mZAxisSpeed = zAxisSpeed;
		
		// Recalculate the end position x value
		_mEndPosition.x = _mStartPosition.x + _mHorizontalSpeed * runningTime;
		
		//_mBeginTime = Time.time;
		_mRunningTime = 0;
		_mCurrentPosition = _mStartPosition;
		
		_mIsEnd = false;
	}
	
	public override void OnStart()
	{
	}
	
	public override void OnEnd()
	{
		if (_mUserData != null)
		{
			MissileL missle = (MissileL)_mUserData;
			missle.OnTouchTarget();
			missle.OnEnd();
		}
	}
	
	public override void Update()
	{
		if (_mIsEnd)
			return;
		
		// First check this frame will be end?
		if(Mathf.Abs(_mEndPosition.x - _mCurrentPosition.x) < Mathf.Abs(_mHorizontalSpeed * Time.deltaTime))
		{
			_mIsEnd = true;
			_mCurrentPosition = _mEndPosition;
			
			this.OnEnd();
			return;
		}
			
		_mCurrentPosition.x += (_mHorizontalSpeed * Time.deltaTime);
		_mCurrentPosition.z += (_mZAxisSpeed * Time.deltaTime);
		
		//_mCurrentPosition.y += (_mVerticalSpeed * Time.deltaTime + _mGravityAcceleration * Time.deltaTime * Time.deltaTime * 0.5f);
		_mRunningTime += Time.deltaTime;
		_mCurrentPosition.y = _mStartPosition.y + (_mVerticalSpeed * _mRunningTime + _mGravityAcceleration * _mRunningTime * _mRunningTime * 0.5f);	
	}
	
	private float _mBeginTime;
	private float _mRunningTime;
	
	// The Horizontal move speed
	public float _mHorizontalSpeed;
	// The Vertical move speed
	public float _mVerticalSpeed;
	public float _mZAxisSpeed;
	
	// The default Gravity Acceleration
	public float _mGravityAcceleration;
	
	public bool _mIsEnd;
}

// Line track
public class LineTrack : MotionTrack
{

	public LineTrack(Vector3 startPosition, Vector3 endPosition, float speed, float targetSpeed)
	{
		_mStartPosition		= startPosition;
		_mEndPosition		= endPosition;
		_mCurrentPosition	= _mStartPosition;
		
		mRunningTime		= (startPosition - endPosition).magnitude / (speed + targetSpeed);
		mRunningTimer		= 0;
		
		OnStart();
	}
	
	public override void OnStart()
	{
	}
	
	public override void OnEnd()
	{
		if (_mUserData != null)
		{
			MissileL missle = (MissileL)_mUserData;
			missle.OnTouchTarget();
			missle.OnEnd();
		}
	}
	
	public override void Update()
	{
	
		// tzz fucked to lerp track...
		if(mRunningTime > 0){
			
			bool tEnd = false;
			
			mRunningTimer += Time.deltaTime;
			if(mRunningTimer > mRunningTime){
				mRunningTimer	= mRunningTime;
				tEnd			= true;
			}
			
			float tDelta = mRunningTimer / mRunningTime;
			
			_mCurrentPosition.x = Mathf.Lerp(_mStartPosition.x,_mEndPosition.x,tDelta);
			_mCurrentPosition.y = Mathf.Lerp(_mStartPosition.y,_mEndPosition.y,tDelta);
			_mCurrentPosition.z = Mathf.Lerp(_mStartPosition.z,_mEndPosition.z,tDelta);	
			
			if(tEnd){
				mRunningTime = 0;
				OnEnd();
			}
		}
		
	}
	
		
	private float mRunningTimer;
	private float mRunningTime;
}

// Line track
public class AirCraftTrack : MotionTrack
{
	public enum TrackState
	{
		TAKE_OFF,
		LINE_FLY,
		LEAVE,
	}
	
	public AirCraftTrack(Vector3 startPosition, Vector3 endPosition, float speed, float targetSpeed)
	{
		// tzz modified for fly
		//
		_mStartPosition = startPosition;
		_mEndPosition = endPosition;
		_mSpeed = speed + targetSpeed;
		
		m_dir		= _mEndPosition - _mStartPosition;
		m_totalTime = m_dir.magnitude / _mSpeed;
		m_dir.Normalize();
		
		_mCurrentPosition = _mStartPosition;
		
		_mIsEnd = false;
		
		OnStart();
		
		_mLeaveTime = 3.0f;
		_mTrackState = TrackState.TAKE_OFF;
		_mTrackState = TrackState.LINE_FLY;
	}

	public override void OnStart()
	{
	}
	
	public void OnTouchTarget()
	{
		if (_mUserData != null)
		{
			MissileL missle = (MissileL)_mUserData;
			missle.OnTouchTarget();
		}
	}
	
	public override void OnEnd()
	{
		if (_mUserData != null)
		{
			MissileL missle = (MissileL)_mUserData;
			missle.OnEnd();
		}
	}
	
	public override void Update()
	{
		m_timer += Time.deltaTime;
		
		switch (_mTrackState)
		{
		case TrackState.TAKE_OFF:
		{
			break;
		}
		case TrackState.LINE_FLY:
		{
			// First check this frame will be end?
			if(m_timer >= m_totalTime + 0.5f)
			{
				OnTouchTarget();
				_mTrackState = TrackState.LEAVE;
			}
			
			_mCurrentPosition += m_dir * Time.deltaTime * _mSpeed; 
						
			break;
		}
		case TrackState.LEAVE:
		{
			if (_mLeaveTime <= 0.0f)
			{
				OnEnd();
				break;
			}
			_mLeaveTime -= Time.deltaTime;
			
			_mCurrentPosition += m_dir * Time.deltaTime * _mSpeed; 
			
			break;
		}
		}
	}
	
	private float _mSpeed;
	private bool _mIsEnd;
	
	private TrackState _mTrackState;
	private float _mLeaveTime;
	
	
	private float m_timer		= 0;
	private float m_totalTime	= 0;
	
	private Vector3 m_dir;
	
}

// Time track
public class TimeTrack : MotionTrack
{
	public TimeTrack(float runTime)
	{	
		_mCurrTime = 0.0f;
		_mRunTime = runTime;
		
		_mIsEnd = false;
		OnStart();
	}
	
	public override void OnStart()
	{
	}
	
	public override void OnEnd()
	{
		if (_mUserData != null)
		{
			MissileL missle = (MissileL)_mUserData;
			missle.OnTouchTarget();
			missle.OnEnd();
		}
	}
	
	public override void Update()
	{
		if (_mIsEnd)
			return;
		
		if (_mCurrTime >= _mRunTime)
		{
			_mIsEnd = true;
			OnEnd();
			return;
		}
		
		_mCurrTime += Time.deltaTime;
	}
	
	private bool _mIsEnd;
	private float _mCurrTime;
	private float _mRunTime;
}

// Guided missile
public class MissileL
{
	//--------------------------------------------------
	public bool IsEnd
	{
		get { return _mIsEnd; }
		set { _mIsEnd = value; }
	}
	
	public bool IsTouchTarget
	{
		get { return _mIsTouchTarget; }
		set { _mIsTouchTarget = value; }
	}
	
	public MotionTrack TrackMode
	{
		get { return _mMotionTrackMode; }
	}
	//--------------------------------------------------
	
	public MissileL(GameObject flyObject, MotionTrack trackMode)
	{
		_mGameObject = flyObject;
		_mMotionTrackMode = trackMode;
		_mGameObject.transform.Rotate(_mMotionTrackMode.MEulerAngle);
		if(_mGameObject.name == "S_TorpedoFly(Clone)")
		{
			Debug.Log("S_TorpedoFly");
		}
		Initialize();
	}
	
	public void Initialize()
	{
		_mIsEnd = false;
		_mMotionTrackMode.Data = this;
	}
	
	public void Release()
	{
		GameObject.DestroyImmediate(_mGameObject);
	}
	
	// Update is called once per frame
	public void Update () {
	
		if (_mIsEnd)
			return;
		
		if (_mMotionTrackMode != null)
		{
			_mMotionTrackMode.Update();
			
			// Transform the GameObject
			_mGameObject.transform.position = _mMotionTrackMode.CurrentPosition;
		}
	}
	
	public void OnStart()
	{}
	
	public void OnTouchTarget()
	{
		_mIsTouchTarget = true;
		_mGameObject.transform.position = _mMotionTrackMode.CurrentPosition;
	}
	
	public void OnEnd()
	{
		_mIsEnd = true;
	}
	
	private MotionTrack _mMotionTrackMode;
	private GameObject _mGameObject;
	
	private bool _mIsTouchTarget;
	private bool _mIsEnd;
}
