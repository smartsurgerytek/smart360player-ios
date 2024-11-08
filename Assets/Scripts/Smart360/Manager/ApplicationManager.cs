﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
//public class  ScriptableSaverLoader<T> : ScriptableObject, ISaverLoader<T>
//{
//    T data;

//    T ILoader<T>.Load()
//    {
//        return SaveLoadUtility.JsonDeepClone(data);
//    }

//    void ISaver<T>.Save(T data)
//    {
//        this.data = SaveLoadUtility.JsonDeepClone(data);
//    }
//}
[Serializable]
public class DefaultModuleModel : IModuleModel
{
    [SerializeField] private Module[] _data;
    Module[] IModuleModel.data { get => _data; set => _data = value; }
}
[Serializable]
public class DefaultVideoModel : IVideoModel
{

    [SerializeField] private Video[] _data;
    Video[] IVideoModel.data { get => _data; set => _data = value; }
}
[Serializable]
public class DefaultEditionReader : DefaultAccessor<Edition[]>
{
}

[Serializable]
public class DefaultApplicationModel : IApplicationModel
{
}
public class MonoMasterModel : MonoBehaviour, IMasterModel
{
    [OdinSerialize] private IApplicationModel _application;
    [OdinSerialize] private IModuleModel _module;
    [OdinSerialize] private IVideoModel _video;
    [OdinSerialize] private IAccessor<Edition[]> _edition;

    IApplicationModel IMasterModel.application => _application;
    IModuleModel IMasterModel.module => _module;
    IVideoModel IMasterModel.video => _video;
    IAccessor<Edition[]> IMasterModel.edition => _edition;
}
public class DefaultMasterModel : IMasterModel
{
    [OdinSerialize] private IApplicationModel _application;
    [OdinSerialize] private IModuleModel _module;
    [OdinSerialize] private IVideoModel _video;
    [OdinSerialize] private IAccessor<Edition[]> _edition;

    IApplicationModel IMasterModel.application => _application ;
    IModuleModel IMasterModel.module => _module ;
    IVideoModel IMasterModel.video => _video ;
    IAccessor<Edition[]> IMasterModel.edition => _edition;
}

[CreateAssetMenu(fileName = "Application Manager", menuName = "Managers/Application Manager")]
public class ApplicationManager : ScriptableObject
{
    [SerializeField] private ManifestManager _manifestManger;
    [SerializeField] private StaffGroupManager _staffGroupManager;
    [SerializeField] private StaffManager _staffManager;
    [SerializeField] private VideoManager _videoManager;
    [SerializeField] private EditionManager _editionManager;
    [SerializeField] private FileManager _fileManager;
    [SerializeField] private ModuleManager _moduleManager;
    [SerializeField] private ApplicationVerificationManager _applicationVerificationManager;
    [SerializeField] private ModuleVerificationManager _moduleVerificationManager;
    [SerializeField] private EditionVerificationManager _editionVerificationManager;
    [SerializeField] private UserManager _userManager;
    [SerializeField] private DeviceManager _deviceManager;
    public StaffGroupManager staffGroupManager { get => _staffGroupManager; }
    public StaffManager staffManager { get => _staffManager; }
    public VideoManager videoManager { get => _videoManager; }
    public EditionManager editionManager { get => _editionManager; }
    public FileManager fileManager { get => _fileManager; }
    public ModuleManager moduleManager { get => _moduleManager; }
    public ApplicationVerificationManager applicationVerificationManager {get=>_applicationVerificationManager;}
    public ModuleVerificationManager moduleVerificationManager {get=>_moduleVerificationManager;}
    public EditionVerificationManager editionVerificationManager {get=>_editionVerificationManager;}
    public UserManager userManager {get=>_userManager;}
    public DeviceManager deviceManager { get => _deviceManager; }



    [Button]
    private void AutoRefreshFilePath()
    {
        var staffs = staffManager.data;
        var videos = videoManager.data;
        var editions = editionManager.data;
        var surroundings = videoManager.surroundingData;
        for (int i = 0; i < videos.Length; i++)
        {
            var editionIndex = videos[i].edition.ToString("D2");
            var editionName = editions[videos[i].edition].name;
            var staffName = staffs[videos[i].staff].acronym;
            videos[i].fileName = $"{editionIndex}_{editionName}_{staffName}.mp4";
        }
        videoManager.data = videos;


        for (int i = 0;i < editions.Length; i++)
        {
            var editionIndex = i.ToString("D2");
            var editionName = editions[i].name;
            surroundings[i].fileName = $"{editionIndex}_{editionName}_Env360.mp4";
        }
        videoManager.surroundingData = surroundings;
    }
    //[Button]
    //private string[] GetFilePathsByEdition(int edition)
    //{
    //    var rt = videoManager.GetFilePathsByEdition(edition).ToList();
    //    rt.Add(editionManager.data[edition].surroundingVideoUrl);
    //    return rt.ToArray() ;
    //}
    //[Button]
    //private string[] CheckFileExist(int edition)
    //{
    //    var paths = videoManager.GetFilePathsByEdition(edition);
    //    return paths.Where(path => File.Exists(path)).ToArray();
    //}
    
}
