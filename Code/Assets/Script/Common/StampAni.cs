using UnityEngine;
using System.Collections;

public class StampAni : MonoBehaviour {
	
	/// <summary>
	/// The max scale.
	/// </summary>
	private static float		MaxScale	= 10;
	
	void Awake()
	{
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
	public void Stamp(UISprite stampPic,float animationTime,iTween.EventDelegate completeDelegate){
		
		transform.parent		= stampPic.transform.parent;
		transform.localPosition	= new Vector3(stampPic.transform.localPosition.x,stampPic.transform.localPosition.y,stampPic.transform.localPosition.z - 1);
		transform.localScale	= new Vector3(MaxScale,MaxScale,1);		
		transform.localRotation	= Quaternion.Euler(stampPic.transform.localRotation.eulerAngles);		
		
		iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(1,1,1),"time",animationTime,"easetype", iTween.EaseType.easeInSine), null, delegate()
		{
			GameObject.Destroy(gameObject);
			stampPic.transform.localScale = Vector3.zero;
			
			// iTween.MoveTo(gameObject,iTween.Hash("position",new Vector3(0,1000,transform.localPosition.z),"time",animationTime * 0.5f,
			// 									// "easetype", iTween.EaseType.easeInSine,"isLocal",true,"delay",animationTime),null,null);
			// 									"easetype", iTween.EaseType.easeInSine,"isLocal",true),null,null);
			// 
			// iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(MaxScale,MaxScale,1),"time",animationTime * 0.5f,
			// 										"easetype", iTween.EaseType.easeInSine/*,"delay",animationTime*/), delegate(){
			// 	
			// 	if(completeDelegate != null){
			// 		completeDelegate();
			// 	}
			// 	
			// 	GameObject.Destroy(gameObject);
			// 	stampPic.transform.localScale = Vector3.zero;
			// });
		});			
	}
	
	public void Stamp(float animationTime,iTween.EventDelegate completeDelegate){
		
		transform.localScale	= new Vector3(MaxScale,MaxScale,1);		
		iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(1,1,1),"time",animationTime,"easetype", iTween.EaseType.easeInSine), null, delegate()
		{
			if (null != completeDelegate)
				completeDelegate();
		});			
	}
	
}
