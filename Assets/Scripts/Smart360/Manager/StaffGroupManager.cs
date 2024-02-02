using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Group Manager", menuName = "Managers/Staff Group Manager")]
public class StaffGroupManager : ScriptableObject
{
    [SerializeField,TableList] private StaffGroupModel[] _data;

    public StaffGroupModel[] data { get => _data.ToArray(); }

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
