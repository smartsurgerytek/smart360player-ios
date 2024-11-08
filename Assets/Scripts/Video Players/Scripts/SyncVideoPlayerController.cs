﻿using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace SmartSurgery.VideoControllers
{
    public class SyncVideoPlayerController : MonoBehaviour
    {
        [SerializeField] private bool _initializeOnEnable; 

        [SerializeField] private VideoSource _videoSource;

        [Header("Components")]
        [SerializeField] private TimelineController _timeline;
        [SerializeField] private RenderTexture _targetTexture;

        [Header("Data")]
        [SerializeField] private int _initialSelected = 0;
        [SerializeField, TableList] private SyncVideoModel[] _syncVideos;
        [SerializeField, AssetsOnly] private ListElementButton _videoButtonPrefab;

        [Header("Events")]
        [SerializeField] private UnityEvent<string> _setTitle;
        [SerializeField] private UnityEvent<int, Transform> _layoutButton;

        [Header("Debug")]
        [NonSerialized, ReadOnly, ShowInInspector] private bool _initialized;
        [NonSerialized, ReadOnly, ShowInInspector] private bool _selectedInvalid;
        [NonSerialized, ReadOnly, ShowInInspector] private int _selected = -1;
        [NonSerialized, ReadOnly, ShowInInspector] private ListElementButton[] _videoButtons;
        [NonSerialized, ReadOnly, ShowInInspector] private VideoPlayer[] _players;

        [NonSerialized] private MultiRangeEventSystem _multiRangeEventSystem;


        public VideoSource videoSource
        {
            get => _videoSource;
            set
            {
                if (_initialized) throw new SetPropertyAfterInitializationException();
                _videoSource = value;
            }
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

        public SyncVideoModel[] syncVideos
        {
            get => _syncVideos;
            set
            {
                if (_initialized) throw new SetPropertyAfterInitializationException();
                _syncVideos = value;
            }
        }

        public TimelineController timeline
        {
            get => _timeline;
            set
            {
                if (_initialized) throw new SetPropertyAfterInitializationException();
                _timeline = value;
            }
        }

        [ShowInInspector] 
        private double time => timeline?.time ?? double.MinValue;
        
        public event Func<int, string> getTitleText;
        private string GetTitleText(int index)
        {
            return getTitleText?.Invoke(index);
        }
        public UnityEvent<string> setTitle { get => _setTitle; }
        public UnityEvent<int, Transform> layoutButton { get => _layoutButton; }

        public void Initialize()
        {
            if (_initialized) return;
            StartCoroutine(InitializeCoroutine());
        }
        private IEnumerator InitializeCoroutine()
        {
            InitializeVideoComponents();
            yield return WaitForPlayerPreparation();
            InitializeRangeSystem();
            SetupTimelineListeners();
            SetInitialState();
            
            _initialized = true;
        }
        private void InitializeVideoComponents()
        {
            var count = _syncVideos.Length;
            _videoButtons = new ListElementButton[count];
            _players = new VideoPlayer[count];

            for (int i = 0; i < count; i++)
            {
                InitializeVideoButton(i);
                InitializeVideoPlayer(i);
            }
        }

        private void InitializeVideoButton(int index)
        {
            _videoButtons[index] = Instantiate(_videoButtonPrefab);
            _videoButtons[index].interactable = false;
            _videoButtons[index].index = index;

            if (_syncVideos[index].icon)
            {
                _videoButtons[index].icon = _syncVideos[index].icon;
            }

            _videoButtons[index].title = _syncVideos[index].title;
            _videoButtons[index].onClick.AddListener(_videoButton_onClick);
            layoutButton?.Invoke(index, _videoButtons[index].transform);
        }

        private void InitializeVideoPlayer(int index)
        {
            _players[index] = _videoButtons[index].GetComponent<VideoPlayer>();
            _players[index].source = videoSource;

            if (videoSource == VideoSource.VideoClip)
            {
                _players[index].clip = _syncVideos[index].clip;
            }
            else
            {
                _players[index].url = _syncVideos[index].url;
            }

            _players[index].timeReference = VideoTimeReference.ExternalTime;

#if UNITY_IOS
            _players[index].prepareCompleted += _players_prepareCompleted;
            _players[index].seekCompleted += _players_seekCompleted;
#endif

            MutePlayer(index, true);
        }

        private IEnumerator WaitForPlayerPreparation()
        {
#if UNITY_IOS
            
            yield break;
#else
            PreparePlayers();
            while(_players.Any(player => !player.isPrepared))
            {
                yield return null;
            }
#endif
        }

        private void InitializeRangeSystem()
        {
            var rangeModels = _syncVideos.Select((video, index) =>
                new RangeModel(index, video.startTime, video.startTime + video.duration)).ToArray();

            _multiRangeEventSystem = new MultiRangeEventSystem();
            _multiRangeEventSystem.Initialize(rangeModels);
            _multiRangeEventSystem.getCurrentValue += _multiRangeEventSystem_getCurrentValue;
            _multiRangeEventSystem.enter += _multiRangeEventSystem_enter;
            _multiRangeEventSystem.exit += _multiRangeEventSystem_exit;
            _multiRangeEventSystem.update += _multiRangeEventSystem_update;
        }

        private void SetupTimelineListeners()
        {
            timeline.playing.AddListener(_timeline_onPlaying);
            timeline.play.AddListener(_timeline_onPlay);
            timeline.pause.AddListener(_timeline_onPause);
            timeline.dragStart.AddListener(_timeline_dragStart);
            timeline.dragging.AddListener(_timeline_onDragging);
            timeline.dragEnd.AddListener(_timeline_dragEnd);
            timeline.stop.AddListener(_timeline_onStop);
        }

        private void SetInitialState()
        {
            SetSelected(initialSelected, false);
        }

#if UNITY_IOS
        private void _players_seekCompleted(VideoPlayer source)
        {
            if (!source.isPlaying && _timeline.isPlaying)
            {
                source.Play();
            }
            if (!source.isPlaying && !_timeline.isPlaying)
            {
                source.Play();
                source.Pause();
                Debug.Log("_Player Seek Completed");
            }

        }
        private void _players_prepareCompleted(VideoPlayer source)
        {
            var index = Array.IndexOf(_players, source);
            if (_selected != index)
            {
                source.Stop();
                throw new Exception("You prepared a player that is not selected.");
            }
            SetPlayerExternalReferenceTime(index, time);
            SetPlayerTime(index, time);
            Debug.Log("_player prepare completed.");
        }
#endif

        private void OnValidate()
        {
            if (_syncVideos == null) return;
            var count = _syncVideos.Length;
            for (int i = 0; i < count; i++)
            {
                _syncVideos[i].index = i;
            }
        }
        private void OnEnable()
        {
            if (_initializeOnEnable) Initialize();
        }
        private void OnDisable()
        {
            if (!_initialized) return;
            _multiRangeEventSystem = null;
        }
        private void Update()
        {
            if (!_initialized) return;
            _multiRangeEventSystem?.Update();
            //for (int i = 0; i < _players.Length; i++)
            //{
            //    if(_players[i].isPlaying || _players[i]. && _selected != i)
            //    {
            //        Debug.LogError($"the players[{i}] is playing, but not selected.");
            //        _players[i].Stop();
            //    }
            //    if (_players[i].targetTexture == _targetTexture && _selected != i)
            //    {
            //        _players[i].targetTexture = _targetTexture;
            //    }
            //}
            //if (!_players[_selected].isPlaying && timeline.isPlaying)
            //{
            //    SetPlayerTime(_selected, time);
            //    SetPlayerExternalReferenceTime(_selected, time);
            //    _players[_selected].Play();
            //}
            //if (_players[_selected].targetTexture != _targetTexture)
            //{
            //    _players[_selected].targetTexture = _targetTexture;
            //}
        }

        private void _videoButton_onClick(int index)
        {
            SetSelected(index, _timeline.isPlaying);
        }
        private double _multiRangeEventSystem_getCurrentValue()
        {
            return timeline.time;
        }

        private void _multiRangeEventSystem_enter(int index)
        {
            _videoButtons[index].interactable = true;
        }

        private void _multiRangeEventSystem_exit(int index)
        {
            _videoButtons[index].interactable = false;
            if (_selected == index) _selectedInvalid = true;
        }

        private void _multiRangeEventSystem_update(int[] interects)
        {
            if (_selectedInvalid)
            {
                if (interects.Length > 0)
                {
                    SetSelected(interects.Last(), _timeline.isPlaying);
                    _selectedInvalid = false;
                }
                else
                {
                    timeline.Stop();
                }
            }
        }
        private void _timeline_onPlaying(float time)
        {
#if UNITY_IOS
            SetPlayerExternalReferenceTime(_selected, time);
#else
            SetPlayersExternalReferenceTime(time);
#endif
        }

        private void _timeline_dragStart(float time)
        {

#if UNITY_IOS
            _players[_selected].Pause();
#else
            SetPlayersExternalReferenceTime(time);
            SetPlayersTime(time);
            PlayPlayers();
            PausePlayers();
#endif
        }
        private void _timeline_onDragging(float time)
        {
            SetPlayerExternalReferenceTime(_selected, time);
            //_players[_selected].Play();
            //_players[_selected].Pause();
        }
        private void _timeline_dragEnd(float time)
        {
#if UNITY_IOS
            SetPlayerTime(_selected, time);
            if (timeline.isPlaying && !_players[_selected].isPlaying)
            {
                //_players[_selected].Stop();
                SetPlayerExternalReferenceTime(_selected, time);
                _players[_selected].Prepare();
            }
#else
            if (timeline.isPlaying){
                SetPlayersExternalReferenceTime(time);
                SetPlayersTime(time);
                PlayPlayers();
            }
#endif
        }
        private void _timeline_onPlay()
        {
#if UNITY_IOS
            SetPlayerExternalReferenceTime(_selected, time);
            SetPlayerTime(_selected, time);
            _players[_selected].Prepare();
#else
            PlayPlayers();
#endif
        }
        private void _timeline_onPause()
        {
#if UNITY_IOS
            StopPlayers();
#else
            PausePlayers();
#endif
        }
        private void _timeline_onStop()
        {
            SetPlayersTime(0);
            SetPlayersExternalReferenceTime(0);
            StopPlayers();
        }
#if UNITY_IOS
#else
        private void PlayPlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i]?.Play();
            }
        }
#endif
        private void PausePlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i]?.Pause();
            }
        }
        private void StopPlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                if (!_players[i]) continue;
                _players[i]?.Stop();
            }
        }
        private void PreparePlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                //_players[i]?.;
                _players[i]?.Prepare();
            }
        }
        private void SetPlayerTime(int playerId, double globalTime)
        {
            var clipTime = globalTime - _syncVideos[playerId].startTime;
            Mathd.Clamp(clipTime, 0, _syncVideos[playerId].duration);
            _players[playerId].time = clipTime;
        }
        private void SetPlayerExternalReferenceTime(int playerId, double globalTime)
        {
            var clipTime = globalTime - _syncVideos[playerId].startTime;
            Mathd.Clamp(clipTime, 0, _syncVideos[playerId].duration);
            _players[playerId].externalReferenceTime = clipTime;
        }
        private void SetPlayersTime(double globalTime)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var clipTime = globalTime - _syncVideos[i].startTime;
                Mathd.Clamp(clipTime, 0, _syncVideos[i].duration);
                _players[i].time = clipTime;
            }
        }
        private void SetPlayersExternalReferenceTime(double globalTime)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var clipTime = globalTime - _syncVideos[i].startTime;
                Mathd.Clamp(clipTime, 0, _syncVideos[i].duration);
                _players[i].externalReferenceTime = clipTime;
            }
        }
        private void MutePlayer(int index, bool mute)
        {
            for (ushort i = 0; i < _players[index].controlledAudioTrackCount; i++)
            {
                _players[index].SetDirectAudioMute(i, mute);
            }
        }
        private void SetSelected(int index, bool play)
        {
            Debug.Log($"SetSelected({index},{play}), _selected={_selected}");
            if (_selected == index) return;
            if (timeline.time < syncVideos[index].startTime || syncVideos[index].startTime + _syncVideos[index].duration < timeline.time)
            {
                _selectedInvalid = true;
                Debug.Log("Selected Invalid");
            }

            if (_selected >= 0)
            {
#if UNITY_IOS
                if (_players[_selected].isPlaying)
                {
                    _players[_selected].Stop(); // IOS
                }
#endif
                _players[_selected].targetTexture = null;
                MutePlayer(_selected, true);
            }

#if UNITY_IOS
            _players[index].targetTexture = _targetTexture;
            _players[index].Stop();
            SetPlayerExternalReferenceTime(index, _timeline.time);
            _players[index].Prepare();
            Debug.Log("_player Prepare");
#else
            _players[index].targetTexture = _targetTexture;
#endif


            MutePlayer(index, false);
            _selected = index;
            var title = GetTitleText(index);
            setTitle?.Invoke(title);
        }

        [Serializable]
        internal struct VideoPlayerInfo
        {
            [NonSerialized, ShowInInspector, ReadOnly] private int _id;
            [NonSerialized, ShowInInspector, ReadOnly] private double _time;
            [NonSerialized, ShowInInspector, ReadOnly] private double _externalReferenceTime;
            [NonSerialized, ShowInInspector, ReadOnly] private bool _isPrepared;
            [NonSerialized, ShowInInspector, ReadOnly] private bool _isPlaying;
            [NonSerialized, ShowInInspector, ReadOnly] private bool[] _mute;
            internal VideoPlayerInfo(int id, VideoPlayer player)
            {
                _id = id;
                _time = player.time;
                _externalReferenceTime = player.externalReferenceTime;
                _isPrepared = player.isPrepared;
                _isPlaying = player.isPlaying;
                _mute = new bool[player.audioTrackCount];
                for (ushort i = 0; i < _mute.Length; i++)
                {
                    _mute[i] = player.GetDirectAudioMute(i);
                }
            }
        }

        // Odin Inspection

        [ShowInInspector, TableList]
        private VideoPlayerInfo[] videoPlayersInfo
        {
            get
            {
                var count = _players?.Length ?? 0;
                var rt = new VideoPlayerInfo[count];
                for (int i = 0; i < count; i++)
                {
                    rt[i] = new VideoPlayerInfo(i,_players[i]);
                }
                return rt;
            }
        }
        private void OnDestroy()
        {
            StopPlayers();
        }
    }
}