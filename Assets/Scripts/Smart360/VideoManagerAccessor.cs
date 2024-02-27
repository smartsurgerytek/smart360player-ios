using UnityEngine;

[SerializeField]
public class VideoManagerAccessor : IAccessor<Video[]>
{
    [SerializeField] private VideoManager _manager;
    Video[] IReader<Video[]>.Read()
    {
        return _manager.data;
    }

    void IWriter<Video[]>.Write(Video[] value)
    {
        _manager.data = value;
    }
    void IWriter.Write(object value)
    {
        ((IWriter<Video[]>)this).Write((Video[])value);
    }
}