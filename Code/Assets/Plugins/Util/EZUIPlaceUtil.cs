using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[System.Serializable]
public class EZUIPlaceUtil : MonoBehaviour
{
	public enum EPlaceType
	{
		LeftTop,
		Left,
		LeftBottom,
		Bottom,
		RightBottom,
		Right,
		RightTop,
		Top,
		BeyondLeftTop,
		BeyongLeft,
		BeyondRight,
		BeyongBottom,
		BeyongRightBottom,
		StrethHorizontal,
		Center,
		
	}
	
	[System.Serializable]
	public class Place
	{
		public EPlaceType type;
		public Vector2 offset;
	}
	
	float widthRatio;
	float heightRatio;
	float zoomRatio;
	public Vector2 size;
	public Place place;
	
	// Use this for initialization
	void Start ()
	{
		
		widthRatio = Screen.width /1536f ;
		heightRatio = Screen.height / 2048f;
		if(widthRatio <= heightRatio)
		{
			zoomRatio = heightRatio;
		}
		else
		{
			zoomRatio = widthRatio;
		}
		
		widthRatio = heightRatio = zoomRatio;
		
		MScale();
		MTransform();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//return;
		if(Application.isEditor && !Application.isPlaying)
		{
			MTransform();
		}
	}
	
	void MScale()
	{
	//	Debug.Log(transform.name);
		
		//transform.localScale = new Vector3(zoomRatio,zoomRatio,1);
		Debug.Log("--------------"+transform.localScale.x+"-"+transform.localScale.y);
	}
	
	void MTransform()
	{
		float x = 0;
		float y = 0;
		float xWidth = 1536;
		float yHeight = 2048;
		//if(Application.isEditor && !Application.isPlaying)
		{
			switch(place.type)
			{
				case EPlaceType.LeftTop:
					x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio - xWidth / 2f;
					y = yHeight / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
					break;
				case EPlaceType.RightTop:
					x =  xWidth / 2f -(place.offset.x * widthRatio  + size.x /2f * zoomRatio);
					y = yHeight / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
					break;
				case EPlaceType.LeftBottom:
					x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio - xWidth / 2f;
					y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - yHeight / 2f;
					break;
				case EPlaceType.RightBottom:
					x =  xWidth / 2f -(place.offset.x * widthRatio  + size.x /2f * zoomRatio);
					y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - yHeight / 2f;
					break;
				case EPlaceType.BeyondLeftTop:
					x =  -(place.offset.x * widthRatio  + size.x /2f) * zoomRatio - xWidth / 2f;
					y = yHeight / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
					break;
				case EPlaceType.BeyongLeft:
					x =  -(place.offset.x * widthRatio  + size.x /2f) * zoomRatio - xWidth / 2f;
					y = transform.localPosition.y;
					break;
				case EPlaceType.BeyongBottom:
					x = transform.localPosition.x;
					y = -(size.y / 2f * zoomRatio + place.offset.y * heightRatio) - yHeight / 2f;
					break;
				case EPlaceType.BeyongRightBottom:
					x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio + xWidth / 2f;
					y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - yHeight / 2f;
					break;
				case EPlaceType.Bottom:
					x =  transform.localPosition.x;
					y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - yHeight / 2f;
					break;
				case EPlaceType.BeyondRight:
					x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio + xWidth / 2f;
					y = transform.localPosition.y;
					break;
				case EPlaceType.StrethHorizontal:
					x =  transform.localPosition.x;
					y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - yHeight / 2f;
					transform.localScale = new Vector3(xWidth / 2048f,zoomRatio,1);
					break;
			}
			transform.localPosition = new Vector3(x,y,transform.localPosition.z);
		}
		//else
	//{
	//	switch(place.type)
	//	{
	//		case EPlaceType.LeftTop:
	//			x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio - Screen.width / 2f;
	//			y = Screen.height / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
	//			break;
	//		case EPlaceType.RightTop:
	//			x =  Screen.width / 2f -(place.offset.x * widthRatio  + size.x /2f * zoomRatio);
	//			y = Screen.height / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
	//			break;
	//		case EPlaceType.LeftBottom:
	//			x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio - Screen.width / 2f;
	//			y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - Screen.height / 2f;
	//			break;
	//		case EPlaceType.RightBottom:
	//			x =  Screen.width / 2f -(place.offset.x * widthRatio  + size.x /2f * zoomRatio);
	//			y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - Screen.height / 2f;
	//			break;
	//		case EPlaceType.BeyondLeftTop:
	//			x =  -(place.offset.x * widthRatio  + size.x /2f) * zoomRatio - Screen.width / 2f;
	//			y = Screen.height / 2f - size.y / 2f * zoomRatio - place.offset.y * heightRatio;
	//			break;
	//		case EPlaceType.BeyongLeft:
	//			x =  -(place.offset.x * widthRatio  + size.x /2f) * zoomRatio - Screen.width / 2f;
	//			y = transform.localPosition.y;
	//			break;
	//		case EPlaceType.BeyongBottom:
	//			x = transform.localPosition.x;
	//			y = -(size.y / 2f * zoomRatio + place.offset.y * heightRatio) - Screen.height / 2f;
	//			break;
	//		case EPlaceType.BeyongRightBottom:
	//			x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio + Screen.width / 2f;
	//			y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - Screen.height / 2f;
	//			break;
	//		case EPlaceType.Bottom:
	//			x =  transform.localPosition.x;
	//			y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - Screen.height / 2f;
	//			break;
	//		case EPlaceType.BeyondRight:
	//			x =  place.offset.x * widthRatio  + size.x /2f * zoomRatio + Screen.width / 2f;
	//			y = transform.localPosition.y;
	//			break;
	//		case EPlaceType.StrethHorizontal:
	//			x =  transform.localPosition.x;
	//			y = size.y / 2f * zoomRatio + place.offset.y * heightRatio - Screen.height / 2f;
	//			transform.localScale = new Vector3(Screen.width / xWidth,zoomRatio,1);
	//			break;
	//	}
	//	transform.localPosition = new Vector3(x,y,transform.localPosition.z);
	//}
	}
}
