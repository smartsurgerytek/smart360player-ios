using Sirenix.OdinInspector;
using Sirenix.Serialization;

public interface IAccessor<T> : IWriter<T>, IReader<T>
{

}
public abstract class MonoAccessor<T> : SerializedMonoBehaviour, IAccessor<T>
{
    [OdinSerialize] protected abstract IAccessor<T> innerAccesor { get; }

    T IReader<T>.Read()
    {
        return innerAccesor.Read();
    }

    void IWriter<T>.Write(T value)
    {
        innerAccesor?.Write(value);
    }
}
