using Sirenix.OdinInspector;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Edition Manager", menuName = "Managers/Edition Manager")]
public class EditionManager : ScriptableObject
{
    
    [SerializeField, TableList] private EditionModel[] _data;
    public EditionModel[] data { get => _data?.ToArray(); internal set => _data = value; }


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
public struct EditionContext : IEditionContext
{
    [SerializeField] private int[] _currentEditions;
    [SerializeField, TableList] private EditionModel[] _data;
    #region IEditionContext

    void IEditionContext.Initialize()
    {

    }

    int IEditionContext.GetCount(int index)
    {
        return _data.Count(o => o.module == index);
    }

    int[] IEditionContext.GetCurrentEditions()
    {
        return _currentEditions.ToArray();
    }

    string IEditionContext.GetName(int index)
    {
        return _data[index].name;
    }

    //private EditionModel[] GetEditions(int module)
    //{
    //    //var editionList = _data.Where(o => o.module == module).ToList();
    //    //editionList.Sort((first, second) => first.index.CompareTo(second.index));
    //    //var rt = new
    //}
    #endregion
}
