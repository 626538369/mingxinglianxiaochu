using UnityEngine;
using System.Collections;
/// <summary>
/// 
/// </summary>
public class CameraPath : MonoBehaviour {
	
	//public List<Transform>  camTarget;
	public Transform  camTarget01 = null;
	public Transform  camTarget02 = null;
	public Transform  camTarget03 = null;
	
	public Transform  camTarget11 = null;
	public Transform  camTarget12 = null;
	public Transform  camTarget13 = null;
	
	public Transform  camTarget21 = null;
	public Transform  camTarget22 = null;
	public Transform  camTarget23 = null;
	
	private const  int _horizonSize = 3;
	private const  int _verticalSize = 3;
	
	private Transform [,]  _camList;
	private Vector2        _camCurrentPos = new Vector2 (1,0);
	private double         _lastMoveTime = 0;
	
	enum direction
	{
		Up =0,
		UpRight,
		Right,
		RightDown,
		Down,
		DownLeft,
		Left,
		LeftUp,
	}
//	public Transform  camTarget03 = null;
	//public Transform  camTarget03 = null;
	// Use this for initialization
	void Start () {
		_camList =  new Transform [_horizonSize,_verticalSize]
		{
			{camTarget01,camTarget11,camTarget21},
			{camTarget02,camTarget12,camTarget22},
			{camTarget03,camTarget13,camTarget23}
		};
			//{camTarget01,camTarget02,camTarget03},{camTarget11,camTarget12,camTarget13}};
		
	}
	
	public void GetDefaultPostion(out Vector3 pos,out Quaternion rotation)
	{
		pos = _camList[1,0].position;
		rotation = _camList[1,0].rotation;
	}
	
	public bool MoveCamera(Vector2 offset,out Vector3 pos,out Quaternion rotation)
	{
		//offset = -offset;
		offset.y = -offset.y;
		//Debug.Log(offset.x+","+offset.y);
		pos = Vector3.zero;
		rotation = Quaternion.identity;
		
		if(Time.realtimeSinceStartup - _lastMoveTime<1)
			return false;
		
		
		
		
		float angle = Mathf.Atan2(offset.x, offset.y);
		if(angle<0)
			angle+=Mathf.PI*2;
		//angle+=Mathf.PI/8;
		
			
		
		float dirval = angle*Mathf.Rad2Deg;
		
		Vector2 moveDelta=Vector2.zero;
		if(dirval<30||dirval >330)
			moveDelta = new Vector2(0,1);
		else if(dirval>=30 && dirval < 60)
			moveDelta = new Vector2(1,1);
		else if(dirval>=60 && dirval < 120)
			moveDelta = new Vector2(1,0);
		else if(dirval>=120 && dirval < 150)
			moveDelta = new Vector2(1,-1);
		else if(dirval>=150 && dirval < 210)
			moveDelta = new Vector2(0,-1);
		else if(dirval>=210 && dirval < 240)
			moveDelta = new Vector2(-1,-1);
		else if(dirval>=240 && dirval < 300)
			moveDelta = new Vector2(-1,0);
		else if(dirval>=300 && dirval < 330)
			moveDelta = new Vector2(-1,1);
		
		Vector2 newSize = _camCurrentPos + moveDelta;
		
		if(newSize.x < 0 || newSize.x >=_horizonSize ||newSize.y < 0 || newSize.y >= _verticalSize )
			return false;
		
		Debug.Log(newSize.x +":"+ newSize.y);
		pos = _camList[(int)newSize.x,(int)newSize.y].position;
		rotation = _camList[(int)newSize.x,(int)newSize.y].rotation;
		
		
		Debug.Log("offsetx:"+offset.x + " offsety:"+offset.y +"  oldSize:"+_camCurrentPos.x+","+_camCurrentPos.y+"  newsize:"+newSize.x +","+ newSize.y+"  movedelta:"+moveDelta.x +"," +moveDelta.y 
			+ "  posx:"+pos.x+"  posy:"+pos.y);
		
		_camCurrentPos = newSize;
		_lastMoveTime = Time.realtimeSinceStartup;
		
		
		
		
		
		
		return true;
		
	}
}
