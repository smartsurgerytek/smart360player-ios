using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Button<T> : MonoBehaviour
{
    [SerializeField] private T _value;
    [SerializeField] private Button _button;
    [SerializeField] private UnityEvent<T> _click;
    public UnityEvent<T> click { get => _click; }

    public T value { get => _value; set => _value = value; }
    public Button button { get => _button; set => _button = value; }

    private void Reset()
    {
        _button = GetComponent<Button>();
    }
    public void Initialize()
    {
        _button?.onClick?.AddListener(_button_onClick);
    }
    private void _button_onClick()
    {
        //Debug.Log($"_value: {_value}");
        _click?.Invoke(_value);
    }
}
