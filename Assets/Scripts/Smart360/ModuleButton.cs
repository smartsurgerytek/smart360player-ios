using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Button<int>))]
public class ModuleButton : SerializedMonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private int _index;

    [Header("Components")]
    [SerializeField] private Button<int> _button;
    [SerializeField] private TextMeshProUGUI _titleText;

    public string title { get => _titleText.text; internal set => _titleText.text = value; }
    public UnityEvent<int> click { get => _button.click; }
    public int moduleId { get => _button.value; set => _button.value = value; }
    private void Reset()
    {
        _button = GetComponent<Button<int>>();
    }
    public void Initialize()
    {
        _button?.Initialize();
    }
}
