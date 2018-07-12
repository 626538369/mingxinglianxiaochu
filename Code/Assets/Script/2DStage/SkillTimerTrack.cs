using UnityEngine;
using System.Collections;

public class SkillTimerTrack : MonoBehaviour 
{
	public static void Begin(GameObject target, float runTime, iTween.EventDelegate complete = null)
	{
		SkillTimerTrack track = target.AddComponent<SkillTimerTrack>() as SkillTimerTrack;
		track.SetParameters(runTime, complete);
	}
	
	public void SetParameters(float runTime, iTween.EventDelegate complete = null)
	{
		StartCoroutine(Do(runTime, complete));
	}
	
	IEnumerator Do(float runTime, iTween.EventDelegate complete = null)
	{
		yield return new WaitForSeconds(runTime);
		
		Destroy(this);
		if (null != complete)
			complete();
	}
}
