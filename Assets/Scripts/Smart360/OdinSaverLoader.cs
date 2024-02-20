using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Xml.Linq;
using UnityEngine;

[Serializable]
public class OdinSaverLoader<T> : ISaverLoader<T>
{
    [SerializeField] private ISaver<T> _saver;
    [SerializeField] private ILoader<T> _loader;
    [OdinSerialize] private T _data;

    void ISaver<T>.Save(T data)
    {
        _saver.Save(data);
    }
    T ILoader<T>.Load()
    {
        return _loader.Load();
    }

    [Button("Save")]
    private void OdinSaveButtonClick()
    {
        _saver.Save(_data);
    }
    [Button("Load")]
    private void OdinLoadButtonClick()
    {
        _data = _loader.Load();
    }
}

public class EditionManagerSaverLoader : ISaverLoader<Edition[]>
{
    [SerializeField] private EditionManager _manager;
    ISaverLoader<Edition[]> _innerSaverLoader;


    void ISaver<Edition[]>.Save(Edition[] data)
    {
        _innerSaverLoader.Save(_manager.data);
    }

    Edition[] ILoader<Edition[]>.Load()
    {
        _manager.data = _innerSaverLoader.Load();
        return _manager.data;
    }
}