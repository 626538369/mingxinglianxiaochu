using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class AssetDownLoadManager
{
	public static void Init(){
		
	}
	public static string up;
	public static System.IO.FileStream fileStream;
}

public class DownLoadedObject
{
		
	public DownLoadedObject (string file, string version, string md5)
	{
		this._file = file;
		this._version = version;
		this._md5 = md5;
	}
		
	public string File {
		get{ return _file;}
		set{ _file = value;}
	}
		
	public string Version {
		get{ return _version;}
		set{ _version = value;}
	}
		
	public string Md5 {
		get{ return _md5;}
		set{ _md5 = value;}
	}
		
	private string _file;
	private string _version;
	private string _md5;
		
}