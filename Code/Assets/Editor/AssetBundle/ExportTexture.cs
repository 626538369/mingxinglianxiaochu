using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;


public class ExportTexture {

    [MenuItem("CONTEXT/Texture2D/Export Texture")]
    static void test(MenuCommand cmd)
    {

        Texture2D tex = cmd.context as Texture2D;

        string filename = EditorUtility.SaveFilePanel("Export", Application.dataPath, "", "png");

        var bytes = tex.EncodeToPNG();
        
        Debug.Log("writing file: " + filename);
        File.WriteAllBytes(filename, bytes);

    }
}
