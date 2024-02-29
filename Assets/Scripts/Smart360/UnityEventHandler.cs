using UnityEngine;
using UnityEngine.Events;

public class UnityEventHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _awake;
    [SerializeField] private UnityEvent _update;
    [SerializeField] private UnityEvent _lateUpdate;
    [SerializeField] private UnityEvent _start;
    [SerializeField] private UnityEvent _onDestroy;
    [SerializeField] private UnityEvent _onEnable;
    [SerializeField] private UnityEvent _onDisable;

    private void Awake()
    {
        _awake?.Invoke();
    }
    private void Update()
    {
        _update?.Invoke();
    }
    private void LateUpdate()
    {
        _lateUpdate?.Invoke();
    }
    private void Start()
    {
        _start?.Invoke();
    }
    private void OnDestroy()
    {
        _onDestroy?.Invoke();
    }
    private void OnEnable()
    {
        _onEnable?.Invoke();
    }
    private void OnDisable()
    {
        _onDisable?.Invoke();
    }
}

