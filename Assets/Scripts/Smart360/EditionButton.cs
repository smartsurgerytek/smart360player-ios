using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditionButton : MonoBehaviour
{
    public enum VerificationState
    {
        Normal,
        Unpaid,
        Expired,
        Unenabled,
        Warning
    }
    
    [Header("Profile")]
    [SerializeField] private int _index;
    [SerializeField] private int _editionId;

    [Header("Components")]
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private CanvasGroup _unpaid;
    [SerializeField] private CanvasGroup _expired;
    [SerializeField] private CanvasGroup _warning;

    [Header("State")]
    [SerializeField,ReadOnly] private VerificationState _verificationState;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> _clickButton;

    public int index { get => _index; internal set => _index = value; }
    public int editionId { get => _editionId; internal set => _editionId = value; }
    public string title { get => _titleText.text; internal set => _titleText.text = value; }
    public VerificationState verificationState { get => _verificationState; internal set => _verificationState = value; }
    public UnityEvent<int> clickButton { get => _clickButton; }

    public void Initialize()
    {
        _button.interactable = verificationState != VerificationState.Unenabled;
        _button.onClick.AddListener(_button_onClick);
        if(verificationState == VerificationState.Unpaid)
        {
            SetCanvasGroup(_unpaid, true);
        }
        if (verificationState == VerificationState.Expired)
        {
            SetCanvasGroup(_expired, true);
        }
        if (verificationState == VerificationState.Warning)
        {
            SetCanvasGroup(_warning, true);
        }
    }

    private void _button_onClick()
    {
        clickButton?.Invoke(_editionId);
    }
    private void SetCanvasGroup(CanvasGroup canvasGroup, bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
