using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Video;

public static class Constants
{
    public const string EditorAssemblyName = "Temp.Editor";
}

[Serializable]
public struct StaffGroupModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;

    public int index { get => _index; internal set => _index = value; }
    public string name { get => _name; }
    public Sprite icon { get => _icon; }
}
[Serializable]
public struct StaffModel
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
public struct SurroundingVideoModel
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
public struct VideoModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private int _edition;
    [SerializeField] private int _staff;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private string _fileName;
    [SerializeField] private string _assetName;
    [SerializeField] private int _startTime;
    public int index { get => _index; internal set => _index = value; }
    public int staff { get => _staff; }
    public int edition { get => _edition; }
    public VideoClip clip { get => _clip; }
    public string fileName { get => _fileName; internal set => _fileName = value; }
    public string assetName { get => _assetName; }
    public double startTime { get => _startTime; }
}
[Serializable]
public struct EditionModel
{
    [SerializeField, ReadOnly] private int _index;
    [SerializeField] private string _name;
    [SerializeField] private string _englishName;
    [SerializeField] private string _displayName;
    [SerializeField] private int _initialSelected;

    public int index { get => _index; internal set => _index = value; }
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
