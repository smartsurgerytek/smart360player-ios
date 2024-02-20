using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class DebugSaverLoader<T> : ISaverLoader<T>
{
    [OdinSerialize] private T _data;

    void ISaver<T>.Save(T data)
    {
        _data = SaveLoadUtility.JsonDeepClone(data);
    }
    T ILoader<T>.Load()
    {
        return SaveLoadUtility.JsonDeepClone(_data);
    }
}
public static class SaveLoadUtility
{
    public static T JsonDeepClone<T>(T obj)
    {
        return JsonUtility.FromJson<T>( JsonUtility.ToJson(obj));
    }
}