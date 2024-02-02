using Eason.Odin;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;



[Serializable]
public struct ModuleVerificationInfo
{
    public static byte[] GOLDEN_FINGER_HASH => new byte[10] {0,1,2,3,4,5,6,7,8,9 };

    [SerializeField] private int _id;
    [SerializeField, HideInInspector] private long _expiredDate;
    [SerializeField] private string _hash;
    [ShowInInspector] UDateTime _time
    {
        get => new DateTime(_expiredDate);
        set => _expiredDate = value.dateTime.Ticks;
    }
    private byte[] GetHash()
    {
        return GOLDEN_FINGER_HASH;
    }
}
[Serializable]
public struct EditionVerificationInfo
{
    public static byte[] GOLDEN_FINGER_HASH => new byte[10] { 9,8,7,6,5,4,3,2,1,0 };

    [SerializeField] private int _id;
    [SerializeField] private long _expiredDate;
    [SerializeField] private string _hash;
    private byte[] GetHash()
    {
        return GOLDEN_FINGER_HASH;
    }
}

[Serializable]
public struct VerificationInfo
{
    [SerializeField] private long _applicationExpiredDate;
    [SerializeField] private ModuleVerificationInfo[] _modules;
    [SerializeField] private EditionVerificationInfo[] _editions;


    [SerializeField] private string _hash;
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
    [NonSerialized, ShowInInspector, ReadOnly] ModuleVerificationInfo[] _modules;
    [NonSerialized, ShowInInspector, ReadOnly] EditionVerificationInfo[] _editions;
    //[Header("Debug")]
    [DateTime, SerializeField] private long _dateTime;


    public bool VerifyPolicyViolation()
    {
        return true;
    }
    public void Initialize(int editionCount, int moduleCount)
    {
        if(_initialized) { throw new Exception("This should not happen. Check the error immdiately!"); }
        
        LoadVerificationResult();
    }

    [Button, HideIf(nameof(folderNotExist))]
    private void SaveVerificationResult()
    {
        var json = JsonUtility.ToJson(_verificationInfo);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, _verificationFolderPath, _resultFileName), json);
    }
    [Button]
    private void LoadVerificationResult()
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