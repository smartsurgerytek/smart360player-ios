using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditionButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private int _index;
    [SerializeField] private UnityEvent<int> _clickButton;
    [SerializeField] private bool _interactive;
    public UnityEvent<int> clickButton { get => _clickButton; }

    public void Initialize(int index, string title, bool interactable = true)
    {
        _index = index;
        _titleText.text = title;
        _button.interactable  = interactable;
        _button.onClick.AddListener(_button_onClick);
    }

    private void _button_onClick()
    {
        clickButton?.Invoke(_index);
    }
}
