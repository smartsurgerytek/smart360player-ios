using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Sirenix.OdinInspector;
using TMPro;
using System;
using System.Linq;
using SmartSurgery.VideoControllers;
using UnityEngine.Events;
using System.Collections;

public class Smart360PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TimelineController _timeline;
    [SerializeField] private SyncVideoPlayerController _flatPlayer;
    [SerializeField] private VideoPlayerController _surroundingPlayer;
    [SerializeField] private Button _quitButton;
    //[SerializeField] private Button _switchViewButton;
    //[SerializeField] private Button _togglePlayerMonitorButton;
    //[SerializeField] private Button _togglePlayerControlPanelButton;
    [SerializeField] private CanvasGroup _playerControlPanel;
    [SerializeField] private GameObject _previewMonitor;
    [SerializeField] private GameObject _fullScreenMonitor;
    

    [Header("Views")]
    [SerializeField] private TextMeshProUGUI _videoTitle;
    [SerializeField, SceneObjectsOnly] private Transform _videoButtonsParent;
    [SerializeField, ReadOnly] private Button[] _videoButtons;
    private int[] _videoCountsInSocket;

    [Header("Data")]
    [SerializeField] private VideoSource _videoSource;
    [SerializeField] private VideoClip _surroundingVideoClip;
    [SerializeField] private string _surroundingVideoUrl;
    [SerializeField] private SyncVideoModel[] _syncVideoModels;

    [Header("Settings")]
    [SerializeField] private int _initialSelected = 0;

    [Header("Events")]
    [SerializeField] private UnityEvent<int, Transform> _layoutButton;
    [SerializeField] private UnityEvent _quit;

    [Header("Flows")]
    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _surroundingPlayer_prepared = false;

    [Header("States")]
    [NonSerialized, ShowInInspector, ReadOnly] private bool _isPreview = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _isPreviewChanged = false;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _displayMonitor = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _displayMonitorChanged = false;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _displayControls = true;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _displayControlsChanged = false;
    private Coroutine _currentCoroutine;

    public UnityEvent<int, Transform> layoutButton { get => _layoutButton; }
    public SyncVideoPlayerController flatPlayer 
    { 
        get => _flatPlayer;
    }
    public int initialSelected
    {
        get => _initialSelected;
        set
        {
            if (_initialized) throw new SetPropertyAfterInitializationException();
            _initialSelected = value;
        }
    }

    public VideoSource videoSource
    {
        get => _videoSource;
        set
        {
            if (_initialized) throw new SetPropertyAfterInitializationException();
            _videoSource = value;
        }
    }
    public VideoClip surroundingVideoClip
    {
        get => _surroundingVideoClip;
        set
        {
            if (_initialized) throw new SetPropertyAfterInitializationException();
            _surroundingVideoClip = value;
        }
    }
    public string surroundingVideoUrl
    {
        get => _surroundingVideoUrl;
        set
        {
            if (_initialized) throw new SetPropertyAfterInitializationException();
            _surroundingVideoUrl = value;
        }
    }
    public SyncVideoModel[] syncVideoModels
    {
        get => _syncVideoModels;
        set
        {
            if (_initialized) throw new SetPropertyAfterInitializationException();
            _syncVideoModels = value;
        }
    }
    public bool isPreview 
    { 
        get => _isPreview; 
        set 
        {
            if (_isPreview == value) return;
            _isPreview = value;
            _isPreviewChanged = true;

        }
    }

    public bool displayMonitor
    {
        get => _displayMonitor;
        set
        {
            if (_displayMonitor == value) return;
            _displayMonitor = value;
            _displayMonitorChanged = true;
        }
    }
    public bool displayConrols
    {
        get => _displayControls;
        set
        {
            if (_displayControls == value) return;
            _displayControls = value;
            _displayControlsChanged = true;
        }
    }

    public UnityEvent quit { get => _quit; }

    public void Initialize()
    {
        if (_initialized) return;
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(InitializeCoroutine());
    }
    private IEnumerator InitializeCoroutine()
    {
        var videoCount = syncVideoModels.Length;
        _surroundingPlayer.player.source = videoSource;
        if (videoSource == VideoSource.VideoClip)
        {
            _surroundingPlayer.player.clip = surroundingVideoClip;
        }
        else
        {
            _surroundingPlayer.player.url = surroundingVideoUrl;
        }
        _surroundingPlayer.player.Prepare();
        while (!_surroundingPlayer.player.isPrepared)
        {
            yield return null;
        }
        _timeline.length = _surroundingPlayer.player.length;
        _surroundingPlayer.timeline = _timeline;


        flatPlayer.videoSource = videoSource;
        flatPlayer.syncVideos = syncVideoModels.ToArray();
        flatPlayer.timeline = _timeline;
        flatPlayer.initialSelected = initialSelected;
        flatPlayer.layoutButton.AddListener(_flatPlayer_layoutButton);
        flatPlayer.setTitle.AddListener(_flatPlayer_setTitle);


        _quitButton.onClick.AddListener(_quitButton_onClick);
        //_switchViewButton.onClick.AddListener(_switchView_onClick);
        //_togglePlayerControlPanelButton.onClick.AddListener(_togglePlayerControlPanel_onClick);
        //_togglePlayerMonitorButton.onClick.AddListener(_togglePlayerMonitor_onClick);

        flatPlayer.Initialize();
        _surroundingPlayer.Initialize();
        _timeline.Initialize();

        _isPreviewChanged = true;
        _displayMonitorChanged = true;
        _displayControlsChanged = true;
        _initialized = true;
    }

    private void _flatPlayer_setTitle(string title)
    {
        _videoTitle.text = title;
    }

    private void _flatPlayer_layoutButton(int index, Transform transform)
    {
        layoutButton?.Invoke(index, transform);
    }

    private void _togglePlayerControlPanel_onClick()
    {
        displayConrols = !displayConrols;
    }

    private void _togglePlayerMonitor_onClick()
    {
        displayMonitor = !displayMonitor;
    }

    private void _switchView_onClick()
    {
        isPreview = !isPreview;
    }

    private void Update()
    {
        if (!_initialized)
        {
            return;
        }
        if (_displayMonitorChanged || _isPreviewChanged)
        {

            _displayMonitorChanged = false;
            _isPreviewChanged = false;
        }
        if (_displayControlsChanged)
        {
            UpdatePlayerControlPanel(displayConrols);
            _displayControlsChanged = false;
        }
    }
    private void UpdatePlayerControlPanel(bool displayConrols)
    {
        _playerControlPanel.alpha = displayConrols ? 1 : 0;
        _playerControlPanel.interactable = displayConrols;
        _playerControlPanel.blocksRaycasts = displayConrols;
    }
    private void _quitButton_onClick()
    {
        quit?.Invoke();
    }
    private void OnDisable()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
    }
}
