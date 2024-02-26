using Sirenix.Serialization;
using System.Linq;

public class EditionsOfModuleReader : IReader<Edition[]>
{
    [OdinSerialize] private IReader<Edition[]> _editions;
    [OdinSerialize] private IReader<int> _moduleId;

    Edition[] IReader<Edition[]>.Read()
    {
        var editions = _editions.Read();
        var moduleId = _moduleId.Read();
        return editions.Where(o => o.module == moduleId).ToArray();
    }
}
