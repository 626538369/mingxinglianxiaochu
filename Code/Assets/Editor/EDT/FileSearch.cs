using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class FileSearch
{
    public static List<FileInfo> search(string path, string pattern)
    {
        DirectoryInfo d = new DirectoryInfo(path);
        return search(d, pattern);
    }
    public static List<FileInfo> search(DirectoryInfo d, string pattern)
    {
        List<FileInfo> files = d.GetFiles(pattern).ToList();
        DirectoryInfo[] dis = d.GetDirectories();
        foreach (DirectoryInfo di in dis)
            files.AddRange(search(di, pattern));
        return files;
    }
}
