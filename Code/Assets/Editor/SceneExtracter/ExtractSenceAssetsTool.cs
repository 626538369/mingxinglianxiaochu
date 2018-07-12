using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

/// <summary>
/// Extract assets list file for the object in sence.
/// </summary>
public class ExtractSenceAssetsTool
{
	[MenuItem ("GfanTools/Extract Scene/Extract Sence Assets")]
	static void ExtractSenceAssets ()
	{
		Debug.Log ("======Extract Begin======");
		
		string projectPath = System.IO.Directory.GetCurrentDirectory ();
		string senceName = EditorApplication.currentScene;
		string[] senceNameArr = senceName.Split(new char[] {'/'});
		
		senceName = senceNameArr[senceNameArr.Length - 1];	
		senceName = projectPath + "/Assets/Resources/Config/SceneAssetsFileConfig/" + senceName;
		senceName = Path.ChangeExtension(senceName, ".xml");
		
		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		
		if (Selection.transforms.Length == 0) 
		{
			Debug.Log ("No Selection Object");
		} 
		else 
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.AppendLine("<Main>");
			//Get root object 
			GameObject rootGO = Selection.gameObjects [0];
			//Get all objects contain root object
			Transform[] transforms = rootGO.GetComponentsInChildren<Transform> ();
			foreach (Transform transform in transforms) {
				
				sb.AppendLine("<Asset");
		
				GameObject go = transform.gameObject;
				Object asset = PrefabUtility.GetPrefabParent (go);
				//Debug.Log ("Asset_Path:" + asset.name);
				
				sb.Append(" Name=\"");
				sb.Append(go.name);
				sb.AppendLine("\" ");
				
				sb.Append(" Source=\"");
				sb.Append(AssetDatabase.GetAssetPath (asset));
				sb.AppendLine("\" ");
				
				System.Text.StringBuilder tmpSb = new System.Text.StringBuilder ();
				tmpSb.Append(transform.position.x.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.position.y.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.position.z.ToString());
				sb.Append(" Position=\"");
				sb.Append(tmpSb.ToString());
				sb.AppendLine("\" ");
				
				tmpSb = new System.Text.StringBuilder ();
				tmpSb.Append(transform.eulerAngles.x.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.eulerAngles.y.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.eulerAngles.z.ToString());
				sb.Append(" Rotation=\"");
				sb.Append(tmpSb.ToString());
				sb.AppendLine("\" ");
				
				tmpSb = new System.Text.StringBuilder ();
				tmpSb.Append(transform.localScale.x.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.localScale.y.ToString());
				tmpSb.Append(",");
				tmpSb.Append(transform.localScale.z.ToString());
				sb.Append(" Scale=\"");
				sb.Append(tmpSb.ToString());
				sb.AppendLine("\" ");
				
				sb.AppendLine(" />");
			}
			sb.AppendLine("</Main>");
		}
		
		FileStream fs = new FileStream (senceName, FileMode.Create);
		StreamWriter sw = new StreamWriter (fs);
		sw.Write (sb.ToString ());
		sw.Flush ();
		sw.Close ();
		fs.Close ();
		
		Debug.Log ("======Extract End======");
	}
}
