using UnityEngine;
using System.Collections;

public class SkillLineTrack : MonoBehaviour 
{
	protected System.Object userData;
	
	protected Vector3 startPosition;
	protected Vector3 endPosition;
	protected Vector3 currPosition;
	
	private float needRunTime;
	private float haveRunTime;
	
	iTween.EventDelegate completeDel = null;
		
	public static void Begin(GameObject target, Vector3 startPosition, Vector3 endPosition, 
		float speed, float targetSpeed, iTween.EventDelegate complete = null)
	{
		SkillLineTrack track = target.AddComponent<SkillLineTrack>() as SkillLineTrack;
		track.SetParameters(startPosition, endPosition, speed, targetSpeed, complete);
	}
	
	public void SetParameters(Vector3 startPos, Vector3 endPos, 
		float speed, float targetSpeed, iTween.EventDelegate complete = null)
	{
		startPosition	= startPos;
		endPosition		= endPos;
		currPosition	= startPosition;
		
		haveRunTime		= (startPosition - endPosition).magnitude / (speed + targetSpeed);
		needRunTime		= 0;
		
		completeDel = complete;
	}
	
	public void Update()
	{
		if(haveRunTime > 0){
			
			bool tEnd = false;
			
			needRunTime += Time.deltaTime;
			if(needRunTime > haveRunTime){
				needRunTime	= haveRunTime;
				tEnd			= true;
			}
			
			float tDelta = needRunTime / haveRunTime;
			currPosition.x = Mathf.Lerp(startPosition.x, endPosition.x, tDelta);
			currPosition.y = Mathf.Lerp(startPosition.y, endPosition.y, tDelta);
			currPosition.z = Mathf.Lerp(startPosition.z, endPosition.z, tDelta);
			
			transform.position = currPosition;
			
			if(tEnd){
				haveRunTime = 0;
				
				Destroy(this);
				if (null != completeDel) completeDel();
			}
		}
	}
}
