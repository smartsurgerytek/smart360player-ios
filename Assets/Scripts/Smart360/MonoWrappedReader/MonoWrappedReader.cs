public class MonoWrappedReader<T> : MonoWrapper<IReader<T>>, IReader<T>
{
    T IReader<T>.Read()
    {
        return innerData.Read();
    }
}
