using UnityEngine;

public class VerificationView : MonoBehaviour
{
    public enum Views
    {
        Warning,
        Purchase,
        Expired,
    }

    [Header("Components")]
    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private PopUpView _warningView;
    [SerializeField] private PopUpView _purchaseView;
    [SerializeField] private PopUpView _expiredView;

    [SerializeField] private bool _needToShowView;
    [SerializeField] private Views _viewToShow;
    public bool needToShowView { get => _needToShowView; internal set => _needToShowView = value; }
    public Views viewToShow { get => _viewToShow; internal set => _viewToShow = value; }

    private PopUpView[] popUpViews => new PopUpView[3]
    {
        _warningView,
        _purchaseView,
        _expiredView
    };

    public void Start()
    {
        _warningView.back.AddListener(Application.Quit);
    }
    public void Update()
    {
        if (!needToShowView) return;
        if ((int)viewToShow < 0 || (int)viewToShow > 2) return;
        ShowView(viewToShow);
        needToShowView = false;
    }
    public void ShowView(Views view)
    {
        var caller = _mainMenu;
        if (view == Views.Warning) caller = null;
        popUpViews[(int)view].Show(caller);
    }

}