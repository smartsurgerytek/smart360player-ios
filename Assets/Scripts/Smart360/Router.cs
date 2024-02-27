using Sirenix.Serialization;
using UnityEngine;
public interface IRouter<TRead, TWrite> : IAccessor<TRead, TWrite>, IController
{

}
public class ArrayRouter<T> : Router<T[]>, IArrayRouter<T>
{
    int ICountProvider.count => ((IReader<T[]>)this).Read().Length;
}
public interface IArrayRouter<T> : IRouter<T[]>, IArrayAccessor<T>
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

    void IWriter.Write(object value)
    {
        ((IWriter<TWrite>)this).Write((TWrite)value);
    }
}

public class Router<T> : Router<T,T> ,IRouter<T>
{

    public override T Route(T value)
    {
        return value;
    }
}
