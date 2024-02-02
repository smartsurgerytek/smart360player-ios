using System;
using UnityEngine;

[Serializable]
public struct RangeModel
{
    [SerializeField] private int _index;
    [SerializeField] private double _min;
    [SerializeField] private double _max;

    public RangeModel(int index, double min, double max)
    {
        _index = index;
        _min = min;
        _max = max;
    }

    public int index { get { return _index; } internal set { _index = value; } }

    public double min => _min;

    public double max => _max;
}
