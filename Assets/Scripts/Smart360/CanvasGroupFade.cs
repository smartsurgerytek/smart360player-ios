using UnityEngine;

public class CanvasGroupFade : MonoBehaviour
{
    [SerializeField] private bool _visible;
    [SerializeField] private bool _visibleChanged;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _speed = 1;
    private void Update()
    {
        if (_visibleChanged)
        {
            _canvasGroup.interactable = _visible;
            _canvasGroup.blocksRaycasts= _visible;
            _visibleChanged = false;
        }

        if (_canvasGroup.alpha == 0 && !_visible) return;
        if (_canvasGroup.alpha == 1 && _visible) return;
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,_visible ? 1 : 0, _speed * Time.deltaTime);
    }
    public void SetVisible(bool visible)
    {
        _visible = visible;
        _visibleChanged = true;
    }
}
