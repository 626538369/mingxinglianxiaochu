using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;

public class ShowFPS : MonoBehaviour 
{
	private float m_timer;
	private int m_frameCounter;
	private float m_updateInterval;
	
	private int frames;
	public float updateInterval = 0.5f;
	private float lastTime = 0.0f;
	private GUIText gui;
	
	private bool IsWriteInfoFile = false;
	private bool IsAutoCreatePrefab = false;
	
	private GUISkin skin;
	
	// Use this for initialization
	void Start () 
	{
		m_timer = 0;
		
		lastTime = Time.realtimeSinceStartup;
   	 	frames = 0;
		gui = null;
	}
	
	void OnDisable()
	{
		if (gui)
			DestroyImmediate (gui.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Process escape key.
#if UNITY_ANDROID || UNITY_IPHONE
		if (Input.GetKey("escape"))
		{
			// CloseFile();
			Application.Quit();
		}
#endif
		
#if !UNITY_FLASH
		frames++;
		float nowTime = Time.realtimeSinceStartup;
		if (nowTime - lastTime > updateInterval)
		{
			if (!gui)
			{
				GameObject go = new GameObject("FPS Display", typeof(GUIText));
				go.hideFlags = HideFlags.HideAndDontSave;
				go.transform.position = Vector3.zero;
				gui = go.GetComponent<GUIText>();
				// gui.pixelOffset = new Vector2(5,55);
				gui.pixelOffset = new Vector2(10, Screen.height - 30);
				
				Font font = gui.font;
				font.material.color = Color.yellow;
				gui.fontStyle = FontStyle.BoldAndItalic;
			}
			
			float fps = frames / (nowTime - lastTime);
			float ms = 1000.0f / Mathf.Max (fps, 0.00001f);
			gui.text = ms.ToString("F2") + "ms    " + fps.ToString("F2") + "fps";
			
	        frames = 0;
			lastTime = nowTime;
		}
#endif
	}
	
	public void DisplayFPS()
	{
		m_timer += Time.deltaTime;
		m_frameCounter++;
		
		if(m_timer > m_updateInterval)
		{
			FPS = (m_frameCounter / m_timer);
			m_frameCounter = 0;
			m_timer = 0;
		}
		
		string text = "WorkPath:" + Application.persistentDataPath + "\n" +
					"GameRunningTime:" + Time.time + "\n" + 
		 			"Fps:" + FPS;
		
		text += "\n";
		text += "Time.deltaTime" + Time.deltaTime + "\n";
		text += "Time.frameCount" + Time.frameCount + "\n";
		text += "Time.renderedFrameCount" + Time.renderedFrameCount + "\n";
		text += "Time.captureFramerate" + Time.captureFramerate;
		
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 14;
		style.fontStyle = FontStyle.Bold;
		
		GUI.skin.label = style;
		GUI.Label(new Rect(0, 100, 500, 200), text);
	}
	
	public void OpenFile()
	{
		// Binary file
		string path = Application.persistentDataPath;
		// path = Application.dataPath;
		string fileName = "FPSRecord.txt";
		FilePath = path + "/" + fileName;
		
		PFileStream = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
		Writer = new StreamWriter(PFileStream);
		
		// Write file header
		string line = Time.time.ToString();
		string fps = (m_frameCounter / m_timer).ToString();
		line = "RunningTime".ToString() + "," + "Fps" + "," 
			+ "GameObjectCount" + "," + "DrawCallCount" + "," + "TriangleCount".ToString() + "," + "VertexCount".ToString();
		
		Writer.WriteLine(line);
		Writer.Flush();
	}
	
	public void WriteFile()
	{
		// StreamWriter writer = new StreamWriter(PFileStream);
		string line = Time.time.ToString() + ",";
		string fps = (m_frameCounter / m_timer).ToString();
		line = line + "," + fps + "," 
			+ GameObjectCount.ToString() + "," + DrawcallCount.ToString() + "," + TriangleCount.ToString() + "," + VertexCount.ToString();
		
		Writer.WriteLine(line);
		Writer.Flush();
	}
	
	public void CloseFile()
	{
		Writer.Close();
		PFileStream.Close();
	}
	
	public void AutoCreatePrefab()
	{
		GameObject go = null;
		Vector3 position = new Vector3();
		
		if (IsPositiveSign)
		{
			go = GameObject.Instantiate(Resources.Load("TempArtist/Prefab/TestCopyScene"), 
			Vector3.one, Quaternion.identity) as GameObject;
	
			if (go == null)
			{
				Debug.Log("GameObject.Instantiate TempArtist/Prefab/TestCopyScene is Wrong!!!!");
				return;
			}
		}
		else
		{
			go = GameObject.Instantiate(Resources.Load("TempArtist/Prefab/ship5"), 
			Vector3.one, Quaternion.identity) as GameObject;
	
			if (go == null)
			{
				Debug.Log("GameObject.Instantiate TempArtist/Prefab/ship5 is Wrong!!!!");
				return;
			}
		}
		
		IsPositiveSign = !IsPositiveSign;
		position.x = Random.Range(Globals.Instance.MSceneManager.mMainCamera.transform.position.x + 100 * -1, Globals.Instance.MSceneManager.mMainCamera.transform.position.x + 100 * 1);
		position.y = Random.Range(Globals.Instance.MSceneManager.mMainCamera.transform.position.y + 100 * -1, Globals.Instance.MSceneManager.mMainCamera.transform.position.y + 100 * 1);
		position.z = Random.Range(Globals.Instance.MSceneManager.mMainCamera.transform.position.z + 100.0f, Globals.Instance.MSceneManager.mMainCamera.transform.position.z + 500 * 1);
		
		go.transform.position = position;
		
		// 
		MaterialNames.Clear();
		for (int idx = 0; idx < go.transform.GetChildCount(); ++idx)
		{
			// Statistics Count
			GameObjectCount++;
			
			GameObject child = go.transform.GetChild(idx).gameObject;
			MeshFilter filter = child.GetComponent<MeshFilter>() as MeshFilter;
			MeshRenderer renderer = child.GetComponent<MeshRenderer>() as MeshRenderer;
			
			// Stat. Mesh information
			if (filter)
			{
				VertexCount += filter.mesh.vertexCount;
				TriangleCount += filter.mesh.triangles.Length;
				
				// DrawcallCount += filter.mesh.subMeshCount;
			}
			
			// Stat. Renderer information
			if (renderer)
			{
				for (int matIdx = 0; matIdx < renderer.materials.Length; ++matIdx)
				{
					Material mat = renderer.materials[matIdx];
					
					if (MaterialNames.Contains(mat.name))
						continue;
					
					MaterialNames.Add(mat.name);
				}
			}
		}
		DrawcallCount += MaterialNames.Count;
	}
	
	float FPS = 0;
	
	string FilePath;
	System.IO.FileStream PFileStream;
	StreamWriter Writer;
	
	bool IsPositiveSign = false;
	
	int GameObjectCount = 0;
	int DrawcallCount = 0;
	int BatchCount = 0;
	int TriangleCount = 0;
	int VertexCount = 0;
	
	List<string> MaterialNames = new List<string>();
}
