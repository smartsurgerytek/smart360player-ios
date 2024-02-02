using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Module Manager", menuName = "Managers/Module Manager")]
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
            _data[i].index = i;
        }
    }
}
