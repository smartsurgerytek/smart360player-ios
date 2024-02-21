using System.Linq;
using UnityEngine;
public class EditionManagerAccessor : IAccessor<Edition[]>
{
    [SerializeField] private EditionManager _manager;

    void IWriter<Edition[]>.Write(Edition[] value)
    {
        _manager.data = value.ToArray();
    }

    Edition[] IReader<Edition[]>.Read()
    {
         return _manager.data.ToArray();
    }
}
