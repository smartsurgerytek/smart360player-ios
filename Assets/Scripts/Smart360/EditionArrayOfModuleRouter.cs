using System.Linq;
using UnityEngine;

public class EditionArrayOfModuleRouter : ArrayRouter<Edition>
{
    [SerializeField] private IReader<int> _moduleId;
    public override Edition[] Route(Edition[] value)
    {
        return base.Route(value).Where(o=>o.module == _moduleId.Read()).ToArray();
    }
}
