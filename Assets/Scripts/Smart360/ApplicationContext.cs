using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
using UnityEngine;

interface ISaverLoader<T> : ISaver<T>, ILoader<T>
{

}
[Serializable]
public class OdinSaverLoader<T> : ISaverLoader<T>
{
    [SerializeField] private string _relativePath;
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
internal class ApplicationContext : MonoBehaviour, IApplicationContext
{
    [SerializeField] private int _currentModule;
    //[SerializeField] private int[] _currentEditions;
    [SerializeField] private EditionManager _editionManager;
    int IApplicationContext.currentModule { get => _currentModule; set => _currentModule = value; }
    int[] IApplicationContext.currentEditions { get => _editionManager.GetEditionsOfModule(_currentModule); set => throw new NotImplementedException(); }
}