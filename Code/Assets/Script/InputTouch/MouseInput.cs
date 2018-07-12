using UnityEngine;
using System.Collections;

public class MouseInput : BaseInput 
{
	// Number of mouse buttons to track
    int maxMouseButtons = 3;
    public override int MaxFingers
    {
        get { return maxMouseButtons; }
    }
	
	protected override BaseInput.FingerPhase GetPhase(BaseInput.Finger finger)
    {
        int button = finger.Index;

        // mouse button down?
        if (Input.GetMouseButton(button))
        {
            // did we just press it?
            if( Input.GetMouseButtonDown( button ) )
			{
				Debug.Log("[MouseInput]: MouseDown");
                return FingerPhase.Began;
			}

            // find out if the mouse has moved since last update
            Vector3 delta = GetPosition(finger) - finger.Position;

            if (delta.sqrMagnitude < 1.0f)
                return FingerPhase.Stationary;

			Debug.Log("[MouseInput]: MouseMove");
            return FingerPhase.Moved;
        }

        // did we just release the button?
        if (Input.GetMouseButtonUp(button))
		{
			Debug.Log("[MouseInput]: MouseUp");
            return FingerPhase.Ended;
		}

        return FingerPhase.None;
    }

    protected override Vector2 GetPosition(Finger finger)
    {
        return Input.mousePosition;
    }
}
