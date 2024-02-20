using UnityEngine;
using UnityEngine.Events;

public class VerificationView : MonoBehaviour
{
    public enum Views
    {
        Warning,
        Purchase,
        Expired,
    }

    [Header("Components")]
    [SerializeField] private PopUpView _warningView;
    [SerializeField] private PopUpView _purchaseView;
    [SerializeField] private PopUpView _expiredView;

    [SerializeField] private bool _needToShowView;
    [SerializeField] private Views _viewToShow;
    [SerializeField] private UnityEvent<Views> _show;
    [SerializeField] private UnityEvent<Views> _back;

    public bool needToShowView { get => _needToShowView; internal set => _needToShowView = value; }
    public Views viewToShow { get => _viewToShow; internal set => _viewToShow = value; }

    private PopUpView[] popUpViews => new PopUpView[3]
    {
        _warningView,
        _purchaseView,
        _expiredView
    };

    public UnityEvent<Views> show { get => _show; }
    public UnityEvent<Views> back { get => _back; }

    public void Update()
    {
        if (!needToShowView) return;
        if ((int)viewToShow < 0 || (int)viewToShow > 2) return;
        ShowView(viewToShow);
        needToShowView = false;
    }
    public void ShowView(Views view)
    {
        show?.Invoke(view);
        popUpViews[(int)view].Show();
    }
    private void warningView_back()
    {
        back.Invoke(Views.Warning);
    }
    private void purchaseView_back()
    {
        back.Invoke(Views.Purchase);
    }
    private void expiredView_back()
    {
        back.Invoke(Views.Expired);
    }
}