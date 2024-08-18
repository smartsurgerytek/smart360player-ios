using Eason.Odin;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Video;

public static class Constants
{
    public const string EditorAssemblyName = "Temp.Editor";
}

[Serializable]
public struct StaffGroup
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _name;
    [SerializeField] private string _icon;

    public int index { get => _index; internal set => _index = value; }
    public string name { get => _name; }
    public string icon { get => _icon; }
}
[Serializable]
public struct Staff
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _displayTitleName;
    [SerializeField] private string _displayButtonName;
    [SerializeField] private string _acronym;
    [SerializeField] private int _group;

    public int index { get => _index; internal set => _index = value; }
    public string displayTitleName { get => _displayTitleName; }
    public string displayButtonName { get => _displayButtonName; }
    public string acronym { get => _acronym; }
    public int group { get => _group; }
}
[Serializable]
public struct SurroundingVideo
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private int _edition;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private string _fileName;
    [SerializeField] private string _assetName;
    public int index { get => _index; internal set => _index = value; }
    public int edition { get => _edition; }
    public VideoClip clip { get => _clip; }
    public string fileName { get => _fileName; internal set => _fileName = value; }
    public string assetName { get => _assetName; }

}
[Serializable]
public struct Video
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private int _edition;
    [SerializeField] private int _staff;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private string _fileName;
    [SerializeField] private string _assetName;
    [SerializeField] private int _startTime;
    [SerializeField] private double _duration;
    public int index { get => _index; internal set => _index = value; }
    public int staff { get => _staff; }
    public int edition { get => _edition; }
    public VideoClip clip { get => _clip; }
    public string fileName { get => _fileName; internal set => _fileName = value; }
    public string assetName { get => _assetName; }
    public double startTime { get => _startTime; }
    public double duration { get => _duration; set => _duration = value; }
}
[Serializable]
public struct Module
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _name;
    [SerializeField] private string _displayName;
    public int index { get => _index; internal set => _index = value; }
    public string name { get => _name; set => _name = value; }
    public string displayName { get => _displayName; set => _displayName = value; }
}

[Serializable]
public struct Edition : IEdition
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _name;
    [SerializeField] private string _englishName;
    [SerializeField] private string _displayName;
    [SerializeField] private int _module;
    [SerializeField] private int _initialSelected;

    public int id { get => _index; internal set => _index = value; }
    public int module { get => _module; }
    public string name { get => _name; set => _name = value; }
    public string englishName { get => _englishName; set => _englishName = value; }
    public string displayName { get => _displayName; set => _displayName = value; }
    public int initialSelected { get => _initialSelected; }
}

[Serializable]
public struct BuildInVideoModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private VideoClip _clip; 
    public VideoClip clip { get => _clip; }
    public int index { get => _index; internal set => _index = value; }
}
[Serializable]
public struct DirectVideoModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _fileName;

    public int index { get => _index; internal set => _index = value; }
    public string fileName { get => _fileName; }
}
[Serializable]
public struct AssetBundleVideoModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _fileName;

    public int index { get => _index; internal set => _index = value; }
    public string fileName { get => _fileName; }
}

[Serializable]
public struct UserModel
{
    [SerializeField, ReadOnly, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField] private string _name;

    public int id { get => _id; internal set => _id = value; }
}
[Serializable]
public struct ApplicationVerificationModel
{
    [SerializeField, ReadOnly, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(70, resizable: false)] private int _userId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private int _deviceId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(90, resizable: false), DateTime] private long _expiredDate;
    public int id { get => _id; internal set => _id = value; }
}
[Serializable]
public struct ModuleVerificationModel
{
    [SerializeField, ReadOnly, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(110, resizable: false)] private int _applicationId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private int _moduleId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(90, resizable: false), DateTime] private long _expiredDate;
    public int id { get => _id; internal set => _id = value; }
    public int applicationId { get => applicationId; }
    public int moduleId { get => moduleId; }
    public bool purchased { get => purchased; }
    public long expiredDate { get => expiredDate; }
}

[Serializable]
public struct EditionVerificationModel
{
    [SerializeField, ReadOnly, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(110, resizable: false)] private int _applicationId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private int _editionId;
    [SerializeField, TableColumnWidth(70, resizable: false)] private bool _purchased;
    [SerializeField, TableColumnWidth(90, resizable: false), DateTime] private long _expiredDate;
    public int id { get => _id; internal set => _id = value; }
    public int applicationId { get => _applicationId; }
    public int editionId {get =>_editionId ;}
    public bool purchased {get =>_purchased ;}
    public long expiredDate {get =>_expiredDate ;}

}
[Serializable]
public struct DeviceModel
{
    [SerializeField, ReadOnly, TableColumnWidth(40, resizable: false)] private int _id;
    [SerializeField, TableColumnWidth(70, resizable: false)] private int _userId;
    [SerializeField, TableColumnWidth(150, resizable: false)] private string _serialNumber;
    [SerializeField] private string _uniqueIdentifier;
    public int id { get => _id; internal set => _id = value; }
    public int userId { get => _userId; internal set => _userId = value; }
    public string serialNumber { get => _serialNumber;internal set => _serialNumber = value; }
    public string uniqueIdentifier { get => _uniqueIdentifier;internal set => _uniqueIdentifier = value; }
}