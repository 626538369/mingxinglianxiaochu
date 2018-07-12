using UnityEngine;
using System.Collections;

public interface IFingerEventListener
{
	bool IsFingerEventActive();
	void SetFingerEventActive(bool active);
		
	bool OnFingerDownEvent( int fingerIndex, Vector2 fingerPos );
    bool OnFingerUpEvent( int fingerIndex, Vector2 fingerPos, float timeHeldDown );
	
	bool OnFingerMoveBeginEvent( int fingerIndex, Vector2 fingerPos );
	bool OnFingerMoveEvent( int fingerIndex, Vector2 fingerPos );
	bool OnFingerMoveEndEvent( int fingerIndex, Vector2 fingerPos );
	
	bool OnFingerPinchBegin( Vector2 fingerPos1, Vector2 fingerPos2 );
	bool OnFingerPinchMove( Vector2 fingerPos1, Vector2 fingerPos2, float delta );
	bool OnFingerPinchEnd( Vector2 fingerPos1, Vector2 fingerPos2 );
	
	#region  Finger Stationary Event
	bool OnFingerStationaryBeginEvent( int fingerIndex, Vector2 fingerPos );
    bool OnFingerStationaryEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime );
    bool OnFingerStationaryEndEvent( int fingerIndex, Vector2 fingerPos, float elapsedTime );
	#endregion
	
	bool OnFingerDragBeginEvent( int fingerIndex, Vector2 fingerPos, Vector2 startPos );
	bool OnFingerDragMoveEvent( int fingerIndex, Vector2 fingerPos, Vector2 delta );
	bool OnFingerDragEndEvent( int fingerIndex, Vector2 fingerPos );
}
