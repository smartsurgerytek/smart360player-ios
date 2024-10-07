using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoLengthRouter : SerializedMonoBehaviour 
{
    [OdinSerialize] private IArrayAccessor<Video> _source;
    [SerializeField, TableList] private Video[] _buffer;
    [SerializeField] private VideoPlayer _player;

    [Button]
    public void Load()
    {
        _buffer = _source.Read();
    }
    [Button]
    public void Save()
    {
        _source.Write(_buffer);
    }

    private void OnEnable()
    {
        Load();
        Prepare();
        _player.prepareCompleted += OnPrepareComplete;
    }
    bool next = false;
    int index = 0;
    private void OnPrepareComplete(VideoPlayer source)
    {
        _buffer[index].duration = source.length;
        if(index < _buffer.Length)
        {
            index++;
            next = true;
        }
    }

    private void Update()
    {
        if (next)
        {
            next = false;
            if (index >= _buffer.Length) return;
            Prepare();
        }
    }
    private void Prepare()
    {
        
        _player.Stop();
        _player.url = GetUrl(index);
        while (!File.Exists(_player.url) && index < _buffer.Length) 
        {
            _player.url = GetUrl(index++);
        }
        _player.Prepare();
    }
    private string GetUrl(int index)
    {
        return Path.Combine(Application.persistentDataPath, "Videos", _buffer[index].fileName).Replace('/', Path.DirectorySeparatorChar);
    }
}
