using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
public class VideoPlayerSceneDummyContextInitializer : IController
{
    [OdinSerialize] private IArrayRouter<Edition> _loadEditions;
    private void CreateDummyContext()
    {
        _loadEditions.Execute();
    }
    void IController.Execute()
    {
        CreateDummyContext();
    }
}
public class MonoBehaviourEventTrigger : SerializedMonoBehaviour
{
    [OdinSerialize,TableList] private MonoBehaviourEventElement[] _events;
    private readonly Dictionary<MonoBehaviourEvent, List<IController>> _map = new Dictionary<MonoBehaviourEvent, List<IController>>();


    void Awake()
    {
        foreach (var e in _events)
        {
            if (!_map.ContainsKey(e.source))
            {
                _map[e.source] = new List<IController>();
            }
            _map[e.source].Add(e.target);
        }
    }
    void OnEnable()
    {
        ExecuteEvents(MonoBehaviourEvent.OnEnable);
    }
    void OnDisable()
    {
        ExecuteEvents(MonoBehaviourEvent.OnDisable);
    }   
    private void ExecuteEvents(MonoBehaviourEvent @event)
    {
        if (!_map.ContainsKey(@event)) return;
        foreach (var e in _map[@event])
        {
            e.Execute();
        }
    }
}
