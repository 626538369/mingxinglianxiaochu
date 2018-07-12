using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseInput : MonoBehaviour 
{
	/// <summary>
	/// Finger Phase
	/// </summary>
	public enum FingerPhase
	{
	    None,
	
	    /// <summary>
	    /// The finger just touched the screen
	    /// </summary>
	    Began,
	
	    /// <summary>
	    /// The finger just moved
	    /// </summary>
	    Moved,
	
	    /// <summary>
	    /// The finger is stationary
	    /// </summary>
	    Stationary,
	
	    /// <summary>
	    /// The finger was lifted off the screen
	    /// </summary>
	    Ended,
	}
	
	/// <summary>
    /// Finger
    /// 
    /// This provides an abstraction for a finger that can touch and move around the screen.
    /// As opposed to Unity's Touch object, a Finger exists independently of whether it is 
    /// currently touching the screen or not
    /// </summary>
    public class Finger
    {
        public int Index
        {
            get { return index; }
        }

        /// <summary>
        /// Current phase
        /// </summary>
        public FingerPhase Phase
        {
            get { return phase; }
        }

        /// <summary>
        /// Return true if the finger is currently down
        /// </summary>
        public bool IsDown
        {
            get { return down; }
        }

        /// <summary>
        /// Return true if the finger was down during the previous update/frame
        /// </summary>
        public bool WasDown
        {
            get { return wasDown; }
        }

        /// <summary>
        /// Get the time of first screen contact
        /// </summary>
        public float StarTime
        {
            get { return startTime; }
        }

        /// <summary>
        /// Get the position of first screen contact
        /// </summary>
        public Vector2 StartPosition
        {
            get { return startPos; }
        }

        /// <summary>
        /// Get the current position
        /// </summary>
        public Vector2 Position
        {
            get { return pos; }
        }

        /// <summary>
        /// Get the position during the previous frame
        /// </summary>
        public Vector2 PreviousPosition
        {
            get { return prevPos; }
        }

        /// <summary>
        /// Get the difference between previous and current position
        /// </summary>
        public Vector2 DeltaPosition
        {
            get { return deltaPos; }
        }

        #region Internal

        int index = 0;
        bool wasDown = false;
        bool down = false;
        float startTime = 0;
        FingerPhase phase = FingerPhase.None;
        Vector2 startPos = Vector2.zero;
        Vector2 pos = Vector2.zero;
        Vector2 prevPos = Vector2.zero;
        Vector2 deltaPos = Vector2.zero;

        public Finger( int index )
        {
            this.index = index;
        }

        public override string ToString()
        {
            return "Finger" + index;
        }

        internal void Update( FingerPhase newPhase, Vector2 newPos )
        {
            // validate phase transitions
            if( phase != newPhase )
            {
                // In low framerate situations, it is possible to miss some input updates and thus 
                // skip the "Ended" phase
                if( newPhase == FingerPhase.None && phase != FingerPhase.Ended )
                {
                    Debug.LogWarning( "Correcting bad FingerPhase transition (FingerPhase.Ended skipped)" );
                    Update( FingerPhase.Ended, PreviousPosition );
                    return;
                }

                // cannot get a Moved or Stationary phase without being down first
                if( !down && ( newPhase == FingerPhase.Moved || newPhase == FingerPhase.Stationary ) )
                {
                    Debug.LogWarning( "Correcting bad FingerPhase transition (FingerPhase.Began skipped)" );
                    Update( FingerPhase.Began, newPos );
                    return;
                }

                if( ( down && newPhase == FingerPhase.Began ) || ( !down && newPhase == FingerPhase.Ended ) )
                {
                    Debug.LogWarning( "Invalid state FingerPhase transition from " + phase + " to " + newPhase + " - Skipping." );
                    return;
                }
            }
            else // same phase as before
            {
                if( newPhase == FingerPhase.Began || newPhase == FingerPhase.Ended )
                {
                    Debug.LogWarning( "Duplicated FingerPhase." + newPhase.ToString() + " - skipping." );
                    return;
                }
            }

            if( newPhase != FingerPhase.None )
            {
                if( newPhase == FingerPhase.Ended )
                {
                    // release
                    down = false;
                }
                else
                {
                    if( newPhase == FingerPhase.Began )
                    {
                        // activate
                        down = true;
                        startPos = newPos;
                        prevPos = newPos;
                        startTime = Time.time;
                    }

                    prevPos = pos;
                    pos = newPos;
                    deltaPos = pos - prevPos;
                }
            }

            phase = newPhase;
        }

        /// <summary>
        /// PostUpdate
        /// We use PostUpdate() to raise the OnDown/OnUp events after all the fingers have been properly updated
        /// </summary>
        internal void PostUpdate()
        {
            // if( wasDown != down )
            // {
            //     if( down )
            //     {
            //         if( OnDown != null )
            //             OnDown( this );
            //     }
            //     else
            //     {
            //         if( OnUp != null )
            //             OnUp( this );
            //     }
            // }

            wasDown = down;
        }

        #endregion
    }
	
	public abstract int MaxFingers
	{
		get;
	}
	
	static Finger[] fingers = null;
	List<Finger> touches = new List<Finger>(); // Current down touches
	void InitFingers(int count)
	{
	    // pre-allocate a touch data entry for each finger
	    if(fingers == null)
	    {
	        fingers = new Finger[count];
	
	        for( int i = 0; i < count; ++i )
	            fingers[i] = new Finger(i);
	    }
	
	    // InitDefaultComponents();
	}
	
	void UpdateFingers()
	{
	    touches.Clear();
	
	    // update all fingers
	    foreach (Finger finger in fingers)
	    {
	        Vector2 pos = Vector2.zero;
	        FingerPhase phase = GetPhase(finger);
	
	        if (phase != FingerPhase.None)
	            pos = GetPosition(finger);
	
	        finger.Update(phase, pos);
	
	        if (finger.IsDown)
	            touches.Add(finger);
	    }
	
	    // post-update
	    foreach (Finger finger in fingers)
			finger.PostUpdate();
	}
	
    public static Finger GetFinger(int index)
    {
        return fingers[index];
    }
	
	/// <summary>
    /// Return the new phase of the finger for this frame
    /// </summary>
    protected abstract FingerPhase GetPhase(Finger finger);

    /// <summary>
    /// Return the new position of the finger on the screen for this frame
    /// </summary>
    protected abstract Vector2 GetPosition(Finger finger);
	
	protected virtual void Awake() 
	{
		InitFingers(MaxFingers);
	}
	
	// Use this for initialization
	protected virtual void Start() 
	{
		if(fingers == null)
	    {
			InitFingers(MaxFingers);
		}
	}
	
	// Update is called once per frame
	protected virtual void Update() 
	{
		UpdateFingers();
	}
}
