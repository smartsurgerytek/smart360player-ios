using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListElementButton : MonoBehaviour
{
    [SerializeField] private int _index;

    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UnityEvent<int> _onClick;

    public int index { get => _index; set => _index = value; }
    public bool interactable { get => _button.interactable; set => _button.interactable = value; }
    public Sprite icon { get => _image.sprite; set => _image.sprite = value; }
    public string title { get => _text.text; set => _text.text = value; }
    public UnityEvent<int> onClick { get => _onClick;}

    private void OnEnable()
    {
        _button.onClick.AddListener(_button_onClick);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(_button_onClick);
    }

    private void _button_onClick()
    {
        _onClick?.Invoke(index);
    }
}
