using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;  
//using cn.sharesdk.unity3d;

public class GUIDormitory : GUIWindowForm 
{
	float width ;
	
	Vector3 pos1;
	Vector3 pos2 ;
	public Material mat;
	
	public UITexture Texture0;
	public UITexture Texture1;
	public UITexture Texture2;
	public UITexture Texture3;
	
	public LineRenderer Line0;
	public LineRenderer Line1;
	public LineRenderer Line2;
	public LineRenderer Line3;
	
	//摄像机位置控制参数//
	float mLeftAndRight = 150.0f;//左右可以偏移的量//
	float mUpMax = 50.0f;//向上可以偏移的量//
	float mDownMax = 150.0f;//向下可以偏移的量//
	
	//转动是否到位的标示；//
	private  Vector3 mLastAngle; 
	
	private long mGirlID;// not logic
	
	private OrbitCamera mOrbitCamera;
	private GameObject SceneHomeObj;
	//feature
	public UIToggle KissCheckbox;

	//distance
	private  float distance = 50.0f;
	private float  LocalZ;
	//private const float  hight = 200.0f;
	//private float  LocalY;
	public bool bIsFeature = false;
    //endfeature
	
	//controll
	private float cameraControllRotSidex;
	private float cameraControllRotSideCurx;
	
	private float cameraControllRotSidey;
	private float cameraControllRotSideCury;
	
	private float cameraControllRotSidez;
	private float cameraControllRotSideCurz;
	
	private float cameraControllPosX;
	private float cameraControllCurPosX;
	
	private float cameraControllPosY;
	private float cameraControllCurPosY;
	
	private float cameraControllPosZ;
	private float cameraControllCurPosZ;
	
	
	
	public Camera MyCamera;
	private Transform ControllTransform;
	private Vector3 mIniPos;
	private Vector3 mWorldIniPos;
	private float mLimitMove = 30.0f;
	private Vector3 mIniRot = new Vector3();
	
	public Transform target;
	private Transform mHead;
	private Transform mLeg;
	private Transform mMouse;
	private Transform mChest;
	private Transform[] hips;
	
	private Transform mPart;
	
	
	public UIImageButton ImageExitBtn;
	//public CharacterCustomizeOne characterCustomizeOne; 
	
	private static Texture2D screenShot = null;
	
	//StartFrame
	public GameObject StartFrameObj;
	public GameObject StartFSleepObj;
	public GameObject StartFStartobj;
	public GameObject StratFTouchGroupObj;
	public  UIGrid StartFUiGrid;
	//public 	UIDraggablePanel StratFDraggablePanel;
	public GameObject StartFImageInstance;
	public CharacterCustomizeOne characterCustomizeOne = new CharacterCustomizeOne();  
	
	public UIButton RefuseButton;
	
	public  WealthGroup wealthgroup;
	public UITexture GirlPic;
	enum MyImageBrn
	{
		START = -1,
		GIRLLIST,
		CHANGECLOTHES,
		BATHROOM,
		LEVELUP,
		CAMERA,
		GIFT,
		SLEEP,
		ICEBOX,
		POTTING,
		INTERACT,	
		SLEEPALONE,
		SEEEPDOUBLE,
		CHAT,
		INTERACTClose,
		END,
		
	}
	enum RoomFuncEnum 
	{	
		INTERACTIVE = 1,//"交互",//		
		BEDSINGLE = 2,//"单人床"),//	
		BEDDOUBLE = 3,//"双人床"),//		
		ICEBOX = 4,//"冰箱"),//		
		BATHROOM = 5,//"浴室"),//		
		PHOTO = 6,//"照相");//
		Flowerpot = 7,//"盆栽");//

	}
	public UIImageButton []DorImageBten;
	
	//
	public UIImageButton InteractiveDisableBtn = null;
	public UIImageButton SLEEPDisableBtn;
	

	public UIImageButton IceboxDisableBtn;
	public UIImageButton BathroomDisableBtn = null;
	public UIImageButton PhotoDisableBtn;
	public UIImageButton FlowerpotDisableBtn;
	
//	public UILabel InteractiveDisableLabel;
//	public UILabel BedSingleDisableLabel;
//	public UILabel BedDoubleDisableLabel;
//	public UILabel IceboxDisableLabel;
//	public UILabel BathroomDisableLabel = null;
//	public UILabel PhotoDisableLabel;

	
	//girllistframe
	public GameObject  GirllistFObj;
	private GirlData mGirldata;
	public UIButton GirlListFNoBtn;
	public GirlInviteSlot girlInviteSlot;
	
	//touchgroup
	
	//Camera
	
	enum Direction
	{
		UP = 1,
		DOWN,
		LEFT,
		RIGHT,	
	}
	class CurrentState
	{
		public Direction dir;
		public bool pressed = false;
	}
	private CurrentState mCurrentState = new CurrentState();
	
	public GameObject CameraFramobj;
	public UIImageButton ReturnToStartBtn;
	public UIImageButton CameraBtn;
	public UIImageButton ChangePossBtn;
	public UIImageButton ChangeDisPlay;
	
    public UIImageButton SaveBtn;
	public UIImageButton ShareBtn;
	public UIImageButton ReturnToCameraBtn;
	
	public UIImageButton ButtonLeft;
	public UIImageButton ButtonRight;
	public UIImageButton ButtonUp;
	public UIImageButton ButtonDown;
	public UIImageButton ButtonReset;
	
	public UIImageButton ButtonLeft2;
	public UIImageButton ButtonRight2;
	public UIImageButton ButtonUp2;
	public UIImageButton ButtonDown2;
	public UIImageButton ButtonReset2;
	
	
	public GameObject CameraFramobj2;
	public UIImageButton ReturnToStartBtn2;
	public UIImageButton CameraBtn2;
	public UIImageButton ChangePossBtn2;
	public UIImageButton ChangeDisPlay2;
	
    public UIImageButton SaveBtn2;
	public UIImageButton ShareBtn2;
	public UIImageButton ReturnToCameraBtn2;
	
	public UITexture mCurrenTexture;
	public UITexture mCurrenTexture2;
	private Texture2D texture2d;
	
	
	private int mPosIndex = 1;
	private int mMaxosIndex = 3;
	public UILabel PosIndexLabel;
	public UILabel PosIndexLabel2;
	
	public WealthGroup gWealthGroup;
	
	//LevelUpFrame
	
	public GameObject LevelUpNeedsFrame;
	public UIImageButton LevelUpBtn;
	public GameObject LevelUpScucessFrame;
	public UIButton ExitScucessFrameBtn;
	public UILabel SuccessLabel;
	
	
	public UITexture []GoodsTexture;
	public UILabel  []GoodsLabel;
	public UILabel  []GoodsNumLabel;
	public UIImageButton ExitLevelUpFrameBtn;
	
	public GameObject LevelUpObj;
	public GameObject AllItem;
	public UITexture LevelBackGround;
	public UISprite LevelItem;
	public UILabel CurrentLevelLabel;
	
	private int ItemNum;
	




	//GiftFrame
	public GameObject GiftFrameObj;
	public UILabel HoursLabel;
	public UILabel MinuteLabel;
	public UILabel SecondLabel;
	private long mGiftCurrentTime;
	private long mGiftNextTime;
	
	public UISprite EffectsSprite;
	
	//交互特效//
	public GameObject IntimacyObj;
	private Camera mUIcamera;
	public GameObject PartileDianjiObj;
	private Vector3 mMousePos = Vector3.zero;
	private int mPartID = -1;
	
	
	//Bed
	
	
	//IceBox
	public GameObject GameObjectTalk;
	public UILabel TalkLabel;
	public UIButton TalkBtn;
	private List<string> mTalkList = new List<string>();
	
	//public  GameObject  ButtonInvisible;
	

	
	//-------------------------------------------------
	protected override void Awake()
	{		
		if(!Application.isPlaying || null == Globals.Instance.MGUIManager) return;
	
		base.Awake();
	
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			
			if(-1 != home.GirID)
			{
				mGirldata = Globals.Instance.MGameDataManager.MActorData.GetGirlDataByLogicID(home.GirID);
				mGirlID = mGirldata.roleCardId;
				home.GirID = -1;
			}
		}
		
		
		InvokeRepeating("TimerTickNotify",0,1);
		UIEventListener.Get(ImageExitBtn.gameObject).onClick += OnClickImageExitBtn;
		
		for(int i = (int)MyImageBrn.START + 1;i < (int)MyImageBrn.END;++i)
		{
			DorImageBten[i].Data = i;
			UIEventListener.Get(DorImageBten[i].gameObject).onClick += OnClickImageBtn;
		}
		//girlInviteSlot.GirlSelcectEvents += OnClickGirl;
		UIEventListener.Get(GirlListFNoBtn.gameObject).onClick += OnClickGirlListFNoBtn;
		UIEventListener.Get(KissCheckbox.gameObject).onClick += OnClickKissCheckbox;
		UIEventListener.Get(ReturnToStartBtn.gameObject).onClick += OnClickReturnToStartBtn;
		UIEventListener.Get(ReturnToStartBtn2.gameObject).onClick += OnClickReturnToStartBtn;
		UIEventListener.Get(CameraBtn.gameObject).onClick += OnClickCameraBtn;
		UIEventListener.Get(CameraBtn2.gameObject).onClick += OnClickCameraBtn;
		UIEventListener.Get(ChangePossBtn.gameObject).onClick += OnClickChangePossBtn;
		UIEventListener.Get(ChangePossBtn2.gameObject).onClick += OnClickChangePossBtn;
		UIEventListener.Get(ChangeDisPlay.gameObject).onClick += OnClickChangeDisPlay;
		UIEventListener.Get(ChangeDisPlay2.gameObject).onClick += OnClickChangeDisPlay2;
		
		UIEventListener.Get(SaveBtn.gameObject).onClick += OnClickSaveBtn;
		UIEventListener.Get(SaveBtn2.gameObject).onClick += OnClickSaveBtn;
		UIEventListener.Get(ShareBtn.gameObject).onClick += OnClickShareBtn;
		UIEventListener.Get(ShareBtn2.gameObject).onClick += OnClickShareBtn;
		UIEventListener.Get(ReturnToCameraBtn.gameObject).onClick += OnClickReturnToCameraBtn;
		UIEventListener.Get(ReturnToCameraBtn2.gameObject).onClick += OnClickReturnToCameraBtn;
		
		UIEventListener.Get(ButtonLeft.gameObject).onPress +=  OnClickButtonLeft;
		UIEventListener.Get(ButtonReset.gameObject).onClick += OnClickButtonReset;
		UIEventListener.Get(ButtonRight.gameObject).onPress +=  OnClickButtonRight;
		UIEventListener.Get(ButtonUp.gameObject).onPress   +=  OnClicButtonUp;
		UIEventListener.Get(ButtonDown.gameObject).onPress +=  OnClickButtonDown;
		
		UIEventListener.Get(ButtonLeft2.gameObject).onPress +=  OnClickButtonLeft;
		UIEventListener.Get(ButtonReset2.gameObject).onClick += OnClickButtonReset;
		UIEventListener.Get(ButtonRight2.gameObject).onPress +=  OnClickButtonRight;
		UIEventListener.Get(ButtonUp2.gameObject).onPress   +=  OnClicButtonUp;
		UIEventListener.Get(ButtonDown2.gameObject).onPress +=  OnClickButtonDown;
		
			
		UIEventListener.Get(LevelUpBtn.gameObject).onClick += OnClickLevelUpBtn;
		UIEventListener.Get(ExitLevelUpFrameBtn.gameObject).onClick += OnClickExitLevelUpFrameBtn;
		
		UIEventListener.Get(TalkBtn.gameObject).onClick += OnClickTalkBtn;
		
		
		List<sg.GS2C_Room_Info_Res.Function_Info> FunctionInfo = Globals.Instance.MGameDataManager.MActorData.FunctionInfo;
		
		if(null != InteractiveDisableBtn)
		{
			InteractiveDisableBtn.Data = FunctionInfo[(int)RoomFuncEnum.INTERACTIVE -1];
			UIEventListener.Get(InteractiveDisableBtn.gameObject).onClick += OnClickDisableBtn;
		}

		IceboxDisableBtn.Data = FunctionInfo[(int)RoomFuncEnum.ICEBOX -1];
		UIEventListener.Get(IceboxDisableBtn.gameObject).onClick += OnClickDisableBtn;
		if(null != BathroomDisableBtn)
		{
			BathroomDisableBtn.Data = FunctionInfo[(int)RoomFuncEnum.BATHROOM -1];
			UIEventListener.Get(BathroomDisableBtn.gameObject).onClick += OnClickDisableBtn;
		}
		PhotoDisableBtn.Data = FunctionInfo[(int)RoomFuncEnum.PHOTO -1];
		UIEventListener.Get(PhotoDisableBtn.gameObject).onClick += OnClickDisableBtn;
		
		FlowerpotDisableBtn .Data = FunctionInfo[(int)RoomFuncEnum.Flowerpot -1];
		UIEventListener.Get(FlowerpotDisableBtn.gameObject).onClick += OnClickDisableBtn;
		
		SLEEPDisableBtn.Data = FunctionInfo[(int)RoomFuncEnum.BEDSINGLE -1];
		UIEventListener.Get(SLEEPDisableBtn.gameObject).onClick += OnClickDisableBtn;
		
		UIEventListener.Get(ExitScucessFrameBtn.gameObject).onClick += OnClickExitScucessFrameBtn;
		
		UIEventListener.Get(RefuseButton.gameObject).onClick = OnClickRefuseButton;
		
		width = Mathf.Min(Screen.width,Screen.height)* 0.5f;
	
		pos1 = new Vector3 (Screen.width/2 - width/2,Screen.width/2 + width/2 ,0) ;
		pos2 = new Vector3 (Screen.width/2 + width/2,Screen.width/2 - width/2 ,0) ;
		
	}
	void DisplayShade()
	{
		Texture0.transform.localScale = new Vector3(2048,((Screen.height-width)/2)/Screen.height *1536,0);
		Texture0.transform.position = mUIcamera.ScreenToWorldPoint(new Vector3( Screen.width/2,Screen.height/2 + width/2 + (Screen.height-width)/4,0));
		Texture0.transform.localPosition = new Vector3(Texture0.transform.localPosition.x,Texture0.transform.localPosition.y,-1);
		
		Texture1.transform.localScale = new Vector3(((Screen.width -width)/2)/Screen.width *2048,width/Screen.width *2048,0);
		Texture1.transform.position = mUIcamera.ScreenToWorldPoint(new Vector3( Screen.width/2 - width /2 - (Screen.width - width)/4,Screen.height/2,0));
		Texture1.transform.localPosition = new Vector3(Texture1.transform.localPosition.x,Texture1.transform.localPosition.y,-1);
		
		Texture2.transform.localScale = new Vector3(((Screen.width -width)/2)/Screen.width *2048,width/Screen.width *2048,0);
		Texture2.transform.position = mUIcamera.ScreenToWorldPoint(new Vector3( Screen.width/2 + width /2 + (Screen.width - width)/4,Screen.height/2,0));
		Texture2.transform.localPosition = new Vector3(Texture2.transform.localPosition.x,Texture2.transform.localPosition.y,-1);
		
		
		Texture3.transform.localScale = new Vector3(2048,((Screen.height-width)/2)/Screen.height *1536,0);
		Texture3.transform.position = mUIcamera.ScreenToWorldPoint(new Vector3( Screen.width/2,Screen.height/2 - width/2 - (Screen.height-width)/4,0));
		Texture3.transform.localPosition = new Vector3(Texture3.transform.localPosition.x,Texture3.transform.localPosition.y,-1);
		
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy ();
	
	}

	public override void InitializeGUI()
	{
		if(_mIsLoaded){
			return;
		}
		_mIsLoaded = true;
		
		this.GUILevel = 10;
		
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,100);
	
		DisplayFunction();
		wealthgroup.showJingLi();
			
	}
	
	protected virtual void Start ()
	{
		base.Start();
       if(null == this)
			return;
		Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
		if(null == dicWarShipData)
			return;
		

		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			
			if(-1 == home.GirID)
			{
				foreach (GirlData girlData in dicWarShipData.Values)
				{
//					if(girlData.BasicData.LogicID == 1217002000)
//					{
//						mGirldata = girlData;
//						mGirlID = girlData.roleCardId;
//						((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).GirID = girlData.BasicData.LogicID;
//						break;
//					}
				}
			}
			
			home.particle.Play();
		
				
			MyCamera = home.camera;
			SceneHomeObj = home.SceneHomeObj;
			characterCustomizeOne  = home .characterCustomizeOne;
			if(null != MyCamera)
			{
				ControllTransform = MyCamera.transform.parent;
				mOrbitCamera = ControllTransform.GetComponent<OrbitCamera>();
				target = characterCustomizeOne.transform;
				mIniPos = ControllTransform.localPosition;
				mIniRot.x = ControllTransform.eulerAngles.x;
				mIniRot.y = ControllTransform.eulerAngles.y;
				mIniRot.z = ControllTransform.eulerAngles.z;
				mWorldIniPos = ControllTransform.position;
			}
		
			hips = target.GetComponentsInChildren<Transform>();
		
			foreach(Transform hip in hips)
			{
				if (hip.name == "Bip001 Head")
					mHead = hip;
				else if(hip.name  == "Bip001 L Calf")
					mChest = hip;
				if (hip.name == "Bone026")
					mLeg = hip;
				else if (hip.name == "zuijiaoL")
				  mMouse = hip;
				
			}
		}
			
	
	
		DisplayStart(true);
		NGUITools.SetActive(StartFrameObj,true);
		NGUITools.SetActive(CameraFramobj,false );
//		HelpUtil.DelListInfo(StartFUiGrid.transform);
		StartFUiGrid.repositionNow = true;
		//StratFDraggablePanel.ResetPosition();
		

		mUIcamera = GameObject.Find("UICamera").GetComponent<Camera>();
		
		mGiftCurrentTime = 12;
	    mGiftNextTime = mGiftCurrentTime + Mathf.CeilToInt(Time.time);
		
		Globals.Instance.MPushDataManager.RefreshPushUI("GUIDormitory");
		
		//IsCanLevelUP();
		
		//mat.color = Color.red;
		DisplayShade();
	}
	//feature
	
    IEnumerator CutImage()  
	{  
		OnPostRender() ;
		
	  Texture2D cutImage = new Texture2D((int)(pos2.x - pos1.x), (int)(pos1.y - pos2.y), TextureFormat.RGB24, true);  
	
	
	//  Rect rect = new Rect((int)pos1.x, Screen.height - (int)(Screen.height - pos2.y), (int)(pos2.x - pos1.x), (int)(pos1.y - pos2.y));  
	Rect rect = new Rect((int)pos1.x, (int)(pos2.y - width/3), (int)(pos2.x - pos1.x), (int)(pos1.y - pos2.y));  
	
	   yield return new WaitForEndOfFrame();  
	   cutImage.ReadPixels(rect, 0, 0, true);    
	   //cutImage.Apply();  
	   yield return cutImage;  
	   byte[] byt = cutImage.EncodeToPNG();  
	   File.WriteAllBytes(Application.streamingAssetsPath + "/CutImage.png", byt);  
	
	} 
	void OnPostRender()  
	{
//		GL.PushMatrix();  
//		mat.SetPass(0);  
//		GL.LoadOrtho();  
//		GL.Begin(GL.LINES);  
//		GL.Color(Color.red);  
//		GL.Vertex3(pos1.x/Screen.width, pos1.y/Screen.height, pos1.z);  
//		GL.Vertex3(pos2.x / Screen.width, pos2.y / Screen.height, pos2.z);  
//		GL.End();  
//		GL.PopMatrix();  
//		
//
//		int a = 0;
//		int b = 0;
	
	}

	void CutPic()
	{
		//Debug.Log("touchNUM" + Input.touchCount.ToString());
		if(Input.GetMouseButton(0)) //&& Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			//Touch temp = Input.touches[0];
			mCurrenTexture.transform.localPosition = new Vector3(mCurrenTexture.transform.localPosition.x + Input.GetAxis("Mouse X") * 15,
																	mCurrenTexture.transform.localPosition.y+ Input.GetAxis("Mouse Y") * 15,
																	mCurrenTexture.transform.localPosition.z) ;
		}
	}
	
	
	protected virtual void Update ()
	{	
		
		/////////
		if (Input.GetMouseButtonDown(0))  
	     {  
	        // pos1 = Input.mousePosition;  
			Debug.Log(pos1.x.ToString() +"y:"+ pos1.y.ToString ());
			Debug.Log(Input.mousePosition .x.ToString() +"mousePositiony:"+ Input.mousePosition.y.ToString ());
	     }  
 
     	if (Input.GetMouseButtonUp(0))  
		{  
			//pos2 = Input.mousePosition;  
			StartCoroutine(CutImage());  
		
		} 
		CutPic();
		
//		float OffsetX = 0;//Screen.width;
//		float OffsetY = 0;//Screen.height;
//		pos1 =  mUIcamera.ScreenToWorldPoint(pos1);
//		
//		Vector3 tpos0 = new Vector3 (pos1.x - OffsetX,pos1.y - OffsetY ,0);
//		Vector3 tpos1 = new Vector3(pos1.x - OffsetX,Input.mousePosition.y - OffsetY,0);
//		Vector3 tpos2 = new Vector3 (Input.mousePosition.x - OffsetX,Input.mousePosition.y - OffsetY ,0);
//		Vector3 tpos3 = new Vector3(Input.mousePosition.x - OffsetX,pos1.y -OffsetY ,0);
//		 
//		Line0.SetPosition(0,tpos0);
//		Line0.SetPosition(1,tpos1);
//		
//		Line1.SetPosition(0,tpos1);
//		Line1.SetPosition(1,tpos2);
//		
//		Line2.SetPosition(0,tpos2);
//		Line2.SetPosition(1,tpos3);
//		
//		Line3.SetPosition(0,tpos3);
//		Line3.SetPosition(1,tpos0);
		////////////////
		if( true == mCurrentState.pressed)
		{
			MoveCamera(mCurrentState.dir);
		}
		
		if(true == bIsFeature && null != this )
		{
			LocalZ = Mathf.Lerp(LocalZ, distance, Time.deltaTime*5);
			Vector3 Rot0 = new Vector3(cameraControllRotSideCurx,cameraControllRotSideCury,cameraControllRotSideCurz); 	
			MyCamera.transform.localPosition = new Vector3(MyCamera.transform.localPosition.x,MyCamera.transform.localPosition.y,LocalZ);
			//旋转时nearClipPlane设置为1//
			MyCamera.nearClipPlane = 1;
			
			cameraControllRotSideCurx = Mathf.LerpAngle(cameraControllRotSideCurx, cameraControllRotSidex, Time.deltaTime*5);
			cameraControllRotSideCury = Mathf.LerpAngle(cameraControllRotSideCury, cameraControllRotSidey, Time.deltaTime*5);
			cameraControllRotSideCurz = Mathf.LerpAngle(cameraControllRotSideCurz, cameraControllRotSidez, Time.deltaTime*5);
			Vector3 Rot = new Vector3(cameraControllRotSideCurx,cameraControllRotSideCury,cameraControllRotSideCurz); 		
			ControllTransform.rotation = Quaternion.Euler(Rot);
			Rot = Rot - Rot0;
			
			Vector3 Pos0 = new Vector3 (cameraControllCurPosX,cameraControllCurPosY,cameraControllCurPosZ);
			cameraControllCurPosX = Mathf.Lerp(cameraControllCurPosX,cameraControllPosX,Time.deltaTime*5);
			cameraControllCurPosY = Mathf.Lerp(cameraControllCurPosY,cameraControllPosY,Time.deltaTime*5);
			cameraControllCurPosZ = Mathf.Lerp(cameraControllCurPosZ,cameraControllPosZ,Time.deltaTime*5);
			Vector3 Pos = new Vector3 (cameraControllCurPosX,cameraControllCurPosY,cameraControllCurPosZ);
			ControllTransform.localPosition = Pos;
			Pos = Pos - Pos0;
				
			//cameraControllCurPosY = Mathf.Lerp(cameraControllCurPosY,mPart.transform.position.y,Time.deltaTime*5);
			//ControllTransform.position = new Vector3 (ControllTransform.position.x,cameraControllCurPosY,ControllTransform.position.z);
			//MyCamera.transform.localPosition = new Vector3(MyCamera.transform.localPosition.x,LocalY,LocalZ);
			//MyCamera.transform.LookAt(ControllTransform.position);
					
//			float text =  Vector3.Magnitude(mLastAngle - ControllTransform.eulerAngles);
			if(0.25f > Mathf.Abs(LocalZ - distance) && 0.25f > Vector3.Magnitude(Rot) && 0.25f > Vector3.Magnitude(Pos) )
			{
				bIsFeature = false;
				mOrbitCamera.IniState();
				DisplayAction();
				HeadLookController headLookController = characterCustomizeOne.transform.GetComponent<HeadLookController>();
				//headLookController.Initial(characterCustomizeOne.transform);
				if(headLookController.enabled == true)
					headLookController.enabled = false;
				//GameObject.DestroyImmediate(headLookController.gameObject);
			}
			
		}
		if(null == characterCustomizeOne)
			return;
		Animator animator = characterCustomizeOne.getCharacterAnimator();
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		if(info.normalizedTime > 1)
		{
			HeadLookController headLookController = characterCustomizeOne.transform.GetComponent<HeadLookController>();
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			if(headLookController.enabled == false &&home.bIsOpenControll == true )
			{
				headLookController.enabled = true;
				headLookController.Initial(characterCustomizeOne.transform);
			}
			
			
			
		}
		if(null !=  ControllTransform)
			mLastAngle = ControllTransform.eulerAngles;
		//Debug.Log (MyCamera.transform.localPosition.z.ToString());
//		if(null != MyCamera)
//		{
//			Camera UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
//			Vector3 mousepos = Input.mousePosition;
//			mousepos = UICamera.ScreenToWorldPoint(mousepos);
//			//EffectsSprite.transform.position = mousepos;
//			Vector3 pos = EffectsSprite.transform.position;
//			EffectsSprite.transform.position = new Vector3 (mousepos.x,mousepos.y,EffectsSprite.transform.position.z);
//			int a = 0;
//		}
		
	}
	protected void DisplayAction()
	{

	}
	public void IniControllPos(Transform Part,float fDistance = 50.0f,int iPartID = -1)
	{
		
		bIsFeature = true;
		mPartID = iPartID;
		cameraControllRotSidex = 340f;
		cameraControllRotSideCurx = ControllTransform.localEulerAngles.x;	
		cameraControllRotSidey = 348f;
		cameraControllRotSideCury  = ControllTransform.localEulerAngles.y;		
		cameraControllRotSidez = 0f;
		cameraControllRotSideCurz = ControllTransform.localEulerAngles.z;
		
		cameraControllPosX = 8f;
		cameraControllCurPosX = ControllTransform.localPosition.x;	
		cameraControllPosY = 280f;
		cameraControllCurPosY = ControllTransform.localPosition.y;	
		cameraControllPosZ = -90f;
		cameraControllCurPosZ = ControllTransform.localPosition.z;
			
		distance = Vector3.Magnitude(new Vector3(0,0,270));
		LocalZ = MyCamera.transform.localPosition.z;
	
	}
	protected void ReturnLastPostion()
	{
		
	}
	 //endfeature
	//startframe
	private void OnClickImageExitBtn(GameObject obj)
	{
		this.Close();
		Globals.Instance.MGUIManager.CreateWindow<GUILoading>(delegate(GUILoading gui)
		{							
			gui.SetVisible(true);
			gui.LoadingType = GUILoading.ELoadingType.ENTER_PORT;	
			string sceneName = Globals.Instance.MGameDataManager.MCurrentPortData.BasicData.SceneName;
			Globals.Instance.MSceneManager.ChangeScene(sceneName, Vector3.zero);
			GameStatusManager.Instance.MGameNextState = GameState.GAME_STATE_PORT;
		});
		
	
		Globals.Instance.MNpcManager.setmbSuoPing(false);
	
		
	}
	private void OnClickImageBtn(GameObject obj)
	{
		UIImageButton box = obj.transform.GetComponent<UIImageButton>();
		LocalMgr((int)box.Data);
	}
	void OnClickRefuseButton(GameObject Obj)
	{
		NGUITools.SetActive(StartFSleepObj,false);
	}

	//endstartframe
	private void LocalMgr(int index, bool IsOpen = true)
	{
		
		switch(index)
		{
			
		case (int)MyImageBrn.BATHROOM:
		{
			DisplayBATHROOM(IsOpen);
			break;
		}
		case (int)MyImageBrn.GIFT:
		{
			DisplayGIFT(IsOpen);
			break;
		}
		case (int)MyImageBrn.SLEEP:
		{
			DisplaySLEEP(IsOpen);
			break;
		}
		case (int)MyImageBrn.ICEBOX:
		{
			DisplayICEBOX(IsOpen);
			break;
		}
		case (int)MyImageBrn.POTTING:
		{
			
			DisplayPOTTING(IsOpen);
			break;
		}
		case (int)MyImageBrn.CHAT:
		{
			DisplayCHAT(IsOpen);
			break;
		}
		case (int)MyImageBrn.GIRLLIST:
		{
			DisplayGIRLLIST(IsOpen);
			break;
		}
		case (int)MyImageBrn.CHANGECLOTHES:
		{
			DisplayCHANGECLOTHES(IsOpen);
			break;
		}
		case (int)MyImageBrn.CAMERA:
		{
			DisplayCAMERA(IsOpen);
			break;
		}
		case (int)MyImageBrn.LEVELUP:
		{
			DisplayLEVELUP(IsOpen);
			break;
		}
		case (int)MyImageBrn.INTERACT:
		{
			DisplayINTERACT(IsOpen);
			break;
		}
		case (int)MyImageBrn.SLEEPALONE:
		{
			DisplaySLEEPALONE(IsOpen);
			break;
		}
		case (int)MyImageBrn.SEEEPDOUBLE:
		{
			DisplaySEEEPDOUBLE(IsOpen);
			break;
		}
		case (int)MyImageBrn.INTERACTClose:
		{
			DisplayINTERACTClose(IsOpen);
			break;
		}
		default:
		{
			break;
		}
		}
		
	}
	

	private void DisplayGIFT(bool IsOpen)
	{
		if(mGiftCurrentTime > 0)
		{
			//Globals.Instance.MDataTableManager.GetWordText(16007);
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(16007,true);
		}
		else
		{
			NGUITools.SetActive(GiftFrameObj,true);
			//TweenAlpha.Begin(LevelUpScucessFrame,0.001f, 1f );
			Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
			int Index =  Random.Range(0,dicWarShipData.Count);
			int i = 0;
			string name = "";
			foreach(GirlData data in dicWarShipData.Values)
			{
				if(i == Index)
				{
//					name = data.BasicData.Name;
					break;
				}
				++i;
			}
			UILabel label = GiftFrameObj.transform.Find("GirlNameLabel").GetComponent<UILabel>();
			label.text = name;
			
			int itemID = 11300001;
			itemID += i;
			ItemConfig config = Globals.Instance.MDataTableManager.GetConfig<ItemConfig> ();		
			ItemConfig.ItemElement element = null;
			
			config.GetItemElement(itemID, out element);
			
			UITexture ItemTexture =  GiftFrameObj.transform.Find("ItemTexture").GetComponent<UITexture>();									
			string texturePath = "Icon/ItemIcon/" + element.Icon;
			ItemTexture.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
			
		
			EventDelegate.Add(TweenAlpha.Begin(GiftFrameObj,2f, 0f ).onFinished ,delegate() {
					UIPanel panel = GiftFrameObj.transform.GetComponent<UIPanel>();
				
					panel.alpha = 1;
					NGUITools.SetActive(GiftFrameObj,false);	
				});
			
		mGiftCurrentTime = 12;
	    mGiftNextTime = mGiftCurrentTime + Mathf.CeilToInt(Time.time);
		}
		
	}
	private void DisplaySLEEP(bool IsOpen)
	{
		
		if(IsOpen)
		{
			NGUITools.SetActive(StartFSleepObj,true);
		}
		else
		{
			NGUITools.SetActive(StartFSleepObj,false);
			return;
		}
	}
	private void DisplayICEBOX(bool IsOpen)
	{

	}
	
	private void DisplayPOTTING(bool IsOpen)
	{
		this.Close();

	}
	private void DisplayCHAT(bool IsOpen)
	{
		string notice = Globals.Instance.MDataTableManager.GetWordText(16016);
		GUISingleDrop.ShowTalkObj(notice);

	}
	private void DisplayGIRLLIST(bool IsOpen)
	{
		NGUITools.SetActive(GirllistFObj,IsOpen);
		
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = false	;
		}
		if(true == IsOpen)
		{
			girlInviteSlot.SetCheck(mGirlID);
		}
	}
	private void DisplayCHANGECLOTHES(bool IsOpen)
	{

	}
	public void  ResetGirl(long id)
	{
		GirlData girlData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(id);
		characterCustomizeOne.ResetCharacter();
	}

	private void DisplayLEVELUP(bool IsOpen)
	{
		RoomConfig roomConfig = Globals.Instance.MDataTableManager.GetConfig<RoomConfig>();
		int MaxLev = roomConfig.GetMaxRoomLev();
		int iRoomLevl =  Globals.Instance.MGameDataManager.MActorData.RoomLevl;
		if( iRoomLevl >= MaxLev)
		{
			string notice = Globals.Instance.MDataTableManager.GetWordText(16027);
			GUISingleDrop.ShowTalkObj(notice);
			return;
		}
		
		
		((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = false; 
		NGUITools.SetActive(LevelUpNeedsFrame,true);
		CurrentLevelLabel.text = Globals.Instance.MDataTableManager.GetWordText(16036) + iRoomLevl.ToString();
		
		
		RoomConfig.RoomObject data = roomConfig.GetRoomObjectByRoomLev(iRoomLevl + 1);
		List<RoomConfig.RoomNeedData> datalist = data.NeedList;
		ItemConfig itemconfig = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
		ItemConfig.ItemElement itemelement = null;
		
		bool disable = false;
		int i = 0;
		foreach(RoomConfig.RoomNeedData GoodsData in datalist)
		{
			bool ishave = itemconfig.GetItemElement(GoodsData.NeedID,out itemelement);	
			GoodsLabel[i].text = 	Globals.Instance.MDataTableManager.GetWordText(itemelement.NameID);
			string texturePath = "Icon/ItemIcon/" + itemelement.Icon;
			GoodsTexture[i].mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
			//GoodsSprite[i].spriteName = itemelement.Icon;
			ItemSlotData itemdata = FindItemByID(GoodsData.NeedID);
			int itemNum = itemdata == null ? 0 : itemdata.MItemData.BasicData.Count;
			
			GoodsNumLabel[i].text = itemNum.ToString() + "/" + datalist[i].NeedNum.ToString();
			if(itemNum >= datalist[i].NeedNum)
			{
				GoodsNumLabel[i].color = new Color(0,255,0,1);
				GoodsLabel[i].effectColor = new Color(0,159,124,1);
			}
			else
			{
				GoodsNumLabel[i].color = new Color(255,0,0,1);
				GoodsLabel[i].effectColor = new Color(255,0,0,1);
				disable = true;
				
			}
			++i;
		}
		LevelUpBtn.isEnabled = !disable;
		//NetSender.Instance.RequestRoomLevelUp();
		
	}
	bool IsCanLevelUP()
	{
		RoomConfig roomConfig = Globals.Instance.MDataTableManager.GetConfig<RoomConfig>();
		int MaxLev = roomConfig.GetMaxRoomLev();
		int iRoomLevl =  Globals.Instance.MGameDataManager.MActorData.RoomLevl;
		
		RoomConfig.RoomObject data = roomConfig.GetRoomObjectByRoomLev(iRoomLevl + 1);
		List<RoomConfig.RoomNeedData> datalist = data.NeedList;
		ItemConfig itemconfig = Globals.Instance.MDataTableManager.GetConfig<ItemConfig>();
		ItemConfig.ItemElement itemelement = null;
		
		bool disable = false;
		
		foreach(RoomConfig.RoomNeedData GoodsData in datalist)
		{
			bool ishave = itemconfig.GetItemElement(GoodsData.NeedID,out itemelement);	
			ItemSlotData itemdata = FindItemByID(GoodsData.NeedID);
			int itemNum = itemdata == null ? 0 : itemdata.MItemData.BasicData.Count;
			if(itemNum < GoodsData.NeedNum)
			{
				disable = true;
			}
		}
		Transform Temp = DorImageBten[3].transform.Find("PushDataUISprite");
		if(false == disable)
		{
			
			if(null == Temp)
			{
				UISprite jianTou = GameObject.Instantiate(Globals.Instance.MPushDataManager.pushTips) as UISprite;
				jianTou.transform.parent = DorImageBten[3].gameObject.transform;
				jianTou.name = "PushDataUISprite";
				jianTou.transform.localPosition = new Vector3(100,80,-8f);
				jianTou.MakePixelPerfect();
			}
		}
		else
		{
			if(null != Temp)
			{
				GameObject.DestroyObject(Temp.gameObject);
			}
		}
		return disable;

	}
	private ItemSlotData FindItemByID(int iItemID)
	{
		List<ItemSlotData> itemdataList = ItemDataManager.Instance.GetItemDataList(ItemSlotType.OTHER_BAG);
		foreach(ItemSlotData item in itemdataList)
		{
//			if(null != item.MItemData && item.MItemData.BasicData.LogicID == iItemID)
//			{
//				return item;
//			}
		}
		return null;
	}

	private void DisplayINTERACT(bool IsOpen)
	{
		string notice = Globals.Instance.MDataTableManager.GetWordText(16016);
		GUISingleDrop.ShowTalkObj(notice);
//		NGUITools.SetActive(DorImageBten[(int)MyImageBrn.INTERACT].transform.gameObject,false);
//		NGUITools.SetActive(DorImageBten[(int)MyImageBrn.INTERACTClose].transform.gameObject,true);
//		TweenPosition.Begin(StratFTouchGroupObj,1f,new Vector3(1800,-600,0));
	}
	private void DisplaySLEEPALONE(bool IsOpen)
	{
		NGUITools.SetActive(StartFSleepObj,false);

	}
	private void DisplaySEEEPDOUBLE(bool IsOpen)
	{
		if(Globals.Instance.MGameDataManager.MActorData.RoomLevl <= 2)
		{
			string notice = Globals.Instance.MDataTableManager.GetWordText(16010);
			GUISingleDrop.ShowTalkObj(notice);
		}
		else
		{
			NGUITools.SetActive(StartFSleepObj,false);
		
		}
			
	
	}
	private void DisplayStart(bool IsOpen)
	{
		if(IsOpen == true)
		{
			NGUITools.SetActive(StartFStartobj,true);
			NGUITools.SetActive(GirllistFObj,false);
		}
		else
		{
			NGUITools.SetActive(StartFStartobj,false);
			return;
		}
		
//		string texturePath = "Icon/AvatarIcon/" + mGirldata.BasicData.shipElement.WarshipIcon;
//		GirlPic.mainTexture = Resources.Load(texturePath,typeof(Texture2D)) as Texture2D;
	}
	private void DisplayINTERACTClose(bool IsOpen)
	{
		string notice = Globals.Instance.MDataTableManager.GetWordText(16016);
		GUISingleDrop.ShowTalkObj(notice);
//		NGUITools.SetActive(DorImageBten[(int)MyImageBrn.INTERACT].transform.gameObject,true);
//		NGUITools.SetActive(DorImageBten[(int)MyImageBrn.INTERACTClose].transform.gameObject,false);
//		TweenPosition.Begin(StratFTouchGroupObj,1f,new Vector3(0,-600,0));
	}
	//girllistframe
	private void OnClickGirl(long id)
	{
		GirlData TempData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(id);
//		if(TempData.PropertyData.IntimateLevel < 4 && 1217002000 != TempData.BasicData.LogicID)
//		{
//
//			Globals.Instance.MGUIManager.ShowSimpleCenterTips(16001);
//			return;
//		}
		mGirldata = TempData;
		LocalMgr((int) MyImageBrn.START);
		DisplayStart(true);
		
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
//			((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).GirID = mGirldata.BasicData.LogicID;
		}
		mGirlID = id;
			
		
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = true;	
		}
	}
	
	private void OnClickGirlListFNoBtn(GameObject obj)
	{
		//DisplayStart(true);
		
		LocalMgr((int) MyImageBrn.START);
		DisplayStart(true);	
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = true;	
		}
	}
	
	//endgirllisfram
	//feature
	private void OnClickKissCheckbox(GameObject obj)
	{
		bIsFeature = true;
		IniControllPos(mMouse);
	}
	
	//camera
	private void DisplayCAMERA(bool IsOpen)
	{
		//NGUITools.SetActive(ButtonInvisible,true);
		NGUITools.SetActive(StartFrameObj,false);
		//characterCustomizeOne.changeCharacterAnimationController("General_Idle");
		characterCustomizeOne.changeCharacterAnimationController("Ningning_Pos1");	
		NGUITools.SetActive(CameraFramobj,true);
		
		HeadLookController headLookController = characterCustomizeOne.transform.GetComponent<HeadLookController>();
		headLookController.Initial(characterCustomizeOne.transform);
		headLookController.enabled = false;
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		home.bIsOpenControll = false;
		home.bClick = false;
		
		//CaptureCamera(MyCamera,new Rect(0,0, 1000, 1000));
	}

	/// for 给多次连续分享用的接口/// 
	public static Texture2D  CaptureCamera(Camera camera, Rect rect)   
	{  
	    // 创建一个RenderTexture对象  
	    RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 16);  
	    // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
	    camera.targetTexture = rt;  
	    camera.Render();  
	        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
	        //ps: camera2.targetTexture = rt;  
	        //ps: camera2.Render();  
	        //ps: -------------------------------------------------------------------  
	  
	    // 激活这个rt, 并从中中读取像素。  
	    RenderTexture.active = rt; 
		if (screenShot == null)
			screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false); 

	    screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
	    screenShot.Apply();
			  
	    // 重置相关参数，以使用camera继续在屏幕上显示  
	    camera.targetTexture = null;  
	        //ps: camera2.targetTexture = null;  
	    RenderTexture.active = null; // JC: added to avoid errors  
	    GameObject.Destroy(rt);  
	    // 最后将这些纹理数据，成一个png图片文件  
	    byte[] bytes = screenShot.EncodeToPNG();  
	    string filename = Application.dataPath + "/Screenshot.png";  
	    //System.IO.File.WriteAllBytes(filename, bytes);  
	    Debug.Log(string.Format("截屏了一张照片: {0}", filename));       
	    return screenShot;  
	} 
	private void OnClickReturnToStartBtn(GameObject obj)
	{
		//NGUITools.SetActive( ButtonInvisible,false);
		ControllTransform.localPosition = mIniPos;
		mOrbitCamera.Reset();
		NGUITools.SetActive(StartFrameObj,true);
		characterCustomizeOne.changeCharacterAnimationController("General_Idle");
		NGUITools.SetActive(CameraFramobj,false );
		NGUITools.SetActive(CameraFramobj2,false );
		HeadLookController headLookController = characterCustomizeOne.transform.GetComponent<HeadLookController>();
		headLookController.Initial(characterCustomizeOne.transform);
		headLookController.enabled = true;
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		home.bIsOpenControll = true;
		home.bClick = true;
		
		if(false == home.bHorizontal)
		{
			Vector3 temp = MyCamera.transform .localEulerAngles;
			MyCamera.transform .localEulerAngles = new Vector3(temp.x,temp.y,temp.z + 90);
			home.bHorizontal = true;
		}
		Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIDormitory");
	}
	private void OnClickCameraBtn(GameObject obj)
	{
		var rtW = Screen.width/2;
		var rtH = Screen.height/2;
		texture2d = CaptureCamera(MyCamera,new Rect(0,0, rtW, rtH));
//		int width = Screen.width/2;	
//		int height = Screen.height/2;	
//		Texture2D tex = new Texture2D(width,height,TextureFormat.RGB24,true);
//		tex.ReadPixels(new Rect(0,0,width,height),0,0,true);
//		byte[] byt = tex.EncodeToPNG();  
//        System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/CutImage.png", byt); 

		
		
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		if(true == home.bHorizontal)
		{
			mCurrenTexture.mainTexture = texture2d;
			NGUITools.SetActive(SaveBtn.transform.parent.gameObject,true);
			NGUITools.SetActive(ChangeDisPlay.transform.parent.gameObject,false);
		}
		else
		{
			mCurrenTexture2.mainTexture = texture2d;
			NGUITools.SetActive(SaveBtn2.transform.parent.gameObject,true);
			NGUITools.SetActive(ChangeDisPlay2.transform.parent.gameObject,false);
		}
		int teachStep = Globals.Instance.MTeachManager.NewGetTeachStep("x04");
		if (teachStep == 1000000)
		{
			Globals.Instance.MTeachManager.NewOpenWindowEvent("GUIDormitory");
		}
	}
	private void  OnClickChangePossBtn(GameObject obj)
	{
		mPosIndex++;
		if(mPosIndex > mMaxosIndex)
		{
			mPosIndex = 1;
		}
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		
		PosIndexLabel.text = mPosIndex.ToString();
		PosIndexLabel2.text = mPosIndex.ToString();
		characterCustomizeOne.changeCharacterAnimationController("Ningning_Pos" + mPosIndex.ToString());	
	}
	private void  OnClickChangeDisPlay(GameObject obj)
	{	
		Vector3 temp = MyCamera.transform .localEulerAngles;
		MyCamera.transform .localEulerAngles = new Vector3(temp.x,temp.y,temp.z - 90);
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			home.bHorizontal = false;
			NGUITools.SetActive(CameraFramobj,false);
			NGUITools.SetActive(CameraFramobj2,true);
		}	
	}
	private void  OnClickChangeDisPlay2(GameObject obj)
	{	
		Vector3 temp = MyCamera.transform .localEulerAngles;
		MyCamera.transform .localEulerAngles = new Vector3(temp.x,temp.y,temp.z + 90);
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			home.bHorizontal = true;
			NGUITools.SetActive(CameraFramobj,true);
			NGUITools.SetActive(CameraFramobj2,false);
		}	
	}
	
	private void  OnClickSaveBtn(GameObject obj)
	{
		Rect rect = new Rect(0,0, 2051, 1538);
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 16);  
//		texture2d.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
//	    texture2d.Apply();  
	  
	    // 重置相关参数，以使用camera继续在屏幕上显示  
	  //  MyCamera.targetTexture = null;  
	        //ps: camera2.targetTexture = null;  
	    RenderTexture.active = null; // JC: added to avoid errors  
	    GameObject.Destroy(rt);  
	    // 最后将这些纹理数据，成一个png图片文件  
	    byte[] bytes =  texture2d.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
	    string filename = Application.persistentDataPath + "/" + cptrAddr;  
	    System.IO.File.WriteAllBytes(filename, bytes);  
	    Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
		
		
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			//U3dAppStoreSender.AppSavePhoth(filename);
		}
		else if (GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
		{
			GUIRadarScan.Show();
			StartCoroutine(InvokeAndroidCameraSaveDelegate());
		}
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		if(true == home.bHorizontal)
		{
			NGUITools.SetActive(SaveBtn.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay.transform.parent.gameObject,true);	
		}
		else
		{
			NGUITools.SetActive(SaveBtn2.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay2.transform.parent.gameObject,true);
		}
	}

	IEnumerator InvokeAndroidCameraSaveDelegate()
	{
		yield return new WaitForSeconds(0.2f);
		// 最后将这些纹理数据，成一个png图片文件  //
		byte[] bytes =  texture2d.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
		string filename = Application.persistentDataPath + "/" + cptrAddr;
		try
		{
			System.IO.File.WriteAllBytes(filename, bytes);  
			Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
			AndroidSDKAgent.SavePhoto(filename,cptrAddr);
			Debug.Log("----------------------" + filename);
		}
		catch
		{
			GUIRadarScan.Hide();
			Globals.Instance.MGUIManager.ShowSimpleCenterTips(10012);
		}
		yield return 0;
	}

	private void  OnClickShareBtn(GameObject obj)
	{
	    // 最后将这些纹理数据，成一个png图片文件  
	    byte[] bytes =  texture2d.EncodeToPNG();  
		string cptrAddr = "Screenshot" + System.DateTime.Now.Second.ToString() + ".png";
	    string filename = Application.persistentDataPath + "/" + cptrAddr;  
	    System.IO.File.WriteAllBytes(filename, bytes);  
	    Debug.Log(string.Format("截屏了一张照片: {0}", filename));   
		
		
		if (GameDefines.OutputVerDefs == OutputVersionDefs.AppStore)
		{
			
			//U3dAppStoreSender.ShareMyPhoto(filename);
		}
		else if(GameDefines.OutputVerDefs == OutputVersionDefs.WPay)
		{
			AndroidSDKAgent.showShare(filename);
		}
						
		
		//share
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		if(true == home.bHorizontal)
		{
			NGUITools.SetActive(SaveBtn.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay.transform.parent.gameObject,true);	
		}
		else
		{
			NGUITools.SetActive(SaveBtn2.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay2.transform.parent.gameObject,true);
		}	
	}
	
	
	private void  OnClickReturnToCameraBtn(GameObject obj)
	{
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		if(true == home.bHorizontal)
		{
			NGUITools.SetActive(SaveBtn.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay.transform.parent.gameObject,true);	
		}
		else
		{
			NGUITools.SetActive(SaveBtn2.transform.parent.gameObject,false);
			NGUITools.SetActive(ChangeDisPlay2.transform.parent.gameObject,true);
		}		
	}
	private void  OnClickButtonLeft(GameObject obj,bool pressed)
	{
		if(true == pressed)
		{

			mCurrentState.dir = Direction.LEFT;
			mCurrentState.pressed = true;
		}
		else
		{
			mCurrentState.pressed = false;
		}	
	}
	private void  OnClickButtonRight(GameObject obj,bool pressed)
	{
		if(true == pressed)
		{

			mCurrentState.dir = Direction.RIGHT;
			mCurrentState.pressed = true;
		}
		else
		{
			mCurrentState.pressed = false;
		}	
	}
	private void  OnClicButtonUp(GameObject obj,bool pressed)
	{
		if(true == pressed)
		{

			mCurrentState.dir = Direction.UP;
			mCurrentState.pressed = true;
		}
		else
		{
			mCurrentState.pressed = false;
		}	
	}
	private void  OnClickButtonDown(GameObject obj,bool pressed)
	{
		if(true == pressed)
		{

			mCurrentState.dir = Direction.DOWN;
			mCurrentState.pressed = true;
		}
		else
		{
			mCurrentState.pressed = false;
		}	
	}

	private void MoveCamera(Direction dir)
	{
		HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
		Vector3 pos = ControllTransform.localPosition;
		Vector3 TempPos = Vector3.zero;
		switch(dir)
		{
		case Direction.LEFT:
		{	
			if(true == home.bHorizontal)
			{
				TempPos = ControllTransform.position - MyCamera.transform.right * Time.deltaTime * 40;
			}
			else
			{
				TempPos = ControllTransform.position - MyCamera.transform.up * Time.deltaTime * 40;
			}
		}break;
		case Direction.RIGHT:
		{	
			if(true == home.bHorizontal)
				TempPos = ControllTransform.position + MyCamera.transform.right * Time.deltaTime * 40;
			else
				TempPos = ControllTransform.position + MyCamera.transform.up * Time.deltaTime * 40;
		}break;
		case Direction.UP:
		{			
			TempPos = ControllTransform.position + Vector3.up * Time.deltaTime * 40;
		}break;
		case Direction.DOWN:
		{			
			TempPos = ControllTransform.position - Vector3.up * Time.deltaTime * 40;
		}break;
			
		}

		
		Vector3 tempPos1 = new Vector3(mWorldIniPos.x,TempPos.y,mWorldIniPos.z);
		if(Vector3.Magnitude(TempPos - tempPos1) > mLimitMove)
			return;
	
		float Y_axis = TempPos.y;
		float Ymin = mWorldIniPos.y - mDownMax;
		float Ymax = mWorldIniPos.y + mUpMax;
		Y_axis = Mathf.Clamp(Y_axis,Ymin,Ymax);
		ControllTransform.position = new Vector3(TempPos.x,Y_axis,TempPos.z);

	}
	private void  OnClickButtonReset(GameObject obj)
	{
		ControllTransform.localPosition = mIniPos;
		//ControllTransform.rotation = Quaternion.Euler(mIniRot);
		mOrbitCamera.Reset();
		
	}
 
	public string MyPath
     {
         get{
             string    path=null;
             if(Application.platform==RuntimePlatform.IPhonePlayer)
             {
                 path= Application.dataPath.Substring (0, Application.dataPath.Length - 5);
                 path = path.Substring(0, path.LastIndexOf('/'))+"/Documents/";    
             }
             else
             {
                 path=Application.dataPath+"/Resource/GameData/";
             }
             return path;
         }     
     }
//endcamera
	//LevelUpFrame
	
	private void OnClickLevelUpBtn(GameObject Obj)
	{

		
		NGUITools.SetActive(LevelUpNeedsFrame,false);
		((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = true; 

	}
	public void ReceiveLevelUp(int iError,int iNewLevel  = 0,int iNewEnergy  = 0,int iNewLovepoint  = 0,int iNewFlowerpotNum = 0,int iNewInviteNum = 0)
	{
		if(10000 == iError)	
		{
			
			HelpUtil.DelListInfo(AllItem.transform);
			Dictionary<int,GameObject> Temp = new Dictionary<int, GameObject>();
			ItemNum = 0;
			RoomConfig roomconfig = Globals.Instance.MDataTableManager.GetConfig<RoomConfig>();
//			if(0 != iNewLevel)
//			{
//				ItemNum++;
//				GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
//				UILabel NameLabel = MyLevelUpObj.transform.FindChild("NameLabel").GetComponent<UILabel>();
//				UILabel DescribeLabel = MyLevelUpObj.transform.FindChild("DescribeLabel").GetComponent<UILabel>();
//				Transform Numtransform = MyLevelUpObj.transform.FindChild("Number");
//				
//				UILabel CurrentLevNumLabel = Numtransform.FindChild("CurrentLevNumLabel").GetComponent<UILabel>();
//				UILabel LastLevNumLabel = Numtransform.FindChild("LastLevNumLabel").GetComponent<UILabel>();
//				Temp.Add(ItemNum,MyLevelUpObj);
//				
//			}
			if(0 != iNewEnergy)
			{
				ItemNum++;
				GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
				UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
				UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
				Transform Numtransform = MyLevelUpObj.transform.Find("Number");
				NameLabel.text = Globals.Instance.MDataTableManager.GetWordText(16004);
				DescribeLabel.text = Globals.Instance.MDataTableManager.GetWordText(16004) + "+" + iNewEnergy.ToString();
				
				//UILabel CurrentLevNumLabel = Numtransform.FindChild("CurrentLevNumLabel").GetComponent<UILabel>();
				//UILabel LastLevNumLabel = Numtransform.FindChild("LastLevNumLabel").GetComponent<UILabel>();
				NGUITools.SetActive(DescribeLabel.gameObject,true);
				NGUITools.SetActive(Numtransform.gameObject,false);
				Temp.Add(ItemNum,MyLevelUpObj);
			}
			if(0 != iNewLovepoint)
			{
				ItemNum++;
	
				GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
				UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
				UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
				Transform Numtransform = MyLevelUpObj.transform.Find("Number");
				NameLabel.text = Globals.Instance.MDataTableManager.GetWordText(16005);
				DescribeLabel.text = Globals.Instance.MDataTableManager.GetWordText(16005) + "+" + iNewEnergy.ToString();
				
				//UILabel CurrentLevNumLabel = Numtransform.FindChild("CurrentLevNumLabel").GetComponent<UILabel>();
				//UILabel LastLevNumLabel = Numtransform.FindChild("LastLevNumLabel").GetComponent<UILabel>();
				NGUITools.SetActive(DescribeLabel.gameObject,true);
				NGUITools.SetActive(Numtransform.gameObject,false);
				Temp.Add(ItemNum,MyLevelUpObj);
			}
			if(0 != iNewFlowerpotNum)
			{
				ItemNum++;
				GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
				
				UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
				UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
				Transform Numtransform = MyLevelUpObj.transform.Find("Number");
				
				UILabel CurrentLevNumLabel = Numtransform.Find("CurrentLevNumLabel").GetComponent<UILabel>();
				UILabel LastLevNumLabel = Numtransform.Find("LastLevNumLabel").GetComponent<UILabel>();
				
//				if(Globals.Instance.MGameDataManager.MActorData.RoomLevl < 2)
//				{
//					Globals.Instance.MGameDataManager.MActorData.RoomLevl = 2;
//				}
				int lastflowerpotnum = roomconfig.GetRoomObjectByRoomLev(iNewLevel - 1).Flowerpot_Count;
				LastLevNumLabel.text = lastflowerpotnum.ToString();
				
				int currentflowerpotnum = roomconfig.GetRoomObjectByRoomLev(iNewLevel).Flowerpot_Count;
				CurrentLevNumLabel.text = currentflowerpotnum.ToString();
				NameLabel.text = Globals.Instance.MDataTableManager.GetWordText(16006);
				
				NGUITools.SetActive(DescribeLabel.gameObject,false);
				NGUITools.SetActive(Numtransform.gameObject,true);
				
				Temp.Add(ItemNum,MyLevelUpObj);
			}
			List<sg.GS2C_Room_Info_Res.Function_Info> FunctionInfo = Globals.Instance.MGameDataManager.MActorData.FunctionInfo;

			int roomlv = iNewLevel;
			int flag = 0;
			foreach(sg.GS2C_Room_Info_Res.Function_Info data in FunctionInfo)
			{
				if(data.openlv == roomlv)
				{
			
					// 0 chat 1，SLEEP 2 null，3 ICEBOX 4 BATHROOM 5 CAMERA 6 POTTING
					switch(flag)
					{
					case 0:
					{}break;
					case 1:
					{
						ItemNum++;
						GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
						UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
						UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
						Transform Numtransform = MyLevelUpObj.transform.Find("Number");
						NameLabel.text =  Globals.Instance.MDataTableManager.GetWordText(16037);
						NGUITools.SetActive(DescribeLabel.gameObject,false);
						NGUITools.SetActive(Numtransform.gameObject,false);
						Temp.Add(ItemNum,MyLevelUpObj);	
					}break;
					case 2:
					{}break;
					case 3:
					{
						ItemNum++;
						GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
						UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
						UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
						Transform Numtransform = MyLevelUpObj.transform.Find("Number");
						NameLabel.text =  Globals.Instance.MDataTableManager.GetWordText(16038);
						NGUITools.SetActive(DescribeLabel.gameObject,false);
						NGUITools.SetActive(Numtransform.gameObject,false);
						Temp.Add(ItemNum,MyLevelUpObj);	
						
					}break;
					case 4:
					{}break;
					case 5:
					{}break;
					case 6:
					{
						ItemNum++;
						GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
						UILabel NameLabel = MyLevelUpObj.transform.Find("NameLabel").GetComponent<UILabel>();
						UILabel DescribeLabel = MyLevelUpObj.transform.Find("DescribeLabel").GetComponent<UILabel>();
						Transform Numtransform = MyLevelUpObj.transform.Find("Number");
						NameLabel.text =  Globals.Instance.MDataTableManager.GetWordText(16039);
						NGUITools.SetActive(DescribeLabel.gameObject,false);
						NGUITools.SetActive(Numtransform.gameObject,false);
						Temp.Add(ItemNum,MyLevelUpObj);	
					}break;
					}
				
				
				}
				flag++;
			}
			
//			if(0 != iNewInviteNum)
//			{
//				ItemNum++;
//				GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
//				Temp.Add(ItemNum,MyLevelUpObj);
//				
//			}
//			if(null != lNewFunctionLis)
//			{
//				ItemNum += lNewFunctionLis.Count;
//			}
			
			
			//float BackGroundH = LevelBackGround.transform.localScale.y;
			float ItemH  = LevelItem.transform.localScale.y;
			float gap = 1.0f;
			float tempY = ItemNum * ItemH + (ItemNum - 1) * gap;
			float firstY = (tempY - ItemH)/2;
			if(2 == ItemNum)
			{
				gap = 150.0f;
				tempY = ItemNum * ItemH + (ItemNum - 1) * gap;
				firstY = (tempY - ItemH)/2;
			}
			
			int i = 0;
			foreach(GameObject Obj in Temp.Values)
			{
				Obj.transform.parent = AllItem.transform;
				Obj.transform.localPosition  = new Vector3(0,firstY - i * (ItemH + gap),-12);
				Obj.transform.localScale = Vector3.one;
//				if(i % 2 == 0)
//				{
//					Transform sprite = Obj.transform.FindChild("LevlSprite");	
//					Transform sprite2 = Obj.transform.FindChild("LevlSprite2");	
//					NGUITools.SetActive(sprite.gameObject,false);
//					NGUITools.SetActive(sprite2.gameObject,true);
//				}
				++i;
			}
			NGUITools.SetActive(LevelUpScucessFrame,true);
			SuccessLabel.text = string.Format(Globals.Instance.MDataTableManager.GetWordText(16040),iNewLevel);
			string Scene = "SceneHome" + iNewLevel.ToString();
			//Globals.Instance.MSceneManager.ChangeScene("SceneHome1", Vector3.zero);
			Globals.Instance.MGameDataManager.MActorData.RoomLevl = iNewLevel;
			DisplayFunction();
		}
		else
		{

		}

	}


	private void OnClickExitLevelUpFrameBtn(GameObject Obj)
	{
		NGUITools.SetActive(LevelUpNeedsFrame,false);
		((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger = true; 
	}
	private void DisplayReward()
	{
		HelpUtil.DelListInfo(AllItem.transform);
		
		//float BackGroundH = LevelBackGround.transform.localScale.y;
		float ItemH  = 128;
		float gap = 1.0f;
		float tempY = ItemNum * ItemH + (ItemNum - 1) * gap;
		float firstY = (tempY - ItemH)/2;
		if(2 == ItemNum)
		{
			gap = 150.0f;
			tempY = ItemNum * ItemH + (ItemNum - 1) * gap;
			firstY = (tempY - ItemH)/2;
		}
		for(int i = 0; i < ItemNum;++i)
		{	
			GameObject MyLevelUpObj = GameObject.Instantiate(LevelUpObj)as GameObject;
			MyLevelUpObj.transform.parent = AllItem.transform;
			MyLevelUpObj.transform.localPosition = new Vector3(0,firstY - i * (ItemH + gap),0);
			MyLevelUpObj.transform.localScale = Vector3.one;
//			if(i % 2 == 0)
//			{
//				Transform texture = MyLevelUpObj.transform.FindChild("LevlTexture");	
//				Transform texture1 = MyLevelUpObj.transform.FindChild("LevlTexture2");	
//				NGUITools.SetActive(texture.gameObject,false);
//				NGUITools.SetActive(texture1.gameObject,true);
//			}
		}
		
		//LevelBackGround
		
	}
	
	//特效相关//
	public void DisplayIntimacyEffect(Vector3 vPos ,bool bScened = false,long iGirlID = -1,int iPartID = -1,bool isScucess = false)
	{
		mMousePos = vPos;
		vPos = mUIcamera.ScreenToWorldPoint(mMousePos);
	
		ParticleSystem particle = PartileDianjiObj.transform.GetComponent<ParticleSystem>();
		PartileDianjiObj.transform.position = new Vector3 (vPos.x,vPos.y,-1);
		particle.Simulate(1.5f,true,true);
		particle.Play();
		if(true == bScened)
		{
			GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(mGirlID);

		
		}
	}
	public void  ReceiveRoomInteractive(int iActionPart,bool  bActionSuccess,int iGirlIntimacy,int nowInteractiveCount,int maxInteractiveCount)
	{
		if(iGirlIntimacy <= 0)
		{
			return;
		}
		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
		{
			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
			if(nowInteractiveCount >= maxInteractiveCount)
			{
				home.particle.Stop();
			}
			else
				home.particle.Play();
		}
		GameObject obj = GameObject.Instantiate(IntimacyObj) as GameObject;
		obj.transform.parent = this.transform;
		//obj.transform.localPosition = new Vector3( vPos.x, vPos.y,obj.transform.position.z);
		Vector3 vPos = mUIcamera.ScreenToWorldPoint(mMousePos);
		obj.transform.position = new Vector3 (vPos.x,vPos.y,obj.transform.position.z);
		obj.transform.localScale = Vector3.one;	
		
		UILabel IntimacyLabel = obj.transform.Find("Label").GetComponent<UILabel>();
		IntimacyLabel.text = iGirlIntimacy.ToString();
		
		UISprite ScucessSprite = obj.transform.Find("ScucessSprite").GetComponent<UISprite>();
		if(false == bActionSuccess)
		{
			ScucessSprite.spriteName = "IconRefuse";
		}
		TweenGroup tweenGroup = obj.GetComponent<TweenGroup>();
		tweenGroup.playTweenAnimation();
		tweenGroup.TweenFinishedEvents += OnTweenGroupFinishedEvent;
		//Vector3 pos = EffectsSprite.transform.position;
		
	}


	private void OnTweenGroupFinishedEvent(GameObject Obj,bool isautoJump)
	{
		TweenGroup tweenGroup = Obj.GetComponent<TweenGroup>();
		tweenGroup.TweenFinishedEvents -= OnTweenGroupFinishedEvent;
		NGUITools.SetActive(Obj,false);
		//GameObject.DestroyImmediate(Obj);
		GameObject.DestroyObject(Obj);
	}
	private void TimerTickNotify()
	{
		if(mGiftCurrentTime > 0)
		{
			mGiftCurrentTime = mGiftNextTime - Mathf.CeilToInt(Time.time);
			int hour = (int)(mGiftCurrentTime/3600);
			int temp = (int)(mGiftCurrentTime%3600);
			int minute = temp/60;
			int second = temp%60;
			if( mGiftCurrentTime < 0)
			{
			 	mGiftCurrentTime = 0;
			}
			HoursLabel.text = hour < 10 ? "0" + hour.ToString () : hour.ToString ();
			HoursLabel.text = HoursLabel.text + " :";
			MinuteLabel.text = minute < 10 ? "0" + minute.ToString () : minute.ToString () + " :";
			SecondLabel.text = second < 10 ? "0" + second.ToString () : second.ToString ();
		}		
	}
	
	public void ReceiveRoomReEnergy(int iErrorCode,int iFunctionEnum,int iEffectEnergy)
	{
		if(10000 == iErrorCode)
		{
			switch(iFunctionEnum)
			{
			case (int)RoomFuncEnum.BEDDOUBLE:
			{

				string notice;
				if(mGirldata.PropertyData.Intimacy == 3 || mGirldata.PropertyData.Intimacy == 4 )
				{
					notice = Globals.Instance.MDataTableManager.GetWordText(16011);
//					notice = string.Format(notice,mGirldata.BasicData.Name);
					NGUITools.SetActive(GameObjectTalk,true);
					TalkLabel.text = notice;
				}
				else if(mGirldata.PropertyData.Intimacy == 5 || mGirldata.PropertyData.Intimacy == 6)
				{
					notice = Globals.Instance.MDataTableManager.GetWordText(16011);
//					notice = string.Format(notice,mGirldata.BasicData.Name);
					NGUITools.SetActive(GameObjectTalk,true);
					TalkLabel.text = notice;
	
				}	
				
			}break;
			case (int)RoomFuncEnum.BEDSINGLE:
			{
				string notice = Globals.Instance.MDataTableManager.GetWordText(16008);
				notice = string.Format(notice,iEffectEnergy);
				NGUITools.SetActive(GameObjectTalk,true);
				TalkLabel.text = notice;
				
			}break;
			}
		}
		else//失败//
		{
			switch(iFunctionEnum)
			{
			case (int)RoomFuncEnum.BEDDOUBLE:
			{

				string notice = Globals.Instance.MDataTableManager.GetWordText(16009);
				NGUITools.SetActive(GameObjectTalk,true);
				TalkLabel.text = notice;
				
			}break;
			case (int)RoomFuncEnum.BEDSINGLE:
			{

				string notice = Globals.Instance.MDataTableManager.GetWordText(16009);
				NGUITools.SetActive(GameObjectTalk,true);
				TalkLabel.text = notice;
				
			}break;
			}
			
		}
	}
	public void  ReceiveRoomReLove(int iErrorCode,int iEffectLovepoint)
	{
		if(10000 == iErrorCode)
		{

			NGUITools.SetActive(GameObjectTalk,true);
			mTalkList.Clear();
			Dictionary<long,GirlData> dicWarShipData =  Globals.Instance.MGameDataManager.MActorData.GetWarshipDataList();
			int Index =  Random.Range(0,dicWarShipData.Count);
			int i = 0;
			string name = "";
			foreach(GirlData data in dicWarShipData.Values)
			{
				if(i == Index)
				{
//					name = data.BasicData.Name;
					break;
				}
				++i;
			}
			PlayerData actorData = Globals.Instance.MGameDataManager.MActorData;
			string MyName = actorData.BasicData.Name;
			string notice = Globals.Instance.MDataTableManager.GetWordText(16014);
			notice = "   " + string.Format(notice,name,MyName);
			//GUISingleDrop.ShowTalkObj(notice);
			mTalkList.Add(notice);
			string notice2 = Globals.Instance.MDataTableManager.GetWordText(16003);
			notice2 = "   " + string.Format(notice2,iEffectLovepoint);
			mTalkList.Add(notice2);
			TalkLabel.text = notice;
				
		}
		else
		{
			string notice = Globals.Instance.MDataTableManager.GetWordText(16015);
			NGUITools.SetActive(GameObjectTalk,true);
			TalkLabel.text = notice;
		}
	}
	private void OnClickTalkBtn(GameObject Obj)
	{
		if(mTalkList.Count > 0)
		{
			mTalkList.Remove(mTalkList[0]);
			if(mTalkList.Count > 0)
				TalkLabel.text = mTalkList[0];
			else
			{
				NGUITools.SetActive(GameObjectTalk,false);
			}
		}
		else
		{
			NGUITools.SetActive(GameObjectTalk,false);
		}
		
	}
	private void  DisplayBATHROOM(bool bOpen)
	{
		string notice = Globals.Instance.MDataTableManager.GetWordText(16016);
		NGUITools.SetActive(GameObjectTalk,true);
		TalkLabel.text = notice;
	}
	private void DisplayFunction()
	{
		UIImageButton []FunctionBtn = {DorImageBten[(int)MyImageBrn.INTERACT],   DorImageBten[(int)MyImageBrn.SLEEP],
									   null/*DorImageBten[(int)MyImageBrn.SEEEPDOUBLE]*/,DorImageBten[(int)MyImageBrn.ICEBOX],
									   DorImageBten[(int)MyImageBrn.BATHROOM],   DorImageBten[(int)MyImageBrn.CAMERA],
									   DorImageBten[(int)MyImageBrn.POTTING]};
		UIImageButton[]DisableFunctionBtn = {InteractiveDisableBtn,SLEEPDisableBtn ,null/*BedDoubleDisableBtn*/,
											IceboxDisableBtn,      BathroomDisableBtn, PhotoDisableBtn,     FlowerpotDisableBtn};

		List<sg.GS2C_Room_Info_Res.Function_Info> FunctionInfo = Globals.Instance.MGameDataManager.MActorData.FunctionInfo;
		int i = 0;
		int roomlv = Globals.Instance.MGameDataManager.MActorData.RoomLevl;
		// 0 chat 1，SLEEP 2 null，3 ICEBOX 4 BATHROOM 5 CAMERA 6 POTTING
		foreach(sg.GS2C_Room_Info_Res.Function_Info data in FunctionInfo)
		{
			if(null != DisableFunctionBtn[i])
			{
				NGUITools.SetActive(DisableFunctionBtn[i].gameObject,data.openlv > roomlv);
				if(data.openlv > roomlv)
				{
					UIImageButton disableimage = DisableFunctionBtn[i].gameObject.transform.GetComponent<UIImageButton>();
					sg.GS2C_Room_Info_Res.Function_Info function = (sg.GS2C_Room_Info_Res.Function_Info)disableimage.Data;
					string note = Globals.Instance.MDataTableManager.GetWordText(16031);
					note = string.Format(note,function.openlv);
					UILabel label = DisableFunctionBtn[i].gameObject.transform.Find("Label").GetComponent<UILabel>();
					label.text = note;
				}
			}
				
			if(null != FunctionBtn[i])
				NGUITools.SetActive(FunctionBtn[i].gameObject,data.openlv <= roomlv);
				
			++i;
		}
	}
	private void OnClickDisableBtn(GameObject Obj)
	{
		UIImageButton disableimage = Obj.transform.GetComponent<UIImageButton>();
		
		sg.GS2C_Room_Info_Res.Function_Info function = (sg.GS2C_Room_Info_Res.Function_Info)disableimage.Data;
		string note = "";
		if(Obj == SLEEPDisableBtn.gameObject)
		{
			note = Globals.Instance.MDataTableManager.GetWordText(16033);
		}
		else if(Obj == IceboxDisableBtn.gameObject)
		{
			note = Globals.Instance.MDataTableManager.GetWordText(16034);
		}
		else if(Obj == FlowerpotDisableBtn.gameObject)
		{
			note = Globals.Instance.MDataTableManager.GetWordText(16035);
		}
		
		
		//note = string.Format(note,function.openlv);
		Globals.Instance.MGUIManager.ShowSimpleCenterTips(note,true);
//		UILabel label = Obj.transform.FindChild("Label").GetComponent<UILabel>();
//		label.text = note;
	}
	private void OnClickExitScucessFrameBtn(GameObject Obj)
	{
		NGUITools.SetActive(LevelUpScucessFrame,false);
	}
	
//	public void ReceiveRoomInfo(sg.GS2C_Room_Info_Res res)
//	{
//		
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
//		{
//			HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
//			if(res.nowInteractiveCount >= res.maxInteractiveCount)
//				home.particle.Stop();
//			else
//				home.particle.Play();
//		}
//		
//	}

	
	ISubscriber netUpdatePackageItemSub;
	ISubscriber netOpenPackageSlotSub;

}
