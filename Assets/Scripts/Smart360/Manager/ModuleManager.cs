using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Module Manager", menuName = "Managers/Module Manager")]
public class ModuleManager : ScriptableObject
{

    [SerializeField, TableList] private ModuleModel[] _data;
    public ModuleModel[] data { get => _data?.ToArray(); internal set => _data = value; }


    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }
    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].index = i;
        }
    }

}
