using Sirenix.OdinInspector;

public class MonoWrappedArrayRouter<T> : MonoWrapper<IArrayRouter<T>>, IArrayRouter<T>
{
    int ICountProvider.count => ((IReader<T[]>)this).Read().Length;
    [Button("Execute")]
    void IController.Execute()
    {
        innerData.Execute();
    }

    T[] IReader<T[]>.Read()
    {
        return innerData.Read();
    }

    void IWriter<T[]>.Write(T[] value)
    {
        innerData.Write(value);
    }

    void IWriter.Write(object value)
    {
        innerData.Write(value);
    }
}
