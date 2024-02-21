using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Device Manager", menuName = "Managers/Device Manager")]
public class DeviceManager : ScriptableObject
{

    [SerializeField, TableList] private DeviceModel[] _data;
    public DeviceModel[] data { get => _data?.ToArray(); internal set => _data = value; }

    private void OnValidate()
    {
        if (_data == null) return;
        ValidateId();
    }
    private void ValidateId()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }
}
