using UnityEngine;
using System.Collections;

public class GraphicsTools 
{
	public static void DrawDottedCube(Vector3 center, Vector3 size)
	{
		
	}
	
	public static void DrawDottedSphere(Vector3 center, float radius)
	{
		
	}
	
	public static GameObject DrawDottedRectangle(Vector3 center, float width, float height, float space, float startW, float endW)
	{
		// int length = 360 / space + 1;
		// Vector3[] list = new Vector3[length + 1];
		// for (int i = 0; i < length + 1; i++)
		// {
		// 	list[i] = center;
		// 	list[i].x += Mathf.Cos( Mathf.Deg2Rad * i * space);
		// 	list[i].y += Mathf.Sin( Mathf.Deg2Rad * i * space);
		// }
		// 
		// return DrawDottedLine(list, 0.1f, 0.1f, Color.white, Color.white);
		
		return null;
	}
	
	public static GameObject DrawDottedCircle(Vector3 center, float radius, int degSpace, float startW, float endW)
	{
		int length = 360 / degSpace + 1;
		Vector3[] list = new Vector3[length];
		for (int i = 0; i < length; i++)
		{
			list[i] = center;
			list[i].x += Mathf.Cos( Mathf.Deg2Rad * i * degSpace) * radius;
			list[i].z += Mathf.Sin( Mathf.Deg2Rad * i * degSpace) * radius;
		}

		return DrawDottedLine(list, startW, endW, Color.white, Color.white);
	}
	
	public static GameObject DrawDottedLine(Vector3[] points, float startW, float endW, Color startCol, Color endCol)
	{
		// if (null == graphicsRoot)
		// {
		// 	graphicsRoot = new GameObject();
		// 	graphicsRoot.name = "GraphicsRoot";
		// }
		
		GameObject go = ManualCreateObject("Common/DottedLine", "LineInst", true);
		LineRenderer render = go.GetComponent<LineRenderer>() as LineRenderer;
		render.useWorldSpace = false;
		
		// float distance = Vector3.Distance(go.transform.position, Globals.Instance.MSceneManager.mMainCamera.transform.position);
		// float width = distance * 0.1f * initWidth;
		// render.SetWidth(width, width);
		
		render.SetWidth(startW, endW);
		render.SetColors(startCol, endCol);
		render.SetVertexCount(points.Length);
		// render.sharedMaterial = ;
		Material mat = render.sharedMaterial;
		mat.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
		mat.SetTextureScale("_MainTex", new Vector2(points.Length, 1.0f));
		// mat.mainTextureScale;
		
		for (int i = 0; i < points.Length; ++i)
		{
			render.SetPosition(i, points[i]);
		}
		
		return go;
	}
	
	public static GameObject ManualCreateObject(string path, string name, bool dontDestroyOnLoad)
	{
		Object obj = Resources.Load(path);
		GameObject go = GameObject.Instantiate(obj) as GameObject;
		go.name = name;
		
		if (dontDestroyOnLoad)
			GameObject.DontDestroyOnLoad(go);
		
		return go;
	}
	
	public static void ManualDestroyObject(GameObject go, bool immediate)
	{
		if (immediate)
			GameObject.DestroyImmediate(go);
		else
			GameObject.Destroy(go);
	}
	
	private static GameObject graphicsRoot = null;
	private static float initWidth = 0.1f;
}
