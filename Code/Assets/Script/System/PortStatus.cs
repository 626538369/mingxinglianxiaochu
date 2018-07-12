using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortStatus 
	: GameStatus
	, IFingerEventListener
{
	//-------------------------------------------------------
	public const float BUILDING_TRIGGER_THRESHOLD = 5.0f;
	
	//-------------------------------------------------------
	
	Vector2 minPortPos,maxPortPos;
	private void InitMinMaxPortPos()
	{
		MSeaAreaData = Globals.Instance.MGameDataManager.MCurrentSeaAreaData;
		PortData portData = MSeaAreaData.MPortData;
		if(portData.PortID == 1600000001)//Liverpool
		{
			minPortPos = new Vector2 (-100,-230);
			maxPortPos = new Vector2(120,20);
		}
		else if(portData.PortID == 1600000002)//Port_Plymouth
		{
			minPortPos = new Vector2 (-130,-190);
			maxPortPos = new Vector2(200,15);
		}
		else if(portData.PortID == 1600000003)//Tema
		{
			minPortPos = new Vector2 (-100,-220);
			maxPortPos = new Vector2(150,15);
		}
		else if(portData.PortID == 1600000004)//NewYork
		{
			minPortPos = new Vector2 (-100,-230);
			maxPortPos = new Vector2(120,20);
		}
		else if(portData.PortID == 1600000005)//Kiel
		{
			minPortPos = new Vector2 (-85,-230);
			maxPortPos = new Vector2(105,20);
		}
		else if(portData.PortID == 1600000006)//Taranto
		{
			minPortPos = new Vector2 (-110,-230);
			maxPortPos = new Vector2(160,10);
		}
		else if(portData.PortID == 1600000007)//Narvik
		{
			minPortPos = new Vector2 (-130,-190);
			maxPortPos = new Vector2(200,15);
		}
		else if(portData.PortID == 1600000008)//Brest
		{
			minPortPos = new Vector2 (-90,-220);
			maxPortPos = new Vector2(100,20);
		}
		else if(portData.PortID == 1600000009)//RioDeJaneiro
		{
			minPortPos = new Vector2 (-100,-250);
			maxPortPos = new Vector2(190,10);
		}
		else if(portData.PortID == 1600000010)//Lisbon
		{
			minPortPos = new Vector2 (-90,-220);
			maxPortPos = new Vector2(100,20);
		}
		else if(portData.PortID == 1600000011)//Casablanca
		{
			minPortPos = new Vector2 (-110,-230);
			maxPortPos = new Vector2(160,10);
		}
		else if(portData.PortID == 1600000012)//Caracas
		{
			minPortPos = new Vector2 (-100,-250);
			maxPortPos = new Vector2(190,10);
		}
		else if(portData.PortID == 1600000013)//Reykjavik
		{
			minPortPos = new Vector2 (-100,-200);
			maxPortPos = new Vector2(130,10);
		}		
		else if(portData.PortID == 1600000014)//Sihanouk
		{
			minPortPos = new Vector2 (-100,-210);
			maxPortPos = new Vector2(150,10);
		}		
		else
		{
			minPortPos = new Vector2 (-90,-200);
			maxPortPos = new Vector2(80,-15);
		}
		
	}
	public OrbitCamera orbitCamera;
	Scene3D scene3d;
	public GameObject SceneHomeObj;
	public CharacterCustomizeOne characterCustomizeOne; 
//	public CharacterCustomizeOne characterCustomizeDog;  
	public Camera camera;
	public ParticleSystem particle;
	
	public PortStatus()
	{
		_mPublisher = new PortStatusPublisher();
	}
	
	public override void Initialize()
	{
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
		
		
		GameObject girobj = GameObject.Find("Room_sushe01");
		if (girobj == null)
			girobj = GameObject.Find("Room_sushe02");
		SceneHomeObj = girobj.transform.parent.gameObject;
		scene3d = girobj.GetComponent<Scene3D>();// as Scene3D;
		characterCustomizeOne = scene3d.characterCustomizeOne;		
		PlayerData playerData =  Globals.Instance.MGameDataManager.MActorData;	

		characterCustomizeOne.generageCharacterFormPlayerData(playerData);
		characterCustomizeOne.changeCharacterAnimationController("General_Idle");
		camera = Globals.Instance.MSceneManager.mMainCamera;
		
//		NPCConfig npcConfig = Globals.Instance.MDataTableManager.GetConfig<NPCConfig>();
//		NPCConfig.NPCObject npcObject ;
//		npcConfig.GetNPCObject(12001,out npcObject);
//		scene3d.characterCustomizeDog.generateCharacterFromConfig(npcObject.NPCGender,"D0101");
//		scene3d.characterCustomizeDog.ChangePart(playerData.PetInfo.itemId);
//		scene3d.characterCustomizeDog.changeCharacterAnimationController("Dog_Move");
//		characterCustomizeDog = scene3d.characterCustomizeDog;
//		
//		Object rigibody = Resources.Load("Common/RigiBody",typeof(Object)) as Object;
//		GameObject rigibodyGameObj = GameObject.Instantiate(rigibody) as GameObject;
//		rigibodyGameObj.transform.parent = characterCustomizeOne.transform;	
//		rigibodyGameObj.transform.localPosition = new Vector3(0f,0.7f,-0.07f);
//		rigibodyGameObj.transform.localEulerAngles = Vector3.zero;
//		rigibodyGameObj.transform.localScale = new Vector3(0.43f,1.0f,0.42f);
//		
//		Rigidbody rigibody1 = rigibodyGameObj.GetComponent<Rigidbody>() as Rigidbody;	
//		rigibody1.freezeRotation = true;
//		rigibody1.constraints = RigidbodyConstraints.FreezeAll;
//		
//		Object btTrigger = Resources.Load("Common/BattleTrigger",typeof(Object)) as Object;
//		GameObject btTriggerGameObj = GameObject.Instantiate(btTrigger) as GameObject;
//		btTriggerGameObj.transform.parent = scene3d.characterCustomizeDog.transform;
//		btTriggerGameObj.transform.localPosition = new Vector3(0.01f,0.19f,0.11f);
//		btTriggerGameObj.transform.localEulerAngles = Vector3.zero;
//		BattleTrigger trigger = btTriggerGameObj.GetComponent<BattleTrigger>() as BattleTrigger;
//		trigger.Radius = 0.28f;
//		trigger.TriggerEnterEvents += ImpactDealWith;
		
		particle = scene3d.particle;
		if (particle != null)
			particle.Stop();	
		//InitPortBuildings();
		InitMinMaxPortPos();
		
		_mPublisher.NotifyEnterPort();
		
		// 
		Globals.Instance.MTeachManager.NewTeachEnterPort();
		
		// tzz added for GUINewCardRetreiveBlueprintBtn clicked
		if(EnterPortDoneEvent != null){
			EnterPortDoneEvent();
			EnterPortDoneEvent = null;
		}
		
		Globals.Instance.MSceneManager.mMainCamera.enabled = true;
		Globals.Instance.MSceneManager.mTaskCamera.enabled = false;
		orbitCamera = Globals.Instance.MSceneManager.mMainCamera.transform.parent.GetComponent<OrbitCamera>();
		
//		int teachStep = Globals.Instance.MTeachManager.NewGetTeachStep("x04");
//		if (Globals.Instance.MTeachManager.IsOpenTeach && teachStep < TeachManager.TeachFinishedValue)
//		{
//			NGUITools.SetActive(characterCustomizeDog.gameObject,false);
//		}
//		else
//		{
//			NGUITools.SetActive(characterCustomizeDog.gameObject,true);
//		}
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
	public static readonly Vector3 DefaultCameraEular   = new Vector3(8.7f,164f,0);
	
	/// <summary>
	/// The default camera position when go into port
	/// </summary>
	public static readonly Vector3 DefaultCameraPosition =new Vector3(9900f,43f,238f);
	
	
	/// <summary>
	/// The default height of the camera.
	/// </summary>
	public static readonly float 	DefaultCameraHeight = 43f;
	
	/// <summary>
	/// The default camera FOV
	/// </summary>
	public static readonly float 	DefaultCameraFOV	= 50;
	
	public static readonly float zoomSpeed = 0.5f;
    //public float zoomAmount = 0;
    public static readonly float minZoomAmount = 30f;
    public static readonly float maxZoomAmount = 65f;
	
	public static readonly float minZoomLimite = 3.5f;
	public static readonly float maxZoomLimite = 50.0f;
	
	/// <summary>
	/// camera move to default position and FOV
	/// </summary>
	public void MoveCameraDefault(bool relativePosition,iTween.EventDelegate dele = null){
		
		
	}
	
	/// <summary>
	/// Mains the camera move to that building
	/// </summary>
	/// <param name='building'>
	/// Building.
	/// </param>
	/// <param name='complete'>
	/// Complete.
	/// </param>
	public void MainCameraMoveTo(GameObject building,iTween.EventDelegate complete){
		// LuShang provide the axes
		MainCameraMoveTo(building.transform.position + AimBuildingDeltaPos,AimBuildingDeltaEular,complete);
	}
	
	/// <summary>
	/// Mains the camera move to.
	/// </summary>
	/// <param name='pos'>
	/// Position.
	/// </param>
	/// <param name='rotate'>
	/// Rotate.
	/// </param>
	/// <param name='complete'>
	/// Complete.
	/// </param>
	public void MainCameraMoveTo(Vector3 pos,Vector3 rotate,iTween.EventDelegate complete){
		iTween.MoveTo(Globals.Instance.MSceneManager.mMainCamera.gameObject, iTween.Hash("position",pos , "easetype", iTween.EaseType.easeInOutQuart),null,complete);
		iTween.RotateTo(Globals.Instance.MSceneManager.mMainCamera.gameObject, iTween.Hash("rotation",rotate, "easetype", iTween.EaseType.easeInOutQuart));
	}
		
	public override void Release()
	{
		this.SetFingerEventActive(false);
		Globals.Instance.MFingerEvent.Remove3DEventListener(this);
		
		_mHoldBuildingList.Clear();
		Globals.Instance.MNpcManager.RemoveAllBuilding();
		
		_mPublisher.NotifyLeavePort();
		Globals.Instance.MSceneManager.mMainCamera.enabled = false;
	}
	
	public override void Pause()
	{
		if (SceneHomeObj == null)
			return;
		SceneHomeObj.SetActive(false);
		orbitCamera.enabled = false;
		NGUITools.SetActive(characterCustomizeOne.gameObject,false);
//		NGUITools.SetActive(scene3d.characterCustomizeDog.gameObject,false);
	}
	
	public override void Resume()
	{
		if (SceneHomeObj == null)
			return;
		SceneHomeObj.SetActive(true);
		orbitCamera.enabled = true;
		NGUITools.SetActive(characterCustomizeOne.gameObject,true);		
//		NGUITools.SetActive(scene3d.characterCustomizeDog.gameObject,true);
//		int teachStep = Globals.Instance.MTeachManager.NewGetTeachStep("x04");
//		if (Globals.Instance.MTeachManager.IsOpenTeach && teachStep < TeachManager.TeachFinishedValue)
//		{
//			NGUITools.SetActive(characterCustomizeDog.gameObject,false);
//		}
		if (particle != null)
			particle.Stop();
	}
	
	public override void Update()
	{}
	
	public void NotifyBuildingCreate(int buildingID)
	{
	}
	
	public void NotifyBuildingDestroy(int buildingID)
	{
		_mHoldBuildingList.Remove(buildingID);
		Globals.Instance.MNpcManager.RemoveBuilding(buildingID);
	}
	
	public void AddBuilding(int id, Building build)
	{
		_mHoldBuildingList[id] = build;
	}
	
	public void RemoveBuilding(int id)
	{
		_mHoldBuildingList.Remove(id);
	}
	
	public Dictionary<int, Building> GetHoldBuildingList()
	{
		return _mHoldBuildingList;
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
		if(Time.deltaTime - lastDownTime <1
			&& lastDownFingerIdx == fingerIndex 
			&& Mathf.Abs(lastDownFingerPos.x - fingerPos.x)<3
			&& Mathf.Abs(lastDownFingerPos.y - fingerPos.y)<3)
		{
			MoveCameraDefault(true);
		}
		
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
				
		// tzz added 
		// main camera is disabled, check GUIManager.EnableDisableMainCamera for detail
		if(Globals.Instance.MSceneManager.mMainCamera.cullingMask == 0){
			return true;
		}
		
		float distance = Vector2.Distance(fingerPos, _mDownPosition);
		if (distance > BUILDING_TRIGGER_THRESHOLD)
			return true;
		
		GameObject hitObject = SceneQueryUtil.PickObject(camera,fingerPos, TagMaskDefine.GFAN_NPC);
		if (null != hitObject)
		{
			Globals.Instance.MTeachManager.NewBuildingClickedEvent(hitObject.name);
			if(!Globals.Instance.MNpcManager.InteractWithPickObject(hitObject))
			{
//				AIPathPatrol DogAI = scene3d.characterCustomizeDog.gameObject.GetComponent<AIPathPatrol>();
//				DogAI.OnClickEvent(1);
			}
			
			return true;
		}
		
		hitObject = SceneQueryUtil.PickObject(camera,fingerPos, TagMaskDefine.GFAN_ACTOR);
		if (null != hitObject)
		{
			Globals.Instance.MTeachManager.NewBuildingClickedEvent(hitObject.name);
			if(!Globals.Instance.MNpcManager.InteractWithPickObject(hitObject))
			{
//				AIPathPatrol ActorAI = characterCustomizeOne.gameObject.GetComponent<AIPathPatrol>();
//				ActorAI.OnClickEvent(0);
			}
			
			return true;
		}

		
		return true;
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
		return true;
	}
	
	/**
	 * tzz added for scale camera in PortStatus and BattleStatus
	 */ 
	public static void OnFingerPinchMove_imple(float delta){
//		Vector3 t_dir = Globals.Instance.MSceneManager.mMainCamera.transform.forward.normalized;
//		Vector3 t_newPosition = Globals.Instance.MSceneManager.mMainCamera.transform.localPosition + t_dir * delta * 0.4f;
//		
//		if(t_newPosition.y < maxZoomAmount && t_newPosition.y > minZoomAmount){
//			Globals.Instance.MSceneManager.mMainCamera.transform.localPosition = t_newPosition;
//		}
		return;
		delta=delta*-0.1f;
		float zoomAmount=delta + Globals.Instance.MSceneManager.mMainCamera.fov;
		zoomAmount = Mathf.Clamp( zoomAmount, minZoomAmount, maxZoomAmount );
		Globals.Instance.MSceneManager.mMainCamera.fov = zoomAmount;
	}
	
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		return false;
		if (!_isPitching)
			return true;
		
		OnFingerPinchMove_imple(delta);
		
		if(!HelpUtil.FloatEquals(Globals.Instance.MSceneManager.mMainCamera.transform.localPosition.y,DefaultCameraHeight)
		&& !mIsCameraMoveDefaultState){
			mIsCameraMoveDefaultState = true;
			
			MoveCameraDefault(true,delegate() {
				mIsCameraMoveDefaultState = false;
			});
		}
		
		return true;
	}
	
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		_isPitching = false;
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
	
	
	
	
	private void InitPortData()
	{
		
	}
	
	private void InitPortBuildings()
	{
		MSeaAreaData = Globals.Instance.MGameDataManager.MCurrentSeaAreaData;
		PortData portData = MSeaAreaData.MPortData;
		foreach (BuildingData data in portData.BuildingDataList.Values)
		{
			_mHoldBuildingList.Add(data.LogicID, InstantiateBuilding(data));
		}
		
		// Play building change effect
		if (portData.BuildingChangeData.IsBuildingChange)
		{
			Building building = null;
			int logicID = portData.BuildingChangeData.CreateBuildingLogicID;
			if (-1 != logicID)
			{
				_mHoldBuildingList.TryGetValue(logicID, out building);
				if (null == building)
					return;
				
				Vector3 worldPos = building.U3DGameObject.transform.position;
				MainCameraMoveTo(building.U3DGameObject, delegate() 
				{
					Globals.Instance.MEffectManager.CreateBuildingChangeEffect(worldPos, true, null);
					
					string wordText = Globals.Instance.MDataTableManager.GetWordText(22000001);
					wordText = string.Format(wordText, 08, 22, building.Property.Name, portData.BasicData.PortName);
					EZ3DItem ezItem = Globals.Instance.M3DItemManager.Create3DSimpleText(building.U3DGameObject, wordText,0);
					// destory item delay 2 second
					Globals.Instance.M3DItemManager.DestroyEZ3DItem(ezItem.gameObject,2.0f);
				});
			}
			
			logicID = portData.BuildingChangeData.DestroyBuildingLogicID;
			if (-1 != logicID)
			{
				PortsBuildingConfig cfg = Globals.Instance.MDataTableManager.GetConfig<PortsBuildingConfig>();
				
				PortBuildingElement element = null;
				bool isExist = cfg.GetPortBuildingElement(portData.PortID, out element);
				if (!isExist)
					return;
				
				BuildingElement bldElement = element.GetBuildingElement(logicID);
				if (null == bldElement)
					return;
				
				Vector3 worldPos = HelpUtil.GetSplitVector3(bldElement._buildingPostion);
				MainCameraMoveTo(worldPos + AimBuildingDeltaPos, AimBuildingDeltaEular, delegate()
				{
					Globals.Instance.MEffectManager.CreateBuildingChangeEffect(worldPos, false, null);
					
					string buildName = Globals.Instance.MDataTableManager.GetWordText(bldElement._buildingNameID);
					string wordText = Globals.Instance.MDataTableManager.GetWordText(22000002);
					wordText = string.Format(wordText, 08, 22, buildName, portData.BasicData.PortName);
					EZ3DItem ezItem = Globals.Instance.M3DItemManager.Create3DSimpleText(worldPos, wordText,0);
					
					Globals.Instance.M3DItemManager.DestroyEZ3DItem(ezItem.gameObject,2.0f);
					// tzz fucked 
					// who create who destory,stuip fuck!
					//GameObject.Destroy(ezItem.gameObject, 2.0f);
				}
				);
			}
		}
		if(Globals.Instance.MNpcManager.mbDatingMapPlay)
		{
			Globals.Instance.MNpcManager.mbDatingMapPlay = false;
			Globals.Instance.MNpcManager.MapMoveAnimation();
		}
	}
	
	public Building InstantiateBuilding(BuildingData data)
	{
		Building buildingInst = 
			Globals.Instance.MNpcManager.CreateBuilding(data);
		
		return buildingInst;
	}
	
	
//	// 碰撞处理---- //
//	private void ImpactDealWith(GameObject first, GameObject other)  
//	{
//		TweenPosition firstTween = characterCustomizeOne.gameObject.GetComponent<TweenPosition>();
//		if(firstTween != null)
//		{
//			AIPathPatrol FirstAi = characterCustomizeOne.gameObject.GetComponent<AIPathPatrol>();
//			FirstAi.GetRandomWayPoint();
//		}
//		
//		TweenPosition otherTween = scene3d.characterCustomizeDog.gameObject.GetComponent<TweenPosition>();
//		if(otherTween != null)
//		{
//			AIPathPatrol OtherAi = scene3d.characterCustomizeDog.gameObject.GetComponent<AIPathPatrol>();
//			OtherAi.GetRandomWayPoint();
//		}
//	}
	
	
	protected PortStatusPublisher _mPublisher;
	
	protected SeaAreaData MSeaAreaData = null;
	protected Dictionary<int, Building> _mHoldBuildingList = new Dictionary<int, Building>();
	
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
