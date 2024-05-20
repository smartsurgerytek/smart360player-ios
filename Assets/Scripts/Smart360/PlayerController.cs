using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Hand Menu")]
    [SerializeField] private CanvasGroupFade _handMenu;
    [SerializeField] private bool _activeHandMenu = false;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _activeHandMenuChanged = false;
    [Header("Buttons")]
    [SerializeField] private OVRInput.Button _handMenuButton = OVRInput.Button.Start;
    [SerializeField] private OVRInput.Button _showMonitorButton = OVRInput.Button.Three;
    [SerializeField] private OVRInput.Button _closeMonitorButton = OVRInput.Button.Four;
    [SerializeField] private EventTrigger _dragButton;
    [Header("Monitor")]
    [SerializeField] private Follow _monitorFollow;
    [SerializeField] private CanvasGroupFade _monitorFade;
    [Header("Follow")]
    [SerializeField] private float _maxFollowDistance = 5f;
    [SerializeField] private float _minFollowDistance = 1f;
    [SerializeField] private float _currentFollowDistance = 2f;
    [SerializeField] private float _followSpeed = 1f;
    [SerializeField] private Transform _fullScreenCanvasAnchor;
    [SerializeField] private Transform _controllerRay;
    [SerializeField] private float _followLerpSpeed = 0.04f;
    [Header("Scale")]
    [SerializeField] private float _maxScale = 2f;
    [SerializeField] private float _minScale = .5f;
    [SerializeField] private float _currentScale = 1f;
    [SerializeField] private Transform _scaleTarget;

    [NonSerialized] private EventTrigger.Entry _pointerDownEntry;
    [NonSerialized] private EventTrigger.Entry _pointerUpEntry;
    [ShowInInspector, NonSerialized, ReadOnly] private bool _isDragging = false;

    public bool activeHandMenu
    {
        get => _activeHandMenu;
        set
        {
            if (_activeHandMenu == value) return;
            _activeHandMenu = value;
            _activeHandMenuChanged = true;
        }

    }
    private void Awake()
    {
        _pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown,
        };
        _pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp,
        };
        _pointerDownEntry.callback.AddListener(_dragButton_OnPointerDown);
        _pointerUpEntry.callback.AddListener(_dragButton_OnPointerUp);
    }



    private void OnEnable()
    {
        _activeHandMenuChanged = true;
        _dragButton.triggers.Add(_pointerDownEntry);
        _dragButton.triggers.Add(_pointerUpEntry);
    }
    private void OnDisable()
    {
        _dragButton.triggers.Remove(_pointerDownEntry);
        _dragButton.triggers.Remove(_pointerUpEntry);
    }
    private void Update()
    {
        if (OVRInput.GetDown(_handMenuButton)) activeHandMenu = !activeHandMenu;
        if (_activeHandMenuChanged)
        {
            _handMenu.SetVisible(_activeHandMenu);
            _activeHandMenuChanged = false;
        }
        if (!_isDragging && OVRInput.GetDown(_showMonitorButton) && !_monitorFade.GetVisible())
        {
            _monitorFade.SetVisible(true);
        }
        else if (!_isDragging && OVRInput.GetDown(_closeMonitorButton) && _monitorFade.GetVisible())
        {
            _monitorFade.SetVisible(false);
        }
        if (_isDragging)
        {
            UpdateScreenPosition(true);
        }
    }
    private void UpdateScreenPosition(bool lerp)
    {
        var yInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
        if (yInput != 0)
        {
            _currentFollowDistance += yInput * _followSpeed * Time.deltaTime;
            _currentFollowDistance = Mathf.Clamp(_currentFollowDistance, _minFollowDistance, _maxFollowDistance);
        }
        var position = _controllerRay.position + _controllerRay.forward * _currentFollowDistance;
        if (lerp)
        {
            _fullScreenCanvasAnchor.position = Vector3.Lerp(_fullScreenCanvasAnchor.position, position,_followLerpSpeed);
        }
    }
    private void _dragButton_OnPointerDown(BaseEventData eventArg)
    {
        _isDragging = true;
        _monitorFollow.following = true;
    }
    private void _dragButton_OnPointerUp(BaseEventData eventArg)
    {
        _isDragging = false;
        _monitorFollow.following = false;
    }
}
