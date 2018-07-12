using UnityEngine;
using System.Collections;

public class EffectsOfListening : MonoBehaviour {

	
	public void SetEffect(string effectName)
	{
		ParticleSystem effectObj = GameObject.Find(effectName).gameObject.GetComponent<ParticleSystem>();
		
		effectObj.Play();
	}
	
	public void SetSound(string soundName)
	{
		
		
		
	}
	
}
