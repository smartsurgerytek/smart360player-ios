using UnityEngine;
using UnityEngine.Events;

public class UnityEventController : IController
{
    [SerializeField] private UnityEvent _event;
    void IController.Execute()
    {
        _event?.Invoke();
    }
}

