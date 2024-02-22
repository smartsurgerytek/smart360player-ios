using UnityEngine;

public interface ISpawnInitializer<T> : ISpawnInitializer where T : Object 
{
    void Initialize(T instance, int index);
}
