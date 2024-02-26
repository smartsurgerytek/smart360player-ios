using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class MonoModulesFreeAccessor : MonoFreeAccessor<Module[]>
{

}
public abstract class MonoSpawner<T> : SerializedMonoBehaviour, ISpawner<T> where T : Object
{
    protected abstract ISpawner<T> innerSpwner { get; }
    T[] ISpawner<T>.Spawn(int count)
    {
        return innerSpwner.Spawn(count);
    }
}
public class FreeMonoSpwaner<T> : MonoSpawner<T> where T : Object
{
    [OdinSerialize] ISpawner<T> _innerSpwaner;
    protected override ISpawner<T> innerSpwner => _innerSpwaner;
}
