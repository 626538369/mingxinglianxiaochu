using UnityEngine;
using System.Collections;

// Must hang to a renderer object
public class VisibleTrigger : MonoBehaviour 
{
	public GameObject ControlTarget
	{
		get { return controlTarget; }
		set { controlTarget = value; }
	}
	
	void Awake()
	{
		controlTarget = null;
	}
	
	void OnBecameVisible()
	{
		if (null != controlTarget)
			controlTarget.SetActiveRecursively(false);
	}
	
	void OnBecameInvisible()
	{
		if (null != controlTarget)
			controlTarget.SetActiveRecursively(true);
	}
	
	private GameObject controlTarget;
}
