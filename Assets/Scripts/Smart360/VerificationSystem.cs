using Eason.Odin;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

public interface ICachedReader
{
    void Load();
}
public interface ICachedWriter
{
    void Save();
}

public interface ICachedAccessor : ICachedReader, ICachedWriter
{

}

public interface ICachedReader<T> : ICachedReader, IReader<T>
{
}
public interface ICachedWriter<T> : ICachedWriter, IWriter<T>
{
}
public interface ICachedAccessor<T> : ICachedReader<T>, ICachedWriter<T>, IAccessor<T>, ICachedAccessor
{

}
public interface IReader<TResult, TParameter>
{
    TResult Read(TParameter parameter);
}
public interface IWriter<TData, TParameter>
{
    void Write(TData data, TParameter parameter);
}
[Serializable]
public struct EasonCredentialSaveLoadParameter
{

    [SerializeField, InfoBox("Folder doesn't exist.", "@!" + nameof(isRootExist), InfoMessageType = InfoMessageType.Error)] private IReader<string> _rootFolderName;
    [SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCredentialExist), InfoMessageType = InfoMessageType.Error)] private string _credentialFileName;


    [ShowInInspector, FoldoutGroup("Debug")] public string rootPath => Path.Combine(Application.persistentDataPath, rootFolderName ?? "").Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector, FoldoutGroup("Debug")] public string credentialPath => Path.Combine(rootPath ?? "", _credentialFileName ?? "");
    [ShowInInspector, FoldoutGroup("Debug")] public bool isRootFolderNameValid => !string.IsNullOrEmpty(rootFolderName);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isCredentialFileNameValid => !string.IsNullOrEmpty(_credentialFileName);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isRootExist => isRootFolderNameValid && Directory.Exists(rootPath);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isCredentialExist => isCredentialFileNameValid && File.Exists(credentialPath);

    public string rootFolderName { get => _rootFolderName?.Read() ?? ""; }

    public void AssertRootFolderName()
    {
        if (!isRootFolderNameValid) throw new Exception("Root folder name cannot be null or empty.");
    }
    public void AssertCredentialFileName()
    {
        if (!isCredentialFileNameValid) throw new Exception("Credential file name cannot be null or empty.");
    }
    public void AssertRootFolder()
    {
        AssertRootFolderName();
        if (!isRootExist) throw new DirectoryNotFoundException($"Root folder \"{rootPath}\" doesn't exist");
    }
    public void AssertCredentialFile()
    {
        AssertCredentialFileName();
        if (!isCredentialExist) throw new FileNotFoundException($"Root folder \"{credentialPath}\" doesn't exist");
    }

    [Button("Open Root Folder"), FoldoutGroup("Debug"), ShowIf(nameof(isRootExist))]
    private void OdinOpen()
    {
        System.Diagnostics.Process.Start("explorer.exe", rootPath);
    }
}
[Serializable]
public struct EasonCredentialSaver : IWriter<Credential, EasonCredentialSaveLoadParameter>
{
    void IWriter<Credential, EasonCredentialSaveLoadParameter>.Write(Credential data, EasonCredentialSaveLoadParameter parameter)
    {
        parameter.AssertRootFolder();
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(parameter.credentialPath, json);
    }
}
[Serializable]
public struct EasonCredentialLoader : IReader<Credential, EasonCredentialSaveLoadParameter>
{
    Credential IReader<Credential, EasonCredentialSaveLoadParameter>.Read(EasonCredentialSaveLoadParameter parameter)
    {
        parameter.AssertCredentialFile();
        var json = File.ReadAllText(parameter.credentialPath);
        return JsonUtility.FromJson<Credential>(json);
    }
}
[Serializable]
public struct ModuleCredential
{
    [SerializeField, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(100, resizable: false), DateTime] private long _expiredDate;
    [SerializeField] private string _deviceUniqueIdentifier;
    [SerializeField, ReadOnly] private string _hash;

    public int id { get => _id; }
    public bool purchased { get => _purchased; internal set => _purchased = value; }
    public long expiredDate { get => _expiredDate; internal set => _expiredDate = value; }
    public string deviceUniqueIdentifier { get => _deviceUniqueIdentifier; }
    public string hash { get => _hash; internal set => _hash = value; }
#if UNITY_EDITOR
    internal void SetDuid(string duid)
    {
        _deviceUniqueIdentifier = duid;
    }

    internal void SetExpiredDate(long time)
    {
        _expiredDate = time;
    }
#endif
}
[Serializable]
public struct EditionCredential
{
    [SerializeField, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(100, resizable: false), DateTime] private long _expiredDate;
    [SerializeField] private string _deviceUniqueIdentifier;
    [SerializeField, ReadOnly] private string _hash;

    public int id { get => _id; }
    public bool purchased { get => _purchased; internal set => _purchased = value; }
    public long expiredDate { get => _expiredDate; }
    public string deviceUniqueIdentifier { get => _deviceUniqueIdentifier; }
    public string hash { get => _hash; internal set => _hash = value; }
#if UNITY_EDITOR
    internal void SetDuid(string duid)
    {
        _deviceUniqueIdentifier = duid;
    }

    internal void SetExpiredDate(long time)
    {
        _expiredDate = time;
    }
#endif
}
[Serializable]
public struct ApplicationCredential
{
    [SerializeField] private bool _purchased;
    [SerializeField, DateTime] private long _expiredDate;
    [SerializeField] private string _deviceUniqueIdentifier;
    [SerializeField] private string _hash;
    public bool purchased { get => _purchased; }
    public long expiredDate { get => _expiredDate; }
    public string deviceUniqueIdentifier
    {
        get => _deviceUniqueIdentifier;
    }
    public string hash
    {
        get => _hash;
        internal set => _hash = value;
    }
#if UNITY_EDITOR
    internal void SetDuid(string duid)
    {
        _deviceUniqueIdentifier = duid;
    }

    internal void SetExpiredDate(long time)
    {
        _expiredDate = time;
    }
#endif
}
