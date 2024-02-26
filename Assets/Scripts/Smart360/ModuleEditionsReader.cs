using System.Linq;
using UnityEngine;

public class ModuleEditionsAccessor : IAccessor<Edition[]>
{
    [SerializeField] private int _module;
    [SerializeField] private IAccessor<Edition[]> _source;

    public ModuleEditionsAccessor(int module)
    {
        this._module = module;
    }

    Edition[] IReader<Edition[]>.Read()
    {
        var source = _source.Read();
        return source.Where(o => o.module == _module).ToArray();
    }

    void IWriter<Edition[]>.Write(Edition[] value)
    {
        if (value.Any(o => o.module != _module)) throw new System.Exception("You cannot write a edition into a module doesn't belong to it.");
        var newValue = _source.Read().Where(o => o.module != _module).Concat(value).ToArray(); 
        _source.Write(newValue);
    }
}