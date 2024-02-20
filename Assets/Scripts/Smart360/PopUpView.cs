using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class PopUpView : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool _initialOpened;
    [Header("Components")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _backButton;

    [Header("Events")]
    [SerializeField] private UnityEvent _back;

    [Header("Debug")]
    //[NonSerialized, ShowInInspector, ReadOnly] private CanvasGroup _callerCanvasGroup;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _isOpened;

    public UnityEvent back { get => _back;}

    private void Reset()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        if (!_initialOpened) Hide(true);
    }
    public void Show() 
    {
        if (_isOpened) throw new Exception("This pop up view  has opened already.");
        _backButton.onClick.AddListener(_backButton_onClick);
        //_callerCanvasGroup = ;

        ShowCanvas(_canvasGroup, true);

        //if(_callerCanvasGroup) ShowCanvas(_callerCanvasGroup, false);
        _isOpened = true;
    }
    private void Hide(bool noCheck = false)
    {
        if (!noCheck && !_isOpened) throw new Exception("This pop up view is not opened.");
        ShowCanvas(_canvasGroup, false);
        //if (_callerCanvasGroup) ShowCanvas(_callerCanvasGroup, true);
        //_callerCanvasGroup = null;
        _isOpened = false;
    }
    private void _backButton_onClick()
    {
        _backButton.onClick.RemoveListener(_backButton_onClick);
        Hide();
        back.Invoke();
    }
    private void ShowCanvas(CanvasGroup canvasGroup, bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}