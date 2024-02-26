public class MonoWrappedAccessor<T> : MonoWrapper<IAccessor<T>>, IAccessor<T>
{
    public T Read()
    {
        return innerData.Read();
    }

    public void Write(T value)
    {
        innerData.Write(value);
    }
}
