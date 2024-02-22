using Sirenix.Serialization;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class DefaultSpawner<T> : ISpawner<T> where T:Object
{
    [SerializeField] T _prefab;
    [SerializeField] Transform _parent;
    [OdinSerialize] ISpawnInitializer[] _initializers;

    public DefaultSpawner(T prefab, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
    }

    public T[] Spawn(int count)
    {
        var rt = new T[count];
        for (int i = 0;i< count; i++) 
        {
            rt[i]= Object.Instantiate(_prefab, _parent);
        }
        for (int j = 0; j < _initializers.Length; j++)
        {
            for (int i = 0; i < count; i++)
            {
                try
                {
                    //_initializers[j].Initialize(rt[i], i);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
        return rt;
    }
}
