using Sirenix.Serialization;

public class CountReader : IReader<int>, ICountProvider
{
    [OdinSerialize] private ICountProvider _source;

    int ICountProvider.count => _source.count;

    int IReader<int>.Read()
    {
        return _source.count;
    }
}