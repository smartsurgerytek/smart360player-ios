using Sirenix.Serialization;
using System.Linq;

public class IsEditorReader : IReader<bool>
{
    bool IReader<bool>.Read()
    {
#if UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
public class AndReader : IReader<bool>
{
    [OdinSerialize] private IReader<bool>[] _conditions;
    bool IReader<bool>.Read()
    {
        return _conditions.All(o => o.Read());
    }
}