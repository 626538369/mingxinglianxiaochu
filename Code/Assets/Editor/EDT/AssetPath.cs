using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** Asset Ä¿Â¼²Ù×÷. */
public static class AssetPath
{
    public static string getAbsolutePath(string pathName)
    {
        return Application.dataPath + pathName.Replace("Assets", "");
    }
    /** 
     * given a path to a node in the filesystem, lop off anything above the project 
     * Assets folder in the pathname so it can work with UnityEditor's built-in commands
     */
    public static string getRelativePath(string pathName)
    {
        //dataPath uses forward slashes on all platforms now
        const string forwardSlash = "/";
        const string backSlash = "\\";
        pathName = pathName.Replace(backSlash, forwardSlash);
        return pathName.Replace(Application.dataPath, "Assets");
    }
}
