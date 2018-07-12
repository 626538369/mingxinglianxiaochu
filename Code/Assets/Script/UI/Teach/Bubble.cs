using UnityEngine;
using System;

public class Bubble : MonoBehaviour {

	//! the bubble sprite name
	private static readonly string[]		BUBBLE_PIC_NAME = 
	{
		"UIBackground",
	};
	
	
	//! the bubble sprite index
	protected static readonly int			TopLeft 	= 0;
	protected static readonly int			Top 		= 1;
	protected static readonly int			TopRight 	= 2;
	protected static readonly int			Right	 	= 3;
	
	protected static readonly int			BottomRight = 4;
	protected static readonly int			Bottom 		= 5;
	protected static readonly int			BottomLeft 	= 6;
	protected static readonly int			Left 		= 7;
	
	protected static readonly int			Inner 		= 8;
	
	//! the border of bubble sprite
	public float						BubbleTextBorder = 20;
		
	//! 4 coner sprite size 
	protected static float				SqrtTwo			= Mathf.Sqrt(2) / 2;
	
		//! attach transform 
	[HideInInspector] public Transform		m_attachTransform = null;
	
	//! sprite text
	protected UILabel				m_spriteText = null;
		
	//! bubble size
	protected Transform[]				m_bubbleSprite = new Transform[BUBBLE_PIC_NAME.Length];
	
	//! bubble original size
	protected Vector2[]					m_bubbleSpirteSize = new Vector2[BUBBLE_PIC_NAME.Length];
	
	//! the whole bubble size
	protected Vector2					m_size = Vector2.zero;
	
	
	//! the bubble popup start position
	private static Vector3 		PromptBubbleStartPos	= new Vector3(0,0,-1);
	private static Vector3 		PromptBubbleStartScale	= new Vector3(0.01f,0.01f,1);
	
	private static float		PromptBubblePopupInterval = 0.5f;
	
		
	//! the prompt bubble show timer
	private float				mBubblePopupShowTimer = 0.0f;
	
	//! show bubble state
	private bool				mBubblePopupShow = false;
	
	//  get the parent game Object
	public GameObject	ParentGameObject{
		get{return transform.parent.gameObject;}
	}
	
	protected void Awake(){
		
		// set the bubble sprite
		for(int i = 0;i < BUBBLE_PIC_NAME.Length;i++){
			m_bubbleSprite[i] 		= gameObject.transform.Find(BUBBLE_PIC_NAME[i]);
			
			UISprite t_ps		= m_bubbleSprite[i].gameObject.GetComponent<UISprite>();
			m_bubbleSpirteSize[i]	= new Vector2(t_ps.transform.localScale.x,t_ps.transform.localScale.y);
		}
		
		Transform t_spriteText;
		if((t_spriteText = gameObject.transform.Find("Text")) == null){
			t_spriteText = gameObject.transform.parent.Find("Text");
			t_spriteText.transform.parent = transform;
		}
		
		m_spriteText 	= t_spriteText.gameObject.GetComponent(typeof(UILabel)) as UILabel;		

	}
	
	//! get the bubble size
	public virtual Vector2 Size{
		get{
			return m_size;
		}
	}
	
	/// <summary>
	/// Sets the bubble visible.
	/// </summary>
	/// <param name='visible'>
	/// Visible.
	/// </param>
	public void SetVisible(bool visible){

		NGUITools.SetActive(this.gameObject,visible);
	}
		
	/// <summary>
	/// Sets the width of the text max.
	/// </summary>
	/// <param name='_maxWidth'>
	/// _max width.
	/// </param>
	public void SetTextMaxWidth(float _maxWidth){
		//m_spriteText.maxWidth = _maxWidth - BubbleTextBorder * 2;
	}
	
	protected virtual void Update(){
		
		if(mBubblePopupShow){
			
			// chat bubble show
			//
			mBubblePopupShowTimer += Time.deltaTime;
			if(mBubblePopupShowTimer >= 5.0f){
				
				mBubblePopupShowTimer = 0;
				mBubblePopupShow = false;
				
				iTween.ScaleTo(ParentGameObject,PromptBubbleStartScale,PromptBubblePopupInterval);
				iTween.MoveTo(ParentGameObject,iTween.Hash("isLocal",true,"position",PromptBubbleStartPos,"time",PromptBubblePopupInterval),null,delegate(){
					SetVisible(false);
				});
			}
		}
	}
	
	/**
	 * popup the text and show the animation
	 */ 
	public void PopupText(String text,Vector2 parentSize){
		
		SetText(text);
					
		float t_x = (-(Size.x + parentSize.x) / 2);
		float t_y = ((Size.y + parentSize.y) / 2);
				
		if(!mBubblePopupShow){	
		
			ParentGameObject.transform.localScale		= PromptBubbleStartScale;
			ParentGameObject.transform.localPosition	= PromptBubbleStartPos;
			
			iTween.MoveTo(ParentGameObject,iTween.Hash("islocal",true,"position",new Vector3(t_x,t_y,-1),"time",PromptBubblePopupInterval),null,null);
			iTween.ScaleTo(ParentGameObject,new Vector3(1,1,1),PromptBubblePopupInterval);
			
		}else{
			
			iTween.Stop(ParentGameObject);
			
			ParentGameObject.transform.localScale	= new Vector3(1,1,1);
			ParentGameObject.transform.localPosition = new Vector3(t_x,t_y,-1);
		}
		
		mBubblePopupShowTimer = 0.0f;
		mBubblePopupShow = true;
		
		SetVisible(true);		
	}
	/**
	 * set the text and relayout the bubble 9 Sprite
	 * 
	 * @param _text 		set null if want to empty text (remain arrow)
	 */ 
	public virtual void SetText(string _text){
				
		if(_text == null || _text.Length == 0){
			HideText(true);
			
			UpdateBubbleSize(Vector2.zero);
			
		}else{
			///暂定不显示文字了///
			HideText(true);	
			
			//! set the text
			m_spriteText.text 			= _text.Replace("\\n","\n");
						
			//! change the bubble sprite relayout background
			Vector2 t_textSize 	= m_spriteText.relativeSize;
			
			//! add the border
			t_textSize.x += BubbleTextBorder;
			t_textSize.y += BubbleTextBorder;
			
			UpdateBubbleSize(t_textSize);
		}		
		
	}
	
	
	/// <summary>
	/// Hides the text except arrow
	/// </summary>
	/// <param name='_hide'>
	/// _hide.
	/// </param>
	protected void HideText(bool _hide){
		
		for(int i = 0;i < BUBBLE_PIC_NAME.Length;i++){
			NGUITools.SetActive(m_bubbleSprite[i].gameObject,!_hide);
		}
		NGUITools.SetActive(m_spriteText.gameObject,!_hide);
	}
	
	//! update the bubble size
	protected void UpdateBubbleSize(Vector2 _size){
				
		
	}
}


