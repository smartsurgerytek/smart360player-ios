using UnityEngine;

internal class MainMenuView
{
    [SerializeField] private CanvasGroup _canvasGroup;
    public void Show()
    {
        ShowCanvas(_canvasGroup, true);
    }
    public void Hide()
    {
        ShowCanvas(_canvasGroup, false);
    }
    private void ShowCanvas(CanvasGroup canvasGroup, bool show)
    {
        canvasGroup.alpha = show ? 1 : 0;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}