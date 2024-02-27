public class MonoWrappedAccessor<T> : MonoWrapper<IAccessor<T>>, IAccessor<T>
{
    T IReader<T>.Read()
    {
        return innerData.Read();
    }

    void IWriter<T>.Write(T value)
    {
        innerData.Write(value);
    }
    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write((T)value);
    }
}
