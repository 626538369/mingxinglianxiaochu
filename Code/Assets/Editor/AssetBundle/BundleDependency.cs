using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BundleDependency{

    List<string> m_Depends = new List<string>();
    bool m_dirty = false;
    string m_name;

    public BundleDependency(string name)
    {
        m_name = name;
    }

    public void AddFileName(string s, List<BundleDependency> bds = null)
    {
        if (string.IsNullOrEmpty(s))
            return;
        if (IsInDependency(s, bds))
            return;
        InnerAddFile(s);
        string metaFile = s + ".meta";
        if( File.Exists(metaFile) )
        {
            InnerAddFile(s+".meta");
        }


    }

    public bool IsInDependency(string s, List<BundleDependency> bds)
    {
        if (bds == null)
            return false;
        foreach (BundleDependency bd in bds)
        {
            if (bd.isContainFile(s))
                return true;
        }
        return false;
    }

    public bool isContainFile(string s)
    {
        return m_Depends.Contains(s);
    }

    private void InnerAddFile(string s)
    {
        bool bFlg = Md5Manager.Instance.IsFileSame(s);
        if (m_Depends.Contains(s))
            return;
        m_Depends.Add(s);
        if (!bFlg && !m_dirty)
        {
            m_dirty = true;
        }
    }

    public bool GetDirty()
    {
        return m_dirty;
    }

    public void PrintDirtyFile()
    {
        foreach(string s in m_Depends)
        {
            if( !Md5Manager.Instance.IsFileSame(s) )
            {
                Debug.Log("dirtyFile: " + s);
            }
        }
    }
}

