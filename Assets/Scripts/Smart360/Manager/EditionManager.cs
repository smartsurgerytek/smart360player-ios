using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct FilePath
{
    [SerializeField] private string _name;
    [SerializeField] private string _path;

    public string name { get => _name; }
    public string path { get => _path; }
}
public struct Manifest
{
    [SerializeField] private FilePath[] _files;
    internal string credentialPath { get => _files.First(o => o.name == nameof(credentialPath)).path; }
    internal string editionPath { get=> _files.First(o => o.name == nameof(credentialPath)).path; }
}

[CreateAssetMenu(fileName = "Edition Manager", menuName = "Managers/Edition Manager")]
public class EditionManager : ScriptableObject
{
    
    [SerializeField, TableList] private Edition[] _data;
    public Edition[] data { get => _data; internal set => _data = value; }

    internal int GetEditionIdByIndexInModule(int module,int index)
    {
        var editions = GetEditionsOfModule(module);
        return editions[index];
    }

    internal int[] GetEditionsOfModule(int module)
    {
        return data.Where(o => o.module == module).Select(o=>o.id).ToArray();
    }

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }
    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }
}


public struct EditionContext 
{
    [SerializeField] private int[] _currentEditions;
    [SerializeField, TableList] private Edition[] _data;
    #region IEditionContext

    //void IEditionContext.Initialize()
    //{
    //}

    //int IEditionContext.GetCount(int index)
    //{
    //    return _data.Count(o => o.module == index);
    //}

    //int[] IEditionContext.GetCurrentEditions()
    //{
    //    return _currentEditions.ToArray();
    //}

    //string IEditionContext.GetName(int index)
    //{
    //    return _data[index].name;
    //}


    //private EditionModel[] GetEditions(int module)
    //{
    //    //var editionList = _data.Where(o => o.module == module).ToList();
    //    //editionList.Sort((first, second) => first.index.CompareTo(second.index));
    //    //var rt = new
    //}
    #endregion
}
