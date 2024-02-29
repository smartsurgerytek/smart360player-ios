using Sirenix.Serialization;
using System;
using UnityEngine;
public class DuidReader : IReader<string>
{
    string IReader<string>.Read()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }
}
public class NowReader : IReader<long>
{
    long IReader<long>.Read()
    {
        return DateTime.Now.Ticks;
    }
}
public class NextYearReader : IReader<long>
{
    [OdinSerialize] private IReader<long> _current;
    long IReader<long>.Read()
    {
        var current = _current.Read();
        return new DateTime(current).AddYears(1).Ticks;
    }
}