#if !SERVER
using UnityEngine;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Text;

public static class StrParser
{
	public static readonly char []splitter = { ',' ,'|'};
	public static readonly string [,]spcChars = 
	{ 
		{ "#quot#", "\"" }, 
		{ "#lt#", "<" }, 
		{ "#gt#", ">" },
		{ "#amp#", "&" },
		{ "#apos#", "'" }
	};
	
#if !SERVER	
	public static Color ParseColor( string str )
	{
		if ( str == null )
			return Defines.INVALID_COLOR;
		
		
		string []colors = str.Split( splitter );
		if ( colors.Length < 4 )
			return Defines.INVALID_COLOR;
			
		return new Color( float.Parse( colors[0] ) / 255, float.Parse( colors[1] ) / 255, float.Parse( colors[2] ) / 255, float.Parse( colors[3] ) / 255 );
	}
	
	public static Color ParseLowerColor( string str)
	{
		if ( str == null )
			return Defines.INVALID_COLOR;
		
		////RGBA(222,222,222)/
		if (str.IndexOf("(") != -1)
		{
			str = str.Substring(str.IndexOf("(") + 1);
		}
		
		if (str.IndexOf(")") != -1)
		{
			str = str.Substring(0,str.IndexOf(")"));
		}
		
		string []colors = str.Split( splitter );
		if ( colors.Length < 4 )
			return Defines.INVALID_COLOR;
			
		return new Color( float.Parse( colors[0] ) , float.Parse( colors[1] ), float.Parse( colors[2] ) , float.Parse( colors[3] ));
	}
	
	public static Rect ParseRect( string str )
	{
		if ( str == null )
			return new Rect( 0, 0, 0, 0 );
		
		string []vecs = str.Split( splitter );
		if ( vecs.Length < 4 )
			return new Rect( 0, 0, 0, 0 );
			
		return new Rect( float.Parse( vecs[0] ), float.Parse( vecs[1] ), float.Parse( vecs[2] ), float.Parse( vecs[3] ) );
	}
	
	public static Vector2 ParseVec2( string str )
	{
		if ( str == null )
			return Vector2.zero;
		
		string[] vecs = str.Split( splitter );
		if ( vecs.Length < 2 )
			return Vector2.zero;

		return new Vector2( float.Parse( vecs[0] ), float.Parse( vecs[1] ) );
	}
	
	public static Vector3 ParseVec3( string str )
	{
		if ( str == null )
			return Vector2.zero;
		
		if (str.IndexOf("\\n") != -1)
		{
			str = str.Replace("\\n", "\n");
		}
		
		
		if (str.IndexOf("(") != -1)
		{
			str = str.Replace("(", "");
		}
		if (str.IndexOf(")") != -1)
		{
			str = str.Replace(")", "");
		}
		
		string[] vecs = str.Split( splitter );
		if ( vecs.Length < 3 )
			return Vector3.zero;

		return new Vector3( float.Parse( vecs[0]), float.Parse( vecs[1] ), float.Parse( vecs[2] ) );
	}
#endif

	public static bool ParseBool( string str, bool defVal )
	{
		bool v;
		
		if ( Boolean.TryParse( str, out v ) )
			return v;
		else
			return defVal;
	}
	
	public static int ParseDecInt( string str, int defVal )
	{
		int v;
		
		if ( Int32.TryParse( str, NumberStyles.Integer, _provider, out v ) )
			return v;
		else
			return defVal;
	}
	
	public static double ParseDouble( string str, double defVal )
	{
		double v;
		
		if ( Double.TryParse( str,out v ) )
			return v;
		else
			return defVal;
	}
	
	public static int ParseHexInt( string str, int defVal )
	{
		int v;
		
		if ( Int32.TryParse( str, NumberStyles.HexNumber, _provider, out v ) )
			return v;
		else
			return defVal;
	}

	public static float ParseFloat( string str, float defVal )
	{
		float v = 0;
		
		if ( Single.TryParse( str, out v ) )
			return v;
		else
			return defVal;
	}

	public static List<float> ParseFloatList( string str, float defVal )
	{
		List<float> values = new List<float>();
		
		if ( str == null )
			return values;
		
		string[] vecs = str.Split( splitter );

		for (int i = 0; i < vecs.Length; i++)
			values.Add( ParseFloat( vecs[i], defVal ) );

		return values;
	}

	public static List<int> ParseDecIntList( string str, int defVal )
	{
		List<int> values = new List<int>();
		
		if ( str == null )
			return values;
		
		string[] vecs = str.Split( splitter );

		for (int i = 0; i < vecs.Length; i++)
			values.Add( ParseDecInt( vecs[i], defVal ) );

		return values;
	}
	
	public static List<int> ParseHexIntList( string str, int defVal )
	{
		List<int> values = new List<int>();
		
		if ( str == null )
			return values;
		
		string[] vecs = str.Split( splitter );

		for (int i = 0; i < vecs.Length; i++)
			values.Add( ParseHexInt( vecs[i], defVal ) );

		return values;
	}

	public static T ParseEnum<T>( string val, T defVal ) where T : struct
	{
		Type type = typeof(T);

		try
		{
			if ( !type.IsEnum )
			{
#if !SERVER			
				Debug.LogError( string.Format( "'{0}' is not enum type.", type ) );
#endif
				return defVal;
			}

			return (T)Enum.Parse( type, val, true );
		}
		catch
		{
#if !SERVER	
		if ( val != "" )
			Debug.LogError( string.Format( "'{0}' is not value of {1}.", val, type ) );
#endif
			return defVal;
		}
	}
	
	public static List<T> ParseEnumList<T>( string str, T defVal ) where T : struct
	{
		List<T> values = new List<T>();
		
		if ( str == null )
			return values;
		
		string[] vecs = str.Split(splitter);

		for (int i = 0; i < vecs.Length; i++)
			values.Add( ParseEnum<T>( vecs[i], defVal ) );

		return values;
	}
	
	public static int ParseEnum( string str, int defVal, string []enumVals )
	{
		if ( enumVals == null || str == null )
			return defVal;
		
		for ( int i = 0; i < enumVals.Length; i ++ )
			if ( str == enumVals[i] )
				return i;
			
		return defVal;
	}
	
	public static List<int> ParseEnumList( string str, int defVal, string []enumVals )
	{
		List<int> values = new List<int>();
		
		if ( str == null )
			return values;
		
		string[] vecs = str.Split(splitter);

		for (int i = 0; i < vecs.Length; i++)
			values.Add( ParseEnum( vecs[i], defVal, enumVals ) );

		return values;
	}
	
	public static string ParseEnumStr( int val, string []enumVals )
	{
		if ( enumVals == null || val < 0 || val >= enumVals.Length )
			return "";
		
		return enumVals[val];
	}
	
	public static string ParseEnumFullStr( int val, string []enumVals, string enumName )
	{
		return enumName + "." +ParseEnumStr( val, enumVals );
	}
	
	public static string ParseStr(string str, string defValue)
	{
		return str == null ? defValue : str;
	}
	
	public static string ParseStr( string str, string defValue, bool prcSpcChar )
	{
		if ( str == null )
			return defValue;
		
		if ( !prcSpcChar )
			return str;
		
		StringBuilder bd = new StringBuilder( str );
		
		for ( int i = 0; i < spcChars.GetLength( 0 ); i ++ )
			bd.Replace( spcChars[i,0], spcChars[i,1] );
		
		return bd.ToString();
	}

	public static string Null2Empty(string str)
	{
		return str == null ? "" : str;
	}
	
	private static CultureInfo _provider = CultureInfo.InvariantCulture;
}