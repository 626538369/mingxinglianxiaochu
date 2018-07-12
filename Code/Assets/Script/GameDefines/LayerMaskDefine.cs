using UnityEngine;
using System.Collections;

// Using in Render
// Also use in collider detection
public class LayerMaskDefine
{
	public static int DEFAULT = LayerMask.NameToLayer("Default");
	public static int IGNORE_RAYCAST = LayerMask.NameToLayer("Ignore Raycast");
	
	// public static int OPAQUE = LayerMask.NameToLayer("Opaque");
	public static int TRANSPARENT_FX = LayerMask.NameToLayer("TransparentFX");
	
	public static int WATER = LayerMask.NameToLayer("Water");
	public static int GUI = LayerMask.NameToLayer("GUI");
	
	public static int EFFECT_FX = LayerMask.NameToLayer("EffectFX");
	public static int SMALL_PART = LayerMask.NameToLayer("SmallPart");
	public static int LARGE_OBJECT = LayerMask.NameToLayer("LargeObject");
	public static int NIVOSE_RENDER = LayerMask.NameToLayer("NivoseRender");
	public static int REFLECTION = LayerMask.NameToLayer("Reflection");
	
	public static int CULLING_EVERY = 
		1 << DEFAULT + 
			1 << IGNORE_RAYCAST + 
			1 << TRANSPARENT_FX + 
			1 << WATER + 
			1 << GUI + 
			1 << EFFECT_FX + 
			1 << SMALL_PART + 
			1 << LARGE_OBJECT + 
			1 << NIVOSE_RENDER
			+ 1 << REFLECTION
		;
	
	public static int GetCullMaskEveryThing()
	{
		int cullingMask = 1 << LayerMaskDefine.DEFAULT;
		cullingMask += 1 << LayerMaskDefine.IGNORE_RAYCAST;
		cullingMask += 1 << LayerMaskDefine.TRANSPARENT_FX;
		cullingMask += 1 << LayerMaskDefine.WATER;
		cullingMask += 1 << LayerMaskDefine.GUI;
		cullingMask += 1 << LayerMaskDefine.EFFECT_FX;
		cullingMask += 1 << LayerMaskDefine.SMALL_PART;
		cullingMask += 1 << LayerMaskDefine.LARGE_OBJECT;
		cullingMask += 1 << LayerMaskDefine.NIVOSE_RENDER;
		cullingMask += 1 << LayerMaskDefine.REFLECTION;
		
		return cullingMask;
	}
	
	public static int GetLayerCullMask(int layer)
	{
		int cullingMask = 1 << layer;
		return cullingMask;
	}
	
	public static void SetLayerRecursively(GameObject go, int layerMask)
	{
		go.layer = layerMask;
		for (int i = 0; i < go.transform.GetChildCount(); ++i)
		{
			GameObject child = go.transform.GetChild(i).gameObject;
			SetLayerRecursively(child, layerMask);
		}
	}
}
