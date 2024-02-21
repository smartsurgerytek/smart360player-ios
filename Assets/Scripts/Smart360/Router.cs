using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Router<T> :ISaverLoader<T>
{
    [OdinSerialize] ISaver<T> _saver;
    [OdinSerialize] ILoader<T> _loader;

    [Button] 
    private void Foward()
    {
        _saver.Save(_loader.Load());
    }

    T ILoader<T>.Load()
    {
        return _loader.Load();
    }

    void ISaver<T>.Save(T data)
    {
        _saver.Save(data);
    }
}
