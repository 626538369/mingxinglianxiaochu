using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class GenerateMaterials
{
    

    // Returns the path to the directory that holds the specified FBX.
    static string CharacterRoot(GameObject character)
    {
        string root = AssetDatabase.GetAssetPath(character);
        return root.Substring(0, root.LastIndexOf('/') + 1);
    }

    // Returns the path to the directory that holds materials generated
    // for the specified FBX.
    public static string MaterialsPath(GameObject character)
    {
		// we will use it only for file enumeration, and separator will be appended for us
		// if we do append here, AssetDatabase will be confused
        //return CharacterRoot(character) + "Materials";
		return "Assets/Resources/Character/Materials";
    }
}