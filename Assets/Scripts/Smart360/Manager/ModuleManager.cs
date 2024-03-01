using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Module Manager", menuName = "Managers/Module Manager")]
public class ModuleManager : ScriptableObject, IArrayReader<Module>
{

    [SerializeField, TableList] private Module[] _data;
    public Module[] data { get => _data?.ToArray(); internal set => _data = value; }

    int ICountProvider.count => _data.Length;

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }

    Module[] IReader<Module[]>.Read()
    {
        return _data?.ToArray();
    }

    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].index = i;
        }
    }

}
