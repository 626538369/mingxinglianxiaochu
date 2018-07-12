using UnityEngine;
using System.Collections;
/// <summary>
/// Role scroll data struct.   
/// single scroll panel :　simpleSprite , note 
/// </summary>

public class RoleScrollStruct
{
	public static readonly int MaxRoleCnt = 3;
	
	//node
	// from left to right stand 0-4
	private class Node
	{
		public static Vector3 _originPosition = new Vector3(0, 0, -180);
		public static float _slopePositionX = 120;//0.2f;
		public static float _slopePositionZ = 30;//1f;
		
		public static Vector3 _originScale = new Vector3(1,1,1);
		public static Vector3 _slopeScale = new Vector3(0.15f,0.15f,0.15f);
		
		public static float _originAlpha = 1f;
		public static float _slopeAlpha = 0f;     
		
		public static Vector3 [] _position = 
		{
			//_originPosition + 2f * new Vector3(-_slopePositionX,0,_slopePositionZ),
			_originPosition + 1 * new Vector3(-_slopePositionX,0,_slopePositionZ),
			_originPosition,
			_originPosition + 1 * new Vector3(_slopePositionX,0,_slopePositionZ),
			//_originPosition + 2f * new Vector3(_slopePositionX,0,_slopePositionZ)
		};
		
		public static Vector3 [] _scale =    
		{
			//_originScale - 2 * _slopeScale,
			_originScale - 1 * _slopeScale,
			_originScale,
			_originScale - 1 * _slopeScale,
			//_originScale - 2 * _slopeScale
		};
		
		public static float [] _alpha =   
		{
			//_originAlpha - 2 * _slopeAlpha,
			_originAlpha - 1 * _slopeAlpha,
			_originAlpha,
			_originAlpha - 1 * _slopeAlpha,
			//_originAlpha - 2 * _slopeAlpha 
		};
	}
	
	//single scroll panel    
	private class SingleScrollPanel
	{
		public SingleScrollPanel(Transform parent,RoleScrollStruct dataStruct,int nodeIndex,string animationName)// panelIndex is from zero on
		{
			//set the aspect				
			this._dataStruct = dataStruct;
			this._nodeIndex = nodeIndex;//set the node index
			this.animationName = animationName;
			
			roleBG = GameObject.Instantiate(dataStruct.roleSpriteBG) as PackedSprite;
			roleBG.transform.parent = parent;
			//创建人物头像
			avatarSprite = GameObject.Instantiate(dataStruct.avatarSprite) as PackedSprite;
			avatarSprite.transform.parent = roleBG.transform;
			avatarSprite.transform.localPosition = new Vector3(0,-13.4f,-1f);
			avatarSprite.PlayAnim(animationName);

			roleBG2 = GameObject.Instantiate(dataStruct.roleSpriteBG2) as PackedSprite;
			roleBG2.transform.parent = roleBG.transform;
			roleBG2.transform.localPosition = new Vector3(0, 0, avatarSprite.transform.localPosition.z - 1.0f);
			roleBG2.transform.localScale = Vector3.one;
			
			//set the transform
			roleBG.transform.localPosition = Node._position[nodeIndex];
			roleBG.transform.localScale = Node._scale[nodeIndex];
		
			// if(nodeIndex == 2)
			// {
			// 	roleBG.PlayAnim("Active");
			// }
			// else
			// {
			// 	roleBG.PlayAnim("Normal");
			// }
			
			if(nodeIndex == MaxRoleCnt / 2)
			// if(nodeIndex == 2)
			{
				roleBG2.transform.localScale = Vector3.one;
			}
			else
			{
				roleBG2.transform.localScale = Vector3.zero;
			}
		}
		
		//scroll function 
		public void SlippingPanle(int direction,float unitRatio) 
		{
			//according to the direction to decide the movespan
			Vector3 moveSpan = Vector3.zero;

			if(direction < 0)// R To L
			{
				switch(_nodeIndex)
				{
				case 0:	
					
					moveSpan = new Vector3(-Node._slopePositionX,0,Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[0] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[0] - Node._slopeScale * unitRatio;
					
					break;
				case 1:
					
					moveSpan = new Vector3(-Node._slopePositionX,0,Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[1] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[1] - Node._slopeScale * unitRatio;
					
				    break;        
				case 2:

					moveSpan = new Vector3(-Node._slopePositionX,0,Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[2] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[2] - Node._slopeScale * unitRatio;
					
					break;
				// case 3:
				// 	
				// 	moveSpan = new Vector3(-Node._slopePositionX,0, - Node._slopePositionZ);
				// 	roleBG.transform.localPosition = Node._position[3] + moveSpan * unitRatio;
				// 	roleBG.transform.localScale = Node._scale[3] + Node._slopeScale * unitRatio;
				// 	
				//     break;
				// case 4:
				// 
				// 	moveSpan = new Vector3(-Node._slopePositionX,0, - Node._slopePositionZ);
				// 	roleBG.transform.localPosition = Node._position[4] + moveSpan * unitRatio;
				// 	roleBG.transform.localScale = Node._scale[4] + Node._slopeScale * unitRatio;
				// 	
				// 	break;
				}
			}
			else // L To R
			{
				switch(_nodeIndex)   
				{
				case 0:
					moveSpan = new Vector3(Node._slopePositionX,0, - Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[0] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[0] + Node._slopeScale * unitRatio;
					
					break;
				case 1:
					
					moveSpan = new Vector3(Node._slopePositionX,0, - Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[1] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[1] + Node._slopeScale * unitRatio;
					
					
				    break;
				case 2:
					moveSpan = new Vector3(Node._slopePositionX,0,Node._slopePositionZ);
					roleBG.transform.localPosition = Node._position[2] + moveSpan * unitRatio;
					roleBG.transform.localScale = Node._scale[2] - Node._slopeScale * unitRatio;

					break;
				// case 3:
				// 
				// 	moveSpan = new Vector3(Node._slopePositionX,0,Node._slopePositionZ);
				// 	roleBG.transform.localPosition = Node._position[3] + moveSpan * unitRatio;
				// 	roleBG.transform.localScale = Node._scale[3] - Node._slopeScale * unitRatio;
				// 	
				// 	
				//     break;
				// case 4:
				// 	moveSpan = new Vector3(Node._slopePositionX,0,Node._slopePositionZ);
				// 	roleBG.transform.localPosition = Node._position[4] + moveSpan * unitRatio;
				// 	roleBG.transform.localScale = Node._scale[4] - Node._slopeScale * unitRatio;
				// 	
				// 	
				// 	break;
				}
			}
		}
		
		//do the release animation
		public void ReleaseAnimation(int direction,bool isContinue)
		{
			if(direction < 0)// R To L
			{
				switch(_nodeIndex)
				{
				case 0:
					if(isContinue)
					{
						roleBG.transform.localPosition = Node._position[4];
						roleBG.transform.localScale = Node._scale[4];
						_nodeIndex = MaxRoleCnt - 1;
						_dataStruct._panelIndex ++;
						//change panel
					//	ChangePanel(this,_dataStruct._panelIndex,_nodeIndex);
					}
					else
					{							
						roleBG.transform.localPosition = Node._position[0];
						roleBG.transform.localScale = Node._scale[0];
						_nodeIndex = 0;
					}
					break;
				case 1:
					
					if(isContinue)
					{
						ItweenMove(0);
						//DoPositionAnimation(roleBG.gameObject,Node._position[0]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[0]);
						_nodeIndex = 0;
					}
					else
					{
						ItweenMove(1);
						//DoPositionAnimation(roleBG.gameObject,Node._position[1]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[1]);
					}
				    break;
				case 2:
					if(isContinue)
					{
						ItweenMove(1);
						//DoPositionAnimation(roleBG.gameObject,Node._position[1]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[1]);
						_nodeIndex = 1;
					}
					else
					{
						ItweenMove(2);
						//DoPositionAnimation(roleBG.gameObject,Node._position[2]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[2]);
					}
					break;
				// case 3:
				// 	if(isContinue)
				// 	{
				// 		ItweenMove(2);
				// 		//DoPositionAnimation(roleBG.gameObject,Node._position[2]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[2]);
				// 		_nodeIndex = 2;
				// 	}
				// 	else
				// 	{
				// 		ItweenMove(3);
				// 		//DoPositionAnimation(roleBG.gameObject,Node._position[3]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[3]);
				// 	}
				// 	
				//     break;
				// case 4:
				// 	
				// 	if(isContinue)
				// 	{
				// 		ItweenMove(3);
				// 		//DoPositionAnimation(roleBG.gameObject,Node._position[3]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[3]);
				// 		_nodeIndex = 3;
				// 	}
				// 	else
				// 	{
				// 		ItweenMove(4);
				// 		//DoPositionAnimation(roleBG.gameObject,Node._position[4]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[4]);
				// 	}
				// 	break;
				}
			}
			else// L To R
			{
				switch(_nodeIndex)
				{
				case 0:
					if(isContinue)
					{
						ItweenMove(1);
						//DoPositionAnimation(roleBG.gameObject,Node._position[1]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[1]);
						_nodeIndex = 1;  
					}
					else
					{	
						ItweenMove(0);
						//DoPositionAnimation(roleBG.gameObject,Node._position[0]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[0]);
					}
					
					break;
				case 1:
					if(isContinue)
					{
						ItweenMove(2);
						//DoPositionAnimation(roleBG.gameObject,Node._position[2]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[2]);
						_nodeIndex = 2;
					}
					else
					{
						ItweenMove(1);
						//DoPositionAnimation(roleBG.gameObject,Node._position[1]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[1]);
					}
				    break;
				case 2:
					if(isContinue)
					{
						ItweenMove(3);
						//DoPositionAnimation(roleBG.gameObject,Node._position[3]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[3]);
						_nodeIndex = MaxRoleCnt - 2;
					}
					else
					{
						ItweenMove(2);
						//DoPositionAnimation(roleBG.gameObject,Node._position[2]);
						DoScaleAnimation(roleBG.gameObject,Node._scale[2]);
					}
					//change the nodeIndex
					break;
				// case 3:
				// 	if(isContinue)
				// 	{
				// 		ItweenMove(4);
				// 		//DoPositionAnimation(roleBG.gameObject,Node._position[4]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[4]);
				// 		_nodeIndex = 4;
				// 	}
				// 	else
				// 	{
				// 		ItweenMove(3);
				// 		DoPositionAnimation(roleBG.gameObject,Node._position[3]);
				// 		DoScaleAnimation(roleBG.gameObject,Node._scale[3]);
				// 	}
				//     break;
				// case 4:
				// 	if(isContinue)
				// 	{
				// 		roleBG.transform.localPosition = Node._position[0];
				// 		roleBG.transform.localScale = Node._scale[0];
				// 		_nodeIndex = 0;
				// 		_dataStruct._panelIndex --;
				// 		//change panel
				// 	//	ChangePanel(this,_dataStruct._panelIndex,_nodeIndex);
				// 	}
				// 	else
				// 	{
				// 		roleBG.transform.localPosition = Node._position[4];
				// 		roleBG.transform.localScale = Node._scale[4];
				// 	}
				// 	break;
				}
			}
			_dataStruct.SetRoleSelect();
			//Debug.Log("==================");
		}
		
		public void ItweenMove(int positionIndex)
		{
			Vector3 des = Node._position[positionIndex];
			des = roleBG.transform.parent.transform.position + des;
			iTween.MoveTo(roleBG.gameObject, iTween.Hash("position", des, "time", 0.1f, "easetype", iTween.EaseType.easeOutExpo),
			null, null);
		}
		
		//translate animation
		public void DoPositionAnimation(GameObject gameObject,Vector3 desPosition)
		{
			AnimatePosition.Do
			(
				gameObject,
				EZAnimation.ANIM_MODE.To, 
				desPosition,
				EZAnimation.backIn,
				0.1f,
				0,
				null, // no starting delegate
	  			null// no ending delegate
			);
		}
		//scale animation
		
		public void DoScaleAnimation(GameObject gameObject,Vector3 desScale)
		{
			AnimateScale.Do
			(
				gameObject,
				EZAnimation.ANIM_MODE.To, 
				desScale,
				EZAnimation.backIn, 
				0.1f,
				0,
				null, // no starting delegate
	  			null// no ending delegate
			);	
		}
		// method to the role picture
		public void ChangeRolePic(int currMapIndex)
		{

		}
		
		//scale animation
		public void DoAnimateScale()
		{
			AnimateScale.Do
			(
				roleBG.gameObject,
				EZAnimation.ANIM_MODE.FromTo, 
				Vector3.zero,
				Vector3.one,	
				EZAnimation.spring, 
				0.5f,
				0,
				null, // no starting delegate
	  			null// no ending delegate
			);	
		}
		
		//change panel 
		public void ChangePanel(SingleScrollPanel currScrollPanel, int panelIndex,int nodeIndex)
		{
			//Debug.Log("panelIndex" + panelIndex + "=-==========  nodeIndex"+nodeIndex);	
			
			//currScrollPanel._text.Text = panelIndex+"";
		}
		
		private RoleScrollStruct _dataStruct;
		//get the node
		public Node _node = new Node();
		//the index of curr node
		public int _nodeIndex;
		public string animationName;
		public PackedSprite avatarSprite;
		public PackedSprite roleBG;
		public PackedSprite roleBG2;
	}
	
	//class for finger touch
	public class FingerTouch : IFingerEventListener
	{
		private bool _mIsFingerEventActive = false;
		public bool IsFingerEventActive()
		{
			return _mIsFingerEventActive;
		}
		
		public void SetFingerEventActive(bool active)
		{
			_mIsFingerEventActive = active;
		}
		
		public FingerTouch(RoleScrollStruct dataStruct)
		{
			this._dataStruct = dataStruct;
			//注册事件
			Globals.Instance.MFingerEvent.AddUIObjectEventListener(this);
			SetFingerEventActive(true);
		}

		public bool OnFingerDownEvent (int fingerIndex, Vector2 fingerPos)
		{
			return false;
		}
		
		public bool OnFingerUpEvent (int fingerIndex, Vector2 fingerPos, float timeHeldDown)
		{
			return false;
		}
		
		public bool OnFingerStationaryBeginEvent (int fingerIndex, Vector2 fingerPos)
		{
			return false;
		}
		
		public bool OnFingerStationaryEndEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
		{
			return false;
		}
		
		public bool OnFingerStationaryEvent (int fingerIndex, Vector2 fingerPos, float elapsedTime)
		{
			return false;
		}
		public bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
		{
			return false;
		}
		public bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
		{
			return true;
		}
		public bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
		{
			return true;
		}
		public bool OnFingerMoveBeginEvent (int fingerIndex, Vector2 fingerPos)
		{
			_lastX = fingerPos.x;
			_lastY = fingerPos.y;
			currRatio = 0;
			if(_lastX < _rightX && _lastX > _leftX && _lastY < _topY && _lastY > _bottomY)
			{
				_isDragOff = true;
			}
		//	Debug.Log("OnFingerMoveBeginEvent "+ fingerPos.x +"  " +fingerPos.y);
			return true;
		}
		
		public bool OnFingerMoveEvent (int fingerIndex, Vector2 fingerPos)
		{
			float currX = fingerPos.x;
			float currY = fingerPos.y;
			
			if(_isDragOff && (currX < _leftX || currX > _rightX || currY > _topY || currY < _bottomY))
			{
				_isDragOff = false;
				if(currRatio < 0.5f && currRatio > 0)
				{
					_isContinue = false;
				}
				else
				{
					_isContinue = true;
				}  
				return true;
			}
			
			float currSpan = currX - _lastX;
			
			if(_isDragOff && currSpan >= 0) // L To R
			{
				_direction = 1;
				if(currSpan < _unitSpan)
				{
					currRatio = currSpan / _unitSpan;
					_dataStruct.SlippingScroll(_direction,currRatio); 
				}
				else
				{
					_lastX = currX;
					_dataStruct._panelIndex --;
					_dataStruct.ChangeNodeIndex(1);
				}
			}
			
			if(_isDragOff && currSpan < 0)
			{
				_direction = -1;	
				if(currSpan > - _unitSpan)
				{
					currRatio = currSpan / -_unitSpan;
					_dataStruct.SlippingScroll(_direction,currRatio);
				}
				else
				{
					_lastX = currX;
					_dataStruct._panelIndex ++;
					_dataStruct.ChangeNodeIndex(-1);	
					
				} 
			}
			return true;
		}
		
		public bool OnFingerMoveEndEvent (int fingerIndex, Vector2 fingerPos)
		{
			
			if(currRatio < 0.5f && currRatio > 0)
			{    
				_isContinue = false;
				_dataStruct.ReleaseAnimation(_direction,_isContinue);
			}
			else if(currRatio > 0.5f)
			{
				_isContinue = true;
				_dataStruct.ReleaseAnimation(_direction,_isContinue);   

			}
			
			return true;
		}
		
		public bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
		{
			return false;
		}
		
		public bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
		{
			return false;
		}
		
		public bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
		{
			return false;
		}
		
		private RoleScrollStruct _dataStruct;
		private int _direction;//RToL -1    LToR 			
		private bool _isDragOff;
		private float _lastX;//the x of last
		private float _lastY;
		private float _unitSpan = 200 * Globals.Instance.MGUIManager.widthRatio;
		private float currRatio;
		private bool _isContinue;//true stands continue   false stands go back
		private float _leftX = 50f * Globals.Instance.MGUIManager.widthRatio;
		private float _rightX = 750f * Globals.Instance.MGUIManager.widthRatio;
		private float _topY = 400 * Globals.Instance.MGUIManager.widthRatio;
		private float _bottomY = 150 * Globals.Instance.MGUIManager.widthRatio;
	}
	
	public RoleScrollStruct(Transform parent,PackedSprite avatarSprite, PackedSprite roleSpriteBG, PackedSprite roleSpriteBG2, int panelIndex)
	{
		this.avatarSprite = avatarSprite;
		this.roleSpriteBG = roleSpriteBG;
		this.roleSpriteBG2 = roleSpriteBG2;
	
		//init the touch input
		_fingerTouch = new FingerTouch(this);

		_singleScroll = new SingleScrollPanel[]    
		{     
			// new SingleScrollPanel(parent,this,0,"role1100001000"),  
			// new SingleScrollPanel(parent,this,1,"role1100002000"),
			// new SingleScrollPanel(parent,this,2,"role1100003000"),
			// new SingleScrollPanel(parent,this,3,"role1100004000"),
			// new SingleScrollPanel(parent,this,4,"role1100005000")
			
			new SingleScrollPanel(parent,this,0,"BigAvatarMan1"),  
			new SingleScrollPanel(parent,this,1,"BigAvatarMan2"),
			new SingleScrollPanel(parent,this,2,"BigAvatarMan3"),
			// new SingleScrollPanel(parent,this,3,"BigAvatarMan4"),
			// new SingleScrollPanel(parent,this,4,"BigAvatarMan5")
		};
		RoleNameSelect = "BigAvatarMan" + (MaxRoleCnt * 0.5f + 1).ToString();
		// RoleNameSelect = "BigAvatarMan3";
	}

	//slipping scroll
	public void SlippingScroll(int direction,float unitRatio)
	{
		for(int i = 0; i < MaxRoleCnt ; i++)
		{
			_singleScroll[i].SlippingPanle(direction,unitRatio);
		}
	}
	
	//do the input release animation
	public void ReleaseAnimation(int direction,bool isContinue)
	{
		for(int i = 0; i < MaxRoleCnt ; i++)
		{
			_singleScroll[i].ReleaseAnimation(direction,isContinue);
		}
	}
	//change the  node index
	public void ChangeNodeIndex(int count)
	{
		if(count > 0)
		{
			for(int i = 0; i < MaxRoleCnt; i++)
			{
				_singleScroll[i]._nodeIndex +=count;
				if(_singleScroll[i]._nodeIndex ==5)
				{
					_singleScroll[i]._nodeIndex = 0;
					//change panel
					_singleScroll[i].ChangePanel(_singleScroll[i],_panelIndex,_singleScroll[i]._nodeIndex);
				}
			}
		}
		else
		{
			for(int i = 0; i < MaxRoleCnt; i++)
			{
				_singleScroll[i]._nodeIndex +=count;
				if(_singleScroll[i]._nodeIndex == -1)
				{
					_singleScroll[i]._nodeIndex = MaxRoleCnt - 1;
					_singleScroll[i].ChangePanel(_singleScroll[i],_panelIndex,_singleScroll[i]._nodeIndex);
				}
			}
		}
		SetRoleSelect();
	//	Debug.Log("==================");
	}
	
	//改变角色
	public void SetRoleSelect()
	{
		for(int i = 0; i < MaxRoleCnt; i++)
		{
			if(_singleScroll[i]._nodeIndex == MaxRoleCnt / 2)
			{
				// _singleScroll[i].roleBG.PlayAnim("Active");
				
				_singleScroll[i].roleBG2.transform.localScale = Vector3.one;
				RoleNameSelect = _singleScroll[i].animationName;
				Debug.Log(RoleNameSelect);
			}
			else
			{
				_singleScroll[i].roleBG2.transform.localScale = Vector3.zero;
				// _singleScroll[i].roleBG.PlayAnim("Normal");
			}
		}
	}

	
	//change the sex 
	public void ChangeSexRoleImage(int index) // 0 -man 1- woman
	{
		switch(index)
		{
		case 0:
			for(int i = 0; i < MaxRoleCnt; i++)
			{
				// _singleScroll[i].animationName = "role110000" + (i+1) + "000";
				_singleScroll[i].animationName = "BigAvatarMan" + (i+1);
				_singleScroll[i].avatarSprite.PlayAnim(_singleScroll[i].animationName);
			}
			// RoleNameSelect = "role1100003000";
			RoleNameSelect = "BigAvatarMan3";
			break;
		case 1:
			for(int i = 0; i < MaxRoleCnt ; i++)
			{
				_singleScroll[i].animationName = "BigAvatarWoman" + (i+1);
				
				// if(i == 4)
				// {
				// 	_singleScroll[i].animationName = "role1100010000";
				// }
				// else
				// {
				// 	_singleScroll[i].animationName = "role110000"+(i+6)+"000";
				// }
				_singleScroll[i].avatarSprite.PlayAnim(_singleScroll[i].animationName);
			}
			// RoleNameSelect = "role1100008000";
			RoleNameSelect = "BigAvatarWoman" + (MaxRoleCnt * 0.5f + 1).ToString();
			// RoleNameSelect = "BigAvatarWoman3";
			break;
		}			
	}
	//关掉当前滚动结构
	public void RoleScrollClose()
	{
		Globals.Instance.MFingerEvent.RemoveUIObjectEventListener(_fingerTouch);
		foreach(SingleScrollPanel singlePanel in _singleScroll)
		{
			if(singlePanel.roleBG != null)
				GameObject.DestroyImmediate(singlePanel.roleBG.gameObject);
		}
	}
	
	private int _panelIndex;
	private int _maxPanelIndex;//max panel index
	private SingleScrollPanel[] _singleScroll ;
	private FingerTouch _fingerTouch;
	private int countRole = 5;
	private Texture2D[] _textureRoleMan;// = new Texture2D[countRole];
	private Texture2D[] _textureRoleWoman;// = new Texture2D[countRole];
	
	//人物的头像
	public PackedSprite avatarSprite;
	public PackedSprite roleSpriteBG;//角色的背景
	public PackedSprite roleSpriteBG2;//角色的背景2
	public string RoleNameSelect;//角色的名字
}

