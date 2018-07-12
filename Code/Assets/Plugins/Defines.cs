using UnityEngine;
using System.Collections;

public static class Defines 
{
	// Invalid color.
	public readonly static Color INVALID_COLOR = new Color( 1, 1, 1, 0 );
	
	// Invisible position.
	public readonly static Vector3 INVISIBLE_POSITION = new Vector3( 0, 9999, 0 );
	
	// Material property names.
	public readonly static string[] MAT_TEX_PROPERTY_NAMES = { "_MainTex", "_BumpMap", "_Cube" };
	
	// Invalid id.
	public const int INVALID_ID = 0;
	
	// Default resource version.
	public const int DEF_RES_VERSION = 1;
	
	// Asset bundle extension.
	public const string ASSET_BUNDLE_EXTENSION = "assetbundle";
	
	// Directory separator
	public const char DIRECTORY_CHAR = '/';
	
	// Local file flag
	public const string LOCAL_FILE_FLAG = "resources/";
}