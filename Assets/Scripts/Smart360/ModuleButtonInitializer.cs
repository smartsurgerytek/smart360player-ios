using Sirenix.Serialization;
using System.Reflection;
using UnityEngine;

public class ModuleButtonInitializer : IController
{
    [OdinSerialize] private IReader<Module[]> _modules;
    [OdinSerialize] private IReader<ModuleButton> _instance;
    [OdinSerialize] private IReader<int> _index;
    [OdinSerialize] private IWriter<int> _moduleToLoad;
    [OdinSerialize] private IController _onClick;
    void IController.Execute()
    {
        var modules = _modules.Read();
        var instance = _instance.Read();
        var index = _index.Read();
        instance.moduleId = modules[index].index;
        instance.title = modules[index].displayName;
        instance.click.AddListener(instance_clickButton);
        instance.Initialize();
        Debug.Log($"instance.name: {instance.name}");
    }

    private void instance_clickButton(int moduleId)
    {
        Debug.Log($"moduleId: {moduleId}");
        _moduleToLoad.Write(moduleId);
        _onClick?.Execute();
    }
}
