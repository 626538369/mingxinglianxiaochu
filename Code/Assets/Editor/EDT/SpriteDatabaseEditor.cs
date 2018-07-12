using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(SpriteDatabase))]
public class SpriteDatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        SpriteDatabase db = target as SpriteDatabase;

        if (db.sprites != null)
            GUILayout.Label("Sprite count:" + db.sprites.Count);
        if (GUILayout.Button("Refresh"))
            refresh(db);

        if (GUI.changed)
            EditorUtility.SetDirty(db);
    }

    void refresh(SpriteDatabase db)
    {
        Debug.Log("Refreshing sprite database \"" + db.name + "\"");
        db.sprites = new List<Sprite>();
        string path = AssetDatabase.GetAssetPath(db);
        path = AssetPath.getAbsolutePath(path);
        path = path.Replace(Path.GetFileName(path), "");
        foreach(var f in FileSearch.search(path, "*"))
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath(AssetPath.getRelativePath(f.FullName));
            foreach(var a in assets)
            {
                Sprite s = a as Sprite;
                if(s != null)
                {
                    db.sprites.Add(s);
                    Debug.Log("Find sprite \"" + s.name + "\"");
                }
            }
        }
        Debug.Log("Sprite database \"" + db.name + "\" refreshed:(" + db.sprites.Count + ")");
    }
}
