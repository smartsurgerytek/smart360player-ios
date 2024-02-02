using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Manager", menuName = "Managers/Staff Manager")]
public class StaffManager : ScriptableObject
{
    [SerializeField, TableList] private StaffModel[] _data;

    public StaffModel[] data { get => _data.ToArray(); }

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
