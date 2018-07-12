using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;

public class Scene
{
	public Scene(){
		_sceneMainGameObject = UnityEngine.GameObject.Find("SceneGameObject");
	}
	
	public void LoadTestPort(){
		string assetFileName = "Config/SceneAssetsFileConfig/TestPortScene";
		TestSceneMainGameObject();
		Load(assetFileName);
	}
	
	public void Load (string assetFile)
	{
	}
	
	private void LoadSceneLight(){
		GameObject light = new GameObject("Light");
		light.AddComponent<Light>();
		light.GetComponent<Light>().type = LightType.Directional;
		light.transform.position = new Vector3(0F,83.5F,0F);
		light.transform.eulerAngles = new Vector3(45F,0F,0F);
		
	}
	
	/*
	public void LoadGangKou(){
		TestSceneMainGameObject();
		
		GameObject go = UnityEngine.MonoBehaviour.Instantiate (UnityEngine.Resources.Load ("Mesh/3D/Narvik")) as GameObject;
		go.transform.position = new Vector3(0F,0F,0F);
		go.transform.eulerAngles = new Vector3(0F,180F,0F);
		go.transform.parent = _sceneMainGameObject.transform;
	}*/
	
	public void LoadBattle(){
		string assetFileName = "Config/SceneAssetsFileConfig/BattleStart";
		TestSceneMainGameObject();
		Load(assetFileName);
	}
	
	protected void TestSceneMainGameObject(){
		Transform[] childObjects = _sceneMainGameObject.GetComponentsInChildren<Transform>();

		foreach (Transform child in childObjects) {
			if(!child.name.Equals("SceneGameObject")){
				UnityEngine.MonoBehaviour.Destroy(child.gameObject);
			}
			
		}
	
	}
	
	public void Release ()
	{
		
	}
	
	public Vector3 GetBattlePosition ()
	{
		return new Vector3 (0, 0);
	}
	
	/*
	public float LoadProres
	{
		
	}
	
	protected OnDownlad()
	{
		
	}
	*/
	//protected  _progress;
	
	protected GameObject _sceneMainGameObject;
}
