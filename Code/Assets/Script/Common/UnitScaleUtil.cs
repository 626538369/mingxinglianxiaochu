using UnityEngine;  
using System.Collections;  
  
public class UnitScaleUtil:MonoBehaviour
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
	
	public enum EPlaceDirection
	{
		TOBIG,
		TOSMALL,
	}
	

	public EPlaceType PlaceType;
	public Vector2 offset;

	float zoomRatio;
	float widthRatio;
	float heightRatio;
	
	public Vector2 size;
	
	public EPlaceDirection direction;
	
	private  Vector3 mInitScale ;
	private  Vector3 mInitPosition;
	
	private bool mInited = false;
    void Start()  
    {  
		if (!mInited)
		{
			mInitScale = transform.localScale;
			mInitPosition = transform.localPosition;
			mInited = true;
		}
		if (direction == EPlaceDirection.TOBIG)
		{
			
			widthRatio = Globals.Instance.MGUIManager.widthRatio;
			heightRatio = Globals.Instance.MGUIManager.heightRatio;
			if(widthRatio >= heightRatio)
			{
				zoomRatio = widthRatio;
			}
			else
			{
				zoomRatio = heightRatio;
			}
			
			transform.localScale = new Vector3(mInitScale.x * zoomRatio /widthRatio,
											   mInitScale.y * zoomRatio/heightRatio,
											   mInitScale.z);
		}
		else
		{
			widthRatio = Globals.Instance.MGUIManager.widthRatio;
			heightRatio = Globals.Instance.MGUIManager.heightRatio;
			if(widthRatio >= heightRatio)
			{
				zoomRatio = heightRatio;
			}
			else
			{
				zoomRatio = widthRatio;
			}
			
			transform.localScale = new Vector3(mInitScale.x * zoomRatio /widthRatio,
											   mInitScale.y * zoomRatio/heightRatio,
											   mInitScale.z);
		}

		
		MTransform();
	}
	
	void MTransform()
	{
		float x = 0;
		float y = 0;
		switch(PlaceType)
		{
			case EPlaceType.LeftTop:
				x =  size.x /2f * ( zoomRatio /widthRatio - 1);
				y =  size.y / 2f *(zoomRatio /heightRatio - 1 );
			break;
		}
		
		transform.localPosition = new Vector3(mInitPosition.x + x ,mInitPosition.y - y,mInitPosition.z);
	}
}  