using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {
	bool IsRotate = true;
	
	float  lastDist;
	float pinchSpeed =0.1f;
	
	public Transform target;
	public Transform cam;
	public Vector3 offset = Vector3.one;
	private float cameraRotSide;
	private float cameraRotUp;
	private float cameraRotSideCur;
	private float cameraRotUpCur;
	private float distance;
	private float cameraLasetRotUpCur;
	
	public float upwardAngle = 8.0f;
	public float overlookAngle = -20f;
	private float currentOverlookAngle ;
	public HeadLookController headLook;
	
	private bool mbIsMouseDown = false;
	private Vector3 mLastPos;
	private bool _pitching = false;
	private Camera mOrbitcamera;
	
	private  float  ResetcameraRotSide ;
	private	 float  ResetcameraRotSideCur;
	private	 float  ResetcameraRotUp ;
	private	 float  ResetcameraRotUpCur;
	private	 float  Resetdistance ;
	private  float  ResetcameraLasetRotUpCur;
	
	
	
	public bool IsPitching 
	{
		set { _pitching = value;}
		get { return _pitching; }
	}
	
	void Start () 
	{
		IniState();
		mOrbitcamera = cam.GetComponent<Camera>();
		mOrbitcamera.nearClipPlane = 1.0f;
		
		
		 ResetcameraRotSide = transform.eulerAngles.y;
		 ResetcameraRotSideCur = transform.eulerAngles.y;
		 ResetcameraRotUp = transform.eulerAngles.x;
		 ResetcameraRotUpCur = transform.eulerAngles.x;
		 Resetdistance = -cam.localPosition.z;
		 ResetcameraLasetRotUpCur = cameraRotUpCur;//只限制俯仰角//
		
		currentOverlookAngle = overlookAngle;
	}

	public void IniState()
	{
		cameraRotSide = transform.eulerAngles.y;
		cameraRotSideCur = transform.eulerAngles.y;
		cameraRotUp = transform.eulerAngles.x;
		cameraRotUpCur = transform.eulerAngles.x;
		distance = -cam.localPosition.z;
		cameraLasetRotUpCur = cameraRotUpCur;//只限制俯仰角//
	}
	public void Reset()
	{
		cameraRotSide       = ResetcameraRotSide;
		cameraRotSideCur    = ResetcameraRotSideCur;
		cameraRotUp         = ResetcameraRotUp;
		cameraRotUpCur      = ResetcameraRotUpCur;
		distance            = Resetdistance;
		cameraLasetRotUpCur = ResetcameraLasetRotUpCur;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
//		if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MPortStatus )
//		{
//			if(((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus).bFinger == false)
//				return;
//		}
//		else
//		{
//			return;
//		}
		if( !IsRotate)
			return;
		if (IsPitching)
			return;
		if(true == Input.GetMouseButton(1))
		{
			if(true == mbIsMouseDown)
			{
				mbIsMouseDown = false;
				mLastPos = Input.mousePosition;
			}
		}
		if(Input.GetMouseButton(0) && false  == Input.GetMouseButton(1))
		{
			if(false == mbIsMouseDown)
			{
				mLastPos = Input.mousePosition;
			}
			float side = 0;
			float up = Input.mousePosition.y- mLastPos.y;
//			if(GameStatusManager.Instance.MCurrentGameStatus == GameStatusManager.Instance.MHomeStatus )
//			{
//				HomeStatus home = ((HomeStatus)GameStatusManager.Instance.MCurrentGameStatus);
				//if(true == home.bHorizontal)
				{
					side = Input.mousePosition.x - mLastPos.x;
					up = mLastPos.y - Input.mousePosition.y ;
				}
//				else
//				{
//					up	= mLastPos.x  - Input.mousePosition.x ;
//					side = Input.mousePosition.y- mLastPos.y;
//				}
//			}
		
			if(Mathf.Abs(side) >  Mathf.Abs(up))
			{
				cameraRotSide += side*0.3f;
			}
			else
			{
				cameraRotUp -= up*0.3f;
			}
			mbIsMouseDown = true;
			//mLastPos = Input.mousePosition;
		}
		
		if(Input.GetMouseButtonUp(0)|| true  == Input.GetMouseButtonUp(1))
		{
			mbIsMouseDown = false;
			mLastPos = Input.mousePosition;
			
		}
		cameraRotSideCur = Mathf.LerpAngle(cameraRotSideCur, cameraRotSide, Time.deltaTime*5);
		cameraRotUpCur = Mathf.Lerp(cameraRotUpCur, cameraRotUp, Time.deltaTime*5);
		
//		if (Input.GetMouseButton(1)) {
//			distance *= (1-0.1f*Input.GetAxis("Mouse Y")); 
//		}
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8		
		if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
		{
			var touch1 = Input.GetTouch(0);
			var touch2 = Input.GetTouch(1);
			
			float curDist = Vector2.Distance(touch1.position, touch2.position);
			
			if(curDist > lastDist)
			{
			      distance += Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/20;
			}
			else
			{
			      distance -= Vector2.Distance(touch1.deltaPosition, touch2.deltaPosition)*pinchSpeed/20;
			}
			lastDist = curDist;
		}
#endif
#if UNITY_EDITOR
		distance *= (1-1*Input.GetAxis("Mouse ScrollWheel"));
#endif
		distance = Mathf.Clamp(distance,-500f,-260f);
		//Debug.Log("distancex:" + Input.GetAxis("Mouse ScrollWheel").ToString());
		cam.localPosition = -Vector3.forward * distance;

		if(cam.localPosition.z >432f)
		{
			currentOverlookAngle = ( Mathf.Abs(overlookAngle) - (Mathf.Abs( overlookAngle) - 23)/(500f-432f) * (cam.localPosition.z - 432f) ) * -1f ;
		}
		else
		{
			currentOverlookAngle = overlookAngle;
		}
		Vector3 targetPoint = target.position;
		transform.position = Vector3.Lerp(transform.position, targetPoint + offset, Time.deltaTime);
		float neardis = (float )((int)(Mathf.Abs(distance) - 100)/20 * 20);
			
		int index = ((int)cameraRotUpCur + 180)/360;
		float min = index * 360 + currentOverlookAngle;
		float max = index * 360 + upwardAngle;
		if(!(cameraRotUpCur > min && cameraRotUpCur < max))
		{
			float temp1 = Mathf.Abs (cameraLasetRotUpCur - min);
			float temp2 = Mathf.Abs(cameraLasetRotUpCur - max);
			cameraRotUpCur = temp1 < temp2 ? min : max;
			cameraRotUp = cameraRotUpCur;
		}
		transform.rotation = Quaternion.Euler(cameraRotUpCur, cameraRotSideCur, 0);
		cameraLasetRotUpCur = cameraRotUpCur;
	}
	///
		
	void LateUpdate () 
	{
		//headLook.target = cam.position;
		if(Input.GetMouseButton(0) && false  == Input.GetMouseButton(1))
			mLastPos = Input.mousePosition;
	}
	public void SetRotationStatus(bool IsRotate)
	{
		this.IsRotate = IsRotate;
		if(IsRotate)
		{
			Reset();	
		}
	}
	public void pinchCamera(float distTheat)
	{

	}
}

