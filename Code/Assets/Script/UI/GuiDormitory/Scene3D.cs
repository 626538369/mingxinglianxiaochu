using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scene3D : MonoBehaviour 
{
	
	public CharacterCustomizeOne characterCustomizeOne;
//	public CharacterCustomizeOne characterCustomizeDog; 
	public Camera camera;
	public OrbitCamera orbitCameraControl;
	public ParticleSystem particle;
	
	public void Initial()
	{
	}
	
	public CharacterCustomizeOne GetMygirl()
	{
		return  characterCustomizeOne;
	}
}
