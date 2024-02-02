using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Hand Menu")]
    [SerializeField] private CanvasGroupFade _handMenu;
    [SerializeField] private bool _activeHandMenu = false;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _activeHandMenuChanged = false;
    [Header("Buttons")]
    [SerializeField] private OVRInput.Button _handMenuButton = OVRInput.Button.Start;
    [SerializeField] private OVRInput.Button _showMonitorAndFollowButton = OVRInput.Button.Three;
    [SerializeField] private OVRInput.Button _closeMonitorButton = OVRInput.Button.Four;
    [Header("Monitor")]
    [SerializeField] private Follow _monitorFollow;
    [SerializeField] private CanvasGroupFade _monitorFade;
    
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
    private void OnEnable()
    {
        _activeHandMenuChanged = true;
    }
    private void Update()
    {
        if (OVRInput.GetDown(_handMenuButton)) activeHandMenu = !activeHandMenu;
        if (_activeHandMenuChanged)
        {
            _handMenu.SetVisible(_activeHandMenu);
            _activeHandMenuChanged = false;
        }


        if (OVRInput.GetDown(_showMonitorAndFollowButton))
        {
            _monitorFade.SetVisible(true);
            _monitorFollow.following = true;
        }
        if (OVRInput.GetUp(_showMonitorAndFollowButton))
        {
            _monitorFollow.following = false;
        }
        if (OVRInput.GetDown(_closeMonitorButton))
        {
            _monitorFade.SetVisible(false);
        }
    }
}
