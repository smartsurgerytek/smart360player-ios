using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Manager", menuName = "Managers/Staff Manager")]
public class StaffManager : ScriptableObject, IArrayReader<Staff>
{
    [SerializeField, TableList] private Staff[] _data;

    public Staff[] data { get => _data.ToArray(); }

    int ICountProvider.count => data.Length;

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }

    Staff[] IReader<Staff[]>.Read()
    {
        return _data;
    }

    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].index = i;
        }
    }
}
