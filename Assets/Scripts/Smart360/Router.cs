using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Router<T> :IAccessor<T>
{
    [OdinSerialize] IWriter<T> _saver;
    [OdinSerialize] IReader<T> _loader;

    [Button] 
    private void Foward()
    {
        _saver.Save(_loader.Load());
    }

    T IReader<T>.Load()
    {
        return _loader.Load();
    }

    void IWriter<T>.Save(T data)
    {
        _saver.Save(data);
    }
}
