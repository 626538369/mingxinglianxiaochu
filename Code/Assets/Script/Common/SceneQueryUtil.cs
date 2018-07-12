using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneQueryUtil 
{
	public class HitInfoComparer : IComparer<RaycastHit>
	{
		public int Compare (RaycastHit x, RaycastHit y)
		{
			if (x.distance < y.distance)
			{
				return -1;
			}
			
			if (x.distance > y.distance)
			{
				return -1;
			}
			
			return 0;
		}
	}
	
	// Return the GameObject at the given screen position, or null if no valid object was found
    public static GameObject PickObject( Vector2 screenPos )
    {
        Ray ray = Globals.Instance.MSceneManager.mMainCamera.ScreenPointToRay( screenPos );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit ) )
            return hit.collider.gameObject;

        return null;
    }
	
	public static GameObject PickObject(Vector2 screenPos, string tagMask)
	{
		Ray ray = Globals.Instance.MSceneManager.mMainCamera.ScreenPointToRay( screenPos );
        RaycastHit hit;
		
		float distance = float.PositiveInfinity;
		RaycastHit[] hitInfos = Physics.RaycastAll(ray, distance);
		
		_mHitInfoList.Clear();
		for (int i = 0; i < hitInfos.Length; ++i)
		{
			if (hitInfos[i].collider.gameObject.tag.Equals(tagMask))
			{
				_mHitInfoList.Add(hitInfos[i]);
			}
		}
		
		_mHitInfoList.Sort(new HitInfoComparer());
		
		if (_mHitInfoList.Count != 0)
			return _mHitInfoList[0].collider.gameObject;
		
		return null;
	}
	
	public static GameObject PickObject(Camera cam, Vector2 screenPos, string tagMask)
	{
		Ray ray = cam.ScreenPointToRay( screenPos );
        RaycastHit hit;
		
		float distance = float.PositiveInfinity;
		RaycastHit[] hitInfos = Physics.RaycastAll(ray, distance);
		
		_mHitInfoList.Clear();
		for (int i = 0; i < hitInfos.Length; ++i)
		{
			if (hitInfos[i].collider.gameObject.tag.Equals(tagMask))
			{
				_mHitInfoList.Add(hitInfos[i]);
			}
		}
		
		_mHitInfoList.Sort(new HitInfoComparer());
		
		if (_mHitInfoList.Count != 0)
			return _mHitInfoList[0].collider.gameObject;
		
		return null;
	}
	
	#region TouchUtils
	// Convert from screen-space coordinates to world-space coordinates on the Z = 0 plane
    public static Vector3 GetWorldPos( Vector2 screenPos )
    {
       // Ray ray = Globals.Instance.MSceneManager.mMainCamera.ScreenPointToRay( screenPos );

        // we solve for intersection with z = 0 plane
       // float t = -ray.origin.z / ray.direction.z;

       // return ray.GetPoint( t );
		
   		 // convert from screen-space coordinates to world-space coordinates in the XY plane
		Camera mainCamera = Globals.Instance.MSceneManager.mMainCamera;
        // return mainCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( transform.position.z - mainCamera.transform.position.z ) ) ); 
        return mainCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs(mainCamera.transform.position.z ) ) ); 
    }
	
	
	#endregion
	
	private static List<RaycastHit> _mHitInfoList = new List<RaycastHit>();
}
