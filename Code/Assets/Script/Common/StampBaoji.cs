
using UnityEngine;
using System.Collections;

public class StampBaoji : MonoBehaviour {
	
	/// <summary>
	/// The max scale.
	/// </summary>
	private static float		MaxScale	= 15;
	private Vector3 mInitScale;
	private GameObject mCloneStampObj;
	void Awake()
	{
		mInitScale = transform.localScale;
				
		
		NGUITools.SetActive(gameObject,false);
		
	}
	
	/// <summary>
	/// Stamp the specified position, animationTime and completeDelegate.
	/// </summary>
	/// <param name='stampPic'>
	/// stampPic object.
	/// </param>
	/// <param name='animationTime'>
	/// Animation time.
	/// </param>
	/// <param name='completeDelegate'>
	/// Complete delegate.
	/// </param>
	public void Stamp(float animationTime,iTween.EventDelegate completeDelegate){
				
		NGUITools.SetActive(gameObject,true);
		if(mCloneStampObj == null)
		{
			mCloneStampObj = GameObject.Instantiate(gameObject) as GameObject;
			mCloneStampObj.transform.parent = transform.parent;
			mCloneStampObj.transform.localPosition = transform.localPosition;
			mCloneStampObj.transform.localScale = mInitScale;
		}
	
		NGUITools.SetActive(mCloneStampObj,true);
		
		transform.localScale	= new Vector3(mInitScale.x*MaxScale,mInitScale.y*MaxScale,1);		
				
		iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(mInitScale.x,mInitScale.y,1),"time",animationTime,"easetype", iTween.EaseType.easeInSine), null, delegate()
		{
			GameObject.DestroyObject(mCloneStampObj);
			if(completeDelegate != null){
				completeDelegate();
			}

		});			
	}
}
