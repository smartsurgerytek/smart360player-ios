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
public class EditionManager : ScriptableObject, IArrayReader<Edition>
{
    [SerializeField, TableList] private Edition[] _data;
    public Edition[] data { get => _data; internal set => _data = value; }

    int ICountProvider.count => _data.Length;

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

    Edition[] IReader<Edition[]>.Read()
    {
        return _data?.ToArray();
    }

    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }

}

[Serializable]
public struct ModuleContext : IModuleContext
{
    [SerializeField, TableList] private Edition[] _data;
    public int GetCount(int module)
    {
        return _data.Length;
    }
}
public struct EditionContext : IEditionContext
{
    [SerializeField, TableList] private Edition[] _data;

    public Edition[] data { get => _data; internal set => _data = value; }

    public void Initialize()
    {
    }

    #region IEditionContext

    int IEditionContext.GetCount(int index)
    {
        return data.Count(o => o.module == index);
    }
    string IEditionContext.GetName(int index)
    {
        return data[index].name;
    }


    //private EditionModel[] GetEditions(int module)
    //{
    //    //var editionList = _data.Where(o => o.module == module).ToList();
    //    //editionList.Sort((first, second) => first.index.CompareTo(second.index));
    //    //var rt = new
    //}
    #endregion
}
