using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Linq;
using UnityEngine;

public class DeviceManagerDuidAccessor : IAccessor<string>
{
    [SerializeField] private DeviceManager _manager;
    [SerializeField] private IAccessor<string> _serialNumber;
    string IReader<string>.Read()
    {
        return _manager.data.First(o=>o.serialNumber == _serialNumber.Read()).uniqueIdentifier;
    }

    void IWriter<string>.Write(string value)
    {
        var mode = _manager.data.First(o => o.serialNumber == _serialNumber.Read());
        var index = _manager.data.ToList().FindIndex(o=>o.serialNumber == _serialNumber.Read());
        _manager.SetDuid(_serialNumber.Read(), value);
    }
    void IWriter.Write(object value)
    {
        ((IWriter<string>)this).Write((string)value);
    }
}

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
    internal void SetDuid(string serialNumber, string duid)
    {
        var index = _data.ToList().FindIndex(o=>o.serialNumber == serialNumber);
        _data[index].uniqueIdentifier = duid;
    }



    private void ValidateId()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i].id = i;
        }
    }

}
public class KeyValueMapAccessor<TKey, TValue> : IAccessor<TValue>
{
    [OdinSerialize] private IAccessor<TKey> _key;
    [OdinSerialize] private IDictionaryAccessor<TKey, TValue> _dictionary;

    TValue IReader<TValue>.Read()
    {
        try
        {
            var key = _key.Read();
            var dictionary = _dictionary.Read();
            return dictionary[key];
        }
        catch
        {
            return default(TValue); 
        }
    }

    void IWriter<TValue>.Write(TValue value)
    {
        try
        {
            var key = _key.Read();
            var dictionary = _dictionary.Read();
            dictionary[key] = value;
        }
        catch
        {

        }
    }
    void IWriter.Write(object value)
    {
        ((IWriter<TValue>)this).Write((TValue)value);
    }
}
public class StringValueMapAccessor<TValue> : KeyValueMapAccessor<string, TValue>
{

}
