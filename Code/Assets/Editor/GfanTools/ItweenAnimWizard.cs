using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ItweenAnimWizard : EditorWindow
{
	//----------------------------------------------------------------------
	[HideInInspector] public bool absoluteAnim = false;
	public Vector3 startPostion = Vector3.zero;
	public Vector3 startEulerAngles = Vector3.zero;
	public List<AnimKeyFrameInfo> animFrames = new List<AnimKeyFrameInfo>();
	//----------------------------------------------------------------------
	
	//----------------------------------------------------------------------
	GameObject currGameObj = null;
	// SerializedProperty vector3Prop;
	//----------------------------------------------------------------------
	public void LoadSettings()
	{
		// PlayerSettings.
		string val = PlayerPrefs.GetString("absoluteAnim");
		absoluteAnim = bool.Parse(val);
		
		val = PlayerPrefs.GetString("startPostion");
		startPostion = Vector3Parse(val);
		
		val = PlayerPrefs.GetString("startEulerAngles");
		startEulerAngles = Vector3Parse(val);
		
		GetSelectionInfo();
	}
	
	public void SaveSettings()
	{
		PlayerPrefs.SetString("absoluteAnim", absoluteAnim.ToString());
		PlayerPrefs.SetString("startPostion", startPostion.ToString());
		PlayerPrefs.SetString("startEulerAngles", startEulerAngles.ToString());
	}
	
	internal Vector3 Vector3Parse(string val, string separate = ",")
	{
		Vector3 result = Vector3.zero;
		
		val = val.Replace("(", "");
		val = val.Replace(")", "");
		string[] vals = val.Split(separate.ToCharArray());
		if (3 == vals.Length)
		{
			result[0] = float.Parse(vals[0]);
			result[1] = float.Parse(vals[1]);
			result[2] = float.Parse(vals[2]);
		}
		
		return result;
	}
	
	internal void GetSelectionByFiltered()
	{
		Object[] sel = Selection.GetFiltered(typeof(Material), SelectionMode.ExcludePrefab);
	}
	
	internal void GetSelectionInfo()
	{
		if (null == Selection.activeGameObject)
			return;
		
		RelAnimControl animCtrl = Selection.activeGameObject.GetComponent<RelAnimControl>() as RelAnimControl;
		if (null != animCtrl)
		{
			animFrames.Capacity = animCtrl.keyFrameInfos.Length + 5;
			foreach (AnimKeyFrameInfo info in animCtrl.keyFrameInfos)
			{
				animFrames.Add(info);
			}
		}
	}
	
	void OnClickOkPoint()
	{
		// string assetPath = Application.dataPath;
		if (null != animFrames && 0 != animFrames.Count)
		{
			RelAnimControl animCtrl = Selection.activeGameObject.GetComponent<RelAnimControl>() as RelAnimControl;
			if (null == animCtrl)
			{
				bool isOk = EditorUtility.DisplayDialog("Warning", "Your select gameObject has no attach a RelAnimControl.cs, will auto attach it?", "Ok", "Cancel");
				if (isOk)
					animCtrl = Selection.activeGameObject.AddComponent<RelAnimControl>() as RelAnimControl;
				else
				{
					Close();
					return;
				}
			}
			
			animCtrl.origPos = startPostion;
			animCtrl.keyFrameInfos = new AnimKeyFrameInfo[animFrames.Count];
			animFrames.ToArray().CopyTo(animCtrl.keyFrameInfos, 0);
		}
		
		SaveSettings();
		Close();
	}
	
	// Response when user click the "AddPoint" Button
	void OnClickThisPoint()
	{
		if (null == Selection.activeGameObject)
		{
			EditorUtility.DisplayDialog("Warning", "Please selection your target gameObject.", "Ok");
			return;
		}
		
		Transform tf = Selection.activeGameObject.transform;
		// Debug.Log("" + tf.position);
		// Debug.Log("" + tf.rotation.eulerAngles);
		
		AnimKeyFrameInfo frame = new AnimKeyFrameInfo();
		frame.Position = absoluteAnim ? tf.position : tf.position - startPostion;
		
		Vector3 tempAngles = absoluteAnim ? tf.rotation.eulerAngles : tf.rotation.eulerAngles - startEulerAngles;
		tempAngles.x %= 360;
		tempAngles.y %= 360;
		tempAngles.z %= 360;
		frame.EulerAngles = tempAngles;
		frame.TransformTime = 1.0f;
		frame.DelayTime = 0.0f;
		
		if (currGameObj != Selection.activeGameObject)
		{
			animFrames.Clear();
			currGameObj = Selection.activeGameObject;
			
			RelAnimControl animCtrl = currGameObj.GetComponent<RelAnimControl>() as RelAnimControl;
			if (null == animCtrl
				|| null == animCtrl.keyFrameInfos || 0 == animCtrl.keyFrameInfos.Length)
			{
				animFrames = new List<AnimKeyFrameInfo>();
				animFrames.Capacity = 5;
			}
			else
			{
				animFrames = new List<AnimKeyFrameInfo>();
				animFrames.Capacity = animCtrl.keyFrameInfos.Length + 5;
				foreach (AnimKeyFrameInfo info in animCtrl.keyFrameInfos)
				{
					animFrames.Add(info);
				}
			}
			
			animFrames.Add(frame);
		}
		else
		{
			animFrames.Add(frame);
		}
		
		needUpdateFrames = true;
	}
	
	void OnWizardUpdate()
	{
	}
	
	string myString = "Hello World";
    bool settingEnabled = true;
    // float myFloat = 1.23f;
	Vector2 guiSize = new Vector2(600.0f, 500.0f);
	bool needUpdateFrames = false;
	Vector2 scrollPos = Vector3.zero;
	
	void OnGUI()
	{
		// GUI.matrix = Matrix4x4.Scale(new Vector3( Screen.width / guiSize.x, Screen.height / guiSize.y, 1));
		// GUILayout.BeginArea(new Rect(0, 0, guiSize.x, guiSize.y));
		
		GUILayout.Label("NOTE: Press AddPoint will add the current point info at the end of list. you can manual modify the time options.", EditorStyles.largeLabel);
	    // myString = EditorGUILayout.TextField ("Text Field", myString);
		// EditorGUILayout.TagField("11111");
		
		settingEnabled = EditorGUILayout.BeginToggleGroup ("Animation Settings", settingEnabled);
		absoluteAnim = EditorGUILayout.Toggle("AbsoluteAnim", absoluteAnim);
	    // myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		
		EditorGUILayout.BeginVertical();  
		startPostion = EditorGUILayout.Vector3Field("startPostion", startPostion);
		startEulerAngles = EditorGUILayout.Vector3Field("startEulerAngles", startEulerAngles);
		// GUILayout.Space(50);
		if (GUILayout.Button("AsStartPoint", GUILayout.Width(100), GUILayout.Height(20)))
		{
			OnClickAddStart();
		}
		EditorGUILayout.EndVertical();
	    EditorGUILayout.EndToggleGroup();
		
		// EditorGUILayout.BeginHorizontal();
		// GUILayout.Box(GUIContent.none, GUI.skin.label, GUILayout.Width(400), GUILayout.Height(100));
		// EditorGUILayout.EndHorizontal();
		
		// int index = 0;
		// index = EditorGUILayout.Popup(index, new string[10], GUILayout.MaxWidth(90f));
		EditorGUILayout.BeginVertical();  
		GUILayout.Space(10);
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical();
		GUILayout.Label("Animation Frame info", EditorStyles.label);
		GUILayout.Label("Size is: " + animFrames.Count.ToString(), EditorStyles.label);
		scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(600), GUILayout.Height(200));
		for (int i = 0; i < animFrames.Count; ++i)
		{
			GUILayout.Label("Element" + i.ToString() + ":", EditorStyles.label);
			animFrames[i].Position = EditorGUILayout.Vector3Field("Postion", animFrames[i].Position);
			animFrames[i].EulerAngles = EditorGUILayout.Vector3Field("EulerAngles", animFrames[i].EulerAngles);
			
			EditorGUILayout.BeginHorizontal();
			animFrames[i].TransformTime = EditorGUILayout.FloatField("TransformTime", animFrames[i].TransformTime);
			animFrames[i].DelayTime = EditorGUILayout.FloatField("DelayTime", animFrames[i].DelayTime);
			EditorGUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(350);
		if (GUILayout.Button("AddThisPoint", GUILayout.Width(100), GUILayout.Height(20)))
		{
			OnClickThisPoint();
		}
		
		if (GUILayout.Button("Ok", GUILayout.Width(100), GUILayout.Height(20)))
		{
			OnClickOkPoint();
		}
		EditorGUILayout.EndHorizontal();

		// GUILayoutOption options = new GUILayoutOption();
		// EditorGUILayout.Vector3Field("Start postion:", Vector3.zero, GUILayout.Width(100), GUILayout.Height(100));
		
		// EditorGUILayout.BeginHorizontal();  
		// EditorGUILayout.PropertyField(vector3Prop, GUIContent.none, MarginTypeWidth);  
		// EditorGUILayout.PropertyField(MarginType, GUIContent.none, LabelWidth);  
		// EditorGUILayout.EndHorizontal();
		// GUILayout.EndArea();
	}
	
	// string t;
	// void OnGUI()
	// {
	// 	EditorGUILayout.BeginVertical();
    //         scrollPos = 
    //         EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (300), GUILayout.Height (100));
    //             GUILayout.Label(t);
    //         EditorGUILayout.EndScrollView();
    //         if(GUILayout.Button("Add More Text", GUILayout.Width (100), GUILayout.Height (100)))
    //             t += " \nAnd this is more text!";
    //     EditorGUILayout.EndVertical();
    //     if(GUILayout.Button("Clear"))
    //         t = "";
	// }
	
	void OnClickAddStart()
	{
		if (null == Selection.activeGameObject)
		{
			EditorUtility.DisplayDialog("Warning", "Please selection your target gameObject.", "Ok");
			return;
		}
		
		Transform tf = Selection.activeGameObject.transform;
		startPostion = tf.transform.position;
		startEulerAngles = tf.transform.rotation.eulerAngles;
	}
}
