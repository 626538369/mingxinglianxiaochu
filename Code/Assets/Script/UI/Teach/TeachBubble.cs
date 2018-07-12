using UnityEngine;
using System;

/**
 * Teach bubble to pin some position of screen
 * author: tzz
 * date: 2012-8-23
 */
public class TeachBubble : Bubble {
	
	//! the arrow move cycle
	public float						ArrowMoveCycle	= 2;
	
	//! the arrow move length(distance)
	public float						ArrowMoveLength = 2;
		
	//! arrow type
	public enum ArrowType{
		e_left,
		e_top,
		e_right,
		e_bottom,
		
		e_topLeft,
		e_topRight,
		e_bottomLeft,
		e_bottomRight,		
		
		e_noArrow,
	};
	
	//! arrow prefab name
	private static readonly string	BubbleArrowPrefabName = "JianTou";
		
	//! offset position
	private Vector2			m_posOffset = Vector2.zero;
	
	//! the arrow EZGUI component to get the size of this Object
	private UISprite	m_arrowComponent = null;
	
	//! the transform of arrow 
	private Transform		m_arrowTransform = null;
	
	//! the arrow type
	private ArrowType		m_arrowType		= ArrowType.e_noArrow;
	
	//! the move base position of arrow 
	private Vector3			m_arrowMoveBasePos;
	
	public delegate void ForTZZDelegate();
	[HideInInspector] public ForTZZDelegate mForTZZDelegate = null;
	
	#region Unity
	public void Awake(){
		
		base.Awake();
		
		// set the ui camera parent
		transform.parent 	= GameObject.Find("UICamera").transform;
				
		// get the arrow UIButton 
		//
		m_arrowTransform = gameObject.transform.Find(BubbleArrowPrefabName);
		m_arrowComponent = (m_arrowTransform.gameObject).GetComponent<UISprite>();
		
		SetArrowType(m_arrowType);
		
		Transform maskTransForm =  gameObject.transform.Find("MaskLayer");
		if (maskTransForm != null)
		{
			UIButton mMaskLayer = maskTransForm.GetComponent( typeof(UIButton) ) as UIButton;
        	 UIEventListener.Get(	mMaskLayer.gameObject).onClick += PressedButton;
		}
	}
	
	private void PressedButton(GameObject obj)
	{
		// tzz added
		// the talk list will be null if used in Teach scene
		// check TeachFirstEnterGame.cs for detail
		if(mForTZZDelegate != null)
		{
			mForTZZDelegate();
		}
		return;
	}
	public void Update(){
		
		if(m_arrowType != ArrowType.e_noArrow){
			
			Vector3 t_offset = Vector3.zero;
			
			float t_waveValue = Mathf.Sin(Time.time * ArrowMoveCycle) * ArrowMoveLength;
			
			// move the arrow
			//
			switch(m_arrowType){
			case ArrowType.e_bottom:
			case ArrowType.e_top:
				t_offset.y					= t_waveValue;
				break;
			case ArrowType.e_right:
			case ArrowType.e_left:
				t_offset.x					= t_waveValue;
				break;
			case ArrowType.e_topLeft:
			case ArrowType.e_bottomRight:
				t_offset.x	= t_waveValue;
				t_offset.y	= -t_waveValue;
				break;
			case ArrowType.e_topRight:
			case ArrowType.e_bottomLeft:
				t_offset.x	= t_offset.y = t_waveValue;
				break;
			}
			
			m_arrowTransform.transform.localPosition = m_arrowMoveBasePos + t_offset;
		}
		
		UpdatePosition();
	}
	#endregion
	
	//! the offset position(pixel in screen axes)
	public Vector2 OffsetPos{
		get{return m_posOffset;}
		set{m_posOffset = value;}
	}
	
	/**
	 * set the arrow type 
	 */ 
	public void SetArrowType(ArrowType _type){
		
		m_arrowType						= _type;
		
		m_arrowTransform.gameObject.SetActiveRecursively(true);
		
		m_arrowMoveBasePos = Vector3.zero;
		
		switch(m_arrowType){
		case ArrowType.e_bottom:
			m_arrowTransform.localEulerAngles = new Vector3(0,180,0);
			m_arrowMoveBasePos.y 					-= (m_arrowComponent.transform.localScale.y + m_size.y) / 2;
			break;
		case ArrowType.e_top:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(180,Vector3.forward);
			m_arrowMoveBasePos.y 					+= (m_arrowComponent.transform.localScale.y + m_size.y) / 2;
			break;
		case ArrowType.e_left:
			m_arrowTransform.localEulerAngles = new Vector3(0,180,90);
			//m_arrowTransform.localRotation 			= Quaternion.AngleAxis(-90,Vector3.forward)*Quaternion.AngleAxis(180,Vector3.right);
			m_arrowMoveBasePos.x 					-= (m_arrowComponent.transform.localScale.y + m_size.x) / 2;
			break;
		case ArrowType.e_right:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(90,Vector3.forward);
			m_arrowMoveBasePos.x 					+= (m_arrowComponent.transform.localScale.y + m_size.x) / 2;
			break;
			
		case ArrowType.e_topLeft:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(-135,Vector3.forward);
			m_arrowMoveBasePos.x					-= (m_arrowComponent.transform.localScale.y + m_size.x) / 2 * SqrtTwo;
			m_arrowMoveBasePos.y					+= (m_arrowComponent.transform.localScale.y + m_size.y) / 2 * SqrtTwo;
			break;
		case ArrowType.e_topRight:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(135,Vector3.forward);
			m_arrowMoveBasePos.x					+= (m_arrowComponent.transform.localScale.y + m_size.x) / 2 * SqrtTwo;
			m_arrowMoveBasePos.y					+= (m_arrowComponent.transform.localScale.y + m_size.y) / 2 * SqrtTwo;
			break;
		case ArrowType.e_bottomLeft:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(-45,Vector3.forward);
			m_arrowMoveBasePos.x					-= (m_arrowComponent.transform.localScale.y + m_size.x) / 2 * SqrtTwo;
			m_arrowMoveBasePos.y					-= (m_arrowComponent.transform.localScale.y + m_size.y) / 2 * SqrtTwo;
			break;
		case ArrowType.e_bottomRight:
			m_arrowTransform.localRotation 			= Quaternion.AngleAxis(45,Vector3.forward);
			m_arrowMoveBasePos.x					+= (m_arrowComponent.transform.localScale.y + m_size.x) / 2 * SqrtTwo;
			m_arrowMoveBasePos.y					-= (m_arrowComponent.transform.localScale.y + m_size.y) / 2 * SqrtTwo;
			break;
		case ArrowType.e_noArrow:
			m_arrowTransform.gameObject.SetActiveRecursively(false);
			break;
		}
		
		m_arrowTransform.transform.localPosition = m_arrowMoveBasePos;
	}
	
	//! override the bubble set text and set the arrow position
	public override void SetText(string _text){
		base.BubbleTextBorder = 45;
		base.SetText(_text);
		
		//! reset the arrow position
		SetArrowType(m_arrowType);
	}
	
	//! get the arrow type 
	public ArrowType GetArrowType(){
		return m_arrowType;		
	}
	
	//! get the whole bubble size in pixel of UICamera
	public override Vector2 Size{
		get{
			if(m_size == Vector2.zero){
				// text size is zero
				if(m_arrowType == ArrowType.e_noArrow){
					return m_size;
				}else{
					return new Vector2(m_arrowComponent.transform.localScale.x,m_arrowComponent.transform.localScale.y);
				}
			}
			
			return m_size;
		}
	}
	
	//! update the position of bubble which has attach Gameobject
	public void UpdatePosition(){
		if(m_attachTransform != null){
			Vector3 t_pos;
			
			if(m_attachTransform.gameObject.layer == 8){
				// if this attach Transform's layer is GUI
				t_pos = m_attachTransform.position;
			}else{
				t_pos = GUIManager.WorldToGUIPoint(m_attachTransform.position);
			}
			
			t_pos.z -= 1.0f;
						
			switch(m_arrowType){
			case ArrowType.e_bottom:
				t_pos.y 		+= m_arrowComponent.transform.localScale.y + m_size.y / 2;
				break;				
			case ArrowType.e_top:
				t_pos.y 		-= m_arrowComponent.transform.localScale.y + m_size.y / 2;
				break;
			case ArrowType.e_left:
				t_pos.x 		+= m_arrowComponent.transform.localScale.x + m_size.x / 2;
				break;
			case ArrowType.e_right:
				t_pos.x 		-= m_arrowComponent.transform.localScale.x + m_size.x / 2;
				break;
			case ArrowType.e_topLeft:
				t_pos.x 		+= (m_arrowComponent.transform.localScale.x * SqrtTwo 	+ m_size.x / 2);
				t_pos.y 		-= (m_arrowComponent.transform.localScale.y * SqrtTwo 	+ m_size.y / 2);
				break;
			case ArrowType.e_topRight:
				t_pos.x 		-= (m_arrowComponent.transform.localScale.x * SqrtTwo 	+ m_size.x / 2);
				t_pos.y 		-= (m_arrowComponent.transform.localScale.y * SqrtTwo 	+ m_size.y / 2);
				break;
			case ArrowType.e_bottomLeft:
				t_pos.x 		+= (m_arrowComponent.transform.localScale.x * SqrtTwo 	+ m_size.x / 2);
				t_pos.y 		+= (m_arrowComponent.transform.localScale.y * SqrtTwo 	+ m_size.y / 2);
				break;
			case ArrowType.e_bottomRight:
				t_pos.x 		-= (m_arrowComponent.transform.localScale.x * SqrtTwo 	+ m_size.x / 2);
				t_pos.y 		+= (m_arrowComponent.transform.localScale.y * SqrtTwo 	+ m_size.y / 2);
				break;
			}
			
			t_pos.x += m_posOffset.x;
			t_pos.y += m_posOffset.y;
			
			transform.localPosition = t_pos;
		}
	}
	
}

