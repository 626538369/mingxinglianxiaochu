using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    public delegate void FingerDownEventHandler( int fingerIndex, Vector2 fingerPos );
    public delegate void FingerUpEventHandler( int fingerIndex, Vector2 fingerPos, float timeHeldDown );
	public delegate void FingerMoveEventHandler( int fingerIndex, Vector2 fingerPos );
	public delegate void FingerTapEventHandler( int fingerIndex, Vector2 fingerPos, int tapCount );
	public delegate void FingerDragBeginEventHandler( int fingerIndex, Vector2 fingerPos, Vector2 startPos );
	public delegate void FingerDragMoveEventHandler( int fingerIndex, Vector2 fingerPos, Vector2 delta );
	public delegate void FingerDragEndEventHandler( int fingerIndex, Vector2 fingerPos );
}