using UnityEngine;
using System.Collections;

public class AttributeTipBubble : MonoBehaviour {
	
	private static readonly	float			InvisiblePosition	= 30000;
		
	public AttributeIcon	AttIcon;
	
	public SpriteText		AttDesc;
	
	public PackedSprite		Arrow;
	public PackedSprite		ArrowBottom;
	
	public PackedSprite		Background;
	
	/// <summary>
	/// The m attach icon.
	/// </summary>
	private SpriteRoot		mAttachSprite;
	
	private float			mDispearTimer = 0;
		
	void Awake(){
		
		Hide();
		DontDestroyOnLoad(gameObject);
		
		AttIcon.HideAttributeNumber = true;
	}
	
	/// <summary>
	/// Shows the attr tips.
	/// </summary>
	/// <param name='icon'>
	/// AttributeIcon
	/// </param>
	public void ShowAttrTips(AttributeIcon icon){
		
		ShowAttrTips(icon.AttributeIconSprite,AttributeIcon.GetAttributeNameIcon(icon.AttriType),
											AttributeIcon.GetAttributeDescIcon(icon.AttriType));

		AttIcon.AttriType		= icon.AttriType;
		AttIcon.AttributeIconSprite.Hide(false);
		AttIcon.UpdateAttributeTypeIcon();
	}
		
	/// <summary>
	/// Shows the attr tips.
	/// </summary>
	/// <param name='attachSprite'>
	/// Attach sprite.
	/// </param>
	/// <param name='title'>
	/// Title.
	/// </param>
	/// <param name='desc'>
	/// Desc.
	/// </param>
	public void ShowAttrTips(SpriteRoot attachSprite,string title,string desc){
		
		mDispearTimer = 0;
		
		float top;
		float left	= attachSprite.transform.position.x + 110;
		
		float tCmpWidth = (Screen.width / 2) - 20;
		
		if(left + Background.width / 2 > tCmpWidth){
			left -= left + Background.width / 2 - tCmpWidth;
		}else if(left - Background.width / 2 < -tCmpWidth){
			left += (-tCmpWidth) - (left - Background.width / 2);
		}
				
		float tOverlayZPos = -7;
		if(attachSprite.transform.position.y - Globals.Instance.MGUIManager.MGUICamera.transform.localPosition.y > 0){
			
			top	= attachSprite.transform.position.y - Arrow.height - attachSprite.height / 2 - Background.height / 2;
			transform.localPosition				= new Vector3(left,top,attachSprite.transform.position.z + tOverlayZPos);		
			
			Arrow.transform.position			= (new Vector3(-Arrow.width / 2,-(Arrow.height + attachSprite.height) / 2,tOverlayZPos)) + 
														attachSprite.transform.position;
			
			ArrowBottom.transform.localPosition		= new Vector3(0,InvisiblePosition,0);
			
		}else{
			top =  attachSprite.transform.position.y + Arrow.height + attachSprite.height / 2 + Background.height / 2;
			transform.localPosition				= new Vector3(left,top,attachSprite.transform.position.z + tOverlayZPos);		
			
			ArrowBottom.transform.position		= (new Vector3(Arrow.width / 2,((Arrow.height + attachSprite.height) / 2 + 1),tOverlayZPos)) + 
														attachSprite.transform.transform.position;
			
			Arrow.transform.localPosition			= new Vector3(0,InvisiblePosition,0);			
		}

		AttIcon.AttributeIconSprite.Hide(true);		
		
		AttIcon.AttributeName	= title;
		AttIcon.UpdateAttributeName();
		
		//AttDesc.Text			= desc;	
		
		mAttachSprite = attachSprite;
		gameObject.SetActiveRecursively(true);
		
		// scale background if text is larger than background
		//float tHeightMax = AttDesc.PixelSize.y + AttIcon.Size.y + 40;
		float tHeightMax = AttIcon.Size.y + 40;
		if(tHeightMax > Background.height){
			
			float tScale = tHeightMax / Background.height;
			
			Background.transform.localPosition	= new Vector3(0,-(Background.height) * (tScale - 1) * 0.5f,0);
			Background.transform.localScale 	= new Vector3(1,tScale,1);
		}else{
			Background.transform.localPosition	= Vector3.one;
			Background.transform.localScale		= Vector3.one;
		}
	}
	
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if(transform.localPosition.y < InvisiblePosition - 10){
			
			try{
				mDispearTimer += Time.deltaTime;
				if(mDispearTimer >= 6.0f){
					Hide ();
					return;
				}
				
				if(mAttachSprite.IsHidden() || !mAttachSprite.gameObject.active){
					Hide();
				}
				
				if(HelpUtil.GetButtonState(true)){
					Hide();
				}				
				
			}catch{
				Hide();			
			}
		}
	}
	
	/// <summary>
	/// Hide this instance.
	/// </summary>
	private void Hide(){
		try{
			transform.parent 		= null;
			transform.localPosition = new Vector3(0,InvisiblePosition,0);
		}catch{
			GameObject.Destroy(gameObject);
			
			// set the instance null
			Globals.Instance.MGUIManager.mAttributeTipInstance = null;
		}	
	}
	
	/// <summary>
	/// Determines whether this instance is hidden.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is hidden; otherwise, <c>false</c>.
	/// </returns>
	private bool IsHidden(){
		return transform.localPosition.y == InvisiblePosition;
	}
		
}
