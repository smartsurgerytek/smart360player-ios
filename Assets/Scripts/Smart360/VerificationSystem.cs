using Eason.Odin;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public interface ILoader<T>
{
    T Load();
}
public interface ISaver<T>
{
    void Save(T data);
}
public interface ILoader<TResult, TParameter>
{
    TResult Load(TParameter parameter);
}
public interface ISaver<TData, TParameter>
{
    void Save(TData data, TParameter parameter);
}
[Serializable]
public struct EasonCredentialSaveLoadParameter
{

    [SerializeField, InfoBox("Folder doesn't exist.", "@!" + nameof(isRootExist), InfoMessageType = InfoMessageType.Error)] private string _rootFolderName;
    [SerializeField, InfoBox("File doesn't exist.", "@!" + nameof(isCredentialExist), InfoMessageType = InfoMessageType.Error)] private string _credentialFileName;


    [ShowInInspector, FoldoutGroup("Debug")] public string rootPath => Path.Combine(Application.persistentDataPath, _rootFolderName ?? "").Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector, FoldoutGroup("Debug")] public string credentialPath => Path.Combine(rootPath ?? "", _credentialFileName ?? "");
    [ShowInInspector, FoldoutGroup("Debug")] public bool isRootFolderNameValid => !string.IsNullOrEmpty(_rootFolderName);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isCredentialFileNameValid => !string.IsNullOrEmpty(_credentialFileName);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isRootExist => isRootFolderNameValid && Directory.Exists(rootPath);
    [ShowInInspector, FoldoutGroup("Debug")] public bool isCredentialExist => isCredentialFileNameValid && File.Exists(credentialPath);

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
public struct EasonCredentialSaver : ISaver<Credential, EasonCredentialSaveLoadParameter>
{
    void ISaver<Credential, EasonCredentialSaveLoadParameter>.Save(Credential data, EasonCredentialSaveLoadParameter parameter)
    {
        parameter.AssertRootFolder();
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(parameter.credentialPath, json);
    }
}
[Serializable]
public struct EasonCredentialLoader : ILoader<Credential, EasonCredentialSaveLoadParameter>
{
    Credential ILoader<Credential, EasonCredentialSaveLoadParameter>.Load(EasonCredentialSaveLoadParameter parameter)
    {
        parameter.AssertCredentialFile();
        var json = File.ReadAllText(parameter.credentialPath);
        return JsonUtility.FromJson<Credential>(json);
    }
}
[Serializable]
public struct ModuleCredential
{
    [SerializeField, TableColumnWidth(40, resizable: false), ReadOnly] private int _id;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(100, resizable: false), DateTime] private long _expiredDate;
    [SerializeField] private string _deviceUniqueIdentifier;
    [SerializeField, ReadOnly] private string _hash;

    public int id { get => _id; }
    public bool purchased { get => _purchased; internal set => _purchased = value; }
    public long expiredDate { get => _expiredDate; internal set => _expiredDate = value; }
    public string deviceUniqueIdentifier { get => _deviceUniqueIdentifier; }
    public string hash { get => _hash; internal set => _hash = value; }
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
    public string deviceUniqueIdentifier { get => _deviceUniqueIdentifier; }
    public string hash
    {
        get => _hash;
        internal set => _hash = value;
    }
}
[Serializable]
public struct Credential
{

    [SerializeField] ApplicationCredential _application;
    [SerializeField, TableList] private ModuleCredential[] _modules;
    [SerializeField, TableList] private EditionCredential[] _editions;

    public ApplicationCredential application { get => _application; }
    public ModuleCredential[] modules { get => _modules.ToArray(); }
    public EditionCredential[] editions { get => _editions.ToArray(); }
    public bool purchased { get => application.purchased; }

    internal void SetApplicationHash(string hash)
    {
        _application.hash = hash;
    }
    internal void SetEditionHash(int id, string hash)
    {
        _editions[id].hash = hash;
    }
    internal void SetModuleHash(int id, string hash)
    {
        _modules[id].hash = hash;
    }
}

[Serializable]
public struct VerificationResult
{

    [SerializeField] bool _verified;
    [SerializeField] private bool _deviceInvalid;
    [SerializeField] private bool _lastTimeLoginInvalid;
    [SerializeField] private bool _applicationHashInvalid;
    [SerializeField] private bool _applicationExpired;
    [SerializeField] private bool _applicationUnpaid;
    [SerializeField] private bool[] _moduleHashInvalid;
    [SerializeField] private bool[] _moduleUnpaid;
    [SerializeField] private bool[] _moduleExpired;
    [SerializeField] private bool[] _editionHashInvalid;
    [SerializeField] private bool[] _editionUnpaid;
    [SerializeField] private bool[] _editionExpired;

    public VerificationResult(int moduleCount, int editionCount) : this()
    {
        _moduleHashInvalid = new bool[moduleCount];
        _moduleUnpaid = new bool[moduleCount];
        _moduleExpired = new bool[moduleCount];
        _editionHashInvalid = new bool[editionCount];
        _editionUnpaid = new bool[editionCount];
        _editionExpired = new bool[editionCount];
    }

    public bool applicationHashInvalid { get => _applicationHashInvalid; internal set => _applicationHashInvalid = value; }
    public bool applicationUnpaid { get => _applicationUnpaid; internal set => _applicationUnpaid = value; }
    public bool applicationExpired { get => _applicationExpired; internal set => _applicationExpired = value; }
    public bool lastTimeLoginInvalid { get => _lastTimeLoginInvalid; internal set => _lastTimeLoginInvalid = value; }
    public bool deviceInvalid { get => _deviceInvalid; internal set => _deviceInvalid = value; }
    public bool applicationInvalid
{
    get
    {
        return !applicationHashInvalid || !applicationUnpaid || !applicationExpired || !lastTimeLoginInvalid || !deviceInvalid;
    }
}
    public bool[] moduleUnpaid { get => _moduleUnpaid; }
    public bool[] moduleExpired { get => _moduleExpired; }
    public bool[] editionUnpaid { get => _editionUnpaid; }
    public bool[] editionExpired { get => _editionExpired; }
    public bool[] moduleHashInvalid { get => _moduleHashInvalid; }
    public bool[] editionHashInvalid { get => _editionHashInvalid; }

    public bool verified { get => _verified; internal set => _verified = value; }
    public bool isValid
    {
        get
        {
            return !applicationHashInvalid && !applicationUnpaid & !applicationExpired & !lastTimeLoginInvalid & !deviceInvalid &
                moduleUnpaid.All(o => !o) &&
                moduleExpired.All(o => !o) &&
                editionUnpaid.All(o => !o) &&
                editionExpired.All(o => !o) &&
                moduleHashInvalid.All(o => !o) &&
                editionHashInvalid.All(o => !o) &&
                verified;
        }
    }
}
[SerializeField]
public struct CredentialCookie
{
    [SerializeField, DateTime] private long _lastTimeLogin;
    public long lastTimeLogin { get => _lastTimeLogin; }
    [Button]
    public void UpdateCookie()
    {
        _lastTimeLogin = DateTime.Now.Ticks;
    }
}
public class VerificationSystem : SerializedMonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private string _credentialFolderPath = "secret";
    [SerializeField] private string _credentialFileName = "result";
    [SerializeField] private string _credentialCookieFileName = "cookie";

    [Header("Credentials")]
    [SerializeField] private Credential _credential;
    [SerializeField] private CredentialCookie _cookie;

    [Header("States")]
    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _needShowVerificationView;
    [NonSerialized, ShowInInspector, ReadOnly] private VerificationView.Views _verificationViewToShow;

    [Header("Components")]
    [SerializeField] private VerificationView _verificationView;

    [Header("Controllers")]
    [SerializeField] private ICredentialVerifier _credentialVerifier;

    #region Debug
    [Header("Debug")]
    [SerializeField, ReadOnly] private VerificationResult _verificationResult;

    [Button("Hash Modules")]
    private string[] HashModuleVerificationInfoButtonClick(bool apply = true)
    {
        var rt = new string[_credential.modules.Length];
        for (int i = 0; i < rt.Length; i++)
        {
            rt[i] = HashModuleVerificationInfo(_credential, _credential.modules[i]);
            if (apply) _credential.SetModuleHash(i, rt[i]);
        }
        return rt;
    }
    [Button("Hash Editions")]
    private string[] HashEditionVerificationInfoButtonClick(bool apply = true)
    {
        var rt = new string[_credential.editions.Length];
        for (int i = 0; i < rt.Length; i++)
        {
            //rt[i] = HashEditionVerificationInfo(_credential, _credential.editions[i]);
            if (apply) _credential.SetEditionHash(i, rt[i]);
        }
        return rt;
    }
    #endregion
    //public ApplicationManager applicationManager { get => _applicationManager; internal set => _applicationManager = value; }
    //public bool needShowVerificationView { get => _needShowVerificationView; internal set => _needShowVerificationView = value; }
    //public VerificationView.Views verificationViewToShow { get => _verificationViewToShow; internal set => _verificationViewToShow = value; }
    //public VerificationView verificationView { get => _verificationView; internal set => _verificationView = value; }
    internal void InternalUpdate()
    {
        //CheckVerificationSystem();
    }

    //private void CheckVerificationSystem()
    //{
    //    if (needShowVerificationView)
    //    {
    //        verificationView.ShowView(verificationViewToShow);
    //        needShowVerificationView = false;
    //    }
    //}
    public bool VerifyPolicyViolation()
    {
        return true;
    }
    public void Initialize()
    {
        if (_initialized) { throw new Exception("This should not happen. Check the error immdiately!"); }
        _verificationResult = _credentialVerifier.Verify(_credential);
        _initialized = true;
    }


    private Credential LoadCredential()
    {
        var json = File.ReadAllText(Path.Combine(Application.persistentDataPath, _credentialFolderPath, _credentialFileName));
        var verificationInfo = JsonUtility.FromJson<Credential>(json);
        return verificationInfo;
    }
    public VerificationResult Verify(Credential verificationInfo, CredentialCookie cookie)
    {
        //var applicationVerification = applicationManager.applicationVerificationManager.GetApplicationVerification(applicationId);
        //var moduleVerifications =applicationManager.moduleVerificationManager.GetModuleVerifications(applicationId);
        //var editionVerifications = applicationManager.editionVerificationManager.GetEditionVerifications(applicationId);
        var result = new VerificationResult(verificationInfo.modules.Length, verificationInfo.editions.Length);


        //if (!VerifyHash(verificationInfo))
        //{
        //    result.applicationHashInvalid = true;
        //}
        //if (!VerifyDevice(verificationInfo))
        //{
        //    result.deviceInvalid = true;
        //}
        //if (!VerifyExpiration(verificationInfo))
        //{
        //    result.applicationExpired = true;
        //}
        if (!VerifyLastTimeLogin(cookie))
        {
            result.lastTimeLoginInvalid = true;
        }
        if (!VerifyPurchased(verificationInfo))
        {
            result.applicationUnpaid = true;
        }
        for (int i = 0; i < verificationInfo.modules.Length; i++)
        {
            if (!VerifyModuleHash(verificationInfo, verificationInfo.modules[i]))
            {
                result.moduleHashInvalid[i] = true;
            }
            if (!VerifyModulePurchased(verificationInfo.modules[i]))
            {
                result.moduleUnpaid[i] = true;
            }
            if (!VerifyModuleExpiredDate(verificationInfo.modules[i]))
            {
                result.moduleExpired[i] = true;
            }
        }
        for (int i = 0; i < verificationInfo.editions.Length; i++)
        {
            //if (!VerifyEditionHash(verificationInfo, verificationInfo.editions[i]))
            //{
            //    result.editionHashInvalid[i] = true;
            //}
            if (!VerifyEditionPurchased(verificationInfo.editions[i]))
            {
                result.editionUnpaid[i] = true;
            }
            if (!VerifyEditionExpiredDate(verificationInfo.editions[i]))
            {
                result.editionExpired[i] = true;
            }
        }
        result.verified = true;
        return result;
    }


    internal bool IsUnpaid()
    {
        return _verificationResult.applicationUnpaid;
    }
    private string HashModuleVerificationInfo(Credential parentInfo, ModuleCredential info)
    {
        var numList = new Queue<long>();
        EnqueneBySalt(numList, info.deviceUniqueIdentifier.GetHashCode());
        if (parentInfo.purchased)
        {
            EnqueneBySalt(numList, info.deviceUniqueIdentifier.GetHashCode());
        }
        EnqueneBySalt(numList, info.id);
        EnqueneBySalt(numList, info.expiredDate);

        long num = numList.Dequeue();
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
    }

    #region Application Verifier
    //public bool VerifyHash(Credential credential)
    //{
    //    return credential.hash == HashVerificationInfo(credential);
    //}
    //    public bool VerifyDevice(Credential credential)
    //    {
    //#if UNITY_ANDROID
    //        return credential.deviceUniqueIdentifier == SystemInfo.deviceUniqueIdentifier;
    //#elif UNITY_EDITOR
    //        return true;
    //#endif
    //    }
    private bool VerifyLastTimeLogin(CredentialCookie cookie)
    {
        return DateTime.Now.Ticks > cookie.lastTimeLogin;
    }
    //private bool VerifyExpiration(Credential verificationInfo)
    //{
    //    return verificationInfo.applicationExpiredDate < DateTime.Now.Ticks;
    //}
    private bool VerifyPurchased(Credential verificationInfo)
    {
        return verificationInfo.purchased;
    }
    #endregion
    #region Module Verifier
    public bool VerifyModuleHash(Credential parentInfo, ModuleCredential info)
    {
        return info.hash == HashModuleVerificationInfo(parentInfo, info);
    }
    private bool VerifyModulePurchased(ModuleCredential moduleVerificationInfo)
    {
        return moduleVerificationInfo.purchased;
    }
    private bool VerifyModuleExpiredDate(ModuleCredential moduleVerificationInfo)
    {
        return DateTime.Now.Ticks < moduleVerificationInfo.expiredDate;
    }
    #endregion
    #region Edition Verifier
    //public bool VerifyEditionHash(Credential parentInfo, EditionCredential info)
    //{
    //    return info.hash == HashEditionVerificationInfo(parentInfo, info);
    //}
    private bool VerifyEditionPurchased(EditionCredential editionVerificationInfo)
    {
        return editionVerificationInfo.purchased;
    }
    private bool VerifyEditionExpiredDate(EditionCredential editionVerificationInfo)
    {
        return DateTime.Now.Ticks < editionVerificationInfo.expiredDate;
    }
    #endregion

    public bool folderNotExist
    {
        get
        {
            var path = Path.Combine(Application.persistentDataPath, _credentialFolderPath);
            return !Directory.Exists(path);
        }
    }
    public bool fileNotExist
    {
        get
        {
            var path = Path.Combine(Application.persistentDataPath, _credentialFolderPath, _credentialFileName);
            return !File.Exists(path);
        }
    }


    [InfoBox("The verification folder dosen't exist. check \"Verification Folder Path\" or create one.", InfoMessageType = InfoMessageType.Warning, VisibleIf = nameof(folderNotExist))]
    [Button, ShowIf(nameof(folderNotExist))]
    public void CreateMissingFolder()
    {
        var path = Path.Combine(Application.persistentDataPath, _credentialFolderPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    internal bool IsModuleUnpaid(int i)
    {
        return _verificationResult.moduleUnpaid[i];
    }
    internal bool IsModuleExpired(int i)
    {
        return _verificationResult.moduleExpired[i];
    }
    internal bool IsEditionUnpaid(int i)
    {
        return _verificationResult.editionUnpaid[i];
    }
    internal bool IsEditionExpired(int i)
    {
        return _verificationResult.editionExpired[i];
    }
    //internal void OnMainMenuSceneInitializing(MainMenuManager mainManuManager)
    //{
    //    //verificationView = mainManuManager.verificationView;
    //}
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
