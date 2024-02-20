using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public struct EasonCredentialContext : ICredentialContext
{
    [SerializeField, InfoBox("Folder doesn't exist.", "@!" + nameof(isRootExist), InfoMessageType = InfoMessageType.Error)] private string _rootFolderName;
    //[SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCredentialExist), InfoMessageType = InfoMessageType.Error)] private string _credentialFileName;
    [SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCookieExist), InfoMessageType = InfoMessageType.Error)] private string _cookieFileName;
    [OdinSerialize] private EasonCredentialSaveLoadParameter _parameter;

    [ShowInInspector] private Credential _credential;
    [ShowInInspector] private CredentialCookie _cookie;

    [Header("Controllers")]
    [SerializeField] private ILoader<Credential,EasonCredentialSaveLoadParameter> _loader;
    [SerializeField] private ISaver<Credential, EasonCredentialSaveLoadParameter> _saver;
    [SerializeField] private ICredentialHasher<ApplicationCredential> _applicationHasher;
    [SerializeField] private ICredentialHasher<EditionCredential> _editionHasher;
    [SerializeField] private ICredentialHasher<ModuleCredential> _moduleHasher;




    private bool _initialized;

    [ShowInInspector, FoldoutGroup("Debug")] private string rootPath => Path.Combine(Application.persistentDataPath, _rootFolderName ?? "").Replace('/', Path.PathSeparator);
    //[ShowInInspector, FoldoutGroup("Debug")] private string credentialPath => Path.Combine(rootPath ?? "", _credentialFileName?? "");
    [ShowInInspector, FoldoutGroup("Debug")] private string cookiePath => Path.Combine(rootPath ?? "", _cookieFileName?? "");
    [ShowInInspector, FoldoutGroup("Debug")] private bool isRootFolderNameValid => !string.IsNullOrEmpty(_rootFolderName);
    //[ShowInInspector, FoldoutGroup("Debug")] private bool isCredentialFileNameValid => !string.IsNullOrEmpty(_credentialFileName);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isCookieFileNameValid => !string.IsNullOrEmpty(_cookieFileName);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isRootExist => isRootFolderNameValid && Directory.Exists(rootPath);
    //[ShowInInspector, FoldoutGroup("Debug")] private bool isCredentialExist => isCredentialFileNameValid && File.Exists(credentialPath);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isCookieExist => isCookieFileNameValid && File.Exists(cookiePath);

    Credential ICredentialContext.credential
    {
        get {
            //if (!_initialized) throw new Exception("");
            return this._credential;
        }

    }
    CredentialCookie ICredentialContext.cookie => this._cookie;


    void ICredentialContext.Initialize()
    {
        if (_initialized) throw new Exception("Cannot initialize twice.");
        EnsureRootFolderExist();
        LoadCredential();
        LoadCookie();
        _initialized = true;
    }
    [Button, FoldoutGroup("Debug/Credential"), EnableIf("@" + nameof(_parameter) + "."+ "isCredentialExist")]
    private void LoadCredential()
    {
        //AssertCredentialFile();
        //var json = File.ReadAllText(credentialPath);
        //this._credential = JsonUtility.FromJson<Credential>(json);
        this._credential = _loader.Load(_parameter);
    }

    [Button, FoldoutGroup("Debug/Cookie"), EnableIf(nameof(isCookieExist))]
    private void LoadCookie()
    {
        AssertCookieFile();
        var json = File.ReadAllText(cookiePath);
        this._cookie = JsonUtility.FromJson<CredentialCookie>(json);
    }


    [Button, FoldoutGroup("Debug"), HideIf(nameof(isRootExist))]
    private void EnsureRootFolderExist()
    {
        if (!isRootExist) Directory.CreateDirectory(rootPath);
    }
    private void AssertRootFolderName()
    {
        if (!isRootFolderNameValid) throw new Exception("Root folder name cannot be null or empty.");
    }
    //private void AssertCredentialFileName()
    //{
    //    //if (!isCredentialFileNameValid) throw new Exception("Credential file name cannot be null or empty.");
    //}
    private void AssertCookieFileName()
    {
        if (!isCookieFileNameValid) throw new Exception("Cookie file name cannot be null or empty.");
    }
    private void AssertRootFolder()
    {
        AssertRootFolderName();
        if (!isRootExist) throw new DirectoryNotFoundException($"Root folder \"{rootPath}\" doesn't exist");
    }
    //private void AssertCredentialFile()
    //{
    //    AssertCredentialFileName();
    //    if (!isCredentialExist) throw new FileNotFoundException($"Root folder \"{credentialPath}\" doesn't exist");
    //}
    private void AssertCookieFile()
    {
        AssertCookieFileName();
        if (!isCookieExist) throw new FileNotFoundException($"Root folder \"{cookiePath}\" doesn't exist");
    }
#if UNITY_EDITOR

    [Button("Save Credential"), FoldoutGroup("Debug/Credential"), EnableIf("@" + nameof(_parameter) + "." + " isCredentialFileNameValid")]
    private void OdinSaveCredential()
    {
        OdinHashAll();
        _saver.Save(_credential, _parameter);
    }
    [Button("Save Credential Without Hashing"), FoldoutGroup("Debug/Credential"), EnableIf("@" + nameof(_parameter) + "." + " isCredentialFileNameValid")]
    private void OdinSaveCredentialWithoutHashing()
    {
        _saver.Save(_credential, _parameter);
    }
    [Button("Hash All"), FoldoutGroup("Debug/Credential")]
    private void OdinHashAll()
    {
        OdinHashApplication();
        OdinHashModules();
        OdinHashEditions();
    }

    [Button("Hash Application"), FoldoutGroup("Debug/Credential")]
    private void OdinHashApplication()
    {
        _credential.SetApplicationHash(_applicationHasher.Hash(_credential.application));
    }
    [Button("Hash Editions"), FoldoutGroup("Debug/Credential")]
    private void OdinHashEditions()
    {
        for (int i = 0; i < _credential.editions.Length; i++)
        {
            _credential.SetEditionHash(i, _editionHasher.Hash(_credential.editions[i]));
        }
    }
    [Button("Hash Modules"), FoldoutGroup("Debug/Credential")]
    private void OdinHashModules()
    {
        for (int i = 0; i < _credential.modules.Length; i++)
        {
            _credential.SetModuleHash(i, _moduleHasher.Hash(_credential.modules[i]));
        }
    }
    [Button("Load Credential"), FoldoutGroup("Debug/Cookie"), EnableIf(nameof(isCookieFileNameValid))]
    private void OdinSaveCookie()
    {
        AssertCookieFileName();
        var json = JsonUtility.ToJson(this._cookie);
        File.WriteAllText(cookiePath, json);
    }
    [Button("Open Root Folder"), FoldoutGroup("Debug"), ShowIf(nameof(isRootExist))]
    private void OdinOpen()
    {
        System.Diagnostics.Process.Start("explorer.exe", rootPath);
    }

#endif
    bool ICredentialContext.IsUnpaid(int edition)
    {
        return _credential.editions.First(o => o.id == edition).purchased;
    }

    bool ICredentialContext.isExpired(int edition)
    {
        return _credential.editions.First(o => o.id == edition).expiredDate > DateTime.Now.Ticks;
    }

}
