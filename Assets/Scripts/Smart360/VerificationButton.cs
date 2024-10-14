using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VerificationButton : SerializedMonoBehaviour
{
    public enum VerificationState
    {
        Normal,
        Unpaid,
        Expired,
        Unenabled,
        Warning
    }

    [Header("Components")]
    [SerializeField] private Button<int> _button;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private CanvasGroup _unpaid;
    [SerializeField] private CanvasGroup _expired;
    [SerializeField] private CanvasGroup _warning;

    [Header("State")]
    [SerializeField, ReadOnly] private VerificationState _verificationState;

    [Header("Events")]
    [SerializeField] private UnityEvent<int> _click;

    public string title { get => _titleText.text; internal set => _titleText.text = value; }
    public VerificationState verificationState { get => _verificationState; internal set => _verificationState = value; }
    public UnityEvent<int> click { get => _click; }

    public void Initialize()
    {
        _button.button.interactable = verificationState != VerificationState.Unenabled;
        if (verificationState == VerificationState.Unpaid)
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

    private void SetCanvasGroup(CanvasGroup canvasGroup, bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
