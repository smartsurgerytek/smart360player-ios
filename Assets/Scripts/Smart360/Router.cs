using Sirenix.Serialization;
public interface IRouter<TRead, TWrite> : IAccessor<TRead, TWrite>, IController
{

}
public interface IRouter<T> : IRouter<T, T>
{

}
public interface IAccessor<TRead,TWrite> : IReader<TRead>, IWriter<TWrite>
{

}
public abstract class Router<TRead, TWrite> : IRouter<TRead, TWrite>
{
    [OdinSerialize] protected IReader<TRead> _reader;
    [OdinSerialize] protected IWriter<TWrite> _writer;
    public abstract TWrite Route(TRead value);
    void IController.Execute()
    {
        var value = Route(_reader.Read());
        _writer.Write(value);
    }

    TRead IReader<TRead>.Read()
    {
        return _reader.Read();
    }

    void IWriter<TWrite>.Write(TWrite value)
    {
        _writer.Write(value);
    }
}

public class Router<T> : Router<T,T> ,IRouter<T>
{

    public override T Route(T value)
    {
        return value;
    }
}
