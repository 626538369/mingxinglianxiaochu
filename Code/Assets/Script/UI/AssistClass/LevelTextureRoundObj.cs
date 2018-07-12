using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTextureRoundObj : MonoBehaviour 
{
	public UITexture textureRound;

	void Awake()
	{
		

	}
	
	void Update()
	{
		textureRound.transform.Rotate(Vector3.forward, 50*Time.deltaTime);
	}
	

}