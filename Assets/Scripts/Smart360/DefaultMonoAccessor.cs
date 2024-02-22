using Sirenix.OdinInspector;
using UnityEngine;

public class DefaultMonoAccessor<T> : MonoAccessor<T>
{
    [SerializeField, HideInInspector] private DefaultAccessor<T> _accessor;
    [ShowInInspector] T value { get=>innerAccesor.Read(); set=>innerAccesor.Write(value); }

    protected override IAccessor<T> innerAccesor
    {
        get
        {
            if (_accessor == null) _accessor = new DefaultAccessor<T>();
            return _accessor;
        }
    }
}