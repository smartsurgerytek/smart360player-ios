using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[Serializable]
public class JsonFileAccessor<T> : IAccessor<T>
{

    [SerializeField] private IReader<string> _relativePath;
    [ShowInInspector]
    private string relativePath
    {
        get
        {
            try
            {
                return _relativePath?.Read();
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                return null;
            }
        }
    }
    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector] private bool isFileExist => File.Exists(absolutePath);

    [ShowInInspector]
    private bool isFolderExist
    {
        get
        {
            try
            {
                return Directory.Exists(Path.GetDirectoryName(absolutePath));
            }
            catch
            {
                return true;
            }
        }
    }
    T IReader<T>.Read()
    {
        var json = File.ReadAllText(absolutePath);
        return JsonUtility.FromJson<T>(json);
    }

    void IWriter<T>.Write(T data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(absolutePath, json);
    }
    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write((T)value);
    }
#if UNITY_EDITOR
    [ShowInInspector, FoldoutGroup("Debug"), HideIf(nameof(isFolderExist)), TableList] private OdinCreateFolderButton[] _createFolderButton
    {
        get
        {
            var rt = new List<OdinCreateFolderButton>();
            try
            {
                var currentFolder = Path.GetDirectoryName(absolutePath);
                while (!string.IsNullOrEmpty(currentFolder) && !Directory.Exists(currentFolder))
                {
                    rt.Add(new(currentFolder));
                    currentFolder = Path.GetDirectoryName(currentFolder);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            rt.Reverse();
            return rt.ToArray();
        }
        set { }
    }

    [Serializable]
    struct OdinCreateFolderButton
    {
        [ReadOnly,ShowInInspector,NonSerialized] private string _folderPath;

        public OdinCreateFolderButton(string folderPath)
        {
            _folderPath = folderPath;
        }

        [Button] private void Create()
        {
            if (Directory.Exists(_folderPath)) return;
            Directory.CreateDirectory(_folderPath);
        }
    }
#endif
}
