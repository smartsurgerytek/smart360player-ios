using Eason.Odin;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



[Serializable]
public struct ModuleVerificationInfo
{
    [SerializeField] private int _id;
    [SerializeField, DateTime] private long _expiredDate;
    [SerializeField,HideInInspector] private string _hash;

    public int id { get => _id; internal set => _id = value; }
    public long expiredDate { get => _expiredDate; }
    public string Hash { get => _hash; internal set => _hash = value; }
}
[Serializable]
public struct EditionVerificationInfo
{
    [SerializeField] private int _id;
    [SerializeField, DateTime] private long _expiredDate;
    [SerializeField, HideInInspector] private string _hash;

    public int id { get => _id; internal set => _id = value; }
    public long expiredDate { get => _expiredDate; }
    public string Hash { get => _hash; internal set => _hash = value; }
}

[Serializable]
public struct VerificationInfo
{
    [SerializeField, DateTime] private long _applicationExpiredDate;
    [SerializeField] private string _serialNumber;
    [SerializeField, TableList] private ModuleVerificationInfo[] _modules;
    [SerializeField, TableList] private EditionVerificationInfo[] _editions;
    [SerializeField] private string _hash;

    public long applicationExpiredDate { get => _applicationExpiredDate; }
    public string serialNumber { get => _serialNumber; }
    public ModuleVerificationInfo[] modules { get => _modules.ToArray(); }
    public EditionVerificationInfo[] editions { get => _editions.ToArray(); }
    public string hash { get => _hash; set => _hash = value; }
}

[Serializable]
public struct VerificationResult
{
    [SerializeField] private bool _policyViolated;
    [SerializeField] private bool _applicationExpired;
    [SerializeField] private bool _applicationUnpaid;
    [SerializeField] private bool[] _moduleExpired;
    [SerializeField] private bool[] _moduleUnpaid;
    [SerializeField] private bool[] _editionExpired;
    [SerializeField] private bool[] _editionUnpaid;

    [SerializeField] private string _hash;

}
public class VerificationSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string _verificationFolderPath = "Secret";
    [SerializeField] private string _resultFileName = "Result";

    [Header("States")]
    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;
    [SerializeField] private VerificationInfo _verificationInfo;



    public bool VerifyPolicyViolation()
    {
        return true;
    }
    public void Initialize(int editionCount, int moduleCount)
    {
        if(_initialized) { throw new Exception("This should not happen. Check the error immdiately!"); }
        
        LoadVerificationInfo();
    }
    [Button]
    private string HashVerificationInfo()
    {
        return HashVerificationInfo(_verificationInfo);
    }

    private string HashVerificationInfo(VerificationInfo info)
    {
        var numList = new Queue<long>();
        numList.Enqueue(info.applicationExpiredDate);
        numList.Enqueue(info.serialNumber.GetHashCode());
        for (int i = 0; i < info.modules.Length; i++)
        {
            numList.Enqueue(info.modules[i].id);
            numList.Enqueue(info.modules[i].expiredDate);
        }
        for (int i = 0; i < info.editions.Length; i++)
        {
            numList.Enqueue(info.editions[i].id);
            numList.Enqueue(info.editions[i].expiredDate);
        }
        long num = numList.Dequeue();
        while (numList.Any())
        {
            var power = numList.Dequeue();
            if (power == 0) continue;
            num = Int64Pow(num, power);
        }
        return num.ToString();
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

    [Button, HideIf(nameof(folderNotExist))]
    private void SaveVerificationInfo()
    {
        var json = JsonUtility.ToJson(_verificationInfo);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, _verificationFolderPath, _resultFileName), json);
    }
    [Button]
    private void LoadVerificationInfo()
    {
        var json = File.ReadAllText(Path.Combine(Application.persistentDataPath, _verificationFolderPath, _resultFileName));
        _verificationInfo = JsonUtility.FromJson<VerificationInfo>(json);
    }

    public bool Verify()
    {
        return true;
    }
    public bool VeritfyLicense()
    {
        return true;
    }
    public bool VerifyExpiration()
    {
        return true;
    }

    public bool folderNotExist
    {
        get
        {
            var path = Path.Combine(Application.persistentDataPath, _verificationFolderPath);
            return !Directory.Exists(path);
        }
    }
    public bool resultNotExist
    {
        get
        {
            var path = Path.Combine(Application.persistentDataPath, _verificationFolderPath, _resultFileName);
            return File.Exists(path);
        }
    }
    [InfoBox("The verification folder dosen't exist. check \"Verification Folder Path\" or create one.", InfoMessageType =InfoMessageType.Warning,VisibleIf = nameof(folderNotExist))]
    [Button, ShowIf(nameof(folderNotExist))]
    public void CreateMissingFolder()
    {
        var path = Path.Combine(Application.persistentDataPath, _verificationFolderPath);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    //private bool VerifyModuleExpiration(int module)
    //{
        
    //}
    //private bool VerifyEditionExpiration(int edition)
    //{

    //}
}
public class VerificationView : MonoBehaviour
{
    public enum Views
    {
        Warning,
        Purchase,
        Expired,
    }
    private PopUpView[] popUpViews => new PopUpView[3]
    {
        _warningView,
        _purchaseView,
        _expiredView
    };

    [Header("Components")]
    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private PopUpView _warningView;
    [SerializeField] private PopUpView _purchaseView;
    [SerializeField] private PopUpView _expiredView;


    public void Start()
    {
        _warningView.back.AddListener(Application.Quit);
    }

    public void ShowView(Views view)
    {
        var caller = _mainMenu;
        if (view == Views.Warning) caller = null;
        popUpViews[(int)view].Show(caller);
    }
}