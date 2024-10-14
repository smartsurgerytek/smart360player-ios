using Sirenix.Serialization;
using UnityEngine;

public class Instantiator : IController
{
    [OdinSerialize] private IReader<Transform> _prefab;
    [OdinSerialize] private IReader<Transform> _parent;
    [OdinSerialize] private IWriter<Transform> _instance;
    void IController.Execute()
    {
        var prefab = _prefab.Read();
        var parent = _parent.Read();
        var instance = GameObject.Instantiate(prefab, parent);
        _instance.Write(instance);
    }
}
