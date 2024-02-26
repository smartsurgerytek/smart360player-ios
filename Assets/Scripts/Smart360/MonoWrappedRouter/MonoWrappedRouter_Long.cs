using Eason.Odin;
using Sirenix.Serialization;
using System;
using UnityEngine;

[AddComponentMenu("Router/Router - Long")]
public class MonoWrappedRouter_Long : MonoWrappedRouter<long>
{

}

[Serializable]
public class DefaultDateTimeAccessor : IAccessor<long>
{
    [DateTime, SerializeField] private long _value;
    long IReader<long>.Read()
    {
        return _value;
    }

    void IWriter<long>.Write(long value)
    {
        _value = value;
    }
}