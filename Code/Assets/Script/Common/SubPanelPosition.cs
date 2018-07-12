using UnityEngine;  
using System.Collections;  
  
public class SubPanelPosition:MonoBehaviour {  
    public ScreenDirection screenDirection;  
    public enum ScreenDirection   
    {  
        horizontal,  
        vertical  
    }  
    private Transform parent;  
    private Transform child;  
    private float ScaleSize;  
    private float rateX;  
    private float rateY;  
    UIPanel PanelScript;  
    void Start()  
    {  
        parent = transform.parent;  
        child = transform.GetChild(0);  
        PanelScript = transform.GetComponent<UIPanel>();  
		
		Invoke("SetPanel",0.5f); 
    }  
    void SetPanel()  
    {         
        transform.parent = null;  
        child.parent = null;  
      
          
		if(screenDirection == ScreenDirection.vertical)  
		{  
			ScaleSize = transform.localScale.y;  
			rateX = ScaleSize/transform.localScale.x;  
			rateY = 1;  
			
			transform.localScale = new Vector4(ScaleSize,ScaleSize,ScaleSize,ScaleSize);      
			transform.parent = parent;  
			child.parent = transform;  
			float widthRatio = Globals.Instance.MGUIManager.widthRatio;
			widthRatio = 1/widthRatio;
			PanelScript.clipRange = new Vector4(PanelScript.clipRange.x, PanelScript.clipRange.y, PanelScript.clipRange.z*(1/rateX),PanelScript.clipRange.w*rateY);  
			
		}  
		else if(screenDirection == ScreenDirection.horizontal)  
		{  
			ScaleSize = transform.localScale.x;  
			rateX = 1;  
			rateY = ScaleSize/transform.localScale.y;  
			
			transform.localScale = new Vector4(ScaleSize,ScaleSize,ScaleSize,ScaleSize);      
			transform.parent = parent;  
			child.parent = transform;  
			PanelScript.clipRange = new Vector4(PanelScript.clipRange.x, PanelScript.clipRange.y, PanelScript.clipRange.z*rateX,PanelScript.clipRange.w*(1/rateY));  
		}  
          
  
    }  
      
}  