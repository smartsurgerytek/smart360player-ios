using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Group Manager", menuName = "Managers/Staff Group Manager")]
public class StaffGroupManager : ScriptableObject, IArrayReader<StaffGroup>
{
    [SerializeField,TableList] private StaffGroup[] _data;

    public StaffGroup[] data { get => _data.ToArray(); }

    int ICountProvider.count => data.Length;
    private void OnValidate()
    {
        if (_data == null) return;
        ValidateIndice();
    }

    StaffGroup[] IReader<StaffGroup[]>.Read()
    {
        return _data.ToArray();
    }

    private void ValidateIndice()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].index = i;
        }
    }
}
