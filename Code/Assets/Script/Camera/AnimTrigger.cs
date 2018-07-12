using UnityEngine;
using System.Collections;

public class AnimTrigger : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Animation anim = GetComponentInChildren<Animation>() as Animation;
		if (anim)
		{	
			anim.CrossFade("die");
		}
	}
	
	void OnTriggerExit(Collider other)
	{
	}
}
