using UnityEngine;
using Sirenix.OdinInspector;
using System;
using SmartSurgery.VideoControllers;
using TMPro;
using UnityEngine.Video;
using Sirenix.Serialization;
public class VideoPlayerSceneManager : SerializedMonoBehaviour, IController
{
    [SerializeField] private bool _initializeOnEnable;

    [Header("Data")]
    //[SerializeField] private ApplicationManager _applicationManager;
    [OdinSerialize] private IReader<Staff[]> _staffs;
    [OdinSerialize] private IReader<StaffGroup[]> _groups;
    [OdinSerialize] private IReader<Edition[]> _editions;
    [OdinSerialize] private IReader<SurroundingVideo[]> _surroundings;
    [OdinSerialize] private IReader<Video[]> _videos;
    [OdinSerialize] private IReader<int> _loadDataMethod;
    [SerializeField] private FileManager _fileManager;
    
    //[SerializeField] private Video[] _videoModels;
    [SerializeField] private IAccessor<int> _currentEdition;


    [Header("Components")]
    [SerializeField] private Smart360PlayerController _smartPlayer;
    [SerializeField] private CanvasGroup _loadingVideoScreen;

    [Header("Views")]
    [SerializeField, SceneObjectsOnly] private ExactPositionLayout[] _layouts;
    [SerializeField] private TextMeshProUGUI _editionTitleText;
    [Header("Controllers")]
    [OdinSerialize] private IController _initializer;
    [OdinSerialize] private IController _quit;

    //[Header("Events")] private UnityEvent _quit = new UnityEvent();

    [NonSerialized, ShowInInspector, ReadOnly] private int[] _videoCountsInSocket;
    [NonSerialized, ShowInInspector, ReadOnly] private bool _initialized;

    //public UnityEvent quit { get => _quit; }

    public void Initialize()
    {
        _initializer.Execute();
        if (_initialized) return;

        var currentEdition = _currentEdition.Read();
        var staffs = _staffs.Read();
        var staffGroups = _groups.Read();
        var editions = _editions.Read();
        var surroundings = _surroundings.Read();
        var loadDataMethod = _loadDataMethod.Read();
        var videos = _videos.Read();

        _editionTitleText.text = editions[currentEdition].displayName;

        _videoCountsInSocket = new int[staffGroups.Length];

        _smartPlayer.layoutButton.AddListener(_smartPlayer_layoutButton);
        _smartPlayer.initialSelected = editions[currentEdition].initialSelected;
        switch ((LoadDataMethod)loadDataMethod)
        {
            case LoadDataMethod.BuildIn:
                _smartPlayer.videoSource = VideoSource.VideoClip;
                _smartPlayer.surroundingVideoClip = surroundings[currentEdition].clip;
                break;
            case LoadDataMethod.DirectFile:
                _smartPlayer.videoSource = VideoSource.Url;
                _smartPlayer.surroundingVideoUrl = _fileManager.GetVideoPath(surroundings[currentEdition].fileName);
                break;
            case LoadDataMethod.AssetBundle:
                _smartPlayer.videoSource = VideoSource.VideoClip;
                break;
            default:
                break;
        }
        _smartPlayer.flatPlayer.getTitleText += _flatPlayer_getTitleText;
        _smartPlayer.quit.AddListener(_smartPlayer_onQuit);
        var videoCount = videos.Length;

        var syncVideoModels = new SyncVideoModel[videoCount];
        for (int i = 0; i < videoCount; i++)
        {
            var staff = staffs[videos[i].staff];
            var group = staffGroups[staff.group];
            var icon = Resources.Load<Sprite>(group.icon);

            syncVideoModels[i] = new SyncVideoModel(
                i, 
                videos[i].startTime, 
                staff.displayButtonName, 
                icon, 
                videos[i].clip,
                _fileManager.GetVideoPath(videos[i].fileName));
        }
        _smartPlayer.syncVideoModels = syncVideoModels;
        _smartPlayer.Initialize();
        _initialized = true;
    }

    private string _flatPlayer_getTitleText(int index)
    {
        var staffs = _staffs.Read();
        var videos = _videos.Read();
        return staffs[videos[index].staff].displayTitleName+"視角";
    }
    private void _smartPlayer_layoutButton(int index, Transform transform)
    {
        var staffs = _staffs.Read();
        var videos = _videos.Read();
        var staff = staffs[videos[index].staff];
        _layouts[staff.group].Layout(_videoCountsInSocket[staff.group]++, transform);
    }
    private void _smartPlayer_onQuit()
    {
        _quit.Execute();
    }
    private void OnEnable()
    {
        if (_initializeOnEnable)
        {
            Initialize();
        }
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

    void IController.Execute()
    {
        Initialize();
    }
}
