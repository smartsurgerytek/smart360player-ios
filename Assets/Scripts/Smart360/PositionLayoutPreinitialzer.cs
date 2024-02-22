using UnityEngine;

public class PositionLayoutPreinitialzer : ISpawnInitializer<Transform>
{
    [SerializeField] private ExactPositionLayout _layout;
    void ISpawnInitializer<Transform>.Initialize(Transform instance, int index)
    {
        _layout.Layout(index, instance);
    }
}