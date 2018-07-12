using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class VersionStruct
{
    public int value;
    public string date;
}

public class Md5Manager{

    public Dictionary<string, string> m_mFileMd5 = new Dictionary<string, string>();
    public Dictionary<string, FileDirtyEnum> m_mFileDirty = new Dictionary<string, FileDirtyEnum>();
    public Dictionary<string, VersionStruct> m_mVersion = new Dictionary<string, VersionStruct>();
    public Dictionary<string, BundleDependency> m_mDepends = new Dictionary<string, BundleDependency>();
    public int m_maxValue = -1;
    public string m_currDate = null;
	public string m_newVersionName = null;
    public enum FileDirtyEnum { FRESH, DIRTY, CLEAN }  //0 fresh 1 dirty 2 nodirty but calculated
	//windows
    //public const string DirectName = "Assets\\VersionData\\";
	//mac
	public const string DirectName = "Assets/VersionData/";
    public const string FileName = DirectName + "md5.txt";
    public const string VersionName = DirectName + "versionFile.txt";

    static readonly Md5Manager instance = new Md5Manager();

    static Md5Manager() { }

    Md5Manager()   { }

    public static Md5Manager Instance
    {
        get
        {
            return instance;
        }
    }

    public void Start()
    {
        OpenData();
    }
	
	public bool CheckVerion()
	{
		SaveData();
		return !string.IsNullOrEmpty(m_newVersionName);
	}
	
    public void End()
    {  
        m_mFileMd5.Clear();
        m_mFileDirty.Clear();
        m_mVersion.Clear();
        m_mDepends.Clear();
        m_maxValue = -1;
		m_newVersionName = null;
    }
	

    public void OpenData()
    {
		if(File.Exists(FileName))
        {		
            StreamReader sr = new StreamReader(new FileStream(FileName, FileMode.Open));
            char[] spiltChar = new char[1] { '$' };
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] strings = line.Split(spiltChar);
                if (strings.Length == 2)
                {
                    m_mFileMd5.Add(strings[0], strings[1]);
                    m_mFileDirty.Add(strings[0], FileDirtyEnum.FRESH);
                }
            }
            sr.Close();
        }
		
		if(File.Exists(VersionName))
        {
            StreamReader sr = new StreamReader(new FileStream(VersionName, FileMode.Open));
            char[] spiltChar = new char[1] { '$' };
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] strings = line.Split(spiltChar);
                if (strings.Length == 3)
                {
                    VersionStruct vs = new VersionStruct();
                    vs.value = int.Parse(strings[1]);
                    vs.date = strings[2];
                    m_mVersion.Add(strings[0], vs);
                    if (vs.value > m_maxValue)
                    {
                        m_maxValue = vs.value;
                    }
                }
            }
            sr.Close();
        }
        m_maxValue++;


        

    }

    public void SaveData()
    {
        if (!Directory.Exists(DirectName))
            Directory.CreateDirectory(DirectName);
        {

            StreamWriter servWriter = new StreamWriter(new FileStream(FileName, FileMode.Create));
            List<string> strings = new List<string>();
            foreach (string k in m_mFileMd5.Keys)
            {
                strings.Add(k);
            }
            strings.Sort();

            foreach (string s in strings)
            {
                servWriter.WriteLine(s + '$' + m_mFileMd5[s]);
            }
            servWriter.Close();
        }

        {
			foreach(string key in m_mDepends.Keys)
			{
				if(m_mDepends[key].GetDirty())
				{
					if(m_mVersion.ContainsKey(key))
					{
						m_mVersion[key].value = m_maxValue;
                        m_mVersion[key].date = m_currDate;
					}
					else
					{
                        VersionStruct vs = new VersionStruct();
                        vs.value = m_maxValue;
                        vs.date = m_currDate;
                        m_mVersion.Add(key, vs);
					}
				}
			}
			
            StreamWriter verWriter = new StreamWriter(new FileStream(VersionName, FileMode.Create));
            List<string> strings = new List<string>();
            foreach (string k in m_mVersion.Keys)
            {
                strings.Add(k);
            }
            strings.Sort();

            foreach (string s in strings)
            {
                verWriter.WriteLine(s + '$' + m_mVersion[s].value.ToString() + '$' + m_mVersion[s].date);
            }
            verWriter.Close();
        }
    }

    public bool IsFileSame(string name)
    {
        bool retflg = false;
        if ( m_mFileMd5.ContainsKey(name) && m_mFileDirty[name] != FileDirtyEnum.FRESH)
        {
            retflg = m_mFileDirty[name] == FileDirtyEnum.CLEAN;
        }
        else
        {
            MD5 md5Hash = MD5.Create();
            Stream stream = File.Open(name, FileMode.Open,  FileAccess.Read, FileShare.Read);
            string md5value = GetMd5Hash(md5Hash, stream);
            if (m_mFileMd5.ContainsKey(name))
            {
                if( md5value == m_mFileMd5[name])
                {
                    retflg = true;
                    m_mFileDirty[name] = FileDirtyEnum.CLEAN;
                }
                else
                {
                    m_mFileDirty[name] = FileDirtyEnum.DIRTY;
                }
                m_mFileMd5[name] = md5value;

            }
            else
            {
                m_mFileMd5.Add(name, md5value);
                m_mFileDirty[name] = FileDirtyEnum.DIRTY;
            }

            stream.Close();
        }

        return retflg;
    }

    string GetMd5Hash(MD5 md5Hash, Stream stream)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash( stream );

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    public BundleDependency CreateBundleDependency(string name)
    {
        if(m_mDepends.ContainsKey(name))
        {
            Debug.LogError("createbundle arleady exist:" + name);
            return m_mDepends[name];
        }
        BundleDependency bd = new BundleDependency(name);
        m_mDepends.Add(name, bd);
        return bd;
    }
	
	public bool CheckMd5PrefabDirty(string name, Object[] objs, string[] deps = null)
	{
		BundleDependency bd = CreateBundleDependency(name);
        List<BundleDependency> depends = new List<BundleDependency>();
        if(deps != null)
        {
            foreach (string d in deps)
            {
                if (m_mDepends.ContainsKey(d))
                {
                    depends.Add(m_mDepends[d]);
                }
            }
        }

        
		List<string> paths = new List<string>();
		foreach(Object obj in objs)
		{
	        string path = AssetDatabase.GetAssetPath(obj);
	        paths.Add(path);
			
			//self arealdy in dependency
	        //bd.AddFileName(path);
		}
	        
	
        //dependency
        string[] ss = AssetDatabase.GetDependencies(paths.ToArray());
        //Debug.Log("ss:" + ss.Length);
        foreach (string s in ss)
        {
            bd.AddFileName(s, depends);
            //Debug.Log("chpath:" + s);
        }
		
		Debug.Log("result:" + name + bd.GetDirty().ToString());
		if(bd.GetDirty())
		{
	        
	        bd.PrintDirtyFile();
			return true;
		}
		
		return false; 
	}
	
	public bool CheckMd5SceneDirty(string mapName, string path)
	{
        EditorApplication.OpenScene(path);
        GameObject scene = GameObject.Find("Scene");
        GameObject camera = GameObject.Find("Main Camera");

        if (scene && camera)
        {
            BundleDependency bd = CreateBundleDependency(mapName);

            //dependency
            Object[] objs = new Object[2] { scene, camera };
            Object[] rets = EditorUtility.CollectDependencies( objs );
            //Debug.Log("rets:" + rets.Length);
            foreach (Object oc in rets)
            {
                //Debug.Log("dep:" + oc.ToString());
                string s = AssetDatabase.GetAssetPath(oc);
                bd.AddFileName(s);
                //Debug.Log("path:" + s);
            }

            //child prefab
            Transform[] trans = scene.GetComponentsInChildren<Transform>();
            foreach(Transform t in trans)
            {
                GameObject child = t.gameObject;
                if (PrefabType.None == EditorUtility.GetPrefabType(child))
                    continue;
                if ((Object)child != EditorUtility.FindPrefabRoot(child))
                    continue;
                //Debug.Log("prefab:" + child.ToString());
                string s = AssetDatabase.GetAssetPath( EditorUtility.GetPrefabParent( child ) );
                bd.AddFileName(s);
                //Debug.Log("prefabPath:" + s);
                
            }

            //bd.AddFileName(path);
            //List<string> assets = new List<string>();
            //string lightMapPath = Path.Combine(Application.dataPath, "Map/" + mapName);
            //BuildAssetStruct.GetAllAssetsString(lightMapPath, assets, true);
            //foreach(string s in assets)
            //{
            //    bd.AddFileName(s);
            //}
            //Debug.Log("result:" + mapName + bd.GetDirty().ToString());
	  		if(bd.GetDirty())
			{
		        
		        bd.PrintDirtyFile();
				return true;
			}

        }	
		else
		{
			return true;
			Debug.LogError("cant find scene or camera obj in " + mapName);
		}
		return false;
	}

    public bool CheckMd5UIMapDirty(string bundleName, string mapName, string path, string[] deps = null)
    {
        EditorApplication.OpenScene(path);
        GameObject scene = GameObject.Find(mapName);

        if (scene)
        {
            BundleDependency bd = CreateBundleDependency(bundleName);

            List<BundleDependency> depends = new List<BundleDependency>();
            if (deps != null)
            {
                foreach (string d in deps)
                {
                    if (m_mDepends.ContainsKey(d))
                    {
                        depends.Add(m_mDepends[d]);
                    }
                }
            }

            Object[] objs = new Object[1] { scene };

            //dependency
            Object[] rets = EditorUtility.CollectDependencies(objs);
            //Debug.Log("rets:" + rets.Length);
            foreach (Object oc in rets)
            {
                //Debug.Log("dep:" + oc.ToString());
                string s = AssetDatabase.GetAssetPath(oc);
                bd.AddFileName(s, depends);
                //Debug.Log("path:" + s);
            }

            bd.AddFileName(path, depends);

            //Debug.Log("result:" + mapName + bd.GetDirty().ToString());
            if (bd.GetDirty())
            {

                bd.PrintDirtyFile();
                return true;
            }

        }
        else
        {
			return true;
            Debug.LogError("cant find uimap obj in " + mapName);
        }
        return false;
    }
	
	public string GetNewVersionName()
	{
		if(string.IsNullOrEmpty(m_newVersionName))
		{
            System.DateTime currentTime = System.DateTime.Now;
            m_currDate = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}", currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute);
            m_newVersionName = m_currDate + "_" + (m_maxValue).ToString();
		}
		return m_newVersionName;
	}
}
