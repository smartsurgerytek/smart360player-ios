using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditionButton : MonoBehaviour
{
    public enum State
    {
        Normal,
        Unpaid,
        Expired,
        Unenabled
    }
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private int _index;
    [SerializeField] private UnityEvent<int> _clickButton;
    [SerializeField] private CanvasGroup _unpaid;
    [SerializeField] private CanvasGroup _expired;
    [SerializeField,ReadOnly] private State _state;
    public UnityEvent<int> clickButton { get => _clickButton; }

    public void Initialize(int index, string title, State state)
    {
        _state = state;
        _index = index;
        _titleText.text = title;
        _button.interactable = state != State.Unenabled;
        _button.onClick.AddListener(_button_onClick);
        if(state == State.Unpaid)
        {
            SetCanvasGroup(_unpaid, true);
        }
        if(state == State.Expired)
        {
            SetCanvasGroup(_expired, true);
        }
    }

    private void _button_onClick()
    {
        clickButton?.Invoke(_index);
    }
    private void SetCanvasGroup(CanvasGroup canvasGroup, bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
