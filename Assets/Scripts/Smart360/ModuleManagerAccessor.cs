using System;
using UnityEngine;

[Serializable]
public class ModuleManagerAccessor : IAccessor<Module[]>
{
    [SerializeField] private ModuleManager _manager;
    Module[] IReader<Module[]>.Read()
    {

        return _manager.data;
    }

    void IWriter<Module[]>.Write(Module[] value)
    {
        _manager.data = value;
    }
}
