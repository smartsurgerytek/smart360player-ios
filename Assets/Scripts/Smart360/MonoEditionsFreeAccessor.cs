using Sirenix.Serialization;

public class MonoEditionsFreeAccessor : MonoFreeAccessor<Edition[]>
{
}


public abstract class MonoFreeAccessor<T> : MonoAccessor<T>
{
    [OdinSerialize] private IAccessor<T> _innerAccesor;
    protected override IAccessor<T> innerAccesor => _innerAccesor;
}
