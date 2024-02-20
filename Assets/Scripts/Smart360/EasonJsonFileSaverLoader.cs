using UnityEngine;

public class EasonJsonFileSaverLoader<T> : ISaverLoader<T>
{
    [SerializeField] private string _relativePath;
    T ILoader<T>.Load()
    {
        var loader = new EasonJsonFileLoader<T>(_relativePath);
        return loader.Load();
    }

    void ISaver<T>.Save(T data)
    {
        var saver = new EasonJsonFileSaver<T>(_relativePath);
        saver.Save(data);
    }

}
