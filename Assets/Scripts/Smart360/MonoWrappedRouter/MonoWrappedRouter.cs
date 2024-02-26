using Sirenix.OdinInspector;

public class MonoWrappedRouter<T> : MonoWrapper<IRouter<T>>, IRouter<T>
{
    [Button("Route")]
    void IController.Execute()
    {
        innerData.Execute();
    }

    T IReader<T>.Read()
    {
        return innerData.Read();
    }

    void IWriter<T>.Write(T value)
    {
        innerData.Write(value);
    }
}
