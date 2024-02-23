using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonoWrappedAccessor<T> : MonoWrapper<IAccessor<T>>, IAccessor<T>
{
    public T Read()
    {
        return innerData.Read();
    }

    public void Write(T value)
    {
        innerData.Write(value);
    }
}
[Serializable]
public class EasonJsonFileAccessor<T> : IAccessor<T>
{

    [SerializeField] private IAccessor<string> _relativePath;
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

    T IReader<T>.Read()
    {
        IReader<T> reader = new EasonJsonFileReader<T>(absolutePath);
        return reader.Read();
    }
#if UNITY_EDITOR


    [ShowInInspector] private bool isFolderExist
    {
        get
        {
            try
            {
                return Directory.Exists(absolutePath);
            }
            catch
            {
                return true;
            }
        }
    }
    [ShowInInspector, FoldoutGroup("Debug"), HideIf(nameof(isFolderExist))] private OdinCreateFolderButton[] _createFolderButton
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
            catch { }
            rt.Reverse();
            return rt.ToArray();
        }
    }

    void IWriter<T>.Write(T data)
    {
        IWriter<T> writer = new EasonJsonFileWriter<T>(absolutePath);
        writer.Write(data);
    }


    [Serializable]
    struct OdinCreateFolderButton
    {
        [ReadOnly] private string _folderPath;

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
