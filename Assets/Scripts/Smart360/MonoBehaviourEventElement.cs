using UnityEngine;
using System;

[Serializable]
public struct MonoBehaviourEventElement 
{
    [SerializeField] private MonoBehaviourEvent _source;
    [SerializeField] private IController _target;

    public MonoBehaviourEvent source { get => _source; }
    public IController target { get => _target; }
}
