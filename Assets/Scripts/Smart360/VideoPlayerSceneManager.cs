using UnityEngine;
using Sirenix.OdinInspector;
using System;
using SmartSurgery.VideoControllers;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Video;
public class VideoPlayerSceneManager : MonoBehaviour
{
    [SerializeField] private bool _initializeOnEnable;

    [Header("Data")]
    [SerializeField] private ApplicationManager _applicationManager;
    [SerializeField] private Video[] _videoModels;
    [SerializeField] private int _currentEdition = 0;

    [Header("Components")]
    [SerializeField] private Smart360PlayerController _smartPlayer;
    [SerializeField] private CanvasGroup _loadingVideoScreen;

    [Header("Views")]
    [SerializeField, SceneObjectsOnly] private ExactPositionLayout[] _layouts;
    [SerializeField] private TextMeshProUGUI _editionTitleText;

    [Header("Events")] private UnityEvent _quit = new UnityEvent();

    [NonSerialized, ShowInInspector, ReadOnly] private int[] _videoCountsInSocket;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;

    public UnityEvent quit { get => _quit; }

    public void Initialize(int edition)
    {
        if (_initialized) return;

        _currentEdition = edition;
        var staffs = _applicationManager.staffManager.data;
        var staffGroups = _applicationManager.staffGroupManager.data;
        var editions = _applicationManager.editionManager.data;
        var surroundings = _applicationManager.videoManager.surroundingData;
        _videoModels = _applicationManager.videoManager.GetVideoModelsByEdition(edition);

        _editionTitleText.text = editions[edition].displayName;

        _videoCountsInSocket = new int[staffGroups.Length];

        _smartPlayer.layoutButton.AddListener(_smartPlayer_layoutButton);
        _smartPlayer.initialSelected = editions[edition].initialSelected;
        switch (_applicationManager.videoManager.loadDataMethod)
        {
            case LoadDataMethod.BuildIn:
                _smartPlayer.videoSource = VideoSource.VideoClip;
                _smartPlayer.surroundingVideoClip = surroundings[edition].clip;
                break;
            case LoadDataMethod.DirectFile:
                _smartPlayer.videoSource = VideoSource.Url;
                _smartPlayer.surroundingVideoUrl = _applicationManager.fileManager.GetVideoPath(surroundings[edition].fileName);
                break;
            case LoadDataMethod.AssetBundle:
                _smartPlayer.videoSource = VideoSource.VideoClip;
                break;
            default:
                break;
        }
        _smartPlayer.flatPlayer.getTitleText += _flatPlayer_getTitleText;
        _smartPlayer.quit.AddListener(_smartPlayer_onQuit);
        var videoCount = _videoModels.Length;

        var syncVideoModels = new SyncVideoModel[videoCount];
        for (int i = 0; i < videoCount; i++)
        {
            var staff = staffs[_videoModels[i].staff];
            var group = staffGroups[staff.group];
            syncVideoModels[i] = new SyncVideoModel(
                i, 
                _videoModels[i].startTime, 
                staff.displayButtonName, 
                group.icon, 
                _videoModels[i].clip,
                _applicationManager.fileManager.GetVideoPath(_videoModels[i].fileName));
        }
        _smartPlayer.syncVideoModels = syncVideoModels;
        _smartPlayer.Initialize();
    }

    private string _flatPlayer_getTitleText(int index)
    {
        var staffs = _applicationManager.staffManager.data;
        return staffs[_videoModels[index].staff].displayTitleName+"視角";
    }
    private void _smartPlayer_layoutButton(int index, Transform transform)
    {
        var staffs = _applicationManager.staffManager.data;
        var staffGroups = _applicationManager.staffGroupManager.data;
        var staff = staffs[_videoModels[index].staff];
        _layouts[staff.group].Layout(_videoCountsInSocket[staff.group]++, transform);
    }
    private void _smartPlayer_onQuit()
    {
        quit?.Invoke();
    }
    private void OnEnable()
    {
        if (_initializeOnEnable) Initialize(_currentEdition);
    }
    private void OnDisable()
    {
    }
    private void Update()
    {
    }

    private void ShowCanvasGroup(CanvasGroup canvasGroup,bool show)
    {
        if (show)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
