using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemNotifyText : MonoBehaviour {
	
	/// <summary>
	/// The viewable area actual.
	/// </summary>
	public Vector2 viewableAreaActual;
	
	/// <summary>
	/// The text prefab.
	/// </summary>
	public SpriteText		TextPrefab;
	
	/// <summary>
	/// The m clip rect.
	/// </summary>
	private Rect3D			mClipRect = new Rect3D();
	
	/// <summary>
	/// The m sprite text list.
	/// </summary>
	private List<MessageData>	mSpriteTextList = new List<MessageData>();
	
	/// <summary>
	/// The move position.
	/// </summary>
	private float			mMovePosition = 0;
	
	/// <summary>
	/// The width of the total message sprite text
	/// </summary>
	private float			mMoveTotalWidth = 0;
		
	/// <summary>
	/// The move speed pre second
	/// </summary>
	private static readonly float MoveSpeed		= 100;
	
	/// <summary>
	/// The message interval.
	/// </summary>
	private static readonly float MessageInterval = 400;
	
	/// <summary>
	/// The instance.
	/// </summary>
	private static SystemNotifyText Instance = null;
		
	// Use this for initialization
	void Awake () {
		// hide the original Text prefab
		TextPrefab.gameObject.active = false;
		DontDestroyOnLoad(gameObject);
	}
	
	void Start(){
		mClipRect.FromPoints(new Vector3(-viewableAreaActual.x * 0.5f, viewableAreaActual.y * 0.5f, 0),
							  new Vector3(viewableAreaActual.x * 0.5f, viewableAreaActual.y * 0.5f, 0),
							  new Vector3(-viewableAreaActual.x * 0.5f, -viewableAreaActual.y * 0.5f, 0));
		
		mClipRect.MultFast(transform.localToWorldMatrix);
	}
	
	
	/// <summary>
	/// Adds the message to scroll
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	private void AddMessageImpl(string message){
		SpriteText tText= (SpriteText)Instantiate(TextPrefab);
		tText.Text		= message;
		
		tText.transform.parent = transform;
		tText.transform.localPosition = new Vector3(0,0,-1);
		mSpriteTextList.Add(new MessageData(tText,tText.PixelSize));
		
		ClipMessageText();
	}
	
	/// <summary>
	/// Clips the message text.
	/// </summary>
	private void ClipMessageText(){
		mMoveTotalWidth = viewableAreaActual.x;
		
		float tStartPos = viewableAreaActual.x / 2;
		
		for(int i = 0 ;i < mSpriteTextList.Count;i++){
			if(i != 0){
				mMoveTotalWidth	+= MessageInterval;
				tStartPos		+= MessageInterval;
			}
			
			MessageData md = mSpriteTextList[i];
			
			mMoveTotalWidth += md.mSize.x;
			tStartPos		+= md.mSize.x / 2;
			
			float tCurrentPos	= tStartPos - mMovePosition;
			float tEdge			= (md.mSize.x + viewableAreaActual.x) / 2;
			
			md.mText.enabled 	= false;

			if(tCurrentPos <= tEdge){
				
				if(tCurrentPos > -tEdge){
					
					md.mText.enabled 		= true;
					md.mText.ClippingRect = mClipRect;
					
					md.mText.transform.localPosition = new Vector3(tCurrentPos,0,0);
				}
			}
			
			tStartPos	+= md.mSize.x / 2;
		}
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {
		ClipMessageText();
		
		mMovePosition += Time.deltaTime * MoveSpeed;
		if(mMovePosition >= mMoveTotalWidth){
			Destroy(gameObject);
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Vector3 ul, ll, lr, ur;

		ul = (transform.position - transform.TransformDirection(Vector3.right * viewableAreaActual.x * 0.5f * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableAreaActual.y * 0.5f * transform.lossyScale.y));
		ll = (transform.position - transform.TransformDirection(Vector3.right * viewableAreaActual.x * 0.5f * transform.lossyScale.x) - transform.TransformDirection(Vector3.up * viewableAreaActual.y * 0.5f * transform.lossyScale.y));
		lr = (transform.position + transform.TransformDirection(Vector3.right * viewableAreaActual.x * 0.5f * transform.lossyScale.x) - transform.TransformDirection(Vector3.up * viewableAreaActual.y * 0.5f * transform.lossyScale.y));
		ur = (transform.position + transform.TransformDirection(Vector3.right * viewableAreaActual.x * 0.5f * transform.lossyScale.x) + transform.TransformDirection(Vector3.up * viewableAreaActual.y * 0.5f * transform.lossyScale.y));

		Gizmos.color = new Color(1f, 0, 0.5f, 1f);
		Gizmos.DrawLine(ul, ll);	// Left
		Gizmos.DrawLine(ll, lr);	// Bottom
		Gizmos.DrawLine(lr, ur);	// Right
		Gizmos.DrawLine(ur, ul);	// Top
	}
	
	/// <summary>
	/// Adds the message to system notify 
	/// </summary>
	/// <param name='message'>
	/// Message.
	/// </param>
	public static void AddMessage(string message){
		
		if(Instance == null){
			Instance = (SystemNotifyText)Instantiate(Globals.Instance.M3DItemManager.SystemNotifyTextPrefab);
			Instance.transform.parent			= Globals.Instance.MGUIManager.MGUICamera.transform;
			Instance.transform.localPosition	= new Vector3(0,Screen.height / 2 * 0.75f,GUIManager.GUI_NEAREST_Z - 1);
			Instance.transform.localScale		= new Vector3(Globals.Instance.MGUIManager.widthRatio,Globals.Instance.MGUIManager.heightRatio,1);
		}
		
		Instance.AddMessageImpl(message);
	}
	
	private class MessageData{
		public SpriteText		mText;
		public Vector2			mSize;
		public MessageData(SpriteText text,Vector2 size){
			mText = text;
			mSize = size;
		}
	}
}
