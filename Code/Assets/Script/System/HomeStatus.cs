using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeStatus 
	: GameStatus
	, IFingerEventListener
{
	
	public CharacterCustomizeOne characterCustomizeOne;  
	public Camera camera;
	Scene3D scene3d;
	public int GirID = -1;
	public GameObject SceneHomeObj;
	private static readonly string[] InnerPartList = {"TAXA","TAXE","zuijiaoL","TAXB"};
//	
//	private static readonly string[] ControllerName = {"","FreeChest","",   "",    ""    ,""    ,"FreeHead"};
//	enum GirlBody
//	{
//		CHEST_TAXB = 1,//胸 //
//		HANUCH_TAXC = 2,// 臀部//
//		FOOT_TAXG = 4,// 脚//
//		LEG_TAXE = 5,//腿 //
//		HEAD_TAXA = 6,//头 //	
//	}
	//-------------------------------------------------------
	public const float BUILDING_TRIGGER_THRESHOLD = 5.0f;
	public bool bIsOpenControll = false;
	
	
	public bool bHorizontal = true;
	public bool bFinger = true;
	public bool bClick = true;
	public ParticleSystem particle;
	//-------------------------------------------------------
	public HomeStatus()
	{

	}
	
	public override void Initialize()
	{
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
		GameObject girobj = GameObject.Find("Room_sushe01");
		SceneHomeObj = girobj;
		scene3d = girobj.GetComponent<Scene3D>();// as Scene3D;
		characterCustomizeOne = scene3d.characterCustomizeOne;		
		Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();	
		foreach (GirlData girlData in dicWarShipData.Values)
		{
//			if(girlData.BasicData.LogicID == 1217002000)
//			{
//				
//				break;
//			}
		}	
		characterCustomizeOne.changeCharacterAnimationController("General_Idle");
		camera = scene3d.camera;
		camera.enabled = true;
		particle = scene3d.particle;
		particle.Stop();		
	}
		
	/// <summary>
	/// The aim building delta position.
	/// </summary>
	public static readonly Vector3 AimBuildingDeltaPos	= new Vector3(0,41,-34.39f);
	
	/// <summary>
	/// The aim building delta eular.
	/// </summary>
	public static readonly Vector3 AimBuildingDeltaEular = new Vector3(50,0,0);
	
	/// <summary>
	/// The default camera eular.
	/// </summary>
	public static readonly Vector3 DefaultCameraEular   = new Vector3(10,153f,0);
	
	
	public static readonly float zoomSpeed = 0.5f;
    //public float zoomAmount = 0;
    public static readonly float minZoomAmount = 30f;
    public static readonly float maxZoomAmount = 65f;
	
	public static readonly float minZoomLimite = 3.5f;
	public static readonly float maxZoomLimite = 50.0f;
	

		
	public override void Release()
	{
		//camera.enabled = false;
		this.SetFingerEventActive(false);
		Globals.Instance.MFingerEvent.Remove3DEventListener(this);	
		
	}
	
	public override void Pause()
	{}
	
	public override void Resume()
	{}
	
	public override void Update()
	{

	}

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
	private int lastDownFingerIdx = -1;
	private Vector2 lastDownFingerPos;
	private float lastDownTime = 0;
	public virtual bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
			
		lastDownFingerIdx = fingerIndex;
		lastDownTime = Time.deltaTime;
		lastDownFingerPos = fingerPos;
		_mDownPosition = fingerPos;
		
		_currentFingerIndex = fingerIndex;
		_startTouchPosition = fingerPos;

		return true;
	}
	
   	public virtual bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{	
		if(Globals.Instance.MSceneManager.mMainCamera.cullingMask == 0){
			return true;
		}
		float distance = Vector2.Distance(fingerPos, _mDownPosition);
		if (distance > BUILDING_TRIGGER_THRESHOLD)
			return true;
		
		if(false  == bClick)
			return false;
		GameObject hitObject = SceneQueryUtil.PickObject(camera,fingerPos, TagMaskDefine.GFAN_ACTOR);
		if (null != hitObject)
		{
			Animator animator = characterCustomizeOne.getCharacterAnimator();
			AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		}
			
		hitObject = SceneQueryUtil.PickObject(camera,fingerPos, TagMaskDefine.GFAN_NPC);
		if (null != hitObject){}
		return true;
	}
	protected int GetPartIndex(string name)
	{
		for(int i = 0;i < InnerPartList.Length; ++i)
		{
			if(name == InnerPartList[i])
				return i + 1;
		}
		return -1;
	}
	public virtual bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		if (_currentFingerIndex != -1)
			return false;
		
		_currentFingerIndex = fingerIndex;
		_startTouchPosition = fingerPos;
		
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

	public virtual bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		return false;	
	}
	
	public bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		// Reset finger flag
		_currentFingerIndex = -1;
		_startTouchPosition = fingerPos;

		return false;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{	
		_isPitching = true;
		scene3d.orbitCameraControl.IsPitching = true;
		Debug.Log("OnFingerPinchBegin");
		return true;
	}
	
	/**
	 * tzz added for scale camera in PortStatus and BattleStatus
	 */ 
	public static void OnFingerPinchMove_imple(float delta){

		return;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		if (_isPitching)
		{
			scene3d.orbitCameraControl.pinchCamera(delta);
		}
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		_isPitching = false;
		scene3d.orbitCameraControl.IsPitching = false;
		Debug.Log("OnFingerPinchEnd");
		return true;
	}
	#endregion
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
	
	
	private float _moveSpeed = 0.2f;
	private int _currentFingerIndex = -1;
	
	protected Vector2 _mDownPosition = Vector2.zero;
	protected Vector2 _startTouchPosition = Vector2.zero;
	
	public bool _isPitching = false;

		
	/// <summary>
	/// tzz added for 
	/// The enter port done event.
	/// </summary>
	public iTween.EventDelegate	EnterPortDoneEvent = null;
	
	private bool mIsCameraMoveDefaultState = false;
	
}
