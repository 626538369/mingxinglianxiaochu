using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FingerEvent : MonoBehaviour 
{
	// Add 3DObject listener which use this FingerEvent
	public List<IFingerEventListener> M3DEventListenerList = new List<IFingerEventListener>();
	// Add UIObject listener which use this FingerEvent
	public List<IFingerEventListener> MUIObjectEventListenerList = new List<IFingerEventListener>();
	
	void Awake()
	{
	}
	
	void OnEnable()
    {
        // subscribe to the finger down event   
		FingerGestures.OnFingerDown += this.OnFingerDownEvent;
        FingerGestures.OnFingerUp += this.OnFingerUpEvent;
		FingerGestures.OnFingerMoveBegin += this.OnFingerMoveBeginEvent;
        FingerGestures.OnFingerMove += this.OnFingerMoveEvent;
        FingerGestures.OnFingerMoveEnd += this.OnFingerMoveEndEvent;
		
        FingerGestures.OnFingerDragBegin += this.OnFingerDragBeginEvent;
        FingerGestures.OnFingerDragMove += this.OnFingerDragMoveEvent;
        FingerGestures.OnFingerDragEnd += this.OnFingerDragEndEvent;
		
		FingerGestures.OnFingerStationaryBegin += this.OnFingerStationaryBeginEvent;
        FingerGestures.OnFingerStationary += this.OnFingerStationaryEvent;
        FingerGestures.OnFingerStationaryEnd += this.OnFingerStationaryEndEvent;
		
		FingerGestures.OnFingerLongPress += this.OnFingerLongPressEvent;
        FingerGestures.OnFingerTap += this.OnFingerTapEvent;
        FingerGestures.OnFingerSwipe += this.OnFingerSwipeEvent;
		
		FingerGestures.OnRotationBegin += this.OnFingerRotationBegin;
        FingerGestures.OnRotationMove += this.OnFingerRotationMove;
        FingerGestures.OnRotationEnd += this.OnFingerRotationEnd;

        FingerGestures.OnPinchBegin += this.OnFingerPinchBegin;
        FingerGestures.OnPinchMove += this.OnFingerPinchMove;
        FingerGestures.OnPinchEnd += this.OnFingerPinchEnd;
    }

    void OnDisable()
    {
        // unsubscribe from the finger down event
		FingerGestures.OnFingerDown -= this.OnFingerDownEvent;
        FingerGestures.OnFingerUp -= this.OnFingerUpEvent;
		
		FingerGestures.OnFingerMoveBegin -= this.OnFingerMoveBeginEvent;
        FingerGestures.OnFingerMove -= this.OnFingerMoveEvent;
        FingerGestures.OnFingerMoveEnd -= this.OnFingerMoveEndEvent;
		
        FingerGestures.OnFingerDragBegin -= this.OnFingerDragBeginEvent;
        FingerGestures.OnFingerDragMove -= this.OnFingerDragMoveEvent;
        FingerGestures.OnFingerDragEnd -= this.OnFingerDragEndEvent;
		
		FingerGestures.OnFingerStationaryBegin -= this.OnFingerStationaryBeginEvent;
        FingerGestures.OnFingerStationary -= this.OnFingerStationaryEvent;
        FingerGestures.OnFingerStationaryEnd -= this.OnFingerStationaryEndEvent;
		
		FingerGestures.OnFingerLongPress -= this.OnFingerLongPressEvent;
        FingerGestures.OnFingerTap -= this.OnFingerTapEvent;
        FingerGestures.OnFingerSwipe -= this.OnFingerSwipeEvent;
		
		FingerGestures.OnRotationBegin -= this.OnFingerRotationBegin;
        FingerGestures.OnRotationMove -= this.OnFingerRotationMove;
        FingerGestures.OnRotationEnd -= this.OnFingerRotationEnd;

        FingerGestures.OnPinchBegin -= this.OnFingerPinchBegin;
        FingerGestures.OnPinchMove -= this.OnFingerPinchMove;
        FingerGestures.OnPinchEnd -= this.OnFingerPinchEnd;
    }
	
	public void Add3DEventListener(IFingerEventListener listener)
	{
		if( M3DEventListenerList.Contains(listener) ) 
			return;
		
		M3DEventListenerList.Add(listener);
	}
	
	public void Remove3DEventListener(IFingerEventListener listener)
	{
		M3DEventListenerList.Remove(listener);
	}
	
	public void AddUIObjectEventListener(IFingerEventListener listener)
	{
		if( MUIObjectEventListenerList.Contains(listener) ) 
			return;
		
		MUIObjectEventListenerList.Add(listener);
	}
	
	public void RemoveUIObjectEventListener(IFingerEventListener listener)
	{
		MUIObjectEventListenerList.Remove(listener);
	}
	
	#region Finger DownUp Event
	// handle the fingerdown event
    void OnFingerDownEvent( int fingerIndex, Vector2 fingerPos )
    {
		
		Globals.Instance.MGUIManager.guiPointerInfoListener(fingerPos);		
		
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerDownEvent(fingerIndex, fingerPos))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener != null && listener.OnFingerDownEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
    }
	
	// handle the fingerup event
    void OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
    {	
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerUpEvent(fingerIndex, fingerPos, timeHeldDown))
				{
					break;
				}
			}
			
			return;
		}
		
		// this shows how to access a finger object using its index
        // The finger object contains useful information not available through the event arguments that you might want to use
        FingerGestures.Finger finger = FingerGestures.GetFinger( fingerIndex );
		//Debug.Log( "Finger " + fingerIndex + " was held down for " + timeHeldDown.ToString( "N2" ) + " seconds" );
        // Debug.Log( "Finger was lifted up at " + finger.Position + " and moved " + finger.DistanceFromStart.ToString( "N0" ) + " pixels from its initial position at " + finger.StartPosition );
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener != null && listener.OnFingerUpEvent(fingerIndex, fingerPos,timeHeldDown))
			{
				//break;
			}
		}
		
    }
	#endregion
	
	
	#region Finger Move Event
	public void OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerMoveBeginEvent(fingerIndex, fingerPos))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerMoveBeginEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
	}
	
	public void OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerMoveEvent(fingerIndex, fingerPos))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerMoveEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
	}
	
	public void OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerMoveEndEvent(fingerIndex, fingerPos))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerMoveEndEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
	}
	#endregion
	
	
	#region Finger Drag Event
	public void OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
//		if (Globals.Instance.MGUIManager.MIsHitUIObject)
//		{
//			 // Dispose the event in UIObject rect
//			 foreach (IFingerEventListener listener in MUIObjectEventListenerList)
//			 {
//			 	if (!listener.IsFingerEventActive())
//			 		continue;
//			 	
//			 	if (listener != null && listener.OnFingerDragBeginEvent(fingerIndex, fingerPos, startPos))
//			 	{
//			 		break;
//			 	}
//			 }
//			
//			return;
//		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener != null && listener.OnFingerDragBeginEvent(fingerIndex, fingerPos, startPos))
			{
				break;
			}
		}
		
	}

    /// <summary>
    /// Delegate for the OnFingernDragMove event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="delta">How much the finger has moved since the last update. This is the difference between the previous finger position and the new one.</param>
    public void OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// // Dispose the event in UIObject rect
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener != null && listener.OnFingerDragMoveEvent(fingerIndex, fingerPos, delta))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener != null && listener.OnFingerDragMoveEvent(fingerIndex, fingerPos, delta))
			{
				break;
			}
		}
	}

    /// <summary>
    /// Delegate for the OnFingernDragEnd event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public void OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// // Dispose the event in UIObject rect
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener != null && listener.OnFingerDragEndEvent(fingerIndex, fingerPos))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener != null && listener.OnFingerDragEndEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
	}
	#endregion
	
	
	#region Finger Stationary Event
	/// <summary>
    /// Delegate for the OnFingerStationaryBegin event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public void OnFingerStationaryBeginEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerStationaryBeginEvent(fingerIndex, fingerPos))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerStationaryBeginEvent(fingerIndex, fingerPos))
			{
				break;
			}
		}
	}

    /// <summary>
    /// Delegate for the OnFingerStationary event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="elapsedTime">How much time has elapsed, in seconds, since the last OnFingerStationaryBegin fired on this finger</param>
    public void OnFingerStationaryEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// Dispose the event in UIObject rect
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener != null && listener.OnFingerStationaryEvent( fingerIndex, fingerPos, elapsedTime ))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerStationaryEvent(fingerIndex, fingerPos, elapsedTime ))
			{
				break;
			}
		}
	}

    /// <summary>
    /// Delegate for the OnFingerStationaryEnd event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="elapsedTime">How much time has elapsed, in seconds, since the last OnFingerStationaryBegin fired on this finger</param>
    public void OnFingerStationaryEndEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener.OnFingerStationaryEndEvent(fingerIndex, fingerPos, elapsedTime ))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerStationaryEndEvent(fingerIndex, fingerPos, elapsedTime ))
			{
				break;
			}
		}
	}
	#endregion
	
	
	#region Finger Others Event
	/// <summary>
    /// Delegate for the OnFingernLongPress event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    public void OnFingerLongPressEvent( int fingerIndex, Vector2 fingerPos )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerLongPressEvent(fingerIndex, fingerPos ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
	}
	
	/// <summary>
    /// Delegate for the OnFingernTap event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="fingerPos">Current position of the finger on the screen</param>
    /// <param name="tapCount">How many times the user has consecutively tapped his finger at this location</param>
    public void OnFingerTapEvent( int fingerIndex, Vector2 fingerPos, int tapCount )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerTapEvent(fingerIndex, fingerPos, tapCount ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
	}
	
	/// <summary>
    /// Delegate for the OnFingernSwipe event
    /// </summary>
    /// <param name="fingerIndex">0-based index uniquely indentifying a specific finger</param>
    /// <param name="startPos">Initial position of the finger</param>
    /// <param name="direction">Direction of the swipe gesture</param>
    /// <param name="velocity">How quickly the finger has moved (in screen pixels per second)</param>
    public void OnFingerSwipeEvent( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
	{
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerTapEvent(fingerIndex, startPos, direction, velocity ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
	}
	#endregion
	
	public void OnFingerRotationBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerTapEvent( fingerPos1, fingerPos2 ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
		
		string text = "Rotation gesture started.";
    }
	
	public void OnFingerRotationMove( Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerRotationMove( fingerPos1, fingerPos2, rotationAngleDelta ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
		
        string text = "Rotation updated by " + rotationAngleDelta + " degrees";
        // apply a rotation around the Z axis by rotationAngleDelta degrees on our target object
        // target.Rotate( 0, 0, rotationAngleDelta );
    }
	
	public void OnFingerRotationEnd( Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			// foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			// {
			// 	if (!listener.IsFingerEventActive())
			// 		continue;
			// 	
			// 	if (listener.OnFingerRotationEnd( fingerPos1, fingerPos2, totalRotationAngle ))
			// 	{
			// 		break;
			// 	}
			// }
			
			return;
		}
		
		string text = "Rotation gesture ended. Total rotation: " + totalRotationAngle;
    }
	
	public void OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{		
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener.OnFingerPinchBegin( fingerPos1, fingerPos2 ))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerPinchBegin(fingerPos1, fingerPos2))
			{
				break;
			}
		}
    }
	
	public void OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener.OnFingerPinchMove( fingerPos1, fingerPos2, delta ))
				{
					break;
				}
			}
			
			return;
		}
		
        // change the scale of the target based on the pinch delta value
        // target.transform.localScale += delta * pinchScaleFactor * Vector3.one;
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerPinchMove(fingerPos1, fingerPos2, delta))
			{
				break;
			}
		}
    }
	
	public void OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 )
    {
		// If hit UIObject, return donn't need dispose 3DObject
		if (Globals.Instance.MGUIManager.MIsHitUIObject)
		{
			foreach (IFingerEventListener listener in MUIObjectEventListenerList)
			{
				if (!listener.IsFingerEventActive())
					continue;
				
				if (listener.OnFingerPinchEnd( fingerPos1, fingerPos2 ))
				{
					break;
				}
			}
			
			return;
		}
		
		foreach (IFingerEventListener listener in M3DEventListenerList)
		{
			if (!listener.IsFingerEventActive())
				continue;
			
			if (listener.OnFingerPinchEnd(fingerPos1, fingerPos2))
			{
				break;
			}
		}
    }
}
