using UnityEngine;

public class EasonJsonFileAccessor<T> : IAccessor<T>
{
    [SerializeField] private string _relativePath;
    T IReader<T>.Read()
    {
        var reader = new EasonJsonFileReader<T>(_relativePath);
        return reader.Load();
    }

    void IWriter<T>.Write(T data)
    {
        var writer = new EasonJsonFileWriter<T>(_relativePath);
        writer.Wr(data);
    }

}
