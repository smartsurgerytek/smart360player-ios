using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
[Serializable]
public struct RangeEventPoint : IComparable<RangeEventPoint>
{
     private int _rangeIndex;
     private double _value;
     private bool _isMin;
     private bool _isEntered;

    [ShowInInspector] public int rangeIndex => _rangeIndex;
    [ShowInInspector] public double value => _value;
    [ShowInInspector] public bool isMin => _isMin;
    [ShowInInspector] public bool isEntered => _isEntered;

    public RangeEventPoint(int rangeIndex, double value, bool isMin, bool isEntered)
    {
        _rangeIndex = rangeIndex;
        _value = value;
        _isMin = isMin;
        _isEntered = isEntered;
    }

    public int CompareTo(RangeEventPoint other)
    {
        return this.value.CompareTo(other.value);  
    }
}
public class MultiRangeEventSystem
{
    private RangeModel[] _ranges;
    private double _previousValue = double.MinValue;
    private double _currentValue = 0;
    private readonly List<int> intersects = new List<int>();

    public event Func<double> getCurrentValue;
    public event Action<int[]> update;
    public event Action<int> enter;
    public event Action<int> exit;

    public void Initialize(RangeModel[] rangeModels)
    {
        _ranges = rangeModels;
    }
    public void Update()
    {
        _currentValue = getCurrentValue();
        if (_previousValue == _currentValue) return;
        var sweepeds = GetSweepedEventPoints(_previousValue, _currentValue);
        InvokeEvents(sweepeds);
        _previousValue = _currentValue;
        update?.Invoke(intersects.ToArray());
    }
    public RangeEventPoint[] GetSweepedEventPoints(double previousValue, double currentValue)
    {
        var min = previousValue;
        var max = currentValue;
        if(min > max)
        {
            var buffer = min;
            min = max;
            max = buffer;
        }
        var eventPoints = new List<RangeEventPoint>();
        var movePositive = previousValue < currentValue;
        for (int i = 0; i < _ranges.Length; i++)
        {
            eventPoints.Add(new RangeEventPoint(i, _ranges[i].min, true, movePositive));
            eventPoints.Add(new RangeEventPoint(i, _ranges[i].max, false, !movePositive));
        }
        eventPoints.Sort();
        var result = new List<RangeEventPoint>();
        var index = 0;
        while (index < eventPoints.Count && eventPoints[index].value <= min)
        {
            index++;
        }
        while (index < eventPoints.Count && eventPoints[index].value <= max)
        {
            result.Add(eventPoints[index++]);
        }

        if (!movePositive) result.Reverse();
        return result.ToArray();
    }
    private void InvokeEvents(RangeEventPoint[] events)
    {
        for (int i = 0; i < events.Length; i++) 
        {
            InvokeEvent(events[i]);
        }
    }

    private void InvokeEvent(RangeEventPoint value)
    {
        if (value.isEntered)
        {
            intersects.Add(value.rangeIndex);
            enter?.Invoke(value.rangeIndex);
        }
        else
        {
            intersects.Remove(value.rangeIndex);
            exit?.Invoke(value.rangeIndex);
        }
    }
}