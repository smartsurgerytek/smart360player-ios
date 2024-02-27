using Sirenix.Serialization;
using System.Linq;

public class VideosOfEditionReader : IArrayReader<Video>
{
    [OdinSerialize] private IReader<int> _editionId;
    [OdinSerialize] private IArrayReader<Video> _innerData;
    int ICountProvider.count => _innerData.count;

    Video[] IReader<Video[]>.Read()
    {
        var editionId = _editionId.Read();
        var innerData = _innerData.Read();
        return innerData.Where(o => o.edition == editionId).ToArray();
    }
}

