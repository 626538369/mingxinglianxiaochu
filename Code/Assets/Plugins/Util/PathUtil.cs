using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class PathUtil
{
	/// <summary>
	/// Check if the file name is in a path name.
	/// </summary>
	/// <param name="fileName">File name to be checked</param>
	/// <param name="pathName">Path name of the file name may be in</param>
	/// <returns>If the file name is in the path name return ture, otherwise return false</returns>
	public static bool IsFileInPath(string fileName, string pathName)
	{
		// Unify the path for comparing
		fileName = UnifyPath(fileName);
		pathName = UnifyPath(pathName);

		return fileName.StartsWith(pathName);
	}
	
	/// <summary>
	/// Get the path name relate root path
	/// </summary>
	/// <param name="path">Source file name</param>
	/// <param name="rootPath">Root path where the source file may be in</param>
	/// <param name="extension">Whether or not the return path contains extension</param>
	/// <returns>Return the relative path</returns>
	public static string GetPathInFolder(string path, string rootPath, bool extension)
	{
		// Unify the path for comparing
		string lowerPath = UnifyPath(path);
		string lowerRootPath = UnifyPath(rootPath);

		// Get sub file name
		if (lowerPath.StartsWith(lowerRootPath))
		{
			lowerPath = lowerPath.Remove(0, lowerRootPath.Length);
		}		

		// Remove extension
		if (!extension)
		{
			lowerPath = CombinePath(Path.GetDirectoryName(lowerPath), Path.GetFileNameWithoutExtension(lowerPath));
		}

		return UnifyPath(lowerPath);
	}
	
	public static string UnifyPath( string path )
	{
		string unifiedPath = path.ToLower();
		unifiedPath = unifiedPath.Replace('\\', Defines.DIRECTORY_CHAR);
		unifiedPath = unifiedPath.Trim(Defines.DIRECTORY_CHAR);

		return unifiedPath;
	}
	
	public static string UnifyPathNoLower( string unifiedPath )
	{
		unifiedPath = unifiedPath.Replace('\\', Defines.DIRECTORY_CHAR);
		unifiedPath = unifiedPath.Trim(Defines.DIRECTORY_CHAR);

		return unifiedPath;
	}
	
	public static string RemoveLocalFlag(string str)
	{
		str = str.ToLower();
		str = str.Replace('\\', Defines.DIRECTORY_CHAR);
		str = str.Replace(Defines.LOCAL_FILE_FLAG ,"");
		return str;
	}
	
	// Combine path.
	public static string CombinePath( params string [] pathes )
	{
		if ( pathes == null || pathes.Length == 0 )
			return string.Empty;
		
		StringBuilder bd = new StringBuilder();
		
		foreach ( string p in pathes )
		{
			bd.AppendFormat("{0}{1}", p, Defines.DIRECTORY_CHAR);
		}
		
		return UnifyPath( bd.ToString() );
	}
}
