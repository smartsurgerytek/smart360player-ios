using Sirenix.Serialization;
using System.Linq;
public class EditionsOfModuleReader : IArrayReader<Edition>
{
    [OdinSerialize] private IReader<Edition[]> _editions;
    [OdinSerialize] private IReader<int> _moduleId;

    int ICountProvider.count => _editions.Read().Count(o => o.module == _moduleId.Read());

    Edition[] IReader<Edition[]>.Read()
    {
        var editions = _editions.Read();
        var moduleId = _moduleId.Read();
        return editions.Where(o => o.module == moduleId).ToArray();
    }
}

