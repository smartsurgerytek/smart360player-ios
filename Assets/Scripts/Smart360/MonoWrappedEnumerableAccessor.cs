using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonoWrappedEnumerableAccessor<T> : MonoWrappedAccessor<IEnumerable<T>>, IEnumerableAccessor<T>
{
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return ((IEnumerable < T > )innerData).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)innerData).GetEnumerator();
    }
}

    [Serializable]
public class Range : IEnumerable<int>
{
    [SerializeField] private IAccessor<int> _start;
    [SerializeField] private IAccessor<int> _end;
    [SerializeField] private IAccessor<int> _step;
    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        var start = _start.Read();
        var end = _end.Read();
        var step = _step.Read();    
        for (int i = start; i < end; i+=step)
        {
            yield return i;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<int>)this).GetEnumerator();
    }
}