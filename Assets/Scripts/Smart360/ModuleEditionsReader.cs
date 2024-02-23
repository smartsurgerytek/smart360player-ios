using System.Linq;
using UnityEngine;
public class ModuleEditionsAccessor : IAccessor<Edition[]>
{
    [SerializeField] private IAccessor<int> _module;
    [SerializeField] private IAccessor<Edition[]> _source;

    public ModuleEditionsAccessor(IAccessor<int> module)
    {
        this._module = module;
    }

    Edition[] IReader<Edition[]>.Read()
    {
        var source = _source.Read();
        var module = _module.Read();
        return source.Where(o => o.module == module).ToArray();
    }

    void IWriter<Edition[]>.Write(Edition[] value)
    {
        var module = _module.Read();
        if (value.Any(o => o.module != module)) throw new System.Exception("You cannot write a edition into a module doesn't belong to it.");
        var newValue = _source.Read().Where(o => o.module != module).Concat(value).ToArray(); 
        _source.Write(newValue);
    }
}