using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterProcessor : MonoBehaviour , IFingerEventListener
{
	
	bool IsRotate = true;
	
	Vector2 mFingerStartPos;
	Vector3 mCameraMainInitPos;
	float minZoomAmount = 10.0f;
	float maxZoomAmount = 75.0f;
	bool mPinchBegin = false;
	bool mFingerMoveBegin = false;
	
	private Transform target;
	
	public bool IsIgnore = false;
	public enum TalkPlace
	{
		START = 0,
		GUICLOTHSHOP = 1,
		GUISHIPBAG = 2,
		GUIPAGEANTJOIN = 3,
		GUIGIRATTRIBUTE = 4,
		
	}
	
	public long GirlID;
	public TalkPlace talkPlace;
	string []place = {"GUIClothShop","GUIShipBag","GUIPageantJoin","GUIGirlAttribute"};
	public Transform TalkTransform;
	
//	Camera camera = Globals.Instance.MSceneManager.mTaskCamera;
	Camera mUIcamera;
	void Awake()
	{
		target = transform;
		mUIcamera = GameObject.Find("UICamera").GetComponent<Camera>();
	}
	
	void Start ()
	{
		Globals.Instance.MFingerEvent.Add3DEventListener(this);
		this.SetFingerEventActive(true);
//		switch(talkPlace)
//		{
//		case TalkPlace.GUICLOTHSHOP:
//		{
////			GUIClothShop gui = GameObject.Find("GUIClothShop").GetComponent<GUIClothShop>();
////			gui.mGirlInviteSlot.GirlSelcectEvents += delegate(long girlID) {
////			GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(girlID);
////			GirlID = wsData.BasicData.LogicID;
////			};
//		}break;
//		case TalkPlace.GUISHIPBAG:
//		{
////			GameObject obj = GameObject.Find("GUIShipBag");
////			if(null != obj)
////			{
////				GUIShipBag gui = obj.GetComponent<GUIShipBag>();
////				if(null != gui)
////				{
////					gui.girlInviteSlot.GirlSelcectEvents +=delegate(long girlID) {
////					GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(girlID);
//////					GirlID = wsData.BasicData.LogicID;
////					};
////					break;
////				}
////			}
//	
//	
//			
////			GUIChangeCloth gui2 = GameObject.Find("GUIChangeCloth").GetComponent<GUIChangeCloth>();
////			if(null != gui2)
////			{
////				gui2.GirlSelcectEvents += delegate(long girlID) {
////				GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(girlID);
//////				GirlID = wsData.BasicData.LogicID;	
////				};
////			}
//		
//		}break;
//		case TalkPlace.GUIPAGEANTJOIN:
//		{
////			GUIPageantJoin gui = GameObject.Find("GUIPageantJoin").GetComponent<GUIPageantJoin>();
////			gui.mGirlInviteSlot.GirlSelcectEvents += delegate(long girlID) {
////			GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(girlID);
////			GirlID = wsData.BasicData.LogicID;
////			};
////			GirlID = 1217001000;
//		}break;
//		case TalkPlace.GUIGIRATTRIBUTE:
//		{
////			GUIGirlAttribute gui = GameObject.Find("GUIGirlAttribute").GetComponent<GUIGirlAttribute>();
////			gui.GirlSelcectEvents += delegate(long girlID) {
////			GirlData wsData = Globals.Instance.MGameDataManager.MActorData.GetGirlData(girlID);
////			GirlID = wsData.BasicData.LogicID;
////			};
//		}break;
//			
//		}
	}
	
	void OnDestroy()
	{
		Globals.Instance.MFingerEvent.Remove3DEventListener(this);
	}
	
	public bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		mFingerMoveBegin = true;
		mFingerStartPos = fingerPos;
		 Debug.Log(" OnFinger move begin  ="  +fingerPos);
		return false;
	}
	public  bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		if(!IsRotate)
			return false;
		if(true == IsIgnore)
			return false;
		if (target == null)
			return false;
		if (mPinchBegin)
			return false;
		if (!mFingerMoveBegin)
			return false;
		Vector2 changeFingerPos = fingerPos - mFingerStartPos;
		
		if ( System.Math.Abs(changeFingerPos.x) >  System.Math.Abs(changeFingerPos.y))
		{
			target.localEulerAngles = new Vector3(target.localEulerAngles.x,target.localEulerAngles.y -0.1f*changeFingerPos.x,target.localEulerAngles.z);
		}
		else{
	
		}
		return false;
	}
	public  bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		mFingerMoveBegin = false;
		return false;
	}
	void Update () 
	{
		if(!IsRotate)
			return;
		if(false == IsIgnore)
			return;
#if UNITY_EDITOR
		if(Input.GetMouseButton(0) && false  == Input.GetMouseButton(1))
#endif
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8

		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
#endif
		{
			float dis = Input.GetAxis("Mouse X");
			target.localEulerAngles = new Vector3(target.localEulerAngles.x,target.localEulerAngles.y -10f* dis,target.localEulerAngles.z);
			//Debug.Log("dis = " + dis.ToString());
		}
	}
	
	public bool IsFingerEventActive(){return true;}
	public void SetFingerEventActive(bool active){;}	
	public bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		GameObject hitObject = SceneQueryUtil.PickObject(Globals.Instance.MSceneManager.mTaskCamera,fingerPos, TagMaskDefine.GFAN_NPC);
		if (null != hitObject)
		{
			
//	
		}
		Debug.Log("OnFingerUpEvent");
		return true;
	}
	public bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
	{
		Debug.Log("OnFinger Down ");
		return true;	
	}
	
	public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		mPinchBegin = true;
		return false;
	}
	public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
	{
		Debug.Log("OnFingerPinchMove !!!!!!!!!!!!!!!!!!!!!!");
		return false;
		delta=delta*-0.1f;
		float zoomAmount=delta + Globals.Instance.MSceneManager.mMainCamera.fov;
		zoomAmount = Mathf.Clamp( zoomAmount, minZoomAmount, maxZoomAmount );
		Globals.Instance.MSceneManager.mMainCamera.fov = zoomAmount;

	}
	public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
	{
		mPinchBegin = false;
		return false;
	}
	public void SetRotationStatus(bool IsRotate)
	{
		this.IsRotate = IsRotate;
	}
	
	
	public bool OnFingerStationaryBeginEvent( int fingerIndex, Vector2 fingerPos ){return false;}
    public bool OnFingerStationaryEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime ){return false;}
    public bool OnFingerStationaryEndEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime ){return false;}
	
	
	public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos ){return false;}
	public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta ){return false;}
	public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos ){return false;}	
}