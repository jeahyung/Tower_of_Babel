using System.IO;
using UnityEngine;

public static class SystemPath
{
    public static string GetPath(string fileName)
    {
        string path = GetPath();
        return Path.Combine(GetPath(), fileName);
    }

    public static string GetPath()
    {
        string path = null;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(Application.persistentDataPath, "Resources/Data/");
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/Data/");
            case RuntimePlatform.WindowsEditor:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Assets", "Resources/Data/");
            default:
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                return Path.Combine(path, "Resources/Data/");
        }
    }

    //public static string GetPath()
    //{
    //    string path = null;
    //    switch (Application.platform)
    //    {
    //        case RuntimePlatform.Android:
    //            path = Application.persistentDataPath;
    //            path = path.Substring(0, path.LastIndexOf('/'));
    //            return Path.Combine(Application.persistentDataPath, "Resources/Data/");
    //        case RuntimePlatform.IPhonePlayer:
    //        case RuntimePlatform.OSXEditor:
    //        case RuntimePlatform.OSXPlayer:
    //            path = Application.persistentDataPath;
    //            path = path.Substring(0, path.LastIndexOf('/'));
    //            return Path.Combine(path, "Assets", "Resources/Data/");
    //        case RuntimePlatform.WindowsEditor:
    //            path = Application.dataPath;
    //            path = path.Substring(0, path.LastIndexOf('/'));
    //            return Path.Combine(path, "Assets", "Resources/Data/");
    //        default:
    //            path = Application.dataPath;
    //            path = path.Substring(0, path.LastIndexOf('/'));
    //            return Path.Combine(path, "Resources/Data/");
    //    }
    //}
}