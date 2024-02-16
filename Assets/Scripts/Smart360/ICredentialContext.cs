﻿using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public interface ICredentialContext
{
    void Initialize();
    bool IsUnpaid(int edition);
    bool isExpired(int edition);

    Credential credential { get; }
    CredentialCookie cookie { get; }


}
public class EasonApplicationCredentialHasher : ICredentialHasher<ApplicationCredential>
{
    string ICredentialHasher<ApplicationCredential>.Hash(ApplicationCredential credential)
    {

        var numList = new Queue<long>();
        numList.Enqueue(credential.deviceUniqueIdentifier.GetHashCode());
        if (credential.purchased)
        {
            EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        }
        numList.Enqueue(credential.expiredDate);
        long num = numList.Dequeue();
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
    }
    private void EnqueneBySalt(Queue<long> queue, long value, long salt = 100)
    {
        queue.Enqueue(value + salt);
    }
    Int64 Int64Pow(Int64 x, Int64 pow)
    {
        Int64 ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }

}
public class EasonEditionCredentialHasher : ICredentialHasher<EditionCredential>
{
    string ICredentialHasher<EditionCredential>.Hash(EditionCredential credential)
    {
        var numList = new Queue<long>();
        EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        if (credential.purchased)
        {
            EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        }
        EnqueneBySalt(numList, credential.id);
        EnqueneBySalt(numList, credential.expiredDate);

        long num = numList.Dequeue();
        Debug.Log(num);
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
    }
    private void EnqueneBySalt(Queue<long> queue, long value, long salt = 532)
    {
        queue.Enqueue(value + salt);
    }
    Int64 Int64Pow(Int64 x, Int64 pow)
    {
        Int64 ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }
}
public class EasonModuleCredentialHasher : ICredentialHasher<ModuleCredential>
{
    string ICredentialHasher<ModuleCredential>.Hash(ModuleCredential credential)
    {

        var numList = new Queue<long>();
        EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        if (credential.purchased)
        {
            EnqueneBySalt(numList, credential.deviceUniqueIdentifier.GetHashCode());
        }
        EnqueneBySalt(numList, credential.id);
        EnqueneBySalt(numList, credential.expiredDate);

        long num = numList.Dequeue();
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
    }
    private void EnqueneBySalt(Queue<long> queue, long value, long salt = 100)
    {
        queue.Enqueue(value + salt);
    }
    Int64 Int64Pow(Int64 x, Int64 pow)
    {
        Int64 ret = 1;
        while (pow != 0)
        {
            if ((pow & 1) == 1)
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }
}
[Serializable]
public struct EasonCredentialContext : ICredentialContext
{
    [SerializeField] private ILoader<Credential> _loader;
    [SerializeField, InfoBox("Folder doesn't exist.", "@!" + nameof(isRootExist), InfoMessageType = InfoMessageType.Error)] private string _rootFolderName;
    [SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCredentialExist), InfoMessageType = InfoMessageType.Error)] private string _credentialFileName;
    [SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCookieExist), InfoMessageType = InfoMessageType.Error)] private string _cookieFileName;
    [SerializeField] private ICredentialHasher<ApplicationCredential> _applicationHasher;
    [SerializeField] private ICredentialHasher<EditionCredential> _editionHasher;
    [SerializeField] private ICredentialHasher<ModuleCredential> _moduleHasher;

    [ShowInInspector] private Credential _credential;
    [ShowInInspector] private CredentialCookie _cookie;



    private bool _initialized;

    [ShowInInspector, FoldoutGroup("Debug")] private string rootPath => Path.Combine(Application.persistentDataPath, _rootFolderName ?? "").Replace('/', '\\');
    [ShowInInspector, FoldoutGroup("Debug")] private string credentialPath => Path.Combine(rootPath ?? "", _credentialFileName?? "");
    [ShowInInspector, FoldoutGroup("Debug")] private string cookiePath => Path.Combine(rootPath ?? "", _cookieFileName?? "");
    [ShowInInspector, FoldoutGroup("Debug")] private bool isRootFolderNameValid => !string.IsNullOrEmpty(_rootFolderName);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isCredentialFileNameValid => !string.IsNullOrEmpty(_credentialFileName);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isCookieFileNameValid => !string.IsNullOrEmpty(_cookieFileName);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isRootExist => isRootFolderNameValid && Directory.Exists(rootPath);
    [ShowInInspector, FoldoutGroup("Debug")] private bool isCredentialExist => isCredentialFileNameValid && File.Exists(credentialPath);
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
    [Button, FoldoutGroup("Debug/Credential"), EnableIf(nameof(isCredentialExist))]
    private void LoadCredential()
    {
        //AssertCredentialFile();
        //var json = File.ReadAllText(credentialPath);
        //this._credential = JsonUtility.FromJson<Credential>(json);
        this._credential = _loader.Load();
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
    private void AssertCredentialFileName()
    {
        if (!isCredentialFileNameValid) throw new Exception("Credential file name cannot be null or empty.");
    }
    private void AssertCookieFileName()
    {
        if (!isCookieFileNameValid) throw new Exception("Cookie file name cannot be null or empty.");
    }
    private void AssertRootFolder()
    {
        AssertRootFolderName();
        if (!isRootExist) throw new DirectoryNotFoundException($"Root folder \"{rootPath}\" doesn't exist");
    }
    private void AssertCredentialFile()
    {
        AssertCredentialFileName();
        if (!isCredentialExist) throw new FileNotFoundException($"Root folder \"{credentialPath}\" doesn't exist");
    }
    private void AssertCookieFile()
    {
        AssertCookieFileName();
        if (!isCookieExist) throw new FileNotFoundException($"Root folder \"{cookiePath}\" doesn't exist");
    }
#if UNITY_EDITOR

    [Button("Save Credential"), FoldoutGroup("Debug/Credential"), EnableIf(nameof(isCredentialFileNameValid))]
    private void OdinSaveCredential()
    {
        AssertCredentialFileName();
        OdinHashAll();
        var json = JsonUtility.ToJson(this._credential);
        File.WriteAllText(credentialPath, json);
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

    bool ICredentialContext.IsUnpaid(int edition)
    {
        return _credential.editions.First(o => o.id == edition).purchased;
    }

    bool ICredentialContext.isExpired(int edition)
    {
        return _credential.editions.First(o => o.id == edition).expiredDate > DateTime.Now.Ticks;
    }

#endif
}
