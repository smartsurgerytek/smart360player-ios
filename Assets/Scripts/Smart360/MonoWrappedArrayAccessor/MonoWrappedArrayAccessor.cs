public class MonoWrappedArrayAccessor<T> : MonoWrapper<IArrayAccessor<T>>, IArrayAccessor<T>
{
    int ICountProvider.count => innerData.count;
    T[] IReader<T[]>.Read()
    {
        return innerData?.Read() ?? default;
    }

    void IWriter<T[]>.Write(T[] value)
    {
        innerData?.Write(value);
    }

    void IWriter.Write(object value)
    {
        ((IWriter<T[]>)this).Write(value);
    }
}
