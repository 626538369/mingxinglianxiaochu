using UnityEngine;
using System.Collections;

public class ShowSelected : MonoBehaviour {
	
	public Shader selectedShader;
	public Color outterColor;

	
	private Color myColor ;
	private Shader myShader;
	private bool Selected = true;
	
	// Use this for initialization
	void Start () {
	//	myColor = renderer.material.color;
	//	myShader = renderer.material.shader;
		selectedShader = Shader.Find("Hidden/RimLightSpce");
		if(!selectedShader)
		{
			enabled = false;
			return;
		}
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
	  void OnMouseEnter() {
          Debug.Log("OnMouseEnter");
        //renderer.material.color = Color.black;
		renderer.material.shader = selectedShader;
		renderer.material.SetColor("_RimColor",outterColor);

    }
	void OnMouseExit(){
        Debug.Log("OnMouseExit");
		renderer.material.color = myColor;
		renderer.material.shader = myShader;
	}
	void OnMouseDown(){
        Debug.Log("OnMouseDown");
		Selected  = !Selected;
		gameObject.layer= Selected ? 16 : 0;
	}
     * */
}
