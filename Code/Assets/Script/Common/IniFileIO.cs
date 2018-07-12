using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class IniFileIO
{
	//-----------------------------------------------
	public static readonly string IniSeperator = "=";
	public static readonly string SectionLSeperator = "[";
	public static readonly string SectionRSeperator = "]";
	public static bool IgonrePascal = true;
	//-----------------------------------------------
	
	public static string FileName
	{
		get;
		set;
	}
	
	public static bool CreateFile(string name)
	{
		try
		{
			Close();
			
			FileName = name;
			
			fileStream = File.Create(name);
			writer = new StreamWriter(fileStream);
			sectionSettings = new Dictionary<string, Dictionary<string, string>>();
			
			return true;
		}
		catch (FileLoadException e)
		{
			fileStream = null;
			Debug.Log("[IniFileIO:]Cannot create the fie " + name + ". Message: " + e.Message);
		}
		
		return false;
	}
	
	public static bool OpenFile(string name, bool canWrite)
	{
		try
		{
			Close();
			
			FileName = name;
			if (canWrite)
			{
				fileStream = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				writer = new StreamWriter(fileStream);
			}
			else
				fileStream = File.Open(name, FileMode.Open, FileAccess.Read);
			
			reader = new StreamReader(fileStream);
			sectionSettings = new Dictionary<string, Dictionary<string, string>>();
			
			string line = string.Empty;
			string section = string.Empty;
			string key = string.Empty;
			string val = string.Empty;
			int lineNum = 0;
			string[] splitStrings = null;
			
			char[] chars = new char[IniSeperator.Length];
			int i = 0;
			foreach (char c in IniSeperator)
			{
				chars[i] = c;
				i++;
			}
			
			Dictionary<string, string> list = null;
			while (!reader.EndOfStream)
			{
				lineNum++;
				line = reader.ReadLine();
				
				section = SplitSection(line);
				if (IgonrePascal)
					section = section.ToLower(); // Ignore the upper or lower case
				
				if (!string.IsNullOrEmpty(section))
				{
					list = new Dictionary<string, string>();
					sectionSettings.Add(section, list);
				}
				else
				{
					splitStrings = line.Split(chars);
					if (splitStrings.Length == 2)
					{
						key = splitStrings[0];
						key = key.Trim();
						if (IgonrePascal)
							key = key.ToLower();
						
						val = splitStrings[1];
						val = val.Trim();
						if (IgonrePascal)
							val = val.ToLower();
						
						list.Add(key, val);
					}
					else
					{
						Debug.Log("[IniFileIO:]The line in file " + name + " is not a INI config. The line num is " + lineNum);
					}
				}
			}
			
			return true;
		}
		catch (FileNotFoundException e)
		{
			fileStream = null;
			Debug.Log("[IniFileIO:]Cannot find the fie " + name + ". Message: " + e.Message);
			return false;
		}
		
		return false;
	}
	
	public static void OpenBuffer(byte[] bytes)
	{
		Close();
		
		MemoryStream ms = new MemoryStream(bytes);
		reader = new StreamReader(ms);
		sectionSettings = new Dictionary<string, Dictionary<string, string>>();
		
		string line = string.Empty;
		string section = string.Empty;
		string key = string.Empty;
		string val = string.Empty;
		int lineNum = 0;
		string[] splitStrings = null;
		
		char[] chars = new char[IniSeperator.Length];
		int i = 0;
		foreach (char c in IniSeperator)
		{
			chars[i] = c;
			i++;
		}
		
		Dictionary<string, string> list = null;
		while (!reader.EndOfStream)
		{
			lineNum++;
			line = reader.ReadLine();
			
			section = SplitSection(line);
			if (IgonrePascal)
				section = section.ToLower(); // Ignore the upper or lower case
			
			if (!string.IsNullOrEmpty(section))
			{
				list = new Dictionary<string, string>();
				sectionSettings.Add(section, list);
			}
			else
			{
				splitStrings = line.Split(chars);
				if (splitStrings.Length == 2)
				{
					key = splitStrings[0];
					key = key.Trim(); // Remove Space char
					if (IgonrePascal)
						key = key.ToLower();
					
					val = splitStrings[1];
					val = val.Trim();
					if (IgonrePascal)
						val = val.ToLower();
					
					list.Add(key, val);
				}
				else
				{
					// Debug.Log("[IniFileIO:]The line in file " + name + " is not a INI config. The line num is " + lineNum);
					continue;
				}
			}
		}
	}
	
	public static void Close()
	{
		if (null != writer)
			writer.Close();
		if (null != reader)
			reader.Close();
	
		if (null != fileStream)
			fileStream.Close();
		
		fileStream = null;
		writer = null;
		reader = null;
		
		if (null != sectionSettings)
		{
			foreach (Dictionary<string, string> list in sectionSettings.Values)
			{
				list.Clear();
			}
			sectionSettings.Clear();
		}
	}
	
	public static void BeginWrite(bool append)
	{
		Debug.Log("[IniFileIO:]Begin write contents.");
		if (!append)
		{
			// Clear the memory stream first.
			fileStream.SetLength(0);
			writer.Flush();
		}
		
		if (null != sectionSettings)
		{
			foreach (Dictionary<string, string> list in sectionSettings.Values)
			{
				list.Clear();
			}
			sectionSettings.Clear();
		}
	}
	
	public static void EndWrite()
	{
		Debug.Log("[IniFileIO:]End write contents.");
	}
	
	public static void Write(byte[] bytes)
	{
		BinaryWriter bw = new BinaryWriter(fileStream);
		bw.Write(bytes);
	}
	
	public static void Write(string buffer)
	{
		writer.WriteLine(buffer);
	}
	
	public static void Write(string section, string key, string val)
	{
		if (IgonrePascal)
		{
			section = section.ToLower();
			key = key.ToLower();
			val = val.ToLower();
		}
		
		if (null == fileStream)
		{
			throw new FileNotFoundException("");
		}
		
		if (null == writer)
		{
			throw new MissingReferenceException("");
		}
		
		Dictionary<string, string> list = null;
		if (sectionSettings.TryGetValue(section, out list))
		{
			if (list.ContainsKey(key))
			{
				Debug.Log("[IniFileIO:]The setting has a same key, don't write it again.");
			}
			else
			{
				string line = key + IniSeperator + val;
				writer.WriteLine(line);
				
				list.Add(key, val.ToString());
			}
		}
		else
		{
			string line = SectionLSeperator + section + SectionRSeperator;
			writer.WriteLine(line);
			line = key + IniSeperator + val;
			writer.WriteLine(line);
			
			list = new Dictionary<string, string>();
			list.Add(key, val.ToString());
			sectionSettings.Add(section, list);
		}
	}
	
	public static void Write(string section, string key, bool val)
	{
		Write(section, key, val.ToString());
	}
	
	public static void Write(string section, string key, int val)
	{
		Write(section, key, val.ToString());
	}
	
	public static void Write(string section, string key, float val)
	{
		Write(section, key, val.ToString());
	}
	
	public static string ReadString(string section, string key)
	{
		if (IgonrePascal)
		{
			section = section.ToLower();
			key = key.ToLower();
		}
		
		string str = string.Empty;
		
		Dictionary<string, string> list = null;
		if (sectionSettings.TryGetValue(section, out list))
		{
			if (list.TryGetValue(key, out str))
			{
				return str;
			}
		}
		
		return str;
	}
	
	public static bool ReadBool(string section, string key)
	{
		if (IgonrePascal)
		{
			section = section.ToLower();
			key = key.ToLower();
		}
		
		bool val = false;
		string str = string.Empty;
		
		Dictionary<string, string> list = null;
		if (sectionSettings.TryGetValue(section, out list))
		{
			if (list.TryGetValue(key, out str))
			{
				val = StrParser.ParseBool(str, false);
			}
		}
		
		return val;
	}
	
	public static int ReadInt(string section, string key)
	{
		if (IgonrePascal)
		{
			section = section.ToLower();
			key = key.ToLower();
		}
		
		int val = -1;
		string str = string.Empty;
		
		Dictionary<string, string> list = null;
		if (sectionSettings.TryGetValue(section, out list))
		{
			if (list.TryGetValue(key, out str))
			{
				val = StrParser.ParseDecInt(str, -1);
			}
		}
		
		return val;
	}
	
	public static float ReadFloat(string section, string key)
	{
		if (IgonrePascal)
		{
			section = section.ToLower();
			key = key.ToLower();
		}
		
		float val = -1;
		string str = string.Empty;
		
		Dictionary<string, string> list = null;
		if (sectionSettings.TryGetValue(section, out list))
		{
			if (list.TryGetValue(key, out str))
			{
				val = StrParser.ParseFloat(str, -1.0f);
			}
		}
		
		return val;
	}
	
	// private static bool IsSection(string str)
	// {
	// 	return str.Contains(SectionLSeperator) && str.Contains(SectionRSeperator);
	// }
	
	private static string SplitSection(string str)
	{
		int startIndex = str.IndexOf(SectionLSeperator);
		if (-1 == startIndex)
			return string.Empty;
		
		int endIndex = str.IndexOf(SectionRSeperator);
		if (-1 == endIndex)
			return string.Empty;
		
		str = str.Substring(startIndex + 1, endIndex - startIndex - 1);
		return str;
	}
	
	private static FileStream fileStream;
	private static StreamWriter writer;
	private static StreamReader reader;
	
	private static Dictionary<string, Dictionary<string, string>> sectionSettings;
}