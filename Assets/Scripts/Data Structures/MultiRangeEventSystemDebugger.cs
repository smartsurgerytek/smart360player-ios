using Sirenix.OdinInspector;
using UnityEngine;

public class MultiRangeEventSystemDebugger : MonoBehaviour
{
    private MultiRangeEventSystem _system;
    [SerializeField,Range(0,10)] private double _time;
    [SerializeField,TableList] private RangeModel[] _ranges;
    [ShowInInspector, ReadOnly] private int[] _intersects;

    [Button]
    private RangeEventPoint[] Sweep(double previous, double current)
    {
        return _system.GetSweepedEventPoints(previous, current);
    }

    private void OnValidate()
    {
        if (_ranges == null) return;
        for (int i = 0; i < _ranges.Length; i++)
        {
            _ranges[i].index = i;
        }
    }
    private void OnEnable()
    {
        _system = new MultiRangeEventSystem();
        _system.update += _system_update;
        _system.getCurrentValue += _system_getCurrentValue;
        _system.enter += _system_enter;
        _system.exit += _system_exit;
        _system.Initialize(_ranges);

    }

    private double _system_getCurrentValue()
    {
        return _time;
    }


    private void _system_enter(int index)
    {
        Debug.Log($"_system_enter: {index}");
    }

    private void _system_exit(int index)
    {
        Debug.Log($"_system_exit: {index}");
    }
    private void _system_update(int[] intersects)
    {
        _intersects = intersects;
    }

    private void OnDisable()
    {
        _system = null;
    }
    private void Update()
    {
        _system?.Update();
    }
}
